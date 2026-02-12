using pos.Security.Authorization;
using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_all_purchases : Form
    {
        PurchasesBLL objBLL = new PurchasesBLL();

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_all_purchases()
        {
            InitializeComponent();
        }

        public void frm_all_purchases_Load(object sender, EventArgs e)
        {
            load_all_purchases_grid();
        }

        public void load_all_purchases_grid()
        {
            using (BusyScope.Show(this, UiMessages.T("Loading purchases...", "جارٍ تحميل المشتريات...")))
            {
                try
                {
                    grid_all_purchases.DataSource = null;

                    // bind data
                    PurchasesBLL objpurchasesBLL = new PurchasesBLL();
                    grid_all_purchases.AutoGenerateColumns = false;
                    grid_all_purchases.DataSource = objpurchasesBLL.GetAllPurchases();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Unable to load purchases.\n" + ex.Message,
                        "تعذر تحميل المشتريات.\n" + ex.Message,
                        captionEn: "Purchases",
                        captionAr: "المشتريات");
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_purchases_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Searching...", "جارٍ البحث...")))
            {
                try
                {
                    string condition = txt_search.Text;
                    if (!string.IsNullOrWhiteSpace(condition))
                    {
                        grid_all_purchases.DataSource = objBLL.SearchRecord(condition);
                    }
                    else
                    {
                        UiMessages.ShowInfo(
                            "Enter an invoice number or keyword to search.",
                            "أدخل رقم الفاتورة أو كلمة للبحث.",
                            captionEn: "Purchases",
                            captionAr: "المشتريات");
                    }

                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Error while searching purchases.\n" + ex.Message,
                        "حدث خطأ أثناء البحث عن المشتريات.\n" + ex.Message,
                        captionEn: "Purchases",
                        captionAr: "المشتريات");
                }
            }
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void frm_all_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                txt_search.Focus();
            }
            if(e.KeyCode == Keys.P && e.Control)
            {
                btn_print_invoice.PerformClick();
            }
        }

        private void grid_all_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var invoice_no = Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value);
                load_purchases_items_detail(invoice_no);
            }
        }

        private void grid_all_purchases_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var invoice_no = Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value);
            load_purchases_items_detail(invoice_no);
        }

        private void load_purchases_items_detail(string invoice_no)
        {
            if (string.IsNullOrWhiteSpace(invoice_no))
            {
                UiMessages.ShowWarning(
                    "The selected row does not contain a valid invoice number.",
                    "السطر المحدد لا يحتوي على رقم فاتورة صحيح.",
                    captionEn: "Purchases",
                    captionAr: "المشتريات");
                return;
            }

            using (var frm_purchases_detail_obj = new frm_purchases_detail())
            {
                frm_purchases_detail_obj.load_purchases_detail_grid(invoice_no);
                frm_purchases_detail_obj.ShowDialog(this);
            }
        }

        private void grid_all_purchases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;

                string name = grid_all_purchases.Columns[e.ColumnIndex].Name;
                if (name == "detail")
                {
                    var invoice_no = Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value);
                    load_purchases_items_detail(invoice_no);
                }
                if (name == "btn_delete")
                {
                    // Permission check
                    if (!_auth.HasPermission(_currentUser, Permissions.Purchases_Delete))
                    {
                        UiMessages.ShowWarning(
                            "You do not have permission to delete purchase transactions.",
                            "ليست لديك صلاحية حذف معاملات الشراء.",
                            captionEn: "Permission denied",
                            captionAr: "صلاحية مرفوضة");
                        return;
                    }

                    var invoice_no = Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value);
                    if (string.IsNullOrWhiteSpace(invoice_no))
                    {
                        UiMessages.ShowWarning(
                            "Please select a valid purchase invoice.",
                            "يرجى اختيار فاتورة شراء صالحة.",
                            captionEn: "Purchases",
                            captionAr: "المشتريات");
                        return;
                    }

                    DialogResult result = UiMessages.ConfirmYesNo(
                        $"Are you sure you want to delete purchase invoice {invoice_no}?",
                        $"هل أنت متأكد أنك تريد حذف فاتورة الشراء {invoice_no}؟",
                        captionEn: "Confirm delete",
                        captionAr: "تأكيد الحذف",
                        defaultButton: MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        using (BusyScope.Show(this, UiMessages.T("Deleting...", "جارٍ الحذف...")))
                        {
                            int qresult = objBLL.DeletePurchases(invoice_no);
                            if (qresult > 0)
                            {
                                UiMessages.ShowInfo(
                                    $"Invoice {invoice_no} was deleted successfully.",
                                    $"تم حذف الفاتورة {invoice_no} بنجاح.",
                                    captionEn: "Purchases",
                                    captionAr: "المشتريات");

                                load_all_purchases_grid();
                            }
                            else
                            {
                                UiMessages.ShowError(
                                    $"Invoice {invoice_no} could not be deleted. Please try again.",
                                    $"تعذر حذف الفاتورة {invoice_no}. يرجى المحاولة مرة أخرى.",
                                    captionEn: "Purchases",
                                    captionAr: "المشتريات");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    ex.Message,
                    ex.Message,
                    captionEn: "Error",
                    captionAr: "خطأ");
            }
        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_purchases.Rows.Count > 0)
            {
                // permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Purchases_Print))
                {
                    UiMessages.ShowWarning(
                        "You do not have permission to print purchase invoices.",
                        "ليست لديك صلاحية طباعة فواتير الشراء.",
                        captionEn: "Permission denied",
                        captionAr: "صلاحية مرفوضة");
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Preparing invoice...", "جارٍ تجهيز الفاتورة...")))
                {
                    var dt = load_purchase_receipt();
                    if (dt == null)
                    {
                        UiMessages.ShowWarning(
                            "No invoice data is available for printing.",
                            "لا توجد بيانات فاتورة للطباعة.",
                            captionEn: "Purchases",
                            captionAr: "المشتريات");
                        return;
                    }

                    using (frm_purchase_invoice obj = new frm_purchase_invoice(dt, false))
                    {
                        obj.ShowDialog(this);
                    }
                }
            }

        }

        public DataTable load_purchase_receipt()
        {
            if (grid_all_purchases.Rows.Count > 0)
            {
                var invoice_no = Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value);
                if (string.IsNullOrWhiteSpace(invoice_no)) return null;

                return objBLL.PurchaseReceipt(invoice_no);
            }
            return null;

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value));

        }

        private void BtnSupplierNameChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_all_purchases.Rows.Count > 0)
                {
                    string invoiceNo = Convert.ToString(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value);
                    if (string.IsNullOrWhiteSpace(invoiceNo))
                    {
                        UiMessages.ShowWarning(
                            "Please select a valid purchase invoice.",
                            "يرجى اختيار فاتورة شراء صالحة.",
                            captionEn: "Purchases",
                            captionAr: "المشتريات");
                        return;
                    }

                    using (var supplierNameChange = new Suppliers.ChangeSupplierName(invoiceNo))
                    {
                        supplierNameChange.ShowDialog(this);
                    }

                    load_all_purchases_grid();
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    ex.Message,
                    ex.Message,
                    captionEn: "Error",
                    captionAr: "خطأ");
            }
        }
    }
}
