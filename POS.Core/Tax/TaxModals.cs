using System;

namespace POS.Core
{
    /// <summary>
    /// Sales Tax line item for tax register
    /// </summary>
    public class SalesTaxModal
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }  // 'SALES' or 'PURCHASES'
        public string InvoiceNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PartyName { get; set; }
        public string NTN { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public string Remarks { get; set; }
        public int BranchId { get; set; }
    }

    /// <summary>
    /// Withholding Tax transaction
    /// </summary>
    public class WHTModal
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string VATNO { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal WHTRate { get; set; }
        public decimal WHTAmount { get; set; }
        public string TaxSection { get; set; }  // '153', '155', etc.
        public string Remarks { get; set; }
        public int BranchId { get; set; }
    }

    /// <summary>
    /// Summary of WHT by tax section for monthly/period summary
    /// </summary>
    public class WHTSummaryModal
    {
        public string TaxSection { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public decimal TotalWHTAmount { get; set; }
        public int TransactionCount { get; set; }
        public decimal AverageWHTRate { get; set; }
    }

    /// <summary>
    /// Sales Tax summary report (output/input/net)
    /// </summary>
    public class SalesTaxSummaryModal
    {
        // Output Tax (Sales)
        public decimal StandardRatedSalesAmount { get; set; }
        public decimal StandardRatedSalesTax { get; set; }
        public decimal ZeroRatedSalesAmount { get; set; }
        public decimal ExemptSalesAmount { get; set; }
        public decimal TotalOutputTax { get; set; }

        // Input Tax (Purchases)
        public decimal TaxablePurchasesAmount { get; set; }
        public decimal TaxablePurchasesTax { get; set; }
        public decimal ImportTaxPaid { get; set; }
        public decimal TotalInputTax { get; set; }

        // Net Tax
        public decimal NetTaxPayable { get; set; }  // Positive = payable, Negative = refundable

        public string TaxPeriod { get; set; }
        public string TaxRegistrationNo { get; set; }
        public DateTime PeriodFromDate { get; set; }
        public DateTime PeriodToDate { get; set; }
    }

    /// <summary>
    /// Trial Balance line item for tax purposes
    /// </summary>
    public class TaxTrialBalanceModal
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string AccountGroupName { get; set; }
        public string AccountType { get; set; }  // 'Income', 'Expense'
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Balance { get; set; }
    }
}
