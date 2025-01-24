namespace pos
{
    partial class frm_sales_report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crystalReportViewer_sales_report = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer_sales_report
            // 
            this.crystalReportViewer_sales_report.ActiveViewIndex = -1;
            this.crystalReportViewer_sales_report.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer_sales_report.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer_sales_report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer_sales_report.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer_sales_report.Name = "crystalReportViewer_sales_report";
            this.crystalReportViewer_sales_report.Size = new System.Drawing.Size(1312, 706);
            this.crystalReportViewer_sales_report.TabIndex = 0;
            this.crystalReportViewer_sales_report.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // frm_sales_report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 706);
            this.Controls.Add(this.crystalReportViewer_sales_report);
            this.Name = "frm_sales_report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_sales_report_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer_sales_report;
    }
}