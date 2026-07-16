using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

                // Get first sheet name
                var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schema == null || schema.Rows.Count == 0)
                    throw new InvalidOperationException("No worksheet found in Excel file.");

                string sheetName = string.Empty;
                foreach (DataRow row in schema.Rows)
                {
                    var tableName = row["TABLE_NAME"].ToString();
                    if (!string.IsNullOrWhiteSpace(tableName) && tableName.EndsWith("$"))
                    {
                        sheetName = tableName;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(sheetName))
                    sheetName = schema.Rows[0]["TABLE_NAME"].ToString();

                using (var command = new OleDbCommand($"SELECT * FROM [{sheetName}]", connection))
                using (var adapter = new OleDbDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
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
                                InsertVoucherEntriesBatch(connection, transaction, headerId, voucher.Entries, userId, branchId);

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
            int headerId, List<JournalEntryImportRow> entries, int userId, int branchId)
        {
            foreach (var entry in entries)
            {
                var query = @"
                    INSERT INTO acc_entries 
                        (invoice_no, account_id, entry_date, debit, credit, description, 
                         user_id, branch_id, date_created)
                    SELECT @InvoiceNo, id, @EntryDate, @Debit, @Credit, @Description,
                           @UserId, @BranchId, GETDATE()
                    FROM acc_accounts
                    WHERE code = @AccountCode";

                using (var cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@InvoiceNo", headerId.ToString());
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
            var query = "SELECT COUNT(*) FROM acc_accounts WHERE account_code = @Code";
            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        /// <summary>
        /// Gets all existing account codes for validation
        /// </summary>
        public HashSet<string> GetExistingAccountCodes()
        {
            var query = "SELECT account_code FROM acc_accounts WHERE is_active = 1";
            var result = _db.ExecuteQuery(query);
            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in result.Rows)
            {
                codes.Add(row["account_code"].ToString());
            }

            return codes;
        }

        /// <summary>
        /// Inserts a new account
        /// </summary>
        public int InsertAccount(ChartOfAccountsImportRow account, int userId)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountCode", account.AccountCode),
                new SqlParameter("@AccountName", account.AccountName),
                new SqlParameter("@ParentCode", (object)account.ParentCode ?? DBNull.Value),
                new SqlParameter("@AccountType", account.AccountType),
                new SqlParameter("@NormalBalance", account.NormalBalance),
                new SqlParameter("@IsBankAccount", account.IsBankAccount),
                new SqlParameter("@BankName", (object)account.BankName ?? DBNull.Value),
                new SqlParameter("@BankAccountNo", (object)account.BankAccountNo ?? DBNull.Value),
                new SqlParameter("@CreatedBy", userId)
            };

            var query = @"
                INSERT INTO acc_accounts 
                    (account_code, account_name, parent_code, account_type, normal_balance, 
                     is_bank_account, bank_name, bank_account_no, is_active, created_by, created_at)
                VALUES 
                    (@AccountCode, @AccountName, @ParentCode, @AccountType, @NormalBalance,
                     @IsBankAccount, @BankName, @BankAccountNo, 1, @CreatedBy, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var result = _db.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0][0]) : 0;
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
                    (InvoiceNo, EntryDate, VoucherType, TotalDebit, TotalCredit, Narration, 
                     branch_id, user_id, created_at, updated_at)
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
            var dataTable = new DataTable();
            dataTable.Columns.Add("entry_id", typeof(int));
            dataTable.Columns.Add("account_code", typeof(string));
            dataTable.Columns.Add("debit", typeof(decimal));
            dataTable.Columns.Add("credit", typeof(decimal));
            dataTable.Columns.Add("description", typeof(string));

            foreach (var entry in entries)
            {
                dataTable.Rows.Add(
                    headerId,
                    entry.AccountCode,
                    entry.DebitAmount,
                    entry.CreditAmount,
                    entry.Narration ?? string.Empty
                );
            }

            BulkInsert(dataTable, "acc_entries");
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
