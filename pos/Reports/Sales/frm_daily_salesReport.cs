using DGVPrinterHelper;
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
using pos.UI;

namespace pos
{
    public partial class frm_daily_salesReport : Form
    {
        public int _product_id = 0;
        public string _product_name;
        
        ProductBLL productsBLL_obj = new ProductBLL();
                
        public frm_daily_salesReport()
        {
            InitializeComponent();
            
        }

        private void daily_salesReport_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            label2.Text = DateTime.Now.Date.ToShortDateString();
            Load_sales_report();
            CustomizeDataGridView();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_sales_report, id);
        }

        private void Load_sales_report()
        {
            try
            {

                DateTime from_date = DateTime.Now.Date;
                DateTime to_date = DateTime.Now.Date;
                int customer_id = 0;
                string product_code = ""; // _product_id; //Convert.ToInt16(cmb_products.SelectedValue);
                string sale_type = "Cash"; // "All"; // cmb_sale_type.SelectedItem.ToString();
                int employee_id = 0; // Convert.ToInt16(cmb_employees.SelectedValue);
                string sale_account = "All"; // cmb_sale_account.SelectedItem.ToString();
                int branch_id = UsersModal.logged_in_branch_id;

                SalesReportBLL sale_report_obj = new SalesReportBLL();
                grid_sales_report.AutoGenerateColumns = false;

                DataTable accounts_dt = new DataTable();
                accounts_dt = sale_report_obj.SaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);

                double _quantity_sold_total = 0;
                double _unit_price_total = 0;
                double _discount_value_total = 0;
                double _vat_total = 0;
                double _total = 0;

                foreach (DataRow dr in accounts_dt.Rows)
                {
                    _quantity_sold_total += (dr["quantity_sold"].ToString() != "" ? Convert.ToDouble(dr["quantity_sold"].ToString()) :0);
                    _unit_price_total += (dr["unit_price"].ToString() != "" ? Convert.ToDouble(dr["unit_price"].ToString()) : 0);
                    _discount_value_total += (dr["discount_value"].ToString() != "" ? Convert.ToDouble(dr["discount_value"].ToString()):0);
                    _vat_total += (dr["vat"].ToString() != "" ? Convert.ToDouble(dr["vat"].ToString()) : 0);
                    _total += (dr["total"].ToString() != "" ? Convert.ToDouble(dr["total"].ToString()) : 0);
                }

                DataRow newRow = accounts_dt.NewRow();
                newRow[10] = "Total";
                newRow[4] = _quantity_sold_total;
                newRow[5] = _unit_price_total;
                newRow[6] = _discount_value_total;
                newRow[9] = _vat_total;
                newRow[7] = _total;
                accounts_dt.Rows.InsertAt(newRow, accounts_dt.Rows.Count);

                grid_sales_report.DataSource = accounts_dt;
               

                
                _product_id = 0;
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


        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Daily Sale Cash Report";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageSettings.Landscape = true;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "khybersoft.com";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_sales_report);
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            Load_sales_report();
            CustomizeDataGridView();
        }

        private void frm_daily_salesReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyCode == Keys.F5)
                {
                    btn_refresh.PerformClick();
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

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString());

        }

        private void grid_sales_report_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sale_id = grid_sales_report.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
            load_sales_items_detail(Convert.ToInt16(sale_id), invoice_no);
        }


        private void load_sales_items_detail(int sale_id, string invoice_no)
        {
            frm_sales_detail frm_sales_detail_obj = new frm_sales_detail(sale_id, invoice_no);
            //frm_sales_detail_obj.load_sales_detail_grid();
            frm_sales_detail_obj.ShowDialog();
        }
    }
}
