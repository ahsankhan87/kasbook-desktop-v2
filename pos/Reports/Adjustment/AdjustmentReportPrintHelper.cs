using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace pos.Reports.Adjustment
{
    /// <summary>
    /// Shared utility that converts a list of string rows (with column widths) to a
    /// PrintDocument and exports to PDF-compatible HTML for printing.
    /// </summary>
    internal static class AdjustmentReportPrintHelper
    {
        private const int MarginLeft = 50;
        private const int MarginTop = 50;

        public static void PrintOrPreview(
            string reportTitle,
            string[] headers,
            int[] colWidths,
            List<string[]> dataRows,
            string footerLine,
            bool preview,
            Form owner)
        {
            int pageWidth = 0;
            foreach (int w in colWidths)
                pageWidth += w;

            var doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = pageWidth > 700;
            doc.DocumentName = reportTitle;

            int currentRow = 0;
            bool headerPrinted = false;

            Font titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            Font headFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 8F);
            Font footFont = new Font("Segoe UI", 7.5F, FontStyle.Italic);
            Pen linePen = new Pen(Color.Silver, 0.5f);
            Brush headBrush = new SolidBrush(Color.FromArgb(245, 247, 250));
            Brush altBrush = new SolidBrush(Color.FromArgb(252, 252, 252));
            Brush textBrush = Brushes.Black;

            doc.PrintPage += (s, e) =>
            {
                Graphics g = e.Graphics;
                int x = MarginLeft;
                int y = MarginTop;
                int rowH = 18;

                if (!headerPrinted)
                {
                    g.DrawString(reportTitle, titleFont, textBrush, x, y);
                    y += 28;
                    g.DrawString(string.Format("Printed: {0}  |  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), footerLine), footFont, Brushes.Gray, x, y);
                    y += 18;
                    g.DrawLine(linePen, x, y, x + pageWidth, y);
                    y += 4;
                    headerPrinted = true;
                }

                // column headers
                int hx = x;
                g.FillRectangle(headBrush, hx, y, pageWidth, rowH);
                for (int i = 0; i < headers.Length; i++)
                {
                    g.DrawString(headers[i], headFont, textBrush, new RectangleF(hx + 2, y + 2, colWidths[i] - 4, rowH - 2));
                    hx += colWidths[i];
                }
                y += rowH;
                g.DrawLine(linePen, x, y, x + pageWidth, y);

                int printableBottom = (int)e.MarginBounds.Bottom - MarginTop - 20;

                while (currentRow < dataRows.Count && y + rowH < printableBottom)
                {
                    var row = dataRows[currentRow];
                    if (currentRow % 2 == 1)
                        g.FillRectangle(altBrush, x, y, pageWidth, rowH);

                    int cx = x;
                    for (int i = 0; i < Math.Min(row.Length, headers.Length); i++)
                    {
                        g.DrawString(row[i] ?? string.Empty, bodyFont, textBrush, new RectangleF(cx + 2, y + 2, colWidths[i] - 4, rowH - 2));
                        cx += colWidths[i];
                    }
                    g.DrawLine(linePen, x, y + rowH, x + pageWidth, y + rowH);
                    y += rowH;
                    currentRow++;
                }

                e.HasMorePages = currentRow < dataRows.Count;
                if (!e.HasMorePages)
                    headerPrinted = false;
            };

            if (preview)
            {
                var pv = new PrintPreviewDialog
                {
                    Document = doc,
                    WindowState = FormWindowState.Maximized,
                    Text = reportTitle + " — Print Preview"
                };
                pv.ShowDialog(owner);
            }
            else
            {
                doc.Print();
            }

            titleFont.Dispose();
            headFont.Dispose();
            bodyFont.Dispose();
            footFont.Dispose();
            linePen.Dispose();
            headBrush.Dispose();
            altBrush.Dispose();
        }

        public static void ExportToCsv(
            string reportTitle,
            string[] headers,
            List<string[]> dataRows,
            Form owner)
        {
            using (var dlg = new SaveFileDialog
            {
                Title = "Export " + reportTitle,
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = reportTitle.Replace(" ", "_") + "_" + DateTime.Today.ToString("yyyyMMdd") + ".csv"
            })
            {
                if (dlg.ShowDialog(owner) != DialogResult.OK)
                    return;

                var sb = new StringBuilder();
                sb.AppendLine(string.Join(",", QuoteCsv(headers)));
                foreach (var row in dataRows)
                    sb.AppendLine(string.Join(",", QuoteCsv(row)));

                File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show(owner, "Exported to:\n" + dlg.FileName, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static IEnumerable<string> QuoteCsv(string[] cells)
        {
            foreach (string c in cells)
            {
                string s = (c ?? string.Empty).Replace("\"", "\"\"");
                if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                    s = "\"" + s + "\"";
                yield return s;
            }
        }
    }
}
