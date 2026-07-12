using System;
using System.Collections.Generic;

namespace POS.Core
{
    public class FinancialPeriodModal
    {
        public int period_id { get; set; }
        public int year_id { get; set; }
        public string financial_year { get; set; }
        public string period_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string status { get; set; }
        public string closed_by { get; set; }
        public DateTime? closed_at { get; set; }
        public int transactions_count { get; set; }
        public bool can_reopen { get; set; }
    }

    public class FinancialPeriodChecklistItemModal
    {
        public string item_key { get; set; }
        public string item_name { get; set; }
        public bool is_passed { get; set; }
        public int pending_count { get; set; }
        public string fix_module { get; set; }
    }

    public class FinancialPeriodCloseOptionsModal
    {
        public int period_id { get; set; }
        public int user_id { get; set; }
        public string close_type { get; set; }
        public bool auto_post_depreciation { get; set; }
        public bool reverse_prior_accruals { get; set; }
        public string confirmation_text { get; set; }
        public string pin_or_password { get; set; }
        public string reopen_reason { get; set; }
    }

    public class FinancialPeriodCloseResultModal
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int affected_rows { get; set; }
        public Dictionary<string, object> extra_data { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }

    public class YearEndCloseOptionsModal
    {
        public int year_id { get; set; }
        public int user_id { get; set; }
        public int? branch_id { get; set; }
        public int? income_summary_account_id { get; set; }
        public int? retained_earnings_account_id { get; set; }
    }

    public class YearEndCloseValidationItemModal
    {
        public string check_key { get; set; }
        public string check_name { get; set; }
        public bool is_passed { get; set; }
        public int failed_count { get; set; }
        public string details { get; set; }
    }

    public class YearEndCloseResultModal
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int run_id { get; set; }
        public decimal net_profit_loss { get; set; }
        public string closing_voucher_no { get; set; }
        public string opening_voucher_no { get; set; }
        public System.Data.DataTable pre_close_validation_report { get; set; } = new System.Data.DataTable();
    }

    public class YearEndRollbackResultModal
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class YearEndCloseProgressEventArgs : EventArgs
    {
        public YearEndCloseProgressEventArgs(string stepMessage)
        {
            StepMessage = stepMessage ?? string.Empty;
        }

        public string StepMessage { get; private set; }
    }
}
