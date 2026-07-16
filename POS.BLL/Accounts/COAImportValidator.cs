using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using POS.Core.Accounts;
using POS.DLL;

namespace POS.BLL.Accounts
{
    /// <summary>
    /// Validator for Chart of Accounts import operations
    /// Performs validation including circular reference detection, duplicate checking, and account type validation
    /// </summary>
    public class COAImportValidator
    {
        private readonly AccountsDLL _accountsDLL = new AccountsDLL();
        private HashSet<string> _existingAccountCodes;
        private HashSet<string> _existingGroupCodes;
        private Dictionary<string, string> _validAccountTypes;

        public COAImportValidator()
        {
            LoadExistingData();
        }

        /// <summary>
        /// Load existing account codes and valid account types from database
        /// </summary>
        private void LoadExistingData()
        {
            _existingAccountCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _existingGroupCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _validAccountTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Asset", "Asset" },
                { "Assets", "Asset" },
                { "Liability", "Liability" },
                { "Liabilities", "Liability" },
                { "Equity", "Equity" },
                { "Capital", "Equity" },
                { "Revenue", "Revenue" },
                { "Income", "Revenue" },
                { "Expense", "Expense" },
                { "Expenses", "Expense" }
            };

            try
            {
                // Load existing accounts
                var accounts = _accountsDLL.GetAll();
                if (accounts != null && accounts.Rows.Count > 0)
                {
                    foreach (DataRow row in accounts.Rows)
                    {
                        var code = row["code"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(code))
                        {
                            _existingAccountCodes.Add(code.Trim());
                        }
                    }
                }

                // Load existing groups
                var groups = _accountsDLL.GetGroupAccountByParent(0);
                if (groups != null)
                {
                    LoadGroupCodesRecursively(groups);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load existing account data for validation: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Recursively load all group codes from hierarchy
        /// </summary>
        private void LoadGroupCodesRecursively(DataTable groups)
        {
            if (groups == null || groups.Rows.Count == 0)
                return;

            foreach (DataRow row in groups.Rows)
            {
                var code = row["code"]?.ToString();
                if (!string.IsNullOrWhiteSpace(code))
                {
                    _existingGroupCodes.Add(code.Trim());
                }

                // Get child groups
                var id = Convert.ToInt32(row["id"]);
                var childGroups = _accountsDLL.GetGroupAccountByParent(id);
                if (childGroups != null && childGroups.Rows.Count > 0)
                {
                    LoadGroupCodesRecursively(childGroups);
                }
            }
        }

        /// <summary>
        /// Validates all Chart of Accounts import rows
        /// </summary>
        public ImportValidationResult Validate(List<ChartOfAccountsImportRow> rows)
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

            // Track codes in this import batch
            var batchCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var batchParentReferences = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Validate each row
            foreach (var row in rows)
            {
                ValidateRow(row, batchCodes, batchParentReferences, result);

                if (row.IsValid)
                {
                    result.ValidRows++;
                    batchCodes.Add(row.AccountCode);
                    if (!string.IsNullOrWhiteSpace(row.ParentCode))
                    {
                        batchParentReferences[row.AccountCode] = row.ParentCode;
                    }
                }
                else
                {
                    result.InvalidRows++;
                }
            }

            // Check for circular references
            DetectCircularReferences(batchParentReferences, result);

            result.IsValid = result.InvalidRows == 0 && !result.ExceedsErrorThreshold;

            // Add metadata
            result.ValidationMetadata["TotalValidCodes"] = result.ValidRows;
            result.ValidationMetadata["DuplicatesFound"] = batchCodes.Count < result.ValidRows;
            result.ValidationMetadata["CircularReferencesChecked"] = true;

            return result;
        }

        /// <summary>
        /// Validate individual row
        /// </summary>
        private void ValidateRow(ChartOfAccountsImportRow row, HashSet<string> batchCodes,
            Dictionary<string, string> batchParentReferences, ImportValidationResult result)
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

                // Check for duplicates in database
                if (_existingAccountCodes.Contains(row.AccountCode) || _existingGroupCodes.Contains(row.AccountCode))
                {
                    errors.Add($"Account Code '{row.AccountCode}' already exists in the system");
                }

                // Check for duplicates in batch
                if (batchCodes.Contains(row.AccountCode))
                {
                    errors.Add($"Duplicate Account Code '{row.AccountCode}' in import file");
                }
            }

            if (string.IsNullOrWhiteSpace(row.AccountName))
            {
                errors.Add("Account Name is required");
            }
            else
            {
                row.AccountName = row.AccountName.Trim();
            }

            if (string.IsNullOrWhiteSpace(row.AccountType))
            {
                errors.Add("Account Type is required");
            }
            else
            {
                row.AccountType = row.AccountType.Trim();

                // Validate account type
                if (!_validAccountTypes.ContainsKey(row.AccountType))
                {
                    errors.Add($"Invalid Account Type '{row.AccountType}'. Valid types: Asset, Liability, Equity, Revenue, Expense");
                }
                else
                {
                    // Normalize account type
                    row.AccountType = _validAccountTypes[row.AccountType];
                }
            }

            // Parent code validation
            if (!string.IsNullOrWhiteSpace(row.ParentCode))
            {
                row.ParentCode = row.ParentCode.Trim();

                // Parent must exist in DB or in batch (and be processed before this row)
                if (!_existingAccountCodes.Contains(row.ParentCode) &&
                    !_existingGroupCodes.Contains(row.ParentCode) &&
                    !batchCodes.Contains(row.ParentCode))
                {
                    errors.Add($"Parent Code '{row.ParentCode}' does not exist. Ensure parent accounts appear before child accounts in the file");
                }

                // Self-reference check
                if (row.ParentCode.Equals(row.AccountCode, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add("Account cannot be its own parent");
                }
            }

            // Normal balance validation
            if (!string.IsNullOrWhiteSpace(row.NormalBalance))
            {
                row.NormalBalance = row.NormalBalance.Trim();
                if (!row.NormalBalance.Equals("Debit", StringComparison.OrdinalIgnoreCase) &&
                    !row.NormalBalance.Equals("Credit", StringComparison.OrdinalIgnoreCase) &&
                    !row.NormalBalance.Equals("Dr", StringComparison.OrdinalIgnoreCase) &&
                    !row.NormalBalance.Equals("Cr", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add("Normal Balance must be 'Debit' or 'Credit'");
                }
                else
                {
                    // Normalize
                    if (row.NormalBalance.Equals("Dr", StringComparison.OrdinalIgnoreCase))
                        row.NormalBalance = "Debit";
                    if (row.NormalBalance.Equals("Cr", StringComparison.OrdinalIgnoreCase))
                        row.NormalBalance = "Credit";
                }
            }

            // Opening balance validation
            if (row.OpeningBalance.HasValue && row.OpeningBalance.Value < 0)
            {
                errors.Add("Opening Balance cannot be negative");
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
                    RowData = $"Code: {row.AccountCode}, Name: {row.AccountName}"
                });
            }
            else
            {
                row.IsValid = true;
                row.ValidationError = null;
            }
        }

        /// <summary>
        /// Detect circular references in parent-child relationships
        /// </summary>
        private void DetectCircularReferences(Dictionary<string, string> parentReferences, ImportValidationResult result)
        {
            foreach (var kvp in parentReferences)
            {
                var accountCode = kvp.Key;
                var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (HasCircularReference(accountCode, parentReferences, visited))
                {
                    result.InvalidRows++;
                    result.Errors.Add(new ImportErrorModal
                    {
                        RowNumber = 0,
                        ErrorType = "CIRCULAR_REFERENCE",
                        ErrorMessage = $"Circular reference detected in account hierarchy for code '{accountCode}'",
                        RowData = $"Chain: {string.Join(" -> ", visited)}"
                    });
                }
            }
        }

        /// <summary>
        /// Recursively check for circular references
        /// </summary>
        private bool HasCircularReference(string accountCode, Dictionary<string, string> parentReferences,
            HashSet<string> visited)
        {
            if (visited.Contains(accountCode))
            {
                return true; // Circular reference found
            }

            if (!parentReferences.ContainsKey(accountCode))
            {
                return false; // No parent, no circular reference possible
            }

            visited.Add(accountCode);
            var parentCode = parentReferences[accountCode];

            // Check if parent is in the batch references
            if (parentReferences.ContainsKey(parentCode))
            {
                return HasCircularReference(parentCode, parentReferences, visited);
            }

            return false;
        }

        /// <summary>
        /// Validate account hierarchy order (parents before children)
        /// </summary>
        public bool ValidateHierarchyOrder(List<ChartOfAccountsImportRow> rows, out string errorMessage)
        {
            errorMessage = null;
            var processedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Add existing codes from database
            foreach (var code in _existingAccountCodes)
            {
                processedCodes.Add(code);
            }
            foreach (var code in _existingGroupCodes)
            {
                processedCodes.Add(code);
            }

            foreach (var row in rows.Where(r => r.IsValid))
            {
                if (!string.IsNullOrWhiteSpace(row.ParentCode))
                {
                    if (!processedCodes.Contains(row.ParentCode))
                    {
                        errorMessage = $"Row {row.RowNumber}: Parent '{row.ParentCode}' must appear before child '{row.AccountCode}' in the import file";
                        return false;
                    }
                }

                processedCodes.Add(row.AccountCode);
            }

            return true;
        }
    }
}
