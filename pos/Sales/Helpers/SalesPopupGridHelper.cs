using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS.BLL;
using pos.UI;

namespace pos.Sales.Helpers
{
    /// <summary>
    /// Helper class for sales popup grid preparation and population.
    /// Extracts repeating lookup-grid logic from frm_sales to improve maintainability.
    /// </summary>
    public static class SalesPopupGridHelper
    {
        public static void SetupLookupGrid(DataGridView grid, string gridName, int width, int height)
        {
            grid.ColumnCount = 2;
            grid.Name = gridName;
            grid.Size = new Size(width, height);
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.Columns[0].Name = "Code";
            grid.Columns[1].Name = "Name";
            grid.Columns[0].ReadOnly = true;
            grid.Columns[1].ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.AutoResizeColumns();
        }

        public static void PopulateBrands(DataGridView grid, string query)
        {
            BrandsBLL brandsBLL_obj = new BrandsBLL();
            DataTable dt = brandsBLL_obj.SearchRecord(query);
            PopulateLookupRows(grid, dt);
        }

        public static void PopulateCategories(DataGridView grid, string query)
        {
            CategoriesBLL categoriesBLL_obj = new CategoriesBLL();
            DataTable dt = categoriesBLL_obj.SearchRecord(query);
            PopulateLookupRows(grid, dt);
        }

        public static void PopulateGroups(DataGridView grid, string query)
        {
            ProductGroupsBLL groupsBLL_obj = new ProductGroupsBLL();
            DataTable dt = groupsBLL_obj.SearchRecordByName(query);
            PopulateLookupRows(grid, dt);
        }

        public static void PopulateCustomers(DataGridView grid, string normalizedSearch)
        {
            var bll = new CustomerBLL();
            DataTable dt = bll.SearchRecord(normalizedSearch) ?? new DataTable();
            PopulateCustomerRows(grid, dt);
        }

        public static void PopulateCustomerRows(DataGridView grid, DataTable dt)
        {
            grid.Rows.Clear();
            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                string[] row0 =
                {
                    dt.Columns.Contains("customer_code") ? dr["customer_code"].ToString() : "",
                    dr["first_name"].ToString() + " " + dr["last_name"].ToString(),
                    dr["id"].ToString(),
                    dr["contact_no"].ToString(),
                    dr["vat_no"].ToString(),
                    dr["credit_limit"].ToString()
                };

                grid.Rows.Add(row0);
            }
        }

        public static void PopulateLookupRows(DataGridView grid, DataTable dt)
        {
            grid.Rows.Clear();
            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                string[] row0 = { dr["code"].ToString(), dr["name"].ToString() };
                grid.Rows.Add(row0);
            }
            grid.ClearSelection();
            grid.CurrentCell = null;
        }

        public static void PositionDropdownGrid(Form form, DataGridView dgv, Control anchor)
        {
            Point pt = form.PointToClient(anchor.Parent.PointToScreen(anchor.Location));
            int x = Math.Max(0, Math.Min(pt.X, form.ClientSize.Width - dgv.Width));
            dgv.Location = new Point(x, pt.Y + anchor.Height + 2);
        }

        public static void PositionCustomersDropdown(Form form, DataGridView dgv, TextBox anchor)
        {
            Point pt = form.PointToClient(anchor.Parent.PointToScreen(anchor.Location));
            int x = Math.Max(0, Math.Min(pt.X, form.ClientSize.Width - dgv.Width));
            dgv.Location = new Point(x, pt.Y + anchor.Height + 2);
        }

        public static void HideAndReturnFocus(DataGridView grid, Control focusControl)
        {
            if (grid != null)
                grid.Visible = false;

            if (focusControl != null)
                focusControl.Focus();
        }

        public static void ApplyGridStyling(DataGridView grid)
        {
            SalesStylingHelper.StyleDropdownGrid(grid);
        }
    }
}
