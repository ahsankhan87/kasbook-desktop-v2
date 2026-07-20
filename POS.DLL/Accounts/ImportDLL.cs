using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using POS.Core;
using POS.Core.Accounts;

namespace POS.DLL.Accounts
{
    /// <summary>
    /// Data access layer for import operations
    /// Handles Excel reading, bulk inserts, and import session management
    /// </summary>
    public class ImportDLL
    {
        private readonly dbConnection _db = new dbConnection();

        #region Excel Reading (OleDb Pattern)

        /// <summary>
        /// Reads Excel file and returns DataTable (supports .xls and .xlsx)
        /// </summary>
        public DataTable ReadExcelFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Excel file not found", filePath);

            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            string connectionString;

            if (extension == ".xlsx")
            {
                connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
            }
            else if (extension == ".xls")
            {
                connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
            }
            else
            {
                throw new InvalidOperationException("Only Excel files (.xlsx, .xls) are supported.");
            }

            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schema == null || schema.Rows.Count == 0)
                    throw new InvalidOperationException("No worksheet found in Excel file.");

                var candidateSheets = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    var tableName = row["TABLE_NAME"].ToString();
                    if (!string.IsNullOrWhiteSpace(tableName) && tableName.EndsWith("$"))
                        candidateSheets.Add(tableName);
                }

                if (!candidateSheets.Any())
                    candidateSheets.Add(schema.Rows[0]["TABLE_NAME"].ToString());

                DataTable bestTable = null;
                int bestScore = -1;

                foreach (var sheetName in candidateSheets)
                {
                    using (var command = new OleDbCommand($"SELECT * FROM [{sheetName}]", connection))
                    using (var adapter = new OleDbDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        int score = GetWorksheetMatchScore(dataTable);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestTable = dataTable;
                        }
                    }
                }

                if (bestTable != null)
                    return bestTable;

                throw new InvalidOperationException("Could not read worksheet data from Excel file.");
            }
        }

        private int GetWorksheetMatchScore(DataTable table)
        {
            if (table == null || table.Columns.Count == 0)
                return 0;

            var normalizedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn column in table.Columns)
            {
                normalizedColumns.Add(NormalizeColumnKey(column.ColumnName));
            }

            int score = 0;
            if (normalizedColumns.Contains("accountcode")) score += 4;
            if (normalizedColumns.Contains("accountname")) score += 2;
            if (normalizedColumns.Contains("accounttype")) score += 2;
            if (normalizedColumns.Contains("debitamount")) score += 2;
            if (normalizedColumns.Contains("creditamount")) score += 2;
            if (normalizedColumns.Contains("partycode")) score += 2;
            if (normalizedColumns.Contains("partycodename")) score += 1;
            if (normalizedColumns.Contains("voucherno")) score += 2;
            if (normalizedColumns.Contains("voucherdate")) score += 2;

            if (table.Rows.Count > 0)
                score += 1;

            return score;
        }

        private string NormalizeColumnKey(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                return string.Empty;

            var chars = columnName
                .Trim()
                .ToLowerInvariant()
                .Where(char.IsLetterOrDigit)
                .ToArray();

            return new string(chars);
        }

        #endregion

        #region Import Session Management

        /// <summary>
        /// Creates a new import session and returns the session ID
        /// </summary>
        public int CreateImportSession(string importType, string fileName, int totalRows, int userId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ImportType", importType),
                new SqlParameter("@FileName", fileName),
                new SqlParameter("@TotalRows", totalRows),
                new SqlParameter("@ImportedBy", userId)
            };

            var query = @"
                INSERT INTO acc_import_sessions 
                    (import_type, file_name, total_rows, imported_by, status)
                VALUES 
                    (@ImportType, @FileName, @TotalRows, @ImportedBy, 'InProgress');
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0][0]) : 0;
        }

        /// <summary>
        /// Updates import session with final counts and status
        /// </summary>
        public void UpdateImportSession(int sessionId, int importedRows, int skippedRows, int errorRows, 
            string status, string errorLog, int rollbackHours = 24)
        {
            var rollbackUntil = status == "Completed" ? DateTime.Now.AddHours(rollbackHours) : (DateTime?)null;

            var parameters = new[]
            {
                new SqlParameter("@SessionId", sessionId),
                new SqlParameter("@ImportedRows", importedRows),
                new SqlParameter("@SkippedRows", skippedRows),
                new SqlParameter("@ErrorRows", errorRows),
                new SqlParameter("@Status", status),
                new SqlParameter("@ErrorLog", (object)errorLog ?? DBNull.Value),
                new SqlParameter("@RollbackUntil", (object)rollbackUntil ?? DBNull.Value)
            };

            var query = @"
                UPDATE acc_import_sessions 
                SET imported_rows = @ImportedRows,
                    skipped_rows = @SkippedRows,
                    error_rows = @ErrorRows,
                    status = @Status,
                    error_log = @ErrorLog,
                    rollback_available_until = @RollbackUntil
                WHERE session_id = @SessionId";

            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Links a voucher to an import session for rollback tracking
        /// </summary>
        public void LinkVoucherToSession(int sessionId, int voucherId)
        {
            var parameters = new[]
            {
                new SqlParameter("@SessionId", sessionId),
                new SqlParameter("@VoucherId", voucherId)
            };

            var query = "INSERT INTO acc_import_vouchers (session_id, voucher_id) VALUES (@SessionId, @VoucherId)";
            _db.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Gets import history using stored procedure
        /// </summary>
        public DataTable GetImportHistory(string importType = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = new[]
            {
                new SqlParameter("@ImportType", (object)importType ?? DBNull.Value),
                new SqlParameter("@FromDate", (object)fromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object)toDate ?? DBNull.Value)
            };

            return _db.ExecuteQuery("EXEC sp_GetImportHistory @ImportType, @FromDate, @ToDate", parameters);
        }

        /// <summary>
        /// Rolls back an import session using stored procedure
        /// </summary>
        public DataTable RollbackImportSession(int sessionId, int userId)
        {
            var parameters = new[]
            {
                new SqlParameter("@SessionId", sessionId),
                new SqlParameter("@UserId", userId)
            };

            return _db.ExecuteQuery("EXEC sp_RollbackImport @SessionId, @UserId", parameters);
        }

        #endregion

        #region Opening Balance Validation

        /// <summary>
        /// Validates opening balance data using stored procedure
        /// </summary>
        public OpeningBalanceValidationResult ValidateOpeningBalance(List<OpeningBalanceImportRow> rows)
        {
            var result = new OpeningBalanceValidationResult();

            // Create a DataTable for table-valued parameter
            var balanceTable = new DataTable();
            balanceTable.Columns.Add("acc_code", typeof(string));
            balanceTable.Columns.Add("dr_amount", typeof(decimal));
            balanceTable.Columns.Add("cr_amount", typeof(decimal));

            foreach (var row in rows.Where(r => r.IsValid))
            {
                balanceTable.Rows.Add(row.AccountCode, row.DebitAmount, row.CreditAmount);
            }

            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("sp_ValidateOpeningBalanceImport", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = new SqlParameter("@BalanceRows", SqlDbType.Structured);
                    parameter.TypeName = "dbo.ImportBalanceRow";
                    parameter.Value = balanceTable;
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        // First result set: summary
                        if (reader.Read())
                        {
                            result.IsValid = reader["validation_status"].ToString() == "PASSED";
                            result.TotalDebit = reader["total_dr"] != DBNull.Value ? Convert.ToDecimal(reader["total_dr"]) : 0;
                            result.TotalCredit = reader["total_cr"] != DBNull.Value ? Convert.ToDecimal(reader["total_cr"]) : 0;
                            result.Difference = reader["difference"] != DBNull.Value ? Convert.ToDecimal(reader["difference"]) : 0;
                        }

                        // Second result set: detailed errors (if any)
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result.Errors.Add(new ImportErrorModal
                                {
                                    ErrorType = reader["ErrorType"].ToString(),
                                    ErrorMessage = reader["ErrorMessage"].ToString(),
                                    RowData = reader["AccountCode"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region Bulk Insert Operations

        /// <summary>
        /// Bulk insert using SqlBulkCopy for performance
        /// </summary>
        public void BulkInsert(DataTable dataTable, string tableName, Dictionary<string, string> columnMappings = null)
        {
            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.BulkCopyTimeout = 600; // 10 minutes

                    if (columnMappings != null)
                    {
                        foreach (var mapping in columnMappings)
                        {
                            bulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                        }
                    }
                    else
                    {
                        // Auto-map by column names
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }

                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        /// <summary>
        /// Bulk insert with progress reporting (for large datasets)
        /// </summary>
        public void BulkInsertWithProgress(DataTable dataTable, string tableName, 
            Dictionary<string, string> columnMappings, 
            Action<int, int> progressCallback,
            int batchSize = 1000)
        {
            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = batchSize;
                    bulkCopy.BulkCopyTimeout = 600; // 10 minutes
                    bulkCopy.NotifyAfter = batchSize;

                    // Progress notification
                    int totalRows = dataTable.Rows.Count;
                    bulkCopy.SqlRowsCopied += (sender, e) =>
                    {
                        progressCallback?.Invoke((int)e.RowsCopied, totalRows);
                    };

                    if (columnMappings != null)
                    {
                        foreach (var mapping in columnMappings)
                        {
                            bulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                        }
                    }
                    else
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }

                    bulkCopy.WriteToServer(dataTable);
                    progressCallback?.Invoke(totalRows, totalRows); // Final callback
                }
            }
        }

        /// <summary>
        /// Bulk insert journal vouchers with batch processing
        /// </summary>
        public int BulkInsertJournalVouchers(List<JournalVoucherGroup> vouchers, int sessionId, int userId, 
            int branchId, Action<int, int> progressCallback = null)
        {
            if (vouchers == null || vouchers.Count == 0)
                return 0;

            int vouchersCreated = 0;
            int totalVouchers = vouchers.Count;

            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Process in batches
                        const int batchSize = 100;
                        for (int i = 0; i < vouchers.Count; i += batchSize)
                        {
                            var batch = vouchers.Skip(i).Take(batchSize).ToList();

                            foreach (var voucher in batch)
                            {
                                // Insert header
                                var headerId = InsertVoucherHeaderBatch(connection, transaction, voucher, userId, branchId);

                                // Insert entries
                                InsertVoucherEntriesBatch(connection, transaction, headerId, voucher.VoucherNo, voucher.Entries, userId, branchId);

                                // Link to import session
                                LinkVoucherToSessionBatch(connection, transaction, sessionId, headerId);

                                vouchersCreated++;

                                // Report progress
                                if (vouchersCreated % 10 == 0 || vouchersCreated == totalVouchers)
                                {
                                    progressCallback?.Invoke(vouchersCreated, totalVouchers);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return vouchersCreated;
        }

        private int InsertVoucherHeaderBatch(SqlConnection connection, SqlTransaction transaction, 
            JournalVoucherGroup voucher, int userId, int branchId)
        {
            var query = @"
                INSERT INTO acc_entries_header 
                    (InvoiceNo, EntryDate, VoucherType, Narration, total_debit, total_credit,
                     status, posted_by, posted_at, is_auto_posted, date_created, date_updated, user_id, branch_id)
                VALUES 
                    (@VoucherNo, @VoucherDate, 'JV', @Narration, @TotalDebit, @TotalCredit,
                     'Posted', @UserId, GETDATE(), 1, GETDATE(), GETDATE(), @UserId, @BranchId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@VoucherNo", voucher.VoucherNo);
                cmd.Parameters.AddWithValue("@VoucherDate", voucher.VoucherDate ?? DateTime.Today);
                cmd.Parameters.AddWithValue("@Narration", voucher.Entries.FirstOrDefault()?.Narration ?? "Imported entry");
                cmd.Parameters.AddWithValue("@TotalDebit", voucher.TotalDebit);
                cmd.Parameters.AddWithValue("@TotalCredit", voucher.TotalCredit);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BranchId", branchId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void InsertVoucherEntriesBatch(SqlConnection connection, SqlTransaction transaction,
            int headerId, string invoiceNo, List<JournalEntryImportRow> entries, int userId, int branchId)
        {
            foreach (var entry in entries)
            {
                var query = @"
                    INSERT INTO acc_entries 
                        (entry_id, invoice_no, account_id, account_name, entry_date, debit, credit, description, cost_center_id,
                         user_id, branch_id, date_created)
                    SELECT @EntryId, @InvoiceNo, id,name, @EntryDate, @Debit, @Credit, @Description, NULL,
                           @UserId, @BranchId, GETDATE()
                    FROM acc_accounts
                    WHERE code = @AccountCode";

                using (var cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@EntryId", headerId);
                    cmd.Parameters.AddWithValue("@InvoiceNo", (object)invoiceNo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccountCode", entry.AccountCode);
                    cmd.Parameters.AddWithValue("@EntryDate", entry.VoucherDate ?? DateTime.Today);
                    cmd.Parameters.AddWithValue("@Debit", entry.DebitAmount);
                    cmd.Parameters.AddWithValue("@Credit", entry.CreditAmount);
                    cmd.Parameters.AddWithValue("@Description", entry.Narration ?? "");
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@BranchId", branchId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LinkVoucherToSessionBatch(SqlConnection connection, SqlTransaction transaction,
            int sessionId, int voucherId)
        {
            var query = "INSERT INTO acc_import_vouchers (session_id, voucher_id) VALUES (@SessionId, @VoucherId)";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.Parameters.AddWithValue("@VoucherId", voucherId);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Account Operations

        /// <summary>
        /// Checks if account code exists
        /// </summary>
        public bool AccountExists(string accountCode)
        {
            var parameters = new[] { new SqlParameter("@Code", accountCode) };
            var query = "SELECT COUNT(*) FROM acc_accounts WHERE code = @Code";
            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        /// <summary>
        /// Gets all existing account codes for validation
        /// </summary>
        public HashSet<string> GetExistingAccountCodes()
        {
            var query = "SELECT code as account_code FROM acc_accounts WHERE is_active = 1";
            var result = _db.ExecuteQuery(query);
            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in result.Rows)
            {
                codes.Add(row["account_code"].ToString());
            }

            return codes;
        }

        /// <summary>
        /// Gets all existing group codes for validation
        /// </summary>
        public HashSet<string> GetExistingGroupCodes()
        {
            var query = "SELECT code as group_code FROM acc_groups WHERE code IS NOT NULL";
            var result = _db.ExecuteQuery(query);
            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in result.Rows)
            {
                var code = row["group_code"]?.ToString();
                if (!string.IsNullOrWhiteSpace(code))
                    codes.Add(code.Trim());
            }

            return codes;
        }

        /// <summary>
        /// Resolves group id from group code
        /// </summary>
        public int? GetGroupIdByCode(string groupCode)
        {
            if (string.IsNullOrWhiteSpace(groupCode))
                return null;

            var parameters = new[] { new SqlParameter("@Code", groupCode.Trim()) };
            var query = "SELECT TOP 1 id FROM acc_groups WHERE code = @Code";
            var result = _db.ExecuteQuery(query, parameters);

            if (result.Rows.Count == 0 || result.Rows[0][0] == DBNull.Value)
                return null;

            return Convert.ToInt32(result.Rows[0][0]);
        }

        /// <summary>
        /// Inserts a new group (acc_groups)
        /// </summary>
        public int InsertGroup(ChartOfAccountsImportRow group, int userId)
        {
            int parentId = 0;
            if (!string.IsNullOrWhiteSpace(group.ParentCode))
            {
                var parentGroupId = GetGroupIdByCode(group.ParentCode);
                if (!parentGroupId.HasValue)
                    throw new InvalidOperationException($"Parent group code not found: {group.ParentCode}");
                parentId = parentGroupId.Value;
            }

            var accountTypeId = ResolveAccountTypeId(group.AccountType);
            if (accountTypeId <= 0)
                throw new InvalidOperationException($"Account type not found: {group.AccountType}");

            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("sp_GroupsCrud", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@parent_id", parentId);
                    command.Parameters.AddWithValue("@account_type_id", accountTypeId);
                    command.Parameters.AddWithValue("@name_2", string.Empty);
                    command.Parameters.AddWithValue("@name", group.AccountName ?? string.Empty);
                    command.Parameters.AddWithValue("@code", group.AccountCode ?? string.Empty);
                    command.Parameters.AddWithValue("@description", (object)group.AccountType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@user_id", userId);
                    command.Parameters.AddWithValue("@date_created", DateTime.Now);
                    command.Parameters.AddWithValue("@OperationType", "1");

                    var scalar = command.ExecuteScalar();
                    return scalar == null || scalar == DBNull.Value ? 0 : Convert.ToInt32(scalar);
                }
            }
        }

        private int ResolveAccountTypeId(string accountType)
        {
            if (string.IsNullOrWhiteSpace(accountType))
                return 0;

            var type = accountType.Trim().ToLowerInvariant();
            if (type == "liability") type = "liability";
            if (type == "revenue") type = "income";
            if (type == "assets") type = "asset";
            if (type == "expenses") type = "expense";

            var parameters = new[] { new SqlParameter("@Search", "%" + type + "%") };
            var query = "SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE @Search OR LOWER(code) LIKE @Search ORDER BY id";
            var result = _db.ExecuteQuery(query, parameters);

            if (result.Rows.Count == 0 || result.Rows[0][0] == DBNull.Value)
                return 0;

            return Convert.ToInt32(result.Rows[0][0]);
        }

        /// <summary>
        /// Inserts a new account
        /// </summary>
        public int InsertAccount(ChartOfAccountsImportRow account, int userId)
        {
            var groupId = GetGroupIdByCode(account.ParentCode);
            if (!groupId.HasValue)
                throw new InvalidOperationException($"Group code not found: {account.ParentCode}");

            var normalBalance = (account.NormalBalance ?? string.Empty).Trim();
            var isCredit = normalBalance.Equals("CR", StringComparison.OrdinalIgnoreCase) ||
                           normalBalance.Equals("CREDIT", StringComparison.OrdinalIgnoreCase);
            var openingBalance = account.OpeningBalance ?? 0m;
            var opDrBalance = isCredit ? 0m : openingBalance;
            var opCrBalance = isCredit ? openingBalance : 0m;

            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("sp_AccountsCrud", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    command.Parameters.AddWithValue("@name", account.AccountName ?? string.Empty);
                    command.Parameters.AddWithValue("@name_2", string.Empty);
                    command.Parameters.AddWithValue("@code", account.AccountCode ?? string.Empty);
                    command.Parameters.AddWithValue("@group_id", groupId.Value);
                    command.Parameters.AddWithValue("@op_dr_balance", opDrBalance);
                    command.Parameters.AddWithValue("@op_cr_balance", opCrBalance);
                    command.Parameters.AddWithValue("@description", (object)account.AccountType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@user_id", userId);
                    command.Parameters.AddWithValue("@date_created", DateTime.Now);
                    command.Parameters.AddWithValue("@OperationType", "1");

                    var scalar = command.ExecuteScalar();
                    return scalar == null || scalar == DBNull.Value ? 0 : Convert.ToInt32(scalar);
                }
            }
        }

        #endregion

        #region Party (Customer/Supplier) Operations

        /// <summary>
        /// Checks if customer exists
        /// </summary>
        public bool CustomerExists(string customerCode)
        {
            var parameters = new[] { new SqlParameter("@Code", customerCode) };
            var query = "SELECT COUNT(*) FROM pos_customers WHERE code = @Code AND deleted = 0";
            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        /// <summary>
        /// Checks if supplier exists
        /// </summary>
        public bool SupplierExists(string supplierCode)
        {
            var parameters = new[] { new SqlParameter("@Code", supplierCode) };
            var query = "SELECT COUNT(*) FROM pos_suppliers WHERE code = @Code AND deleted = 0";
            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        #endregion

        #region Voucher Operations

        /// <summary>
        /// Creates a journal voucher header
        /// </summary>
        public int CreateVoucherHeader(string voucherNo, DateTime voucherDate, string voucherType, 
            decimal totalDebit, decimal totalCredit, string narration, int userId, int branchId = 1)
        {
            var parameters = new[]
            {
                new SqlParameter("@VoucherNo", voucherNo),
                new SqlParameter("@VoucherDate", voucherDate),
                new SqlParameter("@VoucherType", voucherType),
                new SqlParameter("@TotalDebit", totalDebit),
                new SqlParameter("@TotalCredit", totalCredit),
                new SqlParameter("@Narration", (object)narration ?? DBNull.Value),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@BranchId", branchId)
            };

            var query = @"
                INSERT INTO acc_entries_header 
                    (InvoiceNo, EntryDate, VoucherType, total_debit, total_credit, Narration, 
                     branch_id, user_id, date_created, date_updated)
                VALUES 
                    (@VoucherNo, @VoucherDate, @VoucherType, @TotalDebit, @TotalCredit, @Narration,
                     @BranchId, @UserId, GETDATE(), GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0][0]) : 0;
        }

        /// <summary>
        /// Creates journal voucher entries (lines)
        /// </summary>
        public void CreateVoucherEntries(int headerId, List<JournalEntryImportRow> entries)
        {
            if (entries == null || entries.Count == 0)
                return;

            var invoiceNo = GetHeaderInvoiceNo(headerId);
            if (string.IsNullOrWhiteSpace(invoiceNo))
                throw new InvalidOperationException($"Voucher header not found for id: {headerId}");

            foreach (var entry in entries)
            {
                var parameters = new[]
                {
                    new SqlParameter("@EntryId", headerId),
                    new SqlParameter("@InvoiceNo", invoiceNo),
                    new SqlParameter("@AccountCode", (object)entry.AccountCode ?? DBNull.Value),
                    new SqlParameter("@EntryDate", (object)(entry.VoucherDate ?? DateTime.Today)),
                    new SqlParameter("@Debit", entry.DebitAmount),
                    new SqlParameter("@Credit", entry.CreditAmount),
                    new SqlParameter("@Description", (object)(entry.Narration ?? string.Empty)),
                    new SqlParameter("@UserId", UsersModal.logged_in_userid),
                    new SqlParameter("@BranchId", UsersModal.logged_in_branch_id)
                };

                var query = @"
                    INSERT INTO acc_entries
                        (entry_id, invoice_no, account_id,account_name, entry_date, debit, credit, description, cost_center_id, user_id, branch_id, date_created)
                    SELECT @EntryId, @InvoiceNo, id, name, @EntryDate, @Debit, @Credit, @Description, NULL, @UserId, @BranchId, GETDATE()
                    FROM acc_accounts
                    WHERE code = @AccountCode";

                var affectedRows = _db.ExecuteNonQuery(query, parameters);
                if (affectedRows <= 0)
                    throw new InvalidOperationException($"Account code not found for entry: {entry.AccountCode}");
            }
        }

        private string GetHeaderInvoiceNo(int headerId)
        {
            var parameters = new[] { new SqlParameter("@HeaderId", headerId) };
            var result = _db.ExecuteQuery("SELECT TOP 1 InvoiceNo FROM acc_entries_header WHERE id = @HeaderId", parameters);
            if (result.Rows.Count == 0 || result.Rows[0][0] == DBNull.Value)
                return null;

            return Convert.ToString(result.Rows[0][0]);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the next available voucher number with prefix
        /// </summary>
        public string GetNextVoucherNumber(string prefix = "IMP")
        {
            var query = $@"
                SELECT TOP 1 InvoiceNo 
                FROM acc_entries_header 
                WHERE InvoiceNo LIKE '{prefix}-%'
                ORDER BY id DESC";

            var result = _db.ExecuteQuery(query);

            if (result.Rows.Count > 0)
            {
                var lastNo = result.Rows[0][0].ToString();
                var parts = lastNo.Split('-');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int number))
                {
                    return $"{prefix}-{(number + 1):D6}";
                }
            }

            return $"{prefix}-{1:D6}";
        }

        #endregion
    }
}
