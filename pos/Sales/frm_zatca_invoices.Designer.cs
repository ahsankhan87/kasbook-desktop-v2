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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridZatcaInvoices = new System.Windows.Forms.DataGridView();
            this.btnComplianceChecks = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnViewResponse = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btn_viewQR = new System.Windows.Forms.Button();
            this.btn_signInvoice = new System.Windows.Forms.Button();
            this.btn_invoice_report = new System.Windows.Forms.Button();
            this.btn_Invoice_clearance = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_status = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.gridZatcaInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridZatcaInvoices.Location = new System.Drawing.Point(0, 0);
            this.gridZatcaInvoices.MultiSelect = false;
            this.gridZatcaInvoices.Name = "gridZatcaInvoices";
            this.gridZatcaInvoices.ReadOnly = true;
            this.gridZatcaInvoices.RowHeadersWidth = 51;
            this.gridZatcaInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridZatcaInvoices.Size = new System.Drawing.Size(1142, 503);
            this.gridZatcaInvoices.TabIndex = 0;
            // 
            // btnComplianceChecks
            // 
            this.btnComplianceChecks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComplianceChecks.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnComplianceChecks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplianceChecks.Location = new System.Drawing.Point(754, 11);
            this.btnComplianceChecks.Name = "btnComplianceChecks";
            this.btnComplianceChecks.Size = new System.Drawing.Size(126, 54);
            this.btnComplianceChecks.TabIndex = 5;
            this.btnComplianceChecks.Text = "Compliance Checks";
            this.btnComplianceChecks.UseVisualStyleBackColor = false;
            this.btnComplianceChecks.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(972, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 37);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnViewResponse
            // 
            this.btnViewResponse.Location = new System.Drawing.Point(118, 11);
            this.btnViewResponse.Name = "btnViewResponse";
            this.btnViewResponse.Size = new System.Drawing.Size(107, 48);
            this.btnViewResponse.TabIndex = 2;
            this.btnViewResponse.Text = "View ZATCA Response";
            this.btnViewResponse.UseVisualStyleBackColor = true;
            this.btnViewResponse.Click += new System.EventHandler(this.btnViewResponse_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(430, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 32);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "ZATCA E-Invoice Management";
            // 
            // btn_viewQR
            // 
            this.btn_viewQR.Location = new System.Drawing.Point(227, 11);
            this.btn_viewQR.Name = "btn_viewQR";
            this.btn_viewQR.Size = new System.Drawing.Size(107, 48);
            this.btn_viewQR.TabIndex = 3;
            this.btn_viewQR.Text = "View QR";
            this.btn_viewQR.UseVisualStyleBackColor = true;
            this.btn_viewQR.Click += new System.EventHandler(this.btn_viewQR_Click);
            // 
            // btn_signInvoice
            // 
            this.btn_signInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_signInvoice.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_signInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_signInvoice.Location = new System.Drawing.Point(625, 11);
            this.btn_signInvoice.Name = "btn_signInvoice";
            this.btn_signInvoice.Size = new System.Drawing.Size(123, 54);
            this.btn_signInvoice.TabIndex = 4;
            this.btn_signInvoice.Text = "Sign Invoice";
            this.btn_signInvoice.UseVisualStyleBackColor = false;
            this.btn_signInvoice.Click += new System.EventHandler(this.btn_signInvoice_Click);
            // 
            // btn_invoice_report
            // 
            this.btn_invoice_report.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_invoice_report.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_invoice_report.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_invoice_report.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_invoice_report.Location = new System.Drawing.Point(1012, 11);
            this.btn_invoice_report.Name = "btn_invoice_report";
            this.btn_invoice_report.Size = new System.Drawing.Size(118, 54);
            this.btn_invoice_report.TabIndex = 7;
            this.btn_invoice_report.Text = "Reporting";
            this.btn_invoice_report.UseVisualStyleBackColor = false;
            this.btn_invoice_report.Click += new System.EventHandler(this.btn_invoice_report_Click);
            // 
            // btn_Invoice_clearance
            // 
            this.btn_Invoice_clearance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Invoice_clearance.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Invoice_clearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Invoice_clearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_Invoice_clearance.Location = new System.Drawing.Point(886, 11);
            this.btn_Invoice_clearance.Name = "btn_Invoice_clearance";
            this.btn_Invoice_clearance.Size = new System.Drawing.Size(120, 54);
            this.btn_Invoice_clearance.TabIndex = 6;
            this.btn_Invoice_clearance.Text = "Clearance";
            this.btn_Invoice_clearance.UseVisualStyleBackColor = false;
            this.btn_Invoice_clearance.Click += new System.EventHandler(this.btn_Invoice_clearance_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::pos.Properties.Resources._3969301_841013250;
            this.pictureBox1.Location = new System.Drawing.Point(3, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 87);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // cmbSubtype
            // 
            this.cmbSubtype.FormattingEnabled = true;
            this.cmbSubtype.Location = new System.Drawing.Point(759, 78);
            this.cmbSubtype.Name = "cmbSubtype";
            this.cmbSubtype.Size = new System.Drawing.Size(121, 24);
            this.cmbSubtype.TabIndex = 4;
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(632, 78);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 24);
            this.cmbType.TabIndex = 3;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.Location = new System.Drawing.Point(486, 78);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(140, 22);
            this.txtInvoiceNo.TabIndex = 2;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(277, 78);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(100, 22);
            this.dtpFromDate.TabIndex = 0;
            // 
            // btnDownloadUBL
            // 
            this.btnDownloadUBL.Location = new System.Drawing.Point(5, 11);
            this.btnDownloadUBL.Name = "btnDownloadUBL";
            this.btnDownloadUBL.Size = new System.Drawing.Size(107, 48);
            this.btnDownloadUBL.TabIndex = 1;
            this.btnDownloadUBL.Text = "Download UBL";
            this.btnDownloadUBL.UseVisualStyleBackColor = true;
            this.btnDownloadUBL.Click += new System.EventHandler(this.btnDownloadUBL_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1012, 76);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "From Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(483, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Invoice No.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(629, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(756, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Subtype";
            // 
            // panel1
            // 
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1142, 108);
            this.panel1.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(883, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "Status";
            // 
            // cmb_status
            // 
            this.cmb_status.FormattingEnabled = true;
            this.cmb_status.Location = new System.Drawing.Point(885, 78);
            this.cmb_status.Name = "cmb_status";
            this.cmb_status.Size = new System.Drawing.Size(121, 24);
            this.cmb_status.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(1058, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 37);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(380, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "To Date";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(380, 78);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(100, 22);
            this.dtpToDate.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridZatcaInvoices);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 108);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1142, 503);
            this.panel2.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnViewResponse);
            this.panel3.Controls.Add(this.btn_invoice_report);
            this.panel3.Controls.Add(this.btn_Invoice_clearance);
            this.panel3.Controls.Add(this.btnDownloadUBL);
            this.panel3.Controls.Add(this.btn_signInvoice);
            this.panel3.Controls.Add(this.btnComplianceChecks);
            this.panel3.Controls.Add(this.btn_viewQR);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 611);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1142, 77);
            this.panel3.TabIndex = 17;
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
            this.customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.customer.DataPropertyName = "customer";
            this.customer.HeaderText = "Customer";
            this.customer.MinimumWidth = 6;
            this.customer.Name = "customer";
            this.customer.ReadOnly = true;
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
            // zatca_status
            // 
            this.zatca_status.DataPropertyName = "zatca_status";
            this.zatca_status.HeaderText = "Status";
            this.zatca_status.MinimumWidth = 6;
            this.zatca_status.Name = "zatca_status";
            this.zatca_status.ReadOnly = true;
            this.zatca_status.Width = 125;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.total_tax.DefaultCellStyle = dataGridViewCellStyle1;
            this.total_tax.HeaderText = "Total VAT";
            this.total_tax.MinimumWidth = 6;
            this.total_tax.Name = "total_tax";
            this.total_tax.ReadOnly = true;
            this.total_tax.Width = 125;
            // 
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle2;
            this.discount_value.HeaderText = "Total Disc:";
            this.discount_value.MinimumWidth = 6;
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            this.discount_value.Width = 125;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.total.DefaultCellStyle = dataGridViewCellStyle3;
            this.total.HeaderText = "Net Total";
            this.total.MinimumWidth = 6;
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.Width = 125;
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
            // sale_time
            // 
            this.sale_time.DataPropertyName = "sale_time";
            this.sale_time.HeaderText = "Sale Time";
            this.sale_time.MinimumWidth = 6;
            this.sale_time.Name = "sale_time";
            this.sale_time.ReadOnly = true;
            this.sale_time.Visible = false;
            this.sale_time.Width = 125;
            // 
            // frm_zatca_invoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1142, 688);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "frm_zatca_invoices";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZATCA Invoice Management";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_zatca_invoices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridZatcaInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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