using System;
using System.Collections.Generic;

namespace POS.Core
{
    public class JournalsModal
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public int entry_id { get; set; }

        public int customer_id { get; set; }
        public int bank_id { get; set; }

        public int supplier_id { get; set; }

        public string invoice_no { get; set; }

        public string voucher_type { get; set; }

        public string reference_no { get; set; }

        public string narration { get; set; }

        public string attachment { get; set; }

        public decimal total_debit { get; set; }

        public decimal total_credit { get; set; }

        public string status { get; set; }

        public int? reversal_of { get; set; }

        public int? posted_by { get; set; }

        public DateTime? posted_at { get; set; }

        public bool is_auto_posted { get; set; }

        public int account_id { get; set; }

        public string account_name { get; set; }

        public string code { get; set; }

        public string description { get; set; }

        public double debit { get; set; }

        public double credit { get; set; }

        public DateTime date_created { get; set; }

        public DateTime date_updated { get; set; }

        public DateTime entry_date { get; set; }

        public int employee_id { get; set; }
        public int period_id { get; set; }

        public string payment_ref_invoice_no { get; set; }
    }

    public class JVHeaderModel
    {
        public int VoucherId { get; set; }

        public string VoucherNo { get; set; }

        public DateTime VoucherDate { get; set; }

        public string VoucherType { get; set; }

        public string ReferenceNo { get; set; }

        public string Narration { get; set; }

        public string Attachment { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public string Status { get; set; }

        public int? ReversalOf { get; set; }

        public int? PostedBy { get; set; }

        public DateTime? PostedAt { get; set; }

        public bool IsAutoPosted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? BranchId { get; set; }

        public int? CompanyId { get; set; }

        public int? PeriodId { get; set; }

    }

    public class JVLineModel
    {
        public int EntryId { get; set; }

        public int VoucherId { get; set; }

        public int LineNo { get; set; }

        public int AccountId { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public string Narration { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public int CostCenterID { get; set; }

        public string ModuleName { get; set; }

        public int? RefId { get; set; }

        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? BankId { get; set; }
        public int? PeriodId { get; set; }

    }

    public class AutoJVModel
    {
        public string ModuleName { get; set; }

        public string RefModule { get; set; }

        public int RefId { get; set; }

        public DateTime VoucherDate { get; set; }

        public string ReferenceNo { get; set; }

        public string Narration { get; set; }

        public bool IsAutoPosted { get; set; }

        public List<JVLineModel> Lines { get; set; } = new List<JVLineModel>();
    }

    public class PostResult
    {
        public bool Success { get; set; }

        public string VoucherNo { get; set; }

        public int VoucherId { get; set; }

        public List<int> EntryIds { get; set; } = new List<int>();

        public List<ValidationError> Messages { get; set; } = new List<ValidationError>();
    }

    public class BatchPostResult
    {
        public int SuccessCount { get; set; }

        public int FailureCount { get; set; }

        public List<int> PostedVoucherIds { get; set; } = new List<int>();

        public List<ValidationError> FailedVouchers { get; set; } = new List<ValidationError>();
    }

    public class ValidationError
    {
        public string VoucherNo { get; set; }

        public int? LineNo { get; set; }

        public string FieldName { get; set; }

        public string Message { get; set; }

        public string Severity { get; set; }

        public bool IsBlocking { get; set; }
    }

    public class JVVoucherModel
    {
        public JVHeaderModel Header { get; set; } = new JVHeaderModel();

        public List<JVLineModel> Lines { get; set; } = new List<JVLineModel>();
    }
}
