using System.Windows.Forms;

namespace pos.Security.Admin
{
    partial class FrmRolePermissions
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbRoles;
        private CheckedListBox checkedListBoxPermissions;
        private Button btnSave;
        private Button btnClose;
        private Label lblRole;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.cmbRoles = new ComboBox();
            this.checkedListBoxPermissions = new CheckedListBox();
            this.btnSave = new Button();
            this.btnClose = new Button();
            this.lblRole = new Label();

            this.SuspendLayout();

            this.lblRole.AutoSize = true;
            this.lblRole.Text = "Role:";
            this.lblRole.Left = 12;
            this.lblRole.Top = 15;

            this.cmbRoles.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRoles.Left = 60;
            this.cmbRoles.Top = 10;
            this.cmbRoles.Width = 220;
            this.cmbRoles.SelectedIndexChanged += (s, e) => LoadPermissionsForRole();

            this.checkedListBoxPermissions.Left = 12;
            this.checkedListBoxPermissions.Top = 45;
            this.checkedListBoxPermissions.Width = 420;
            this.checkedListBoxPermissions.Height = 360;
            this.checkedListBoxPermissions.CheckOnClick = true;

            this.btnSave.Text = "Save";
            this.btnSave.Left = 246;
            this.btnSave.Top = 415;
            this.btnSave.Width = 90;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnClose.Text = "Close";
            this.btnClose.Left = 342;
            this.btnClose.Top = 415;
            this.btnClose.Width = 90;
            this.btnClose.Click += (s, e) => this.Close();

            this.ClientSize = new System.Drawing.Size(450, 455);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbRoles);
            this.Controls.Add(this.checkedListBoxPermissions);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Role Permissions";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}