using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace pos.Reports.Common
{
    public static class ExcelExportHelper
    {
        /// <summary>
        /// Exports DataTable to Excel-compatible file via SaveFileDialog
        /// </summary>
        /// <param name="source">DataTable to export</param>
        /// <param name="defaultFileName">Default filename without extension</param>
        /// <param name="owner">Owner form for dialog</param>
        /// <param name="includeLastRow">Set false to drop trailing "Total" row</param>
        public static void ExportDataTableToExcel(DataTable source, string defaultFileName, IWin32Window owner, bool includeLastRow = true)
        {
            if (source == null || source.Rows.Count == 0)
            {
                MessageBox.Show(owner, "No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Prepare export copy and optionally remove last row (e.g., "Total" row appended for grid)
            var dt = source.Copy();
            if (!includeLastRow && dt.Rows.Count > 0)
            {
                dt.Rows.RemoveAt(dt.Rows.Count - 1);
                dt.AcceptChanges();
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Export to Excel";
                sfd.FileName = string.IsNullOrWhiteSpace(defaultFileName) ? "report" : defaultFileName;
                sfd.Filter = "Excel Workbook (*.xls)|*.xls|CSV (Comma delimited) (*.csv)|*.csv";
                sfd.AddExtension = true;
                sfd.DefaultExt = "xls";
                
                if (sfd.ShowDialog(owner) != DialogResult.OK) return;

                var ext = Path.GetExtension(sfd.FileName)?.ToLowerInvariant();
                try
                {
                    if (ext == ".csv")
                    {
                        File.WriteAllText(sfd.FileName, ToCsv(dt), Encoding.UTF8);
                    }
                    else // default to .xls (HTML table that Excel opens)
                    {
                        File.WriteAllText(sfd.FileName, ToHtmlTable(dt), Encoding.UTF8);
                    }

                    if (MessageBox.Show(owner, "Export completed successfully!\n\nDo you want to open the file now?", 
                        "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(owner, "Export failed: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string ToCsv(DataTable dt)
        {
            var sb = new StringBuilder();

            // Header
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(EscapeCsv(dt.Columns[i].ColumnName));
            }
            sb.AppendLine();

            // Rows
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(EscapeCsv(Convert.ToString(row[i])));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string EscapeCsv(string input)
        {
            if (input == null) return "";
            bool mustQuote = input.Contains(",") || input.Contains("\"") || input.Contains("\r") || input.Contains("\n");
            var output = input.Replace("\"", "\"\"");
            return mustQuote ? $"\"{output}\"" : output;
        }

        private static string ToHtmlTable(DataTable dt)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head><body>");
            sb.AppendLine("<table border='1' cellspacing='0' cellpadding='4' style='border-collapse:collapse;font-family:Segoe UI, Arial; font-size:9pt;'>");

            // Header
            sb.AppendLine("<thead><tr style='background-color:#4CAF50;color:white;font-weight:bold;'>");
            foreach (DataColumn col in dt.Columns)
            {
                sb.Append("<th>").Append(System.Net.WebUtility.HtmlEncode(col.ColumnName)).Append("</th>");
            }
            sb.AppendLine("</tr></thead>");

            // Body
            sb.AppendLine("<tbody>");
            int rowNum = 0;
            foreach (DataRow row in dt.Rows)
            {
                string bgColor = rowNum % 2 == 0 ? "#f9f9f9" : "#ffffff";
                sb.AppendLine($"<tr style='background-color:{bgColor};'>");
                foreach (DataColumn col in dt.Columns)
                {
                    var val = Convert.ToString(row[col]);
                    // Right-align numeric columns
                    string align = IsNumericType(col.DataType) ? " style='text-align:right;'" : "";
                    sb.Append("<td").Append(align).Append(">").Append(System.Net.WebUtility.HtmlEncode(val)).Append("</td>");
                }
                sb.AppendLine("</tr>");
                rowNum++;
            }
            sb.AppendLine("</tbody></table></body></html>");
            return sb.ToString();
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(decimal) ||
                   type == typeof(double) || type == typeof(float) || type == typeof(short) ||
                   type == typeof(byte);
        }
    }
}
