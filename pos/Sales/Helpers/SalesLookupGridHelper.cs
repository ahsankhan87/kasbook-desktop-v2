using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS.BLL;

namespace pos.Sales.Helpers
{
    /// <summary>
    /// Helper class for sales lookup dropdown grids.
    /// Extracts DataGridView setup and binding logic from frm_sales to improve maintainability.
    /// </summary>
    public static class SalesLookupGridHelper
    {
        public static DataGridView CreateSimpleLookupGrid(Form host, Control anchor, int width, int height, KeyEventHandler keyDownHandler, DataGridViewCellEventHandler cellClickHandler)
        {
            var grid = new DataGridView();
            grid.ColumnCount = 2;
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
            grid.CellClick += cellClickHandler;
            grid.KeyDown += keyDownHandler;

            host.Controls.Add(grid);
            PositionDropdownGrid(host, grid, anchor);
            grid.BringToFront();
            SalesStylingHelper.StyleDropdownGrid(grid);
            return grid;
        }

        public static DataGridView CreateCustomersLookupGrid(Form host, TextBox anchor, KeyEventHandler keyDownHandler, DataGridViewCellEventHandler cellClickHandler)
        {
            var grid = new DataGridView();
            grid.ColumnCount = 6;
            grid.Size = new Size(520, 240);
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.AutoGenerateColumns = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            grid.Columns[0].Name = "Code";
            grid.Columns[1].Name = "Name";
            grid.Columns[2].Name = "ID";
            grid.Columns[3].Name = "Contact";
            grid.Columns[4].Name = "VAT No";
            grid.Columns[5].Name = "Credit Limit";

            grid.Columns[0].ReadOnly = true;
            grid.Columns[1].ReadOnly = true;
            grid.Columns[2].Visible = false;
            grid.Columns[3].ReadOnly = true;
            grid.Columns[4].ReadOnly = true;
            grid.Columns[5].Visible = false;

            grid.Columns[0].Width = 90;
            grid.Columns[1].Width = 220;
            grid.Columns[3].Width = 130;
            grid.Columns[4].Width = 120;

            grid.CellClick += cellClickHandler;
            grid.KeyDown += keyDownHandler;
            grid.Visible = false;

            host.Controls.Add(grid);
            grid.BringToFront();
            SalesStylingHelper.StyleDropdownGrid(grid);
            return grid;
        }

        public static void PositionDropdownGrid(Form host, DataGridView grid, Control anchor)
        {
            Point pt = host.PointToClient(anchor.Parent.PointToScreen(anchor.Location));
            int x = Math.Max(0, Math.Min(pt.X, host.ClientSize.Width - grid.Width));
            grid.Location = new Point(x, pt.Y + anchor.Height + 2);
        }

        public static void BindSimpleLookupRows(DataGridView grid, DataTable dt, string codeColumn, string nameColumn)
        {
            grid.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                grid.Rows.Add(Convert.ToString(dr[codeColumn]), Convert.ToString(dr[nameColumn]));
            }

            grid.ClearSelection();
            grid.CurrentCell = null;
        }

        public static void BindCustomerLookupRows(DataGridView grid, DataTable dt)
        {
            grid.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                grid.Rows.Add(
                    dt.Columns.Contains("customer_code") ? Convert.ToString(dr["customer_code"]) : string.Empty,
                    Convert.ToString(dr["first_name"]) + " " + Convert.ToString(dr["last_name"]),
                    Convert.ToString(dr["id"]),
                    Convert.ToString(dr["contact_no"]),
                    Convert.ToString(dr["vat_no"]),
                    Convert.ToString(dr["credit_limit"]));
            }

            grid.ClearSelection();
            grid.CurrentCell = null;
        }

        public static DataTable SearchBrands(string keyword)
        {
            return new BrandsBLL().SearchRecord(keyword);
        }

        public static DataTable SearchCategories(string keyword)
        {
            return new CategoriesBLL().SearchRecord(keyword);
        }

        public static DataTable SearchGroups(string keyword)
        {
            return new ProductGroupsBLL().SearchRecordByName(keyword);
        }

        public static DataTable SearchCustomers(string keyword)
        {
            return new CustomerBLL().SearchRecord(keyword) ?? new DataTable();
        }

        public static decimal GetCustomerBalance(int customerId)
        {
            return new CustomerBLL().GetCustomerAccountBalance(customerId);
        }

        public static string NormalizeCustomerCodeInput(string value)
        {
            return new CustomerBLL().NormalizeCustomerCodeInput(value);
        }
    }
}
