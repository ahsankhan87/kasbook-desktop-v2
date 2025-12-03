using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace pos.Security.Authorization
{
    public class SecuredForm : Form
    {
        protected IAuthorizationService Auth => AppSecurityContext.Auth;
        protected UserIdentity CurrentUser => AppSecurityContext.User;

        private bool IsDesignHost()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            if (Site?.DesignMode == true) return true;
            // Entry assembly is null in designer
            if (System.Reflection.Assembly.GetEntryAssembly() == null) return true;
            // VS process name check
            var proc = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLowerInvariant();
            if (proc.Contains("devenv") || proc.Contains("wdexpress")) return true;
            return false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Skip in design-time or if no user context (designer surface)
            if (IsDesignHost() || CurrentUser == null)
                return;

            AppSecurityContext.RefreshUserClaims();
            this.ApplyPermissions(Auth, CurrentUser);
        }

        // Keep empty InitializeComponent for inheritance safety (not used directly)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(320, 240);
            this.Name = "SecuredForm";
            this.ResumeLayout(false);
        }
    }
}