using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using POS.Core;
using POS.Core.Accounts;
using POS.Core.POS;
using POS.DLL.Accounts;

namespace POS.BLL.Accounts
{
    /// <summary>
    /// Business logic layer for import operations
    /// Handles validation, orchestration, and import session management
    /// </summary>
    public class ImportBLL
    {
        private readonly ImportDLL _dll = new ImportDLL();

        #region Chart of Accounts Import

        /// <summary>
        /// Parses Chart of Accounts from Excel DataTable
        /// </summary>
        public List<ChartOfAccountsImportRow> ParseChartOfAccounts(DataTable data)
        {
            var rows = new List<ChartOfAccountsImportRow>();
            int rowNumber = 1; // Excel header is row 1, data starts at row 2

            foreach (DataRow row in data.Rows)
            {
                rowNumber++;

                // Skip empty rows
                if (IsEmptyRow(row))
                    continue;

                var importRow = new ChartOfAccountsImportRow
                {
                    RowNumber = rowNumber,
                    AccountCode = GetStringValue(row, "Account Code"),
                    AccountName = GetStringValue(row, "Account Name"),
                    ParentCode = GetStringValue(row, "Parent Code"),
                    AccountType = GetStringValue(row, "Account Type"),
                    NormalBalance = GetStringValue(row, "Normal Balance"),
                    OpeningBalance = GetDecimalValue(row, "Opening Balance"),
                    OpeningBalanceDate = GetDateValue(row, "Opening Balance Date"),
                    IsBankAccount = GetBooleanValue(row, "Is Bank Account"),
                    BankName = GetStringValue(row, "Bank Name"),
                    BankAccountNo = GetStringValue(row, "Bank Account No")
                };

                rows.Add(importRow);
            }

            return rows;
        }

        /// <summary>
        /// Validates Chart of Accounts rows
        /// </summary>
        public void ValidateChartOfAccounts(List<ChartOfAccountsImportRow> rows)
        {
            var existingCodes = _dll.GetExistingAccountCodes();
            var validAccountTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
                { "Asset", "Liability", "Equity", "Revenue", "Expense", "Income" };
            var validNormalBalances = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
                { "Dr", "Debit", "Cr", "Credit" };

            var importCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var errors = new List<string>();

                // Required fields
                if (string.IsNullOrWhiteSpace(row.AccountCode))
                    errors.Add("Account Code is required");

                if (string.IsNullOrWhiteSpace(row.AccountName))
                    errors.Add("Account Name is required");

                if (string.IsNullOrWhiteSpace(row.AccountType))
                    errors.Add("Account Type is required");
                else if (!validAccountTypes.Contains(row.AccountType))
                    errors.Add($"Invalid Account Type. Must be: {string.Join(", ", validAccountTypes)}");

                // Normal Balance validation
                if (!string.IsNullOrWhiteSpace(row.NormalBalance) && !validNormalBalances.Contains(row.NormalBalance))
                    errors.Add("Normal Balance must be Dr/Debit or Cr/Credit");

                // Duplicate check within import
                if (!string.IsNullOrWhiteSpace(row.AccountCode))
                {
                    if (existingCodes.Contains(row.AccountCode))
                        errors.Add("Account Code already exists in system");
                    else if (importCodes.Contains(row.AccountCode))
                        errors.Add("Duplicate Account Code in import file");
                    else
                        importCodes.Add(row.AccountCode);
                }

                // Parent code validation
                if (!string.IsNullOrWhiteSpace(row.ParentCode))
                {
                    if (!existingCodes.Contains(row.ParentCode) && !importCodes.Contains(row.ParentCode))
                        errors.Add("Parent Code not found (must exist in system or earlier in this import)");
                }

                // Bank account validation
                if (row.IsBankAccount && string.IsNullOrWhiteSpace(row.BankName))
                    errors.Add("Bank Name required when Is Bank Account = Yes");

                row.IsValid = errors.Count == 0;
                row.ValidationError = errors.Count > 0 ? string.Join("; ", errors) : null;
            }
        }

        /// <summary>
        /// Imports Chart of Accounts with session tracking
        /// </summary>
        public ImportResultModal ImportChartOfAccounts(List<ChartOfAccountsImportRow> rows, string fileName, 
            ImportConfigModal config)
        {
            var result = new ImportResultModal
            {
                TotalRows = rows.Count
            };

            int userId = UsersModal.logged_in_userid;
            int sessionId = 0;

            try
            {
                // Create import session
                sessionId = _dll.CreateImportSession("COA", fileName, rows.Count, userId);
                result.SessionId = sessionId;

                if (config.DryRunMode)
                {
                    result.Success = true;
                    result.Message = "Dry run completed. No data was imported.";
                    result.ImportedRows = rows.Count(r => r.IsValid);
                    result.SkippedRows = rows.Count(r => !r.IsValid);
                    return result;
                }

                // Import valid rows
                foreach (var row in rows.Where(r => r.IsValid))
                {
                    try
                    {
                        _dll.InsertAccount(row, userId);
                        result.ImportedRows++;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorRows++;
                        result.Errors.Add(new ImportErrorModal
                        {
                            RowNumber = row.RowNumber,
                            ErrorType = "INSERT_ERROR",
                            ErrorMessage = ex.Message,
                            RowData = $"{row.AccountCode} - {row.AccountName}"
                        });
                    }
                }

                result.SkippedRows = rows.Count - result.ImportedRows - result.ErrorRows;
                result.Success = result.ErrorRows == 0;
                result.Message = $"Import completed: {result.ImportedRows} imported, {result.SkippedRows} skipped, {result.ErrorRows} errors";

                // Update session
                var errorLog = result.Errors.Any() ? BuildErrorLog(result.Errors) : null;
                _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, result.ErrorRows, 
                    result.Success ? "Completed" : "PartialSuccess", errorLog, config.RollbackHours);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Import failed: " + ex.Message;

                if (sessionId > 0)
                {
                    _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, result.ErrorRows, 
                        "Failed", ex.Message, 0);
                }
            }

            return result;
        }

        #endregion

        #region Opening Balance Import

        /// <summary>
        /// Parses Opening Balance from Excel DataTable
        /// </summary>
        public List<OpeningBalanceImportRow> ParseOpeningBalance(DataTable data)
        {
            var rows = new List<OpeningBalanceImportRow>();
            int rowNumber = 1;

            foreach (DataRow row in data.Rows)
            {
                rowNumber++;

                if (IsEmptyRow(row))
                    continue;

                var importRow = new OpeningBalanceImportRow
                {
                    RowNumber = rowNumber,
                    AccountCode = GetStringValue(row, "Account Code"),
                    AccountName = GetStringValue(row, "Account Name"),
                    DebitAmount = GetDecimalValue(row, "Debit Amount") ?? 0,
                    CreditAmount = GetDecimalValue(row, "Credit Amount") ?? 0,
                    Remarks = GetStringValue(row, "Remarks")
                };

                rows.Add(importRow);
            }

            return rows;
        }

        /// <summary>
        /// Validates Opening Balance rows
        /// </summary>
        public OpeningBalanceValidationResult ValidateOpeningBalance(List<OpeningBalanceImportRow> rows)
        {
            var existingCodes = _dll.GetExistingAccountCodes();

            // Row-level validation
            foreach (var row in rows)
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(row.AccountCode))
                    errors.Add("Account Code is required");
                else if (!existingCodes.Contains(row.AccountCode))
                    errors.Add("Account Code does not exist in Chart of Accounts");

                if (row.DebitAmount < 0 || row.CreditAmount < 0)
                    errors.Add("Amounts cannot be negative");

                if (row.DebitAmount > 0 && row.CreditAmount > 0)
                    errors.Add("Account can have either Debit OR Credit, not both");

                if (row.DebitAmount == 0 && row.CreditAmount == 0)
                    errors.Add("Either Debit or Credit amount must be greater than 0");

                row.IsValid = errors.Count == 0;
                row.ValidationError = errors.Count > 0 ? string.Join("; ", errors) : null;
            }

            // Use stored procedure for balance validation
            return _dll.ValidateOpeningBalance(rows);
        }

        /// <summary>
        /// Posts Opening Balance as a journal voucher
        /// </summary>
        public ImportResultModal PostOpeningBalance(List<OpeningBalanceImportRow> rows, DateTime voucherDate, 
            string fileName, ImportConfigModal config)
        {
            var result = new ImportResultModal
            {
                TotalRows = rows.Count
            };

            int userId = UsersModal.logged_in_userid;
            int sessionId = 0;

            try
            {
                // Validate first
                var validation = ValidateOpeningBalance(rows);
                if (!validation.IsValid)
                {
                    result.Success = false;
                    result.Message = $"Validation failed: {string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))}";
                    result.Errors = validation.Errors;
                    return result;
                }

                // Create import session
                sessionId = _dll.CreateImportSession("OPENING_BALANCE", fileName, rows.Count, userId);
                result.SessionId = sessionId;

                if (config.DryRunMode)
                {
                    result.Success = true;
                    result.Message = $"Dry run completed. Would post opening balance voucher with Dr={validation.TotalDebit:N2}, Cr={validation.TotalCredit:N2}";
                    result.ImportedRows = rows.Count(r => r.IsValid);
                    return result;
                }

                // Create journal voucher
                var voucherNo = _dll.GetNextVoucherNumber("OB");
                var voucherId = _dll.CreateVoucherHeader(voucherNo, voucherDate, "Opening Balance", 
                    validation.TotalDebit, validation.TotalCredit, "Opening Balance Import", userId);

                if (voucherId == 0)
                    throw new Exception("Failed to create voucher header");

                // Create entries using batch insert
                var entries = rows.Where(r => r.IsValid).Select(r => new JournalEntryImportRow
                {
                    AccountCode = r.AccountCode,
                    DebitAmount = r.DebitAmount,
                    CreditAmount = r.CreditAmount,
                    Narration = r.Remarks ?? "Opening Balance"
                }).ToList();

                _dll.CreateVoucherEntries(voucherId, entries);

                // Link voucher to session for rollback
                _dll.LinkVoucherToSession(sessionId, voucherId);

                result.ImportedRows = rows.Count(r => r.IsValid);
                result.SkippedRows = rows.Count - result.ImportedRows;
                result.Success = true;
                result.Message = $"Opening Balance posted successfully. Voucher No: {voucherNo}";

                // Update session
                _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, 0, 
                    "Completed", null, config.RollbackHours);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Posting failed: " + ex.Message;

                if (sessionId > 0)
                {
                    _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, result.ErrorRows, 
                        "Failed", ex.Message, 0);
                }
            }

            return result;
        }

        #endregion

        #region Party Balance Import

        /// <summary>
        /// Parses Party (Customer/Supplier) Balance from Excel
        /// </summary>
        public List<PartyBalanceImportRow> ParsePartyBalance(DataTable data)
        {
            var rows = new List<PartyBalanceImportRow>();
            int rowNumber = 1;

            foreach (DataRow row in data.Rows)
            {
                rowNumber++;

                if (IsEmptyRow(row))
                    continue;

                var importRow = new PartyBalanceImportRow
                {
                    RowNumber = rowNumber,
                    PartyCode = GetStringValue(row, "Party Code") ?? GetStringValue(row, "Customer Code") ?? GetStringValue(row, "Supplier Code"),
                    PartyName = GetStringValue(row, "Party Name") ?? GetStringValue(row, "Customer Name") ?? GetStringValue(row, "Supplier Name"),
                    InvoiceNo = GetStringValue(row, "Invoice No"),
                    InvoiceDate = GetDateValue(row, "Invoice Date"),
                    DueDate = GetDateValue(row, "Due Date"),
                    Amount = GetDecimalValue(row, "Amount") ?? 0,
                    OutstandingAmount = GetDecimalValue(row, "Outstanding Amount") ?? 0,
                    Remarks = GetStringValue(row, "Remarks")
                };

                rows.Add(importRow);
            }

            return rows;
        }

        /// <summary>
        /// Validates Party Balance rows
        /// </summary>
        public void ValidatePartyBalance(List<PartyBalanceImportRow> rows, bool isCustomer)
        {
            foreach (var row in rows)
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(row.PartyCode))
                    errors.Add($"{(isCustomer ? "Customer" : "Supplier")} Code is required");
                else
                {
                    bool exists = isCustomer ? _dll.CustomerExists(row.PartyCode) : _dll.SupplierExists(row.PartyCode);
                    if (!exists)
                        errors.Add($"{(isCustomer ? "Customer" : "Supplier")} Code does not exist");
                }

                if (string.IsNullOrWhiteSpace(row.InvoiceNo))
                    errors.Add("Invoice No is required");

                if (!row.InvoiceDate.HasValue)
                    errors.Add("Invoice Date is required");

                if (row.Amount <= 0)
                    errors.Add("Amount must be greater than 0");

                if (row.OutstandingAmount < 0 || row.OutstandingAmount > row.Amount)
                    errors.Add("Outstanding Amount must be between 0 and Invoice Amount");

                row.IsValid = errors.Count == 0;
                row.ValidationError = errors.Count > 0 ? string.Join("; ", errors) : null;
            }
        }

        #endregion

        #region Historical Journal Import

        /// <summary>
        /// Parses Historical Journal Entries from Excel
        /// </summary>
        public List<JournalEntryImportRow> ParseJournalEntries(DataTable data)
        {
            var rows = new List<JournalEntryImportRow>();
            int rowNumber = 1;

            foreach (DataRow row in data.Rows)
            {
                rowNumber++;

                if (IsEmptyRow(row))
                    continue;

                var importRow = new JournalEntryImportRow
                {
                    RowNumber = rowNumber,
                    VoucherDate = GetDateValue(row, "Voucher Date"),
                    VoucherNo = GetStringValue(row, "Voucher No"),
                    AccountCode = GetStringValue(row, "Account Code"),
                    DebitAmount = GetDecimalValue(row, "Debit Amount") ?? 0,
                    CreditAmount = GetDecimalValue(row, "Credit Amount") ?? 0,
                    Narration = GetStringValue(row, "Narration"),
                    ReferenceNo = GetStringValue(row, "Reference No")
                };

                rows.Add(importRow);
            }

            return rows;
        }

        /// <summary>
        /// Validates and groups journal entries by voucher number
        /// </summary>
        public List<JournalVoucherGroup> ValidateAndGroupJournals(List<JournalEntryImportRow> rows)
        {
            var existingCodes = _dll.GetExistingAccountCodes();

            // Row-level validation
            foreach (var row in rows)
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(row.VoucherNo))
                    errors.Add("Voucher No is required");

                if (!row.VoucherDate.HasValue)
                    errors.Add("Voucher Date is required");

                if (string.IsNullOrWhiteSpace(row.AccountCode))
                    errors.Add("Account Code is required");
                else if (!existingCodes.Contains(row.AccountCode))
                    errors.Add("Account Code does not exist");

                if (row.DebitAmount < 0 || row.CreditAmount < 0)
                    errors.Add("Amounts cannot be negative");

                if (row.DebitAmount > 0 && row.CreditAmount > 0)
                    errors.Add("Entry can have either Debit OR Credit, not both");

                if (row.DebitAmount == 0 && row.CreditAmount == 0)
                    errors.Add("Either Debit or Credit must be greater than 0");

                row.IsValid = errors.Count == 0;
                row.ValidationError = errors.Count > 0 ? string.Join("; ", errors) : null;
            }

            // Group by voucher and validate balance
            var groups = rows
                .Where(r => r.IsValid)
                .GroupBy(r => new { r.VoucherNo, r.VoucherDate })
                .Select(g => new JournalVoucherGroup
                {
                    VoucherNo = g.Key.VoucherNo,
                    VoucherDate = g.Key.VoucherDate,
                    Entries = g.ToList(),
                    TotalDebit = g.Sum(e => e.DebitAmount),
                    TotalCredit = g.Sum(e => e.CreditAmount),
                    Difference = g.Sum(e => e.DebitAmount) - g.Sum(e => e.CreditAmount)
                })
                .ToList();

            return groups;
        }

        /// <summary>
        /// Imports Historical Journal Entries in batches
        /// </summary>
        public ImportResultModal ImportJournalEntries(List<JournalEntryImportRow> rows, string fileName, 
            ImportConfigModal config)
        {
            var result = new ImportResultModal
            {
                TotalRows = rows.Count
            };

            int userId = UsersModal.logged_in_userid;
            int sessionId = 0;

            try
            {
                // Validate and group
                var vouchers = ValidateAndGroupJournals(rows);
                var unbalancedVouchers = vouchers.Where(v => !v.IsBalanced).ToList();

                if (unbalancedVouchers.Any())
                {
                    result.Success = false;
                    result.Message = $"Validation failed: {unbalancedVouchers.Count} unbalanced vouchers found";
                    foreach (var v in unbalancedVouchers)
                    {
                        result.Errors.Add(new ImportErrorModal
                        {
                            ErrorType = "UNBALANCED_VOUCHER",
                            ErrorMessage = $"Voucher {v.VoucherNo}: Dr={v.TotalDebit:N2}, Cr={v.TotalCredit:N2}, Diff={v.Difference:N2}",
                            RowData = v.VoucherNo
                        });
                    }
                    return result;
                }

                // Create import session
                sessionId = _dll.CreateImportSession("JOURNAL_HISTORY", fileName, rows.Count, userId);
                result.SessionId = sessionId;

                if (config.DryRunMode)
                {
                    result.Success = true;
                    result.Message = $"Dry run completed. Would import {vouchers.Count} vouchers with {rows.Count(r => r.IsValid)} entries";
                    result.ImportedRows = rows.Count(r => r.IsValid);
                    return result;
                }

                // Import vouchers in batches
                foreach (var voucher in vouchers)
                {
                    try
                    {
                        var voucherId = _dll.CreateVoucherHeader(
                            voucher.VoucherNo,
                            voucher.VoucherDate.Value,
                            "Historical Journal",
                            voucher.TotalDebit,
                            voucher.TotalCredit,
                            "Imported Historical Entry",
                            userId
                        );

                        if (voucherId > 0)
                        {
                            _dll.CreateVoucherEntries(voucherId, voucher.Entries);
                            _dll.LinkVoucherToSession(sessionId, voucherId);
                            result.ImportedRows += voucher.Entries.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorRows += voucher.Entries.Count;
                        result.Errors.Add(new ImportErrorModal
                        {
                            ErrorType = "VOUCHER_ERROR",
                            ErrorMessage = ex.Message,
                            RowData = voucher.VoucherNo
                        });
                    }
                }

                result.SkippedRows = rows.Count - result.ImportedRows - result.ErrorRows;
                result.Success = result.ErrorRows == 0;
                result.Message = $"Import completed: {vouchers.Count} vouchers, {result.ImportedRows} entries imported";

                // Update session
                var errorLog = result.Errors.Any() ? BuildErrorLog(result.Errors) : null;
                _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, result.ErrorRows, 
                    result.Success ? "Completed" : "PartialSuccess", errorLog, config.RollbackHours);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Import failed: " + ex.Message;

                if (sessionId > 0)
                {
                    _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, result.ErrorRows, 
                        "Failed", ex.Message, 0);
                }
            }

            return result;
        }

        #endregion

        #region Import Session Management

        /// <summary>
        /// Gets import history
        /// </summary>
        public DataTable GetImportHistory(string importType = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return _dll.GetImportHistory(importType, fromDate, toDate);
        }

        /// <summary>
        /// Rolls back an import session
        /// </summary>
        public ImportResultModal RollbackImport(int sessionId)
        {
            var result = new ImportResultModal();

            try
            {
                int userId = UsersModal.logged_in_userid;
                var rollbackResult = _dll.RollbackImportSession(sessionId, userId);

                if (rollbackResult.Rows.Count > 0)
                {
                    var row = rollbackResult.Rows[0];
                    result.Success = row["Result"].ToString() == "SUCCESS";
                    result.Message = row["Message"].ToString();
                    result.SessionId = sessionId;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Rollback failed: No result returned";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Rollback failed: " + ex.Message;
            }

            return result;
        }

        #endregion

        #region Helper Methods

        private bool IsEmptyRow(DataRow row)
        {
            foreach (var item in row.ItemArray)
            {
                if (item != null && item != DBNull.Value && !string.IsNullOrWhiteSpace(item.ToString()))
                    return false;
            }
            return true;
        }

        private string GetStringValue(DataRow row, string columnName)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                    return row[columnName].ToString().Trim();
            }
            catch { }
            return null;
        }

        private decimal? GetDecimalValue(DataRow row, string columnName)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                {
                    if (decimal.TryParse(row[columnName].ToString(), out decimal value))
                        return value;
                }
            }
            catch { }
            return null;
        }

        private DateTime? GetDateValue(DataRow row, string columnName)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                {
                    if (DateTime.TryParse(row[columnName].ToString(), out DateTime value))
                        return value;
                }
            }
            catch { }
            return null;
        }

        private bool GetBooleanValue(DataRow row, string columnName)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                {
                    var value = row[columnName].ToString().Trim().ToUpperInvariant();
                    return value == "YES" || value == "TRUE" || value == "1" || value == "Y";
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Builds a simple error log string from error list
        /// </summary>
        private string BuildErrorLog(List<ImportErrorModal> errors)
        {
            var sb = new StringBuilder();
            foreach (var error in errors.Take(100)) // Limit to first 100 errors
            {
                sb.AppendLine($"Row {error.RowNumber}: {error.ErrorType} - {error.ErrorMessage}");
            }
            if (errors.Count > 100)
                sb.AppendLine($"... and {errors.Count - 100} more errors");
            return sb.ToString();
        }

        #endregion

        #region Import Engine Methods

        /// <summary>
        /// Execute COA import with validation and progress reporting
        /// </summary>
        public ImportExecutionResult ExecuteCOAImport(List<ChartOfAccountsImportRow> rows, string fileName,
            ImportConfigModal config, ImportProgressHandler progressCallback = null)
        {
            var result = new ImportExecutionResult
            {
                TotalRows = rows?.Count ?? 0
            };

            var startTime = DateTime.Now;
            var validator = new COAImportValidator();
            int userId = UsersModal.logged_in_userid;

            try
            {
                // Step 1: Validation
                ReportProgress(progressCallback, "Validating Chart of Accounts...", 0, result.TotalRows, 0, 0, 5);

                var validationResult = validator.Validate(rows);

                if (validationResult.ExceedsErrorThreshold)
                {
                    result.Success = false;
                    result.Message = $"Validation failed: {validationResult.ErrorRate:P0} error rate exceeds 50% threshold";
                    result.Errors = validationResult.Errors;
                    return result;
                }

                result.ErrorRows = validationResult.InvalidRows;

                // Step 2: Check hierarchy order
                ReportProgress(progressCallback, "Checking account hierarchy...", 0, result.TotalRows, 0, 0, 10);

                if (!validator.ValidateHierarchyOrder(rows, out string hierarchyError))
                {
                    result.Success = false;
                    result.Message = "Hierarchy validation failed: " + hierarchyError;
                    return result;
                }

                // Step 3: Create import session
                var sessionId = _dll.CreateImportSession("COA", fileName, result.TotalRows, userId);
                result.SessionId = sessionId;

                if (config.DryRunMode)
                {
                    result.Success = true;
                    result.Message = $"Dry run completed. {validationResult.ValidRows} accounts would be imported.";
                    result.ImportedRows = validationResult.ValidRows;
                    result.SkippedRows = validationResult.InvalidRows;
                    return result;
                }

                // Step 4: Import accounts
                var validRows = rows.Where(r => r.IsValid).ToList();
                int imported = 0;

                foreach (var row in validRows)
                {
                    imported++;
                    var percent = 10 + (imported * 80 / validRows.Count); // 10-90%

                    ReportProgress(progressCallback, $"Importing account {imported} of {validRows.Count}: {row.AccountCode}",
                        imported, validRows.Count, validationResult.ValidRows, validationResult.InvalidRows, percent);

                    // Insert account using stored procedure or direct insert
                    _dll.InsertAccount(row, userId);
                }

                result.ImportedRows = imported;
                result.SkippedRows = result.TotalRows - imported;
                result.Success = true;
                result.Message = $"Successfully imported {imported} accounts";

                // Step 5: Update session
                ReportProgress(progressCallback, "Finalizing import...", imported, validRows.Count, imported, result.SkippedRows, 95);

                _dll.UpdateImportSession(sessionId, imported, result.SkippedRows, validationResult.InvalidRows,
                    "Completed", BuildErrorLog(validationResult.Errors), config.RollbackHours);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Import failed: " + ex.Message;

                if (result.SessionId > 0)
                {
                    _dll.UpdateImportSession(result.SessionId, result.ImportedRows, result.SkippedRows,
                        result.ErrorRows, "Failed", ex.Message);
                }
            }
            finally
            {
                result.Duration = DateTime.Now - startTime;
                ReportProgress(progressCallback, "Import complete", result.ImportedRows, result.TotalRows,
                    result.ImportedRows, result.SkippedRows, 100);
            }

            return result;
        }

        /// <summary>
        /// Execute Opening Balance import with trial balance validation
        /// </summary>
        public ImportExecutionResult ExecuteOpeningBalanceImport(List<OpeningBalanceImportRow> rows, DateTime balanceDate,
            string fileName, ImportConfigModal config, ImportProgressHandler progressCallback = null)
        {
            var result = new ImportExecutionResult
            {
                TotalRows = rows?.Count ?? 0
            };

            var startTime = DateTime.Now;
            var validator = new OpeningBalanceValidator();
            int userId = UsersModal.logged_in_userid;
            int branchId = UsersModal.logged_in_branch_id;

            try
            {
                // Step 1: Validation
                ReportProgress(progressCallback, "Validating opening balances...", 0, result.TotalRows, 0, 0, 5);

                var validationResult = validator.ValidateWithAnalysis(rows);

                if (!validationResult.IsValid || validationResult.ExceedsErrorThreshold)
                {
                    result.Success = false;
                    result.Message = "Validation failed: " + (validationResult.ExceedsErrorThreshold
                        ? $"{validationResult.ErrorRate:P0} error rate exceeds 50% threshold"
                        : "Trial balance is not balanced");
                    result.Errors = validationResult.Errors;
                    return result;
                }

                // Step 2: Create import session
                var sessionId = _dll.CreateImportSession("OPENING_BALANCE", fileName, result.TotalRows, userId);
                result.SessionId = sessionId;

                if (config.DryRunMode)
                {
                    result.Success = true;
                    result.Message = $"Dry run completed. Opening balance voucher would be created with {validationResult.ValidRows} entries.";
                    result.ImportedRows = validationResult.ValidRows;
                    return result;
                }

                // Step 3: Create opening balance voucher
                ReportProgress(progressCallback, "Creating opening balance voucher...", 0, result.TotalRows, 0, 0, 50);
                var voucherNo = _dll.GetNextVoucherNumber("OB");

                var totalDebit = (decimal)validationResult.ValidationMetadata["TotalDebit"];
                var totalCredit = (decimal)validationResult.ValidationMetadata["TotalCredit"];

                // Create header
                var headerId = _dll.CreateVoucherHeader(voucherNo, balanceDate, "JV", totalDebit, totalCredit,
                    "Opening Balance", userId, branchId);

                // Create entries
                var entries = rows.Where(r => r.IsValid).Select(r => new JournalEntryImportRow
                {
                    AccountCode = r.AccountCode,
                    DebitAmount = r.DebitAmount,
                    CreditAmount = r.CreditAmount,
                    Narration = r.Remarks ?? "Opening Balance",
                    VoucherDate = balanceDate
                }).ToList();

                _dll.CreateVoucherEntries(headerId, entries);

                // Link to session
                _dll.LinkVoucherToSession(sessionId, headerId);

                result.VouchersCreated = 1;
                result.ImportedRows = validationResult.ValidRows;
                result.SkippedRows = validationResult.InvalidRows;
                result.Success = true;
                result.Message = $"Opening balance posted successfully. Voucher: {voucherNo}";

                // Update session
                _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, validationResult.InvalidRows,
                    "Completed", BuildErrorLog(validationResult.Errors), config.RollbackHours);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Import failed: " + ex.Message;

                if (result.SessionId > 0)
                {
                    _dll.UpdateImportSession(result.SessionId, result.ImportedRows, result.SkippedRows,
                        result.ErrorRows, "Failed", ex.Message);
                }
            }
            finally
            {
                result.Duration = DateTime.Now - startTime;
                ReportProgress(progressCallback, "Import complete", result.ImportedRows, result.TotalRows,
                    result.ImportedRows, result.SkippedRows, 100);
            }

            return result;
        }

        /// <summary>
        /// Execute Journal import with batch processing and cancellation support
        /// </summary>
        public ImportExecutionResult ExecuteJournalImport(List<JournalEntryImportRow> rows, string fileName,
            ImportConfigModal config, ImportProgressHandler progressCallback = null,
            ImportCancellationHandler cancellationCheck = null)
        {
            var result = new ImportExecutionResult
            {
                TotalRows = rows?.Count ?? 0
            };

            var startTime = DateTime.Now;
            var validator = new JournalImportValidator();
            int userId = UsersModal.logged_in_userid;
            int branchId = UsersModal.logged_in_branch_id;

            try
            {
                // Step 1: Validation and grouping
                ReportProgress(progressCallback, "Validating journal entries...", 0, result.TotalRows, 0, 0, 5);

                var validationResult = validator.ValidateWithAnalysis(rows);

                if (!validationResult.IsValid || validationResult.ExceedsErrorThreshold)
                {
                    result.Success = false;
                    result.Message = "Validation failed: " + (validationResult.ExceedsErrorThreshold
                        ? $"{validationResult.ErrorRate:P0} error rate exceeds 50% threshold"
                        : "One or more vouchers are not balanced");
                    result.Errors = validationResult.Errors;
                    return result;
                }

                var vouchers = validator.ValidateAndGroupJournals(rows);
                var balancedVouchers = vouchers.Where(v => v.IsBalanced).ToList();

                // Step 2: Create import session
                var sessionId = _dll.CreateImportSession("JOURNAL_HISTORY", fileName, result.TotalRows, userId);
                result.SessionId = sessionId;

                if (config.DryRunMode)
                {
                    result.Success = true;
                    result.Message = $"Dry run completed. {balancedVouchers.Count} vouchers with {validationResult.ValidRows} entries would be imported.";
                    result.VouchersCreated = balancedVouchers.Count;
                    result.ImportedRows = validationResult.ValidRows;
                    return result;
                }

                // Step 3: Bulk insert with progress
                ReportProgress(progressCallback, "Importing journal vouchers...", 0, balancedVouchers.Count, 0, 0, 10);

                var vouchersImported = _dll.BulkInsertJournalVouchers(balancedVouchers, sessionId, userId, branchId,
                    (current, total) =>
                    {
                        // Check for cancellation
                        if (cancellationCheck != null && cancellationCheck())
                        {
                            throw new OperationCanceledException("Import cancelled by user");
                        }

                        var percent = 10 + (current * 85 / total); // 10-95%
                        ReportProgress(progressCallback, $"Importing voucher {current} of {total}...",
                            current, total, current, 0, percent);
                    });

                result.VouchersCreated = vouchersImported;
                result.ImportedRows = balancedVouchers.Sum(v => v.Entries.Count);
                result.SkippedRows = result.TotalRows - result.ImportedRows;
                result.Success = true;
                result.Message = $"Successfully imported {vouchersImported} vouchers with {result.ImportedRows} entries";

                // Update session
                _dll.UpdateImportSession(sessionId, result.ImportedRows, result.SkippedRows, validationResult.InvalidRows,
                    "Completed", BuildErrorLog(validationResult.Errors), config.RollbackHours);
            }
            catch (OperationCanceledException)
            {
                result.Success = false;
                result.WasCancelled = true;
                result.Message = "Import was cancelled by user";

                if (result.SessionId > 0)
                {
                    // Rollback the import
                    _dll.RollbackImportSession(result.SessionId, userId);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Import failed: " + ex.Message;

                if (result.SessionId > 0)
                {
                    _dll.UpdateImportSession(result.SessionId, result.ImportedRows, result.SkippedRows,
                        result.ErrorRows, "Failed", ex.Message);
                }
            }
            finally
            {
                result.Duration = DateTime.Now - startTime;
                ReportProgress(progressCallback, "Import complete", result.ImportedRows, result.TotalRows,
                    result.ImportedRows, result.SkippedRows, 100);
            }

            return result;
        }

        /// <summary>
        /// Helper to report progress
        /// </summary>
        private void ReportProgress(ImportProgressHandler callback, string operation, int processed, int total,
            int valid, int errors, int percent)
        {
            if (callback == null)
                return;

            var elapsed = DateTime.Now.Subtract(DateTime.Now);
            TimeSpan? estimated = null;

            if (processed > 0 && total > processed)
            {
                var avgTime = elapsed.TotalSeconds / processed;
                var remaining = total - processed;
                estimated = TimeSpan.FromSeconds(avgTime * remaining);
            }

            callback(this, new ImportProgressEventArgs
            {
                CurrentOperation = operation,
                TotalRows = total,
                ProcessedRows = processed,
                ValidRows = valid,
                ErrorRows = errors,
                PercentComplete = percent,
                Elapsed = elapsed,
                Estimated = estimated,
                CanCancel = true
            });
        }

        #endregion
    }
}
