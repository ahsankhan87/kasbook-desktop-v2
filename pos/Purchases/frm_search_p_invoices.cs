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
    public partial class frm_search_p_invoices : Form
    {
        frm_purchases mainForm;

        public frm_search_p_invoices(frm_purchases mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }
        
        public frm_search_p_invoices()
        {
            InitializeComponent();
            
        }

        private void search_p_invoices_Load(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 3;
            txt_condition.Focus();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                string from_date = "";
                string to_date = "";
                string supplier = "";
                string supplier_inv_no = "";
                string invoice_no = "";
                double total_amount = 0;
                int branch_id = UsersModal.logged_in_branch_id;

                if (Listbox_method.SelectedIndex == 0)
                {
                    invoice_no = txt_condition.Text;
                }

                if (Listbox_method.SelectedIndex == 1)
                {
                    supplier = txt_condition.Text;
                }

                if (Listbox_method.SelectedIndex == 2)
                {
                    supplier_inv_no = txt_condition.Text;
                } 
                
                if (Listbox_method.SelectedIndex == 3)
                {
                    from_date = txt_from_date.Value.Date.ToString("yyyy-MM-dd");
                    to_date = txt_to_date.Value.Date.ToString("yyyy-MM-dd"); ;
                
                }

                if (Listbox_method.SelectedIndex == 4)
                {
                    total_amount = (string.IsNullOrEmpty(txt_condition.Text) ? 0 : double.Parse(txt_condition.Text));
                }
                grid_sales_report.AutoGenerateColumns = false;

                if (!string.IsNullOrEmpty(invoice_no) || !string.IsNullOrEmpty(supplier) || !string.IsNullOrEmpty(supplier_inv_no)
                    || total_amount != 0 || !string.IsNullOrEmpty(from_date) || !string.IsNullOrEmpty(to_date))
                {
                    PurchasesReportBLL report_obj = new PurchasesReportBLL();

                    DataTable accounts_dt = new DataTable();
                    if (chk_hold_purchases.Checked)
                    {
                        accounts_dt = report_obj.Hold_PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
                    }
                    else
                    {
                        accounts_dt = report_obj.PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);

                    }

                    grid_sales_report.DataSource = accounts_dt;
                }
               
                this.ActiveControl = grid_sales_report;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                
            }
        }


        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

        private void frm_search_p_invoices_KeyDown(object sender, KeyEventArgs e)
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
                if(grid_sales_report.RowCount > 0)
                {
                    var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
                    PurchasesBLL PurchasesObj = new PurchasesBLL();
                    DataTable _dt = new DataTable();

                    if (chk_hold_purchases.Checked)
                    {
                        _dt = PurchasesObj.GetAll_Hold_PurchaseByInvoice(invoice_no);
                    }
                    else
                    {
                        _dt = PurchasesObj.GetAllPurchaseByInvoice(invoice_no);
                    }
                    mainForm.Load_products_to_grid_by_invoiceno(_dt, invoice_no);
                    this.Close();
                }
                
            }
            
        }

        private void grid_sales_report_DoubleClick(object sender, EventArgs e)
        {
            var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
            PurchasesBLL PurchasesObj = new PurchasesBLL();
            DataTable _dt = new DataTable();

            if (chk_hold_purchases.Checked)
            {
                _dt = PurchasesObj.GetAll_Hold_PurchaseByInvoice(invoice_no);
            }
            else
            {
                _dt = PurchasesObj.GetAllPurchaseByInvoice(invoice_no);
            }
            mainForm.Load_products_to_grid_by_invoiceno(_dt, invoice_no);
            this.Close();
        }

        private void txt_from_date_ValueChanged(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 3;
        }

        private void txt_to_date_ValueChanged(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 3;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(grid_sales_report.Rows.Count > 0)
            {
                var invoice_no = grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString();
                PurchasesBLL PurchasesObj = new PurchasesBLL();
                DataTable _dt = new DataTable();

                if (chk_hold_purchases.Checked)
                {
                    _dt = PurchasesObj.GetAll_Hold_PurchaseByInvoice(invoice_no);
                }
                else
                {
                    _dt = PurchasesObj.GetAllPurchaseByInvoice(invoice_no);
                }
                mainForm.Load_products_to_grid_by_invoiceno(_dt, invoice_no);
                this.Close();
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_sales_report.CurrentRow.Cells["invoice_no"].Value.ToString());
        }

    }
}
