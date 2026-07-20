using pos.UI;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_account_lookup : Form
    {
        private DataTable _accountsTable;
        private DataView _accountsView;
        private string _initialSearchText = string.Empty;

        public frm_account_lookup()
        {
            InitializeComponent();
            gridAccounts.AutoGenerateColumns = false;
        }

        public DataRow SelectedAccountRow { get; private set; }

        public void LoadAccounts(DataTable accountsTable, string initialSearchText)
        {
            _accountsTable = accountsTable;
            _initialSearchText = initialSearchText ?? string.Empty;
        }

        private void frm_account_lookup_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);

            _accountsView = _accountsTable != null ? new DataView(_accountsTable) : new DataView(new DataTable());
            gridAccounts.DataSource = _accountsView;
            txtSearch.Text = _initialSearchText;
            ApplyFilter();
            txtSearch.Focus();
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.SelectAll();
            }
            else
            {
                txtSearch.SelectionStart = txtSearch.Text.Length;
                txtSearch.SelectionLength = 0;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && gridAccounts.Rows.Count > 0)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                gridAccounts.Focus();
                if (gridAccounts.CurrentCell == null)
                {
                    gridAccounts.CurrentCell = gridAccounts.Rows[0].Cells[0];
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SelectCurrentAccount();
            }
        }

        private void gridAccounts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectCurrentAccount();
            }
        }

        private void gridAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SelectCurrentAccount();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectCurrentAccount();
        }

        private void frm_account_lookup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void ApplyFilter()
        {
            if (_accountsView == null)
            {
                return;
            }

            string text = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                _accountsView.RowFilter = string.Empty;
            }
            else
            {
                DataRow exactCodeRow = FindExactAccountByCode(text);
                if (exactCodeRow != null)
                {
                    _accountsView.RowFilter = "id = " + Convert.ToInt32(exactCodeRow["id"]);
                }
                else
                {
                    string[] parts = text
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => p.Length > 0)
                        .ToArray();

                    _accountsView.RowFilter = parts.Length == 0
                        ? string.Empty
                        : string.Join(" AND ", parts.Select(BuildTermFilter));
                }
            }

            UpdateSelection();
        }

        private string BuildTermFilter(string term)
        {
            string value = EscapeRowFilterValue(term);
            return string.Format("(code LIKE '%{0}%' OR name LIKE '%{0}%' OR name_2 LIKE '%{0}%' OR display LIKE '%{0}%')", value);
        }

        private string EscapeRowFilterValue(string text)
        {
            return (text ?? string.Empty).Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
        }

        private DataRow FindExactAccountByCode(string code)
        {
            if (_accountsTable == null || string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            string search = code.Trim();
            foreach (DataRow row in _accountsTable.Rows)
            {
                if (string.Equals(Convert.ToString(row["code"]).Trim(), search, StringComparison.OrdinalIgnoreCase))
                {
                    return row;
                }
            }

            return null;
        }

        private void UpdateSelection()
        {
            btnSelect.Enabled = gridAccounts.Rows.Count > 0;
            if (gridAccounts.Rows.Count == 0)
            {
                SelectedAccountRow = null;
                return;
            }

            int rowIndex = gridAccounts.CurrentCell != null && gridAccounts.CurrentCell.RowIndex >= 0 && gridAccounts.CurrentCell.RowIndex < gridAccounts.Rows.Count
                ? gridAccounts.CurrentCell.RowIndex
                : 0;

            gridAccounts.CurrentCell = gridAccounts.Rows[rowIndex].Cells[0];
            gridAccounts.Rows[rowIndex].Selected = true;
        }

        private void SelectCurrentAccount()
        {
            if (gridAccounts.Rows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = gridAccounts.CurrentRow ?? gridAccounts.Rows[0];
            DataRowView rowView = row.DataBoundItem as DataRowView;
            if (rowView == null)
            {
                return;
            }

            SelectedAccountRow = rowView.Row;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
