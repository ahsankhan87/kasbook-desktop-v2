using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using POS.BLL;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_customers : Form
    {
        private readonly Color _accent = Color.FromArgb(245, 127, 23);
        private readonly Color _overdueRed = Color.FromArgb(198, 40, 40);
        private readonly Color _amber = Color.FromArgb(245, 124, 0);
        private readonly Color _okGreen = Color.FromArgb(46, 125, 50);

        private DataTable _dashboardSource = new DataTable();
        private DataView _dashboardView;

        private bool _targetDetailsOpen;
        private int _detailsExpandedWidth = 280;
        private int _selectedCustomerId;
        private string _avatarInitials = "?";

        public frm_customers()
        {
            InitializeComponent();
        }

        public void frm_customers_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            ApplyDashboardTheme();
            InitializeFilters();
            LoadDashboard();
        }

        private void ApplyDashboardTheme()
        {
            BackColor = Color.White;
            lblTitle.ForeColor = AppTheme.TextPrimary;

            cardTotalCustomers.BackColor = Color.White;
            cardReceivables.BackColor = Color.White;
            cardOverdue.BackColor = Color.White;
            cardNewCustomers.BackColor = Color.White;

            cardTotalCustomers.Padding = new Padding(12, 8, 12, 8);
            cardReceivables.Padding = new Padding(12, 8, 12, 8);
            cardOverdue.Padding = new Padding(12, 8, 12, 8);
            cardNewCustomers.Padding = new Padding(12, 8, 12, 8);

            cardTotalCustomers.Paint += KpiCard_Paint;
            cardReceivables.Paint += KpiCard_Paint;
            cardOverdue.Paint += KpiCard_Paint;
            cardNewCustomers.Paint += KpiCard_Paint;

            splitMain.Panel2MinSize = 0;
            splitMain.SplitterDistance = splitMain.Width - 8;

            grid_customers.EnableHeadersVisualStyles = false;
            grid_customers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 235);
            grid_customers.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid_customers.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_customers.RowTemplate.Height = 32;
            grid_customers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            grid_customers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 243, 224);
            grid_customers.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid_customers.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_customers.AutoGenerateColumns = false;

            pnlDetails.BackColor = Color.White;
            pnlDetails.BorderStyle = BorderStyle.FixedSingle;

            btnViewProfile.BackColor = Color.White;
            btnViewProfile.FlatStyle = FlatStyle.Flat;
            btnViewProfile.FlatAppearance.BorderColor = _accent;

            btnNewInvoice.BackColor = _accent;
            btnNewInvoice.ForeColor = Color.White;
            btnNewInvoice.FlatStyle = FlatStyle.Flat;
            btnNewInvoice.FlatAppearance.BorderSize = 0;

            btnReceivePayment.BackColor = AppTheme.Primary;
            btnReceivePayment.ForeColor = Color.White;
            btnReceivePayment.FlatStyle = FlatStyle.Flat;
            btnReceivePayment.FlatAppearance.BorderSize = 0;
        }

        private void KpiCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            using (Pen pen = new Pen(Color.FromArgb(235, 235, 235)))
            {
                Rectangle r = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                e.Graphics.DrawRectangle(pen, r);
            }

            using (SolidBrush brush = new SolidBrush(_accent))
            {
                e.Graphics.FillRectangle(brush, 0, 0, 4, card.Height);
            }
        }

        private void InitializeFilters()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[] { "All", "Active", "Inactive", "Overdue" });
            cmbStatus.SelectedIndex = 0;

            cmbArea.Items.Clear();
            cmbArea.Items.Add("All Areas");
            cmbArea.SelectedIndex = 0;
        }

        private void LoadDashboard()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading customer dashboard...", "جاري تحميل لوحة العملاء...")))
                {
                    CustomerBLL bll = new CustomerBLL();
                    DataTable raw = bll.GetCustomerSummaryDashboard();
                    _dashboardSource = BuildDashboardData(raw);
                    _dashboardView = _dashboardSource.DefaultView;
                    grid_customers.DataSource = _dashboardView;
                    
                    PopulateAreaFilter();
                    ApplyFilters();
                    
                    if (grid_customers.Rows.Count > 0)
                    {
                        grid_customers.Rows[0].Selected = true;
                        UpdateCustomerDetailsFromSelectedRow();
                        ToggleDetailsPanel(true);
                    }
                    else
                    {
                        ResetDetails();
                        ToggleDetailsPanel(false);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Customers", "العملاء");
            }
        }

        private DataTable BuildDashboardData(DataTable raw)
        {
            DataTable dt = raw.Copy();

            if (!dt.Columns.Contains("customer_name"))
                dt.Columns.Add("customer_name", typeof(string));
            if (!dt.Columns.Contains("area"))
                dt.Columns.Add("area", typeof(string));
            if (!dt.Columns.Contains("row_no"))
                dt.Columns.Add("row_no", typeof(int));

            int i = 1;
            foreach (DataRow r in dt.Rows)
            {
                r["customer_name"] = (Convert.ToString(r["first_name"]) + " " + Convert.ToString(r["last_name"])).Trim();
                r["area"] = ExtractArea(Convert.ToString(r["address"]));
                r["row_no"] = i++;
            }

            return dt;
        }

        private string ExtractArea(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return "N/A";
            string[] parts = address.Split(',');
            string first = parts[0].Trim();
            return string.IsNullOrWhiteSpace(first) ? "N/A" : first;
        }

        private void PopulateAreaFilter()
        {
            string selected = cmbArea.SelectedItem == null ? "All Areas" : cmbArea.SelectedItem.ToString();
            var areas = _dashboardSource.AsEnumerable()
                .Select(r => Convert.ToString(r["area"]))
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            cmbArea.Items.Clear();
            cmbArea.Items.Add("All Areas");
            foreach (var area in areas)
                cmbArea.Items.Add(area);

            int index = cmbArea.Items.IndexOf(selected);
            cmbArea.SelectedIndex = index >= 0 ? index : 0;
        }

        private void ApplyFilters()
        {
            if (_dashboardView == null) return;

            string text = (txt_search.Text ?? string.Empty).Trim().Replace("'", "''");
            string area = cmbArea.SelectedItem == null ? "All Areas" : cmbArea.SelectedItem.ToString().Replace("'", "''");
            string status = cmbStatus.SelectedItem == null ? "All" : cmbStatus.SelectedItem.ToString();

            string filter = string.Empty;

            if (!string.IsNullOrWhiteSpace(text))
            {
                filter = "(customer_name LIKE '%" + text + "%' OR customer_code LIKE '%" + text + "%')";
            }

            if (!string.Equals(area, "All Areas", StringComparison.OrdinalIgnoreCase))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += "area = '" + area + "'";
            }

            if (!string.Equals(status, "All", StringComparison.OrdinalIgnoreCase))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += "status_text = '" + status.Replace("'", "''") + "'";
            }

            _dashboardView.RowFilter = filter;
            UpdateRowNumbers(_dashboardView.ToTable());
            UpdateKpis();

            if (grid_customers.Rows.Count == 0)
            {
                ResetDetails();
                ToggleDetailsPanel(false);
            }
        }

        private void UpdateRowNumbers(DataTable current)
        {
            int n = 1;
            foreach (DataRowView rv in _dashboardView)
            {
                rv["row_no"] = n++;
            }
        }

        private void UpdateKpis()
        {
            DataTable filtered = _dashboardView.ToTable();
            int totalActive = filtered.AsEnumerable().Count(r => string.Equals(Convert.ToString(r["status_text"]), "Active", StringComparison.OrdinalIgnoreCase));
            decimal receivables = filtered.AsEnumerable().Sum(r => ToDecimal(r["outstanding_balance"]));
            decimal overdue = filtered.AsEnumerable().Sum(r => ToDecimal(r["overdue_over_30"]));

            DateTime now = DateTime.Now;
            int newThisMonth = _dashboardSource.AsEnumerable().Count(r => ToDateTime(r["date_created"]).Month == now.Month && ToDateTime(r["date_created"]).Year == now.Year);
            DateTime prev = now.AddMonths(-1);
            int newPrevMonth = _dashboardSource.AsEnumerable().Count(r => ToDateTime(r["date_created"]).Month == prev.Month && ToDateTime(r["date_created"]).Year == prev.Year);
            int trend = newThisMonth - newPrevMonth;

            lblTotalCustomersValue.Text = totalActive.ToString("N0");
            lblReceivablesValue.Text = receivables.ToString("N2");
            lblOverdueValue.Text = overdue.ToString("N2");
            lblNewCustomersValue.Text = newThisMonth.ToString("N0");
            lblNewCustomersTrend.Text = (trend >= 0 ? "▲ " : "▼ ") + Math.Abs(trend).ToString("N0") + " vs PM";
            lblNewCustomersTrend.ForeColor = trend >= 0 ? _okGreen : _overdueRed;
        }

        private decimal ToDecimal(object value)
        {
            decimal d;
            decimal.TryParse(Convert.ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out d);
            return d;
        }

        private DateTime ToDateTime(object value)
        {
            DateTime dt;
            DateTime.TryParse(Convert.ToString(value), out dt);
            return dt;
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void grid_customers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCustomerDetailsFromSelectedRow();
        }

        private void UpdateCustomerDetailsFromSelectedRow()
        {
            if (grid_customers.CurrentRow == null)
            {
                ResetDetails();
                ToggleDetailsPanel(false);
                return;
            }

            DataGridViewRow row = grid_customers.CurrentRow;
            _selectedCustomerId = SafeInt(row.Cells["colId"].Value);

            string customerName = Convert.ToString(row.Cells["colCustomerName"].Value);
            string customerCode = Convert.ToString(row.Cells["colCustomerCode"].Value);

            lblCustomerHeader.Text = customerName + " (" + customerCode + ")";
            _avatarInitials = GetInitials(customerName);
            panelAvatar.Invalidate();

            lblPhoneValue.Text = Convert.ToString(row.Cells["colPhone"].Value);
            lblAddressValue.Text = Convert.ToString(row.Cells["colAddress"].Value);
            lblAreaValue.Text = Convert.ToString(row.Cells["colArea"].Value);

            lblTotalSalesDetailValue.Text = ToDecimal(row.Cells["colTotalSales"].Value).ToString("N2");
            lblPaidValue.Text = ToDecimal(row.Cells["colPaidAmount"].Value).ToString("N2");
            lblOutstandingValue.Text = ToDecimal(row.Cells["colOutstanding"].Value).ToString("N2");

            lblCreditLimitValue.Text = ToDecimal(row.Cells["colCreditLimit"].Value).ToString("N2");
            lblCreditUsedValue.Text = ToDecimal(row.Cells["colCreditUsed"].Value).ToString("N2") + "%";
            lblCreditAvailableValue.Text = ToDecimal(row.Cells["colCreditAvailable"].Value).ToString("N2");

            LoadRecentTransactions(_selectedCustomerId);
            ToggleDetailsPanel(true);
        }

        private int SafeInt(object value)
        {
            int x;
            int.TryParse(Convert.ToString(value), out x);
            return x;
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            string[] parts = name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private void LoadRecentTransactions(int customerId)
        {
            lstTransactions.Items.Clear();
            if (customerId <= 0) return;

            try
            {
                CustomerBLL bll = new CustomerBLL();
                DataTable dt = bll.GetCustomerRecentTransactions(customerId, 5);

                foreach (DataRow r in dt.Rows)
                {
                    DateTime d = ToDateTime(r["entry_date"]);
                    string invoice = Convert.ToString(r["invoice_no"]);
                    decimal bal = ToDecimal(r["balance"]);
                    lstTransactions.Items.Add(d.ToString("yyyy-MM-dd") + " | " + invoice + " | " + bal.ToString("N2"));
                }

                if (lstTransactions.Items.Count == 0)
                    lstTransactions.Items.Add("No transactions");
            }
            catch
            {
                lstTransactions.Items.Add("Unable to load transactions");
            }
        }

        private void ResetDetails()
        {
            _selectedCustomerId = 0;
            lblCustomerHeader.Text = "Select customer";
            _avatarInitials = "?";
            panelAvatar.Invalidate();

            lblPhoneValue.Text = "-";
            lblAddressValue.Text = "-";
            lblAreaValue.Text = "-";

            lblTotalSalesDetailValue.Text = "0.00";
            lblPaidValue.Text = "0.00";
            lblOutstandingValue.Text = "0.00";
            lblCreditLimitValue.Text = "0.00";
            lblCreditUsedValue.Text = "0.00%";
            lblCreditAvailableValue.Text = "0.00";

            lstTransactions.Items.Clear();
        }

        private void ToggleDetailsPanel(bool show)
        {
            _targetDetailsOpen = show;
            detailSlideTimer.Start();
        }

        private void detailSlideTimer_Tick(object sender, EventArgs e)
        {
            int totalWidth = splitMain.Width;
            int current = totalWidth - splitMain.SplitterDistance - splitMain.SplitterWidth;
            int target = _targetDetailsOpen ? _detailsExpandedWidth : 0;

            if (Math.Abs(current - target) <= 10)
            {
                int finalDistance = Math.Max(0, Math.Min(totalWidth - splitMain.SplitterWidth, totalWidth - target - splitMain.SplitterWidth));
                splitMain.SplitterDistance = finalDistance;
                detailSlideTimer.Stop();
                return;
            }

            int delta = current < target ? 20 : -20;
            int next = current + delta;
            if (next < 0) next = 0;
            if (next > _detailsExpandedWidth) next = _detailsExpandedWidth;

            int newDistance = Math.Max(0, Math.Min(totalWidth - splitMain.SplitterWidth, totalWidth - next - splitMain.SplitterWidth));
            splitMain.SplitterDistance = newDistance;
        }

        private void grid_customers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewColumn col = grid_customers.Columns[e.ColumnIndex];

            if (col.Name == "colTotalSales" || col.Name == "colOutstanding" || col.Name == "colCreditLimit")
            {
                e.Value = ToDecimal(e.Value).ToString("N2");
                e.FormattingApplied = true;
            }

            if (col.Name == "colLastTransaction")
            {
                DateTime d = ToDateTime(e.Value);
                e.Value = d == DateTime.MinValue ? "-" : d.ToString("yyyy-MM-dd");
                e.FormattingApplied = true;
            }

            if (col.Name == "colOutstanding")
            {
                decimal overdue30 = ToDecimal(grid_customers.Rows[e.RowIndex].Cells["colOverdue30"].Value);
                decimal overdue1530 = ToDecimal(grid_customers.Rows[e.RowIndex].Cells["colOverdue1530"].Value);
                if (overdue30 > 0)
                    e.CellStyle.ForeColor = _overdueRed;
                else if (overdue1530 > 0)
                    e.CellStyle.ForeColor = _amber;
                else
                    e.CellStyle.ForeColor = _okGreen;
            }

            if (col.Name == "colCreditUsed")
            {
                e.Value = ToDecimal(e.Value).ToString("N0") + "%";
                e.FormattingApplied = true;
            }
        }

        private void grid_customers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = grid_customers.Columns[e.ColumnIndex].Name;

            if (columnName == "colCreditUsed")
            {
                e.PaintBackground(e.CellBounds, true);
                decimal percent = ToDecimal(grid_customers.Rows[e.RowIndex].Cells["colCreditUsed"].Value);

                Rectangle barRect = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + 9, e.CellBounds.Width - 16, 14);
                using (SolidBrush back = new SolidBrush(Color.FromArgb(236, 236, 236)))
                    e.Graphics.FillRectangle(back, barRect);

                int fillWidth = (int)(barRect.Width * Math.Min(100m, Math.Max(0m, percent)) / 100m);
                Color fillColor = percent > 90 ? _overdueRed : (percent > 75 ? _amber : _okGreen);
                using (SolidBrush fill = new SolidBrush(fillColor))
                    e.Graphics.FillRectangle(fill, new Rectangle(barRect.X, barRect.Y, fillWidth, barRect.Height));

                using (Pen pen = new Pen(Color.FromArgb(210, 210, 210)))
                    e.Graphics.DrawRectangle(pen, barRect);

                TextRenderer.DrawText(e.Graphics, percent.ToString("N0") + "%", e.CellStyle.Font,
                    new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height),
                    AppTheme.TextPrimary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
                return;
            }

            if (columnName == "colStatus")
            {
                e.PaintBackground(e.CellBounds, true);
                string status = Convert.ToString(e.FormattedValue);
                Color badgeColor = status == "Overdue" ? _overdueRed : (status == "Inactive" ? Color.Gray : _okGreen);

                Rectangle badge = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + 7, e.CellBounds.Width - 20, e.CellBounds.Height - 14);
                using (GraphicsPath path = RoundedRect(badge, 8))
                using (SolidBrush brush = new SolidBrush(badgeColor))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(e.Graphics, status, e.CellStyle.Font, badge, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void panelAvatar_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = panelAvatar.ClientRectangle;
            r.Inflate(-2, -2);

            using (SolidBrush b = new SolidBrush(Color.FromArgb(255, 224, 178)))
                e.Graphics.FillEllipse(b, r);
            using (Pen p = new Pen(_accent, 1.5f))
                e.Graphics.DrawEllipse(p, r);

            TextRenderer.DrawText(e.Graphics, _avatarInitials, new Font("Segoe UI Semibold", 10F), r, _accent,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void grid_customers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenCustomerProfile();
        }

        private void OpenCustomerProfile()
        {
            if (grid_customers.CurrentRow == null) return;
            int id = SafeInt(grid_customers.CurrentRow.Cells["colId"].Value);
            if (id <= 0) return;

            string fullName = Convert.ToString(grid_customers.CurrentRow.Cells["colCustomerName"].Value);
            frm_customer_detail frm = new frm_customer_detail(id, fullName);
            frm.Show();
        }

        private void btnViewProfile_Click(object sender, EventArgs e)
        {
            OpenCustomerProfile();
        }

        private void btnNewInvoice_Click(object sender, EventArgs e)
        {
            frm_sales frm = new frm_sales();
            frm.Show();
        }

        private void btnReceivePayment_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId <= 0)
            {
                UiMessages.ShowWarning("Please select a customer first.", "يرجى اختيار عميل أولاً.");
                return;
            }

            string customerName = Convert.ToString(grid_customers.CurrentRow.Cells["colCustomerName"].Value);
            using (frm_customer_payment frm = new frm_customer_payment(null, _selectedCustomerId, customerName))
            {
                frm.ShowDialog(this);
            }

            LoadDashboard();
        }
    }
}
