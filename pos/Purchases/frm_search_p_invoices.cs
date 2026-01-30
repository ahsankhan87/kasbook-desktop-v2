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
using pos.UI.Busy;

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
                    to_date = txt_to_date.Value.Date.ToString("yyyy-MM-dd");
                }

                if (Listbox_method.SelectedIndex == 4)
                {
                    total_amount = (string.IsNullOrEmpty(txt_condition.Text) ? 0 : double.Parse(txt_condition.Text));
                }

                grid_sales_report.AutoGenerateColumns = false;

                if (string.IsNullOrEmpty(invoice_no) && string.IsNullOrEmpty(supplier) && string.IsNullOrEmpty(supplier_inv_no)
                    && total_amount == 0 && string.IsNullOrEmpty(from_date) && string.IsNullOrEmpty(to_date))
                {
                    UiMessages.ShowWarning(
                        "Please enter a search value or select a date range.",
                        "يرجى إدخال قيمة للبحث أو تحديد نطاق تاريخ.");
                    txt_condition.Focus();
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Searching purchase invoices...", "جارٍ البحث عن فواتير المشتريات...")))
                {
                    PurchasesReportBLL report_obj = new PurchasesReportBLL();

                    DataTable accounts_dt;
                    if (chk_hold_purchases.Checked)
                    {
                        accounts_dt = report_obj.Hold_PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
                    }
                    else
                    {
                        accounts_dt = report_obj.PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
                    }

                    grid_sales_report.DataSource = accounts_dt;

                    if (accounts_dt == null || accounts_dt.Rows.Count == 0)
                    {
                        UiMessages.ShowInfo(
                            "No invoices matched your search criteria.",
                            "لا توجد فواتير مطابقة لمعايير البحث.");
                    }
                }

                this.ActiveControl = grid_sales_report;
            }
            catch (FormatException)
            {
                UiMessages.ShowWarning(
                    "Please enter a valid amount.",
                    "يرجى إدخال مبلغ صحيح.");
                txt_condition.Focus();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "An unexpected error occurred while searching purchase invoices. Please try again.",
                    "حدث خطأ غير متوقع أثناء البحث عن فواتير المشتريات. يرجى المحاولة مرة أخرى.");
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
                if (e.KeyCode == Keys.Escape)
                {
                    btn_close.PerformClick();
                }
                if ((e.KeyCode == Keys.Return) && (e.Modifiers == Keys.Control))
                {
                    btn_ok.PerformClick();
                }
                if (e.KeyCode == Keys.F2 && (e.Modifiers == Keys.Control))
                {
                    txt_condition.Focus();
                }
                if (e.KeyData == Keys.Down)
                {
                    if (this.ActiveControl != grid_sales_report)
                    {
                        this.ActiveControl = grid_sales_report;
                    }
                }
                if(e.KeyData == Keys.F4)
                {
                    chk_hold_purchases.Checked = !chk_hold_purchases.Checked;
                }
                if( e.KeyData == Keys.F5)
                {
                    Listbox_method.Focus();
                }
            }
            catch (Exception)
            {
                UiMessages.ShowWarning(
                    "Unable to process the selected shortcut key.",
                    "تعذر تنفيذ اختصار لوحة المفاتيح المحدد.");
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
                LoadPurchaseByInvoiceNo(); 
                this.Close();

            }
            
        }

        private void grid_sales_report_DoubleClick(object sender, EventArgs e)
        {
            LoadPurchaseByInvoiceNo();
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
            LoadPurchaseByInvoiceNo();
            this.Close();
        }
        private void LoadPurchaseByInvoiceNo()
        {
            try
            {
                if (mainForm == null)
                {
                    UiMessages.ShowWarning(
                        "Unable to load the selected invoice because the purchase form is not available.",
                        "تعذر تحميل الفاتورة المحددة لأن شاشة المشتريات غير متاحة.");
                    return;
                }

                if (grid_sales_report.CurrentRow == null || grid_sales_report.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice to continue.",
                        "يرجى تحديد فاتورة للمتابعة.");
                    return;
                }

                var cell = grid_sales_report.CurrentRow.Cells["invoice_no"];
                var selectedInvoiceNo = cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty;

                if (string.IsNullOrWhiteSpace(selectedInvoiceNo))
                {
                    UiMessages.ShowWarning(
                        "The selected row does not contain a valid invoice number.",
                        "الصف المحدد لا يحتوي على رقم فاتورة صالح.");
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Loading invoice details...", "جارٍ تحميل تفاصيل الفاتورة...")))
                {
                    PurchasesBLL PurchasesObj = new PurchasesBLL();
                    DataTable _dt;

                    if (chk_hold_purchases.Checked)
                    {
                        _dt = PurchasesObj.GetAll_Hold_PurchaseByInvoice(selectedInvoiceNo);
                    }
                    else
                    {
                        _dt = PurchasesObj.GetAllPurchaseByInvoice(selectedInvoiceNo);
                    }

                    mainForm.Load_products_to_grid_by_invoiceno(_dt, selectedInvoiceNo);
                }
            }
            catch (Exception)
            {
                UiMessages.ShowError(
                    "An error occurred while loading the selected purchase invoice.",
                    "حدث خطأ أثناء تحميل فاتورة المشتريات المحددة.");
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_sales_report.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice first.",
                        "يرجى تحديد فاتورة أولاً.");
                    return;
                }

                var cell = grid_sales_report.CurrentRow.Cells["invoice_no"];
                var value = cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty;

                if (string.IsNullOrWhiteSpace(value))
                {
                    UiMessages.ShowWarning(
                        "The selected invoice number is empty.",
                        "رقم الفاتورة المحدد فارغ.");
                    return;
                }

                Clipboard.SetText(value);
                UiMessages.ShowInfo(
                    "Invoice number copied to the clipboard.",
                    "تم نسخ رقم الفاتورة إلى الحافظة.");
            }
            catch (Exception)
            {
                UiMessages.ShowError(
                    "Unable to copy the invoice number to the clipboard.",
                    "تعذر نسخ رقم الفاتورة إلى الحافظة.");
            }
        }

    }
}
