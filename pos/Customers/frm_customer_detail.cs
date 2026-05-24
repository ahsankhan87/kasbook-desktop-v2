using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;
using pos.Security.Authorization;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_customer_detail : Form
    {
        public int _customer_id;
        public string _customer_name;

        private string _customerCode = string.Empty;
        private string _avatarInitials = "?";

        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_customer_detail(int customer_id, string customer_name)
        {
            _customer_id = customer_id;
            _customer_name = customer_name;
            InitializeComponent();
        }

        public frm_customer_detail()
        {
            InitializeComponent();
        }

        public void frm_customer_detail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            EnsureCurrentUser();
            StyleForm();

            lbl_title.Text = "Customer Profile";

            dtLedgerTo.Value = DateTime.Today;
            dtLedgerFrom.Value = DateTime.Today.AddMonths(-3);

            LoadAllTabs();
        }

        private void EnsureCurrentUser()
        {
            if (_currentUser != null) return;

            var parsedRole = SystemRole.Viewer;
            Enum.TryParse(UsersModal.logged_in_user_role, true, out parsedRole);
            AppSecurityContext.SetUser(new UserIdentity
            {
                UserId = UsersModal.logged_in_userid,
                BranchId = UsersModal.logged_in_branch_id,
                Username = UsersModal.logged_in_username,
                Role = parsedRole
            });
            _currentUser = AppSecurityContext.User;
        }

        private void StyleForm()
        {
            BackColor = Color.White;
            panelTop.BackColor = Color.White;
            tabProfile.SizeMode = TabSizeMode.Fixed;

            var grids = new[] { gridLedger, gridOutstanding, gridNotes };
            foreach (var grid in grids)
            {
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 235);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
                grid.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
                grid.RowTemplate.Height = 30;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 243, 224);
                grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            }

            pnlKpi1.Paint += CardPaint;
            pnlKpi2.Paint += CardPaint;
            pnlKpi3.Paint += CardPaint;
            pnlKpi4.Paint += CardPaint;

            chkAccountStatus.Visible = _auth.HasPermission(_currentUser, Permissions.Customers_Edit);
            chkAccountStatus.Enabled = chkAccountStatus.Visible;

            gridLedger.RowPrePaint += gridLedger_RowPrePaint;
        }

        private void CardPaint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            if (p == null) return;

            using (var pen = new Pen(Color.FromArgb(230, 230, 230)))
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);

            using (var b = new SolidBrush(Color.FromArgb(245, 127, 23)))
                e.Graphics.FillRectangle(b, 0, 0, 4, p.Height);
        }

        private void LoadAllTabs()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading customer profile...", "جاري تحميل ملف العميل...")))
                {
                    LoadOverviewTab();
                    LoadLedgerTab();
                    LoadOutstandingTab();
                    LoadNotesTab();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Customer Profile", "ملف العميل");
            }
        }

        private void LoadOverviewTab()
        {
            var bll = new CustomerBLL();
            DataTable dt = bll.GetCustomerProfileOverview(_customer_id);
            if (dt == null || dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];
            string fullName = (Convert.ToString(r["first_name"]) + " " + Convert.ToString(r["last_name"])).Trim();
            _customerCode = Convert.ToString(r["customer_code"]);
            _avatarInitials = GetInitials(fullName);
            pnlAvatar.Invalidate();

            lblHeaderName.Text = fullName;
            lblHeaderCode.Text = "Code: " + _customerCode;
            lblHeaderArea.Text = "Area: " + GetAreaFromAddress(Convert.ToString(r["address"]));
            lblHeaderPhone.Text = "Phone: " + Convert.ToString(r["contact_no"]);
            lblHeaderEmail.Text = "Email: " + (string.IsNullOrWhiteSpace(Convert.ToString(r["email"])) ? "-" : Convert.ToString(r["email"]));
            lblHeaderCreditLimit.Text = "Credit Limit: " + ToDecimal(r["credit_limit"]).ToString("N2");

            lblLifetimeSalesValue.Text = ToDecimal(r["lifetime_sales"]).ToString("N2");
            lblTotalPaidValue.Text = ToDecimal(r["total_paid"]).ToString("N2");
            lblOutstandingValue.Text = ToDecimal(r["current_outstanding"]).ToString("N2");
            lblAvailableCreditValue.Text = ToDecimal(r["available_credit"]).ToString("N2");

            chkAccountStatus.CheckedChanged -= chkAccountStatus_CheckedChanged;
            chkAccountStatus.Checked = ToInt(r["status"]) == 1;
            chkAccountStatus.CheckedChanged += chkAccountStatus_CheckedChanged;

            DataTable months = bll.GetCustomerMonthlyPurchaseHistory(_customer_id, 12);
            chartMonthly.DataSource = months;
            chartMonthly.Series["Monthly"].XValueMember = "month_label";
            chartMonthly.Series["Monthly"].YValueMembers = "amount";
            chartMonthly.Series["Monthly"].IsValueShownAsLabel = false;
            chartMonthly.ChartAreas[0].AxisX.Interval = 1;
            chartMonthly.ChartAreas[0].AxisX.LabelStyle.Angle = -30;
            chartMonthly.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartMonthly.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartMonthly.DataBind();
        }

        private void LoadLedgerTab()
        {
            DateTime from = dtLedgerFrom.Value.Date;
            DateTime to = dtLedgerTo.Value.Date;
            if (to < from)
            {
                var tmp = from;
                from = to;
                to = tmp;
            }

            var bll = new CustomerBLL();
            DataTable dt = bll.GetCustomerLedger(_customer_id, from, to);

            decimal dr = 0m;
            decimal cr = 0m;
            foreach (DataRow row in dt.Rows)
            {
                dr += ToDecimal(row["debit"]);
                cr += ToDecimal(row["credit"]);
            }

            DataRow total = dt.NewRow();
            total["reference_no"] = "TOTAL";
            total["debit"] = Math.Round(dr, 2);
            total["credit"] = Math.Round(cr, 2);
            total["running_balance"] = Math.Round(dr - cr, 2);
            dt.Rows.Add(total);

            gridLedger.DataSource = dt;
            lblLedgerTotals.Text = "Totals: DR " + dr.ToString("N2") + " | CR " + cr.ToString("N2") + " | Balance " + (dr - cr).ToString("N2");

            foreach (DataGridViewColumn col in gridLedger.Columns)
            {
                if (col.Name == "colLedgerDebit" || col.Name == "colLedgerCredit" || col.Name == "colLedgerBalance")
                    col.DefaultCellStyle.Format = "N2";
            }
        }

        private void LoadOutstandingTab()
        {
            var bll = new CustomerBLL();
            DataTable dt = bll.GetCustomerOutstandingInvoices(_customer_id);
            gridOutstanding.DataSource = dt;

            foreach (DataGridViewColumn col in gridOutstanding.Columns)
            {
                if (col.Name == "colOutAmount" || col.Name == "colOutPaid" || col.Name == "colOutBalance")
                    col.DefaultCellStyle.Format = "N2";
                if (col.Name == "colOutDate" || col.Name == "colOutDueDate")
                    col.DefaultCellStyle.Format = "yyyy-MM-dd";
            }

            DataTable aging = bll.GetCustomerAgingSummary(_customer_id);
            if (aging != null && aging.Rows.Count > 0)
            {
                DataRow a = aging.Rows[0];
                lblAge0to30Value.Text = ToDecimal(a["bucket_0_30"]).ToString("N2");
                lblAge31to60Value.Text = ToDecimal(a["bucket_31_60"]).ToString("N2");
                lblAge61to90Value.Text = ToDecimal(a["bucket_61_90"]).ToString("N2");
                lblAge90PlusValue.Text = ToDecimal(a["bucket_90_plus"]).ToString("N2");
            }
            else
            {
                lblAge0to30Value.Text = "0.00";
                lblAge31to60Value.Text = "0.00";
                lblAge61to90Value.Text = "0.00";
                lblAge90PlusValue.Text = "0.00";
            }
        }

        private void LoadNotesTab()
        {
            var bll = new CustomerBLL();
            DataTable dt = bll.GetCustomerNotes(_customer_id);
            gridNotes.DataSource = dt;
            if (gridNotes.Columns.Contains("colNoteDate"))
                gridNotes.Columns["colNoteDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

        private void gridLedger_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (gridLedger.Rows[e.RowIndex].IsNewRow) return;

            var typeVal = Convert.ToString(gridLedger.Rows[e.RowIndex].Cells["colLedgerType"].Value);
            var refVal = Convert.ToString(gridLedger.Rows[e.RowIndex].Cells["colLedgerRef"].Value);

            if (string.Equals(refVal, "TOTAL", StringComparison.OrdinalIgnoreCase))
            {
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(gridLedger.Font, FontStyle.Bold);
                return;
            }

            if (string.Equals(typeVal, "Invoice", StringComparison.OrdinalIgnoreCase))
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(232, 244, 253);
            else if (string.Equals(typeVal, "Payment", StringComparison.OrdinalIgnoreCase))
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            else
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }

        private void btnLoadLedger_Click(object sender, EventArgs e)
        {
            LoadLedgerTab();
        }

        private void btnPrintLedger_Click(object sender, EventArgs e)
        {
            var table = gridLedger.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                UiMessages.ShowWarning("No ledger data to print.", "لا توجد بيانات كشف للطباعة.");
                return;
            }

            var dict = new Dictionary<string, DataTable>
            {
                { "Ledger Statement", table.Copy() }
            };

            using (var viewer = new DataGridReportViewerForm("Customer Ledger - " + _customer_name, dict))
            {
                viewer.ShowDialog(this);
            }
        }

        private void btnReceivePayment_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_customer_payment(null, _customer_id, _customer_name))
            {
                frm.ShowDialog(this);
            }

            LoadAllTabs();
        }

        private void btnAddNote_Click(object sender, EventArgs e)
        {
            string noteText = PromptForNote();
            if (string.IsNullOrWhiteSpace(noteText))
                return;

            var bll = new CustomerBLL();
            bll.AddCustomerNote(_customer_id, noteText.Trim());
            LoadNotesTab();
        }

        private void btnReminder_Click(object sender, EventArgs e)
        {
            UiMessages.ShowInfo(
                "Reminder queued for SMS/WhatsApp integration.",
                "تمت جدولة التذكير لتكامل الرسائل القصيرة/واتساب.",
                "Reminder",
                "تذكير");
        }

        private void chkAccountStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (!_auth.HasPermission(_currentUser, Permissions.Customers_Edit))
                return;

            var confirm = UiMessages.ConfirmYesNo(
                "Change customer account status?",
                "هل تريد تغيير حالة حساب العميل؟",
                "Confirm",
                "تأكيد");

            if (confirm != DialogResult.Yes)
            {
                chkAccountStatus.CheckedChanged -= chkAccountStatus_CheckedChanged;
                chkAccountStatus.Checked = !chkAccountStatus.Checked;
                chkAccountStatus.CheckedChanged += chkAccountStatus_CheckedChanged;
                return;
            }

            try
            {
                var bll = new CustomerBLL();
                bll.UpdateCustomerStatus(_customer_id, chkAccountStatus.Checked);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void tabProfile_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = tabProfile.TabPages[e.Index];
            Rectangle rect = e.Bounds;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            using (Brush b = new SolidBrush(selected ? Color.FromArgb(245, 127, 23) : Color.White))
                e.Graphics.FillRectangle(b, rect);

            using (Pen p = new Pen(Color.FromArgb(220, 220, 220)))
                e.Graphics.DrawRectangle(p, rect);

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                new Font("Segoe UI Semibold", 9F),
                rect,
                selected ? Color.White : Color.FromArgb(50, 49, 48),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void pnlAvatar_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = pnlAvatar.ClientRectangle;
            r.Inflate(-8, -8);

            using (var fill = new SolidBrush(Color.FromArgb(255, 224, 178)))
                e.Graphics.FillEllipse(fill, r);
            using (var pen = new Pen(Color.FromArgb(245, 127, 23), 2f))
                e.Graphics.DrawEllipse(pen, r);

            TextRenderer.DrawText(
                e.Graphics,
                _avatarInitials,
                new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                r,
                Color.FromArgb(245, 127, 23),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private decimal ToDecimal(object value)
        {
            decimal d;
            decimal.TryParse(Convert.ToString(value), out d);
            return d;
        }

        private int ToInt(object value)
        {
            int x;
            int.TryParse(Convert.ToString(value), out x);
            return x;
        }

        private string GetAreaFromAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return "-";
            string[] p = address.Split(',');
            return p.Length == 0 ? "-" : p[0].Trim();
        }

        private string PromptForNote()
        {
            Form f = new Form();
            f.Text = "Add Note";
            f.StartPosition = FormStartPosition.CenterParent;
            f.Size = new Size(520, 230);
            f.MinimumSize = new Size(520, 230);
            f.MaximizeBox = false;
            f.MinimizeBox = false;

            TextBox txt = new TextBox();
            txt.Multiline = true;
            txt.ScrollBars = ScrollBars.Vertical;
            txt.Dock = DockStyle.Top;
            txt.Height = 130;

            Panel pnl = new Panel { Dock = DockStyle.Bottom, Height = 48 };
            Button ok = new Button { Text = "Save", DialogResult = DialogResult.OK, Width = 100, Height = 30, Left = 296, Top = 9 };
            Button cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 100, Height = 30, Left = 402, Top = 9 };
            pnl.Controls.Add(ok);
            pnl.Controls.Add(cancel);

            f.Controls.Add(txt);
            f.Controls.Add(pnl);
            f.AcceptButton = ok;
            f.CancelButton = cancel;

            return f.ShowDialog(this) == DialogResult.OK ? txt.Text : string.Empty;
        }
    }
}
