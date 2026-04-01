using POS.BLL;
using POS.Core;
using pos.UI;
using System;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_session_lock : Form
    {
        private readonly string _loginUsername;
        private readonly int _userId;
        private bool _allowClose;

        public frm_session_lock(string displayName, string loginUsername, int userId)
        {
            InitializeComponent();

            lblUserValue.Text = displayName;
            _loginUsername = loginUsername;
            _userId = userId;

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ShowInTaskbar = false;
            this.TopMost = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void frm_session_lock_Load(object sender, EventArgs e)
        {
            try { AppTheme.Apply(this); } catch { }

            txtPassword.Focus();
            txtPassword.SelectAll();
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!ValidatePassword(txtPassword.Text))
            {
                UiMessages.ShowWarning(
                    "Invalid password.",
                    "كلمة المرور غير صحيحة.",
                    captionEn: "Unlock",
                    captionAr: "فتح");

                txtPassword.SelectAll();
                txtPassword.Focus();
                return;
            }

            _allowClose = true;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            _allowClose = true;
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(_loginUsername))
                return false;

            var userModal = new UsersModal
            {
                username = _loginUsername,
                password = password.Trim()
            };

            int matchedUserId = new UsersBLL().Login(userModal);
            return matchedUserId == _userId;
        }

        private void frm_session_lock_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_allowClose)
                e.Cancel = true;
        }
    }
}
