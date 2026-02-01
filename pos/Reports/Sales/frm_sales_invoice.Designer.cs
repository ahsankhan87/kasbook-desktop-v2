namespace pos
{
    partial class frm_sales_invoice
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
            this.crystalReportViewer_sales_invoice = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer_sales_invoice
            // 
            this.crystalReportViewer_sales_invoice.ActiveViewIndex = -1;
            this.crystalReportViewer_sales_invoice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer_sales_invoice.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer_sales_invoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer_sales_invoice.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer_sales_invoice.Margin = new System.Windows.Forms.Padding(2);
            this.crystalReportViewer_sales_invoice.Name = "crystalReportViewer_sales_invoice";
            this.crystalReportViewer_sales_invoice.Size = new System.Drawing.Size(1022, 785);
            this.crystalReportViewer_sales_invoice.TabIndex = 0;
            this.crystalReportViewer_sales_invoice.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer_sales_invoice.Load += new System.EventHandler(this.crystalReportViewer_sales_invoice_Load);
            // 
            // frm_sales_invoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 785);
            this.Controls.Add(this.crystalReportViewer_sales_invoice);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frm_sales_invoice";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Invoice";
            this.Load += new System.EventHandler(this.frm_sales_invoice_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_sales_invoice_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer_sales_invoice;
    }
}