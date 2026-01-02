using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_product_adjustment : Form
    {
        private PrintDocument _printDoc;
        private PrintPreviewDialog _printPreview;
        private int _printRowIndex;

        private readonly Font _fontHeader = new Font("Segoe UI", 11F, FontStyle.Bold);
        private readonly Font _fontRegular = new Font("Segoe UI", 9F, FontStyle.Regular);
        private readonly Font _fontSmall = new Font("Segoe UI", 8F, FontStyle.Regular);

        private void EnsurePrinterInitialized()
        {
            if (_printDoc != null) return;

            _printDoc = new PrintDocument();
            _printDoc.PrintPage += PrintDoc_PrintPage;
            _printDoc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            _printPreview = new PrintPreviewDialog
            {
                Document = _printDoc,
                Width = 1000,
                Height = 800,
                StartPosition = FormStartPosition.CenterParent
            };
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintAdjustment();
        }

        private void PrintAdjustment()
        {
            if (grid_search_products.Rows.Count == 0)
            {
                MessageBox.Show("No adjustment rows to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            EnsurePrinterInitialized();
            _printRowIndex = 0;
            _printPreview.ShowDialog(this);
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var margin = e.MarginBounds;
            int x = margin.Left;
            int y = margin.Top;

            // Improve compatibility with PDF/printer drivers
            g.PageUnit = GraphicsUnit.Display;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            // Header
            g.DrawString("NOZUM Technologies", _fontHeader, Brushes.Black, x, y);
            g.DrawString("Product Adjustment", _fontHeader, Brushes.Black, margin.Right - 220, y);
            y += (int)_fontHeader.GetHeight(g) + 6;

            g.DrawString("Invoice: " + (txt_ref_no.Text ?? string.Empty), _fontRegular, Brushes.Black, x, y);
            g.DrawString("Date: " + txt_date.Value.ToString("yyyy-MM-dd"), _fontRegular, Brushes.Black, margin.Right - 200, y);
            y += (int)_fontRegular.GetHeight(g) + 8;

            // Column layout
            int colCode = x;
            int wCode = 80;
            int colName = colCode + wCode + 10;
            int wName = Math.Max(240, margin.Width - 560);
            int colLoc = colName + wName + 10;
            int wLoc = 60;
            int colQty = colLoc + wLoc + 10;
            int wQty = 60;
            int colAdj = colQty + wQty + 10;
            int wAdj = 60;
            int colCost = colAdj + wAdj + 10;
            int wCost = 80;
            int colValue = colCost + wCost + 10;

            // Column headers
            g.DrawString("Code", _fontRegular, Brushes.Black, colCode, y);
            g.DrawString("Name", _fontRegular, Brushes.Black, colName, y);
            g.DrawString("Loc", _fontRegular, Brushes.Black, colLoc, y);
            g.DrawString("Qty", _fontRegular, Brushes.Black, colQty, y);
            g.DrawString("Adj", _fontRegular, Brushes.Black, colAdj, y);
            g.DrawString("Cost", _fontRegular, Brushes.Black, colCost, y);
            g.DrawString("Value", _fontRegular, Brushes.Black, colValue, y);
            y += (int)_fontRegular.GetHeight(g) + 6;

            g.DrawLine(Pens.Black, x, y, margin.Right, y);
            y += 6;

            // Resolve column indices safely.
            // Fallback indices are based on your row0 order in Load_product_to_grid:
            // 0:id,1:code,2:category,3:name,4:name_ar,5:location_code,6:qty,7:adjustment_qty,8:avg_cost,9:unit_price,...
            int idxCode = GetColIndex(grid_search_products, "code", 1);
            int idxName = GetColIndex(grid_search_products, "name", 3);
            int idxLoc = GetColIndex(grid_search_products, "location_code", 5);
            int idxQty = GetColIndex(grid_search_products, "qty", 6);
            int idxAdj = GetColIndex(grid_search_products, "adjustment_qty", 7);
            int idxCost = GetColIndex(grid_search_products, "avg_cost", 8);

            int rowHeight = (int)_fontSmall.GetHeight(g) + 6;
            int rowBitmapWidth = margin.Width;
            int rowBitmapHeight = Math.Max(1, rowHeight);

            while (_printRowIndex < grid_search_products.Rows.Count)
            {
                var row = grid_search_products.Rows[_printRowIndex];

                // Skip the DataGridView "new row" placeholder.
                if (row.IsNewRow)
                {
                    _printRowIndex++;
                    continue;
                }

                string code = GetCellString(row, idxCode);
                string name = GetCellString(row, idxName);
                string loc = GetCellString(row, idxLoc);

                decimal qty = GetCellDecimal(row, idxQty);
                decimal adj = GetCellDecimal(row, idxAdj);
                decimal cost = GetCellDecimal(row, idxCost);
                decimal value = Math.Round((adj - qty) * cost, 2);

                // Skip completely empty rows
                if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name) && qty == 0m && adj == 0m)
                {
                    _printRowIndex++;
                    continue;
                }

                // Render the detail row into a bitmap then draw it to printer graphics (PDF-safe)
                using (var bmp = new Bitmap(rowBitmapWidth, rowBitmapHeight))
                using (var gg = Graphics.FromImage(bmp))
                {
                    gg.Clear(Color.White);
                    gg.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                    gg.DrawString(code, _fontSmall, Brushes.Black, colCode - x, 0);
                    gg.DrawString(TruncateString(name, wName, gg, _fontSmall), _fontSmall, Brushes.Black, colName - x, 0);
                    gg.DrawString(loc, _fontSmall, Brushes.Black, colLoc - x, 0);
                    gg.DrawString(qty.ToString("N2"), _fontSmall, Brushes.Black, colQty - x, 0);
                    gg.DrawString(adj.ToString("N2"), _fontSmall, Brushes.Black, colAdj - x, 0);
                    gg.DrawString(cost.ToString("N2"), _fontSmall, Brushes.Black, colCost - x, 0);
                    gg.DrawString(value.ToString("N2"), _fontSmall, Brushes.Black, colValue - x, 0);

                    g.DrawImageUnscaled(bmp, x, y);
                }

                y += rowHeight;
                _printRowIndex++;

                if (y + 80 > margin.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // Totals (use same resolved indices)
            decimal totalValue = 0m;
            for (int i = 0; i < grid_search_products.Rows.Count; i++)
            {
                var r = grid_search_products.Rows[i];
                if (r.IsNewRow) continue;

                decimal q = GetCellDecimal(r, idxQty);
                decimal a = GetCellDecimal(r, idxAdj);
                decimal c = GetCellDecimal(r, idxCost);
                totalValue += Math.Round((a - q) * c, 2);
            }

            y += 8;
            g.DrawLine(Pens.Black, x, y, margin.Right, y);
            y += 6;
            g.DrawString("Total Adjustment Value: " + totalValue.ToString("N2"), _fontRegular, Brushes.Black, margin.Right - 270, y);

            e.HasMorePages = false;
        }

        private static int GetColIndex(DataGridView grid, string name, int fallbackIndex)
        {
            return grid.Columns.Contains(name) ? grid.Columns[name].Index : fallbackIndex;
        }

        private static string GetCellString(DataGridViewRow row, int index)
        {
            if (index < 0 || index >= row.Cells.Count) return string.Empty;
            return Convert.ToString(row.Cells[index].Value);
        }

        private static decimal GetCellDecimal(DataGridViewRow row, int index)
        {
            if (index < 0 || index >= row.Cells.Count) return 0m;

            decimal d;
            return decimal.TryParse(Convert.ToString(row.Cells[index].Value), out d) ? d : 0m;
        }

        private static string TruncateString(string text, int maxWidth, Graphics g, Font f)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var rendered = g.MeasureString(text, f);
            if (rendered.Width <= maxWidth) return text;

            const string ellipsis = "...";
            for (int len = text.Length - 1; len > 0; len--)
            {
                var t = text.Substring(0, len) + ellipsis;
                if (g.MeasureString(t, f).Width <= maxWidth) return t;
            }
            return text;
        }
    }
}
