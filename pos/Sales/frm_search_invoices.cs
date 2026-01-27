using pos.UI.Busy;
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

namespace pos
{
    public partial class frm_search_invoices : Form
    {
        frm_sales salesForm;
        frm_sales_v1 salesForm_v1;

        public frm_search_invoices(frm_sales salesForm,frm_sales_v1 salesForm_v1=null)
        {
            this.salesForm = salesForm;
            this.salesForm_v1 = salesForm_v1;
            InitializeComponent();
        }
        
        public frm_search_invoices()
        {
            InitializeComponent();
            
        }

        private void search_invoices_Load(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 2;
            txt_condition.Focus();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, "Searching..."))
            {
                try
                {

                    string from_date = "";
                    string to_date = "";
                    string customer = "";
                    string invoice_no = "";
                    double total_amount = 0;

                    if (Listbox_method.SelectedIndex == 0)
                    {
                        invoice_no = txt_condition.Text;
                    }

                    if (Listbox_method.SelectedIndex == 1)
                    {
                        customer = txt_condition.Text;
                    } 
                    
                    if (Listbox_method.SelectedIndex == 2)
                    {
                        from_date = txt_from_date.Value.Date.ToString("yyyy-MM-dd");
                        to_date = txt_to_date.Value.Date.ToString("yyyy-MM-dd"); ;
                    
                    }

                    if (Listbox_method.SelectedIndex == 3)
                    {
                        total_amount = (string.IsNullOrEmpty(txt_condition.Text) ? 0 : double.Parse(txt_condition.Text));
                    }
                                   
                    grid_sales_report.AutoGenerateColumns = false;

                    if (!string.IsNullOrEmpty(invoice_no) || !string.IsNullOrEmpty(customer) 
                        || total_amount != 0 || !string.IsNullOrEmpty(from_date) || !string.IsNullOrEmpty(to_date))
                    {
                        SalesReportBLL sale_report_obj = new SalesReportBLL();
                        DataTable accounts_dt = new DataTable();
                        accounts_dt = sale_report_obj.InvoiceReport(from_date, to_date, customer, invoice_no, total_amount);

                        grid_sales_report.DataSource = accounts_dt;
                    }

                    this.ActiveControl = grid_sales_report;
                    grid_sales_report.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    
                }
            }
        }


        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

        private void frm_search_invoices_KeyDown(object sender, KeyEventArgs e)
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
                    btn_ok.PerformClick();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void grid_sales_report_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grid_sales_report.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString()))
                    {
                        var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
                        SalesBLL salesObj = new SalesBLL();
                        DataTable _dt = new DataTable();

                        _dt = salesObj.GetSaleAndSalesItems(invoice_no);
                        load_sales_grid_products(_dt, invoice_no);
                        this.Close();
                    }
                }
                
            }
            
        }

        private void grid_sales_report_DoubleClick(object sender, EventArgs e)
        {
            var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
            SalesBLL salesObj = new SalesBLL();
            DataTable _dt = new DataTable();

            _dt = salesObj.GetSaleAndSalesItems(invoice_no);
            load_sales_grid_products(_dt, invoice_no);
            this.Close();
        }

        private void txt_from_date_ValueChanged(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 2;
        }

        private void txt_to_date_ValueChanged(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 2;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(grid_sales_report.Rows.Count > 0)
            {
                var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
                SalesBLL salesObj = new SalesBLL();
                DataTable _dt = new DataTable();

                _dt = salesObj.GetSaleAndSalesItems(invoice_no);
                load_sales_grid_products(_dt, invoice_no);
                this.Close();
            }
           
        }

        private void load_sales_grid_products(DataTable dt,string invoice_no)
        {

            if (salesForm != null)
            {
                salesForm.Load_products_to_grid_by_invoiceno(dt, invoice_no);
            }
            else if (salesForm_v1 != null)
            {
                salesForm_v1.Load_products_to_grid_by_invoiceno(dt, invoice_no);
            }
                
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString());
        }

    }
}
