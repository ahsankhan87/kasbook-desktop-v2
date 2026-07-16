using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using POS.Core.Accounts;
using POS.DLL;

namespace POS.BLL.Accounts
{
    /// <summary>
    /// Validator for Historical Journal Entry import operations
    /// Groups entries by voucher number and validates balanced vouchers
    /// </summary>
    public class JournalImportValidator
    {
        private readonly AccountsDLL _accountsDLL = new AccountsDLL();
        private Dictionary<string, int> _accountCodeToId;

        public JournalImportValidator()
        {
            LoadAccountData();
        }

        /// <summary>
        /// Load account codes and IDs from database
        /// </summary>
        private void LoadAccountData()
        {
            _accountCodeToId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var accounts = _accountsDLL.GetAll();
                if (accounts != null && accounts.Rows.Count > 0)
                {
                    foreach (DataRow row in accounts.Rows)
                    {
                        var code = row["code"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(code))
                        {
                            var id = Convert.ToInt32(row["id"]);
                            _accountCodeToId[code.Trim()] = id;
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
        /// Validates journal entries and groups them by voucher number
        /// </summary>
        public List<JournalVoucherGroup> ValidateAndGroupJournals(List<JournalEntryImportRow> rows)
        {
            if (rows == null || rows.Count == 0)
                return new List<JournalVoucherGroup>();

            // First, validate individual rows
            foreach (var row in rows)
            {
                ValidateRow(row);
            }

            // Group by voucher number
            var voucherGroups = rows
                .Where(r => r.IsValid)
                .GroupBy(r => r.VoucherNo, StringComparer.OrdinalIgnoreCase)
                .Select(g => new JournalVoucherGroup
                {
                    VoucherNo = g.Key,
                    VoucherDate = g.First().VoucherDate,
                    Entries = g.ToList(),
                    TotalDebit = g.Sum(e => e.DebitAmount),
                    TotalCredit = g.Sum(e => e.CreditAmount)
                })
                .ToList();

            // Calculate difference and validate balance
            foreach (var voucher in voucherGroups)
            {
                voucher.Difference = voucher.TotalDebit - voucher.TotalCredit;

                // Validate voucher has at least 2 entries
                if (voucher.Entries.Count < 2)
                {
                    // Mark entries as invalid
                    foreach (var entry in voucher.Entries)
                    {
                        entry.IsValid = false;
                        entry.ValidationError = "Voucher must have at least 2 entries (one debit and one credit)";
                    }
                }

                // Validate all entries have same voucher date
                var distinctDates = voucher.Entries.Select(e => e.VoucherDate).Distinct().Count();
                if (distinctDates > 1)
                {
                    foreach (var entry in voucher.Entries)
                    {
                        entry.IsValid = false;
                        entry.ValidationError = "All entries in a voucher must have the same voucher date";
                    }
                }
            }

            return voucherGroups;
        }

        /// <summary>
        /// Validate individual journal entry row
        /// </summary>
        private void ValidateRow(JournalEntryImportRow row)
        {
            var errors = new List<string>();

            // Required field validation
            if (string.IsNullOrWhiteSpace(row.VoucherNo))
            {
                errors.Add("Voucher No is required");
            }
            else
            {
                row.VoucherNo = row.VoucherNo.Trim();
            }

            if (!row.VoucherDate.HasValue)
            {
                errors.Add("Voucher Date is required");
            }
            else if (row.VoucherDate.Value > DateTime.Today)
            {
                errors.Add("Voucher Date cannot be in the future");
            }

            if (string.IsNullOrWhiteSpace(row.AccountCode))
            {
                errors.Add("Account Code is required");
            }
            else
            {
                row.AccountCode = row.AccountCode.Trim();

                // Check if account exists
                if (!_accountCodeToId.ContainsKey(row.AccountCode))
                {
                    errors.Add($"Account Code '{row.AccountCode}' does not exist in Chart of Accounts");
                }
            }

            // Amount validation - must have either debit or credit, not both, not neither
            bool hasDebit = row.DebitAmount > 0;
            bool hasCredit = row.CreditAmount > 0;

            if (hasDebit && hasCredit)
            {
                errors.Add("Entry cannot have both Debit and Credit amounts");
            }
            else if (!hasDebit && !hasCredit)
            {
                errors.Add("Entry must have either Debit or Credit amount");
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
            }
            else
            {
                row.IsValid = true;
                row.ValidationError = null;
            }
        }

        /// <summary>
        /// Advanced validation with voucher grouping and error analysis
        /// </summary>
        public ImportValidationResult ValidateWithAnalysis(List<JournalEntryImportRow> rows)
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

            // Validate and group
            var voucherGroups = ValidateAndGroupJournals(rows);

            // Count valid/invalid rows
            result.ValidRows = rows.Count(r => r.IsValid);
            result.InvalidRows = rows.Count - result.ValidRows;

            // Collect errors from invalid rows
            foreach (var row in rows.Where(r => !r.IsValid))
            {
                result.Errors.Add(new ImportErrorModal
                {
                    RowNumber = row.RowNumber,
                    ErrorType = "VALIDATION",
                    ErrorMessage = row.ValidationError,
                    RowData = $"Voucher: {row.VoucherNo}, Account: {row.AccountCode}"
                });
            }

            // Validate voucher balances
            var balancedVouchers = 0;
            var unbalancedVouchers = 0;

            foreach (var voucher in voucherGroups)
            {
                if (voucher.IsBalanced)
                {
                    balancedVouchers++;
                }
                else
                {
                    unbalancedVouchers++;
                    result.Errors.Add(new ImportErrorModal
                    {
                        RowNumber = 0,
                        ErrorType = "UNBALANCED_VOUCHER",
                        ErrorMessage = $"Voucher '{voucher.VoucherNo}' is not balanced. Dr: {voucher.TotalDebit:N2}, Cr: {voucher.TotalCredit:N2}, Diff: {voucher.Difference:N2}",
                        RowData = voucher.VoucherNo
                    });
                }
            }

            // Set validation result
            result.IsValid = result.InvalidRows == 0 && unbalancedVouchers == 0 && !result.ExceedsErrorThreshold;

            // Add metadata
            result.ValidationMetadata["TotalVouchers"] = voucherGroups.Count;
            result.ValidationMetadata["BalancedVouchers"] = balancedVouchers;
            result.ValidationMetadata["UnbalancedVouchers"] = unbalancedVouchers;
            result.ValidationMetadata["TotalEntries"] = rows.Count;
            result.ValidationMetadata["ValidEntries"] = result.ValidRows;

            return result;
        }

        /// <summary>
        /// Get account ID by code
        /// </summary>
        public int? GetAccountId(string accountCode)
        {
            if (string.IsNullOrWhiteSpace(accountCode))
                return null;

            return _accountCodeToId.TryGetValue(accountCode.Trim(), out var id) ? (int?)id : null;
        }

        /// <summary>
        /// Validate voucher date range
        /// </summary>
        public List<ImportErrorModal> ValidateDateRange(List<JournalEntryImportRow> rows, DateTime? minDate = null, DateTime? maxDate = null)
        {
            var errors = new List<ImportErrorModal>();

            if (!minDate.HasValue)
                minDate = new DateTime(2000, 1, 1);

            if (!maxDate.HasValue)
                maxDate = DateTime.Today;

            foreach (var row in rows.Where(r => r.IsValid && r.VoucherDate.HasValue))
            {
                if (row.VoucherDate.Value < minDate.Value || row.VoucherDate.Value > maxDate.Value)
                {
                    errors.Add(new ImportErrorModal
                    {
                        RowNumber = row.RowNumber,
                        ErrorType = "DATE_RANGE",
                        ErrorMessage = $"Voucher date {row.VoucherDate.Value:yyyy-MM-dd} is outside allowed range ({minDate.Value:yyyy-MM-dd} to {maxDate.Value:yyyy-MM-dd})",
                        RowData = row.VoucherNo
                    });
                }
            }

            return errors;
        }

        /// <summary>
        /// Check for duplicate voucher numbers in database
        /// </summary>
        public List<ImportErrorModal> CheckDuplicateVouchers(List<JournalVoucherGroup> vouchers)
        {
            var errors = new List<ImportErrorModal>();

            // This would need to query the database to check for existing voucher numbers
            // For now, we'll validate uniqueness within the batch

            var voucherNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var voucher in vouchers)
            {
                if (!voucherNumbers.Add(voucher.VoucherNo))
                {
                    errors.Add(new ImportErrorModal
                    {
                        RowNumber = 0,
                        ErrorType = "DUPLICATE_VOUCHER",
                        ErrorMessage = $"Duplicate voucher number '{voucher.VoucherNo}' found in import file",
                        RowData = voucher.VoucherNo
                    });
                }
            }

            return errors;
        }

        /// <summary>
        /// Validate batch consistency
        /// </summary>
        public List<ImportErrorModal> ValidateBatchConsistency(List<JournalVoucherGroup> vouchers)
        {
            var errors = new List<ImportErrorModal>();

            foreach (var voucher in vouchers)
            {
                // Check entry count
                if (voucher.Entries.Count < 2)
                {
                    errors.Add(new ImportErrorModal
                    {
                        RowNumber = 0,
                        ErrorType = "INSUFFICIENT_ENTRIES",
                        ErrorMessage = $"Voucher '{voucher.VoucherNo}' has only {voucher.Entries.Count} entry. Minimum 2 entries required",
                        RowData = voucher.VoucherNo
                    });
                }

                // Check for at least one debit and one credit
                bool hasDebit = voucher.Entries.Any(e => e.DebitAmount > 0);
                bool hasCredit = voucher.Entries.Any(e => e.CreditAmount > 0);

                if (!hasDebit || !hasCredit)
                {
                    errors.Add(new ImportErrorModal
                    {
                        RowNumber = 0,
                        ErrorType = "MISSING_SIDE",
                        ErrorMessage = $"Voucher '{voucher.VoucherNo}' must have at least one debit and one credit entry",
                        RowData = voucher.VoucherNo
                    });
                }
            }

            return errors;
        }
    }
}
