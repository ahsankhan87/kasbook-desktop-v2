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
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnViewResponse = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btn_viewQR = new System.Windows.Forms.Button();
            this.btn_signInvoice = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).BeginInit();
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
            this.gridZatcaInvoices.Location = new System.Drawing.Point(12, 60);
            this.gridZatcaInvoices.MultiSelect = false;
            this.gridZatcaInvoices.Name = "gridZatcaInvoices";
            this.gridZatcaInvoices.ReadOnly = true;
            this.gridZatcaInvoices.RowHeadersWidth = 51;
            this.gridZatcaInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridZatcaInvoices.Size = new System.Drawing.Size(1019, 338);
            this.gridZatcaInvoices.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.Location = new System.Drawing.Point(280, 404);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(120, 37);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send to ZATCA";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReport.Location = new System.Drawing.Point(410, 404);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(120, 37);
            this.btnReport.TabIndex = 4;
            this.btnReport.Text = "Report to ZATCA";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(540, 404);
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
            this.btnViewResponse.Location = new System.Drawing.Point(630, 404);
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
            this.lblTitle.Location = new System.Drawing.Point(389, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(337, 32);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "ZATCA Invoice Management";
            // 
            // btn_viewQR
            // 
            this.btn_viewQR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_viewQR.Location = new System.Drawing.Point(797, 404);
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
            this.btn_signInvoice.Location = new System.Drawing.Point(154, 404);
            this.btn_signInvoice.Name = "btn_signInvoice";
            this.btn_signInvoice.Size = new System.Drawing.Size(120, 37);
            this.btn_signInvoice.TabIndex = 3;
            this.btn_signInvoice.Text = "Sign Invoice";
            this.btn_signInvoice.UseVisualStyleBackColor = true;
            this.btn_signInvoice.Click += new System.EventHandler(this.btn_signInvoice_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
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
            // frm_zatca_invoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 459);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btn_viewQR);
            this.Controls.Add(this.btnViewResponse);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btn_signInvoice);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.gridZatcaInvoices);
            this.Name = "frm_zatca_invoices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZATCA Invoice Management";
            this.Load += new System.EventHandler(this.frm_zatca_invoices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridZatcaInvoices;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnViewResponse;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btn_viewQR;
        private System.Windows.Forms.Button btn_signInvoice;
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
    }
}