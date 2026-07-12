using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    /// <summary>
    /// Data access layer for Withholding Tax (WHT) queries
    /// </summary>
    public class WHTRegistrationDLL
    {
        private dbConnection _db = new dbConnection();

        /// <summary>
        /// Retrieves all WHT deductions for a date range
        /// </summary>
        public DataTable GetWHTReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_WHTReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves WHT report as modal list
        /// </summary>
        public List<WHTModal> GetWHTReportList(DateTime fromDate, DateTime toDate)
        {
            List<WHTModal> result = new List<WHTModal>();
            DataTable dt = GetWHTReport(fromDate, toDate);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new WHTModal
                    {
                        SupplierId = Convert.ToInt32(row["SupplierId"]),
                        SupplierName = Convert.ToString(row["SupplierName"]),
                        VATNO = Convert.ToString(row["VATNO"] ?? string.Empty),
                        PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                        PaymentAmount = Convert.ToDecimal(row["PaymentAmount"]),
                        WHTRate = Convert.ToDecimal(row["WHTRate"]),
                        WHTAmount = Convert.ToDecimal(row["WHTAmount"]),
                        TaxSection = Convert.ToString(row["TaxSection"] ?? string.Empty),
                        Remarks = Convert.ToString(row["Remarks"] ?? string.Empty),
                        BranchId = Convert.ToInt32(row["branch_id"])
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets WHT summary grouped by tax section
        /// </summary>
        public List<WHTSummaryModal> GetWHTSummaryBySection(DateTime fromDate, DateTime toDate)
        {
            List<WHTSummaryModal> result = new List<WHTSummaryModal>();
            List<WHTModal> allWHT = GetWHTReportList(fromDate, toDate);

            if (allWHT.Count == 0)
                return result;

            // Group by tax section
            Dictionary<string, WHTSummaryModal> summaryDict = new Dictionary<string, WHTSummaryModal>();

            foreach (var wht in allWHT)
            {
                string section = wht.TaxSection ?? "Other";

                if (!summaryDict.ContainsKey(section))
                {
                    summaryDict[section] = new WHTSummaryModal
                    {
                        TaxSection = section,
                        TotalPaymentAmount = 0,
                        TotalWHTAmount = 0,
                        TransactionCount = 0,
                        AverageWHTRate = 0
                    };
                }

                summaryDict[section].TotalPaymentAmount += wht.PaymentAmount;
                summaryDict[section].TotalWHTAmount += wht.WHTAmount;
                summaryDict[section].TransactionCount += 1;
            }

            // Calculate average rates
            foreach (var summary in summaryDict.Values)
            {
                if (summary.TransactionCount > 0)
                {
                    summary.AverageWHTRate = (summary.TotalWHTAmount / summary.TotalPaymentAmount) * 100;
                }
            }

            result.AddRange(summaryDict.Values);
            return result;
        }

        /// <summary>
        /// Gets WHT summary grouped by month
        /// </summary>
        public DataTable GetWHTMonthlySummary(DateTime fromDate, DateTime toDate)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_WHTReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }

                // Create summary table
                DataTable summaryDt = new DataTable();
                summaryDt.Columns.Add("Month", typeof(string));
                summaryDt.Columns.Add("TotalPaymentAmount", typeof(decimal));
                summaryDt.Columns.Add("TotalWHTAmount", typeof(decimal));
                summaryDt.Columns.Add("TransactionCount", typeof(int));

                if (dt != null && dt.Rows.Count > 0)
                {
                    Dictionary<string, DataRow> monthSummary = new Dictionary<string, DataRow>();

                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime paymentDate = Convert.ToDateTime(row["PaymentDate"]);
                        string monthKey = paymentDate.ToString("yyyy-MM");

                        if (!monthSummary.ContainsKey(monthKey))
                        {
                            DataRow summaryRow = summaryDt.NewRow();
                            summaryRow["Month"] = monthKey;
                            summaryRow["TotalPaymentAmount"] = 0m;
                            summaryRow["TotalWHTAmount"] = 0m;
                            summaryRow["TransactionCount"] = 0;
                            summaryDt.Rows.Add(summaryRow);
                            monthSummary[monthKey] = summaryRow;
                        }

                        DataRow mRow = monthSummary[monthKey];
                        mRow["TotalPaymentAmount"] = Convert.ToDecimal(mRow["TotalPaymentAmount"]) + Convert.ToDecimal(row["PaymentAmount"]);
                        mRow["TotalWHTAmount"] = Convert.ToDecimal(mRow["TotalWHTAmount"]) + Convert.ToDecimal(row["WHTAmount"]);
                        mRow["TransactionCount"] = Convert.ToInt32(mRow["TransactionCount"]) + 1;
                    }
                }

                return summaryDt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
