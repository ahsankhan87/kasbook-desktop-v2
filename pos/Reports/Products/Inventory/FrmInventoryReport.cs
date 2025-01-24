using CrystalDecisions.CrystalReports.Engine;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Reports.Products.Inventory
{
    public partial class FrmInventoryReport : Form
    {
        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();

        public FrmInventoryReport()
        {
            InitializeComponent();
        }

        private void Btn_generate_Click(object sender, EventArgs e)
        {
            /// Assuming you retrieve logged -in user details
            int branchId = UsersModal.logged_in_branch_id;
            int userId = UsersModal.logged_in_userid;

            string selectedCategory = txt_category_code.Text?.Trim();
            string selectedBrand = txt_brand_code.Text?.Trim();
            string selectedLocation = txtLocation.Text.Trim();

            // Load the report
            LoadStockReport(branchId, userId, selectedCategory, selectedBrand, selectedLocation);
        }


        public void LoadStockReport(int branchId, int userId, string category = null, string brand = null, string location = null)
        {
            // Create a new dataset
            //_ = new DataSet();
            WarehouseReportBLL sale_report_obj = new WarehouseReportBLL();
            DataSet ds = sale_report_obj.InventoryReport(branchId, userId, category, brand, location);

            // Load Crystal Report
            // Create an instance of your report
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(appPath + @"\\reports\\Accounts\\Inventory\\InventoryReportQOH.rpt");

            reportDoc.SetDataSource(ds.Tables["StockReport"]);

            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";

            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();

            }

            // Set the parameter value
            reportDoc.SetParameterValue("CompanyName", company_name);

            // Set report to viewer
            crystalReportViewer1.ReportSource = reportDoc;
            crystalReportViewer1.Refresh();
        }

        private void FrmInventoryReport_1_Load(object sender, EventArgs e)
        {

        }

        private void SetupCategoriesDataGridView()
        {
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            categoriesDataGridView.ColumnCount = 2;
            int xLocation = txt_categories.Location.X;
            int yLocation = txt_categories.Location.Y;

            categoriesDataGridView.Name = "categoriesDataGridView";
            if (current_lang_code == "en-US")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);

            }
            else if (current_lang_code == "ar-SA")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);
            }

            categoriesDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            categoriesDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            categoriesDataGridView.Columns[0].Name = "Code";
            categoriesDataGridView.Columns[1].Name = "Name";
            categoriesDataGridView.Columns[0].ReadOnly = true;
            categoriesDataGridView.Columns[1].ReadOnly = true;
            categoriesDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            categoriesDataGridView.MultiSelect = false;
            categoriesDataGridView.AllowUserToAddRows = false;
            categoriesDataGridView.AllowUserToDeleteRows = false;

            categoriesDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            categoriesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            categoriesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            categoriesDataGridView.AutoResizeColumns();

            this.categoriesDataGridView.CellClick += new DataGridViewCellEventHandler(categoriesDataGridView_CellClick);
            this.categoriesDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(categoriesDataGridView_KeyDown);

            this.Controls.Add(categoriesDataGridView);
            categoriesDataGridView.BringToFront();

        }

        void categoriesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(categoriesDataGridView);
                //grid_sales.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            //grid_sales.Focus();

        }

        private void txt_categories_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_categories.Text != "")
                {
                    SetupCategoriesDataGridView();

                    CategoriesBLL categoriesBLL = new CategoriesBLL();
                    string category_name = txt_categories.Text;

                    DataTable dt = categoriesBLL.SearchRecord(category_name);

                    if (dt.Rows.Count > 0)
                    {
                        categoriesDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            categoriesDataGridView.Rows.Add(row0);
                        }
                        //categoriesDataGridView.CurrentCell = categoriesDataGridView.Rows[0].Cells[0];
                        categoriesDataGridView.ClearSelection();
                        categoriesDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_category_code.Text = "";
                    this.Controls.Remove(categoriesDataGridView);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_categories_Leave(object sender, EventArgs e)
        {
            if (!categoriesDataGridView.Focused)
            {
                this.Controls.Remove(categoriesDataGridView);
            }

        }
        private void SetupBrandDataGridView()
        {
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            brandsDataGridView.ColumnCount = 2;
            int xLocation = txt_brands.Location.X;
            int yLocation = txt_brands.Location.Y;

            brandsDataGridView.Name = "brandsDataGridView";
            if (current_lang_code == "en-US")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }
            else if (current_lang_code == "ar-SA")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }

            brandsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            brandsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            brandsDataGridView.Columns[0].Name = "Code";
            brandsDataGridView.Columns[1].Name = "Name";
            brandsDataGridView.Columns[0].ReadOnly = true;
            brandsDataGridView.Columns[1].ReadOnly = true;
            brandsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            brandsDataGridView.MultiSelect = false;
            brandsDataGridView.AllowUserToAddRows = false;
            brandsDataGridView.AllowUserToDeleteRows = false;

            brandsDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            brandsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            brandsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            brandsDataGridView.AutoResizeColumns();

            brandsDataGridView.CellClick += new DataGridViewCellEventHandler(brandsDataGridView_CellClick);
            this.brandsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(brandsDataGridView_KeyDown);

            this.Controls.Add(brandsDataGridView);
            brandsDataGridView.BringToFront();

        }

        void brandsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(brandsDataGridView);
                //grid_sales.Focus();
               
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            //grid_sales.Focus();
            
        }

        private void txt_brands_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_brands.Text != "")
                {
                    SetupBrandDataGridView();

                    BrandsBLL brandsBLL_obj = new BrandsBLL();
                    string brand_name = txt_brands.Text;

                    DataTable dt = brandsBLL_obj.SearchRecord(brand_name);

                    if (dt.Rows.Count > 0)
                    {
                        brandsDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            brandsDataGridView.Rows.Add(row0);
                        }
                        //brandsDataGridView.CurrentCell = brandsDataGridView.Rows[0].Cells[0];
                        brandsDataGridView.ClearSelection();
                        brandsDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_brand_code.Text = "";
                    this.Controls.Remove(brandsDataGridView);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_brands_Leave(object sender, EventArgs e)
        {
            if (!brandsDataGridView.Focused)
            {
                this.Controls.Remove(brandsDataGridView);
            }
        }

        private void FrmInventoryReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                Btn_generate.PerformClick();
            }
            if (e.KeyData == Keys.Down)
            {
                brandsDataGridView.Focus();
                categoriesDataGridView.Focus();
                //groupsDataGridView.Focus();
            }
        }
    }
}
