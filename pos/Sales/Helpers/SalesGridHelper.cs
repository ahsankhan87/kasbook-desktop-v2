using System;
using System.Data;
using System.Windows.Forms;
using POS.Core;

namespace pos.Sales.Helpers
{
    /// <summary>
    /// Helper class for sales grid operations.
    /// Extracts grid manipulation logic from frm_sales to improve maintainability.
    /// </summary>
    public static class SalesGridHelper
    {
        private static readonly HashSet<string> _numericColumns =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Qty", "unit_price", "discount", "discount_percent", "total_without_vat" };

        /// <summary>
        /// Loads a product into the sales grid.
        /// </summary>
        public static int LoadProductToGrid(DataGridView grid_sales, DataRow myProductView, string invoice_no = "")
        {
            bool found = false;
            int rowIndex = -1;

            // Check if product already exists in grid
            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                if (grid_sales.Rows[i].Cells["code"].Value != null && 
                    grid_sales.Rows[i].Cells["code"].Value.ToString() == myProductView["item_number"].ToString())
                {
                    found = true;
                    rowIndex = i;
                    break;
                }
            }

            if (found)
            {
                // Update existing row
                double currentQty = Convert.ToDouble(grid_sales.Rows[rowIndex].Cells["qty"].Value);
                grid_sales.Rows[rowIndex].Cells["qty"].Value = currentQty + 1;

                // Recalculate row totals
                double unitPrice = Convert.ToDouble(grid_sales.Rows[rowIndex].Cells["unit_price"].Value);
                double discountPercent = Convert.ToDouble(grid_sales.Rows[rowIndex].Cells["discount_percent"].Value);
                double taxRate = Convert.ToDouble(grid_sales.Rows[rowIndex].Cells["tax_rate"].Value);

                double total_value = unitPrice * Convert.ToDouble(grid_sales.Rows[rowIndex].Cells["qty"].Value);
                double discount = total_value * discountPercent / 100;
                double tax_1 = ((total_value - discount) * taxRate / 100);
                double sub_total_1 = tax_1 + total_value - discount;

                grid_sales.Rows[rowIndex].Cells["sub_total"].Value = sub_total_1;
                grid_sales.Rows[rowIndex].Cells["tax"].Value = tax_1;
                grid_sales.Rows[rowIndex].Cells["discount"].Value = discount;
            }
            else
            {
                // Add new row
                object[] row = new object[grid_sales.Columns.Count];
                
                row[grid_sales.Columns["sno"].Index] = grid_sales.Rows.Count + 1;
                row[grid_sales.Columns["id"].Index] = myProductView["id"];
                row[grid_sales.Columns["code"].Index] = myProductView["item_number"];
                row[grid_sales.Columns["name"].Index] = myProductView["name"];
                row[grid_sales.Columns["qty"].Index] = 1;
                row[grid_sales.Columns["unit"].Index] = myProductView["unit_name"];
                row[grid_sales.Columns["unit_price"].Index] = myProductView["selling_price"];
                row[grid_sales.Columns["cost_price"].Index] = myProductView["cost_price"];
                row[grid_sales.Columns["discount_percent"].Index] = 0;
                row[grid_sales.Columns["discount"].Index] = 0;
                row[grid_sales.Columns["tax_rate"].Index] = myProductView["tax_rate"];
                row[grid_sales.Columns["tax"].Index] = 0;
                row[grid_sales.Columns["sub_total"].Index] = 0;
                row[grid_sales.Columns["total_without_vat"].Index] = 0;
                row[grid_sales.Columns["invoice_no"].Index] = invoice_no;

                rowIndex = grid_sales.Rows.Add(row);

                // Check stock availability
                if (Convert.ToDouble(myProductView["qty"]) <= 0 || myProductView["qty"].ToString() == string.Empty)
                {
                    grid_sales.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
                }
            }

            return rowIndex;
        }

        /// <summary>
        /// Configures numeric columns in the sales grid for proper alignment and formatting.
        /// </summary>
        public static void ConfigureNumericColumns(DataGridView grid_sales)
        {
            foreach (DataGridViewColumn column in grid_sales.Columns)
            {
                if (_numericColumns.Contains(column.Name))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.Format = "N2";
                }
            }
        }

        /// <summary>
        /// Updates serial numbers in the sales grid.
        /// </summary>
        public static void UpdateSerialNumbers(DataGridView grid_sales)
        {
            for (int i = 0; i < grid_sales.Rows.Count; i++)
            {
                if (grid_sales.Columns["sno"] != null)
                {
                    grid_sales.Rows[i].Cells["sno"].Value = i + 1;
                }
            }
        }

        /// <summary>
        /// Removes empty rows from the end of the sales grid.
        /// </summary>
        public static void RemoveEmptyTrailingRows(DataGridView grid_sales)
        {
            while (grid_sales.Rows.Count > 1)
            {
                var lastRow = grid_sales.Rows[grid_sales.Rows.Count - 1];
                if (lastRow.Cells["code"].Value == null || 
                    string.IsNullOrWhiteSpace(lastRow.Cells["code"].Value.ToString()))
                {
                    grid_sales.Rows.Remove(lastRow);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Ensures there's an empty row at the end of the grid for data entry.
        /// </summary>
        public static void EnsureEmptyRowForEntry(DataGridView grid_sales)
        {
            if (grid_sales.Rows.Count == 0 || 
                (grid_sales.Rows[grid_sales.Rows.Count - 1].Cells["code"].Value != null && 
                 !string.IsNullOrWhiteSpace(grid_sales.Rows[grid_sales.Rows.Count - 1].Cells["code"].Value.ToString())))
            {
                grid_sales.Rows.Add();
            }
        }

        /// <summary>
        /// Clears all rows from the sales grid.
        /// </summary>
        public static void ClearGrid(DataGridView grid_sales)
        {
            grid_sales.Rows.Clear();
            grid_sales.Rows.Add();
        }

        /// <summary>
        /// Gets the current product ID from the selected row in the grid.
        /// </summary>
        public static int GetCurrentProductId(DataGridView grid_sales)
        {
            if (grid_sales.CurrentRow != null && 
                grid_sales.CurrentRow.Cells["id"].Value != null && 
                grid_sales.CurrentRow.Cells["id"].Value != DBNull.Value)
            {
                return Convert.ToInt32(grid_sales.CurrentRow.Cells["id"].Value);
            }
            return 0;
        }

        /// <summary>
        /// Validates that all required cells in a row have values.
        /// </summary>
        public static bool ValidateRow(DataGridViewRow row, params string[] requiredColumns)
        {
            foreach (string columnName in requiredColumns)
            {
                if (row.Cells[columnName].Value == null || 
                    string.IsNullOrWhiteSpace(row.Cells[columnName].Value.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
