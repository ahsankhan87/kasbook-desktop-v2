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
            _header = header;
            _lines = lines;
            BuildUi();
            LoadData();
        }

        private void BuildUi()
        {
            Text = "Create Reversal Entry";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ClientSize = new Size(860, 620);
            MinimumSize = new Size(820, 580);

            var top = new Panel { Dock = DockStyle.Top, Height = 104, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(12) };
            _lblVoucher = new Label { AutoSize = true, Font = new Font(Font, FontStyle.Bold), Location = new Point(14, 12), Text = "Voucher: -" };
            _lblDate = new Label { AutoSize = true, Font = new Font(Font, FontStyle.Bold), Location = new Point(14, 36), Text = "Date: -" };
            var lblReversalDate = new Label { AutoSize = true, Location = new Point(14, 64), Text = "Reversal Date" };
            _dtpReversalDate = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Location = new Point(106, 60), Width = 120 };
            var lblReason = new Label { AutoSize = true, Location = new Point(248, 64), Text = "Reason" };
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
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Account Code", Name = "AccountCode", ReadOnly = true, Width = 100 });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Account Name", Name = "AccountName", ReadOnly = true, Width = 190 });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", Name = "Description", ReadOnly = true });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Debit", Name = "Debit", ReadOnly = true, Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Credit", Name = "Credit", ReadOnly = true, Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });

            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 92, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(12) };
            _lblDr = new Label { AutoSize = true, Location = new Point(16, 16), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = "Total Debit: 0.00" };
            _lblCr = new Label { AutoSize = true, Location = new Point(16, 38), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = "Total Credit: 0.00" };
            _lblBalance = new Label { AutoSize = true, Location = new Point(16, 60), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = "Balanced ✓" };
            _btnCreate = new Button { Text = "Create Reversal", BackColor = Color.FromArgb(192, 57, 43), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Width = 140, Height = 32, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            _btnCancel = new Button { Text = "Cancel", Width = 90, Height = 32, Anchor = AnchorStyles.Right | AnchorStyles.Top };
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
            _lblVoucher.Text = string.Format("Voucher: {0}", invoiceNo);
            _lblDate.Text = string.Format("Date: {0:d}", voucherDate);

            _gridPreview.Rows.Clear();
            decimal totalDr = 0m;
            decimal totalCr = 0m;
            foreach (DataRow line in _lines.Rows)
            {
                decimal debit = line["Debit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Debit"]);
                decimal credit = line["Credit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Credit"]);
                decimal reversedDebit = credit;
                decimal reversedCredit = debit;
                totalDr += reversedDebit;
                totalCr += reversedCredit;
                _gridPreview.Rows.Add(Convert.ToString(line["AccountCode"]), Convert.ToString(line["AccountName"]), Convert.ToString(line["Description"]), reversedDebit, reversedCredit);
            }

            _lblDr.Text = string.Format("Total Debit: {0:N2}", totalDr);
            _lblCr.Text = string.Format("Total Credit: {0:N2}", totalCr);
            bool balanced = Math.Abs(totalDr - totalCr) < 0.005m;
            _lblBalance.Text = balanced ? "Balanced ✓" : "Not Balanced";
            _lblBalance.ForeColor = balanced ? Color.DarkGreen : Color.DarkRed;
        }

        private void ConfirmDialog()
        {
            if (string.IsNullOrWhiteSpace(_txtReason.Text))
            {
                MessageBox.Show(this, "Reason for reversal is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_header == null)
            {
                MessageBox.Show(this, "Voucher header is missing.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int voucherId = _header.Table.Columns.Contains("id") && _header["id"] != DBNull.Value ? Convert.ToInt32(_header["id"]) : 0;
            if (voucherId <= 0)
            {
                MessageBox.Show(this, "Voucher id is missing.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
