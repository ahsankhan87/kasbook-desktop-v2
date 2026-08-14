using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.DLL;
using POS.Core;

namespace POS.BLL
{
    /// <summary>
    /// Business Logic Layer for Non-Moving Items Report (slow-velocity / stagnant inventory).
    /// Non-moving items are those with no recent sales activity or very low turnover.
    /// </summary>
    public class NonMovingItemsReportBLL
    {
        public DataTable GetNonMovingItems(
            int branchId,
            int daysThreshold = 90,
            decimal minQtyOnHand = 1,
            string categoryCode = null,
            string brandCode = null,
            string locationCode = null)
        {
            try
            {
                NonMovingItemsReportDLL objDLL = new NonMovingItemsReportDLL();
                return objDLL.GetNonMovingItems(branchId, daysThreshold, minQtyOnHand, categoryCode, brandCode, locationCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching non-moving items report: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get summary statistics for non-moving items.
        /// </summary>
        public DataTable GetNonMovingItemsSummary(int branchId, int daysThreshold = 90)
        {
            try
            {
                NonMovingItemsReportDLL objDLL = new NonMovingItemsReportDLL();
                return objDLL.GetNonMovingItemsSummary(branchId, daysThreshold);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching non-moving items summary: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Export non-moving items to CSV.
        /// </summary>
        public string ExportNonMovingItemsToCSV(DataTable data, string filePath)
        {
            try
            {
                if (data == null || data.Rows.Count == 0)
                    return "No data to export.";

                using (var writer = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Write header
                    var headers = new List<string>();
                    foreach (DataColumn col in data.Columns)
                        headers.Add($"\"{col.ColumnName}\"");
                    writer.WriteLine(string.Join(",", headers));

                    // Write rows
                    foreach (DataRow row in data.Rows)
                    {
                        var values = new List<string>();
                        foreach (var item in row.ItemArray)
                            values.Add($"\"{(item ?? "").ToString().Replace("\"", "\"\"")}\"");
                        writer.WriteLine(string.Join(",", values));
                    }
                }

                return $"File exported successfully: {filePath}";
            }
            catch (Exception ex)
            {
                throw new Exception("Error exporting non-moving items to CSV: " + ex.Message, ex);
            }
        }
    }
}
