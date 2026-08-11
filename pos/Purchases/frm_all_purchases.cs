using pos.Security.Authorization;
using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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

            // Wire up bulk posting button
            if (btnPostToJournalEntry != null)
                btnPostToJournalEntry.Click += btnPostToJournalEntry_Click;
        }

        public void frm_all_purchases_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();

            // Wire up grid formatting events
            grid_all_purchases.DataBindingComplete += Grid_DataBindingComplete;
            grid_all_purchases.CellFormatting += Grid_CellFormatting;

            load_all_purchases_grid();
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
                null, grid_all_purchases, new object[] { true });

            grid_all_purchases.BackgroundColor = SystemColors.AppWorkspace;
            grid_all_purchases.RowHeadersVisible = false;
            grid_all_purchases.ColumnHeadersHeight = 36;
            grid_all_purchases.RowTemplate.Height = 30;
            grid_all_purchases.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_all_purchases.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_all_purchases.DefaultCellStyle.BackColor = SystemColors.Window;
            grid_all_purchases.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_all_purchases.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_all_purchases.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid_all_purchases.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Hide internal id column
            id.Visible = false;
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
                    DataTable dt = objpurchasesBLL.GetAllPurchases();
                    grid_all_purchases.DataSource = dt;

                    // Style foreign purchases with visual indicator
                    StyleForeignPurchases();
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

        private void StyleForeignPurchases()
        {
            try
            {
                foreach (DataGridViewRow row in grid_all_purchases.Rows)
                {
                    string currencyCode = Convert.ToString(row.Cells["currency_code"].Value ?? "SAR");

                    // Highlight foreign purchases (non-SAR)
                    if (currencyCode != "SAR" && !string.IsNullOrEmpty(currencyCode))
                    {
                        // Light blue background for foreign purchase rows
                        row.DefaultCellStyle.BackColor = Color.FromArgb(200, 220, 240); // Light blue
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail on styling, data is still visible
                System.Diagnostics.Debug.WriteLine("Styling error: " + ex.Message);
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
                    string supplierInvoiceNo = Convert.ToString(grid_all_purchases.CurrentRow.Cells["supplier_invoice_no"].Value);
                    if (string.IsNullOrWhiteSpace(invoiceNo))
                    {
                        UiMessages.ShowWarning(
                            "Please select a valid purchase invoice.",
                            "يرجى اختيار فاتورة شراء صالحة.",
                            captionEn: "Purchases",
                            captionAr: "المشتريات");
                        return;
                    }

                    using (var supplierNameChange = new Suppliers.ChangeSupplierName(invoiceNo, supplierInvoiceNo))
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

        /// <summary>
        /// Get list of checked (selected) unposted rows for bulk posting.
        /// Only returns rows where posted = false/0.
        /// </summary>
        private List<DataGridViewRow> GetCheckedUnpostedRows()
        {
            List<DataGridViewRow> checkedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in grid_all_purchases.Rows)
            {
                // Check if row is selected via checkbox
                object cellValue = row.Cells["colSelect"].Value;
                if (cellValue is bool && (bool)cellValue)
                {
                    // Verify row is actually unposted (posted = false/0)
                    object postedObj = row.Cells["posted"].Value;
                    bool isPosted = false;

                    if (postedObj != null && postedObj != DBNull.Value)
                    {
                        if (postedObj is bool)
                            isPosted = (bool)postedObj;
                        else if (postedObj is int)
                            isPosted = Convert.ToInt32(postedObj) != 0;
                        else if (postedObj is string)
                            isPosted = !string.IsNullOrEmpty(Convert.ToString(postedObj)) && 
                                      !Convert.ToString(postedObj).Equals("0", StringComparison.OrdinalIgnoreCase) &&
                                      !Convert.ToString(postedObj).Equals("false", StringComparison.OrdinalIgnoreCase);
                    }

                    // Only include if not posted
                    if (!isPosted)
                    {
                        checkedRows.Add(row);
                    }
                }
            }
            return checkedRows;
        }

        /// <summary>
        /// Bulk post selected unposted purchases to journal entries.
        /// </summary>
        private void btnPostToJournalEntry_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> selectedRows = GetCheckedUnpostedRows();

            if (selectedRows.Count == 0)
            {
                UiMessages.ShowInfo(
                    UiMessages.T("Please select one or more unposted purchases to post to journal.", 
                                  "يرجى اختيار شراء واحد أو أكثر من المشتريات غير المسجلة للنشر في دفتر اليوميات."),
                    UiMessages.T("No Selection", "لا توجد اختيارات"),
                    UiMessages.T("Purchases", "المشتريات"),
                    UiMessages.T("المشتريات", "المشتريات"));
                return;
            }

            // Confirm bulk posting
            string confirmMsg = UiMessages.T(
                string.Format("Post {0} purchases to journal entries?", selectedRows.Count),
                string.Format("نشر {0} شراء في دفتر اليوميات؟", selectedRows.Count));

            if (UiMessages.ConfirmYesNo(confirmMsg, "Confirm Post to Journal", "تأكيد النشر في دفتر اليوميات") != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Posting purchases to journal...", "جاري نشر المشتريات في دفتر اليوميات...")))
                {
                    // Extract invoice numbers from selected rows
                    List<string> invoiceNos = new List<string>();
                    foreach (DataGridViewRow row in selectedRows)
                    {
                        object invObj = row.Cells["invoice_no"].Value;
                        if (invObj != null)
                        {
                            string invoiceNo = Convert.ToString(invObj);
                            if (!string.IsNullOrWhiteSpace(invoiceNo))
                                invoiceNos.Add(invoiceNo);
                        }
                    }

                    if (invoiceNos.Count == 0)
                    {
                        UiMessages.ShowWarning(
                            UiMessages.T("No valid invoice numbers found.", "لم يتم العثور على أرقام فواتير صحيحة."),
                            UiMessages.T("Invalid Data", "بيانات غير صحيحة"));
                        return;
                    }

                    // Call BLL to post purchases to journal
                    int successCount = 0;
                    int failureCount = 0;
                    List<string> failedInvoices = new List<string>();

                    foreach (string invoiceNo in invoiceNos)
                    {
                        try
                        {
                            bool posted = objBLL.PostPurchaseToJournal(invoiceNo, UsersModal.logged_in_userid);
                            if (posted)
                                successCount++;
                            else
                            {
                                failureCount++;
                                failedInvoices.Add(invoiceNo);
                            }
                        }
                        catch (Exception ex)
                        {
                            failureCount++;
                            failedInvoices.Add(invoiceNo + " - " + ex.Message);
                        }
                    }

                    // Show results
                    string resultMsg = string.Format(
                        UiMessages.T("Posted: {0}\r\nFailed: {1}", "تم النشر: {0}\r\nفشل: {1}"),
                        successCount, failureCount);

                    if (failedInvoices.Count > 0)
                    {
                        resultMsg += "\r\n\r\n" + UiMessages.T("Failed Invoices:", "الفواتير الفاشلة:") + "\r\n";
                        resultMsg += string.Join("\r\n", failedInvoices.Take(10)); // Show first 10
                        if (failedInvoices.Count > 10)
                            resultMsg += string.Format("\r\n... and {0} more", failedInvoices.Count - 10);
                    }

                    UiMessages.ShowInfo(resultMsg, 
                        UiMessages.T("Bulk Post Result", "نتيجة النشر الجماعي"),
                        UiMessages.T("Purchases", "المشتريات"),
                        UiMessages.T("المشتريات", "المشتريات"));

                    // Reload data to refresh posted flags
                    load_all_purchases_grid();

                    // Clear checkbox selections for newly posted purchases
                    foreach (DataGridViewRow row in grid_all_purchases.Rows)
                    {
                        object postedObj = row.Cells["posted"].Value;
                        bool isPosted = false;

                        if (postedObj != null && postedObj != DBNull.Value)
                        {
                            if (postedObj is bool)
                                isPosted = (bool)postedObj;
                            else if (postedObj is int)
                                isPosted = Convert.ToInt32(postedObj) != 0;
                            else if (postedObj is string)
                                isPosted = !string.IsNullOrEmpty(Convert.ToString(postedObj)) && 
                                          !Convert.ToString(postedObj).Equals("0", StringComparison.OrdinalIgnoreCase);
                        }

                        // Uncheck posted purchases
                        if (isPosted && row.Cells["colSelect"].Value is bool)
                            row.Cells["colSelect"].Value = false;
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    UiMessages.T("Failed to post purchases to journal.", "فشل نشر المشتريات في دفتر اليوميات."),
                    ex.Message,
                    captionEn: "Post to Journal",
                    captionAr: "نشر في دفتر اليوميات");
            }
        }

        /// <summary>
        /// Format grid after data binding: disable checkboxes for posted rows and color unposted rows.
        /// </summary>
        private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in grid_all_purchases.Rows)
                {
                    // Get posted status
                    object postedObj = row.Cells["posted"].Value;
                    bool isPosted = false;

                    if (postedObj != null && postedObj != DBNull.Value)
                    {
                        if (postedObj is bool)
                            isPosted = (bool)postedObj;
                        else if (postedObj is int)
                            isPosted = Convert.ToInt32(postedObj) != 0;
                        else if (postedObj is string)
                            isPosted = !string.IsNullOrEmpty(Convert.ToString(postedObj)) && 
                                      !Convert.ToString(postedObj).Equals("0", StringComparison.OrdinalIgnoreCase) &&
                                      !Convert.ToString(postedObj).Equals("false", StringComparison.OrdinalIgnoreCase);
                    }

                    // Disable checkbox for posted purchases
                    if (row.Cells["colSelect"] is DataGridViewCheckBoxCell checkCell)
                    {
                        checkCell.ReadOnly = isPosted;
                    }

                    // Color unposted rows light red
                    if (!isPosted)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230); // Light red
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White; // White for posted
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail for formatting (doesn't block functionality)
                System.Diagnostics.Debug.WriteLine("Grid formatting error: " + ex.Message);
            }
        }

        /// <summary>
        /// Format individual cells: prevent checkbox editing for posted rows.
        /// </summary>
        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Check if this is the checkbox column
                if (e.ColumnIndex >= 0 && grid_all_purchases.Columns[e.ColumnIndex].Name == "colSelect")
                {
                    DataGridViewRow row = grid_all_purchases.Rows[e.RowIndex];

                    // Get posted status
                    object postedObj = row.Cells["posted"].Value;
                    bool isPosted = false;

                    if (postedObj != null && postedObj != DBNull.Value)
                    {
                        if (postedObj is bool)
                            isPosted = (bool)postedObj;
                        else if (postedObj is int)
                            isPosted = Convert.ToInt32(postedObj) != 0;
                        else if (postedObj is string)
                            isPosted = !string.IsNullOrEmpty(Convert.ToString(postedObj)) && 
                                      !Convert.ToString(postedObj).Equals("0", StringComparison.OrdinalIgnoreCase) &&
                                      !Convert.ToString(postedObj).Equals("false", StringComparison.OrdinalIgnoreCase);
                    }

                    // Disable/enable checkbox based on posted status
                    if (row.Cells["colSelect"] is DataGridViewCheckBoxCell checkCell)
                    {
                        checkCell.ReadOnly = isPosted;

                        // If posted, clear the checkbox
                        if (isPosted && row.Cells["colSelect"].Value is bool)
                        {
                            row.Cells["colSelect"].Value = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail for formatting
                System.Diagnostics.Debug.WriteLine("Cell formatting error: " + ex.Message);
            }
        }
    }
}
