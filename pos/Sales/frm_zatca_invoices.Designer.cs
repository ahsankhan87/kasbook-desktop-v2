namespace pos.Sales
{
    partial class frm_zatca_invoices
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
            this.gridZatcaInvoices = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zatca_mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_subtype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zatca_message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prevSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prevInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnComplianceChecks = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnViewResponse = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btn_viewQR = new System.Windows.Forms.Button();
            this.btn_signInvoice = new System.Windows.Forms.Button();
            this.btn_invoice_report = new System.Windows.Forms.Button();
            this.btn_Invoice_clearance = new System.Windows.Forms.Button();
            this.btn_PCSID_sign = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridZatcaInvoices
            // 
            this.gridZatcaInvoices.AllowUserToAddRows = false;
            this.gridZatcaInvoices.AllowUserToDeleteRows = false;
            this.gridZatcaInvoices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridZatcaInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridZatcaInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.customer,
            this.zatca_mode,
            this.account,
            this.invoice_subtype,
            this.status,
            this.sale_date,
            this.total_tax,
            this.toal,
            this.zatca_message,
            this.prevSaleDate,
            this.prevInvoiceNo});
            this.gridZatcaInvoices.Location = new System.Drawing.Point(12, 103);
            this.gridZatcaInvoices.MultiSelect = false;
            this.gridZatcaInvoices.Name = "gridZatcaInvoices";
            this.gridZatcaInvoices.ReadOnly = true;
            this.gridZatcaInvoices.RowHeadersWidth = 51;
            this.gridZatcaInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridZatcaInvoices.Size = new System.Drawing.Size(1019, 295);
            this.gridZatcaInvoices.TabIndex = 0;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 125;
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            this.invoice_no.HeaderText = "Invoice No";
            this.invoice_no.MinimumWidth = 6;
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            this.invoice_no.Width = 125;
            // 
            // customer
            // 
            this.customer.DataPropertyName = "customer";
            this.customer.HeaderText = "Customer";
            this.customer.MinimumWidth = 6;
            this.customer.Name = "customer";
            this.customer.ReadOnly = true;
            this.customer.Width = 125;
            // 
            // zatca_mode
            // 
            this.zatca_mode.DataPropertyName = "zatca_mode";
            this.zatca_mode.HeaderText = "Mode";
            this.zatca_mode.MinimumWidth = 6;
            this.zatca_mode.Name = "zatca_mode";
            this.zatca_mode.ReadOnly = true;
            this.zatca_mode.Width = 125;
            // 
            // account
            // 
            this.account.DataPropertyName = "account";
            this.account.HeaderText = "Type";
            this.account.MinimumWidth = 6;
            this.account.Name = "account";
            this.account.ReadOnly = true;
            this.account.Width = 125;
            // 
            // invoice_subtype
            // 
            this.invoice_subtype.DataPropertyName = "invoice_subtype";
            this.invoice_subtype.HeaderText = "Sub type";
            this.invoice_subtype.MinimumWidth = 6;
            this.invoice_subtype.Name = "invoice_subtype";
            this.invoice_subtype.ReadOnly = true;
            this.invoice_subtype.Width = 125;
            // 
            // status
            // 
            this.status.DataPropertyName = "zatca_status";
            this.status.HeaderText = "Status";
            this.status.MinimumWidth = 6;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 125;
            // 
            // sale_date
            // 
            this.sale_date.DataPropertyName = "sale_date";
            this.sale_date.HeaderText = "Date";
            this.sale_date.MinimumWidth = 6;
            this.sale_date.Name = "sale_date";
            this.sale_date.ReadOnly = true;
            this.sale_date.Width = 125;
            // 
            // total_tax
            // 
            this.total_tax.DataPropertyName = "total_tax";
            this.total_tax.HeaderText = "Total Tax";
            this.total_tax.MinimumWidth = 6;
            this.total_tax.Name = "total_tax";
            this.total_tax.ReadOnly = true;
            this.total_tax.Width = 125;
            // 
            // toal
            // 
            this.toal.DataPropertyName = "total";
            this.toal.HeaderText = "Net Total";
            this.toal.MinimumWidth = 6;
            this.toal.Name = "toal";
            this.toal.ReadOnly = true;
            this.toal.Width = 125;
            // 
            // zatca_message
            // 
            this.zatca_message.DataPropertyName = "zatca_message";
            this.zatca_message.HeaderText = "zatca message";
            this.zatca_message.MinimumWidth = 6;
            this.zatca_message.Name = "zatca_message";
            this.zatca_message.ReadOnly = true;
            this.zatca_message.Visible = false;
            this.zatca_message.Width = 125;
            // 
            // prevSaleDate
            // 
            this.prevSaleDate.DataPropertyName = "prevSaleDate";
            this.prevSaleDate.HeaderText = "prevSaleDate";
            this.prevSaleDate.MinimumWidth = 6;
            this.prevSaleDate.Name = "prevSaleDate";
            this.prevSaleDate.ReadOnly = true;
            this.prevSaleDate.Visible = false;
            this.prevSaleDate.Width = 125;
            // 
            // prevInvoiceNo
            // 
            this.prevInvoiceNo.DataPropertyName = "prevInvoiceNo";
            this.prevInvoiceNo.HeaderText = "prevInvoiceNo";
            this.prevInvoiceNo.MinimumWidth = 6;
            this.prevInvoiceNo.Name = "prevInvoiceNo";
            this.prevInvoiceNo.ReadOnly = true;
            this.prevInvoiceNo.Visible = false;
            this.prevInvoiceNo.Width = 125;
            // 
            // btnComplianceChecks
            // 
            this.btnComplianceChecks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnComplianceChecks.Location = new System.Drawing.Point(149, 404);
            this.btnComplianceChecks.Name = "btnComplianceChecks";
            this.btnComplianceChecks.Size = new System.Drawing.Size(142, 37);
            this.btnComplianceChecks.TabIndex = 4;
            this.btnComplianceChecks.Text = "Compliance Checks";
            this.btnComplianceChecks.UseVisualStyleBackColor = true;
            this.btnComplianceChecks.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(699, 410);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 37);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnViewResponse
            // 
            this.btnViewResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewResponse.Location = new System.Drawing.Point(789, 410);
            this.btnViewResponse.Name = "btnViewResponse";
            this.btnViewResponse.Size = new System.Drawing.Size(161, 37);
            this.btnViewResponse.TabIndex = 6;
            this.btnViewResponse.Text = "View ZATCA Response";
            this.btnViewResponse.UseVisualStyleBackColor = true;
            this.btnViewResponse.Click += new System.EventHandler(this.btnViewResponse_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(269, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(337, 32);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "ZATCA Invoice Management";
            // 
            // btn_viewQR
            // 
            this.btn_viewQR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_viewQR.Location = new System.Drawing.Point(956, 410);
            this.btn_viewQR.Name = "btn_viewQR";
            this.btn_viewQR.Size = new System.Drawing.Size(72, 37);
            this.btn_viewQR.TabIndex = 6;
            this.btn_viewQR.Text = "View QR";
            this.btn_viewQR.UseVisualStyleBackColor = true;
            this.btn_viewQR.Click += new System.EventHandler(this.btn_viewQR_Click);
            // 
            // btn_signInvoice
            // 
            this.btn_signInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_signInvoice.Location = new System.Drawing.Point(12, 404);
            this.btn_signInvoice.Name = "btn_signInvoice";
            this.btn_signInvoice.Size = new System.Drawing.Size(131, 37);
            this.btn_signInvoice.TabIndex = 3;
            this.btn_signInvoice.Text = "CSID Sign Invoice";
            this.btn_signInvoice.UseVisualStyleBackColor = true;
            this.btn_signInvoice.Click += new System.EventHandler(this.btn_signInvoice_Click);
            // 
            // btn_invoice_report
            // 
            this.btn_invoice_report.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_invoice_report.Location = new System.Drawing.Point(908, 12);
            this.btn_invoice_report.Name = "btn_invoice_report";
            this.btn_invoice_report.Size = new System.Drawing.Size(120, 37);
            this.btn_invoice_report.TabIndex = 3;
            this.btn_invoice_report.Text = "Reporting";
            this.btn_invoice_report.UseVisualStyleBackColor = true;
            this.btn_invoice_report.Click += new System.EventHandler(this.btn_invoice_report_Click);
            // 
            // btn_Invoice_clearance
            // 
            this.btn_Invoice_clearance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Invoice_clearance.Location = new System.Drawing.Point(782, 10);
            this.btn_Invoice_clearance.Name = "btn_Invoice_clearance";
            this.btn_Invoice_clearance.Size = new System.Drawing.Size(120, 37);
            this.btn_Invoice_clearance.TabIndex = 3;
            this.btn_Invoice_clearance.Text = "Clearance";
            this.btn_Invoice_clearance.UseVisualStyleBackColor = true;
            this.btn_Invoice_clearance.Click += new System.EventHandler(this.btn_Invoice_clearance_Click);
            // 
            // btn_PCSID_sign
            // 
            this.btn_PCSID_sign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_PCSID_sign.Location = new System.Drawing.Point(656, 10);
            this.btn_PCSID_sign.Name = "btn_PCSID_sign";
            this.btn_PCSID_sign.Size = new System.Drawing.Size(120, 37);
            this.btn_PCSID_sign.TabIndex = 3;
            this.btn_PCSID_sign.Text = "PCSID Sign";
            this.btn_PCSID_sign.UseVisualStyleBackColor = true;
            this.btn_PCSID_sign.Click += new System.EventHandler(this.btn_PCSID_sign_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::pos.Properties.Resources._3969301_841013250;
            this.pictureBox1.Location = new System.Drawing.Point(12, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 87);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // frm_zatca_invoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 459);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btn_viewQR);
            this.Controls.Add(this.btnViewResponse);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnComplianceChecks);
            this.Controls.Add(this.btn_signInvoice);
            this.Controls.Add(this.btn_PCSID_sign);
            this.Controls.Add(this.btn_Invoice_clearance);
            this.Controls.Add(this.btn_invoice_report);
            this.Controls.Add(this.gridZatcaInvoices);
            this.Name = "frm_zatca_invoices";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZATCA Invoice Management";
            this.Load += new System.EventHandler(this.frm_zatca_invoices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridZatcaInvoices;
        private System.Windows.Forms.Button btnComplianceChecks;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnViewResponse;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btn_viewQR;
        private System.Windows.Forms.Button btn_signInvoice;
        private System.Windows.Forms.Button btn_invoice_report;
        private System.Windows.Forms.Button btn_Invoice_clearance;
        private System.Windows.Forms.Button btn_PCSID_sign;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn zatca_mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_subtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn toal;
        private System.Windows.Forms.DataGridViewTextBoxColumn zatca_message;
        private System.Windows.Forms.DataGridViewTextBoxColumn prevSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn prevInvoiceNo;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}