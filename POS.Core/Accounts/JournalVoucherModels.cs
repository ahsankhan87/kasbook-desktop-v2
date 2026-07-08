using System;
using System.Collections.Generic;

namespace POS.Core
{
    /// <summary>
    /// Represents the journal voucher header record used by the accounting engine.
    /// </summary>
    public class JVHeaderModel
    {
        /// <summary>
        /// Gets or sets the header identifier.
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the voucher number.
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// Gets or sets the voucher date.
        /// </summary>
        public DateTime VoucherDate { get; set; }

        /// <summary>
        /// Gets or sets the voucher type.
        /// </summary>
        public string VoucherType { get; set; }

        /// <summary>
        /// Gets or sets the optional reference number.
        /// </summary>
        public string ReferenceNo { get; set; }

        /// <summary>
        /// Gets or sets the voucher narration.
        /// </summary>
        public string Narration { get; set; }

        /// <summary>
        /// Gets or sets the attachment path or attachment reference.
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// Gets or sets the total debit amount for the voucher.
        /// </summary>
        public decimal TotalDebit { get; set; }

        /// <summary>
        /// Gets or sets the total credit amount for the voucher.
        /// </summary>
        public decimal TotalCredit { get; set; }

        /// <summary>
        /// Gets or sets the voucher status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the original voucher identifier when this voucher is a reversal.
        /// </summary>
        public int? ReversalOf { get; set; }

        /// <summary>
        /// Gets or sets the user who posted the voucher.
        /// </summary>
        public int? PostedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the voucher was posted.
        /// </summary>
        public DateTime? PostedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the voucher was auto-posted by another module.
        /// </summary>
        public bool IsAutoPosted { get; set; }

        /// <summary>
        /// Gets or sets the user who created the voucher.
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the voucher was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated the voucher.
        /// </summary>
        public int? UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the voucher was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the branch identifier.
        /// </summary>
        public int? BranchId { get; set; }

        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the source module name for auto-posting.
        /// </summary>
        public string RefModule { get; set; }

        /// <summary>
        /// Gets or sets the source record identifier for auto-posting.
        /// </summary>
        public int? RefId { get; set; }
    }

    /// <summary>
    /// Represents a single journal voucher line.
    /// </summary>
    public class JVLineModel
    {
        /// <summary>
        /// Gets or sets the entry identifier.
        /// </summary>
        public int EntryId { get; set; }

        /// <summary>
        /// Gets or sets the voucher identifier.
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        public int LineNo { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the account code for display.
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// Gets or sets the account name for display.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the narration for this line.
        /// </summary>
        public string Narration { get; set; }

        /// <summary>
        /// Gets or sets the debit amount.
        /// </summary>
        public decimal Debit { get; set; }

        /// <summary>
        /// Gets or sets the credit amount.
        /// </summary>
        public decimal Credit { get; set; }

        /// <summary>
        /// Gets or sets the optional cost center code.
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets the originating module name.
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the originating reference identifier.
        /// </summary>
        public int? RefId { get; set; }
    }

    /// <summary>
    /// Represents a journal voucher request from an external module for auto-posting.
    /// </summary>
    public class AutoJVModel
    {
        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the reference module name.
        /// </summary>
        public string RefModule { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int RefId { get; set; }

        /// <summary>
        /// Gets or sets the voucher date.
        /// </summary>
        public DateTime VoucherDate { get; set; }

        /// <summary>
        /// Gets or sets the optional reference number.
        /// </summary>
        public string ReferenceNo { get; set; }

        /// <summary>
        /// Gets or sets the narration for the auto-posted entry.
        /// </summary>
        public string Narration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entry should be flagged as auto-posted.
        /// </summary>
        public bool IsAutoPosted { get; set; }

        /// <summary>
        /// Gets or sets the journal lines.
        /// </summary>
        public List<JVLineModel> Lines { get; set; } = new List<JVLineModel>();
    }

    /// <summary>
    /// Represents the result of posting a journal voucher.
    /// </summary>
    public class PostResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the posted voucher number.
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// Gets or sets the voucher identifier.
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the inserted entry identifiers.
        /// </summary>
        public List<int> EntryIds { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets a collection of validation or processing messages.
        /// </summary>
        public List<ValidationError> Messages { get; set; } = new List<ValidationError>();
    }

    /// <summary>
    /// Represents the result of posting a batch of vouchers.
    /// </summary>
    public class BatchPostResult
    {
        /// <summary>
        /// Gets or sets the number of successful vouchers.
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed vouchers.
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// Gets or sets the posted voucher identifiers.
        /// </summary>
        public List<int> PostedVoucherIds { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the failed voucher details.
        /// </summary>
        public List<ValidationError> FailedVouchers { get; set; } = new List<ValidationError>();
    }

    /// <summary>
    /// Represents a validation or processing error for a journal voucher line or batch item.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets or sets the voucher number associated with the error.
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// Gets or sets the line number associated with the error.
        /// </summary>
        public int? LineNo { get; set; }

        /// <summary>
        /// Gets or sets the field name associated with the error.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the severity of the error.
        /// </summary>
        public string Severity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the error blocks posting.
        /// </summary>
        public bool IsBlocking { get; set; }
    }

    /// <summary>
    /// Represents a voucher with its header and associated journal lines.
    /// </summary>
    public class JVVoucherModel
    {
        /// <summary>
        /// Gets or sets the voucher header.
        /// </summary>
        public JVHeaderModel Header { get; set; } = new JVHeaderModel();

        /// <summary>
        /// Gets or sets the voucher lines.
        /// </summary>
        public List<JVLineModel> Lines { get; set; } = new List<JVLineModel>();
    }
}
