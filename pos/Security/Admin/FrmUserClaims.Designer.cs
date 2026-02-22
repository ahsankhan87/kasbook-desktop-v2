using System.Windows.Forms;

namespace pos.Security.Admin
{
    partial class FrmUserClaims
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblUserId;
        private NumericUpDown numUserId;
        private CheckedListBox checkedListBoxClaims;
        private Button btnLoad;
        private Button btnSave;
        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.lblUserId = new Label();
            this.numUserId = new NumericUpDown();
            this.checkedListBoxClaims = new CheckedListBox();
            this.btnLoad = new Button();
            this.btnSave = new Button();
            this.btnClose = new Button();

            this.SuspendLayout();

            this.lblUserId.AutoSize = true;
            this.lblUserId.Text = "User ID:";
            this.lblUserId.Left = 12;
            this.lblUserId.Top = 15;

            this.numUserId.Left = 75;
            this.numUserId.Top = 12;
            this.numUserId.Width = 100;
            this.numUserId.Minimum = 1;
            this.numUserId.Maximum = 1000000;

            this.btnLoad.Text = "Load";
            this.btnLoad.Left = 185;
            this.btnLoad.Top = 10;
            this.btnLoad.Width = 70;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            this.checkedListBoxClaims.Left = 12;
            this.checkedListBoxClaims.Top = 45;
            this.checkedListBoxClaims.Width = 420;
            this.checkedListBoxClaims.Height = 360;
            this.checkedListBoxClaims.CheckOnClick = true;

            this.btnSave.Text = "Save";
            this.btnSave.Left = 246;
            this.btnSave.Top = 415;
            this.btnSave.Width = 90;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnClose.Text = "Close";
            this.btnClose.Left = 342;
            this.btnClose.Top = 415;
            this.btnClose.Width = 90;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.ClientSize = new System.Drawing.Size(450, 455);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.numUserId);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.checkedListBoxClaims);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "User Claims (Overrides)";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}