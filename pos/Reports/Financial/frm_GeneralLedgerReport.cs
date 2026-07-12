using DGVPrinterHelper;
using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using pos.Expenses;
using pos.Reports.Common;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Financial
{
    public partial class frm_GeneralLedgerReport : Form
    {
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private readonly Timer _debounceTimer = new Timer();

        private DataTable _accountsTable;
        private DataTable _accountsFiltered;
        private DataTable _rawLedgerRows;

        private int _currentPage = 1;
        private const int PageSize = 500;
        private int _totalRows;
        private decimal _openingBalance;
        private decimal _periodDebit;
        private decimal _periodCredit;

        public frm_GeneralLedgerReport()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += Frm_GeneralLedgerReport_Load;
            _btnLoad.Click += (s, e) => LoadLedgerFirstPage();
            _btnLoadMore.Click += (s, e) => LoadMore();
            _cmbShow.SelectedIndexChanged += (s, e) => LoadLedgerFirstPage();
            _cmbGroupBy.SelectedIndexChanged += (s, e) => RebuildLedgerGridFromRaw();
            _cmbAccount.SelectedIndexChanged += (s, e) => ScheduleAutoLoad();
            _cmbAccount.TextUpdate += CmbAccount_TextUpdate;
            _cmbAccount.DrawItem += CmbAccount_DrawItem;
            _gridLedger.CellContentClick += GridLedger_CellContentClick;
            _gridLedger.CellDoubleClick += GridLedger_CellDoubleClick;
            _gridLedger.CellFormatting += GridLedger_CellFormatting;
            _btnViewFullHistory.Click += BtnViewFullHistory_Click;
            _btnPrint.Click += BtnPrint_Click;
            _btnExport.Click += BtnExport_Click;

            _debounceTimer.Interval = 300;
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                if (_cmbAccount.SelectedValue != null)
                {
                    LoadLedgerFirstPage();
                }
            };
        }

        private void Frm_GeneralLedgerReport_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            LoadAccounts();
            ResetSummary();
            SetPinnedRows(0m, 0m, 0m);
        }

        private void LoadAccounts()
        {
            try
            {
                _accountsTable = _accountsBll.GetLedgerAccounts();
                if (!_accountsTable.Columns.Contains("display_text"))
                {
                    _accountsTable.Columns.Add("display_text", typeof(string));
                }

                if (!_accountsTable.Columns.Contains("search_text"))
                {
                    _accountsTable.Columns.Add("search_text", typeof(string));
                }

                foreach (DataRow row in _accountsTable.Rows)
                {
                    decimal current = row["current_balance"] == DBNull.Value ? 0m : Convert.ToDecimal(row["current_balance"]);
                    string code = Convert.ToString(row["acc_code"]);
                    string name = Convert.ToString(row["acc_name"]);
                    string type = Convert.ToString(row["account_type"]);
                    row["display_text"] = string.Format("{0} - {1}    [{2}]    Bal: {3}", code, name, type, FormatSigned(current));
                    row["search_text"] = (code + " " + name + " " + type).ToLowerInvariant();
                }

                _accountsFiltered = _accountsTable.Copy();
                _cmbAccount.DataSource = _accountsFiltered;
                _cmbAccount.DisplayMember = "display_text";
                _cmbAccount.ValueMember = "acc_id";
                _cmbAccount.SelectedIndex = _accountsFiltered.Rows.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void CmbAccount_TextUpdate(object sender, EventArgs e)
        {
            if (_accountsTable == null)
            {
                return;
            }

            string term = (_cmbAccount.Text ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(term))
            {
                _accountsFiltered = _accountsTable.Copy();
            }
            else
            {
                var rows = _accountsTable.AsEnumerable()
                    .Where(x => Convert.ToString(x["search_text"]).Contains(term))
                    .ToArray();

                _accountsFiltered = _accountsTable.Clone();
                foreach (var row in rows)
                {
                    _accountsFiltered.ImportRow(row);
                }
            }

            _cmbAccount.DataSource = _accountsFiltered;
            _cmbAccount.DisplayMember = "display_text";
            _cmbAccount.ValueMember = "acc_id";
            _cmbAccount.Text = term;
            _cmbAccount.SelectionStart = _cmbAccount.Text.Length;
            _cmbAccount.DroppedDown = true;
            Cursor.Current = Cursors.Default;
        }

        private void CmbAccount_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0)
            {
                return;
            }

            var item = _cmbAccount.Items[e.Index] as DataRowView;
            if (item == null)
            {
                return;
            }

            string text = Convert.ToString(item["display_text"]);
            Color fore = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Color.White : AppTheme.TextPrimary;
            using (var brush = new SolidBrush(fore))
            {
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        private void ScheduleAutoLoad()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void LoadLedgerFirstPage()
        {
            try
            {
                int accId = GetSelectedAccountId();
                if (accId <= 0)
                {
                    return;
                }

                _currentPage = 1;
                _rawLedgerRows = null;

                using (BusyScope.Show(this, UiMessages.T("Loading ledger...", "جاري تحميل دفتر الأستاذ...")))
                {
                    LoadSummary(accId);
                    var result = _accountsBll.GetAccountLedgerPage(accId, _dtFrom.Value.Date, _dtTo.Value.Date, _currentPage, PageSize, GetShowFilterValue());
                    _openingBalance = result.Item3;
                    _totalRows = result.Item2;
                    _rawLedgerRows = result.Item1 == null ? new DataTable() : result.Item1.Copy();
                    RebuildLedgerGridFromRaw();
                    SetPinnedRows(_openingBalance, _periodDebit, _periodCredit);
                    UpdateLoadMoreState();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void LoadMore()
        {
            try
            {
                int accId = GetSelectedAccountId();
                if (accId <= 0)
                {
                    return;
                }

                if (_gridLedger.Rows.Count >= _totalRows)
                {
                    UpdateLoadMoreState();
                    return;
                }

                _currentPage++;
                using (BusyScope.Show(this, UiMessages.T("Loading more entries...", "جاري تحميل المزيد من القيود...")))
                {
                    var result = _accountsBll.GetAccountLedgerPage(accId, _dtFrom.Value.Date, _dtTo.Value.Date, _currentPage, PageSize, GetShowFilterValue());
                    if (_rawLedgerRows == null)
                    {
                        _rawLedgerRows = result.Item1.Clone();
                    }

                    foreach (DataRow row in result.Item1.Rows)
                    {
                        _rawLedgerRows.ImportRow(row);
                    }

                    RebuildLedgerGridFromRaw();
                    UpdateLoadMoreState();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void LoadSummary(int accId)
        {
            var dt = _accountsBll.GetAccountLedgerSummary(accId, _dtFrom.Value.Date, _dtTo.Value.Date);
            if (dt == null || dt.Rows.Count == 0)
            {
                ResetSummary();
                _periodDebit = 0m;
                _periodCredit = 0m;
                return;
            }

            DataRow row = dt.Rows[0];
            string code = Convert.ToString(row["acc_code"]);
            string name = Convert.ToString(row["acc_name"]);
            string type = Convert.ToString(row["account_type"]);
            string normal = Convert.ToString(row["normal_balance"]);
            decimal current = row["current_balance"] == DBNull.Value ? 0m : Convert.ToDecimal(row["current_balance"]);
            _periodDebit = row["period_debit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["period_debit"]);
            _periodCredit = row["period_credit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["period_credit"]);
            int count = row["period_count"] == DBNull.Value ? 0 : Convert.ToInt32(row["period_count"]);
            string lastDate = row["last_txn_date"] == DBNull.Value
                ? "-"
                : Convert.ToDateTime(row["last_txn_date"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            _lblSummaryAccount.Text = code + "\n" + name;
            _lblSummaryType.Text = string.IsNullOrWhiteSpace(type) ? "Type: -" : type;
            _lblSummaryNormal.Text = "Normal Balance: " + (string.IsNullOrWhiteSpace(normal) ? "-" : normal);
            _lblSummaryCurrent.Text = FormatSigned(current);
            _lblSummaryCurrent.ForeColor = current >= 0 ? Color.ForestGreen : Color.Firebrick;
            _lblSummaryTurnover.Text = string.Format("Period Turnover\nDr: {0} \nCr: {1}", _periodDebit.ToString("N2"), _periodCredit.ToString("N2"));
            _lblSummaryCount.Text = "Entries: " + count.ToString();
            _lblSummaryLastTxn.Text = "Last Txn: " + lastDate;

            _lblSummaryType.BackColor = GetModuleColor(type);
            _lblSummaryType.ForeColor = Color.White;
        }

        private void RebuildLedgerGridFromRaw()
        {
            _gridLedger.Rows.Clear();
            if (_rawLedgerRows == null || _rawLedgerRows.Rows.Count == 0)
            {
                SetPinnedRows(_openingBalance, _periodDebit, _periodCredit);
                UpdateLoadMoreState();
                return;
            }

            decimal running = _openingBalance;
            string groupMode = _cmbGroupBy.SelectedItem == null ? "None" : _cmbGroupBy.SelectedItem.ToString();

            if (string.Equals(groupMode, "By Voucher", StringComparison.OrdinalIgnoreCase))
            {
                var grouped = _rawLedgerRows.AsEnumerable()
                    .GroupBy(r => new
                    {
                        Date = r.Field<DateTime>("entry_date").Date,
                        VoucherNo = Convert.ToString(r["voucher_no"]),
                        VoucherType = Convert.ToString(r["voucher_type"]),
                        RefModule = Convert.ToString(r["ref_module"]),
                        RefId = Convert.ToString(r["ref_id"]),
                        Status = Convert.ToString(r["status"])
                    })
                    .OrderBy(g => g.Key.Date)
                    .ThenBy(g => g.Key.VoucherNo);

                foreach (var g in grouped)
                {
                    decimal debit = g.Sum(x => x.Field<decimal>("debit"));
                    decimal credit = g.Sum(x => x.Field<decimal>("credit"));
                    running += debit - credit;
                    string narration = string.Join(" | ", g.Select(x => Convert.ToString(x["narration"]).Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Take(3));
                    AddLedgerRow(g.Key.Date.ToString("yyyy-MM-dd"), g.Key.VoucherNo, g.Key.VoucherType, g.Key.RefModule, narration, debit, credit, running, g.Key.RefId, g.Key.Status);
                }
            }
            else if (string.Equals(groupMode, "By Month", StringComparison.OrdinalIgnoreCase))
            {
                var grouped = _rawLedgerRows.AsEnumerable()
                    .GroupBy(r => new DateTime(r.Field<DateTime>("entry_date").Year, r.Field<DateTime>("entry_date").Month, 1))
                    .OrderBy(g => g.Key);

                foreach (var g in grouped)
                {
                    decimal debit = g.Sum(x => x.Field<decimal>("debit"));
                    decimal credit = g.Sum(x => x.Field<decimal>("credit"));
                    running += debit - credit;
                    AddLedgerRow(g.Key.ToString("yyyy-MM"), g.Key.ToString("yyyy-MM"), "MONTH", "MONTH", "Monthly Ledger Total", debit, credit, running, string.Empty, "Posted");
                }
            }
            else
            {
                foreach (DataRow row in _rawLedgerRows.Rows)
                {
                    decimal debit = row["debit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["debit"]);
                    decimal credit = row["credit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["credit"]);
                    decimal balance = row["running_balance"] == DBNull.Value ? running + debit - credit : Convert.ToDecimal(row["running_balance"]);
                    running = balance;
                    AddLedgerRow(
                        Convert.ToDateTime(row["entry_date"]).ToString("yyyy-MM-dd"),
                        Convert.ToString(row["voucher_no"]),
                        Convert.ToString(row["voucher_type"]),
                        Convert.ToString(row["ref_module"]),
                        Convert.ToString(row["narration"]),
                        debit,
                        credit,
                        balance,
                        Convert.ToString(row["ref_id"]),
                        Convert.ToString(row["status"]));
                }
            }

            SetPinnedRows(_openingBalance, _periodDebit, _periodCredit);
            UpdateLoadMoreState();
        }

        private void AddLedgerRow(string date, string voucherNo, string voucherType, string refModule, string narration, decimal debit, decimal credit, decimal running, string refId, string status)
        {
            _gridLedger.Rows.Add(
                date,
                voucherNo,
                voucherType,
                string.IsNullOrWhiteSpace(refModule) ? "ADJ" : refModule,
                narration,
                debit.ToString("N2"),
                credit.ToString("N2"),
                FormatSigned(running),
                refId,
                status);
        }

        private void SetPinnedRows(decimal opening, decimal periodDebit, decimal periodCredit)
        {
            _gridOpening.Rows.Clear();
            _gridClosing.Rows.Clear();

            decimal openingDr = opening >= 0 ? opening : 0m;
            decimal openingCr = opening < 0 ? -opening : 0m;
            decimal closing = opening + periodDebit - periodCredit;

            _gridOpening.Rows.Add("Opening Balance", string.Empty, string.Empty, string.Empty, "Balance before From Date", openingDr.ToString("N2"), openingCr.ToString("N2"), FormatSigned(opening));
            _gridClosing.Rows.Add("Closing Balance", string.Empty, string.Empty, string.Empty, "Totals", periodDebit.ToString("N2"), periodCredit.ToString("N2"), FormatSigned(closing));

            foreach (DataGridViewCell cell in _gridOpening.Rows[0].Cells)
            {
                cell.Style.BackColor = Color.FromArgb(247, 249, 252);
                cell.Style.Font = new Font(_gridOpening.Font, FontStyle.Bold);
            }

            foreach (DataGridViewCell cell in _gridClosing.Rows[0].Cells)
            {
                cell.Style.BackColor = Color.FromArgb(255, 249, 228);
                cell.Style.Font = new Font(_gridClosing.Font, FontStyle.Bold);
            }

            _gridOpening.Rows[0].Cells[7].Style.ForeColor = opening >= 0 ? Color.ForestGreen : Color.Firebrick;
            _gridClosing.Rows[0].Cells[7].Style.ForeColor = closing >= 0 ? Color.ForestGreen : Color.Firebrick;
        }

        private void UpdateLoadMoreState()
        {
            int loaded = _rawLedgerRows == null ? 0 : _rawLedgerRows.Rows.Count;
            _btnLoadMore.Visible = loaded > 0 && loaded < _totalRows;
            _btnLoadMore.Enabled = _btnLoadMore.Visible;
            _btnLoadMore.Text = string.Format("Load More ({0}/{1})", loaded, _totalRows);
        }

        private int GetSelectedAccountId()
        {
            if (_cmbAccount.SelectedValue == null)
            {
                return 0;
            }

            int accId;
            return int.TryParse(_cmbAccount.SelectedValue.ToString(), out accId) ? accId : 0;
        }

        private string GetShowFilterValue()
        {
            string selected = _cmbShow.SelectedItem == null ? "All Entries" : _cmbShow.SelectedItem.ToString();
            if (selected == "Debits Only") return "Debits";
            if (selected == "Credits Only") return "Credits";
            if (selected == "Unposted/Draft Only") return "Unposted";
            return "All";
        }

        private void GridLedger_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _gridLedger.Columns[e.ColumnIndex].Name != "VoucherNo")
            {
                return;
            }

            OpenVoucherFromRow(_gridLedger.Rows[e.RowIndex]);
        }

        private void GridLedger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            OpenVoucherFromRow(_gridLedger.Rows[e.RowIndex]);
        }

        private void OpenVoucherFromRow(DataGridViewRow row)
        {
            string voucherNo = Convert.ToString(row.Cells["VoucherNo"].Value);
            string refModule = Convert.ToString(row.Cells["RefModule"].Value).ToUpperInvariant();
            string refId = Convert.ToString(row.Cells["RefId"].Value);

            if (string.IsNullOrWhiteSpace(voucherNo) || voucherNo.Length < 2)
            {
                return;
            }

            try
            {
                if (refModule == "SALE" || voucherNo.StartsWith("S", StringComparison.OrdinalIgnoreCase))
                {
                    int saleId;
                    if (int.TryParse(refId, out saleId) && saleId > 0)
                    {
                        using (var frm = new frm_sales_detail(saleId, voucherNo))
                        {
                            frm.ShowDialog(this);
                        }
                    }
                    return;
                }

                if (refModule == "PURCHASE" || voucherNo.StartsWith("P", StringComparison.OrdinalIgnoreCase))
                {
                    using (var frm = new frm_purchases_detail())
                    {
                        frm.load_purchases_detail_grid(voucherNo);
                        frm.ShowDialog(this);
                    }
                    return;
                }

                if (refModule == "EXPENSE" || refModule == "PAYMENT" || voucherNo.StartsWith("E", StringComparison.OrdinalIgnoreCase))
                {
                    using (var frm = new frm_expenses(voucherNo))
                    {
                        frm.ShowDialog(this);
                    }
                    return;
                }

                using (var frm = new frm_journal_entries())
                {
                    if (frm.LoadVoucherForEdit(voucherNo))
                    {
                        frm.ShowDialog(this);
                    }
                    else
                    {
                        using (var manager = new frm_journal_voucher_manager())
                        {
                            manager.ShowDialog(this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowWarning(ex.Message, ex.Message);
            }
        }

        private void GridLedger_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            string columnName = _gridLedger.Columns[e.ColumnIndex].Name;
            if (columnName == "Balance")
            {
                decimal value;
                if (decimal.TryParse(Convert.ToString(e.Value).Replace("Dr", string.Empty).Replace("Cr", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture, out value) ||
                    decimal.TryParse(Convert.ToString(e.Value).Split(' ').FirstOrDefault(), NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                {
                    e.CellStyle.ForeColor = Convert.ToString(e.Value).Contains("Cr") ? Color.Firebrick : Color.ForestGreen;
                }
            }

            if (columnName == "RefModule")
            {
                string module = Convert.ToString(e.Value).ToUpperInvariant();
                e.CellStyle.BackColor = GetModuleColor(module);
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.CellStyle.Font = new Font(_gridLedger.Font, FontStyle.Bold);
            }
        }

        private Color GetModuleColor(string module)
        {
            switch ((module ?? string.Empty).Trim().ToUpperInvariant())
            {
                case "SALE": return Color.FromArgb(0, 120, 212);
                case "PURCHASE": return Color.FromArgb(16, 124, 16);
                case "EXPENSE": return Color.FromArgb(255, 185, 0);
                case "JV": return Color.FromArgb(136, 84, 208);
                case "PAYMENT": return Color.FromArgb(0, 120, 136);
                case "ADJ": return Color.FromArgb(130, 130, 130);
                default: return Color.FromArgb(130, 130, 130);
            }
        }

        private static string FormatSigned(decimal value)
        {
            if (value < 0)
            {
                return string.Format("{0:N2} Cr", Math.Abs(value));
            }

            return string.Format("{0:N2} Dr", value);
        }

        private void BtnViewFullHistory_Click(object sender, EventArgs e)
        {
            _dtFrom.Value = new DateTime(2000, 1, 1);
            _dtTo.Value = DateTime.Today;
            LoadLedgerFirstPage();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (_gridLedger.Rows.Count == 0)
            {
                UiMessages.ShowInfo("No entries to print.", "لا توجد قيود للطباعة.");
                return;
            }

            var printer = new DGVPrinter
            {
                Title = "General Ledger",
                SubTitle = string.Format("{0} - {1}", _dtFrom.Value.ToString("yyyy-MM-dd"), _dtTo.Value.ToString("yyyy-MM-dd")),
                SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip,
                PageNumbers = true,
                PageNumberInHeader = false,
                PorportionalColumns = true,
                HeaderCellAlignment = StringAlignment.Near,
                Footer = "kasbook",
                FooterSpacing = 10
            };
            printer.PrintPreviewDataGridView(_gridLedger);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            var table = new DataTable();
            table.Columns.Add("Date");
            table.Columns.Add("Voucher No");
            table.Columns.Add("Voucher Type");
            table.Columns.Add("Ref Module");
            table.Columns.Add("Narration");
            table.Columns.Add("Debit");
            table.Columns.Add("Credit");
            table.Columns.Add("Balance");

            table.Rows.Add("Opening Balance", "", "", "", "Balance before From Date", _gridOpening.Rows[0].Cells[5].Value, _gridOpening.Rows[0].Cells[6].Value, _gridOpening.Rows[0].Cells[7].Value);

            foreach (DataGridViewRow row in _gridLedger.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                table.Rows.Add(
                    row.Cells["Date"].Value,
                    row.Cells["VoucherNo"].Value,
                    row.Cells["VoucherType"].Value,
                    row.Cells["RefModule"].Value,
                    row.Cells["Narration"].Value,
                    row.Cells["Debit"].Value,
                    row.Cells["Credit"].Value,
                    row.Cells["Balance"].Value);
            }

            table.Rows.Add("Closing Balance", "", "", "", "Totals", _gridClosing.Rows[0].Cells[5].Value, _gridClosing.Rows[0].Cells[6].Value, _gridClosing.Rows[0].Cells[7].Value);
            ExcelExportHelper.ExportDataTableToExcel(table, "GeneralLedger", this, true);
        }

        private void ResetSummary()
        {
            _lblSummaryAccount.Text = "-";
            _lblSummaryType.Text = "Type: -";
            _lblSummaryType.BackColor = Color.Gainsboro;
            _lblSummaryType.ForeColor = AppTheme.TextPrimary;
            _lblSummaryNormal.Text = "Normal Balance: -";
            _lblSummaryCurrent.Text = "0.00 Dr";
            _lblSummaryCurrent.ForeColor = Color.ForestGreen;
            _lblSummaryTurnover.Text = "Period Turnover\nDr: 0.00 \nCr: 0.00";
            _lblSummaryCount.Text = "Entries: 0";
            _lblSummaryLastTxn.Text = "Last Txn: -";
        }
    }
}
