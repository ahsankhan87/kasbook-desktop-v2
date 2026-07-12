using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using POS.Core;
using POS.DLL;

namespace POS.BLL
{
    /// <summary>
    /// Centralized, thread-safe singleton that owns all accounting configuration.
    ///
    /// Lifecycle
    /// ---------
    ///   1. Call <see cref="Instance"/>.<see cref="LoadAll"/> once at application startup
    ///      (e.g. after login succeeds).
    ///   2. All subsequent reads come from the in-memory cache — zero DB round-trips.
    ///   3. <see cref="Set"/> persists to the DB and refreshes the cache atomically.
    ///
    /// Usage
    /// -----
    ///   bool autoPost = AccountingSettingsService.Instance.GetBool(SettingKeys.AutoPostSales);
    ///   AccountsModal ar = AccountingSettingsService.Instance.GetDefaultAccount("AR");
    /// </summary>
    public sealed class AccountingSettingsService
    {
        // ── Singleton ─────────────────────────────────────────────────────

        private static readonly Lazy<AccountingSettingsService> _instance =
            new Lazy<AccountingSettingsService>(() => new AccountingSettingsService(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static AccountingSettingsService Instance => _instance.Value;

        // ── Internal state ────────────────────────────────────────────────

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly AccountingSettingsDLL _dal;
        private bool _loaded;

        private AccountingSettingsService()
        {
            _dal = new AccountingSettingsDLL();
        }

        /// <summary>
        /// Constructor for unit testing — accepts an injected DAL.
        /// Use <see cref="CreateForTesting"/> instead of accessing <see cref="Instance"/>.
        /// </summary>
        internal AccountingSettingsService(AccountingSettingsDLL dal)
        {
            _dal = dal ?? throw new ArgumentNullException("dal");
        }

        /// <summary>
        /// Creates an isolated instance pre-loaded with the supplied settings.
        /// Intended exclusively for unit tests.
        /// </summary>
        public static AccountingSettingsService CreateForTesting(Dictionary<string, string> seed = null)
        {
            var svc = new AccountingSettingsService(new AccountingSettingsDLL());
            if (seed != null)
            {
                foreach (KeyValuePair<string, string> kv in seed)
                    svc._cache[kv.Key] = kv.Value;
                svc._loaded = true;
            }
            return svc;
        }

        // ── 1. LoadAll ────────────────────────────────────────────────────

        /// <summary>
        /// Loads all settings from the database into the in-memory cache.
        /// Must be called once after the application starts (e.g. after login).
        /// Safe to call again to refresh the cache.
        /// </summary>
        public void LoadAll()
        {
            List<AccountingSettingModel> rows = _dal.LoadAll();

            _lock.EnterWriteLock();
            try
            {
                _cache.Clear();
                foreach (AccountingSettingModel row in rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.Key))
                        _cache[row.Key] = row.Value ?? string.Empty;
                }
                _loaded = true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        // ── 2. Type-safe getters ──────────────────────────────────────────

        /// <summary>Returns the raw string value for <paramref name="key"/>.</summary>
        public string GetString(string key, string defaultValue = "")
        {
            string raw = ReadRaw(key);
            return raw != null ? raw : defaultValue;
        }

        /// <summary>Returns the setting as <see cref="int"/>.</summary>
        public int GetInt(string key, int defaultValue = 0)
        {
            string raw = ReadRaw(key);
            int v;
            return (raw != null && int.TryParse(raw, out v)) ? v : defaultValue;
        }

        /// <summary>Returns the setting as <see cref="decimal"/>.</summary>
        public decimal GetDecimal(string key, decimal defaultValue = 0m)
        {
            string raw = ReadRaw(key);
            decimal v;
            return (raw != null && decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) ? v : defaultValue;
        }

        /// <summary>
        /// Returns the setting as <see cref="bool"/>.
        /// Treats "true", "1", "yes" (case-insensitive) as <c>true</c>.
        /// </summary>
        public bool GetBool(string key, bool defaultValue = false)
        {
            string raw = ReadRaw(key);
            if (raw == null) return defaultValue;
            string norm = raw.Trim().ToLowerInvariant();
            if (norm == "true" || norm == "1" || norm == "yes") return true;
            if (norm == "false" || norm == "0" || norm == "no") return false;
            return defaultValue;
        }

        /// <summary>
        /// Returns the account ID (integer) stored in the setting.
        /// Returns 0 when not set or not parsable.
        /// </summary>
        public int GetAccountId(string key, int defaultValue = 0)
        {
            return GetInt(key, defaultValue);
        }

        // ── 3. Set ────────────────────────────────────────────────────────

        /// <summary>
        /// Persists a new value to the database and updates the in-memory cache.
        /// </summary>
        /// <param name="key">Setting key (use <see cref="SettingKeys"/> constants).</param>
        /// <param name="value">New value. Converted to string via <see cref="object.ToString"/>.</param>
        /// <param name="userId">ID of the user making the change (for audit trail).</param>
        public void Set(string key, object value, int userId)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            string stringValue = value == null ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);

            _dal.Upsert(key, stringValue, userId);

            _lock.EnterWriteLock();
            try
            {
                _cache[key] = stringValue;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        // ── 4. GetDefaultAccount ─────────────────────────────────────────

        /// <summary>
        /// Returns the <see cref="AccountsModal"/> configured as the default for
        /// <paramref name="accountPurpose"/>.
        ///
        /// <para>Recognised purposes (case-insensitive):</para>
        /// <list type="bullet">
        ///   <item>AR / RECEIVABLE / DEBTORS</item>
        ///   <item>AP / PAYABLE / CREDITORS</item>
        ///   <item>CASH</item>
        ///   <item>BANK</item>
        ///   <item>SALES / REVENUE</item>
        ///   <item>PURCHASE / COGS</item>
        ///   <item>INVENTORY / STOCK</item>
        ///   <item>TAX / GST / VAT</item>
        ///   <item>OPENING / EQUITY</item>
        ///   <item>RETAINED</item>
        ///   <item>DISCOUNT</item>
        ///   <item>FREIGHT</item>
        /// </list>
        /// </summary>
        /// <exception cref="AccountingConfigException">
        /// Thrown when the setting is empty or the account code does not exist in
        /// <c>acc_accounts</c>.
        /// </exception>
        public AccountsModal GetDefaultAccount(string accountPurpose)
        {
            string key = ResolveAccountKey(accountPurpose);
            string code = GetString(key);

            if (string.IsNullOrWhiteSpace(code))
                throw new AccountingConfigException(
                    string.Format("{0} account is not configured. Go to Accounting Settings → Default Accounts.", accountPurpose),
                    key);

            AccountsModal account = _dal.GetAccountByCode(code);
            if (account == null)
                throw new AccountingConfigException(
                    string.Format("Account code '{0}' for {1} was not found in the chart of accounts.", code, accountPurpose),
                    key);

            return account;
        }

        // ── 5. ValidateAllDefaults ────────────────────────────────────────

        /// <summary>
        /// Checks every entry in <see cref="SettingKeys.RequiredDefaults"/> and returns
        /// a list of descriptive strings for any that are blank or missing.
        /// Returns an empty list when everything is configured correctly.
        /// </summary>
        public List<string> ValidateAllDefaults()
        {
            var missing = new List<string>();

            foreach (string key in SettingKeys.RequiredDefaults)
            {
                string val = GetString(key);
                if (string.IsNullOrWhiteSpace(val))
                    missing.Add(string.Format("Setting '{0}' is not configured.", key));
            }

            return missing;
        }

        // ── 6. GenerateVoucherNo ──────────────────────────────────────────

        /// <summary>
        /// Generates the next sequential voucher number for the given voucher type.
        /// The format is:  {prefix}-{yyyy}-{seq:D5}
        /// e.g. JV-2025-00042
        ///
        /// The sequence counter is stored in <c>pos_settings</c> under key
        /// <c>ACC_VOUCHER_SEQ_{voucherType}</c> and is incremented atomically using
        /// SQL UPDLOCK, making this safe under concurrent users.
        /// </summary>
        /// <param name="voucherType">Short type code, e.g. "JV", "PV", "RV".</param>
        public string GenerateVoucherNo(string voucherType)
        {
            if (string.IsNullOrWhiteSpace(voucherType))
                throw new ArgumentNullException("voucherType");

            string type   = voucherType.Trim().ToUpperInvariant();
            string prefix = GetString(SettingKeys.JournalNumberPrefix, type);
            int    seq    = _dal.GetNextVoucherSequence(type);
            int    year   = DateTime.Today.Year;

            // Format: JV-2025-00042
            return string.Format("{0}-{1}-{2}", prefix, year, seq.ToString("D5"));
        }

        // ── 7. FormatAmount ───────────────────────────────────────────────

        /// <summary>
        /// Formats <paramref name="amount"/> according to the <c>ACC_AMOUNT_FORMAT</c>
        /// setting.
        /// <list type="bullet">
        ///   <item><b>PAKISTANI</b> — uses South-Asian grouping: 1,00,000 (first group
        ///   of 3, subsequent groups of 2).</item>
        ///   <item><b>INTERNATIONAL</b> — standard Western grouping: 1,000,000.</item>
        /// </list>
        /// </summary>
        public string FormatAmount(decimal amount)
        {
            string fmt = GetString(SettingKeys.AmountFormat, "INTERNATIONAL").Trim().ToUpperInvariant();

            if (fmt == "PAKISTANI")
                return FormatPakistani(amount);

            // International / default
            return amount.ToString("N2", CultureInfo.InvariantCulture);
        }

        // ── 8. GetFinancialYearDates ──────────────────────────────────────

        /// <summary>
        /// Returns the start and end dates of the financial year that contains
        /// <paramref name="forDate"/>, based on the <c>ACC_FINANCIAL_YEAR_START_MONTH</c>
        /// setting.
        /// </summary>
        /// <returns>
        /// A tuple of (startDate, endDate) where endDate is the last day of the
        /// final month of the year (i.e. one day before the next year's start date).
        /// </returns>
        public Tuple<DateTime, DateTime> GetFinancialYearDates(DateTime forDate)
        {
            int startMonth = GetInt(SettingKeys.FinancialYearStartMonth, 7);

            // Clamp to valid range in case of bad data
            if (startMonth < 1 || startMonth > 12) startMonth = 7;

            // If forDate's month is on or after the start month, the FY begins this
            // calendar year; otherwise it began the previous calendar year.
            int fyStartYear = forDate.Month >= startMonth ? forDate.Year : forDate.Year - 1;

            DateTime startDate = new DateTime(fyStartYear, startMonth, 1);
            DateTime endDate   = startDate.AddYears(1).AddDays(-1);

            return Tuple.Create(startDate, endDate);
        }

        // ── Private helpers ───────────────────────────────────────────────

        private string ReadRaw(string key)
        {
            if (!_loaded)
            {
                // Lazy-load once; LoadAll handles its own write lock.
                LoadAll();
            }

            _lock.EnterReadLock();
            try
            {
                string val;
                return _cache.TryGetValue(key, out val) ? val : null;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private static string ResolveAccountKey(string purpose)
        {
            if (string.IsNullOrWhiteSpace(purpose))
                throw new ArgumentNullException("purpose");

            switch (purpose.Trim().ToUpperInvariant())
            {
                case "AR":
                case "RECEIVABLE":
                case "DEBTORS":
                    return SettingKeys.DefaultArAccount;

                case "AP":
                case "PAYABLE":
                case "CREDITORS":
                    return SettingKeys.DefaultApAccount;

                case "CASH":
                    return SettingKeys.DefaultCashAccount;

                case "BANK":
                    return SettingKeys.DefaultBankAccount;

                case "SALES":
                case "REVENUE":
                    return SettingKeys.DefaultSalesAccount;

                case "PURCHASE":
                case "PURCHASES":
                case "COGS":
                    return SettingKeys.DefaultPurchaseAccount;

                case "INVENTORY":
                case "STOCK":
                    return SettingKeys.DefaultInventoryAccount;

                case "TAX":
                case "GST":
                case "VAT":
                    return SettingKeys.DefaultSalesTaxAccount;

                case "OPENING":
                case "EQUITY":
                    return SettingKeys.DefaultOpeningEquityAccount;

                case "RETAINED":
                case "RETAINED_EARNINGS":
                    return SettingKeys.DefaultRetainedEarnings;

                case "DISCOUNT":
                    return SettingKeys.DefaultDiscountAccount;

                case "FREIGHT":
                    return SettingKeys.DefaultFreightAccount;

                default:
                    throw new AccountingConfigException(
                        string.Format("Unknown account purpose '{0}'. Valid values: AR, AP, CASH, BANK, SALES, PURCHASE, INVENTORY, TAX, OPENING, RETAINED, DISCOUNT, FREIGHT.", purpose));
            }
        }

        /// <summary>
        /// Formats a decimal using Pakistani/South-Asian number grouping.
        /// Groups: 1,23,45,67,890.00
        /// </summary>
        private static string FormatPakistani(decimal amount)
        {
            bool negative = amount < 0;
            decimal abs   = Math.Abs(amount);

            // Split integer and fractional parts
            long intPart  = (long)Math.Truncate(abs);
            decimal frac  = abs - (decimal)intPart;

            string fracStr = frac.ToString("0.00", CultureInfo.InvariantCulture).Substring(1); // ".XX"

            string intStr  = intPart.ToString(CultureInfo.InvariantCulture);
            string grouped = GroupPakistani(intStr);

            string result  = grouped + fracStr;
            return negative ? "-" + result : result;
        }

        private static string GroupPakistani(string digits)
        {
            if (digits.Length <= 3) return digits;

            // Last 3 digits form the first group from the right
            var sb       = new StringBuilder();
            int firstEnd = digits.Length - 3;
            string rest  = digits.Substring(0, firstEnd);
            string last3 = digits.Substring(firstEnd);

            // Remaining digits are grouped in 2s from the right
            var groups = new List<string>();
            int pos = rest.Length;
            while (pos > 0)
            {
                int start = Math.Max(0, pos - 2);
                groups.Add(rest.Substring(start, pos - start));
                pos = start;
            }

            groups.Reverse();
            foreach (string g in groups)
            {
                sb.Append(g);
                sb.Append(',');
            }
            sb.Append(last3);
            return sb.ToString();
        }
    }
}
