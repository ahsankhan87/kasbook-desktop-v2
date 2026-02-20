using POS.BLL;
using POS.Core;
using pos.UI;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace pos
{
    public partial class Login : Form
    {
        private static readonly Font Label2Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        private static readonly Font SubTextFont = new Font("Segoe UI", 13F, FontStyle.Regular);
        private static readonly Font Label1Font = new Font("Segoe UI", 8F, FontStyle.Regular);
        private static readonly Font LinkLabel2Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        private static readonly Font HeadingFont = new Font("Segoe UI Semibold", 15F, FontStyle.Regular);
        private static readonly Font LoginButtonFont = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
        private static readonly Font RegisterButtonFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
        private static readonly Font CloseButtonFont = new Font("Segoe UI", 13F, FontStyle.Regular);
        private static readonly Font ForgetPasswordFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        private static readonly Font SupportFont = new Font("Segoe UI", 8F, FontStyle.Regular);
        private static readonly Font InputFont = new Font("Segoe UI", 11F, FontStyle.Regular);

        public Login()
        {
            InitializeComponent();
        }

        
        private void Frm_Login_Load(object sender, EventArgs e)
        {
            StyleLoginForm();

            TxtUsername.Focus();
            this.ActiveControl = TxtUsername;
            ActivateTxtUsername();

            if (is_company_exist())
                BtnRegister.Visible = false;
            else
                BtnRegister.Visible = true;
        }

        // ────────────────────────────────────────────────────────────────────
        //  LOGIN FORM STYLING
        // ────────────────────────────────────────────────────────────────────
        private void StyleLoginForm()
        {
            var originalClientSize = this.ClientSize;
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;

            // ── Form ──────────────────────────────────────────────────────
            this.BackColor = AppTheme.Surface;
            this.Font      = AppTheme.FontDefault;

            // ── Left brand panel (primary blue) ───────────────────────────
            panel1.BackColor = AppTheme.Primary;
            panel1.Padding   = new Padding(24, 20, 24, 20);

            label2.Font      = Label2Font;          // app name
            label2.ForeColor = Color.White;
            label2.BackColor = Color.Transparent;
            // Center label2 horizontally below the logo
            label2.AutoSize     = false;
            label2.TextAlign    = ContentAlignment.MiddleCenter;
            label2.Left         = 0;
            label2.Width        = panel1.ClientSize.Width;
            label2.Height       = 36;
            label2.Top          = pictureBox3.Bottom + 8;

            Color subTextColor = Color.FromArgb(210, 232, 255);
            foreach (Label lbl in new[] { label3, label4, label5, label6 })
            {
                lbl.Font      = SubTextFont;
                lbl.ForeColor = subTextColor;
                lbl.BackColor = Color.Transparent;
            }

            label1.Font      = Label1Font;
            label1.ForeColor = Color.FromArgb(160, 200, 240);
            label1.BackColor = Color.Transparent;

            linkLabel2.Font            = LinkLabel2Font;
            linkLabel2.LinkColor       = Color.FromArgb(180, 220, 255);
            linkLabel2.ActiveLinkColor = Color.White;
            linkLabel2.BackColor       = Color.Transparent;

            // Divider accent line below logo — drawn by painting panel1
            panel1.Paint -= Panel1_Paint;
            panel1.Paint += Panel1_Paint;

            // ── Right form panel ──────────────────────────────────────────
            panel2.BackColor   = AppTheme.Surface;
            panel2.BorderStyle = BorderStyle.None;

            // Heading
            label7.Font      = HeadingFont;
            label7.ForeColor = AppTheme.TextPrimary;
            label7.BackColor = AppTheme.Surface;
            label7.Text      = "Sign In";

            // Input panels — set to inactive state initially
            StyleInputPanel(panel3, TxtUsername, active: false);
            StyleInputPanel(panel4, TxtPassword, active: false);

            // Login button
            BtnLogin.FlatStyle  = FlatStyle.Flat;
            BtnLogin.FlatAppearance.BorderSize         = 0;
            BtnLogin.FlatAppearance.MouseOverBackColor = AppTheme.PrimaryDark;
            BtnLogin.FlatAppearance.MouseDownBackColor = AppTheme.PrimaryDarker;
            BtnLogin.BackColor  = AppTheme.Primary;
            BtnLogin.ForeColor  = Color.White;
            BtnLogin.Font       = LoginButtonFont;
            BtnLogin.Cursor     = Cursors.Hand;
            BtnLogin.UseVisualStyleBackColor = false;
            BtnLogin.Text       = "Sign In";

            // Register button
            BtnRegister.FlatStyle  = FlatStyle.Flat;
            BtnRegister.FlatAppearance.BorderSize = 0;
            BtnRegister.BackColor  = AppTheme.Accent;
            BtnRegister.ForeColor  = Color.White;
            BtnRegister.Font       = RegisterButtonFont;
            BtnRegister.Cursor     = Cursors.Hand;
            BtnRegister.UseVisualStyleBackColor = false;

            // Close button — minimal ✕
            Btn_close.FlatStyle  = FlatStyle.Flat;
            Btn_close.FlatAppearance.BorderSize         = 0;
            Btn_close.FlatAppearance.MouseOverBackColor = AppTheme.DangerLight;
            Btn_close.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 190, 190);
            Btn_close.BackColor  = Color.Transparent;
            Btn_close.ForeColor  = AppTheme.TextSecondary;
            Btn_close.Font       = CloseButtonFont;
            Btn_close.Cursor     = Cursors.Hand;
            Btn_close.Text       = "✕";

            // Forget password link-style button
            Btn_forget_password.FlatStyle  = FlatStyle.Flat;
            Btn_forget_password.FlatAppearance.BorderSize = 0;
            Btn_forget_password.BackColor  = Color.Transparent;
            Btn_forget_password.ForeColor  = AppTheme.Primary;
            Btn_forget_password.Font       = ForgetPasswordFont;
            Btn_forget_password.Cursor     = Cursors.Hand;

            // Support section
            foreach (Label lbl in new[] { label8, label9, label10, label11 })
            {
                lbl.Font      = SupportFont;
                lbl.ForeColor = AppTheme.TextDisabled;
                lbl.BackColor = AppTheme.Surface;
            }
            label8.ForeColor = AppTheme.TextSecondary;

            linkLabel1.Font            = LinkLabel2Font;
            linkLabel1.LinkColor       = AppTheme.Primary;
            linkLabel1.ActiveLinkColor = AppTheme.PrimaryDark;
            linkLabel1.BackColor       = AppTheme.Surface;

            this.ClientSize = originalClientSize;
            this.ResumeLayout(true);
        }

        // Accent divider on the blue brand panel
        private void Panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int y = pictureBox3.Bottom + 12;
            using (var pen = new System.Drawing.Pen(Color.FromArgb(80, Color.White), 1))
                e.Graphics.DrawLine(pen, 20, y, panel1.Width - 20, y);
        }

        // Style an input panel; active=true shows a Primary blue left accent border
        private static void StyleInputPanel(Panel pnl, TextBox txt, bool active)
        {
            Color bg = active ? AppTheme.PrimarySubtle : AppTheme.Background;

            pnl.BorderStyle = BorderStyle.None;
            pnl.BackColor   = bg;
            pnl.Padding     = new Padding(0);

            txt.BackColor   = bg;
            txt.ForeColor   = AppTheme.TextPrimary;
            txt.Font        = InputFont;
            txt.BorderStyle = BorderStyle.None;

            foreach (Control child in pnl.Controls)
                if (child is PictureBox pb) pb.BackColor = bg;

            // Paint a 2 px bottom-border accent to show focus state
            pnl.Paint -= InputPanel_Paint;
            pnl.Paint += InputPanel_Paint;
        }

        // Owner-drawn bottom border for the input panels
        private static void InputPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            bool active = pnl.BackColor == AppTheme.PrimarySubtle;
            Color lineColor = active ? AppTheme.Primary : AppTheme.BorderStrong;
            int thickness  = active ? 2 : 1;
            using (var pen = new System.Drawing.Pen(lineColor, thickness))
                e.Graphics.DrawLine(pen, 0, pnl.Height - thickness,
                    pnl.Width, pnl.Height - thickness);
        }

        private void ActivateTxtUsername()
        {
            StyleInputPanel(panel3, TxtUsername, active: true);
            StyleInputPanel(panel4, TxtPassword, active: false);
            panel3.Invalidate();
            panel4.Invalidate();
        }
        private void ActivateTxtPassword()
        {
            StyleInputPanel(panel4, TxtPassword, active: true);
            StyleInputPanel(panel3, TxtUsername, active: false);
            panel4.Invalidate();
            panel3.Invalidate();
        }

        private void TxtUsername_Click(object sender, EventArgs e)
        {
            this.ActivateTxtUsername();
        }

        private void TxtPassword_Click(object sender, EventArgs e)
        {
            ActivateTxtPassword();
        }
        private void Btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtUsername_Enter(object sender, EventArgs e)
        {
            this.ActivateTxtUsername();
        }

        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            ActivateTxtPassword();
        }

        private void Bunifu_btn_submit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Login successful");
        }

        private void Frm_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                        SendKeys.Send("{TAB}");
            }
        }

        private enum SubscriptionPromptResult
        {
            Continue,
            Renew,
            Cancel
        }

        private SubscriptionPromptResult ShowSubscriptionExpiryPrompt(DateTime expiryDate, double daysLeft)
        {
            using (var dlg = new Form())
            using (var layout = new TableLayoutPanel())
            using (var lbl = new Label())
            using (var pnlButtons = new FlowLayoutPanel())
            using (var btnContinue = new Button())
            using (var btnRenew = new Button())
            using (var btnCancel = new Button())
            {
                dlg.Text = "Software Expiration";
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.MinimizeBox = false;
                dlg.MaximizeBox = false;
                dlg.ShowInTaskbar = false;
                dlg.Padding = new Padding(12);
                dlg.KeyPreview = true;

                // deterministic size so the buttons don't collapse/disappear
                dlg.ClientSize = new Size(520, 220);
                dlg.MinimumSize = new Size(520, 220);

                var daysText = daysLeft < 0 ? "0" : Math.Ceiling(daysLeft).ToString("0");

                lbl.Dock = DockStyle.Fill;
                lbl.AutoSize = false;
                lbl.TextAlign = ContentAlignment.TopLeft;
                lbl.Text =
                    "Your account will expire soon." + Environment.NewLine + Environment.NewLine +
                    "Expiry date: " + FormatDateForPrompt(expiryDate) + Environment.NewLine +
                    "Days left: " + daysText + Environment.NewLine + Environment.NewLine +
                    "Choose an option:";

                pnlButtons.FlowDirection = FlowDirection.RightToLeft;
                pnlButtons.Dock = DockStyle.Fill;
                pnlButtons.WrapContents = false;
                pnlButtons.AutoSize = false;
                pnlButtons.Padding = new Padding(0);

                btnContinue.Text = "Continue";
                btnContinue.AutoSize = true;
                btnContinue.TabIndex = 0;
                btnContinue.UseVisualStyleBackColor = true;

                btnRenew.Text = "Renew";
                btnRenew.AutoSize = true;
                btnRenew.TabIndex = 1;
                btnRenew.UseVisualStyleBackColor = true;

                btnCancel.Text = "Cancel";
                btnCancel.AutoSize = true;
                btnCancel.TabIndex = 2;
                btnCancel.UseVisualStyleBackColor = true;

                // IMPORTANT: ensure AcceptButton works by having the button on the form and focusable,
                // then focus it when dialog is shown.
                dlg.Shown += (s, e) =>
                {
                    dlg.AcceptButton = btnContinue;
                    dlg.CancelButton = btnCancel;
                    btnContinue.Focus();
                    btnContinue.Select();
                };

                SubscriptionPromptResult result = SubscriptionPromptResult.Cancel;

                btnContinue.Click += (s, e) =>
                {
                    result = SubscriptionPromptResult.Continue;
                    dlg.DialogResult = DialogResult.OK;
                    dlg.Close();
                };
                btnRenew.Click += (s, e) =>
                {
                    result = SubscriptionPromptResult.Renew;
                    dlg.DialogResult = DialogResult.OK;
                    dlg.Close();
                };
                btnCancel.Click += (s, e) =>
                {
                    result = SubscriptionPromptResult.Cancel;
                    dlg.DialogResult = DialogResult.Cancel;
                    dlg.Close();
                };

                // fallback: make Enter act like Continue even if focus is weird
                dlg.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        btnContinue.PerformClick();
                    }
                };

                pnlButtons.Controls.Add(btnCancel);
                pnlButtons.Controls.Add(btnRenew);
                pnlButtons.Controls.Add(btnContinue);

                layout.Dock = DockStyle.Fill;
                layout.ColumnCount = 1;
                layout.RowCount = 2;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45f));

                layout.Controls.Add(lbl, 0, 0);
                layout.Controls.Add(pnlButtons, 0, 1);

                dlg.Controls.Add(layout);

                dlg.ShowDialog(this);
                return result;
            }
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            using (pos.UI.Busy.BusyScope.Show(this, "Signing in..."))
            {
                try
                {
                    // Run heavy/IO work off the UI thread.
                    var loginResult = await Task.Run(() =>
                    {
                        // ---------- NOTE ----------
                        // This block MUST NOT touch any UI components.
                        // --------------------------

                        var systemID_obj = new HardwareIdentifier();
                        string systemID = systemID_obj.GetUniqueHardwareId();

                        var company_obj = new CompaniesBLL();
                        DataTable company_dt = company_obj.GetCompany();

                        int locked = 1;
                        int company_id = 0;
                        string company_name = "";
                        string subscriptionKey = "";
                        bool useZatca = false;

                        foreach (DataRow dr_company in company_dt.Rows)
                        {
                            company_id = Convert.ToInt32(dr_company["id"].ToString());
                            company_name = dr_company["name"].ToString();
                            locked = Convert.ToInt32(dr_company["locked"]);
                            subscriptionKey = dr_company["subscriptionKey"].ToString();
                            useZatca = !(string.IsNullOrEmpty(dr_company["useZatcaEInvoice"].ToString())) && Convert.ToBoolean(dr_company["useZatcaEInvoice"]);
                        }

                        if (locked > 0)
                        {
                            return new
                            {
                                Locked = true,
                                CompanyId = company_id,
                                CompanyName = company_name,
                                SubscriptionKey = subscriptionKey,
                                UseZatca = useZatca,
                                VerifyOk = false,
                                ExpiryDate = DateTime.MinValue,
                                UserId = 0
                            };
                        }

                        var subcription_obj = new Subscription();
                        bool verifyOk = subcription_obj.VerifySubscriptionKey(company_id, subscriptionKey, out DateTime expiryDate, systemID);

                        var userModal_obj = new UsersModal
                        {
                            username = TxtUsername.Text.Trim(),
                            password = TxtPassword.Text.Trim()
                        };

                        var user_obj = new UsersBLL();
                        int user_id = user_obj.Login(userModal_obj);

                        return new
                        {
                            Locked = false,
                            CompanyId = company_id,
                            CompanyName = company_name,
                            SubscriptionKey = subscriptionKey,
                            UseZatca = useZatca,
                            VerifyOk = verifyOk,
                            ExpiryDate = expiryDate,
                            UserId = user_id
                        };
                    });

                    // Apply company info to global state (UI thread)
                    UsersModal.loggedIncompanyID = loginResult.CompanyId;
                    UsersModal.logged_in_company_name = loginResult.CompanyName;
                    UsersModal.useZatcaEInvoice = loginResult.UseZatca;

                    if (loginResult.Locked)
                    {
                        var dr = MessageBox.Show(
                            "Your account has expired. Please contact your software provider for renewal. (khybersoft.com) OR click yes to re-new account",
                            "Software Expiration",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Warning);

                        if (dr == DialogResult.Yes)
                        {
                            using (var frm = new frmRenewSubscrption())
                                frm.ShowDialog(this);
                        }
                        return;
                    }

                    // Subscription warnings / expiry
                    DateTime today = DateTime.Today;
                    DateTime expiry = loginResult.ExpiryDate;

                    // Only show expiry prompt if we got a valid expiry date from verification (not MinValue)
                    if (loginResult.VerifyOk && expiry != DateTime.MinValue)
                    {
                        // no_of_days calculation is safe on DateTime itself
                        double no_of_days = (expiry.Date - today).TotalDays;

                        if (no_of_days <= 14)
                        {
                            var prompt = ShowSubscriptionExpiryPrompt(expiry, no_of_days);

                            if (prompt == SubscriptionPromptResult.Renew)
                            {
                                using (var frm = new frmRenewSubscrption())
                                    frm.ShowDialog(this);
                                return;
                            }

                            if (prompt == SubscriptionPromptResult.Cancel)
                                return;
                        }
                    }

                    if (!loginResult.VerifyOk)
                    {
                        var dr = MessageBox.Show(
                            "Your account has expired. Please contact your software provider for renewal. (khybersoft.com) OR click yes to re-new account",
                            "Software Expiration",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Warning);

                        if (dr == DialogResult.Yes)
                        {
                            using (var frm = new frmRenewSubscrption())
                                frm.ShowDialog(this);
                        }
                        return;
                    }

                    if (loginResult.UserId <= 0)
                    {
                        MessageBox.Show("Username or password is incorrect!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                        TxtUsername.Focus();
                        this.ActiveControl = TxtUsername;
                        return;
                    }

                    // Load user details (can be heavy too)
                    var userData = await Task.Run(() =>
                    {
                        var obj_bll = new UsersBLL();
                        var dt = obj_bll.GetUser(loginResult.UserId);

                        int userId = 0;
                        string username = "";
                        int branchId = 0;
                        string language = "";
                        string userRole = "";
                        int userLevel = 0;

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            userId = (int)dr["id"];
                            username = dr["name"].ToString();
                            branchId = (int)dr["branch_id"];
                            language = dr["language"].ToString();
                            userRole = dr["user_role"].ToString();
                            userLevel = (int)dr["user_level"];
                        }

                        var branchesBLL = new BranchesBLL();
                        string branchName = branchId > 0 ? branchesBLL.GetBranchNameByID(branchId) : "";

                        var fiscalyear_obj = new FiscalYearBLL();
                        DataTable fiscalyear_dt = fiscalyear_obj.GetActiveFiscalYear();
                        string fyName = "";
                        DateTime fyFrom = DateTime.MinValue;
                        DateTime fyTo = DateTime.MinValue;
                        if (fiscalyear_dt != null && fiscalyear_dt.Rows.Count > 0)
                        {
                            var fy = fiscalyear_dt.Rows[0];
                            fyName = fy["name"].ToString();
                            fyFrom = Convert.ToDateTime(fy["from_date"]);
                            fyTo = Convert.ToDateTime(fy["to_date"]);
                        }

                        return new
                        {
                            UserId = userId,
                            Username = username,
                            BranchId = branchId,
                            BranchName = branchName,
                            Language = language,
                            UserRole = userRole,
                            UserLevel = userLevel,
                            FiscalYearName = fyName,
                            FiscalYearFrom = fyFrom,
                            FiscalYearTo = fyTo
                        };
                    });

                    // After userData computed and UsersModal fields are set:
                    UsersModal.logged_in_userid = userData.UserId;
                    UsersModal.logged_in_username = userData.Username;
                    UsersModal.logged_in_branch_id = userData.BranchId;
                    UsersModal.logged_in_branch_name = userData.BranchName;
                    UsersModal.logged_in_lang = userData.Language;
                    UsersModal.logged_in_user_role = userData.UserRole;
                    UsersModal.logged_in_user_level = userData.UserLevel;

                    UsersModal.fiscal_year = userData.FiscalYearName;
                    UsersModal.fy_from_date = userData.FiscalYearFrom;
                    UsersModal.fy_to_date = userData.FiscalYearTo;

                    // IMPORTANT: Apply language before creating main form so resources load in correct culture.
                    var lang = string.IsNullOrWhiteSpace(UsersModal.logged_in_lang) ? "en-US" : UsersModal.logged_in_lang;
                    try
                    {
                        var culture = new System.Globalization.CultureInfo(lang);

                        // For ar-SA, keep Arabic language but force Gregorian calendar for dates.
                        if (string.Equals(lang, "ar-SA", StringComparison.OrdinalIgnoreCase))
                        {
                            var dtf = (System.Globalization.DateTimeFormatInfo)culture.DateTimeFormat.Clone();
                            dtf.Calendar = new System.Globalization.GregorianCalendar();
                            // Optional: consistent digits (Arabic-Indic vs Latin). Keep Arabic-Indic by default; comment out if not desired.
                            // dtf.NativeDigits = new[] { "0","1","2","3","4","5","6","7","8","9" };
                            // dtf.DigitSubstitution = System.Globalization.DigitShapes.None;
                            culture.DateTimeFormat = dtf;
                        }

                        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

                        // Ensure any new threads (and WinForms resource lookups) also use this culture.
                        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
                        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
                    }
                    catch
                    {
                        var culture = new System.Globalization.CultureInfo("en-US");
                        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
                        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
                    }

                    // Initialize application security context
                    if (pos.Security.Authorization.AppSecurityContext.RoleRepo == null)
                    {
                        pos.Security.Authorization.AppSecurityContext.RoleRepo = new pos.Security.Authorization.SqlRoleRepository();
                        pos.Security.Authorization.AppSecurityContext.HydrateFromDb();
                    }
                    var parsedRole = pos.Security.Authorization.SystemRole.User;
                    System.Enum.TryParse(UsersModal.logged_in_user_role, true, out parsedRole);
                    pos.Security.Authorization.AppSecurityContext.SetUser(new pos.Security.Authorization.UserIdentity
                    {
                        UserId = UsersModal.logged_in_userid,
                        BranchId = UsersModal.logged_in_branch_id,
                        Username = UsersModal.logged_in_username,
                        Role = parsedRole
                    });

                    this.Hide();
                    var frm_main_obj = new frm_main();
                    frm_main_obj.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static DateTime ClampToSupportedDate(DateTime dt)
        {
            // Known to be safe for Saudi Hijri calendars (UmAlQuraCalendar has a limited supported range)
            // and safe for .NET DateTime operations.
            // If verification returns an uninitialized value, normalize it.
            if (dt == DateTime.MinValue) return new DateTime(1900, 1, 1);
            if (dt < new DateTime(1900, 1, 1)) return new DateTime(1900, 1, 1);
            if (dt > new DateTime(9999, 12, 31)) return new DateTime(9999, 12, 31);
            return dt;
        }

        private static string FormatDateForPrompt(DateTime dt)
        {
            // If current UI culture is Hijri-based (common in ar-SA), formatting a DateTime
            // outside the calendar's range can throw. We choose a stable Gregorian display for prompts.
            var safe = ClampToSupportedDate(dt);
            return safe.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private bool is_company_exist()
        {
            GeneralBLL objBLL = new GeneralBLL();
            String keyword = "*";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            if (companies_dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            frm_register_company registerfrm = new frm_register_company();
            registerfrm.ShowDialog();
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
