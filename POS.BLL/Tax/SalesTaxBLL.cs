using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace POS.BLL
{
    /// <summary>
    /// Business logic layer for Sales Tax calculations and reporting
    /// </summary>
    public class SalesTaxBLL
    {
        private TaxRegistrationDLL _taxDll = new TaxRegistrationDLL();
        private const decimal STANDARD_TAX_RATE = 0.17m;  // 17% standard rate for Saudi Arabia

        /// <summary>
        /// Calculates comprehensive sales tax summary
        /// </summary>
        public SalesTaxSummaryModal CalculateSalesTaxSummary(DateTime fromDate, DateTime toDate, string taxRegNo)
        {
            SalesTaxSummaryModal summary = new SalesTaxSummaryModal
            {
                TaxPeriod = GetTaxPeriodString(fromDate, toDate),
                TaxRegistrationNo = taxRegNo,
                PeriodFromDate = fromDate,
                PeriodToDate = toDate
            };

            try
            {
                List<SalesTaxModal> salesData = _taxDll.GetSalesTaxRegisterSalesOnly(fromDate, toDate);
                List<SalesTaxModal> purchaseData = _taxDll.GetSalesTaxRegisterPurchasesOnly(fromDate, toDate);

                // Calculate Output Tax from Sales
                if (salesData != null && salesData.Count > 0)
                {
                    // Group by tax rate to categorize sales
                    var salesByRate = salesData.GroupBy(s => s.TaxRate);

                    foreach (var group in salesByRate)
                    {
                        decimal rate = group.Key;
                        decimal amount = group.Sum(s => s.Amount);
                        decimal tax = group.Sum(s => s.TaxAmount);

                        if (rate == STANDARD_TAX_RATE)
                        {
                            summary.StandardRatedSalesAmount += amount;
                            summary.StandardRatedSalesTax += tax;
                        }
                        else if (rate == 0)
                        {
                            summary.ZeroRatedSalesAmount += amount;
                        }
                        else if (rate < 0.01m)  // Exempt transactions typically have 0 or negligible rate
                        {
                            summary.ExemptSalesAmount += amount;
                        }
                    }
                }

                summary.TotalOutputTax = summary.StandardRatedSalesTax;

                // Calculate Input Tax from Purchases
                if (purchaseData != null && purchaseData.Count > 0)
                {
                    summary.TaxablePurchasesAmount = purchaseData.Sum(p => p.Amount);
                    summary.TaxablePurchasesTax = purchaseData.Sum(p => p.TaxAmount);
                }

                summary.TotalInputTax = summary.TaxablePurchasesTax + summary.ImportTaxPaid;

                // Calculate Net Tax Payable or Refundable
                summary.NetTaxPayable = summary.TotalOutputTax - summary.TotalInputTax;

                return summary;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets sales tax register with optional filtering
        /// </summary>
        public List<SalesTaxModal> GetSalesTaxRegister(DateTime fromDate, DateTime toDate, string taxType = "ALL")
        {
            return _taxDll.GetSalesTaxRegisterList(fromDate, toDate, taxType);
        }

        /// <summary>
        /// Gets data table for grid binding
        /// </summary>
        public DataTable GetSalesTaxRegisterDataTable(DateTime fromDate, DateTime toDate, string taxType = "ALL")
        {
            return _taxDll.GetSalesTaxRegister(fromDate, toDate, taxType);
        }

        /// <summary>
        /// Exports sales tax register to Excel format
        /// </summary>
        public DataTable PrepareExportData(DateTime fromDate, DateTime toDate, string taxType = "ALL")
        {
            DataTable dt = GetSalesTaxRegisterDataTable(fromDate, toDate, taxType);

            if (dt != null)
            {
                // Rename columns for export-friendly display
                dt.Columns["TransactionType"].ColumnName = "Type";
                dt.Columns["InvoiceNo"].ColumnName = "Invoice No";
                dt.Columns["Date"].ColumnName = "Transaction Date";
                dt.Columns["PartyName"].ColumnName = "Party Name";
                dt.Columns["NTN"].ColumnName = "NTN/VAT No";
                dt.Columns["Amount"].ColumnName = "Taxable Amount";
                dt.Columns["TaxRate"].ColumnName = "Tax Rate %";
                dt.Columns["TaxAmount"].ColumnName = "Tax Amount";

                // Remove unnecessary columns
                if (dt.Columns.Contains("TransactionId"))
                    dt.Columns.Remove("TransactionId");
                if (dt.Columns.Contains("Remarks"))
                    dt.Columns.Remove("Remarks");
                if (dt.Columns.Contains("branch_id"))
                    dt.Columns.Remove("branch_id");
            }

            return dt;
        }

        /// <summary>
        /// Helper to format tax period display string
        /// </summary>
        private string GetTaxPeriodString(DateTime fromDate, DateTime toDate)
        {
            int daysInRange = (toDate - fromDate).Days;

            if (daysInRange <= 31)
            {
                return $"{fromDate:MMMM yyyy}";  // Monthly format
            }
            else if (daysInRange <= 100)
            {
                int quarter = (fromDate.Month - 1) / 3 + 1;
                return $"Q{quarter} {fromDate:yyyy}";  // Quarterly format
            }
            else
            {
                return $"{fromDate:yyyy}";  // Annual format
            }
        }
    }
}
