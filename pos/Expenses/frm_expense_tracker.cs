using DGVPrinterHelper;
using pos.Reports.Common;
using pos.UI;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos.Expenses
{
    public partial class frm_expense_tracker : Form
    {
        private DataTable _gridData = new DataTable();
        private readonly Bitmap _attachmentIcon = SystemIcons.Information.ToBitmap();

        public frm_expense_tracker()
        {
            InitializeComponent();
        }

        private void frm_expense_tracker_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            InitializeGridColumns();

            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;

            LoadExpenseAccountsFilter();
            LoadPaymentModesFilter();
            LoadGrid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panelFilters, null, panelBody, gridExpenses);

            panelSummary.BackColor = AppTheme.Background;
            cardTotalExpenses.BackColor = Color.White;
            cardTotalTax.BackColor = Color.White;
            cardNetTotal.BackColor = Color.White;

            cardTotalExpenses.BorderStyle = BorderStyle.FixedSingle;
            cardTotalTax.BorderStyle = BorderStyle.FixedSingle;
            cardNetTotal.BorderStyle = BorderStyle.FixedSingle;

            lblTotalExpensesValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTotalTaxValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblNetTotalValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);

            btnExport.BackColor = AppTheme.Primary;
            btnExport.ForeColor = AppTheme.TextOnPrimary;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;

            btnPrint.BackColor = AppTheme.Primary;
            btnPrint.ForeColor = AppTheme.TextOnPrimary;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.FlatAppearance.BorderSize = 0;
        }

        private void InitializeGridColumns()
        {
            gridExpenses.AutoGenerateColumns = false;
            gridExpenses.Columns.Clear();
            gridExpenses.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gridExpenses.EnableHeadersVisualStyles = false;
            gridExpenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            gridExpenses.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            gridExpenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
            gridExpenses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            gridExpenses.RowTemplate.Height = 32;

            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colVoucherNo", HeaderText = "Voucher No", DataPropertyName = "VoucherNo", Width = 110 });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Date", DataPropertyName = "Date", Width = 95, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd-MMM-yyyy" } });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAccountName", HeaderText = "Account Name", DataPropertyName = "AccountName", Width = 190 });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescription", HeaderText = "Description", DataPropertyName = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPaymentMode", HeaderText = "Payment Mode", DataPropertyName = "PaymentMode", Width = 95 });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAmount", HeaderText = "Amount", DataPropertyName = "Amount", Width = 95, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTax", HeaderText = "Tax", DataPropertyName = "Tax", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNetAmount", HeaderText = "Net Amount", DataPropertyName = "NetAmount", Width = 105, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            gridExpenses.Columns.Add(new DataGridViewImageColumn { Name = "colAttachment", HeaderText = "Attachment", Width = 70, ImageLayout = DataGridViewImageCellLayout.Zoom });
            gridExpenses.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colPosted", HeaderText = "Posted", DataPropertyName = "Posted", Width = 70 });
            gridExpenses.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHasAttachment", DataPropertyName = "HasAttachment", Visible = false });
        }

        private void LoadExpenseAccountsFilter()
        {
            GeneralBLL bll = new GeneralBLL();
            DataTable dt = bll.GetRecord("A.id, A.name", "acc_accounts A INNER JOIN acc_groups G ON A.group_id = G.id WHERE A.branch_id = " + UsersModal.logged_in_branch_id + " AND G.name LIKE '%Expense%'");

            DataRow row = dt.NewRow();
            row["id"] = 0;
            row["name"] = "All";
            dt.Rows.InsertAt(row, 0);

            cmbExpenseAccount.DataSource = dt;
            cmbExpenseAccount.DisplayMember = "name";
            cmbExpenseAccount.ValueMember = "id";
        }

        private void LoadPaymentModesFilter()
        {
            cmbPaymentMode.Items.Clear();
            cmbPaymentMode.Items.Add("All");
            cmbPaymentMode.Items.Add("Cash");
            cmbPaymentMode.Items.Add("Bank");
            cmbPaymentMode.Items.Add("Credit");
            cmbPaymentMode.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            try
            {
                ExpenseBLL bll = new ExpenseBLL();

                int accountId = cmbExpenseAccount.SelectedValue == null ? 0 : Convert.ToInt32(cmbExpenseAccount.SelectedValue);
                string paymentMode = cmbPaymentMode.SelectedItem == null ? "All" : cmbPaymentMode.SelectedItem.ToString();
                string search = txtSearch.Text == null ? string.Empty : txtSearch.Text.Trim();

                _gridData = bll.GetExpenseTrackerList(dtpFrom.Value.Date, dtpTo.Value.Date, accountId, paymentMode, search);
                gridExpenses.DataSource = _gridData;

                foreach (DataGridViewRow row in gridExpenses.Rows)
                {
                    bool hasAttachment = false;
                    if (row.Cells["colHasAttachment"].Value != null)
                    {
                        bool.TryParse(row.Cells["colHasAttachment"].Value.ToString(), out hasAttachment);
                    }

                    row.Cells["colAttachment"].Value = hasAttachment ? _attachmentIcon : null;

                    string mode = Convert.ToString(row.Cells["colPaymentMode"].Value);
                    if (string.Equals(mode, "Bank", StringComparison.OrdinalIgnoreCase))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(238, 246, 255);
                    }
                    else if (string.Equals(mode, "Credit", StringComparison.OrdinalIgnoreCase))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 252, 230);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }

                RefreshSummary();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void RefreshSummary()
        {
            decimal totalAmount = 0;
            decimal totalTax = 0;
            decimal netTotal = 0;

            foreach (DataRow dr in _gridData.Rows)
            {
                totalAmount += dr["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Amount"]);
                totalTax += dr["Tax"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Tax"]);
                netTotal += dr["NetAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["NetAmount"]);
            }

            lblTotalExpensesValue.Text = totalAmount.ToString("N2");
            lblTotalTaxValue.Text = totalTax.ToString("N2");
            lblNetTotalValue.Text = netTotal.ToString("N2");
        }

        private string GetSelectedVoucherNo()
        {
            if (gridExpenses.CurrentRow == null)
            {
                return string.Empty;
            }

            return Convert.ToString(gridExpenses.CurrentRow.Cells["colVoucherNo"].Value);
        }

        private void OpenSelectedInEditMode()
        {
            string voucherNo = GetSelectedVoucherNo();
            if (string.IsNullOrWhiteSpace(voucherNo))
            {
                return;
            }

            using (frm_expenses frm = new frm_expenses(voucherNo))
            {
                frm.ShowDialog(this);
            }

            LoadGrid();
        }

        private void DeleteSelected()
        {
            string voucherNo = GetSelectedVoucherNo();
            if (string.IsNullOrWhiteSpace(voucherNo))
            {
                return;
            }

            var result = UiMessages.ConfirmYesNo(
                "Delete selected expense voucher?",
                "هل تريد حذف سند المصروف المحدد؟",
                "Confirm Delete",
                "تأكيد الحذف");

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                ExpenseBLL bll = new ExpenseBLL();
                int deleted = bll.DeleteByVoucher(voucherNo);
                if (deleted > 0)
                {
                    Log.LogAction("Expense Delete", "Deleted expense voucher: " + voucherNo, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void PrintFilteredGrid()
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Expense List";
            printer.SubTitle = string.Format("From {0} To {1}", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
            printer.PageNumbers = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(gridExpenses);
        }

        private void PrintSelectedVoucher()
        {
            string voucherNo = GetSelectedVoucherNo();
            if (string.IsNullOrWhiteSpace(voucherNo))
            {
                return;
            }

            DataRow[] rows = _gridData.Select("VoucherNo = '" + voucherNo.Replace("'", "''") + "'");
            if (rows.Length == 0)
            {
                return;
            }

            DataTable single = _gridData.Clone();
            single.ImportRow(rows[0]);

            using (DataGridView tempGrid = new DataGridView())
            {
                tempGrid.AutoGenerateColumns = true;
                tempGrid.DataSource = single;

                DGVPrinter printer = new DGVPrinter();
                printer.Title = "Expense Voucher";
                printer.SubTitle = "Voucher No: " + voucherNo;
                printer.PageNumbers = false;
                printer.PrintPreviewDataGridView(tempGrid);
            }
        }

        private void ShowJournalForSelected()
        {
            string voucherNo = GetSelectedVoucherNo();
            if (string.IsNullOrWhiteSpace(voucherNo))
            {
                return;
            }

            GeneralBLL bll = new GeneralBLL();
            DataTable dt = bll.GetRecord("invoice_no,entry_date,account_name,debit,credit,description", "acc_entries WHERE branch_id = " + UsersModal.logged_in_branch_id + " AND invoice_no = '" + voucherNo.Replace("'", "''") + "'");

            Form frm = new Form();
            frm.Text = "Journal Entry - " + voucherNo;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Size = new Size(900, 450);

            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DataSource = dt;
            frm.Controls.Add(dgv);

            frm.ShowDialog(this);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelExportHelper.ExportDataTableToExcel(_gridData, "expense_list_" + DateTime.Now.ToString("yyyyMMdd"), this, true);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintFilteredGrid();
        }

        private void gridExpenses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                OpenSelectedInEditMode();
            }
        }

        private void gridExpenses_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            var hit = gridExpenses.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0)
            {
                gridExpenses.ClearSelection();
                gridExpenses.Rows[hit.RowIndex].Selected = true;
                gridExpenses.CurrentCell = gridExpenses.Rows[hit.RowIndex].Cells[0];
            }
        }

        private void mnuViewDetails_Click(object sender, EventArgs e)
        {
            string voucherNo = GetSelectedVoucherNo();
            if (string.IsNullOrWhiteSpace(voucherNo)) return;

            ExpenseBLL bll = new ExpenseBLL();
            DataTable dt = bll.GetExpenseByVoucher(voucherNo);
            if (dt.Rows.Count == 0) return;

            DataRow row = dt.Rows[0];
            UiMessages.ShowInfo(
                "Voucher: " + voucherNo + "\nDate: " + Convert.ToDateTime(row["payment_date"]).ToShortDateString() + "\nAmount: " + Convert.ToDecimal(row["amount"]).ToString("N2") + "\nTax: " + Convert.ToDecimal(row["tax_amount"]).ToString("N2") + "\nDescription: " + Convert.ToString(row["description"]),
                "السند: " + voucherNo,
                "Expense Details",
                "تفاصيل المصروف");
        }

        private void mnuEdit_Click(object sender, EventArgs e)
        {
            OpenSelectedInEditMode();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void mnuPrintVoucher_Click(object sender, EventArgs e)
        {
            PrintSelectedVoucher();
        }

        private void mnuViewJournal_Click(object sender, EventArgs e)
        {
            ShowJournalForSelected();
        }
    }
}
