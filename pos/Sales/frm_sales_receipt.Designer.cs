namespace pos
{
    partial class frm_sales_receipt
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
            this.components = new System.ComponentModel.Container();
            //Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            //this.SalesModalBindingSource = new System.Windows.Forms.BindingSource(this.components);
            //this.reportViewer_sales = new Microsoft.Reporting.WinForms.ReportViewer();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            ((System.ComponentModel.ISupportInitialize)(this.SalesModalBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SalesModalBindingSource
            // 
            this.SalesModalBindingSource.DataSource = typeof(POS.Core.SalesModal);
            // 
            // reportViewer_sales
            // 
            //this.reportViewer_sales.Dock = System.Windows.Forms.DockStyle.Fill;
            //reportDataSource1.Name = "ds_sales_receipt";
            //reportDataSource1.Value = this.SalesModalBindingSource;
            //this.reportViewer_sales.LocalReport.DataSources.Add(reportDataSource1);
            //this.reportViewer_sales.LocalReport.ReportEmbeddedResource = "pos.Sales.Report_sales_receipt.rdlc";
            //this.reportViewer_sales.Location = new System.Drawing.Point(0, 0);
            //this.reportViewer_sales.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            //this.reportViewer_sales.Name = "reportViewer_sales";
            //this.reportViewer_sales.Size = new System.Drawing.Size(507, 674);
            //this.reportViewer_sales.TabIndex = 0;
            // 
            // frm_sales_receipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 674);
            //this.Controls.Add(this.reportViewer_sales);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frm_sales_receipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Receipt";
            this.Load += new System.EventHandler(this.frm_sales_receipt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SalesModalBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private Microsoft.Reporting.WinForms.ReportViewer reportViewer_sales;
        private System.Windows.Forms.BindingSource SalesModalBindingSource;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
    }
}