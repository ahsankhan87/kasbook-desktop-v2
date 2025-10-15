using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Dashboard
{
    partial class frm_dashboard
    {
        private IContainer components = null;
        private Panel welcomePanel;
        private Label lblWelcome;
        private Label lblInfo;
        private FlowLayoutPanel quickAccessPanel;
        private Label lblQuickAccess;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.welcomePanel = new Panel();
            this.lblWelcome = new Label();
            this.lblInfo = new Label();
            this.quickAccessPanel = new FlowLayoutPanel();
            this.lblQuickAccess = new Label();
            this.SuspendLayout();
            // 
            // welcomePanel
            // 
            this.welcomePanel.BackColor = Color.FromArgb(41, 128, 185);
            this.welcomePanel.Dock = DockStyle.Top;
            this.welcomePanel.Height = 120;
            this.welcomePanel.Padding = new Padding(30, 20, 30, 20);
            this.welcomePanel.Name = "welcomePanel";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.ForeColor = Color.White;
            this.lblWelcome.Location = new Point(30, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Text = "Welcome";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = Color.FromArgb(220, 230, 240);
            this.lblInfo.Location = new Point(30, 60);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Text = "Info";
            // 
            // quickAccessPanel
            // 
            this.quickAccessPanel.AutoScroll = true;
            this.quickAccessPanel.BackColor = Color.FromArgb(245, 247, 250);
            this.quickAccessPanel.Dock = DockStyle.Fill;
            this.quickAccessPanel.Padding = new Padding(20, 60, 20, 20);
            this.quickAccessPanel.Name = "quickAccessPanel";
            // 
            // lblQuickAccess
            // 
            this.lblQuickAccess.AutoSize = true;
            this.lblQuickAccess.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblQuickAccess.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblQuickAccess.Location = new Point(20, 20);
            this.lblQuickAccess.Padding = new Padding(10);
            this.lblQuickAccess.Name = "lblQuickAccess";
            this.lblQuickAccess.Text = "Quick Access";
            // 
            // frm_dashboard
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(900, 650);
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_dashboard";
            this.Text = "Dashboard";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.frm_dashboard_Load);
            // add controls to containers
            this.welcomePanel.Controls.Add(this.lblWelcome);
            this.welcomePanel.Controls.Add(this.lblInfo);
            this.quickAccessPanel.Controls.Add(this.lblQuickAccess);
            this.Controls.Add(this.quickAccessPanel);
            this.Controls.Add(this.welcomePanel);
            this.ResumeLayout(false);
        }
    }
}
