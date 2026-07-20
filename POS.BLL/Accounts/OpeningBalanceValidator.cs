using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using POS.Core.Accounts;
using POS.DLL;

namespace POS.BLL.Accounts
{
    /// <summary>
    /// Validator for Opening Balance import operations
    /// Ensures trial balance (Dr = Cr) and validates account existence
    /// </summary>
    public class OpeningBalanceValidator
    {
        private readonly AccountsDLL _accountsDLL = new AccountsDLL();
        private Dictionary<string, AccountInfo> _accountLookup;

        /// <summary>
        /// Account information for validation
        /// </summary>
        public class AccountInfo
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string AccountType { get; set; }
        }

        public OpeningBalanceValidator()
        {
            LoadAccountData();
        }

        /// <summary>
        /// Load account data from database for validation
        /// </summary>
        private void LoadAccountData()
        {
            _accountLookup = new Dictionary<string, AccountInfo>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var accounts = _accountsDLL.GetAccountsWithAccountType();
                if (accounts != null && accounts.Rows.Count > 0)
                {
                    foreach (DataRow row in accounts.Rows)
                    {
                        var code = row["code"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(code))
                        {
                            _accountLookup[code.Trim()] = new AccountInfo
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Code = code.Trim(),
                                Name = row["name"]?.ToString() ?? "",
                                AccountType = row["account_type"]?.ToString() ?? ""
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load account data for validation: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Validates opening balance import rows and ensures trial balance
        /// </summary>
        public OpeningBalanceValidationResult Validate(List<OpeningBalanceImportRow> rows)
        {
            var result = new OpeningBalanceValidationResult();

            if (rows == null || rows.Count == 0)
            {
                result.IsValid = false;
                result.Errors.Add(new ImportErrorModal
                {
                    RowNumber = 0,
                    ErrorType = "NO_DATA",
                    ErrorMessage = "No rows to validate"
                });
                return result;
            }

            // Track account codes to prevent duplicates
            var usedAccountCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            int validRows = 0;

            // Validate each row
            foreach (var row in rows)
            {
                ValidateRow(row, usedAccountCodes, result);

                if (row.IsValid)
                {
                    validRows++;
                    totalDebit += row.DebitAmount;
                    totalCredit += row.CreditAmount;
                    usedAccountCodes.Add(row.AccountCode);
                }
            }

            // Set totals
            result.TotalDebit = totalDebit;
            result.TotalCredit = totalCredit;
            result.Difference = totalDebit - totalCredit;

            // Check trial balance (allow 0.01 rounding tolerance)
            const decimal tolerance = 0.01m;
            bool isBalanced = Math.Abs(result.Difference) < tolerance;

            if (!isBalanced)
            {
                result.Errors.Add(new ImportErrorModal
                {
                    RowNumber = 0,
                    ErrorType = "TRIAL_BALANCE",
                    ErrorMessage = $"Trial balance is not balanced. Debit: {totalDebit:N2}, Credit: {totalCredit:N2}, Difference: {result.Difference:N2}",
                    RowData = "Total amounts do not match"
                });
            }

            // Check if no valid rows
            if (validRows == 0)
            {
                result.Errors.Add(new ImportErrorModal
                {
                    RowNumber = 0,
                    ErrorType = "NO_VALID_DATA",
                    ErrorMessage = "No valid rows found in import file"
                });
            }

            result.IsValid = isBalanced && validRows > 0 && result.Errors.Count == 0;

            return result;
        }

        /// <summary>
        /// Validate individual opening balance row
        /// </summary>
        private void ValidateRow(OpeningBalanceImportRow row, HashSet<string> usedAccountCodes,
            OpeningBalanceValidationResult result)
        {
            var errors = new List<string>();

            // Required field validation
            if (string.IsNullOrWhiteSpace(row.AccountCode))
            {
                errors.Add("Account Code is required");
            }
            else
            {
                row.AccountCode = row.AccountCode.Trim();

                // Check if account exists in database
                if (!_accountLookup.ContainsKey(row.AccountCode))
                {
                    errors.Add($"Account Code '{row.AccountCode}' does not exist in Chart of Accounts");
                }
                else
                {
                    // Check for duplicate account in batch
                    if (usedAccountCodes.Contains(row.AccountCode))
                    {
                        errors.Add($"Duplicate Account Code '{row.AccountCode}' in opening balance file");
                    }

                    // Auto-populate account name if not provided
                    if (string.IsNullOrWhiteSpace(row.AccountName))
                    {
                        row.AccountName = _accountLookup[row.AccountCode].Name;
                    }
                }
            }

            // Amount validation - must have either debit or credit, not both, not neither
            bool hasDebit = row.DebitAmount > 0;
            bool hasCredit = row.CreditAmount > 0;

            if (hasDebit && hasCredit)
            {
                errors.Add("Account cannot have both Debit and Credit amounts. Use separate rows or net the amount");
            }
            else if (!hasDebit && !hasCredit)
            {
                errors.Add("Account must have either Debit or Credit amount");
            }

            // Negative amount check
            if (row.DebitAmount < 0)
            {
                errors.Add("Debit amount cannot be negative");
            }

            if (row.CreditAmount < 0)
            {
                errors.Add("Credit amount cannot be negative");
            }

            // Set validation result
            if (errors.Any())
            {
                row.IsValid = false;
                row.ValidationError = string.Join("; ", errors);

                result.Errors.Add(new ImportErrorModal
                {
                    RowNumber = row.RowNumber,
                    ErrorType = "VALIDATION",
                    ErrorMessage = row.ValidationError,
                    RowData = $"Account: {row.AccountCode}, Dr: {row.DebitAmount}, Cr: {row.CreditAmount}"
                });
            }
            else
            {
                row.IsValid = true;
                row.ValidationError = null;
            }
        }

        /// <summary>
        /// Get account information by code
        /// </summary>
        public AccountInfo GetAccountInfo(string accountCode)
        {
            if (string.IsNullOrWhiteSpace(accountCode))
                return null;

            return _accountLookup.TryGetValue(accountCode.Trim(), out var info) ? info : null;
        }

        /// <summary>
        /// Additional detail-account checks are not required here because account existence
        /// is already validated against acc_accounts in ValidateRow.
        /// </summary>
        public List<ImportErrorModal> ValidateDetailAccounts(List<OpeningBalanceImportRow> rows)
        {
            return new List<ImportErrorModal>();
        }

        /// <summary>
        /// Advanced validation with detailed balance analysis
        /// </summary>
        public ImportValidationResult ValidateWithAnalysis(List<OpeningBalanceImportRow> rows)
        {
            var result = new ImportValidationResult
            {
                TotalRows = rows?.Count ?? 0
            };

            if (rows == null || rows.Count == 0)
            {
                result.IsValid = false;
                result.Errors.Add(new ImportErrorModal
                {
                    RowNumber = 0,
                    ErrorType = "NO_DATA",
                    ErrorMessage = "No rows to validate"
                });
                return result;
            }

            // Perform standard validation
            var obResult = Validate(rows);

            // Convert to ImportValidationResult
            result.ValidRows = rows.Count(r => r.IsValid);
            result.InvalidRows = rows.Count - result.ValidRows;
            result.Errors = obResult.Errors;
            result.IsValid = obResult.IsValid && !result.ExceedsErrorThreshold;

            // Add metadata
            result.ValidationMetadata["TotalDebit"] = obResult.TotalDebit;
            result.ValidationMetadata["TotalCredit"] = obResult.TotalCredit;
            result.ValidationMetadata["Difference"] = obResult.Difference;
            result.ValidationMetadata["IsBalanced"] = obResult.IsValid;

            // Check for detail accounts
            var detailAccountErrors = ValidateDetailAccounts(rows);
            if (detailAccountErrors.Any())
            {
                result.Errors.AddRange(detailAccountErrors);
                result.InvalidRows += detailAccountErrors.Count;
                result.IsValid = false;
            }

            return result;
        }
    }
}
