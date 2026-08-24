using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public class frm_journal_reversal : Form
    {
        private readonly DataRow _header;
        private readonly DataTable _lines;

        private Label _lblVoucher;
        private Label _lblDate;
        private DateTimePicker _dtpReversalDate;
        private TextBox _txtReason;
        private DataGridView _gridPreview;
        private Label _lblDr;
        private Label _lblCr;
        private Label _lblBalance;
        private Button _btnCreate;
        private Button _btnCancel;

        public DateTime ReversalDate => _dtpReversalDate.Value.Date;
        public string Reason => _txtReason.Text.Trim();

        public frm_journal_reversal(DataRow header, DataTable lines)
        {
            // Set RTL mode based on user language
            bool isArabic = string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);
            this.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            this.RightToLeftLayout = isArabic;

            _header = header;
            _lines = lines;
            BuildUi();
            LoadData();
        }

        /// <summary>
        /// Translates text based on current user language (Arabic/English).
        /// </summary>
        private string T(string englishText, string arabicText)
        {
            return string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase) 
                ? arabicText 
                : englishText;
        }

        private void BuildUi()
        {
            Text = T("Create Reversal Entry", "إنشاء قيد معاكس");
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ClientSize = new Size(860, 620);
            MinimumSize = new Size(820, 580);

            var top = new Panel { Dock = DockStyle.Top, Height = 104, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(12) };
            _lblVoucher = new Label { AutoSize = true, Font = new Font(Font, FontStyle.Bold), Location = new Point(14, 12), Text = T("Voucher: -", "الكشف: -") };
            _lblDate = new Label { AutoSize = true, Font = new Font(Font, FontStyle.Bold), Location = new Point(14, 36), Text = T("Date: -", "التاريخ: -") };
            var lblReversalDate = new Label { AutoSize = true, Location = new Point(14, 64), Text = T("Reversal Date", "تاريخ المعاكسة") };
            _dtpReversalDate = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Location = new Point(106, 60), Width = 120 };
            var lblReason = new Label { AutoSize = true, Location = new Point(248, 64), Text = T("Reason", "السبب") };
            _txtReason = new TextBox { Location = new Point(298, 59), Width = 530, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

            top.Controls.Add(_lblVoucher);
            top.Controls.Add(_lblDate);
            top.Controls.Add(lblReversalDate);
            top.Controls.Add(_dtpReversalDate);
            top.Controls.Add(lblReason);
            top.Controls.Add(_txtReason);

            _gridPreview = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            _gridPreview.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            _gridPreview.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _gridPreview.ColumnHeadersHeight = 30;
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = T("Account Code", "رمز الحساب"), Name = "AccountCode", ReadOnly = true, Width = 100 });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = T("Account Name", "اسم الحساب"), Name = "AccountName", ReadOnly = true, Width = 190 });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = T("Description", "الوصف"), Name = "Description", ReadOnly = true });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = T("Debit", "مدين"), Name = "Debit", ReadOnly = true, Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = T("Credit", "دائن"), Name = "Credit", ReadOnly = true, Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });

            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 92, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(12) };
            _lblDr = new Label { AutoSize = true, Location = new Point(16, 16), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = T("Total Debit: 0.00", "إجمالي المدين: 0.00") };
            _lblCr = new Label { AutoSize = true, Location = new Point(16, 38), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = T("Total Credit: 0.00", "إجمالي الدائن: 0.00") };
            _lblBalance = new Label { AutoSize = true, Location = new Point(16, 60), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = T("Balanced ✓", "متوازن ✓") };
            _btnCreate = new Button { Text = T("Create Reversal", "إنشاء معاكسة"), BackColor = Color.FromArgb(192, 57, 43), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Width = 140, Height = 32, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            _btnCancel = new Button { Text = T("Cancel", "إلغاء"), Width = 90, Height = 32, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            _btnCreate.Location = new Point(ClientSize.Width - 170, 26);
            _btnCancel.Location = new Point(ClientSize.Width - 270, 26);
            _btnCreate.Click += (s, e) => ConfirmDialog();
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            bottom.Resize += (s, e) =>
            {
                _btnCreate.Left = bottom.ClientSize.Width - 154;
                _btnCancel.Left = bottom.ClientSize.Width - 252;
                _btnCreate.Top = 24;
                _btnCancel.Top = 24;
            };

            bottom.Controls.Add(_lblDr);
            bottom.Controls.Add(_lblCr);
            bottom.Controls.Add(_lblBalance);
            bottom.Controls.Add(_btnCreate);
            bottom.Controls.Add(_btnCancel);

            Controls.Add(_gridPreview);
            Controls.Add(bottom);
            Controls.Add(top);
        }

        private void LoadData()
        {
            string invoiceNo = Convert.ToString(_header["VoucherNo"]);
            DateTime voucherDate = _header["VoucherDate"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(_header["VoucherDate"]);
            _lblVoucher.Text = string.Format(T("Voucher: {0}", "الكشف: {0}"), invoiceNo);
            _lblDate.Text = string.Format(T("Date: {0:d}", "التاريخ: {0:d}"), voucherDate);

            _gridPreview.Rows.Clear();
            decimal totalDr = 0m;
            decimal totalCr = 0m;

            // Detect if Arabic mode for loading account names
            bool isArabic = string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);

            foreach (DataRow line in _lines.Rows)
            {
                decimal debit = line["Debit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Debit"]);
                decimal credit = line["Credit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Credit"]);
                decimal reversedDebit = credit;
                decimal reversedCredit = debit;
                totalDr += reversedDebit;
                totalCr += reversedCredit;

                // Load account name based on language mode
                string accountName = Convert.ToString(line["AccountName"]);
                if (isArabic && line.Table.Columns.Contains("AccountName_2"))
                {
                    string accountName2 = Convert.ToString(line["AccountName_2"]);
                    if (!string.IsNullOrWhiteSpace(accountName2))
                        accountName = accountName2;
                }

                _gridPreview.Rows.Add(Convert.ToString(line["AccountCode"]), accountName, Convert.ToString(line["Description"]), reversedDebit, reversedCredit);
            }

            _lblDr.Text = string.Format(T("Total Debit: {0:N2}", "إجمالي المدين: {0:N2}"), totalDr);
            _lblCr.Text = string.Format(T("Total Credit: {0:N2}", "إجمالي الدائن: {0:N2}"), totalCr);
            bool balanced = Math.Abs(totalDr - totalCr) < 0.005m;
            _lblBalance.Text = balanced ? T("Balanced ✓", "متوازن ✓") : T("Not Balanced", "غير متوازن");
            _lblBalance.ForeColor = balanced ? Color.DarkGreen : Color.DarkRed;
        }

        private void ConfirmDialog()
        {
            if (string.IsNullOrWhiteSpace(_txtReason.Text))
            {
                MessageBox.Show(this, T("Reason for reversal is required.", "السبب مطلوب لإنشاء المعاكسة."), T("Validation", "التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_header == null)
            {
                MessageBox.Show(this, T("Voucher header is missing.", "بيانات رأس الكشف غير موجودة."), T("Validation", "التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int voucherId = _header.Table.Columns.Contains("id") && _header["id"] != DBNull.Value ? Convert.ToInt32(_header["id"]) : 0;
            if (voucherId <= 0)
            {
                MessageBox.Show(this, T("Voucher id is missing.", "رقم الكشف غير موجود."), T("Validation", "التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //JournalsBLL bll = new JournalsBLL();
            //PostResult result = bll.ReverseJournalVoucher(voucherId, ReversalDate, Reason, UsersModal.logged_in_userid);
            //if (!result.Success)
            //{
            //    string message = result.Messages.Count > 0 ? string.Join("\r\n", result.Messages.Select(x => x.Message)) : "Unable to create reversal.";
            //    MessageBox.Show(this, message, "Reversal Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //MessageBox.Show(this, string.Format("Reversal posted successfully. Voucher No: {0}", result.VoucherNo), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
