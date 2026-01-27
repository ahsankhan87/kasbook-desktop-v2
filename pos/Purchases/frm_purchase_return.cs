using pos.UI;
using pos.UI.Busy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;
using POS.Core;

namespace pos
{
    public partial class frm_purchase_return : Form
    {
        PurchasesBLL objPurchaseBLL = new PurchasesBLL();
        DataTable Purchases_dt;

        public int cash_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        private CheckBox _chkHeader;
        private bool _bulkChecking;

        public frm_purchase_return()
        {
            InitializeComponent();
            Get_AccountID_From_Company();
        }

        public void frm_purchase_return_Load(object sender, EventArgs e)
        {
            LoadReturnReasonsDDL();
            autoCompleteInvoice();

            SetupHeaderCheckBox();
        }

        private void SetupHeaderCheckBox()
        {
            if (grid_purchase_return == null) return;
            if (!grid_purchase_return.Columns.Contains("chk")) return;
            if (_chkHeader != null) return;

            _chkHeader = new CheckBox();
            _chkHeader.Size = new Size(15, 15);
            _chkHeader.BackColor = Color.Transparent;
            _chkHeader.Checked = false;
            _chkHeader.Click += HeaderCheckBox_Click;

            grid_purchase_return.Controls.Add(_chkHeader);

            PositionHeaderCheckBox();
            grid_purchase_return.ColumnWidthChanged += (s, e) => PositionHeaderCheckBox();
            grid_purchase_return.Scroll += (s, e) => PositionHeaderCheckBox();

            grid_purchase_return.CellValueChanged += grid_purchase_return_CellValueChanged;
            grid_purchase_return.CurrentCellDirtyStateChanged += grid_purchase_return_CurrentCellDirtyStateChanged;
        }

        private void PositionHeaderCheckBox()
        {
            if (_chkHeader == null) return;
            if (!grid_purchase_return.Columns.Contains("chk")) return;

            Rectangle rect = grid_purchase_return.GetCellDisplayRectangle(grid_purchase_return.Columns["chk"].Index, -1, true);
            int x = rect.X + (rect.Width - _chkHeader.Width) / 2;
            int y = rect.Y + (rect.Height - _chkHeader.Height) / 2;
            _chkHeader.Location = new Point(Math.Max(0, x), Math.Max(0, y));
        }

        private void HeaderCheckBox_Click(object sender, EventArgs e)
        {
            if (_bulkChecking) return;

            try
            {
                _bulkChecking = true;
                bool check = _chkHeader.Checked;

                foreach (DataGridViewRow row in grid_purchase_return.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (row.ReadOnly) continue;

                    row.Cells["chk"].Value = check;
                }

                grid_purchase_return.EndEdit();
            }
            finally
            {
                _bulkChecking = false;
            }
        }

        private void grid_purchase_return_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grid_purchase_return.CurrentCell is DataGridViewCheckBoxCell)
                grid_purchase_return.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void grid_purchase_return_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_bulkChecking) return;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;

            if (grid_purchase_return.Columns[e.ColumnIndex].Name != "chk")
                return;

            bool allChecked = true;
            bool anyChecked = false;

            foreach (DataGridViewRow row in grid_purchase_return.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.ReadOnly) continue;

                bool isChecked = false;
                if (row.Cells["chk"].Value != null)
                    isChecked = Convert.ToBoolean(row.Cells["chk"].Value);

                anyChecked |= isChecked;
                allChecked &= isChecked;
            }

            try
            {
                _bulkChecking = true;
                _chkHeader.Checked = anyChecked && allChecked;
            }
            finally
            {
                _bulkChecking = false;
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            var invoiceNo = txt_invoice_no.Text.Trim();
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                UiMessages.ShowWarning(
                    "Please enter an invoice number to search.",
                    "يرجى إدخال رقم الفاتورة للبحث.",
                    captionEn: "Purchase Return",
                    captionAr: "مرتجع مشتريات");
                txt_invoice_no.Focus();
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Loading invoice items...", "جارٍ تحميل أصناف الفاتورة...")))
            {
                try
                {
                    grid_purchase_return.DataSource = null;
                    grid_purchase_return.Refresh();
                    grid_purchase_return.AutoGenerateColumns = false;

                    grid_purchase_return.DataSource = objPurchaseBLL.GetReturnPurchaseItems(invoiceNo);
                    Purchases_dt = load_purchase_return_grid(invoiceNo);

                    MarkFullyReturnedRows();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Unable to load invoice items.\n" + ex.Message,
                        "تعذر تحميل أصناف الفاتورة.\n" + ex.Message,
                        captionEn: "Purchase Return",
                        captionAr: "مرتجع مشتريات");
                }
            }
        }

        private void MarkFullyReturnedRows()
        {
            foreach (DataGridViewRow row in grid_purchase_return.Rows)
            {
                decimal avail = 0;
                if (row.Cells["ReturnableQty"].Value != null)
                    decimal.TryParse(row.Cells["ReturnableQty"].Value.ToString(), out avail);

                if (avail <= 0)
                {
                    row.ReadOnly = true;
                    row.Cells["ReturnQty"].Value = 0m;
                    row.Cells["chk"].Value = false;
                }
            }
            ApplyRowStyles();
        }

        private void ApplyRowStyles()
        {
            foreach (DataGridViewRow row in grid_purchase_return.Rows)
            {
                decimal avail = 0;
                if (row.Cells["ReturnableQty"].Value != null)
                    decimal.TryParse(row.Cells["ReturnableQty"].Value.ToString(), out avail);

                if (avail <= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }
        }

        private bool TryGetSelectedReturnLines(out int selectedCount, out string validationMessageEn, out string validationMessageAr)
        {
            selectedCount = 0;
            validationMessageEn = null;
            validationMessageAr = null;

            if (grid_purchase_return.Rows.Count == 0)
            {
                validationMessageEn = "Please search for an invoice first.";
                validationMessageAr = "يرجى البحث عن فاتورة أولاً.";
                return false;
            }

            foreach (DataGridViewRow row in grid_purchase_return.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.ReadOnly) continue;

                bool isChecked = row.Cells["chk"].Value != null && Convert.ToBoolean(row.Cells["chk"].Value);
                if (!isChecked) continue;

                decimal avail = 0m;
                decimal entered = 0m;

                if (row.Cells["ReturnableQty"].Value != null)
                    decimal.TryParse(row.Cells["ReturnableQty"].Value.ToString(), out avail);

                if (row.Cells["ReturnQty"].Value != null)
                    decimal.TryParse(row.Cells["ReturnQty"].Value.ToString(), out entered);

                if (entered <= 0)
                {
                    validationMessageEn = "Return quantity must be greater than zero for all selected items.";
                    validationMessageAr = "يجب أن تكون كمية الإرجاع أكبر من صفر لجميع الأصناف المحددة.";
                    return false;
                }

                if (entered > avail)
                {
                    validationMessageEn = $"Return quantity cannot exceed the available quantity ({avail}).";
                    validationMessageAr = $"لا يمكن أن تتجاوز كمية الإرجاع الكمية المتاحة ({avail}).";
                    return false;
                }

                selectedCount++;
            }

            if (selectedCount == 0)
            {
                validationMessageEn = "Please select at least one item to return.";
                validationMessageAr = "يرجى اختيار صنف واحد على الأقل للإرجاع.";
                return false;
            }

            return true;
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            var prev_invoice_no = txt_invoice_no.Text.Trim();
            if (string.IsNullOrWhiteSpace(prev_invoice_no))
            {
                UiMessages.ShowWarning(
                    "Please enter and search an invoice number before saving a return.",
                    "يرجى إدخال رقم الفاتورة والبحث عنها قبل حفظ المرتجع.",
                    captionEn: "Purchase Return",
                    captionAr: "مرتجع مشتريات");
                return;
            }

            if (!TryGetSelectedReturnLines(out var selectedCount, out var msgEn, out var msgAr))
            {
                UiMessages.ShowWarning(msgEn, msgAr, captionEn: "Purchase Return", captionAr: "مرتجع مشتريات");
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Do you want to save this purchase return transaction?",
                "هل تريد حفظ معاملة مرتجع المشتريات؟",
                captionEn: "Confirm",
                captionAr: "تأكيد",
                defaultButton: MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            using (BusyScope.Show(this, UiMessages.T("Saving return...", "جارٍ حفظ المرتجع...")))
            {
                try
                {
                    string returnReason = ((KeyValuePair<string, string>)cmbReturnReason.SelectedItem).Value;
                    string new_invoice_no = GetMAXInvoiceNo();

                    decimal total_tax = 0;
                    decimal total_amount = 0;
                    decimal total_discount = 0;
                    decimal sub_total = 0;

                    // Calculate totals from selected rows
                    for (int i = 0; i < grid_purchase_return.RowCount; i++)
                    {
                        if (grid_purchase_return.Rows[i].IsNewRow) continue;
                        if (grid_purchase_return.Rows[i].ReadOnly) continue;

                        if (grid_purchase_return.Rows[i].Cells["chk"].Value != null && Convert.ToBoolean(grid_purchase_return.Rows[i].Cells["chk"].Value))
                        {
                            decimal tax_rate = (grid_purchase_return.Rows[i].Cells["tax_rate"].Value == null || grid_purchase_return.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchase_return.Rows[i].Cells["tax_rate"].Value.ToString()));
                            decimal returnQty = Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["ReturnQty"].Value);
                            decimal costPrice = Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["cost_price"].Value);
                            decimal discountVal = Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["discount_value"].Value);

                            sub_total = (returnQty * costPrice - discountVal);
                            decimal tax = (sub_total * tax_rate / 100);

                            total_tax += tax;
                            total_amount += sub_total + discountVal;
                            total_discount += discountVal;
                        }
                    }

                    List<PurchaseModalHeader> purchase_model_header = new List<PurchaseModalHeader>();
                    List<PurchasesModal> purchase_model_detail = new List<PurchasesModal>();

                    // Header from original invoice data
                    if (Purchases_dt == null || Purchases_dt.Rows.Count == 0)
                    {
                        UiMessages.ShowError(
                            "Unable to save return because the original invoice header could not be loaded. Please search again.",
                            "تعذر حفظ المرتجع لأن بيانات رأس الفاتورة الأصلية غير متاحة. يرجى البحث مرة أخرى.",
                            captionEn: "Purchase Return",
                            captionAr: "مرتجع مشتريات");
                        return;
                    }

                    foreach (DataRow Purchases_dr in Purchases_dt.Rows)
                    {
                        purchase_model_header.Add(new PurchaseModalHeader
                        {
                            supplier_id = (Purchases_dr["supplier_id"].ToString() == string.Empty ? 0 : int.Parse(Purchases_dr["supplier_id"].ToString())),
                            employee_id = (Purchases_dr["employee_id"].ToString() == string.Empty ? 0 : int.Parse(Purchases_dr["employee_id"].ToString())),
                            invoice_no = new_invoice_no,
                            total_amount = total_amount,
                            total_tax = Math.Round(total_tax, 6),
                            total_discount = total_discount,
                            purchase_type = Purchases_dr["purchase_type"].ToString(),
                            purchase_date = DateTime.Now,
                            purchase_time = DateTime.Now,
                            description = "Purchase Return Inv #:" + prev_invoice_no,
                            account = "Return",
                            old_invoice_no = prev_invoice_no,
                            returnReason = returnReason,

                            cash_account_id = cash_account_id,
                            payable_account_id = payable_account_id,
                            tax_account_id = tax_account_id,
                            purchases_discount_acc_id = purchases_discount_acc_id,
                            inventory_acc_id = inventory_acc_id,
                            purchases_acc_id = purchases_acc_id,
                        });
                    }

                    for (int i = 0; i < grid_purchase_return.Rows.Count; i++)
                    {
                        var row = grid_purchase_return.Rows[i];
                        if (row.IsNewRow) continue;
                        if (row.ReadOnly) continue;

                        if (row.Cells["id"].Value != null && row.Cells["chk"].Value != null && Convert.ToBoolean(row.Cells["chk"].Value))
                        {
                            purchase_model_detail.Add(new PurchasesModal
                            {
                                invoice_no = new_invoice_no,
                                item_number = row.Cells["item_number"].Value.ToString(),
                                code = row.Cells["item_code"].Value.ToString(),
                                quantity = decimal.Parse(row.Cells["ReturnQty"].Value.ToString()),
                                packet_qty = decimal.Parse(row.Cells["packet_qty"].Value.ToString()),
                                cost_price = Convert.ToDecimal(row.Cells["cost_price"].Value.ToString()),
                                unit_price = decimal.Parse(row.Cells["unit_price"].Value.ToString()),
                                discount = decimal.Parse(row.Cells["discount_value"].Value.ToString()),
                                tax_id = Convert.ToInt16(row.Cells["tax_id"].Value.ToString()),
                                tax_rate = Convert.ToDecimal(row.Cells["tax_rate"].Value.ToString()),
                                purchase_date = DateTime.Now,
                                location_code = row.Cells["loc_code"].Value.ToString()
                            });
                        }
                    }

                    int Purchase_id = objPurchaseBLL.InsertReturnPurchase(purchase_model_header, purchase_model_detail);

                    if (Purchase_id > 0)
                    {
                        UiMessages.ShowInfo(
                            "Purchase return has been saved successfully.",
                            "تم حفظ مرتجع المشتريات بنجاح.",
                            captionEn: "Purchase Return",
                            captionAr: "مرتجع مشتريات");

                        grid_purchase_return.DataSource = null;
                        grid_purchase_return.Rows.Clear();
                        grid_purchase_return.Refresh();
                        txt_invoice_no.Clear();
                        txt_invoice_no.Focus();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "The purchase return could not be saved. Please try again.",
                            "تعذر حفظ مرتجع المشتريات. يرجى المحاولة مرة أخرى.",
                            captionEn: "Purchase Return",
                            captionAr: "مرتجع مشتريات");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "An error occurred while saving the return.\n" + ex.Message,
                        "حدث خطأ أثناء حفظ المرتجع.\n" + ex.Message,
                        captionEn: "Purchase Return",
                        captionAr: "مرتجع مشتريات");
                }
            }
        }

        private int Insert_Journal_entry(string invoice_no, int account_id, decimal debit, decimal credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = Convert.ToDouble(debit);
            JournalsModal_obj.credit = Convert.ToDouble(credit);
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.customer_id = customer_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.entry_id = entry_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
            return journal_id;
        }

        private void Get_AccountID_From_Company()
        {
            GeneralBLL objBLL = new GeneralBLL();

            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                cash_account_id = (int)dr["cash_acc_id"];
                payable_account_id = (int)dr["payable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                purchases_discount_acc_id = (int)dr["purchases_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        public DataTable load_Purchases_items_return_grid(string invoice_no)
        {
            DataTable dt = objPurchaseBLL.GetReturnPurchaseItems(invoice_no);
            return dt;

        }

        public DataTable load_purchase_return_grid(string invoice_no)
        {
            DataTable dt = objPurchaseBLL.GetReturnPurchase(invoice_no);
            return dt;

        }

        public string GetMAXInvoiceNo()
        {
            PurchasesBLL PurchasesBLL_obj = new PurchasesBLL();
            return PurchasesBLL_obj.GetMaxReturnInvoiceNo();
        }

        public void autoCompleteInvoice()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading invoices...", "جارٍ تحميل الفواتير...")))
                {
                    txt_invoice_no.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txt_invoice_no.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                    GeneralBLL invoicesBLL_obj = new GeneralBLL();
                    string keyword = "TOP 500 invoice_no ";
                    string table = "pos_purchases WHERE account <> 'return'  AND branch_id=" + UsersModal.logged_in_branch_id + " ORDER BY id desc";
                    DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            coll.Add(dr["invoice_no"].ToString());

                        }

                    }

                    txt_invoice_no.AutoCompleteCustomSource = coll;
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

        private void frm_purchase_return_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }

            if (e.KeyCode == Keys.F3)
            {
                btn_return.PerformClick();
            }
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_purchase_return_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            UiMessages.ShowError(
                "Invalid value detected in the grid. Please review the entered data.",
                "تم إدخال قيمة غير صالحة في الجدول. يرجى مراجعة البيانات المدخلة.",
                captionEn: "Purchase Return",
                captionAr: "مرتجع مشتريات");

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "Invalid value.";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "Invalid value.";

                anError.ThrowException = false;
            }
        }

        private void grid_purchase_return_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (grid_purchase_return.Columns[e.ColumnIndex].Name == "ReturnQty")
            {
                var row = grid_purchase_return.Rows[e.RowIndex];
                if (row.ReadOnly) return;

                decimal avail = Convert.ToDecimal(row.Cells["ReturnableQty"].Value);
                if (string.IsNullOrWhiteSpace(e.FormattedValue?.ToString()))
                {
                    row.Cells["ReturnQty"].Value = 0m;
                    return;
                }

                if (!decimal.TryParse(e.FormattedValue.ToString(), out var entered) || entered < 0)
                {
                    e.Cancel = true;
                    UiMessages.ShowWarning(
                        "Please enter a valid quantity.",
                        "يرجى إدخال كمية صحيحة.",
                        captionEn: "Purchase Return",
                        captionAr: "مرتجع مشتريات");
                    return;
                }
                if (entered > avail)
                {
                    e.Cancel = true;
                    UiMessages.ShowWarning(
                        $"Return quantity cannot exceed the available quantity ({avail}).",
                        $"لا يمكن أن تتجاوز كمية الإرجاع الكمية المتاحة ({avail}).",
                        captionEn: "Purchase Return",
                        captionAr: "مرتجع مشتريات");
                }
            }
        }

        private void LoadReturnReasonsDDL()
        {
            var reasons = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("01", "Goods returned"),
                new KeyValuePair<string, string>("02", "Invoice correction"),
                new KeyValuePair<string, string>("03", "Service not provided"),
                new KeyValuePair<string, string>("04", "Duplicate invoice"),
                new KeyValuePair<string, string>("05", "Incorrect amount"),
                new KeyValuePair<string, string>("06", "Cancellation of order"),
                new KeyValuePair<string, string>("07", "Price adjustment"),
                new KeyValuePair<string, string>("08", "Damaged goods"),
                new KeyValuePair<string, string>("09", "Incorrect tax calculation"),
                new KeyValuePair<string, string>("10", "Other")
            };

            cmbReturnReason.DataSource = new BindingSource(reasons, null);
            cmbReturnReason.DisplayMember = "Value";
            cmbReturnReason.ValueMember = "Key";

            cmbReturnReason.SelectedIndex = 0;
        }

    }
}
