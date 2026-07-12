using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using pos.Reports.Common;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Financial
{
    public partial class frm_BankReconciliation : Form
    {
        private readonly BankBLL _bankBll = new BankBLL();
        private DataTable _transactions = new DataTable();
        private DataTable _uncleared = new DataTable();

        public frm_BankReconciliation()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += Frm_BankReconciliation_Load;
            btnLoad.Click += (s, e) => LoadReconciliation();
            btnSave.Click += (s, e) => SaveReconciliation();
            btnPrint.Click += (s, e) => PrintReconciliation();

            dgvSystemTransactions.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvSystemTransactions.IsCurrentCellDirty)
                {
                    dgvSystemTransactions.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
            dgvSystemTransactions.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    RefreshSummary();
                }
            };
            txtStatementBalance.TextChanged += (s, e) => RefreshSummary();
            dtpStatementDate.ValueChanged += (s, e) => LoadReconciliation();
        }

        private void Frm_BankReconciliation_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            ConfigureGrids();
            LoadBankAccounts();

            if (cmbBankAccount.Items.Count > 0)
            {
                cmbBankAccount.SelectedIndex = 0;
                LoadReconciliation();
            }
        }

        private void ConfigureGrids()
        {
            dgvSystemTransactions.AutoGenerateColumns = false;
            dgvUncleared.AutoGenerateColumns = false;

            ApplyNumericColumnStyle(dgvSystemTransactions, "colDebit");
            ApplyNumericColumnStyle(dgvSystemTransactions, "colCredit");
            ApplyNumericColumnStyle(dgvSystemTransactions, "colAmount");
            ApplyNumericColumnStyle(dgvUncleared, "colUnclearedDebit");
            ApplyNumericColumnStyle(dgvUncleared, "colUnclearedCredit");
            ApplyNumericColumnStyle(dgvUncleared, "colUnclearedAmount");
        }

        private static void ApplyNumericColumnStyle(DataGridView grid, string columnName)
        {
            if (grid == null || !grid.Columns.Contains(columnName))
            {
                return;
            }

            DataGridViewColumn column = grid.Columns[columnName];
            column.DefaultCellStyle.Format = "N2";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void LoadBankAccounts()
        {
            try
            {
                DataTable accounts = _bankBll.GetBankAccountsForReconciliation();
                cmbBankAccount.DisplayMember = "bank_name";
                cmbBankAccount.ValueMember = "account_id";
                cmbBankAccount.DataSource = accounts;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Unable to load bank accounts.", ex.Message);
            }
        }

        private void LoadReconciliation()
        {
            int bankAccountId = GetSelectedBankAccountId();
            if (bankAccountId <= 0)
            {
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading bank reconciliation...", "جاري تحميل مطابقة البنك...")))
                {
                    _transactions = _bankBll.GetBankReconciliationTransactions(bankAccountId, dtpStatementDate.Value.Date);
                    _uncleared = _bankBll.GetUnclearedBankTransactions(bankAccountId, dtpStatementDate.Value.Date);

                    EnsureBooleanColumn(_transactions, "is_cleared");

                    dgvSystemTransactions.DataSource = _transactions;
                    dgvUncleared.DataSource = _uncleared;
                    RefreshSummary();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Unable to load reconciliation transactions.", ex.Message);
            }
        }

        private static void EnsureBooleanColumn(DataTable table, string columnName)
        {
            if (table == null || !table.Columns.Contains(columnName))
            {
                return;
            }

            if (table.Columns[columnName].DataType == typeof(bool))
            {
                return;
            }

            DataColumn boolColumn = new DataColumn(columnName + "_bool", typeof(bool));
            table.Columns.Add(boolColumn);
            foreach (DataRow row in table.Rows)
            {
                row[boolColumn] = row[columnName] != DBNull.Value && Convert.ToBoolean(row[columnName]);
            }
            table.Columns.Remove(columnName);
            boolColumn.ColumnName = columnName;
        }

        private void RefreshSummary()
        {
            decimal statementBalance = ParseAmount(txtStatementBalance.Text);
            decimal outstandingDeposits = 0m;
            decimal outstandingCheques = 0m;
            decimal bookBalance = 0m;

            if (_transactions != null)
            {
                foreach (DataRow row in _transactions.Rows)
                {
                    decimal debit = row["debit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["debit"]);
                    decimal credit = row["credit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["credit"]);
                    decimal amount = row["amount"] == DBNull.Value ? debit - credit : Convert.ToDecimal(row["amount"]);
                    bool isCleared = row.Table.Columns.Contains("is_cleared") && row["is_cleared"] != DBNull.Value && Convert.ToBoolean(row["is_cleared"]);

                    bookBalance += amount;

                    if (!isCleared)
                    {
                        if (debit > 0m)
                        {
                            outstandingDeposits += debit;
                        }

                        if (credit > 0m)
                        {
                            outstandingCheques += credit;
                        }
                    }
                }
            }

            decimal adjustedBalance = statementBalance + outstandingDeposits - outstandingCheques;
            decimal difference = adjustedBalance - bookBalance;

            lblStatementValue.Text = statementBalance.ToString("N2", CultureInfo.InvariantCulture);
            lblOutstandingDepositsValue.Text = outstandingDeposits.ToString("N2", CultureInfo.InvariantCulture);
            lblOutstandingChequesValue.Text = outstandingCheques.ToString("N2", CultureInfo.InvariantCulture);
            lblAdjustedBalanceValue.Text = adjustedBalance.ToString("N2", CultureInfo.InvariantCulture);
            lblBookBalanceValue.Text = bookBalance.ToString("N2", CultureInfo.InvariantCulture);
            lblDifferenceValue.Text = difference.ToString("N2", CultureInfo.InvariantCulture);
            lblDifferenceValue.ForeColor = Math.Abs(difference) < 0.01m ? Color.DarkGreen : Color.DarkRed;
        }

        private void SaveReconciliation()
        {
            int bankAccountId = GetSelectedBankAccountId();
            if (bankAccountId <= 0)
            {
                UiMessages.ShowError("Please select a bank account.", "يرجى اختيار حساب البنك.");
                return;
            }

            decimal statementBalance = ParseAmount(txtStatementBalance.Text);
            decimal adjustedBalance = ParseAmount(lblAdjustedBalanceValue.Text);
            decimal bookBalance = ParseAmount(lblBookBalanceValue.Text);
            decimal difference = ParseAmount(lblDifferenceValue.Text);

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Saving reconciliation...", "جاري حفظ المطابقة...")))
                {
                    DataTable saveRows = new DataTable();
                    saveRows.Columns.Add("entry_id", typeof(int));
                    saveRows.Columns.Add("is_cleared", typeof(bool));

                    foreach (DataRow row in _transactions.Rows)
                    {
                        DataRow item = saveRows.NewRow();
                        item["entry_id"] = row["entry_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["entry_id"]);
                        item["is_cleared"] = row.Table.Columns.Contains("is_cleared") && row["is_cleared"] != DBNull.Value && Convert.ToBoolean(row["is_cleared"]);
                        saveRows.Rows.Add(item);
                    }

                    _bankBll.SaveBankReconciliation(
                        bankAccountId,
                        dtpStatementDate.Value.Date,
                        statementBalance,
                        adjustedBalance,
                        bookBalance,
                        difference,
                        UsersModal.logged_in_userid,
                        saveRows);
                }

                UiMessages.ShowInfo("Bank reconciliation saved successfully.", "تم حفظ مطابقة البنك بنجاح.");
                LoadReconciliation();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Unable to save reconciliation.", ex.Message);
            }
        }

        private void PrintReconciliation()
        {
            IDictionary<string, DataTable> tables = new Dictionary<string, DataTable>();
            tables["System Transactions"] = _transactions == null ? new DataTable() : _transactions.Copy();
            tables["Uncleared Transactions"] = _uncleared == null ? new DataTable() : _uncleared.Copy();

            DataTable summary = new DataTable();
            summary.Columns.Add("Metric", typeof(string));
            summary.Columns.Add("Amount", typeof(decimal));
            summary.Rows.Add("Bank Statement Balance", ParseAmount(lblStatementValue.Text));
            summary.Rows.Add("Outstanding Deposits", ParseAmount(lblOutstandingDepositsValue.Text));
            summary.Rows.Add("Outstanding Cheques", ParseAmount(lblOutstandingChequesValue.Text));
            summary.Rows.Add("Adjusted Bank Balance", ParseAmount(lblAdjustedBalanceValue.Text));
            summary.Rows.Add("System Book Balance", ParseAmount(lblBookBalanceValue.Text));
            summary.Rows.Add("Difference", ParseAmount(lblDifferenceValue.Text));
            tables["Summary"] = summary;

            using (DataGridReportViewerForm viewer = new DataGridReportViewerForm("Bank Reconciliation", tables))
            {
                viewer.ShowDialog(this);
            }
        }

        private int GetSelectedBankAccountId()
        {
            if (cmbBankAccount.SelectedValue == null || cmbBankAccount.SelectedValue == DBNull.Value)
            {
                return 0;
            }

            int accountId;
            return int.TryParse(Convert.ToString(cmbBankAccount.SelectedValue), out accountId) ? accountId : 0;
        }

        private static decimal ParseAmount(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0m;
            }

            decimal value;
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
            {
                return value;
            }

            return 0m;
        }
    }
}
