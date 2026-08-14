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
    /// Business Logic Layer for Moving Items Report (high-velocity inventory).
    /// Moving items are those with recent sales activity or high turnover.
    /// </summary>
    public class MovingItemsReportBLL
    {
        public DataTable GetMovingItems(
            int branchId,
            int daysThreshold = 30,
            decimal minQtyOnHand = 0,
            string categoryCode = null,
            string brandCode = null,
            string locationCode = null)
        {
            try
            {
                MovingItemsReportDLL objDLL = new MovingItemsReportDLL();
                return objDLL.GetMovingItems(branchId, daysThreshold, minQtyOnHand, categoryCode, brandCode, locationCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching moving items report: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get summary statistics for moving items.
        /// </summary>
        public DataTable GetMovingItemsSummary(int branchId, int daysThreshold = 30)
        {
            try
            {
                MovingItemsReportDLL objDLL = new MovingItemsReportDLL();
                return objDLL.GetMovingItemsSummary(branchId, daysThreshold);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching moving items summary: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Export moving items to CSV.
        /// </summary>
        public string ExportMovingItemsToCSV(DataTable data, string filePath)
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
                throw new Exception("Error exporting moving items to CSV: " + ex.Message, ex);
            }
        }
    }
}
