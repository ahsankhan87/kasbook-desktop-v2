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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_zatca_invoices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridZatcaInvoices = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zatca_mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_subtype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zatca_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zatca_message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prevSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prevInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnComplianceChecks = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnViewResponse = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btn_viewQR = new System.Windows.Forms.Button();
            this.btn_signInvoice = new System.Windows.Forms.Button();
            this.btn_invoice_report = new System.Windows.Forms.Button();
            this.btn_Invoice_clearance = new System.Windows.Forms.Button();
            this.cmbSubtype = new System.Windows.Forms.ComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnDownloadUBL = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chk_ShowZatcaInvoice = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmb_status = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.NewComplianceCheckButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridZatcaInvoices
            // 
            this.gridZatcaInvoices.AllowUserToAddRows = false;
            this.gridZatcaInvoices.AllowUserToDeleteRows = false;
            this.gridZatcaInvoices.AllowUserToOrderColumns = true;
            this.gridZatcaInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridZatcaInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.customer,
            this.zatca_mode,
            this.account,
            this.invoice_subtype,
            this.zatca_status,
            this.sale_date,
            this.total_tax,
            this.discount_value,
            this.total,
            this.zatca_message,
            this.prevSaleDate,
            this.prevInvoiceNo,
            this.sale_time});
            resources.ApplyResources(this.gridZatcaInvoices, "gridZatcaInvoices");
            this.gridZatcaInvoices.MultiSelect = false;
            this.gridZatcaInvoices.Name = "gridZatcaInvoices";
            this.gridZatcaInvoices.ReadOnly = true;
            this.gridZatcaInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.invoice_no.DataPropertyName = "invoice_no";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invoice_no.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // customer
            // 
            this.customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.customer.DataPropertyName = "customer";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customer.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.customer, "customer");
            this.customer.Name = "customer";
            this.customer.ReadOnly = true;
            // 
            // zatca_mode
            // 
            this.zatca_mode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.zatca_mode.DataPropertyName = "zatca_mode";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zatca_mode.DefaultCellStyle = dataGridViewCellStyle3;
            this.zatca_mode.FillWeight = 30F;
            resources.ApplyResources(this.zatca_mode, "zatca_mode");
            this.zatca_mode.Name = "zatca_mode";
            this.zatca_mode.ReadOnly = true;
            // 
            // account
            // 
            this.account.DataPropertyName = "account";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.account.DefaultCellStyle = dataGridViewCellStyle4;
            this.account.FillWeight = 30F;
            resources.ApplyResources(this.account, "account");
            this.account.Name = "account";
            this.account.ReadOnly = true;
            // 
            // invoice_subtype
            // 
            this.invoice_subtype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.invoice_subtype.DataPropertyName = "invoice_subtype";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invoice_subtype.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.invoice_subtype, "invoice_subtype");
            this.invoice_subtype.Name = "invoice_subtype";
            this.invoice_subtype.ReadOnly = true;
            // 
            // zatca_status
            // 
            this.zatca_status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.zatca_status.DataPropertyName = "zatca_status";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zatca_status.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.zatca_status, "zatca_status");
            this.zatca_status.Name = "zatca_status";
            this.zatca_status.ReadOnly = true;
            // 
            // sale_date
            // 
            this.sale_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sale_date.DataPropertyName = "sale_date";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sale_date.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.sale_date, "sale_date");
            this.sale_date.Name = "sale_date";
            this.sale_date.ReadOnly = true;
            // 
            // total_tax
            // 
            this.total_tax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.total_tax.DataPropertyName = "total_tax";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = null;
            this.total_tax.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.total_tax, "total_tax");
            this.total_tax.Name = "total_tax";
            this.total_tax.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = null;
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // total
            // 
            this.total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = null;
            this.total.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // zatca_message
            // 
            this.zatca_message.DataPropertyName = "zatca_message";
            resources.ApplyResources(this.zatca_message, "zatca_message");
            this.zatca_message.Name = "zatca_message";
            this.zatca_message.ReadOnly = true;
            // 
            // prevSaleDate
            // 
            this.prevSaleDate.DataPropertyName = "prevSaleDate";
            resources.ApplyResources(this.prevSaleDate, "prevSaleDate");
            this.prevSaleDate.Name = "prevSaleDate";
            this.prevSaleDate.ReadOnly = true;
            // 
            // prevInvoiceNo
            // 
            this.prevInvoiceNo.DataPropertyName = "prevInvoiceNo";
            resources.ApplyResources(this.prevInvoiceNo, "prevInvoiceNo");
            this.prevInvoiceNo.Name = "prevInvoiceNo";
            this.prevInvoiceNo.ReadOnly = true;
            // 
            // sale_time
            // 
            this.sale_time.DataPropertyName = "sale_time";
            resources.ApplyResources(this.sale_time, "sale_time");
            this.sale_time.Name = "sale_time";
            this.sale_time.ReadOnly = true;
            // 
            // btnComplianceChecks
            // 
            resources.ApplyResources(this.btnComplianceChecks, "btnComplianceChecks");
            this.btnComplianceChecks.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnComplianceChecks.Name = "btnComplianceChecks";
            this.btnComplianceChecks.UseVisualStyleBackColor = false;
            this.btnComplianceChecks.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnViewResponse
            // 
            resources.ApplyResources(this.btnViewResponse, "btnViewResponse");
            this.btnViewResponse.Name = "btnViewResponse";
            this.btnViewResponse.UseVisualStyleBackColor = true;
            this.btnViewResponse.Click += new System.EventHandler(this.btnViewResponse_Click);
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // btn_viewQR
            // 
            resources.ApplyResources(this.btn_viewQR, "btn_viewQR");
            this.btn_viewQR.Name = "btn_viewQR";
            this.btn_viewQR.UseVisualStyleBackColor = true;
            this.btn_viewQR.Click += new System.EventHandler(this.btn_viewQR_Click);
            // 
            // btn_signInvoice
            // 
            resources.ApplyResources(this.btn_signInvoice, "btn_signInvoice");
            this.btn_signInvoice.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_signInvoice.Name = "btn_signInvoice";
            this.btn_signInvoice.UseVisualStyleBackColor = false;
            this.btn_signInvoice.Click += new System.EventHandler(this.btn_signInvoice_Click);
            // 
            // btn_invoice_report
            // 
            resources.ApplyResources(this.btn_invoice_report, "btn_invoice_report");
            this.btn_invoice_report.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_invoice_report.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_invoice_report.Name = "btn_invoice_report";
            this.btn_invoice_report.UseVisualStyleBackColor = false;
            this.btn_invoice_report.Click += new System.EventHandler(this.btn_invoice_report_Click);
            // 
            // btn_Invoice_clearance
            // 
            resources.ApplyResources(this.btn_Invoice_clearance, "btn_Invoice_clearance");
            this.btn_Invoice_clearance.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Invoice_clearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_Invoice_clearance.Name = "btn_Invoice_clearance";
            this.btn_Invoice_clearance.UseVisualStyleBackColor = false;
            this.btn_Invoice_clearance.Click += new System.EventHandler(this.btn_Invoice_clearance_Click);
            // 
            // cmbSubtype
            // 
            this.cmbSubtype.FormattingEnabled = true;
            resources.ApplyResources(this.cmbSubtype, "cmbSubtype");
            this.cmbSubtype.Name = "cmbSubtype";
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            // 
            // txtInvoiceNo
            // 
            resources.ApplyResources(this.txtInvoiceNo, "txtInvoiceNo");
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            // 
            // btnDownloadUBL
            // 
            resources.ApplyResources(this.btnDownloadUBL, "btnDownloadUBL");
            this.btnDownloadUBL.Name = "btnDownloadUBL";
            this.btnDownloadUBL.UseVisualStyleBackColor = true;
            this.btnDownloadUBL.Click += new System.EventHandler(this.btnDownloadUBL_Click);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.chk_ShowZatcaInvoice);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmb_status);
            this.panel1.Controls.Add(this.cmbSubtype);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbType);
            this.panel1.Controls.Add(this.txtInvoiceNo);
            this.panel1.Controls.Add(this.dtpToDate);
            this.panel1.Controls.Add(this.dtpFromDate);
            this.panel1.Name = "panel1";
            // 
            // chk_ShowZatcaInvoice
            // 
            resources.ApplyResources(this.chk_ShowZatcaInvoice, "chk_ShowZatcaInvoice");
            this.chk_ShowZatcaInvoice.Name = "chk_ShowZatcaInvoice";
            this.chk_ShowZatcaInvoice.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::pos.Properties.Resources._3969301_841013250;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // cmb_status
            // 
            this.cmb_status.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_status, "cmb_status");
            this.cmb_status.Name = "cmb_status";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridZatcaInvoices);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnViewResponse);
            this.panel3.Controls.Add(this.btn_invoice_report);
            this.panel3.Controls.Add(this.btn_Invoice_clearance);
            this.panel3.Controls.Add(this.btnDownloadUBL);
            this.panel3.Controls.Add(this.btn_signInvoice);
            this.panel3.Controls.Add(this.NewComplianceCheckButton);
            this.panel3.Controls.Add(this.btnComplianceChecks);
            this.panel3.Controls.Add(this.btn_viewQR);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // NewComplianceCheckButton
            // 
            resources.ApplyResources(this.NewComplianceCheckButton, "NewComplianceCheckButton");
            this.NewComplianceCheckButton.BackColor = System.Drawing.Color.LightSkyBlue;
            this.NewComplianceCheckButton.Name = "NewComplianceCheckButton";
            this.NewComplianceCheckButton.UseVisualStyleBackColor = false;
            this.NewComplianceCheckButton.Click += new System.EventHandler(this.NewComplianceCheckButton_Click);
            // 
            // frm_zatca_invoices
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "frm_zatca_invoices";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_zatca_invoices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmbSubtype;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnDownloadUBL;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb_status;
        private System.Windows.Forms.Button NewComplianceCheckButton;
        private System.Windows.Forms.CheckBox chk_ShowZatcaInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn zatca_mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_subtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn zatca_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn zatca_message;
        private System.Windows.Forms.DataGridViewTextBoxColumn prevSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn prevInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_time;
    }
}