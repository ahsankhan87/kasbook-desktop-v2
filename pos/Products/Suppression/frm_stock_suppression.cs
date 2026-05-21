using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace pos.Products.Suppression
{
    public partial class frm_stock_suppression : Form
    {
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly List<int> _selectedBranchIds = new List<int>();
        
        // Debounce timers and state
        private System.Windows.Forms.Timer _oldPartDebounceTimer;
        private System.Windows.Forms.Timer _newPartDebounceTimer;
        private string _lastOldSearchTerm = string.Empty;
        private string _lastNewSearchTerm = string.Empty;
        private const int DEBOUNCE_DELAY_MS = 500; // 500ms debounce

        public frm_stock_suppression()
        {
            InitializeComponent();
            InitializeDebounceTimers();
        }

        private void InitializeDebounceTimers()
        {
            // Old part debounce timer
            _oldPartDebounceTimer = new System.Windows.Forms.Timer();
            _oldPartDebounceTimer.Interval = DEBOUNCE_DELAY_MS;
            _oldPartDebounceTimer.Tick += (s, e) =>
            {
                _oldPartDebounceTimer.Stop();
                PerformOldPartSearch();
            };

            // New part debounce timer
            _newPartDebounceTimer = new System.Windows.Forms.Timer();
            _newPartDebounceTimer.Interval = DEBOUNCE_DELAY_MS;
            _newPartDebounceTimer.Tick += (s, e) =>
            {
                _newPartDebounceTimer.Stop();
                PerformNewPartSearch();
            };
        }

        private void frm_stock_suppression_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            lblOldCaption.ForeColor = AppTheme.TextPrimary;
            lblAlreadySupersededTo.ForeColor = AppTheme.TextPrimary;
            lblBranchSummary.ForeColor = AppTheme.TextPrimary;

            // Make textboxes editable (remove readonly if set in designer)
            txtOldPartCode.ReadOnly = false;
            txtNewPartCode.ReadOnly = false;
            
            // Wire up text change events for debouncing
            txtOldPartCode.TextChanged += TxtOldPartCode_TextChanged;
            txtNewPartCode.TextChanged += TxtNewPartCode_TextChanged;

            txtOldPartCode.Focus();
            InitializeDefaultOptions();
            LoadDefaultBranch();
        }

        private void TxtOldPartCode_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtOldPartCode.Text.Trim();
            if (currentText != _lastOldSearchTerm && currentText.Length > 0)
            {
                _oldPartDebounceTimer.Stop();
                _oldPartDebounceTimer.Start();
            }
        }

        private void TxtNewPartCode_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtNewPartCode.Text.Trim();
            if (currentText != _lastNewSearchTerm && currentText.Length > 0)
            {
                _newPartDebounceTimer.Stop();
                _newPartDebounceTimer.Start();
            }
        }

        private void PerformOldPartSearch()
        {
            string searchTerm = txtOldPartCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 1)
                return;

            _lastOldSearchTerm = searchTerm;
            
            try
            {
                // Search for product by code or item number
                var dt = _productBLL.SearchRecordByProductNumber(searchTerm);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Found exact match
                    string itemNumber = Convert.ToString(dt.Rows[0]["item_number"]);
                    string name = Convert.ToString(dt.Rows[0]["name"]);
                    txtOldPartCode.Text = itemNumber;
                    lblOldCaption.Text = $"{itemNumber} ({name})";
                    LoadAlreadySupersededTo(itemNumber);
                }
                else
                {
                    // Try searching by code
                    dt = _productBLL.SearchRecordByProductCode(searchTerm);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string itemNumber = Convert.ToString(dt.Rows[0]["item_number"]);
                        string name = Convert.ToString(dt.Rows[0]["name"]);
                        txtOldPartCode.Text = itemNumber;
                        lblOldCaption.Text = $"{itemNumber} ({name})";
                        LoadAlreadySupersededTo(itemNumber);
                    }
                    else
                    {
                        lblOldCaption.Text = "Product not found";
                        lblOldCaption.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblOldCaption.Text = "Error searching product";
                lblOldCaption.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void PerformNewPartSearch()
        {
            string searchTerm = txtNewPartCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 1)
                return;

            _lastNewSearchTerm = searchTerm;

            try
            {
                // Search for product by code or item number
                var dt = _productBLL.SearchRecordByProductNumber(searchTerm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Found exact match
                    string itemNumber = Convert.ToString(dt.Rows[0]["item_number"]);
                    string name = Convert.ToString(dt.Rows[0]["name"]);
                    txtNewPartCode.Text = itemNumber;
                    // Don't show caption for new part, but could add if needed
                }
                else
                {
                    // Try searching by code
                    dt = _productBLL.SearchRecordByProductCode(searchTerm);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string itemNumber = Convert.ToString(dt.Rows[0]["item_number"]);
                        string name = Convert.ToString(dt.Rows[0]["name"]);
                        txtNewPartCode.Text = itemNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently handle errors
            }
        }

        private void InitializeDefaultOptions()
        {
            chkTransferStock.Checked = true;
            chkTransferPurchaseOrders.Checked = true;
            chkTransferPurchaseOrders.Enabled = false;
            chkTransferBackOrders.Checked = true;
            chkTransferBackOrders.Enabled = false;
            chkTransferHistory.Checked = true;
            chkTransferHistory.Enabled = false;
            chkZeroDemand.Checked = true;
            chkResetReorder.Checked = true;
            chkTransferPartDescription.Checked = false;
            chkTransferAssemblies.Checked = true;
            chkTransferAssemblies.Enabled = false;
        }

        private void LoadDefaultBranch()
        {
            _selectedBranchIds.Clear();
            _selectedBranchIds.Add(UsersModal.logged_in_branch_id);
            txtCompany.Text = UsersModal.logged_in_branch_id + "  " + UsersModal.logged_in_branch_name;
            lblBranchSummary.Text = "1 branch selected";
        }

        private void btnSelectOldPart_Click(object sender, EventArgs e)
        {
            SelectPart(isOldPart: true);
        }

        private void btnSelectNewPart_Click(object sender, EventArgs e)
        {
            SelectPart(isOldPart: false);
        }

        private void SelectPart(bool isOldPart)
        {
            // If user already typed in the textbox, use that as initial search term
            string initialSearchTerm = isOldPart ? 
                txtOldPartCode.Text.Trim() : 
                txtNewPartCode.Text.Trim();

            using (var dlg = new frm_stock_suppression_stock_records(initialSearchTerm))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                if (isOldPart)
                {
                    txtOldPartCode.Text = dlg.SelectedItemNumber;
                    lblOldCaption.Text = dlg.SelectedDisplayText;
                    lblOldCaption.ForeColor = AppTheme.TextPrimary;
                    LoadAlreadySupersededTo(dlg.SelectedItemNumber);
                }
                else
                {
                    txtNewPartCode.Text = dlg.SelectedItemNumber;
                }
            }
        }

        /// <summary>
        /// Load and display any existing supersession link for the old item.
        /// Uses the new superseded_to_item_code field.
        /// </summary>
        private void LoadAlreadySupersededTo(string oldItemNumber)
        {
            lblAlreadySupersededTo.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(oldItemNumber))
                return;

            try
            {
                var dt = _productBLL.SearchRecordByProductNumber(oldItemNumber.Trim());
                if (dt == null || dt.Rows.Count == 0)
                    return;

                // Read the new superseded_to_item_code field
                string supersededToCode = Convert.ToString(dt.Rows[0]["superseded_to_item_code"] ?? "");
                if (string.IsNullOrWhiteSpace(supersededToCode))
                {
                    lblAlreadySupersededTo.Text = "Not superseded";
                    lblAlreadySupersededTo.ForeColor = AppTheme.TextPrimary;
                    return;
                }

                // Look up the new item's details
                var mappedDt = _productBLL.SearchRecordByProductNumber(supersededToCode);
                if (mappedDt != null && mappedDt.Rows.Count > 0)
                {
                    string itemNum = Convert.ToString(mappedDt.Rows[0]["item_number"]);
                    string itemName = Convert.ToString(mappedDt.Rows[0]["name"]);
                    lblAlreadySupersededTo.Text = $"Already superseded to: {itemNum} ({itemName})";
                    lblAlreadySupersededTo.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    lblAlreadySupersededTo.Text = $"Already superseded to: {supersededToCode}";
                    lblAlreadySupersededTo.ForeColor = System.Drawing.Color.Orange;
                }
            }
            catch (Exception ex)
            {
                lblAlreadySupersededTo.Text = "Error loading supersession info";
                lblAlreadySupersededTo.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnSelectCompany_Click(object sender, EventArgs e)
        {
            using (var dlg = new frm_stock_suppression_companies(_selectedBranchIds))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                _selectedBranchIds.Clear();
                _selectedBranchIds.AddRange(dlg.SelectedBranchIds);
                txtCompany.Text = dlg.SelectedBranchText;
                lblBranchSummary.Text = _selectedBranchIds.Count + (_selectedBranchIds.Count == 1 ? " branch selected" : " branches selected");
            }
        }

        private void btnSupersede_Click(object sender, EventArgs e)
        {
            ExecuteSupersede();
        }

        private void ExecuteSupersede()
        {
            string oldPart = (txtOldPartCode.Text ?? string.Empty).Trim();
            string newPart = (txtNewPartCode.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(oldPart))
            {
                UiMessages.ShowWarning("Please select old part number.", "يرجى اختيار رقم القطعة القديمة.", captionEn: "Stock Suppression", captionAr: "استبدال المخزون");
                txtOldPartCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(newPart))
            {
                UiMessages.ShowWarning("Please select new part number.", "يرجى اختيار رقم القطعة الجديدة.", captionEn: "Stock Suppression", captionAr: "استبدال المخزون");
                txtNewPartCode.Focus();
                return;
            }

            if (string.Equals(oldPart, newPart, StringComparison.OrdinalIgnoreCase))
            {
                UiMessages.ShowWarning("Old and new part cannot be the same.", "لا يمكن أن يكون رقم القطعة القديمة والجديدة متطابقًا.", captionEn: "Stock Suppression", captionAr: "استبدال المخزون");
                return;
            }

            if (_selectedBranchIds.Count == 0)
            {
                UiMessages.ShowWarning("Please select at least one branch.", "يرجى اختيار فرع واحد على الأقل.", captionEn: "Stock Suppression", captionAr: "استبدال المخزون");
                return;
            }

            // Check if old part is already superseded
            var oldDt = _productBLL.SearchRecordByProductNumber(oldPart);
            if (oldDt != null && oldDt.Rows.Count > 0)
            {
                string existingSupersedeTo = Convert.ToString(oldDt.Rows[0]["superseded_to_item_code"] ?? "");
                if (!string.IsNullOrWhiteSpace(existingSupersedeTo))
                {
                    var confirmChain = UiMessages.ConfirmYesNo(
                        $"This item is already superseded to: {existingSupersedeTo}\n\nDo you want to create a chain supersession (old → intermediate → new)?",
                        $"هذا العنصر مستبدل بالفعل إلى: {existingSupersedeTo}\n\nهل تريد إنشاء تسلسل استبدال؟",
                        captionEn: "Chain Supersession",
                        captionAr: "تسلسل الاستبدال");

                    if (confirmChain != DialogResult.Yes)
                        return;
                }
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Stock Suppression Action:\n\n" +
                "• Old item will be marked as superseded (immutable history preserved)\n" +
                "• Stock will transfer to new item\n" +
                "• Forward link created for traceability\n\n" +
                "Do you want to proceed?",
                "إجراء استبدال المخزون:\n\n" +
                "• سيتم وضع علامة على العنصر القديم كمستبدل (التاريخ محفوظ)\n" +
                "• سيتم نقل المخزون إلى العنصر الجديد\n" +
                "• تم إنشاء ارتباط للتتبع\n\n" +
                "هل تريد المتابعة؟",
                captionEn: "Confirm Supersede",
                captionAr: "تأكيد الاستبدال");

            if (confirm != DialogResult.Yes)
                return;

            using (BusyScope.Show(this, UiMessages.T("Processing stock suppression...", "جاري تنفيذ استبدال المخزون...")))
            {
                try
                {
                    int result = _productBLL.ExecuteStockSuppression(
                        oldPart,
                        newPart,
                        _selectedBranchIds,
                        chkTransferStock.Checked,
                        chkZeroDemand.Checked,
                        chkTransferPartDescription.Checked,
                        chkResetReorder.Checked);

                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Stock suppression completed successfully.\n\n" +
                            "• Old item preserved with history\n" +
                            "• New item ready for future transactions\n" +
                            "• Supersession link established",
                            "تم تنفيذ استبدال المخزون بنجاح.\n\n" +
                            "• تم الحفاظ على العنصر القديم مع السجل\n" +
                            "• العنصر الجديد جاهز للمعاملات المستقبلية\n" +
                            "• تم إنشاء ارتباط الاستبدال",
                            captionEn: "Success",
                            captionAr: "نجاح");
                        
                        // Refresh the display
                        LoadAlreadySupersededTo(oldPart);
                        ClearNewPartSelection();
                    }
                    else
                    {
                        UiMessages.ShowWarning("No changes were saved.", "لم يتم حفظ أي تغييرات.", captionEn: "Warning", captionAr: "تنبيه");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private void ClearNewPartSelection()
        {
            txtNewPartCode.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            UiMessages.ShowInfo(
                "Stock Suppression Guide:\n\n" +
                "1. Type or select the OLD part number (item to be superseded)\n" +
                "2. Review if already superseded (if shown)\n" +
                "3. Select options (stock transfer, descriptions, etc.)\n" +
                "4. Select branch/company\n" +
                "5. Type or select the NEW part number (replacement item)\n" +
                "6. Click Supersede to complete\n\n" +
                "• You can TYPE the item code directly in the textboxes\n" +
                "• Or click SEARCH to browse and select\n" +
                "• Auto-search will find matching items as you type (500ms delay)\n" +
                "• Result:\n" +
                "  - Old item remains immutable with historical records\n" +
                "  - Stock transfers to new item\n" +
                "  - Traceability link established",
                "دليل استبدال المخزون:\n\n" +
                "1. اكتب أو اختر رقم القطعة القديمة (العنصر المراد استبداله)\n" +
                "2. راجع ما إذا كان مستبدلاً بالفعل\n" +
                "3. اختر الخيارات (نقل المخزون، الوصفات، إلخ)\n" +
                "4. اختر الفرع/الشركة\n" +
                "5. اكتب أو اختر رقم القطعة الجديدة (العنصر البديل)\n" +
                "6. انقر استبدال للإنهاء\n\n" +
                "• يمكنك كتابة رقم العنصر مباشرة في صناديق النص\n" +
                "• أو انقر فوق البحث للاستعراض والتحديد\n" +
                "• سيعثر البحث التلقائي على العناصر المطابقة أثناء الكتابة\n" +
                "• النتيجة:\n" +
                "  - يبقى العنصر القديم دون تغيير مع السجلات التاريخية\n" +
                "  - نقل المخزون إلى العنصر الجديد\n" +
                "  - تم إنشاء رابط القابلية للتتبع",
                captionEn: "Stock Suppression Help",
                captionAr: "مساعدة استبدال المخزون");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _oldPartDebounceTimer?.Dispose();
            _newPartDebounceTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
