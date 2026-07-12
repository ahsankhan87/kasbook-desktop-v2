namespace POS.Core
{
    /// <summary>
    /// Strongly-typed constants for every key stored in pos_settings.
    /// Use these instead of magic strings throughout the accounting module.
    /// </summary>
    public static class SettingKeys
    {
        // ── Default Accounts ──────────────────────────────────────────────
        /// <summary>Account code for trade debtors / accounts receivable.</summary>
        public const string DefaultArAccount          = "ACC_DEFAULT_AR_ACCOUNT";

        /// <summary>Account code for trade creditors / accounts payable.</summary>
        public const string DefaultApAccount          = "ACC_DEFAULT_AP_ACCOUNT";

        /// <summary>Account code for petty cash.</summary>
        public const string DefaultCashAccount        = "ACC_DEFAULT_CASH_ACCOUNT";

        /// <summary>Account code for the primary bank account.</summary>
        public const string DefaultBankAccount        = "ACC_DEFAULT_BANK_ACCOUNT";

        /// <summary>Account code for the default sales / revenue account.</summary>
        public const string DefaultSalesAccount       = "ACC_DEFAULT_SALES_ACCOUNT";

        /// <summary>Account code for purchases / COGS.</summary>
        public const string DefaultPurchaseAccount    = "ACC_DEFAULT_PURCHASE_ACCOUNT";

        /// <summary>Account code for inventory / stock in hand.</summary>
        public const string DefaultInventoryAccount   = "ACC_DEFAULT_STOCK_ACCOUNT";

        /// <summary>Account code for cost of goods sold.</summary>
        public const string DefaultCogsAccount        = "ACC_DEFAULT_PURCHASE_ACCOUNT";

        /// <summary>Account code for GST / sales tax payable.</summary>
        public const string DefaultSalesTaxAccount    = "ACC_DEFAULT_TAX_PAYABLE";

        /// <summary>Account code for opening balance suspense / equity.</summary>
        public const string DefaultOpeningEquityAccount = "ACC_DEFAULT_OPENING_BALANCE_AC";

        /// <summary>Account code for retained earnings.</summary>
        public const string DefaultRetainedEarnings   = "ACC_DEFAULT_RETAINED_EARNINGS";

        /// <summary>Account code for trade discounts given.</summary>
        public const string DefaultDiscountAccount    = "ACC_DEFAULT_DISCOUNT_ACCOUNT";

        /// <summary>Account code for freight / carriage inward.</summary>
        public const string DefaultFreightAccount     = "ACC_DEFAULT_FREIGHT_ACCOUNT";

        // ── Accounting Behaviour ──────────────────────────────────────────
        /// <summary>Month (1-12) when the financial year starts. 7 = July.</summary>
        public const string FinancialYearStartMonth   = "ACC_FINANCIAL_YEAR_START_MONTH";

        /// <summary>ISO currency code for the base currency (e.g. PKR).</summary>
        public const string BaseCurrency              = "ACC_BASE_CURRENCY";

        /// <summary>Inventory valuation method: WAC | FIFO | LIFO.</summary>
        public const string ValuationMethod           = "ACC_VALUATION_METHOD";

        /// <summary>Maximum days a transaction can be backdated.</summary>
        public const string BackdatingLimitDays       = "ACC_BACKDATING_LIMIT_DAYS";

        /// <summary>Whether every journal entry must have a narration.</summary>
        public const string RequireNarration          = "ACC_REQUIRE_NARRATION";

        /// <summary>Whether to auto-post entries when a sales invoice is saved.</summary>
        public const string AutoPostSales             = "ACC_AUTO_POST_SALES";

        /// <summary>Whether to auto-post entries when a purchase invoice is saved.</summary>
        public const string AutoPostPurchases         = "ACC_AUTO_POST_PURCHASES";

        /// <summary>Percentage at which a budget warning is shown (0-100).</summary>
        public const string BudgetWarningPct          = "ACC_BUDGET_WARNING_PCT";

        /// <summary>Amount above which journal entries require approval.</summary>
        public const string ApprovalThreshold         = "ACC_APPROVAL_THRESHOLD";

        // ── Display / Formatting ──────────────────────────────────────────
        /// <summary>Amount display format: PAKISTANI | INTERNATIONAL.</summary>
        public const string AmountFormat              = "ACC_AMOUNT_FORMAT";

        /// <summary>Text appended to the footer of every accounting report.</summary>
        public const string ReportFooter              = "ACC_REPORT_FOOTER";

        // ── Journal Auto-Numbering ────────────────────────────────────────
        /// <summary>Whether to auto-generate sequential journal voucher numbers.</summary>
        public const string JournalAutoNumber         = "ACC_JOURNAL_AUTO_NUMBER";

        /// <summary>Prefix for auto-generated journal voucher numbers.</summary>
        public const string JournalNumberPrefix       = "ACC_JOURNAL_NUMBER_PREFIX";

        /// <summary>Whether unbalanced draft entries are allowed.</summary>
        public const string AllowUnbalancedDrafts     = "ACC_ALLOW_UNBALANCED_DRAFTS";

        // ── Setup / General ───────────────────────────────────────────────
        /// <summary>True once the company setup wizard has been completed.</summary>
        public const string CompanySetupComplete      = "COMPANY_SETUP_COMPLETE";

        // ── Voucher sequence counter key prefix ───────────────────────────
        /// <summary>
        /// Prefix for per-voucher-type sequence counter keys.
        /// Full key = VoucherSeqPrefix + voucherType  (e.g. "ACC_VOUCHER_SEQ_JV").
        /// </summary>
        public const string VoucherSeqPrefix          = "ACC_VOUCHER_SEQ_";

        // ── Critical defaults that must be set before posting ─────────────
        /// <summary>All keys that are required for the accounting module to function.</summary>
        public static readonly string[] RequiredDefaults = new[]
        {
            DefaultArAccount,
            DefaultApAccount,
            DefaultCashAccount,
            DefaultBankAccount,
            DefaultSalesAccount,
            DefaultPurchaseAccount,
            DefaultInventoryAccount,
            DefaultSalesTaxAccount,
            DefaultOpeningEquityAccount,
            FinancialYearStartMonth,
        };
    }
}
