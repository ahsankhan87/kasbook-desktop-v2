using DGVPrinterHelper;
using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.Core;

namespace pos
{
    public partial class frm_productWiseSalesReport : Form
    {
        public int _product_id = 0;
        public string _product_name;
        
        ProductBLL productsBLL_obj = new ProductBLL();
                
        public frm_productWiseSalesReport()
        {
            InitializeComponent();
            
            //get_products_dropdownlist();
            
        }

        private void productWiseSalesReport_Load(object sender, EventArgs e)
        {
            cmb_sale_type.SelectedIndex = 0;
            cmb_sale_account.SelectedIndex = 0;
            get_employees_dropdownlist();
            get_customers_dropdownlist();
        }

        public void load_products()
        {
            //txt_product_code.Text = _product_name;
        }

        
        public void get_customers_dropdownlist()
        {
            CustomerBLL customerBLL = new CustomerBLL();
            DataTable customers = customerBLL.GetAll();

            DataRow emptyRow = customers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "Select Customer";              // Set Column Value
            customers.Rows.InsertAt(emptyRow, 0);
            
            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = customers;

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                string from_date = txt_from_date.Value.Date.ToString("yyyy-MM-dd");
                string to_date = txt_to_date.Value.Date.ToString("yyyy-MM-dd"); ;
                int customer_id = Convert.ToInt16(cmb_customers.SelectedValue);
                string product_code = ""; //Convert.ToInt16(cmb_products.SelectedValue);
                string sale_type = cmb_sale_type.SelectedItem.ToString();
                int employee_id = Convert.ToInt16(cmb_employees.SelectedValue);
                string sale_account = cmb_sale_account.SelectedItem.ToString();
                int branch_id = UsersModal.logged_in_branch_id;

                SalesReportBLL sale_report_obj = new SalesReportBLL();
                grid_sales_report.AutoGenerateColumns = false;

                DataTable accounts_dt = new DataTable();
                accounts_dt = sale_report_obj.ProductWiseSaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account,branch_id);

                //double _quantity_sold_total = 0;
                //double _unit_price_total = 0;
                //double _discount_value_total = 0;
               // double _vat_total = 0;
                double _total = 0;

                foreach (DataRow dr in accounts_dt.Rows)
                {
                    //_quantity_sold_total += Convert.ToDouble(dr["quantity_sold"].ToString());
                    //_unit_price_total += Convert.ToDouble(dr["unit_price"].ToString());
                    //_discount_value_total += Convert.ToDouble(dr["discount_value"].ToString());
                   // _vat_total += Convert.ToDouble(dr["vat"].ToString());
                    _total += Convert.ToDouble(dr["qty"].ToString());
                }

                DataRow newRow = accounts_dt.NewRow();
                newRow[1] = "Total";
                //newRow[4] = _quantity_sold_total;
                //newRow[5] = _unit_price_total;
                //newRow[6] = _discount_value_total;
                //newRow[9] = _vat_total;
                newRow[0] =  _total;
                accounts_dt.Rows.InsertAt(newRow, accounts_dt.Rows.Count);

                grid_sales_report.DataSource = accounts_dt;
                CustomizeDataGridView();
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
        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

        public void get_employees_dropdownlist()
        {
            EmployeeBLL employeeBLL = new EmployeeBLL();
            DataTable employees = employeeBLL.GetAll();

            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "Select Employee";              // Set Column Value
            employees.Rows.InsertAt(emptyRow, 0);
            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;


        }
        private void frm_productWiseSalesReport_KeyDown(object sender, KeyEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Product Wise Sale Report";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_sales_report);
            //printer.PrintDataGridView(grid_sales_report);
        }

    }
}
