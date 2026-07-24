using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DGVPrinterHelper;
using pos.Reports.Common;
using pos.UI;

namespace pos.Reports.FixedAssets
{
    public static class FixedAssetReportPrintExportHelper
    {
        public static void ExportExcel(DataTable data, string fileName, IWin32Window owner)
        {
            ExcelExportHelper.ExportDataTableToExcel(data, fileName, owner, true);
        }

        public static void ShowGridPrintPreview(DataGridView grid, string title, string subTitle, bool landscape)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = title;
            printer.SubTitle = subTitle;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageSettings.Landscape = landscape;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.Porportional;
            printer.Footer = "POWERED BY NOZUM ERP";
            printer.FooterSpacing = 12;
            printer.PrintMargins = new Margins(20, 20, 20, 20);
            printer.PrintPreviewDataGridView(grid);
        }

        public static bool ExportPdfFromDataTable(IWin32Window owner, DataTable data, string companyName, string reportTitle, string subTitle, bool landscape, string defaultFileName)
        {
            if (data == null || data.Rows.Count == 0)
            {
                UiMessages.ShowWarning("No data to export.", "لا توجد بيانات للتصدير");
                return false;
            }

            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.Title = "Export PDF";
                save.Filter = "PDF files (*.pdf)|*.pdf";
                save.FileName = (defaultFileName ?? "Report") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
                if (save.ShowDialog(owner) != DialogResult.OK)
                {
                    return false;
                }

                return ExportPdfToPath(data, companyName, reportTitle, subTitle, landscape, save.FileName);
            }
        }

        private static bool ExportPdfToPath(DataTable data, string companyName, string reportTitle, string subTitle, bool landscape, string path)
        {
            try
            {
                PrintDocument document = CreatePrintDocument(data, companyName, reportTitle, subTitle, landscape);
                document.PrinterSettings.PrinterName = "Microsoft Print to PDF";

                if (!document.PrinterSettings.IsValid)
                {
                    UiMessages.ShowWarning("Microsoft Print to PDF printer is not available.", "طابعة Microsoft Print to PDF غير متوفرة.");
                    return false;
                }

                document.PrinterSettings.PrintToFile = true;
                document.PrinterSettings.PrintFileName = path;
                document.PrintController = new StandardPrintController();
                document.Print();

                if (File.Exists(path))
                {
                    UiMessages.ShowInfo("PDF export completed.", "تم تصدير ملف PDF بنجاح.");
                    return true;
                }

                UiMessages.ShowWarning("PDF export failed.", "فشل تصدير ملف PDF.");
                return false;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
                return false;
            }
        }

        public static PrintDocument CreatePrintDocument(DataTable data, string companyName, string reportTitle, string subTitle, bool landscape)
        {
            DataTable dt = data ?? new DataTable();
            int rowIndex = 0;

            PrintDocument document = new PrintDocument();
            document.DocumentName = reportTitle;
            document.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            document.DefaultPageSettings.Landscape = landscape;

            document.PrintPage += (s, e) =>
            {
                int left = e.MarginBounds.Left;
                int top = e.MarginBounds.Top;
                int width = e.MarginBounds.Width;
                int bottom = e.MarginBounds.Bottom;
                int y = top;

                using (Font companyFont = new Font("Segoe UI", 11F, FontStyle.Bold))
                using (Font titleFont = new Font("Segoe UI", 10F, FontStyle.Bold))
                using (Font headerFont = new Font("Segoe UI", 8F, FontStyle.Bold))
                using (Font bodyFont = new Font("Segoe UI", 8F, FontStyle.Regular))
                using (Pen linePen = new Pen(Color.Silver))
                {
                    e.Graphics.DrawString(companyName ?? string.Empty, companyFont, Brushes.Black, left, y);
                    y += 22;
                    e.Graphics.DrawString(reportTitle ?? string.Empty, titleFont, Brushes.Black, left, y);
                    y += 18;
                    if (!string.IsNullOrWhiteSpace(subTitle))
                    {
                        e.Graphics.DrawString(subTitle, bodyFont, Brushes.DimGray, left, y);
                        y += 16;
                    }

                    if (dt.Columns.Count == 0)
                    {
                        e.Graphics.DrawString("No data", bodyFont, Brushes.Black, left, y);
                        e.HasMorePages = false;
                        return;
                    }

                    int[] colWidths = BuildColumnWidths(dt, width);
                    int x = left;
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        Rectangle rect = new Rectangle(x, y, colWidths[c], 18);
                        e.Graphics.FillRectangle(Brushes.Gainsboro, rect);
                        e.Graphics.DrawRectangle(Pens.Gray, rect);
                        e.Graphics.DrawString(dt.Columns[c].ColumnName, headerFont, Brushes.Black, rect, new StringFormat { LineAlignment = StringAlignment.Center });
                        x += colWidths[c];
                    }

                    y += 20;

                    while (rowIndex < dt.Rows.Count)
                    {
                        if (y + 18 > bottom)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        x = left;
                        DataRow row = dt.Rows[rowIndex];
                        for (int c = 0; c < dt.Columns.Count; c++)
                        {
                            Rectangle rect = new Rectangle(x, y, colWidths[c], 18);
                            e.Graphics.DrawRectangle(linePen, rect);
                            string text = Convert.ToString(row[c]);

                            StringFormat format = new StringFormat { LineAlignment = StringAlignment.Center };
                            if (IsNumericType(dt.Columns[c].DataType))
                            {
                                format.Alignment = StringAlignment.Far;
                            }
                            else
                            {
                                format.Alignment = StringAlignment.Near;
                            }

                            e.Graphics.DrawString(text, bodyFont, Brushes.Black, rect, format);
                            x += colWidths[c];
                        }

                        y += 18;
                        rowIndex++;
                    }

                    e.HasMorePages = false;
                }
            };

            return document;
        }

        private static int[] BuildColumnWidths(DataTable dt, int totalWidth)
        {
            int count = dt.Columns.Count;
            int[] widths = new int[count];
            if (count == 0) return widths;

            int minWidth = 70;
            int calculated = totalWidth / count;
            int width = Math.Max(minWidth, calculated);

            for (int i = 0; i < count; i++)
            {
                widths[i] = width;
            }

            int used = widths.Sum();
            if (used > totalWidth)
            {
                int diff = used - totalWidth;
                widths[count - 1] = Math.Max(minWidth, widths[count - 1] - diff);
            }
            else if (used < totalWidth)
            {
                widths[count - 1] += (totalWidth - used);
            }

            return widths;
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                   type == typeof(decimal) || type == typeof(float) || type == typeof(double);
        }
    }
}
