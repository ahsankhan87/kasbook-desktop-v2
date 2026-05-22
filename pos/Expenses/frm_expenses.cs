using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Expenses
{
    public partial class frm_expenses : Form
    {
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        private string _selectedAttachmentPath = string.Empty;
        private readonly string _editVoucherNo;

        public frm_expenses()
        {
            InitializeComponent();
        }

        public frm_expenses(string voucherNo) : this()
        {
            _editVoucherNo = voucherNo;
        }

        private void frm_expenses_Load(object sender, EventArgs e)
        {
            if (!_auth.HasPermission(_currentUser, Permissions.Expenses_Create))
            {
                UiMessages.ShowWarning(
                    "You do not have permission to access Expenses.",
                    "ليس لديك صلاحية للوصول إلى المصروفات.",
                    "Access Denied",
                    "تم رفض الوصول"
                );
            }

            try
            {
                AppTheme.Apply(this);
                ApplyCustomStyle();

                using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                {
                    LoadVoucherInfo();
                    LoadExpenseAccounts();
                    LoadVatAccounts();
                    LoadPaymentModes();
                    LoadCreditAccounts();
                    UpdateCreditAccountByMode();
                    CalculateTotals();

                    if (!string.IsNullOrWhiteSpace(_editVoucherNo))
                    {
                        LoadExpenseForEdit(_editVoucherNo);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void ApplyCustomStyle()
        {
            BackColor = AppTheme.Background;
            pnlHeader.BackColor = AppTheme.Background;
            pnlContent.BackColor = AppTheme.Background;
            pnlActions.BackColor = AppTheme.Background;

            btnSave.BackColor = AppTheme.Primary;
            btnSave.ForeColor = AppTheme.TextOnPrimary;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;

            btnClear.BackColor = Color.White;
            btnClear.ForeColor = AppTheme.TextPrimary;
            btnClose.BackColor = Color.White;
            btnClose.ForeColor = AppTheme.TextPrimary;
        }

        private void LoadVoucherInfo()
        {
            dtpVoucherDate.Value = DateTime.Now;
            var expenseBLL = new ExpenseBLL();
            txtVoucherNo.Text = expenseBLL.GetMaxInvoiceNo();
            txtReferenceNo.Text = string.Empty;
        }

        private void LoadExpenseAccounts()
        {
            var generalBLL = new GeneralBLL();
            var dt = generalBLL.GetRecord(
                "A.id, A.name",
                "acc_accounts A INNER JOIN acc_groups G ON A.group_id = G.id WHERE A.branch_id = " + UsersModal.logged_in_branch_id + " AND G.name LIKE '%Expense%'"
            );

            cmbExpenseAccount.DataSource = dt;
            cmbExpenseAccount.DisplayMember = "name";
            cmbExpenseAccount.ValueMember = "id";
        }

        private void LoadVatAccounts()
        {
            var generalBLL = new GeneralBLL();
            var dt = generalBLL.GetRecord(
                "id,name",
                "acc_accounts WHERE branch_id = " + UsersModal.logged_in_branch_id
            );

            cmbVatAccount.DataSource = dt;
            cmbVatAccount.DisplayMember = "name";
            cmbVatAccount.ValueMember = "id";

            var vatItem = cmbVatAccount.Items
                .Cast<DataRowView>()
                .FirstOrDefault(x =>
                    x["name"].ToString().IndexOf("vat", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x["name"].ToString().IndexOf("tax", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x["name"].ToString().IndexOf("ضريبة", StringComparison.OrdinalIgnoreCase) >= 0);

            if (vatItem != null)
            {
                cmbVatAccount.SelectedValue = vatItem["id"];
            }
        }

        private void LoadPaymentModes()
        {
            var modes = new List<string> { "Cash", "Bank", "Credit" };
            cmbPaymentMode.DataSource = modes;
        }

        private void LoadCreditAccounts()
        {
            var generalBLL = new GeneralBLL();
            var dt = generalBLL.GetRecord("id,name", "acc_accounts WHERE branch_id = " + UsersModal.logged_in_branch_id);

            cmbCreditAccount.DataSource = dt;
            cmbCreditAccount.DisplayMember = "name";
            cmbCreditAccount.ValueMember = "id";
        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCreditAccountByMode();
        }

        private void UpdateCreditAccountByMode()
        {
            if (cmbCreditAccount.Items.Count == 0 || cmbPaymentMode.SelectedItem == null)
            {
                return;
            }

            var selectedMode = cmbPaymentMode.SelectedItem.ToString();
            var filterText = selectedMode == "Cash" ? "cash" : selectedMode == "Bank" ? "bank" : "payable";

            var matchedItem = cmbCreditAccount.Items
                .Cast<DataRowView>()
                .FirstOrDefault(x => x["name"].ToString().IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0);

            if (matchedItem != null)
            {
                cmbCreditAccount.SelectedValue = matchedItem["id"];
            }
        }

        private void amountOrTax_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            var amount = Convert.ToDecimal(nudAmount.Value);
            var taxPercent = Convert.ToDecimal(nudTaxPercent.Value);
            var taxAmount = amount * taxPercent / 100m;
            var netTotal = amount + taxAmount;

            txtTaxAmount.Text = taxAmount.ToString("N2");
            txtNetTotal.Text = netTotal.ToString("N2");
        }

        private void btnAttachment_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                _selectedAttachmentPath = openFileDialog1.FileName;
                txtAttachment.Text = _selectedAttachmentPath;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_auth.HasPermission(_currentUser, Permissions.Expenses_Create))
                {
                    UiMessages.ShowWarning(
                        "You do not have permission to perform this action.",
                        "ليس لديك صلاحية لتنفيذ هذا الإجراء.",
                        "Access Denied",
                        "تم رفض الوصول"
                    );
                    return;
                }

                if (cmbExpenseAccount.SelectedValue == null)
                {
                    UiMessages.ShowInfo(
                        "Please select an expense account.",
                        "يرجى اختيار حساب المصروف.",
                        "Expenses",
                        "المصروفات"
                    );
                    return;
                }

                if (nudAmount.Value <= 0)
                {
                    UiMessages.ShowInfo(
                        "Please enter a valid amount.",
                        "يرجى إدخال مبلغ صحيح.",
                        "Expenses",
                        "المصروفات"
                    );
                    return;
                }

                if (nudTaxPercent.Value > 0 && cmbVatAccount.SelectedValue == null)
                {
                    UiMessages.ShowInfo(
                        "Please select a VAT account.",
                        "يرجى اختيار حساب الضريبة.",
                        "Expenses",
                        "المصروفات"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Save this expense voucher?",
                    "هل تريد حفظ سند المصروف؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Saving...", "جاري الحفظ...")))
                {
                    var amount = Convert.ToDouble(nudAmount.Value);
                    var taxRate = Convert.ToDouble(nudTaxPercent.Value);

                    var voucherNo = !string.IsNullOrWhiteSpace(_editVoucherNo)
                        ? _editVoucherNo
                        : (string.IsNullOrWhiteSpace(txtVoucherNo.Text)
                            ? new ExpenseBLL().GetMaxInvoiceNo()
                            : txtVoucherNo.Text.Trim());

                    var expenseBLL = new ExpenseBLL();

                    if (!string.IsNullOrWhiteSpace(_editVoucherNo))
                    {
                        expenseBLL.DeleteByVoucher(voucherNo);
                    }

                    var modelHeader = new List<ExpenseModal_Header>
                    {
                        new ExpenseModal_Header
                        {
                            cash_account = Convert.ToString(cmbCreditAccount.SelectedValue),
                            vat_account = cmbVatAccount.SelectedValue == null ? string.Empty : Convert.ToString(cmbVatAccount.SelectedValue),
                            invoice_no = voucherNo,
                            sale_date = dtpVoucherDate.Value.Date,
                            expense_account = Convert.ToString(cmbExpenseAccount.SelectedValue),
                            amount = amount,
                            vat = taxRate,
                            description = BuildDescription(),
                            expense_account_name = cmbExpenseAccount.Text
                        }
                    };

                    var saveId = expenseBLL.Insert(modelHeader);

                    if (saveId.ToString().Length > 0)
                    {
                        POS.DLL.Log.LogAction(
                            "Expense Save",
                            $"Expense voucher saved. Voucher: {voucherNo}, Ref: {txtReferenceNo.Text.Trim()}, Amount: {nudAmount.Value:N2}",
                            UsersModal.logged_in_userid,
                            UsersModal.logged_in_branch_id
                        );

                        tslLastSaved.Text = $"Last Saved: {voucherNo} at {DateTime.Now:g}";

                        UiMessages.ShowInfo(
                            !string.IsNullOrWhiteSpace(_editVoucherNo) ? "Expense voucher updated successfully." : "Expense voucher saved successfully.",
                            !string.IsNullOrWhiteSpace(_editVoucherNo) ? "تم تحديث سند المصروف بنجاح." : "تم حفظ سند المصروف بنجاح.",
                            "Success",
                            "نجاح"
                        );

                        ClearForm(keepDate: true, refreshVoucher: true);
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Expense voucher could not be saved. Please try again.",
                            "تعذر حفظ سند المصروف. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private string BuildDescription()
        {
            var narration = txtNarration.Text?.Trim() ?? string.Empty;
            var reference = txtReferenceNo.Text?.Trim() ?? string.Empty;
            var attachment = string.IsNullOrWhiteSpace(_selectedAttachmentPath) ? string.Empty : Path.GetFileName(_selectedAttachmentPath);
            var paymentMode = cmbPaymentMode.SelectedItem == null ? string.Empty : cmbPaymentMode.SelectedItem.ToString();

            var parts = new List<string>
            {
                narration,
                string.IsNullOrWhiteSpace(reference) ? string.Empty : "Ref: " + reference,
                string.IsNullOrWhiteSpace(paymentMode) ? string.Empty : "Mode: " + paymentMode,
                string.IsNullOrWhiteSpace(attachment) ? string.Empty : "Attachment: " + attachment
            };

            return string.Join(" | ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private void LoadExpenseForEdit(string voucherNo)
        {
            var bll = new ExpenseBLL();
            var dt = bll.GetExpenseByVoucher(voucherNo);
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            var row = dt.Rows[0];

            txtVoucherNo.Text = Convert.ToString(row["invoice_no"]);
            dtpVoucherDate.Value = Convert.ToDateTime(row["payment_date"]);

            var expenseAccount = Convert.ToString(row["account_code"]);
            if (!string.IsNullOrWhiteSpace(expenseAccount))
            {
                cmbExpenseAccount.SelectedValue = expenseAccount;
            }

            nudAmount.Value = Convert.ToDecimal(row["amount"]);
            nudTaxPercent.Value = Convert.ToDecimal(row["tax_rate"]);

            if (nudTaxPercent.Value > 0 && cmbVatAccount.Items.Count > 0)
            {
                var vatNameItem = cmbVatAccount.Items
                    .Cast<DataRowView>()
                    .FirstOrDefault(x =>
                        x["name"].ToString().IndexOf("vat", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        x["name"].ToString().IndexOf("tax", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        x["name"].ToString().IndexOf("ضريبة", StringComparison.OrdinalIgnoreCase) >= 0);
                if (vatNameItem != null)
                {
                    cmbVatAccount.SelectedValue = vatNameItem["id"];
                }
            }

            txtNarration.Text = Convert.ToString(row["description"]);
            ParseDescriptionFields(txtNarration.Text);

            btnSave.Text = "Update";
        }

        private void ParseDescriptionFields(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return;
            }

            var parts = description.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            var narration = parts.FirstOrDefault(x => !x.StartsWith("Ref:", StringComparison.OrdinalIgnoreCase)
                                                   && !x.StartsWith("Mode:", StringComparison.OrdinalIgnoreCase)
                                                   && !x.StartsWith("Attachment:", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(narration))
            {
                txtNarration.Text = narration;
            }

            var reference = parts.FirstOrDefault(x => x.StartsWith("Ref:", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(reference))
            {
                txtReferenceNo.Text = reference.Substring(4).Trim();
            }

            var mode = parts.FirstOrDefault(x => x.StartsWith("Mode:", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(mode))
            {
                var modeValue = mode.Substring(5).Trim();
                if (cmbPaymentMode.Items.Contains(modeValue))
                {
                    cmbPaymentMode.SelectedItem = modeValue;
                }
            }

            var attachment = parts.FirstOrDefault(x => x.StartsWith("Attachment:", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(attachment))
            {
                txtAttachment.Text = attachment.Substring(11).Trim();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm(keepDate: false, refreshVoucher: false);
        }

        private void ClearForm(bool keepDate, bool refreshVoucher)
        {
            if (!keepDate)
            {
                dtpVoucherDate.Value = DateTime.Now;
            }

            if (refreshVoucher)
            {
                txtVoucherNo.Text = new ExpenseBLL().GetMaxInvoiceNo();
            }

            txtReferenceNo.Text = string.Empty;
            cmbExpenseAccount.SelectedIndex = cmbExpenseAccount.Items.Count > 0 ? 0 : -1;
            cmbVatAccount.SelectedIndex = cmbVatAccount.Items.Count > 0 ? 0 : -1;
            cmbPaymentMode.SelectedIndex = cmbPaymentMode.Items.Count > 0 ? 0 : -1;
            nudAmount.Value = 0;
            nudTaxPercent.Value = 0;
            txtNarration.Text = string.Empty;
            _selectedAttachmentPath = string.Empty;
            txtAttachment.Text = string.Empty;
            CalculateTotals();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            var confirm = UiMessages.ConfirmYesNo(
                "Close this window?",
                "هل تريد إغلاق النافذة؟",
                captionEn: "Close",
                captionAr: "إغلاق"
            );

            if (confirm == DialogResult.Yes)
                this.Close();
        }

        private void frm_expenses_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                btnSave.PerformClick();
            }
            if (e.KeyData == Keys.Escape)
            {
                btnClose.PerformClick();
            }
        }
    }
}
