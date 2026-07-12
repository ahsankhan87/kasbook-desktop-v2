using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace POS.BLL
{
    /// <summary>
    /// Business logic layer for Withholding Tax (WHT) calculations and reporting
    /// </summary>
    public class WHTCalculationBLL
    {
        private WHTRegistrationDLL _whtDll = new WHTRegistrationDLL();

        /// <summary>
        /// Gets all WHT transactions for a period
        /// </summary>
        public List<WHTModal> GetWHTReport(DateTime fromDate, DateTime toDate)
        {
            return _whtDll.GetWHTReportList(fromDate, toDate);
        }

        /// <summary>
        /// Gets WHT transactions as data table for grid binding
        /// </summary>
        public DataTable GetWHTReportDataTable(DateTime fromDate, DateTime toDate)
        {
            return _whtDll.GetWHTReport(fromDate, toDate);
        }

        /// <summary>
        /// Gets WHT summary grouped by tax section (for monthly summary grid)
        /// </summary>
        public List<WHTSummaryModal> GetWHTSummaryBySection(DateTime fromDate, DateTime toDate)
        {
            return _whtDll.GetWHTSummaryBySection(fromDate, toDate);
        }

        /// <summary>
        /// Gets monthly summary of WHT deductions
        /// </summary>
        public DataTable GetWHTMonthlySummary(DateTime fromDate, DateTime toDate)
        {
            return _whtDll.GetWHTMonthlySummary(fromDate, toDate);
        }

        /// <summary>
        /// Calculates total WHT for a period
        /// </summary>
        public (decimal TotalPaymentAmount, decimal TotalWHTAmount, int TransactionCount) GetWHTTotals(DateTime fromDate, DateTime toDate)
        {
            List<WHTModal> whtData = GetWHTReport(fromDate, toDate);

            if (whtData == null || whtData.Count == 0)
                return (0, 0, 0);

            return (
                whtData.Sum(w => w.PaymentAmount),
                whtData.Sum(w => w.WHTAmount),
                whtData.Count
            );
        }

        /// <summary>
        /// Gets average WHT rate for the period
        /// </summary>
        public decimal GetAverageWHTRate(DateTime fromDate, DateTime toDate)
        {
            var totals = GetWHTTotals(fromDate, toDate);

            if (totals.TotalPaymentAmount == 0)
                return 0;

            return (totals.TotalWHTAmount / totals.TotalPaymentAmount) * 100;
        }

        /// <summary>
        /// Prepares WHT data for Excel export
        /// </summary>
        public DataTable PrepareWHTExportData(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = GetWHTReportDataTable(fromDate, toDate);

            if (dt != null)
            {
                // Rename columns for export-friendly display
                dt.Columns["SupplierName"].ColumnName = "Supplier Name";
                dt.Columns["VATNO"].ColumnName = "VAT/NTN No";
                dt.Columns["PaymentDate"].ColumnName = "Payment Date";
                dt.Columns["PaymentAmount"].ColumnName = "Payment Amount";
                dt.Columns["WHTRate"].ColumnName = "WHT Rate %";
                dt.Columns["WHTAmount"].ColumnName = "WHT Amount";
                dt.Columns["TaxSection"].ColumnName = "Tax Section";

                // Remove unnecessary columns
                if (dt.Columns.Contains("SupplierId"))
                    dt.Columns.Remove("SupplierId");
                if (dt.Columns.Contains("Remarks"))
                    dt.Columns.Remove("Remarks");
                if (dt.Columns.Contains("branch_id"))
                    dt.Columns.Remove("branch_id");
            }

            return dt;
        }

        /// <summary>
        /// Groups WHT by supplier for analysis
        /// </summary>
        public DataTable GetWHTBySupplier(DateTime fromDate, DateTime toDate)
        {
            List<WHTModal> whtData = GetWHTReport(fromDate, toDate);

            DataTable resultDt = new DataTable();
            resultDt.Columns.Add("Supplier Name", typeof(string));
            resultDt.Columns.Add("Total Payments", typeof(decimal));
            resultDt.Columns.Add("Total WHT", typeof(decimal));
            resultDt.Columns.Add("Average WHT Rate %", typeof(decimal));
            resultDt.Columns.Add("Transaction Count", typeof(int));

            if (whtData != null && whtData.Count > 0)
            {
                var supplierGroups = whtData.GroupBy(w => w.SupplierName);

                foreach (var group in supplierGroups)
                {
                    decimal totalPayments = group.Sum(w => w.PaymentAmount);
                    decimal totalWHT = group.Sum(w => w.WHTAmount);
                    decimal avgRate = totalPayments > 0 ? (totalWHT / totalPayments) * 100 : 0;

                    resultDt.Rows.Add(
                        group.Key,
                        totalPayments,
                        totalWHT,
                        avgRate,
                        group.Count()
                    );
                }
            }

            return resultDt;
        }
    }
}
