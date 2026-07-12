using pos.Security.Authorization;
using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_financial_periods : Form
    {
        private readonly FinancialPeriodBLL _bll = new FinancialPeriodBLL();
        private readonly FiscalYearBLL _fiscalYearBll = new FiscalYearBLL();
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;
        private int _activeYearId;

        public frm_financial_periods()
        {
            InitializeComponent();
        }

        private void frm_financial_periods_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            AppTheme.ApplyListFormStyle(panelHeader, lblTitle, panelBody, gridPeriods, colPeriodId, colYearId);

            EnsureCurrentUser();
            this.ApplyPermissions(_auth, _currentUser);

            LoadActiveYear();
            LoadPeriods();
        }

        private void EnsureCurrentUser()
        {
            if (_currentUser != null)
            {
                return;
            }

            SystemRole parsedRole = SystemRole.Viewer;
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

        private void LoadActiveYear()
        {
            DataTable dt = _fiscalYearBll.GetActiveFiscalYear();
            if (dt.Rows.Count == 0)
            {
                _activeYearId = 0;
                lblYearCaption.Text = "Active Year: N/A";
                return;
            }

            _activeYearId = Convert.ToInt32(dt.Rows[0]["id"]);
            lblYearCaption.Text = "Active Year: " + Convert.ToString(dt.Rows[0]["name"]);
        }

        private void LoadPeriods()
        {
            if (_activeYearId <= 0)
            {
                gridPeriods.DataSource = null;
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading financial periods...", "جاري تحميل الفترات المالية...")))
                {
                    gridPeriods.AutoGenerateColumns = false;
                    gridPeriods.DataSource = _bll.GetPeriods(_activeYearId);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private bool TryGetSelectedPeriod(out int periodId, out string periodName, out string status)
        {
            periodId = 0;
            periodName = string.Empty;
            status = string.Empty;

            if (gridPeriods.CurrentRow == null)
            {
                return false;
            }

            periodId = Convert.ToInt32(gridPeriods.CurrentRow.Cells["colPeriodId"].Value);
            periodName = Convert.ToString(gridPeriods.CurrentRow.Cells["colPeriodName"].Value);
            status = Convert.ToString(gridPeriods.CurrentRow.Cells["colStatusBadge"].Value);
            return periodId > 0;
        }

        private void btnOpenNewPeriod_Click(object sender, EventArgs e)
        {
            if (_activeYearId <= 0)
            {
                UiMessages.ShowInfo("No active fiscal year found.", "لا توجد سنة مالية نشطة.", "Financial Periods", "الفترات المالية");
                return;
            }

            try
            {
                int newPeriodId;
                using (BusyScope.Show(this, UiMessages.T("Opening new period...", "جاري فتح فترة جديدة...")))
                {
                    newPeriodId = _bll.OpenNewPeriod(_activeYearId);
                }

                if (newPeriodId <= 0)
                {
                    UiMessages.ShowInfo("No new period was created.", "لم يتم إنشاء فترة جديدة.", "Open New Period", "فتح فترة جديدة");
                    return;
                }

                Log.LogAction("Open New Financial Period", "Period ID: " + newPeriodId, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                LoadPeriods();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnSoftClose_Click(object sender, EventArgs e)
        {
            int periodId;
            string periodName;
            string status;
            if (!TryGetSelectedPeriod(out periodId, out periodName, out status))
            {
                UiMessages.ShowInfo("Please select a period.", "يرجى اختيار فترة.", "Financial Periods", "الفترات المالية");
                return;
            }

            if (!string.Equals(status, "Open", StringComparison.OrdinalIgnoreCase))
            {
                UiMessages.ShowInfo("Only open periods can be soft-closed.", "يمكن إغلاق الفترات المفتوحة فقط إغلاقاً أولياً.", "Soft Close", "إغلاق أولي");
                return;
            }

            using (frm_period_close_wizard wizard = new frm_period_close_wizard(periodId, periodName))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    Log.LogAction("Soft Close Financial Period", "Period ID: " + periodId, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    LoadPeriods();
                }
            }
        }

        private void btnHardLock_Click(object sender, EventArgs e)
        {
            int periodId;
            string periodName;
            string status;
            if (!TryGetSelectedPeriod(out periodId, out periodName, out status))
            {
                UiMessages.ShowInfo("Please select a period.", "يرجى اختيار فترة.", "Financial Periods", "الفترات المالية");
                return;
            }

            DialogResult confirm = UiMessages.ConfirmYesNo(
                "Hard lock this period? No entries will be allowed.",
                "هل تريد قفل هذه الفترة نهائياً؟ لن يُسمح بأي قيود.",
                "Hard Lock",
                "قفل نهائي");

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                FinancialPeriodCloseResultModal result;
                using (BusyScope.Show(this, UiMessages.T("Hard locking period...", "جاري القفل النهائي للفترة...")))
                {
                    result = _bll.HardLockPeriod(periodId);
                }

                if (!result.success)
                {
                    UiMessages.ShowInfo(result.message, result.message, "Hard Lock", "قفل نهائي");
                    return;
                }

                Log.LogAction("Hard Lock Financial Period", "Period ID: " + periodId, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                LoadPeriods();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnReopen_Click(object sender, EventArgs e)
        {
            int periodId;
            string periodName;
            string status;
            if (!TryGetSelectedPeriod(out periodId, out periodName, out status))
            {
                UiMessages.ShowInfo("Please select a period.", "يرجى اختيار فترة.", "Financial Periods", "الفترات المالية");
                return;
            }

            bool canReopen = gridPeriods.CurrentRow.Cells["colCanReopen"].Value != DBNull.Value &&
                             Convert.ToBoolean(gridPeriods.CurrentRow.Cells["colCanReopen"].Value);

            if (!canReopen || !string.Equals(status, "Soft-Closed", StringComparison.OrdinalIgnoreCase))
            {
                UiMessages.ShowInfo("Only soft-closed periods can be reopened.", "يمكن إعادة فتح الفترات المغلقة أولياً فقط.", "Reopen Period", "إعادة فتح الفترة");
                return;
            }

            string reason = PromptText("Reopen Reason", "Enter reopen reason:", true);
            if (string.IsNullOrWhiteSpace(reason))
            {
                return;
            }

            string pin = PromptText("Admin Confirmation", "Enter admin password/PIN:", false);
            if (string.IsNullOrWhiteSpace(pin))
            {
                return;
            }

            try
            {
                FinancialPeriodCloseResultModal result;
                using (BusyScope.Show(this, UiMessages.T("Reopening period...", "جاري إعادة فتح الفترة...")))
                {
                    result = _bll.ReopenPeriod(periodId, reason, pin);
                }

                UiMessages.ShowInfo(result.message, result.message, "Reopen Period", "إعادة فتح الفترة");
                if (result.success)
                {
                    Log.LogAction("Reopen Financial Period", "Period ID: " + periodId + ", Reason: " + reason, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    LoadPeriods();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnViewTransactions_Click(object sender, EventArgs e)
        {
            int periodId;
            string periodName;
            string status;
            if (!TryGetSelectedPeriod(out periodId, out periodName, out status))
            {
                UiMessages.ShowInfo("Please select a period.", "يرجى اختيار فترة.", "Financial Periods", "الفترات المالية");
                return;
            }

            try
            {
                DataTable dt;
                using (BusyScope.Show(this, UiMessages.T("Loading period transactions...", "جاري تحميل معاملات الفترة...")))
                {
                    dt = _bll.GetPeriodTransactions(periodId);
                }

                ShowTransactionsPopup(periodName, dt);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnYearEndClose_Click(object sender, EventArgs e)
        {
            if (_activeYearId <= 0)
            {
                UiMessages.ShowInfo("No active fiscal year found.", "لا توجد سنة مالية نشطة.", "Year-End Close", "إقفال السنة");
                return;
            }

            string yearName = lblYearCaption.Text.Replace("Active Year:", string.Empty).Trim();
            using (frm_year_end_close_wizard wizard = new frm_year_end_close_wizard(_activeYearId, yearName))
            {
                wizard.ShowDialog(this);
            }

            LoadPeriods();
        }

        private void ShowTransactionsPopup(string periodName, DataTable dt)
        {
            Form popup = new Form();
            popup.Text = "Period Transactions - " + periodName;
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.Size = new Size(980, 560);

            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.RowHeadersVisible = false;
            grid.DataSource = dt;

            popup.Controls.Add(grid);
            AppTheme.Apply(popup);
            popup.ShowDialog(this);
        }

        private static string PromptText(string title, string labelText, bool multiline)
        {
            Form prompt = new Form();
            prompt.Text = title;
            prompt.Size = new Size(520, multiline ? 240 : 190);
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MinimizeBox = false;
            prompt.MaximizeBox = false;

            Label textLabel = new Label();
            textLabel.Text = labelText;
            textLabel.SetBounds(12, 12, 480, 24);

            TextBox textBox = new TextBox();
            textBox.Multiline = multiline;
            textBox.SetBounds(12, 42, 480, multiline ? 100 : 30);
            textBox.UseSystemPasswordChar = !multiline;

            Button confirmation = new Button();
            confirmation.Text = "OK";
            confirmation.SetBounds(332, multiline ? 150 : 84, 75, 30);
            confirmation.DialogResult = DialogResult.OK;

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.SetBounds(417, multiline ? 150 : 84, 75, 30);
            cancel.DialogResult = DialogResult.Cancel;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            AppTheme.Apply(prompt);

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : string.Empty;
        }

        private void gridPeriods_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || gridPeriods.Columns[e.ColumnIndex].Name != "colStatusBadge")
            {
                return;
            }

            e.PaintBackground(e.CellBounds, true);
            string status = Convert.ToString(e.FormattedValue);

            Color color = AppTheme.Primary;
            if (string.Equals(status, "Open", StringComparison.OrdinalIgnoreCase))
            {
                color = Color.ForestGreen;
            }
            else if (string.Equals(status, "Soft-Closed", StringComparison.OrdinalIgnoreCase))
            {
                color = Color.DarkOrange;
            }
            else if (string.Equals(status, "Hard-Locked", StringComparison.OrdinalIgnoreCase))
            {
                color = Color.Firebrick;
            }

            Rectangle badge = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + 6, e.CellBounds.Width - 20, e.CellBounds.Height - 12);
            using (GraphicsPath path = RoundedRect(badge, 9))
            using (SolidBrush brush = new SolidBrush(color))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillPath(brush, path);
            }

            TextRenderer.DrawText(e.Graphics, status, e.CellStyle.Font, badge, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            e.Handled = true;
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void gridPeriods_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            DataGridView.HitTestInfo hit = gridPeriods.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0)
            {
                gridPeriods.ClearSelection();
                gridPeriods.Rows[hit.RowIndex].Selected = true;
                gridPeriods.CurrentCell = gridPeriods.Rows[hit.RowIndex].Cells["colPeriodName"];
                contextPeriodActions.Show(gridPeriods, e.Location);
            }
        }
    }
}
