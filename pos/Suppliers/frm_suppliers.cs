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
    public partial class frm_suppliers : Form
    {
        private readonly Color _accent = Color.FromArgb(0, 105, 92);
        private readonly Color _accentDark = Color.FromArgb(0, 77, 64);
        private readonly Color _overdueRed = Color.FromArgb(198, 40, 40);
        private readonly Color _amber = Color.FromArgb(245, 124, 0);
        private readonly Color _okGreen = Color.FromArgb(46, 125, 50);

        private DataTable _dashboardSource = new DataTable();
        private DataView _dashboardView;

        private int _selectedSupplierId;
        private string _selectedSupplierName = string.Empty;
        private string _avatarInitials = "?";
        private int _detailsExpandedWidth = 280;
        private bool _targetDetailsOpen;

        public frm_suppliers()
        {
            InitializeComponent();
        }

        public void frm_suppliers_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            ApplyDashboardTheme();
            InitializeFilters();
            LoadDashboard();
        }

        public void load_Suppliers_grid()
        {
            LoadDashboard();
        }

        private void ApplyDashboardTheme()
        {
            BackColor = Color.White;
            pnlTop.BackColor = Color.White;
            pnlBottom.BackColor = Color.White;
            lblTitle.ForeColor = AppTheme.TextPrimary;

            cardTotalSuppliers.Paint += KpiCard_Paint;
            cardTotalPayables.Paint += KpiCard_Paint;
            cardOverduePayables.Paint += KpiCard_Paint;
            cardThisMonth.Paint += KpiCard_Paint;

            splitMain.Panel2MinSize = 0;
            splitMain.SplitterDistance = splitMain.Width - 8;

            grid_suppliers.EnableHeadersVisualStyles = false;
            grid_suppliers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 245, 243);
            grid_suppliers.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid_suppliers.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_suppliers.RowTemplate.Height = 32;
            grid_suppliers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            grid_suppliers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 241);
            grid_suppliers.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid_suppliers.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_suppliers.AutoGenerateColumns = false;

            pnlDetails.BackColor = Color.White;
            pnlDetails.BorderStyle = BorderStyle.FixedSingle;

            btnViewProfile.BackColor = Color.White;
            btnViewProfile.FlatStyle = FlatStyle.Flat;
            btnViewProfile.FlatAppearance.BorderColor = _accent;

            btnNewBill.BackColor = _accent;
            btnNewBill.ForeColor = Color.White;
            btnNewBill.FlatStyle = FlatStyle.Flat;
            btnNewBill.FlatAppearance.BorderSize = 0;

            btnMakePayment.BackColor = _accentDark;
            btnMakePayment.ForeColor = Color.White;
            btnMakePayment.FlatStyle = FlatStyle.Flat;
            btnMakePayment.FlatAppearance.BorderSize = 0;

            detailSlideTimer.Tick += detailSlideTimer_Tick;
        }

        private void KpiCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            using (Pen pen = new Pen(Color.FromArgb(235, 235, 235)))
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);

            using (SolidBrush brush = new SolidBrush(_accent))
                e.Graphics.FillRectangle(brush, 0, 0, 4, card.Height);
        }

        private void InitializeFilters()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[] { "All", "Active", "Inactive", "Overdue" });
            cmbStatus.SelectedIndex = 0;

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All Categories");
            cmbCategory.SelectedIndex = 0;
        }

        private void LoadDashboard()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading supplier dashboard...", "جاري تحميل لوحة الموردين...")))
                {
                    SupplierBLL bll = new SupplierBLL();
                    DataTable raw = bll.GetSupplierSummaryDashboard();
                    _dashboardSource = raw ?? new DataTable();
                    _dashboardView = _dashboardSource.DefaultView;
                    grid_suppliers.DataSource = _dashboardView;

                    PopulateCategoryFilter();
                    LoadKpis();
                    ApplyFilters();

                    if (grid_suppliers.Rows.Count > 0)
                    {
                        grid_suppliers.Rows[0].Selected = true;
                        UpdateSupplierDetailsFromSelectedRow();
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
                UiMessages.ShowError(ex.Message, ex.Message, "Suppliers", "الموردين");
            }
        }

        private void LoadKpis()
        {
            SupplierBLL bll = new SupplierBLL();
            DataTable dt = bll.GetSupplierDashboardKPIs();

            if (dt == null || dt.Rows.Count == 0)
            {
                lblTotalSuppliersValue.Text = "0";
                lblTotalPayablesValue.Text = "0.00";
                lblOverduePayablesValue.Text = "0.00";
                lblThisMonthValue.Text = "0.00";
                lblThisMonthTrend.Text = "▲ 0 vs PM";
                return;
            }

            DataRow r = dt.Rows[0];
            int totalSuppliers = ToInt(GetColumn(r, "TotalSuppliers"));
            decimal totalPayables = ToDecimal(GetColumn(r, "TotalPayables"));
            decimal overdue = ToDecimal(GetColumn(r, "OverduePayables"));
            decimal thisMonth = ToDecimal(GetColumn(r, "TotalPurchasesThisMonth"));
            decimal prevMonth = ToDecimal(GetColumn(r, "TotalPurchasesPrevMonth"));

            lblTotalSuppliersValue.Text = totalSuppliers.ToString("N0");
            lblTotalPayablesValue.Text = totalPayables.ToString("N2");
            lblOverduePayablesValue.Text = overdue.ToString("N2");
            lblThisMonthValue.Text = thisMonth.ToString("N2");

            decimal diff = thisMonth - prevMonth;
            lblThisMonthTrend.Text = (diff >= 0 ? "▲ " : "▼ ") + Math.Abs(diff).ToString("N2") + " vs PM";
            lblThisMonthTrend.ForeColor = diff >= 0 ? _okGreen : _overdueRed;
        }

        private object GetColumn(DataRow row, string name)
        {
            return row.Table.Columns.Contains(name) ? row[name] : 0;
        }

        private void PopulateCategoryFilter()
        {
            string selected = cmbCategory.SelectedItem == null ? "All Categories" : cmbCategory.SelectedItem.ToString();
            var categories = _dashboardSource.AsEnumerable()
                .Select(r => Convert.ToString(r["category"]))
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All Categories");
            foreach (var cat in categories)
                cmbCategory.Items.Add(cat);

            int index = cmbCategory.Items.IndexOf(selected);
            cmbCategory.SelectedIndex = index >= 0 ? index : 0;
        }

        private void ApplyFilters()
        {
            if (_dashboardView == null) return;

            string text = (txt_search.Text ?? string.Empty).Trim().Replace("'", "''");
            string category = cmbCategory.SelectedItem == null ? "All Categories" : cmbCategory.SelectedItem.ToString().Replace("'", "''");
            string status = cmbStatus.SelectedItem == null ? "All" : cmbStatus.SelectedItem.ToString();

            string filter = string.Empty;

            if (!string.IsNullOrWhiteSpace(text))
                filter = "(supplier_name LIKE '%" + text + "%' OR supplier_code LIKE '%" + text + "%')";

            if (!string.Equals(category, "All Categories", StringComparison.OrdinalIgnoreCase))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += "category = '" + category + "'";
            }

            if (!string.Equals(status, "All", StringComparison.OrdinalIgnoreCase))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += "status_text = '" + status.Replace("'", "''") + "'";
            }

            _dashboardView.RowFilter = filter;
            UpdateRowNumbers();
            UpdateBottomSummary();

            if (grid_suppliers.Rows.Count == 0)
            {
                ResetDetails();
                ToggleDetailsPanel(false);
            }
        }

        private void UpdateRowNumbers()
        {
            int n = 1;
            foreach (DataRowView rv in _dashboardView)
                rv["row_no"] = n++;
        }

        private void UpdateBottomSummary()
        {
            DataTable filtered = _dashboardView.ToTable();
            int count = filtered.Rows.Count;
            decimal payable = filtered.AsEnumerable().Sum(r => ToDecimal(r["payable_balance"]));
            lblBottomSummary.Text = count.ToString("N0") + " suppliers shown | Payable: " + payable.ToString("N2");
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void grid_suppliers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSupplierDetailsFromSelectedRow();
        }

        private void UpdateSupplierDetailsFromSelectedRow()
        {
            if (grid_suppliers.CurrentRow == null)
            {
                ResetDetails();
                ToggleDetailsPanel(false);
                return;
            }

            DataGridViewRow row = grid_suppliers.CurrentRow;
            _selectedSupplierId = ToInt(row.Cells["colId"].Value);
            _selectedSupplierName = Convert.ToString(row.Cells["colSupplierName"].Value);
            string supplierCode = Convert.ToString(row.Cells["colSupplierCode"].Value);

            lblSupplierHeader.Text = _selectedSupplierName + " (" + supplierCode + ")";
            _avatarInitials = GetInitials(_selectedSupplierName);
            panelAvatar.Invalidate();

            lblPhoneValue.Text = "Phone: " + Convert.ToString(row.Cells["colPhone"].Value);
            lblAddressValue.Text = "Address: " + Convert.ToString(row.Cells["colAddress"].Value);
            lblCategoryValue.Text = "Category: " + Convert.ToString(row.Cells["colCategory"].Value);

            lblPurchasedValue.Text = ToDecimal(row.Cells["colTotalPurchases"].Value).ToString("N2");
            lblPaidValue.Text = ToDecimal(row.Cells["colTotalPaid"].Value).ToString("N2");
            lblPayableValue.Text = ToDecimal(row.Cells["colPayable"].Value).ToString("N2");
            lblCreditDaysValue.Text = ToInt(row.Cells["colCreditDays"].Value).ToString("N0");
            lblCreditLimitValue.Text = ToDecimal(row.Cells["colCreditLimit"].Value).ToString("N2");

            LoadRecentBills(_selectedSupplierId);
            ToggleDetailsPanel(true);
        }

        private void LoadRecentBills(int supplierId)
        {
            lstBills.Items.Clear();
            if (supplierId <= 0) return;

            try
            {
                SupplierBLL bll = new SupplierBLL();
                DataTable dt = bll.GetSupplierRecentBills(supplierId, 5);

                foreach (DataRow r in dt.Rows)
                {
                    DateTime d = ToDateTime(r.Table.Columns.Contains("purchase_date") ? r["purchase_date"] : DBNull.Value);
                    string invoice = Convert.ToString(r.Table.Columns.Contains("invoice_no") ? r["invoice_no"] : string.Empty);
                    decimal bal = ToDecimal(r.Table.Columns.Contains("balance") ? r["balance"] : 0m);
                    lstBills.Items.Add(d.ToString("yyyy-MM-dd") + " | " + invoice + " | " + bal.ToString("N2"));
                }

                if (lstBills.Items.Count == 0)
                    lstBills.Items.Add("No bills");
            }
            catch
            {
                lstBills.Items.Add("Unable to load bills");
            }
        }

        private void ResetDetails()
        {
            _selectedSupplierId = 0;
            _selectedSupplierName = string.Empty;
            lblSupplierHeader.Text = "Select supplier";
            _avatarInitials = "?";
            panelAvatar.Invalidate();

            lblPhoneValue.Text = "Phone: -";
            lblAddressValue.Text = "Address: -";
            lblCategoryValue.Text = "Category: -";
            lblPurchasedValue.Text = "0.00";
            lblPaidValue.Text = "0.00";
            lblPayableValue.Text = "0.00";
            lblCreditDaysValue.Text = "0";
            lblCreditLimitValue.Text = "0.00";
            lstBills.Items.Clear();
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

        private void grid_suppliers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var col = grid_suppliers.Columns[e.ColumnIndex];

            if (col.Name == "colTotalPurchases" || col.Name == "colPayable")
            {
                e.Value = ToDecimal(e.Value).ToString("N2");
                e.FormattingApplied = true;
            }

            if (col.Name == "colLastBill")
            {
                DateTime d = ToDateTime(e.Value);
                e.Value = d == DateTime.MinValue ? "-" : d.ToString("yyyy-MM-dd");
                e.FormattingApplied = true;
            }

            if (col.Name == "colPayable")
            {
                bool overdue = ToInt(grid_suppliers.Rows[e.RowIndex].Cells["colIsOverdue"].Value) == 1;
                decimal payable = ToDecimal(grid_suppliers.Rows[e.RowIndex].Cells["colPayable"].Value);

                if (overdue && payable > 0)
                    e.CellStyle.ForeColor = _overdueRed;
                else if (payable > 0)
                    e.CellStyle.ForeColor = _amber;
                else
                    e.CellStyle.ForeColor = _okGreen;
            }

            if (col.Name == "colDaysSince")
            {
                int days = ToInt(e.Value);
                e.Value = days <= 0 ? "-" : days.ToString("N0");
                e.FormattingApplied = true;

                if (days > 180)
                    e.CellStyle.ForeColor = _overdueRed;
                else if (days > 60)
                    e.CellStyle.ForeColor = _amber;
                else
                    e.CellStyle.ForeColor = AppTheme.TextPrimary;
            }
        }

        private void grid_suppliers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (grid_suppliers.Columns[e.ColumnIndex].Name == "colStatus")
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
            r.Inflate(-4, -4);

            using (var fill = new SolidBrush(Color.FromArgb(178, 223, 219)))
                e.Graphics.FillEllipse(fill, r);
            using (var pen = new Pen(_accent, 2f))
                e.Graphics.DrawEllipse(pen, r);

            TextRenderer.DrawText(
                e.Graphics,
                _avatarInitials,
                new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                r,
                _accent,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            string[] parts = name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private int ToInt(object value)
        {
            int x;
            int.TryParse(Convert.ToString(value), out x);
            return x;
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

        private void grid_suppliers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_selectedSupplierId <= 0) return;
            using (frm_supplier_detail frm = new frm_supplier_detail(_selectedSupplierId, _selectedSupplierName))
            {
                frm.ShowDialog(this);
            }
        }

        private void btnViewProfile_Click(object sender, EventArgs e)
        {
            if (_selectedSupplierId <= 0)
            {
                UiMessages.ShowWarning("Please select a supplier.", "يرجى اختيار مورد.");
                return;
            }

            using (frm_supplier_detail frm = new frm_supplier_detail(_selectedSupplierId, _selectedSupplierName))
            {
                frm.ShowDialog(this);
            }
        }

        private void btnNewBill_Click(object sender, EventArgs e)
        {
            frm_purchases frm = new frm_purchases();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            if (_selectedSupplierId <= 0)
            {
                UiMessages.ShowWarning("Please select a supplier.", "يرجى اختيار مورد.");
                return;
            }

            using (frm_supplier_payment frm = new frm_supplier_payment(null, _selectedSupplierId, _selectedSupplierName))
            {
                frm.ShowDialog(this);
            }

            LoadDashboard();
        }
    }
}
