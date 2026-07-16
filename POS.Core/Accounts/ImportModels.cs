using System;
using System.Collections.Generic;

namespace POS.Core.Accounts
{
    #region Import Session Models

    /// <summary>
    /// Represents an import session for tracking and rollback
    /// </summary>
    public class ImportSessionModal
    {
        public int SessionId { get; set; }
        public string ImportType { get; set; }
        public string FileName { get; set; }
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int SkippedRows { get; set; }
        public int ErrorRows { get; set; }
        public string Status { get; set; }
        public string ErrorLog { get; set; }
        public DateTime? RollbackAvailableUntil { get; set; }
        public int ImportedBy { get; set; }
        public DateTime ImportedAt { get; set; }
        public bool CanRollback { get; set; }
        public int VoucherCount { get; set; }
    }

    /// <summary>
    /// Result of an import operation
    /// </summary>
    public class ImportResultModal
    {
        public bool Success { get; set; }
        public int SessionId { get; set; }
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int SkippedRows { get; set; }
        public int ErrorRows { get; set; }
        public string Message { get; set; }
        public List<ImportErrorModal> Errors { get; set; }

        public ImportResultModal()
        {
            Errors = new List<ImportErrorModal>();
        }
    }

    /// <summary>
    /// Validation/import error details
    /// </summary>
    public class ImportErrorModal
    {
        public int RowNumber { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string RowData { get; set; }
    }

    #endregion

    #region Chart of Accounts Import

    /// <summary>
    /// Chart of Accounts import row
    /// </summary>
    public class ChartOfAccountsImportRow
    {
        public int RowNumber { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ParentCode { get; set; }
        public string AccountType { get; set; }
        public string NormalBalance { get; set; }
        public decimal? OpeningBalance { get; set; }
        public DateTime? OpeningBalanceDate { get; set; }
        public bool IsBankAccount { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }

        // Validation
        public bool IsValid { get; set; }
        public string ValidationError { get; set; }
    }

    #endregion

    #region Opening Balance Import

    /// <summary>
    /// Opening balance import row
    /// </summary>
    public class OpeningBalanceImportRow
    {
        public int RowNumber { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Remarks { get; set; }

        // Validation
        public bool IsValid { get; set; }
        public string ValidationError { get; set; }
    }

    /// <summary>
    /// Opening balance validation summary
    /// </summary>
    public class OpeningBalanceValidationResult
    {
        public bool IsValid { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Difference { get; set; }
        public List<ImportErrorModal> Errors { get; set; }

        public OpeningBalanceValidationResult()
        {
            Errors = new List<ImportErrorModal>();
        }
    }

    #endregion

    #region Customer/Supplier Balance Import

    /// <summary>
    /// Customer/Supplier opening balance import row
    /// </summary>
    public class PartyBalanceImportRow
    {
        public int RowNumber { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Remarks { get; set; }

        // Validation
        public bool IsValid { get; set; }
        public string ValidationError { get; set; }
    }

    #endregion

    #region Historical Journal Import

    /// <summary>
    /// Historical journal entry import row
    /// </summary>
    public class JournalEntryImportRow
    {
        public int RowNumber { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string AccountCode { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
        public string ReferenceNo { get; set; }

        // Validation
        public bool IsValid { get; set; }
        public string ValidationError { get; set; }
    }

    /// <summary>
    /// Grouped journal voucher for validation
    /// </summary>
    public class JournalVoucherGroup
    {
        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public List<JournalEntryImportRow> Entries { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Difference { get; set; }
        public bool IsBalanced => Math.Abs(Difference) < 0.01m;

        public JournalVoucherGroup()
        {
            Entries = new List<JournalEntryImportRow>();
        }
    }

    #endregion

    #region Import Configuration

    /// <summary>
    /// Import configuration options
    /// </summary>
    public class ImportConfigModal
    {
        public bool DryRunMode { get; set; }
        public bool SkipValidationErrors { get; set; }
        public int BatchSize { get; set; }
        public int RollbackHours { get; set; }

        public ImportConfigModal()
        {
            DryRunMode = false;
            SkipValidationErrors = false;
            BatchSize = 1000;
            RollbackHours = 24;
        }
    }

    #endregion

    #region Import Engine Models

    /// <summary>
    /// Progress reporting event arguments
    /// </summary>
    public class ImportProgressEventArgs : EventArgs
    {
        public string CurrentOperation { get; set; }
        public int TotalRows { get; set; }
        public int ProcessedRows { get; set; }
        public int ValidRows { get; set; }
        public int ErrorRows { get; set; }
        public int PercentComplete { get; set; }
        public TimeSpan Elapsed { get; set; }
        public TimeSpan? Estimated { get; set; }
        public bool CanCancel { get; set; }
    }

    /// <summary>
    /// Import validation result with detailed error tracking
    /// </summary>
    public class ImportValidationResult
    {
        public bool IsValid { get; set; }
        public int TotalRows { get; set; }
        public int ValidRows { get; set; }
        public int InvalidRows { get; set; }
        public decimal ErrorRate => TotalRows > 0 ? (decimal)InvalidRows / TotalRows : 0;
        public bool ExceedsErrorThreshold => ErrorRate > 0.5m; // 50% threshold
        public List<ImportErrorModal> Errors { get; set; }
        public Dictionary<string, object> ValidationMetadata { get; set; }

        public ImportValidationResult()
        {
            Errors = new List<ImportErrorModal>();
            ValidationMetadata = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// Import execution result with session tracking
    /// </summary>
    public class ImportExecutionResult
    {
        public bool Success { get; set; }
        public int SessionId { get; set; }
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int SkippedRows { get; set; }
        public int ErrorRows { get; set; }
        public int VouchersCreated { get; set; }
        public string Message { get; set; }
        public List<ImportErrorModal> Errors { get; set; }
        public TimeSpan Duration { get; set; }
        public bool WasCancelled { get; set; }

        public ImportExecutionResult()
        {
            Errors = new List<ImportErrorModal>();
        }
    }

    /// <summary>
    /// Batch processing context for bulk imports
    /// </summary>
    public class ImportBatchContext
    {
        public int BatchNumber { get; set; }
        public int BatchSize { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int TotalBatches { get; set; }
        public bool IsLastBatch { get; set; }
    }

    #endregion

    #region Import Engine Delegates

    /// <summary>
    /// Delegate for progress reporting during import operations
    /// </summary>
    public delegate void ImportProgressHandler(object sender, ImportProgressEventArgs e);

    /// <summary>
    /// Delegate for cancellation requests
    /// </summary>
    public delegate bool ImportCancellationHandler();

    #endregion
}
