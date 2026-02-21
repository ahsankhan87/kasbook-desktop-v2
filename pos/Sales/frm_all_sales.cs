using Newtonsoft.Json;
using pos.Master.Companies.zatca;
using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ZATCA.EInvoice.SDK;
using ZATCA.EInvoice.SDK.Contracts.Models;
using pos.UI;
using pos.UI.Busy;
using System.Drawing;

namespace pos
{
    public partial class frm_all_sales : SecuredForm
    {
        public SalesBLL objSalesBLL = new SalesBLL();

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        private readonly Timer _searchDebounce = new Timer();
        private const int DebounceMs = 300;

        public frm_all_sales()
        {
            InitializeComponent();

            // Ensure user identity exists; hydrate claims from DB
            if (_currentUser == null)
            {
                var parsedRole = SystemRole.Viewer;
                System.Enum.TryParse(UsersModal.logged_in_user_role, true, out parsedRole);
                AppSecurityContext.SetUser(new UserIdentity
                {
                    UserId = UsersModal.logged_in_userid,
                    BranchId = UsersModal.logged_in_branch_id,
                    Username = UsersModal.logged_in_username,
                    Role = parsedRole
                });
                _currentUser = AppSecurityContext.User;
            }

            // Tag permission-aware controls
            //btn_print_invoice.Tag = Permissions.Sales_Print;
            //Btn_PrintPOS80.Tag = Permissions.Sales_Print;

            // Debounce for search
            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
            if (txt_search != null)
                txt_search.TextChanged += txt_search_TextChanged;
        }

        private void frm_all_sales_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_all_sales_grid();
        }

        private void StyleForm()
        {
            // ── Header panel ──────────────────────────────────────────
            panel2.BackColor = AppTheme.PrimaryDark;
            panel2.ForeColor = Color.White;
            lbl_taxes_title.Font = AppTheme.FontHeader;
            lbl_taxes_title.ForeColor = Color.White;

            // ── Body panel ────────────────────────────────────────────
            panel1.BackColor = SystemColors.Control;

            // ── Grid ──────────────────────────────────────────────────
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, grid_all_sales, new object[] { true });

            grid_all_sales.BackgroundColor = SystemColors.AppWorkspace;
            grid_all_sales.RowHeadersVisible = false;
            grid_all_sales.ColumnHeadersHeight = 36;
            grid_all_sales.RowTemplate.Height = 30;
            grid_all_sales.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_all_sales.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_all_sales.DefaultCellStyle.BackColor = SystemColors.Window;
            grid_all_sales.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_all_sales.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_all_sales.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid_all_sales.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Hide internal id column
            id.Visible = false;
        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            btn_search.PerformClick();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        public void load_all_sales_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading sales...", "جاري تحميل المبيعات...")))
                {
                    grid_all_sales.DataSource = null;
                    grid_all_sales.AutoGenerateColumns = false;
                    grid_all_sales.DataSource = objSalesBLL.GetAllSales();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    UiMessages.T("Failed to load sales. Please try again.", "فشل تحميل المبيعات. يرجى المحاولة مرة أخرى."),
                    ex.Message,
                    captionEn: "Sales",
                    captionAr: "المبيعات");
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_sales_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    string condition = (txt_search.Text ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(condition))
                    {
                        load_all_sales_grid();
                        return;
                    }

                    grid_all_sales.DataSource = objSalesBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                _searchDebounce.Stop();
                btn_search.PerformClick();
            }
        }

        private bool TryGetSelectedSale(out int saleId, out string invoiceNo)
        {
            saleId = 0;
            invoiceNo = null;

            if (grid_all_sales.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a sale first.",
                    "يرجى اختيار عملية بيع أولاً.",
                    "Sales",
                    "المبيعات");
                return false;
            }

            var idObj = grid_all_sales.CurrentRow.Cells["id"].Value;
            var invObj = grid_all_sales.CurrentRow.Cells["invoice_no"].Value;

            if (idObj == null || !int.TryParse(Convert.ToString(idObj), out saleId) || saleId <= 0)
            {
                UiMessages.ShowWarning(
                    "Invalid sale selected.",
                    "تم اختيار عملية بيع غير صالحة.",
                    "Sales",
                    "المبيعات");
                return false;
            }

            invoiceNo = Convert.ToString(invObj);
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                UiMessages.ShowWarning(
                    "Invalid invoice number.",
                    "رقم فاتورة غير صالح.",
                    "Sales",
                    "المبيعات");
                return false;
            }

            return true;
        }

        private void grid_all_sales_KeyDown(object sender, KeyEventArgs e)
        {
            if (grid_all_sales.RowCount > 0 && e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (TryGetSelectedSale(out int saleId, out string invoiceNo))
                    load_sales_items_detail(saleId, invoiceNo);
            }
        }

        private void grid_all_sales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (TryGetSelectedSale(out int saleId, out string invoiceNo))
                load_sales_items_detail(saleId, invoiceNo);
        }

        private void load_sales_items_detail(int sale_id, string invoice_no)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading sale details...", "جاري تحميل تفاصيل البيع...")))
            {
                frm_sales_detail frm_sales_detail_obj = new frm_sales_detail(sale_id, invoice_no);
                frm_sales_detail_obj.ShowDialog();
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {

            var receipt = load_sales_receipt();
            if (receipt == null)
            {
                UiMessages.ShowInfo(
                    "Please select a sale to print.",
                    "يرجى اختيار عملية بيع للطباعة.",
                    "Print",
                    "طباعة");
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Preparing receipt...", "جاري تجهيز الإيصال...")))
            using (frm_sales_receipt obj = new frm_sales_receipt(receipt))
            {
                obj.ShowDialog();
            }
        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_sales.Rows.Count <= 0 || grid_all_sales.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "No sales available.",
                    "لا توجد مبيعات.",
                    "Sales",
                    "المبيعات");
                return;
            }

            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Sales_Print))
            {
                UiMessages.ShowWarning(
                    "You don't have permission to print sales invoices.",
                    "ليس لديك صلاحية لطباعة فواتير المبيعات.",
                    "Permission Denied",
                    "تم رفض الصلاحية");
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Print invoice with product codes?",
                "هل تريد طباعة الفاتورة مع أكواد المنتجات؟",
                captionEn: "Print Invoice",
                captionAr: "طباعة الفاتورة");

            bool isPrintInvoiceWithCode = (confirm == DialogResult.Yes);

            using (BusyScope.Show(this, UiMessages.T("Opening invoice...", "جاري فتح الفاتورة...")))
            using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(), false, isPrintInvoiceWithCode))
            {
                obj.ShowDialog();
            }
        }

        private void grid_all_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_all_sales.Columns[e.ColumnIndex].Name;

                if (name == "detail")
                {
                    if (TryGetSelectedSale(out int saleId, out string invoiceNo))
                        load_sales_items_detail(saleId, invoiceNo);
                    return;
                }

                if (name == "btn_delete" && !_auth.HasPermission(_currentUser, Permissions.Sales_Delete))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to delete sales.",
                        "ليس لديك صلاحية لحذف المبيعات.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }
                if (name == "btn_send_zatca" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_Sign))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to sign invoices to ZATCA.",
                        "ليس لديك صلاحية لتوقيع الفواتير لإرسالها إلى زاتكا.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }
                if (name == "btn_report_to_zatca" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_Report))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to report invoices to ZATCA.",
                        "ليس لديك صلاحية لإرسال الفواتير (إبلاغ) إلى زاتكا.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }
                if (name == "btn_download_ubl" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_DownloadUBL))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to download UBL.",
                        "ليس لديك صلاحية لتنزيل ملف UBL.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }
                if (name == "btn_show_qrcode" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_Qr_Show))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to view QR code.",
                        "ليس لديك صلاحية لعرض رمز الاستجابة السريعة.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }

                if (name == "btn_delete")
                {
                    if (!TryGetSelectedSale(out _, out string invoiceNo))
                        return;

                    var confirm = UiMessages.ConfirmYesNo(
                        "Delete this sale? This action cannot be undone.",
                        "هل تريد حذف عملية البيع؟ لا يمكن التراجع عن هذا الإجراء.",
                        captionEn: "Confirm Delete",
                        captionAr: "تأكيد الحذف");

                    if (confirm != DialogResult.Yes)
                        return;

                    using (BusyScope.Show(this, UiMessages.T("Deleting sale...", "جاري حذف عملية البيع...")))
                    {
                        int qresult = objSalesBLL.DeleteSales(invoiceNo);
                        if (qresult > 0)
                        {
                            UiMessages.ShowInfo(
                                "Sale deleted successfully.",
                                "تم حذف عملية البيع بنجاح.",
                                "Deleted",
                                "تم الحذف");
                            load_all_sales_grid();
                        }
                        else
                        {
                            UiMessages.ShowError(
                                "Sale could not be deleted. Please try again.",
                                "تعذر حذف عملية البيع. يرجى المحاولة مرة أخرى.",
                                "Delete Failed",
                                "فشل الحذف");
                        }
                    }
                    return;
                }

                if (name == "btn_send_zatca")
                {
                    if (!TryGetSelectedSale(out _, out string invoiceNo))
                        return;
                    using (BusyScope.Show(this, UiMessages.T("Signing invoice...", "جاري توقيع الفاتورة...")))
                    {
                        // Use shared helper so this form doesn't need to own signing logic
                        ZatcaHelper.SignInvoiceToZatca(invoiceNo);
                    }
                    load_all_sales_grid();
                    return;
                }

                if (name == "btn_zatca_compliance_check")
                {
                    if (!TryGetSelectedSale(out _, out string invoiceNo))
                        return;
                    UiMessages.ShowInfo(
                        UiMessages.T("Compliance check is available in the ZATCA invoices screen.", "فحص المطابقة متاح في شاشة فواتير زاتكا."),
                        UiMessages.T("Compliance check is available in the ZATCA invoices screen.", "فحص المطابقة متاح في شاشة فواتير زاتكا."),
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                if (name == "btn_report_to_zatca")
                {
                    if (!TryGetSelectedSale(out _, out string invoiceNo))
                        return;
                    using (BusyScope.Show(this, UiMessages.T("Reporting invoice...", "جاري رفع/إبلاغ الفاتورة...")))
                    {
                        // Reporting/clearance depends on subtype; use helper which handles it elsewhere
                        // Default to reporting API for simplified invoices.
                        ZatcaHelper.ZatcaInvoiceReportingAsync(invoiceNo);
                    }
                    load_all_sales_grid();
                    return;
                }

                if (name == "btn_download_ubl")
                {
                    if (!TryGetSelectedSale(out _, out string invoiceNo))
                        return;
                    DownloadUBL(invoiceNo);
                    return;
                }

                if (name == "btn_show_qrcode")
                {
                    if (!TryGetSelectedSale(out _, out string invoiceNo))
                        return;

                    using (BusyScope.Show(this, UiMessages.T("Loading QR code...", "جاري تحميل رمز الاستجابة السريعة...")))
                    {
                        DataTable dt = objSalesBLL.SearchRecord(invoiceNo);

                        if (dt.Rows.Count > 0 && dt.Rows[0]["zatca_qrcode_phase2"] != DBNull.Value)
                        {
                            byte[] qrCodeBytes = (byte[])dt.Rows[0]["zatca_qrcode_phase2"];
                            pos.Master.Companies.zatca.ShowQRCodePhase2 showQRCodePhase2 = new ShowQRCodePhase2(qrCodeBytes, invoiceNo);
                            showQRCodePhase2.ShowDialog();
                        }
                        else
                        {
                            UiMessages.ShowInfo(
                                "No QR code found for this invoice.",
                                "لا يوجد رمز QR لهذه الفاتورة.",
                                "QR Code",
                                "رمز QR");
                        }
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void DownloadUBL(string invoiceNo)
        {
            string path = objSalesBLL.GetUblPath(invoiceNo);
            if (File.Exists(path))
                Process.Start("explorer.exe", "/select,\"" + path + "\"");
            else
                UiMessages.ShowWarning(
                    "UBL XML file not found.",
                    "ملف UBL XML غير موجود.",
                    "UBL",
                    "UBL");
        }

        private void btnSendWhatsApp_Click(object sender, EventArgs e)
        {
            if (grid_all_sales.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select an invoice first.",
                    "يرجى اختيار فاتورة أولاً.",
                    "WhatsApp",
                    "واتساب");
                return;
            }

            string invoiceNo = Convert.ToString(grid_all_sales.CurrentRow.Cells["invoice_no"].Value);
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                UiMessages.ShowInfo(
                    "Please select a valid invoice.",
                    "يرجى اختيار فاتورة صالحة.",
                    "WhatsApp",
                    "واتساب");
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Opening WhatsApp...", "جاري فتح واتساب...")))
            {
                frm_send_whatsapp _frm_Send_Whatsapp = new frm_send_whatsapp(invoiceNo);
                _frm_Send_Whatsapp.ShowDialog();
            }
        }

        private void Btn_PrintPOS80_Click(object sender, EventArgs e)
        {
            // Permission Check
            if (!_auth.HasPermission(_currentUser, Permissions.Sales_Print))
            {
                UiMessages.ShowWarning(
                    "You don't have permission to print POS 80mm receipt. Please contact the administrator.",
                    "ليس لديك صلاحية لطباعة إيصال 80 مم. يرجى التواصل مع المسؤول.",
                    "Permission Denied",
                    "تم رفض الصلاحية");
                return;
            }

            if (grid_all_sales.Rows.Count > 0 && grid_all_sales.CurrentRow != null)
            {
                string invoiceNo = Convert.ToString(grid_all_sales.CurrentRow.Cells["invoice_no"].Value);
                if (string.IsNullOrWhiteSpace(invoiceNo))
                {
                    UiMessages.ShowInfo(
                        "Please select a valid invoice.",
                        "يرجى اختيار فاتورة صالحة.",
                        "Print",
                        "طباعة");
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Preparing POS receipt...", "جاري تجهيز إيصال نقاط البيع...")))
                using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(), false, false, true))
                {
                    obj.ShowDialog();
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "No sales available.",
                    "لا توجد مبيعات.",
                    "Sales",
                    "المبيعات");
            }
        }

        public DataTable load_sales_receipt()
        {
            if (grid_all_sales.Rows.Count > 0 && grid_all_sales.CurrentRow != null)
            {
                var invoice_no = Convert.ToString(grid_all_sales.CurrentRow.Cells["invoice_no"].Value);
                if (!string.IsNullOrWhiteSpace(invoice_no))
                    return objSalesBLL.SaleReceipt(invoice_no);
            }
            return null;
        }

        private void frm_all_sales_KeyDown(object sender, KeyEventArgs e)
        {
            // Reserved for shortcuts (e.g. refresh)
            if (e.KeyCode == Keys.F5)
            {
                btn_refresh.PerformClick();
                e.Handled = true;
            }
            if(e.KeyCode == Keys.P && e.Control)
            {
                btn_print_invoice.PerformClick();
                e.Handled = true;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid_all_sales.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select an invoice first.",
                    "يرجى اختيار فاتورة أولاً.",
                    "Sales",
                    "المبيعات");
                return;
            }

            var invoice = Convert.ToString(grid_all_sales.CurrentRow.Cells["invoice_no"].Value);
            if (string.IsNullOrWhiteSpace(invoice))
            {
                UiMessages.ShowWarning(
                    "Invoice number is empty.",
                    "رقم الفاتورة فارغ.",
                    "Sales",
                    "المبيعات");
                return;
            }

            Clipboard.SetText(invoice);
            UiMessages.ShowInfo(
                "Invoice number copied.",
                "تم نسخ رقم الفاتورة.",
                "Copy",
                "نسخ");
        }

        private void BtnCustomerNameChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_all_sales.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select an invoice first.",
                        "يرجى اختيار فاتورة أولاً.",
                        "Sales",
                        "المبيعات");
                    return;
                }

                string invoiceNo = Convert.ToString(grid_all_sales.CurrentRow.Cells["invoice_no"].Value);
                if (string.IsNullOrWhiteSpace(invoiceNo))
                {
                    UiMessages.ShowWarning(
                        "Invalid invoice number.",
                        "رقم فاتورة غير صالح.",
                        "Sales",
                        "المبيعات");
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Opening customer name change...", "جاري فتح تعديل اسم العميل...")))
                {
                    pos.Customers.CustomerNameChange customerNameChange = new Customers.CustomerNameChange(invoiceNo);
                    customerNameChange.ShowDialog();
                }

                load_all_sales_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }
    }
}
