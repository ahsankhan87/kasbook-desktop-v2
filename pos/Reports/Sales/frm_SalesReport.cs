using pos.Reports.Common; // added for Excel export
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_SalesReport : Form
    {
        public string _product_code = "";
        public string _product_name;
        DataTable sales_report_dt = new DataTable();
                
        ProductBLL productsBLL_obj = new ProductBLL();
                
        public frm_SalesReport()
        {
            InitializeComponent();
            get_customers_dropdownlist();
            //get_products_dropdownlist();
            autoCompleteProductCode();
        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            CmbCondition.Items.AddRange(new string[]
            {
                "Custom", "Today", "Yesterday", "This Week", "Last Week",
                "This Month", "Last Month", "This Quarter", "Last Quarter",
                "This Year", "Last Year", "Year to Date (YTD)", "Last 7 Days",
                "Last 30 Days", "Last 90 Days", "Last 6 Months",
                "Previous Fiscal Year", "Next Fiscal Year"
            });
            CmbCondition.SelectedIndex = 0;
            cmb_sale_type.SelectedIndex = 0;
            cmb_sale_account.SelectedIndex = 0;
            get_employees_dropdownlist();
        }

        public void load_products()
        {
            //txt_product_code.Text = _product_name;
        }

        public void autoCompleteProductCode()
        {
            try
            {
                txt_product_code.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_product_code.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection Products_coll = new AutoCompleteStringCollection();

                DataTable dt = productsBLL_obj.GetAllProductCodes();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Products_coll.Add(dr["code"].ToString());

                    }

                }

                txt_product_code.AutoCompleteCustomSource = Products_coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public void get_employees_dropdownlist()
        {
            EmployeeBLL employeeBLL = new EmployeeBLL();

            DataTable employees = employeeBLL.GetAll();
            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "All Employee";              // Set Column Value
            employees.Rows.InsertAt(emptyRow, 0);

            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;


        }
        public void get_customers_dropdownlist()
        {
            CustomerBLL customerBLL = new CustomerBLL();

            DataTable customers = customerBLL.GetAll();
            DataRow emptyRow = customers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "All Customer";              // Set Column Value
            customers.Rows.InsertAt(emptyRow, 0);
            
            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = customers;

        }

        public void get_products_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_products";

            DataTable products = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = products.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Product";              // Set Column Value
            products.Rows.InsertAt(emptyRow, 0);
            
            //cmb_products.DataSource = products;

            //cmb_products.DisplayMember = "name";
            //cmb_products.ValueMember = "id";
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from_date = txt_from_date.Value.Date;
                DateTime to_date = txt_to_date.Value.Date;
                int customer_id = Convert.ToInt16(cmb_customers.SelectedValue);
                string product_code = _product_code; //Convert.ToInt16(cmb_products.SelectedValue);
                string sale_type = cmb_sale_type.SelectedItem.ToString();
                int employee_id = Convert.ToInt16(cmb_employees.SelectedValue);
                string sale_account = cmb_sale_account.SelectedItem.ToString();
                int branch_id = UsersModal.logged_in_branch_id;

                SalesReportBLL sale_report_obj = new SalesReportBLL ();
                grid_sales_report.AutoGenerateColumns = false;

                sales_report_dt = sale_report_obj.SaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account,branch_id);

                double _quantity_sold_total = 0;
                double _unit_price_total = 0;
                double _discount_value_total = 0;
                double _vat_total = 0;
                double _total = 0;
                double _total_with_vat = 0;

                foreach (DataRow dr in sales_report_dt.Rows)
                {
                    _quantity_sold_total += (dr["quantity_sold"].ToString() == "" ? 0 : Convert.ToDouble(dr["quantity_sold"].ToString()));
                    _unit_price_total += (dr["unit_price"].ToString() == "" ? 0 : Convert.ToDouble(dr["unit_price"].ToString()));
                    _discount_value_total += (dr["discount_value"].ToString() == "" ? 0 : Convert.ToDouble(dr["discount_value"].ToString()));
                    _vat_total += (dr["vat"].ToString() == "" ? 0 : Convert.ToDouble(dr["vat"].ToString()));
                    _total_with_vat += (dr["total_with_vat"].ToString() == "" ? 0 : Convert.ToDouble(dr["total_with_vat"].ToString()));
                    _total += (dr["total"].ToString() == "" ? 0 : Convert.ToDouble(dr["total"].ToString())); ;
                }

                DataRow newRow = sales_report_dt.NewRow();
                newRow[6] = "Total";
                newRow[8] = _quantity_sold_total;
                newRow[9] = _unit_price_total;
                newRow[10] = _discount_value_total;
                newRow[12] = _vat_total;
                newRow[13] = _total_with_vat;
                newRow[14] = _total;
                sales_report_dt.Rows.InsertAt(newRow, sales_report_dt.Rows.Count);

                grid_sales_report.DataSource = sales_report_dt;
                CustomizeDataGridView();

                _product_code = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                
            }
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_sales_report.Rows[grid_sales_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_sales_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }
        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
             if (txt_product_code.Text != string.Empty && e.KeyCode == Keys.Enter)
            {
                DataTable product_dt = new DataTable();
                product_dt = productsBLL_obj.SearchRecordByProductCode(txt_product_code.Text);
                  
                if (product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        txt_product_name.Text = myProductView["name"].ToString();
                        _product_code = myProductView["code"].ToString();
                       
                    }
                }
            }
             else
             {
                 _product_code = "";
                 txt_product_name.Text = "";
             }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

      
        private void frm_SalesReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.KeyCode == Keys.F3)
                {
                    btn_search.PerformClick();
                }
                
                if (e.Control && e.KeyCode == Keys.P)
                {
                    btn_print.PerformClick();
                }

                //Export to Excel button
                if (e.Control && e.KeyCode == Keys.E)
                {
                    btn_export.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                if (sales_report_dt == null || sales_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to print. Please run a search first.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string date_range = txt_from_date.Value.ToString("dd-MM-yyyy") + " To " + txt_to_date.Value.ToString("dd-MM-yyyy");
                string sale_type = cmb_sale_type.Text;
                string employee = cmb_employees.Text;
                string sale_account = cmb_sale_account.Text;

                frm_sales_report obj = new frm_sales_report(sales_report_dt, date_range, sale_type, employee, sale_account, false);
                obj.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                if (sales_report_dt == null || sales_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to export. Please run a search first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Build filename with date range
                string range = $"{txt_from_date.Value:yyyyMMdd}-{txt_to_date.Value:yyyyMMdd}";
                string defaultName = $"SalesReport_{range}";

                // Drop the appended "Total" row to avoid duplication (Excel can sum itself)
                ExcelExportHelper.ExportDataTableToExcel(sales_report_dt, defaultName, this, includeLastRow: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today;

            switch (CmbCondition.SelectedItem.ToString())
            {
                case "Custom":
                    return;

                case "Today":
                    startDate = endDate = today;
                    break;

                case "Yesterday":
                    startDate = endDate = today.AddDays(-1);
                    break;

                case "This Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek);
                    endDate = startDate.AddDays(6);
                    break;

                case "Last Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek - 7);
                    endDate = startDate.AddDays(6);
                    break;

                case "This Month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;

                case "Last Month":
                    startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;

                case "This Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case "Last Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1).AddMonths(-3);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case "Year to Date (YTD)":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = today;
                    break;

                case "Last 7 Days":
                    startDate = today.AddDays(-6);
                    break;

                case "Last 30 Days":
                    startDate = today.AddDays(-29);
                    break;

                case "Last 90 Days":
                    startDate = today.AddDays(-89);
                    break;

                case "Last 6 Months":
                    startDate = today.AddMonths(-6);
                    break;

                case "This Year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;

                case "Last Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;

                case "Previous Fiscal Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;

                case "Next Fiscal Year":
                    startDate = new DateTime(today.Year + 1, 1, 1);
                    endDate = new DateTime(today.Year + 1, 12, 31);
                    break;
            }

            txt_from_date.Value = startDate;
            txt_to_date.Value = endDate;
        }
    }
}
