namespace pos
{
    partial class frm_accounting_settings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabCompany = new System.Windows.Forms.TabPage();
            this.groupCompanyHeader = new System.Windows.Forms.GroupBox();
            this.lblCurrencyLockNote = new System.Windows.Forms.Label();
            this.txtLogoPath = new System.Windows.Forms.TextBox();
            this.btnBrowseLogo = new System.Windows.Forms.Button();
            this.picLogoPreview = new System.Windows.Forms.PictureBox();
            this.cmbCountry = new System.Windows.Forms.ComboBox();
            this.cmbBaseCurrency = new System.Windows.Forms.ComboBox();
            this.cmbFyEndMonth = new System.Windows.Forms.ComboBox();
            this.cmbFyStartMonth = new System.Windows.Forms.ComboBox();
            this.txtWebsite = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtStrn = new System.Windows.Forms.TextBox();
            this.txtNtnVat = new System.Windows.Forms.TextBox();
            this.txtRegistrationNo = new System.Windows.Forms.TextBox();
            this.txtLegalName = new System.Windows.Forms.TextBox();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.lblBaseCurrency = new System.Windows.Forms.Label();
            this.lblFyEnd = new System.Windows.Forms.Label();
            this.lblFyStart = new System.Windows.Forms.Label();
            this.lblWebsite = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblStrn = new System.Windows.Forms.Label();
            this.lblNtnVat = new System.Windows.Forms.Label();
            this.lblRegistration = new System.Windows.Forms.Label();
            this.lblLegalName = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.tabDefaults = new System.Windows.Forms.TabPage();
            this.btnTestAutoPostingRules = new System.Windows.Forms.Button();
            this.tblDefaults = new System.Windows.Forms.TableLayoutPanel();
            this.lblSalesAr = new System.Windows.Forms.Label();
            this.cmbSalesAr = new System.Windows.Forms.ComboBox();
            this.lblSalesRevenue = new System.Windows.Forms.Label();
            this.cmbSalesRevenue = new System.Windows.Forms.ComboBox();
            this.lblSalesTaxOutput = new System.Windows.Forms.Label();
            this.cmbSalesTaxOutput = new System.Windows.Forms.ComboBox();
            this.lblPurchaseAp = new System.Windows.Forms.Label();
            this.cmbPurchaseAp = new System.Windows.Forms.ComboBox();
            this.lblPurchaseCogs = new System.Windows.Forms.Label();
            this.cmbPurchaseCogs = new System.Windows.Forms.ComboBox();
            this.lblPurchaseTaxInput = new System.Windows.Forms.Label();
            this.cmbPurchaseTaxInput = new System.Windows.Forms.ComboBox();
            this.lblDefaultExpense = new System.Windows.Forms.Label();
            this.cmbDefaultExpense = new System.Windows.Forms.ComboBox();
            this.lblDefaultCash = new System.Windows.Forms.Label();
            this.cmbDefaultCash = new System.Windows.Forms.ComboBox();
            this.lblDefaultBank = new System.Windows.Forms.Label();
            this.cmbDefaultBank = new System.Windows.Forms.ComboBox();
            this.lblSalaryExpense = new System.Windows.Forms.Label();
            this.cmbSalaryExpense = new System.Windows.Forms.ComboBox();
            this.lblSalaryPayable = new System.Windows.Forms.Label();
            this.cmbSalaryPayable = new System.Windows.Forms.ComboBox();
            this.lblInventoryAsset = new System.Windows.Forms.Label();
            this.cmbInventoryAsset = new System.Windows.Forms.ComboBox();
            this.lblInventoryCogs = new System.Windows.Forms.Label();
            this.cmbInventoryCogs = new System.Windows.Forms.ComboBox();
            this.lblInventoryAdjustment = new System.Windows.Forms.Label();
            this.cmbInventoryAdjustment = new System.Windows.Forms.ComboBox();
            this.lblFaAsset = new System.Windows.Forms.Label();
            this.cmbFaAsset = new System.Windows.Forms.ComboBox();
            this.lblFaAccumDep = new System.Windows.Forms.Label();
            this.cmbFaAccumDep = new System.Windows.Forms.ComboBox();
            this.lblFaDepExpense = new System.Windows.Forms.Label();
            this.cmbFaDepExpense = new System.Windows.Forms.ComboBox();
            this.lblInterBranchRec = new System.Windows.Forms.Label();
            this.cmbInterBranchRec = new System.Windows.Forms.ComboBox();
            this.lblInterBranchPay = new System.Windows.Forms.Label();
            this.cmbInterBranchPay = new System.Windows.Forms.ComboBox();
            this.lblOpeningEquity = new System.Windows.Forms.Label();
            this.cmbOpeningEquity = new System.Windows.Forms.ComboBox();
            this.tabVoucher = new System.Windows.Forms.TabPage();
            this.gridVoucher = new System.Windows.Forms.DataGridView();
            this.colVoucherType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoucherPrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoucherBranchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoucherFormat = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colVoucherReset = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colVoucherStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoucherPreview = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabTax = new System.Windows.Forms.TabPage();
            this.gridWhtRates = new System.Windows.Forms.DataGridView();
            this.colWhtId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWhtType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaxSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWhtDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWhtRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffectiveFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupTaxTop = new System.Windows.Forms.GroupBox();
            this.cmbFilingFrequency = new System.Windows.Forms.ComboBox();
            this.txtFbrStrn = new System.Windows.Forms.TextBox();
            this.txtFbrNtn = new System.Windows.Forms.TextBox();
            this.cmbTaxMode = new System.Windows.Forms.ComboBox();
            this.numSalesTaxRate = new System.Windows.Forms.NumericUpDown();
            this.lblFilingFrequency = new System.Windows.Forms.Label();
            this.lblFbrStrn = new System.Windows.Forms.Label();
            this.lblFbrNtn = new System.Windows.Forms.Label();
            this.lblTaxMode = new System.Windows.Forms.Label();
            this.lblSalesTaxRate = new System.Windows.Forms.Label();
            this.tabPosting = new System.Windows.Forms.TabPage();
            this.groupPosting = new System.Windows.Forms.GroupBox();
            this.numApprovalThreshold = new System.Windows.Forms.NumericUpDown();
            this.numBackdatingDays = new System.Windows.Forms.NumericUpDown();
            this.numBudgetWarningPct = new System.Windows.Forms.NumericUpDown();
            this.chkRequireNarration = new System.Windows.Forms.CheckBox();
            this.chkAllowLockedPeriodPosting = new System.Windows.Forms.CheckBox();
            this.chkAutoPostPurchases = new System.Windows.Forms.CheckBox();
            this.chkAutoPostSales = new System.Windows.Forms.CheckBox();
            this.lblApprovalThreshold = new System.Windows.Forms.Label();
            this.lblBackdatingDays = new System.Windows.Forms.Label();
            this.lblBudgetWarning = new System.Windows.Forms.Label();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.groupReports = new System.Windows.Forms.GroupBox();
            this.txtDigitalSignature = new System.Windows.Forms.TextBox();
            this.txtReportFooter = new System.Windows.Forms.TextBox();
            this.txtReportHeader = new System.Windows.Forms.TextBox();
            this.cmbShowAmountsIn = new System.Windows.Forms.ComboBox();
            this.cmbReportDateFormat = new System.Windows.Forms.ComboBox();
            this.cmbAmountFormat = new System.Windows.Forms.ComboBox();
            this.lblDigitalSignature = new System.Windows.Forms.Label();
            this.lblReportFooter = new System.Windows.Forms.Label();
            this.lblReportHeader = new System.Windows.Forms.Label();
            this.lblShowAmountsIn = new System.Windows.Forms.Label();
            this.lblDateFormat = new System.Windows.Forms.Label();
            this.lblAmountFormat = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnResetDefaults = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.tabMain.SuspendLayout();
            this.tabCompany.SuspendLayout();
            this.groupCompanyHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoPreview)).BeginInit();
            this.tabDefaults.SuspendLayout();
            this.tblDefaults.SuspendLayout();
            this.tabVoucher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridVoucher)).BeginInit();
            this.tabTax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridWhtRates)).BeginInit();
            this.groupTaxTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTaxRate)).BeginInit();
            this.tabPosting.SuspendLayout();
            this.groupPosting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numApprovalThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBackdatingDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBudgetWarningPct)).BeginInit();
            this.tabReports.SuspendLayout();
            this.groupReports.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(275, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Accounting Settings (Admin)";
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabCompany);
            this.tabMain.Controls.Add(this.tabDefaults);
            this.tabMain.Controls.Add(this.tabVoucher);
            this.tabMain.Controls.Add(this.tabTax);
            this.tabMain.Controls.Add(this.tabPosting);
            this.tabMain.Controls.Add(this.tabReports);
            this.tabMain.Location = new System.Drawing.Point(12, 40);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1360, 661);
            this.tabMain.TabIndex = 1;
            // 
            // tabCompany
            // 
            this.tabCompany.Controls.Add(this.groupCompanyHeader);
            this.tabCompany.Location = new System.Drawing.Point(4, 30);
            this.tabCompany.Name = "tabCompany";
            this.tabCompany.Padding = new System.Windows.Forms.Padding(8);
            this.tabCompany.Size = new System.Drawing.Size(1352, 627);
            this.tabCompany.TabIndex = 0;
            this.tabCompany.Text = "Company & Financial Year";
            this.tabCompany.UseVisualStyleBackColor = true;
            // 
            // groupCompanyHeader
            // 
            this.groupCompanyHeader.Controls.Add(this.lblCurrencyLockNote);
            this.groupCompanyHeader.Controls.Add(this.txtLogoPath);
            this.groupCompanyHeader.Controls.Add(this.btnBrowseLogo);
            this.groupCompanyHeader.Controls.Add(this.picLogoPreview);
            this.groupCompanyHeader.Controls.Add(this.cmbCountry);
            this.groupCompanyHeader.Controls.Add(this.cmbBaseCurrency);
            this.groupCompanyHeader.Controls.Add(this.cmbFyEndMonth);
            this.groupCompanyHeader.Controls.Add(this.cmbFyStartMonth);
            this.groupCompanyHeader.Controls.Add(this.txtWebsite);
            this.groupCompanyHeader.Controls.Add(this.txtEmail);
            this.groupCompanyHeader.Controls.Add(this.txtPhone);
            this.groupCompanyHeader.Controls.Add(this.txtAddress);
            this.groupCompanyHeader.Controls.Add(this.txtStrn);
            this.groupCompanyHeader.Controls.Add(this.txtNtnVat);
            this.groupCompanyHeader.Controls.Add(this.txtRegistrationNo);
            this.groupCompanyHeader.Controls.Add(this.txtLegalName);
            this.groupCompanyHeader.Controls.Add(this.txtCompanyName);
            this.groupCompanyHeader.Controls.Add(this.lblCountry);
            this.groupCompanyHeader.Controls.Add(this.lblBaseCurrency);
            this.groupCompanyHeader.Controls.Add(this.lblFyEnd);
            this.groupCompanyHeader.Controls.Add(this.lblFyStart);
            this.groupCompanyHeader.Controls.Add(this.lblWebsite);
            this.groupCompanyHeader.Controls.Add(this.lblEmail);
            this.groupCompanyHeader.Controls.Add(this.lblPhone);
            this.groupCompanyHeader.Controls.Add(this.lblAddress);
            this.groupCompanyHeader.Controls.Add(this.lblStrn);
            this.groupCompanyHeader.Controls.Add(this.lblNtnVat);
            this.groupCompanyHeader.Controls.Add(this.lblRegistration);
            this.groupCompanyHeader.Controls.Add(this.lblLegalName);
            this.groupCompanyHeader.Controls.Add(this.lblCompanyName);
            this.groupCompanyHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupCompanyHeader.Location = new System.Drawing.Point(8, 8);
            this.groupCompanyHeader.Name = "groupCompanyHeader";
            this.groupCompanyHeader.Size = new System.Drawing.Size(1336, 611);
            this.groupCompanyHeader.TabIndex = 0;
            this.groupCompanyHeader.TabStop = false;
            this.groupCompanyHeader.Text = "Company Profile";
            // 
            // lblCurrencyLockNote
            // 
            this.lblCurrencyLockNote.AutoSize = true;
            this.lblCurrencyLockNote.ForeColor = System.Drawing.Color.Firebrick;
            this.lblCurrencyLockNote.Location = new System.Drawing.Point(436, 288);
            this.lblCurrencyLockNote.Name = "lblCurrencyLockNote";
            this.lblCurrencyLockNote.Size = new System.Drawing.Size(0, 23);
            this.lblCurrencyLockNote.TabIndex = 29;
            // 
            // txtLogoPath
            // 
            this.txtLogoPath.Location = new System.Drawing.Point(154, 396);
            this.txtLogoPath.Name = "txtLogoPath";
            this.txtLogoPath.Size = new System.Drawing.Size(600, 29);
            this.txtLogoPath.TabIndex = 14;
            // 
            // btnBrowseLogo
            // 
            this.btnBrowseLogo.Location = new System.Drawing.Point(769, 396);
            this.btnBrowseLogo.Name = "btnBrowseLogo";
            this.btnBrowseLogo.Size = new System.Drawing.Size(75, 29);
            this.btnBrowseLogo.TabIndex = 15;
            this.btnBrowseLogo.Text = "Browse";
            this.btnBrowseLogo.UseVisualStyleBackColor = true;
            this.btnBrowseLogo.Click += new System.EventHandler(this.btnBrowseLogo_Click);
            // 
            // picLogoPreview
            // 
            this.picLogoPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogoPreview.Location = new System.Drawing.Point(942, 28);
            this.picLogoPreview.Name = "picLogoPreview";
            this.picLogoPreview.Size = new System.Drawing.Size(180, 120);
            this.picLogoPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogoPreview.TabIndex = 26;
            this.picLogoPreview.TabStop = false;
            // 
            // cmbCountry
            // 
            this.cmbCountry.FormattingEnabled = true;
            this.cmbCountry.Location = new System.Drawing.Point(602, 217);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(294, 29);
            this.cmbCountry.TabIndex = 13;
            // 
            // cmbBaseCurrency
            // 
            this.cmbBaseCurrency.FormattingEnabled = true;
            this.cmbBaseCurrency.Location = new System.Drawing.Point(159, 285);
            this.cmbBaseCurrency.Name = "cmbBaseCurrency";
            this.cmbBaseCurrency.Size = new System.Drawing.Size(300, 29);
            this.cmbBaseCurrency.TabIndex = 12;
            // 
            // cmbFyEndMonth
            // 
            this.cmbFyEndMonth.FormattingEnabled = true;
            this.cmbFyEndMonth.Location = new System.Drawing.Point(602, 181);
            this.cmbFyEndMonth.Name = "cmbFyEndMonth";
            this.cmbFyEndMonth.Size = new System.Drawing.Size(294, 29);
            this.cmbFyEndMonth.TabIndex = 11;
            // 
            // cmbFyStartMonth
            // 
            this.cmbFyStartMonth.FormattingEnabled = true;
            this.cmbFyStartMonth.Location = new System.Drawing.Point(159, 250);
            this.cmbFyStartMonth.Name = "cmbFyStartMonth";
            this.cmbFyStartMonth.Size = new System.Drawing.Size(300, 29);
            this.cmbFyStartMonth.TabIndex = 10;
            this.cmbFyStartMonth.SelectedIndexChanged += new System.EventHandler(this.cmbFyStartMonth_SelectedIndexChanged);
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(602, 145);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(294, 29);
            this.txtWebsite.TabIndex = 9;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(159, 213);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 29);
            this.txtEmail.TabIndex = 8;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(602, 109);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(294, 29);
            this.txtPhone.TabIndex = 7;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(159, 145);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(300, 60);
            this.txtAddress.TabIndex = 6;
            // 
            // txtStrn
            // 
            this.txtStrn.Location = new System.Drawing.Point(602, 73);
            this.txtStrn.Name = "txtStrn";
            this.txtStrn.Size = new System.Drawing.Size(294, 29);
            this.txtStrn.TabIndex = 5;
            // 
            // txtNtnVat
            // 
            this.txtNtnVat.Location = new System.Drawing.Point(159, 108);
            this.txtNtnVat.Name = "txtNtnVat";
            this.txtNtnVat.Size = new System.Drawing.Size(300, 29);
            this.txtNtnVat.TabIndex = 4;
            // 
            // txtRegistrationNo
            // 
            this.txtRegistrationNo.Location = new System.Drawing.Point(602, 37);
            this.txtRegistrationNo.Name = "txtRegistrationNo";
            this.txtRegistrationNo.Size = new System.Drawing.Size(294, 29);
            this.txtRegistrationNo.TabIndex = 3;
            // 
            // txtLegalName
            // 
            this.txtLegalName.Location = new System.Drawing.Point(159, 34);
            this.txtLegalName.Name = "txtLegalName";
            this.txtLegalName.Size = new System.Drawing.Size(300, 29);
            this.txtLegalName.TabIndex = 2;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(159, 71);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(300, 29);
            this.txtCompanyName.TabIndex = 1;
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(467, 217);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(71, 23);
            this.lblCountry.TabIndex = 12;
            this.lblCountry.Text = "Country";
            // 
            // lblBaseCurrency
            // 
            this.lblBaseCurrency.AutoSize = true;
            this.lblBaseCurrency.Location = new System.Drawing.Point(25, 288);
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            this.lblBaseCurrency.Size = new System.Drawing.Size(118, 23);
            this.lblBaseCurrency.TabIndex = 11;
            this.lblBaseCurrency.Text = "Base Currency";
            // 
            // lblFyEnd
            // 
            this.lblFyEnd.AutoSize = true;
            this.lblFyEnd.Location = new System.Drawing.Point(467, 181);
            this.lblFyEnd.Name = "lblFyEnd";
            this.lblFyEnd.Size = new System.Drawing.Size(117, 23);
            this.lblFyEnd.TabIndex = 10;
            this.lblFyEnd.Text = "FY End Month";
            // 
            // lblFyStart
            // 
            this.lblFyStart.AutoSize = true;
            this.lblFyStart.Location = new System.Drawing.Point(25, 252);
            this.lblFyStart.Name = "lblFyStart";
            this.lblFyStart.Size = new System.Drawing.Size(123, 23);
            this.lblFyStart.TabIndex = 9;
            this.lblFyStart.Text = "FY Start Month";
            // 
            // lblWebsite
            // 
            this.lblWebsite.AutoSize = true;
            this.lblWebsite.Location = new System.Drawing.Point(467, 145);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(70, 23);
            this.lblWebsite.TabIndex = 8;
            this.lblWebsite.Text = "Website";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(25, 216);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(51, 23);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Email";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(467, 109);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(59, 23);
            this.lblPhone.TabIndex = 6;
            this.lblPhone.Text = "Phone";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(25, 148);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(70, 23);
            this.lblAddress.TabIndex = 5;
            this.lblAddress.Text = "Address";
            // 
            // lblStrn
            // 
            this.lblStrn.AutoSize = true;
            this.lblStrn.Location = new System.Drawing.Point(467, 73);
            this.lblStrn.Name = "lblStrn";
            this.lblStrn.Size = new System.Drawing.Size(51, 23);
            this.lblStrn.TabIndex = 4;
            this.lblStrn.Text = "STRN";
            // 
            // lblNtnVat
            // 
            this.lblNtnVat.AutoSize = true;
            this.lblNtnVat.Location = new System.Drawing.Point(25, 112);
            this.lblNtnVat.Name = "lblNtnVat";
            this.lblNtnVat.Size = new System.Drawing.Size(113, 23);
            this.lblNtnVat.TabIndex = 3;
            this.lblNtnVat.Text = "NTN/VAT No.";
            // 
            // lblRegistration
            // 
            this.lblRegistration.AutoSize = true;
            this.lblRegistration.Location = new System.Drawing.Point(467, 37);
            this.lblRegistration.Name = "lblRegistration";
            this.lblRegistration.Size = new System.Drawing.Size(129, 23);
            this.lblRegistration.TabIndex = 2;
            this.lblRegistration.Text = "Registration No";
            // 
            // lblLegalName
            // 
            this.lblLegalName.AutoSize = true;
            this.lblLegalName.Location = new System.Drawing.Point(25, 40);
            this.lblLegalName.Name = "lblLegalName";
            this.lblLegalName.Size = new System.Drawing.Size(101, 23);
            this.lblLegalName.TabIndex = 1;
            this.lblLegalName.Text = "Legal Name";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(25, 76);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(134, 23);
            this.lblCompanyName.TabIndex = 0;
            this.lblCompanyName.Text = "Company Name";
            // 
            // tabDefaults
            // 
            this.tabDefaults.Controls.Add(this.btnTestAutoPostingRules);
            this.tabDefaults.Controls.Add(this.tblDefaults);
            this.tabDefaults.Location = new System.Drawing.Point(4, 30);
            this.tabDefaults.Name = "tabDefaults";
            this.tabDefaults.Padding = new System.Windows.Forms.Padding(8);
            this.tabDefaults.Size = new System.Drawing.Size(1352, 627);
            this.tabDefaults.TabIndex = 1;
            this.tabDefaults.Text = "Default Accounts";
            this.tabDefaults.UseVisualStyleBackColor = true;
            // 
            // btnTestAutoPostingRules
            // 
            this.btnTestAutoPostingRules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestAutoPostingRules.Location = new System.Drawing.Point(1121, 594);
            this.btnTestAutoPostingRules.Name = "btnTestAutoPostingRules";
            this.btnTestAutoPostingRules.Size = new System.Drawing.Size(220, 27);
            this.btnTestAutoPostingRules.TabIndex = 1;
            this.btnTestAutoPostingRules.Text = "Test Auto-Posting Rules";
            this.btnTestAutoPostingRules.UseVisualStyleBackColor = true;
            this.btnTestAutoPostingRules.Click += new System.EventHandler(this.btnTestAutoPostingRules_Click);
            // 
            // tblDefaults
            // 
            this.tblDefaults.ColumnCount = 4;
            this.tblDefaults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tblDefaults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblDefaults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tblDefaults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblDefaults.Controls.Add(this.lblSalesAr, 0, 0);
            this.tblDefaults.Controls.Add(this.cmbSalesAr, 1, 0);
            this.tblDefaults.Controls.Add(this.lblSalesRevenue, 2, 0);
            this.tblDefaults.Controls.Add(this.cmbSalesRevenue, 3, 0);
            this.tblDefaults.Controls.Add(this.lblSalesTaxOutput, 0, 1);
            this.tblDefaults.Controls.Add(this.cmbSalesTaxOutput, 1, 1);
            this.tblDefaults.Controls.Add(this.lblPurchaseAp, 2, 1);
            this.tblDefaults.Controls.Add(this.cmbPurchaseAp, 3, 1);
            this.tblDefaults.Controls.Add(this.lblPurchaseCogs, 0, 2);
            this.tblDefaults.Controls.Add(this.cmbPurchaseCogs, 1, 2);
            this.tblDefaults.Controls.Add(this.lblPurchaseTaxInput, 2, 2);
            this.tblDefaults.Controls.Add(this.cmbPurchaseTaxInput, 3, 2);
            this.tblDefaults.Controls.Add(this.lblDefaultExpense, 0, 3);
            this.tblDefaults.Controls.Add(this.cmbDefaultExpense, 1, 3);
            this.tblDefaults.Controls.Add(this.lblDefaultCash, 2, 3);
            this.tblDefaults.Controls.Add(this.cmbDefaultCash, 3, 3);
            this.tblDefaults.Controls.Add(this.lblDefaultBank, 0, 4);
            this.tblDefaults.Controls.Add(this.cmbDefaultBank, 1, 4);
            this.tblDefaults.Controls.Add(this.lblSalaryExpense, 2, 4);
            this.tblDefaults.Controls.Add(this.cmbSalaryExpense, 3, 4);
            this.tblDefaults.Controls.Add(this.lblSalaryPayable, 0, 5);
            this.tblDefaults.Controls.Add(this.cmbSalaryPayable, 1, 5);
            this.tblDefaults.Controls.Add(this.lblInventoryAsset, 2, 5);
            this.tblDefaults.Controls.Add(this.cmbInventoryAsset, 3, 5);
            this.tblDefaults.Controls.Add(this.lblInventoryCogs, 0, 6);
            this.tblDefaults.Controls.Add(this.cmbInventoryCogs, 1, 6);
            this.tblDefaults.Controls.Add(this.lblInventoryAdjustment, 2, 6);
            this.tblDefaults.Controls.Add(this.cmbInventoryAdjustment, 3, 6);
            this.tblDefaults.Controls.Add(this.lblFaAsset, 0, 7);
            this.tblDefaults.Controls.Add(this.cmbFaAsset, 1, 7);
            this.tblDefaults.Controls.Add(this.lblFaAccumDep, 2, 7);
            this.tblDefaults.Controls.Add(this.cmbFaAccumDep, 3, 7);
            this.tblDefaults.Controls.Add(this.lblFaDepExpense, 0, 8);
            this.tblDefaults.Controls.Add(this.cmbFaDepExpense, 1, 8);
            this.tblDefaults.Controls.Add(this.lblInterBranchRec, 2, 8);
            this.tblDefaults.Controls.Add(this.cmbInterBranchRec, 3, 8);
            this.tblDefaults.Controls.Add(this.lblInterBranchPay, 0, 9);
            this.tblDefaults.Controls.Add(this.cmbInterBranchPay, 1, 9);
            this.tblDefaults.Controls.Add(this.lblOpeningEquity, 2, 9);
            this.tblDefaults.Controls.Add(this.cmbOpeningEquity, 3, 9);
            this.tblDefaults.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblDefaults.Location = new System.Drawing.Point(8, 8);
            this.tblDefaults.Name = "tblDefaults";
            this.tblDefaults.RowCount = 10;
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblDefaults.Size = new System.Drawing.Size(1336, 410);
            this.tblDefaults.TabIndex = 0;
            // 
            // lblSalesAr
            // 
            this.lblSalesAr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalesAr.Location = new System.Drawing.Point(3, 0);
            this.lblSalesAr.Name = "lblSalesAr";
            this.lblSalesAr.Size = new System.Drawing.Size(194, 40);
            this.lblSalesAr.TabIndex = 0;
            this.lblSalesAr.Text = "Sales AR Account";
            this.lblSalesAr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSalesAr
            // 
            this.cmbSalesAr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSalesAr.FormattingEnabled = true;
            this.cmbSalesAr.Location = new System.Drawing.Point(203, 3);
            this.cmbSalesAr.Name = "cmbSalesAr";
            this.cmbSalesAr.Size = new System.Drawing.Size(452, 29);
            this.cmbSalesAr.TabIndex = 1;
            // 
            // lblSalesRevenue
            // 
            this.lblSalesRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalesRevenue.Location = new System.Drawing.Point(661, 0);
            this.lblSalesRevenue.Name = "lblSalesRevenue";
            this.lblSalesRevenue.Size = new System.Drawing.Size(214, 40);
            this.lblSalesRevenue.TabIndex = 2;
            this.lblSalesRevenue.Text = "Sales Revenue";
            this.lblSalesRevenue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSalesRevenue
            // 
            this.cmbSalesRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSalesRevenue.FormattingEnabled = true;
            this.cmbSalesRevenue.Location = new System.Drawing.Point(881, 3);
            this.cmbSalesRevenue.Name = "cmbSalesRevenue";
            this.cmbSalesRevenue.Size = new System.Drawing.Size(452, 29);
            this.cmbSalesRevenue.TabIndex = 3;
            // 
            // lblSalesTaxOutput
            // 
            this.lblSalesTaxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalesTaxOutput.Location = new System.Drawing.Point(3, 40);
            this.lblSalesTaxOutput.Name = "lblSalesTaxOutput";
            this.lblSalesTaxOutput.Size = new System.Drawing.Size(194, 40);
            this.lblSalesTaxOutput.TabIndex = 4;
            this.lblSalesTaxOutput.Text = "Sales Tax Output";
            this.lblSalesTaxOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSalesTaxOutput
            // 
            this.cmbSalesTaxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSalesTaxOutput.FormattingEnabled = true;
            this.cmbSalesTaxOutput.Location = new System.Drawing.Point(203, 43);
            this.cmbSalesTaxOutput.Name = "cmbSalesTaxOutput";
            this.cmbSalesTaxOutput.Size = new System.Drawing.Size(452, 29);
            this.cmbSalesTaxOutput.TabIndex = 5;
            // 
            // lblPurchaseAp
            // 
            this.lblPurchaseAp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPurchaseAp.Location = new System.Drawing.Point(661, 40);
            this.lblPurchaseAp.Name = "lblPurchaseAp";
            this.lblPurchaseAp.Size = new System.Drawing.Size(214, 40);
            this.lblPurchaseAp.TabIndex = 6;
            this.lblPurchaseAp.Text = "Purchase AP Account";
            this.lblPurchaseAp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPurchaseAp
            // 
            this.cmbPurchaseAp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPurchaseAp.FormattingEnabled = true;
            this.cmbPurchaseAp.Location = new System.Drawing.Point(881, 43);
            this.cmbPurchaseAp.Name = "cmbPurchaseAp";
            this.cmbPurchaseAp.Size = new System.Drawing.Size(452, 29);
            this.cmbPurchaseAp.TabIndex = 7;
            // 
            // lblPurchaseCogs
            // 
            this.lblPurchaseCogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPurchaseCogs.Location = new System.Drawing.Point(3, 80);
            this.lblPurchaseCogs.Name = "lblPurchaseCogs";
            this.lblPurchaseCogs.Size = new System.Drawing.Size(194, 40);
            this.lblPurchaseCogs.TabIndex = 8;
            this.lblPurchaseCogs.Text = "Purchase/COGS";
            this.lblPurchaseCogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPurchaseCogs
            // 
            this.cmbPurchaseCogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPurchaseCogs.FormattingEnabled = true;
            this.cmbPurchaseCogs.Location = new System.Drawing.Point(203, 83);
            this.cmbPurchaseCogs.Name = "cmbPurchaseCogs";
            this.cmbPurchaseCogs.Size = new System.Drawing.Size(452, 29);
            this.cmbPurchaseCogs.TabIndex = 9;
            // 
            // lblPurchaseTaxInput
            // 
            this.lblPurchaseTaxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPurchaseTaxInput.Location = new System.Drawing.Point(661, 80);
            this.lblPurchaseTaxInput.Name = "lblPurchaseTaxInput";
            this.lblPurchaseTaxInput.Size = new System.Drawing.Size(214, 40);
            this.lblPurchaseTaxInput.TabIndex = 10;
            this.lblPurchaseTaxInput.Text = "Purchase Tax Input";
            this.lblPurchaseTaxInput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPurchaseTaxInput
            // 
            this.cmbPurchaseTaxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPurchaseTaxInput.FormattingEnabled = true;
            this.cmbPurchaseTaxInput.Location = new System.Drawing.Point(881, 83);
            this.cmbPurchaseTaxInput.Name = "cmbPurchaseTaxInput";
            this.cmbPurchaseTaxInput.Size = new System.Drawing.Size(452, 29);
            this.cmbPurchaseTaxInput.TabIndex = 11;
            // 
            // lblDefaultExpense
            // 
            this.lblDefaultExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDefaultExpense.Location = new System.Drawing.Point(3, 120);
            this.lblDefaultExpense.Name = "lblDefaultExpense";
            this.lblDefaultExpense.Size = new System.Drawing.Size(194, 40);
            this.lblDefaultExpense.TabIndex = 12;
            this.lblDefaultExpense.Text = "Default Expense";
            this.lblDefaultExpense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDefaultExpense
            // 
            this.cmbDefaultExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDefaultExpense.FormattingEnabled = true;
            this.cmbDefaultExpense.Location = new System.Drawing.Point(203, 123);
            this.cmbDefaultExpense.Name = "cmbDefaultExpense";
            this.cmbDefaultExpense.Size = new System.Drawing.Size(452, 29);
            this.cmbDefaultExpense.TabIndex = 13;
            // 
            // lblDefaultCash
            // 
            this.lblDefaultCash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDefaultCash.Location = new System.Drawing.Point(661, 120);
            this.lblDefaultCash.Name = "lblDefaultCash";
            this.lblDefaultCash.Size = new System.Drawing.Size(214, 40);
            this.lblDefaultCash.TabIndex = 14;
            this.lblDefaultCash.Text = "Default Cash";
            this.lblDefaultCash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDefaultCash
            // 
            this.cmbDefaultCash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDefaultCash.FormattingEnabled = true;
            this.cmbDefaultCash.Location = new System.Drawing.Point(881, 123);
            this.cmbDefaultCash.Name = "cmbDefaultCash";
            this.cmbDefaultCash.Size = new System.Drawing.Size(452, 29);
            this.cmbDefaultCash.TabIndex = 15;
            // 
            // lblDefaultBank
            // 
            this.lblDefaultBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDefaultBank.Location = new System.Drawing.Point(3, 160);
            this.lblDefaultBank.Name = "lblDefaultBank";
            this.lblDefaultBank.Size = new System.Drawing.Size(194, 40);
            this.lblDefaultBank.TabIndex = 16;
            this.lblDefaultBank.Text = "Default Bank";
            this.lblDefaultBank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDefaultBank
            // 
            this.cmbDefaultBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDefaultBank.FormattingEnabled = true;
            this.cmbDefaultBank.Location = new System.Drawing.Point(203, 163);
            this.cmbDefaultBank.Name = "cmbDefaultBank";
            this.cmbDefaultBank.Size = new System.Drawing.Size(452, 29);
            this.cmbDefaultBank.TabIndex = 17;
            // 
            // lblSalaryExpense
            // 
            this.lblSalaryExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalaryExpense.Location = new System.Drawing.Point(661, 160);
            this.lblSalaryExpense.Name = "lblSalaryExpense";
            this.lblSalaryExpense.Size = new System.Drawing.Size(214, 40);
            this.lblSalaryExpense.TabIndex = 18;
            this.lblSalaryExpense.Text = "Salary Expense";
            this.lblSalaryExpense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSalaryExpense
            // 
            this.cmbSalaryExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSalaryExpense.FormattingEnabled = true;
            this.cmbSalaryExpense.Location = new System.Drawing.Point(881, 163);
            this.cmbSalaryExpense.Name = "cmbSalaryExpense";
            this.cmbSalaryExpense.Size = new System.Drawing.Size(452, 29);
            this.cmbSalaryExpense.TabIndex = 19;
            // 
            // lblSalaryPayable
            // 
            this.lblSalaryPayable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalaryPayable.Location = new System.Drawing.Point(3, 200);
            this.lblSalaryPayable.Name = "lblSalaryPayable";
            this.lblSalaryPayable.Size = new System.Drawing.Size(194, 40);
            this.lblSalaryPayable.TabIndex = 20;
            this.lblSalaryPayable.Text = "Salary Payable";
            this.lblSalaryPayable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSalaryPayable
            // 
            this.cmbSalaryPayable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSalaryPayable.FormattingEnabled = true;
            this.cmbSalaryPayable.Location = new System.Drawing.Point(203, 203);
            this.cmbSalaryPayable.Name = "cmbSalaryPayable";
            this.cmbSalaryPayable.Size = new System.Drawing.Size(452, 29);
            this.cmbSalaryPayable.TabIndex = 21;
            // 
            // lblInventoryAsset
            // 
            this.lblInventoryAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInventoryAsset.Location = new System.Drawing.Point(661, 200);
            this.lblInventoryAsset.Name = "lblInventoryAsset";
            this.lblInventoryAsset.Size = new System.Drawing.Size(214, 40);
            this.lblInventoryAsset.TabIndex = 22;
            this.lblInventoryAsset.Text = "Inventory Asset";
            this.lblInventoryAsset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbInventoryAsset
            // 
            this.cmbInventoryAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbInventoryAsset.FormattingEnabled = true;
            this.cmbInventoryAsset.Location = new System.Drawing.Point(881, 203);
            this.cmbInventoryAsset.Name = "cmbInventoryAsset";
            this.cmbInventoryAsset.Size = new System.Drawing.Size(452, 29);
            this.cmbInventoryAsset.TabIndex = 23;
            // 
            // lblInventoryCogs
            // 
            this.lblInventoryCogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInventoryCogs.Location = new System.Drawing.Point(3, 240);
            this.lblInventoryCogs.Name = "lblInventoryCogs";
            this.lblInventoryCogs.Size = new System.Drawing.Size(194, 40);
            this.lblInventoryCogs.TabIndex = 24;
            this.lblInventoryCogs.Text = "Inventory COGS";
            this.lblInventoryCogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbInventoryCogs
            // 
            this.cmbInventoryCogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbInventoryCogs.FormattingEnabled = true;
            this.cmbInventoryCogs.Location = new System.Drawing.Point(203, 243);
            this.cmbInventoryCogs.Name = "cmbInventoryCogs";
            this.cmbInventoryCogs.Size = new System.Drawing.Size(452, 29);
            this.cmbInventoryCogs.TabIndex = 25;
            // 
            // lblInventoryAdjustment
            // 
            this.lblInventoryAdjustment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInventoryAdjustment.Location = new System.Drawing.Point(661, 240);
            this.lblInventoryAdjustment.Name = "lblInventoryAdjustment";
            this.lblInventoryAdjustment.Size = new System.Drawing.Size(214, 40);
            this.lblInventoryAdjustment.TabIndex = 26;
            this.lblInventoryAdjustment.Text = "Stock Adjustment";
            this.lblInventoryAdjustment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbInventoryAdjustment
            // 
            this.cmbInventoryAdjustment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbInventoryAdjustment.FormattingEnabled = true;
            this.cmbInventoryAdjustment.Location = new System.Drawing.Point(881, 243);
            this.cmbInventoryAdjustment.Name = "cmbInventoryAdjustment";
            this.cmbInventoryAdjustment.Size = new System.Drawing.Size(452, 29);
            this.cmbInventoryAdjustment.TabIndex = 27;
            // 
            // lblFaAsset
            // 
            this.lblFaAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFaAsset.Location = new System.Drawing.Point(3, 280);
            this.lblFaAsset.Name = "lblFaAsset";
            this.lblFaAsset.Size = new System.Drawing.Size(194, 40);
            this.lblFaAsset.TabIndex = 28;
            this.lblFaAsset.Text = "FA Asset";
            this.lblFaAsset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbFaAsset
            // 
            this.cmbFaAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFaAsset.FormattingEnabled = true;
            this.cmbFaAsset.Location = new System.Drawing.Point(203, 283);
            this.cmbFaAsset.Name = "cmbFaAsset";
            this.cmbFaAsset.Size = new System.Drawing.Size(452, 29);
            this.cmbFaAsset.TabIndex = 29;
            // 
            // lblFaAccumDep
            // 
            this.lblFaAccumDep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFaAccumDep.Location = new System.Drawing.Point(661, 280);
            this.lblFaAccumDep.Name = "lblFaAccumDep";
            this.lblFaAccumDep.Size = new System.Drawing.Size(214, 40);
            this.lblFaAccumDep.TabIndex = 30;
            this.lblFaAccumDep.Text = "Accum Depreciation";
            this.lblFaAccumDep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbFaAccumDep
            // 
            this.cmbFaAccumDep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFaAccumDep.FormattingEnabled = true;
            this.cmbFaAccumDep.Location = new System.Drawing.Point(881, 283);
            this.cmbFaAccumDep.Name = "cmbFaAccumDep";
            this.cmbFaAccumDep.Size = new System.Drawing.Size(452, 29);
            this.cmbFaAccumDep.TabIndex = 31;
            // 
            // lblFaDepExpense
            // 
            this.lblFaDepExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFaDepExpense.Location = new System.Drawing.Point(3, 320);
            this.lblFaDepExpense.Name = "lblFaDepExpense";
            this.lblFaDepExpense.Size = new System.Drawing.Size(194, 40);
            this.lblFaDepExpense.TabIndex = 32;
            this.lblFaDepExpense.Text = "Depreciation Expense";
            this.lblFaDepExpense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbFaDepExpense
            // 
            this.cmbFaDepExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFaDepExpense.FormattingEnabled = true;
            this.cmbFaDepExpense.Location = new System.Drawing.Point(203, 323);
            this.cmbFaDepExpense.Name = "cmbFaDepExpense";
            this.cmbFaDepExpense.Size = new System.Drawing.Size(452, 29);
            this.cmbFaDepExpense.TabIndex = 33;
            // 
            // lblInterBranchRec
            // 
            this.lblInterBranchRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInterBranchRec.Location = new System.Drawing.Point(661, 320);
            this.lblInterBranchRec.Name = "lblInterBranchRec";
            this.lblInterBranchRec.Size = new System.Drawing.Size(214, 40);
            this.lblInterBranchRec.TabIndex = 34;
            this.lblInterBranchRec.Text = "Inter-Branch Receivable";
            this.lblInterBranchRec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbInterBranchRec
            // 
            this.cmbInterBranchRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbInterBranchRec.FormattingEnabled = true;
            this.cmbInterBranchRec.Location = new System.Drawing.Point(881, 323);
            this.cmbInterBranchRec.Name = "cmbInterBranchRec";
            this.cmbInterBranchRec.Size = new System.Drawing.Size(452, 29);
            this.cmbInterBranchRec.TabIndex = 35;
            // 
            // lblInterBranchPay
            // 
            this.lblInterBranchPay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInterBranchPay.Location = new System.Drawing.Point(3, 360);
            this.lblInterBranchPay.Name = "lblInterBranchPay";
            this.lblInterBranchPay.Size = new System.Drawing.Size(194, 50);
            this.lblInterBranchPay.TabIndex = 36;
            this.lblInterBranchPay.Text = "Inter-Branch Payable";
            this.lblInterBranchPay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbInterBranchPay
            // 
            this.cmbInterBranchPay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbInterBranchPay.FormattingEnabled = true;
            this.cmbInterBranchPay.Location = new System.Drawing.Point(203, 363);
            this.cmbInterBranchPay.Name = "cmbInterBranchPay";
            this.cmbInterBranchPay.Size = new System.Drawing.Size(452, 29);
            this.cmbInterBranchPay.TabIndex = 37;
            // 
            // lblOpeningEquity
            // 
            this.lblOpeningEquity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOpeningEquity.Location = new System.Drawing.Point(661, 360);
            this.lblOpeningEquity.Name = "lblOpeningEquity";
            this.lblOpeningEquity.Size = new System.Drawing.Size(214, 50);
            this.lblOpeningEquity.TabIndex = 38;
            this.lblOpeningEquity.Text = "Opening Balance Equity";
            this.lblOpeningEquity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbOpeningEquity
            // 
            this.cmbOpeningEquity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbOpeningEquity.FormattingEnabled = true;
            this.cmbOpeningEquity.Location = new System.Drawing.Point(881, 363);
            this.cmbOpeningEquity.Name = "cmbOpeningEquity";
            this.cmbOpeningEquity.Size = new System.Drawing.Size(452, 29);
            this.cmbOpeningEquity.TabIndex = 39;
            // 
            // tabVoucher
            // 
            this.tabVoucher.Controls.Add(this.gridVoucher);
            this.tabVoucher.Location = new System.Drawing.Point(4, 30);
            this.tabVoucher.Name = "tabVoucher";
            this.tabVoucher.Padding = new System.Windows.Forms.Padding(8);
            this.tabVoucher.Size = new System.Drawing.Size(1352, 627);
            this.tabVoucher.TabIndex = 2;
            this.tabVoucher.Text = "Voucher Numbering";
            this.tabVoucher.UseVisualStyleBackColor = true;
            // 
            // gridVoucher
            // 
            this.gridVoucher.AllowUserToAddRows = false;
            this.gridVoucher.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridVoucher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridVoucher.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVoucherType,
            this.colVoucherPrefix,
            this.colVoucherBranchId,
            this.colVoucherFormat,
            this.colVoucherReset,
            this.colVoucherStart,
            this.colVoucherPreview});
            this.gridVoucher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridVoucher.Location = new System.Drawing.Point(8, 8);
            this.gridVoucher.Name = "gridVoucher";
            this.gridVoucher.RowHeadersWidth = 51;
            this.gridVoucher.Size = new System.Drawing.Size(1336, 611);
            this.gridVoucher.TabIndex = 0;
            this.gridVoucher.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridVoucher_CellEndEdit);
            // 
            // colVoucherType
            // 
            this.colVoucherType.HeaderText = "Voucher Type";
            this.colVoucherType.MinimumWidth = 6;
            this.colVoucherType.Name = "colVoucherType";
            this.colVoucherType.ReadOnly = true;
            // 
            // colVoucherPrefix
            // 
            this.colVoucherPrefix.HeaderText = "Prefix";
            this.colVoucherPrefix.MinimumWidth = 6;
            this.colVoucherPrefix.Name = "colVoucherPrefix";
            // 
            // colVoucherBranchId
            // 
            this.colVoucherBranchId.HeaderText = "Branch ID";
            this.colVoucherBranchId.MinimumWidth = 6;
            this.colVoucherBranchId.Name = "colVoucherBranchId";
            this.colVoucherBranchId.ReadOnly = true;
            // 
            // colVoucherFormat
            // 
            this.colVoucherFormat.HeaderText = "Number Format";
            this.colVoucherFormat.Items.AddRange(new object[] {
            "YYYY-NNNN",
            "YY-NNNN",
            "NNNN",
            "YYYYMMDD-NNNN",
            "YYYY-MM-DD-NNNN",
            "YYYYMMDD-YYYY-NNNN",
            "YYYY-MM-DD-YYYY-NNNN"});
            this.colVoucherFormat.MinimumWidth = 6;
            this.colVoucherFormat.Name = "colVoucherFormat";
            // 
            // colVoucherReset
            // 
            this.colVoucherReset.HeaderText = "Reset";
            this.colVoucherReset.Items.AddRange(new object[] {
            "Daily",
            "Annually",
            "Never",
            "Per Financial Year"});
            this.colVoucherReset.MinimumWidth = 6;
            this.colVoucherReset.Name = "colVoucherReset";
            // 
            // colVoucherStart
            // 
            this.colVoucherStart.HeaderText = "Starting Number";
            this.colVoucherStart.MinimumWidth = 6;
            this.colVoucherStart.Name = "colVoucherStart";
            // 
            // colVoucherPreview
            // 
            this.colVoucherPreview.HeaderText = "Preview";
            this.colVoucherPreview.MinimumWidth = 6;
            this.colVoucherPreview.Name = "colVoucherPreview";
            this.colVoucherPreview.ReadOnly = true;
            // 
            // tabTax
            // 
            this.tabTax.Controls.Add(this.gridWhtRates);
            this.tabTax.Controls.Add(this.groupTaxTop);
            this.tabTax.Location = new System.Drawing.Point(4, 30);
            this.tabTax.Name = "tabTax";
            this.tabTax.Padding = new System.Windows.Forms.Padding(8);
            this.tabTax.Size = new System.Drawing.Size(1352, 627);
            this.tabTax.TabIndex = 3;
            this.tabTax.Text = "Tax Configuration";
            this.tabTax.UseVisualStyleBackColor = true;
            // 
            // gridWhtRates
            // 
            this.gridWhtRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridWhtRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridWhtRates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWhtId,
            this.colWhtType,
            this.colTaxSection,
            this.colWhtDescription,
            this.colWhtRate,
            this.colEffectiveFrom,
            this.colIsActive});
            this.gridWhtRates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridWhtRates.Location = new System.Drawing.Point(8, 134);
            this.gridWhtRates.Name = "gridWhtRates";
            this.gridWhtRates.RowHeadersWidth = 51;
            this.gridWhtRates.Size = new System.Drawing.Size(1336, 485);
            this.gridWhtRates.TabIndex = 1;
            // 
            // colWhtId
            // 
            this.colWhtId.HeaderText = "ID";
            this.colWhtId.MinimumWidth = 6;
            this.colWhtId.Name = "colWhtId";
            this.colWhtId.Visible = false;
            // 
            // colWhtType
            // 
            this.colWhtType.HeaderText = "WHT Type";
            this.colWhtType.MinimumWidth = 6;
            this.colWhtType.Name = "colWhtType";
            // 
            // colTaxSection
            // 
            this.colTaxSection.HeaderText = "Tax Section";
            this.colTaxSection.MinimumWidth = 6;
            this.colTaxSection.Name = "colTaxSection";
            // 
            // colWhtDescription
            // 
            this.colWhtDescription.HeaderText = "Description";
            this.colWhtDescription.MinimumWidth = 6;
            this.colWhtDescription.Name = "colWhtDescription";
            // 
            // colWhtRate
            // 
            this.colWhtRate.HeaderText = "Rate %";
            this.colWhtRate.MinimumWidth = 6;
            this.colWhtRate.Name = "colWhtRate";
            // 
            // colEffectiveFrom
            // 
            this.colEffectiveFrom.HeaderText = "Effective From";
            this.colEffectiveFrom.MinimumWidth = 6;
            this.colEffectiveFrom.Name = "colEffectiveFrom";
            // 
            // colIsActive
            // 
            this.colIsActive.HeaderText = "Is Active";
            this.colIsActive.MinimumWidth = 6;
            this.colIsActive.Name = "colIsActive";
            // 
            // groupTaxTop
            // 
            this.groupTaxTop.Controls.Add(this.cmbFilingFrequency);
            this.groupTaxTop.Controls.Add(this.txtFbrStrn);
            this.groupTaxTop.Controls.Add(this.txtFbrNtn);
            this.groupTaxTop.Controls.Add(this.cmbTaxMode);
            this.groupTaxTop.Controls.Add(this.numSalesTaxRate);
            this.groupTaxTop.Controls.Add(this.lblFilingFrequency);
            this.groupTaxTop.Controls.Add(this.lblFbrStrn);
            this.groupTaxTop.Controls.Add(this.lblFbrNtn);
            this.groupTaxTop.Controls.Add(this.lblTaxMode);
            this.groupTaxTop.Controls.Add(this.lblSalesTaxRate);
            this.groupTaxTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupTaxTop.Location = new System.Drawing.Point(8, 8);
            this.groupTaxTop.Name = "groupTaxTop";
            this.groupTaxTop.Size = new System.Drawing.Size(1336, 126);
            this.groupTaxTop.TabIndex = 0;
            this.groupTaxTop.TabStop = false;
            this.groupTaxTop.Text = "Tax Defaults / FBR-ZATCA";
            // 
            // cmbFilingFrequency
            // 
            this.cmbFilingFrequency.Location = new System.Drawing.Point(897, 67);
            this.cmbFilingFrequency.Name = "cmbFilingFrequency";
            this.cmbFilingFrequency.Size = new System.Drawing.Size(200, 29);
            this.cmbFilingFrequency.TabIndex = 0;
            // 
            // txtFbrStrn
            // 
            this.txtFbrStrn.Location = new System.Drawing.Point(549, 66);
            this.txtFbrStrn.Name = "txtFbrStrn";
            this.txtFbrStrn.Size = new System.Drawing.Size(200, 29);
            this.txtFbrStrn.TabIndex = 1;
            // 
            // txtFbrNtn
            // 
            this.txtFbrNtn.Location = new System.Drawing.Point(180, 67);
            this.txtFbrNtn.Name = "txtFbrNtn";
            this.txtFbrNtn.Size = new System.Drawing.Size(220, 29);
            this.txtFbrNtn.TabIndex = 2;
            // 
            // cmbTaxMode
            // 
            this.cmbTaxMode.Location = new System.Drawing.Point(549, 29);
            this.cmbTaxMode.Name = "cmbTaxMode";
            this.cmbTaxMode.Size = new System.Drawing.Size(200, 29);
            this.cmbTaxMode.TabIndex = 3;
            // 
            // numSalesTaxRate
            // 
            this.numSalesTaxRate.DecimalPlaces = 2;
            this.numSalesTaxRate.Location = new System.Drawing.Point(180, 30);
            this.numSalesTaxRate.Name = "numSalesTaxRate";
            this.numSalesTaxRate.Size = new System.Drawing.Size(120, 29);
            this.numSalesTaxRate.TabIndex = 4;
            // 
            // lblFilingFrequency
            // 
            this.lblFilingFrequency.AutoSize = true;
            this.lblFilingFrequency.Location = new System.Drawing.Point(758, 67);
            this.lblFilingFrequency.Name = "lblFilingFrequency";
            this.lblFilingFrequency.Size = new System.Drawing.Size(133, 23);
            this.lblFilingFrequency.TabIndex = 5;
            this.lblFilingFrequency.Text = "Filing Frequency";
            // 
            // lblFbrStrn
            // 
            this.lblFbrStrn.AutoSize = true;
            this.lblFbrStrn.Location = new System.Drawing.Point(483, 70);
            this.lblFbrStrn.Name = "lblFbrStrn";
            this.lblFbrStrn.Size = new System.Drawing.Size(51, 23);
            this.lblFbrStrn.TabIndex = 6;
            this.lblFbrStrn.Text = "STRN";
            // 
            // lblFbrNtn
            // 
            this.lblFbrNtn.AutoSize = true;
            this.lblFbrNtn.Location = new System.Drawing.Point(20, 70);
            this.lblFbrNtn.Name = "lblFbrNtn";
            this.lblFbrNtn.Size = new System.Drawing.Size(135, 23);
            this.lblFbrNtn.TabIndex = 7;
            this.lblFbrNtn.Text = "FBR/ZATCA NTN";
            // 
            // lblTaxMode
            // 
            this.lblTaxMode.AutoSize = true;
            this.lblTaxMode.Location = new System.Drawing.Point(400, 33);
            this.lblTaxMode.Name = "lblTaxMode";
            this.lblTaxMode.Size = new System.Drawing.Size(143, 23);
            this.lblTaxMode.TabIndex = 8;
            this.lblTaxMode.Text = "Default Tax Mode";
            // 
            // lblSalesTaxRate
            // 
            this.lblSalesTaxRate.AutoSize = true;
            this.lblSalesTaxRate.Location = new System.Drawing.Point(20, 33);
            this.lblSalesTaxRate.Name = "lblSalesTaxRate";
            this.lblSalesTaxRate.Size = new System.Drawing.Size(156, 23);
            this.lblSalesTaxRate.TabIndex = 9;
            this.lblSalesTaxRate.Text = "Default Sales Tax %";
            // 
            // tabPosting
            // 
            this.tabPosting.Controls.Add(this.groupPosting);
            this.tabPosting.Location = new System.Drawing.Point(4, 30);
            this.tabPosting.Name = "tabPosting";
            this.tabPosting.Padding = new System.Windows.Forms.Padding(8);
            this.tabPosting.Size = new System.Drawing.Size(1352, 627);
            this.tabPosting.TabIndex = 4;
            this.tabPosting.Text = "Posting Rules";
            this.tabPosting.UseVisualStyleBackColor = true;
            // 
            // groupPosting
            // 
            this.groupPosting.Controls.Add(this.numApprovalThreshold);
            this.groupPosting.Controls.Add(this.numBackdatingDays);
            this.groupPosting.Controls.Add(this.numBudgetWarningPct);
            this.groupPosting.Controls.Add(this.chkRequireNarration);
            this.groupPosting.Controls.Add(this.chkAllowLockedPeriodPosting);
            this.groupPosting.Controls.Add(this.chkAutoPostPurchases);
            this.groupPosting.Controls.Add(this.chkAutoPostSales);
            this.groupPosting.Controls.Add(this.lblApprovalThreshold);
            this.groupPosting.Controls.Add(this.lblBackdatingDays);
            this.groupPosting.Controls.Add(this.lblBudgetWarning);
            this.groupPosting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPosting.Location = new System.Drawing.Point(8, 8);
            this.groupPosting.Name = "groupPosting";
            this.groupPosting.Size = new System.Drawing.Size(1336, 611);
            this.groupPosting.TabIndex = 0;
            this.groupPosting.TabStop = false;
            this.groupPosting.Text = "Posting / Control Rules";
            // 
            // numApprovalThreshold
            // 
            this.numApprovalThreshold.DecimalPlaces = 2;
            this.numApprovalThreshold.Location = new System.Drawing.Point(260, 236);
            this.numApprovalThreshold.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numApprovalThreshold.Name = "numApprovalThreshold";
            this.numApprovalThreshold.Size = new System.Drawing.Size(120, 29);
            this.numApprovalThreshold.TabIndex = 0;
            // 
            // numBackdatingDays
            // 
            this.numBackdatingDays.Location = new System.Drawing.Point(260, 200);
            this.numBackdatingDays.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numBackdatingDays.Name = "numBackdatingDays";
            this.numBackdatingDays.Size = new System.Drawing.Size(120, 29);
            this.numBackdatingDays.TabIndex = 1;
            // 
            // numBudgetWarningPct
            // 
            this.numBudgetWarningPct.Location = new System.Drawing.Point(260, 164);
            this.numBudgetWarningPct.Name = "numBudgetWarningPct";
            this.numBudgetWarningPct.Size = new System.Drawing.Size(120, 29);
            this.numBudgetWarningPct.TabIndex = 2;
            // 
            // chkRequireNarration
            // 
            this.chkRequireNarration.AutoSize = true;
            this.chkRequireNarration.Location = new System.Drawing.Point(30, 122);
            this.chkRequireNarration.Name = "chkRequireNarration";
            this.chkRequireNarration.Size = new System.Drawing.Size(304, 27);
            this.chkRequireNarration.TabIndex = 3;
            this.chkRequireNarration.Text = "Require narration on journal entries";
            // 
            // chkAllowLockedPeriodPosting
            // 
            this.chkAllowLockedPeriodPosting.AutoSize = true;
            this.chkAllowLockedPeriodPosting.Location = new System.Drawing.Point(30, 92);
            this.chkAllowLockedPeriodPosting.Name = "chkAllowLockedPeriodPosting";
            this.chkAllowLockedPeriodPosting.Size = new System.Drawing.Size(336, 27);
            this.chkAllowLockedPeriodPosting.TabIndex = 4;
            this.chkAllowLockedPeriodPosting.Text = "Allow posting to locked periods (Admin)";
            // 
            // chkAutoPostPurchases
            // 
            this.chkAutoPostPurchases.AutoSize = true;
            this.chkAutoPostPurchases.Location = new System.Drawing.Point(30, 62);
            this.chkAutoPostPurchases.Name = "chkAutoPostPurchases";
            this.chkAutoPostPurchases.Size = new System.Drawing.Size(217, 27);
            this.chkAutoPostPurchases.TabIndex = 5;
            this.chkAutoPostPurchases.Text = "Auto-post purchase bills";
            // 
            // chkAutoPostSales
            // 
            this.chkAutoPostSales.AutoSize = true;
            this.chkAutoPostSales.Location = new System.Drawing.Point(30, 32);
            this.chkAutoPostSales.Name = "chkAutoPostSales";
            this.chkAutoPostSales.Size = new System.Drawing.Size(215, 27);
            this.chkAutoPostSales.TabIndex = 6;
            this.chkAutoPostSales.Text = "Auto-post sales invoices";
            // 
            // lblApprovalThreshold
            // 
            this.lblApprovalThreshold.AutoSize = true;
            this.lblApprovalThreshold.Location = new System.Drawing.Point(30, 239);
            this.lblApprovalThreshold.Name = "lblApprovalThreshold";
            this.lblApprovalThreshold.Size = new System.Drawing.Size(220, 23);
            this.lblApprovalThreshold.TabIndex = 7;
            this.lblApprovalThreshold.Text = "Approval threshold amount";
            // 
            // lblBackdatingDays
            // 
            this.lblBackdatingDays.AutoSize = true;
            this.lblBackdatingDays.Location = new System.Drawing.Point(30, 203);
            this.lblBackdatingDays.Name = "lblBackdatingDays";
            this.lblBackdatingDays.Size = new System.Drawing.Size(170, 23);
            this.lblBackdatingDays.TabIndex = 8;
            this.lblBackdatingDays.Text = "Max backdating days";
            // 
            // lblBudgetWarning
            // 
            this.lblBudgetWarning.AutoSize = true;
            this.lblBudgetWarning.Location = new System.Drawing.Point(30, 167);
            this.lblBudgetWarning.Name = "lblBudgetWarning";
            this.lblBudgetWarning.Size = new System.Drawing.Size(227, 23);
            this.lblBudgetWarning.TabIndex = 9;
            this.lblBudgetWarning.Text = "Budget warning threshold %";
            // 
            // tabReports
            // 
            this.tabReports.Controls.Add(this.groupReports);
            this.tabReports.Location = new System.Drawing.Point(4, 30);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(8);
            this.tabReports.Size = new System.Drawing.Size(1352, 627);
            this.tabReports.TabIndex = 5;
            this.tabReports.Text = "Report Settings";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // groupReports
            // 
            this.groupReports.Controls.Add(this.txtDigitalSignature);
            this.groupReports.Controls.Add(this.txtReportFooter);
            this.groupReports.Controls.Add(this.txtReportHeader);
            this.groupReports.Controls.Add(this.cmbShowAmountsIn);
            this.groupReports.Controls.Add(this.cmbReportDateFormat);
            this.groupReports.Controls.Add(this.cmbAmountFormat);
            this.groupReports.Controls.Add(this.lblDigitalSignature);
            this.groupReports.Controls.Add(this.lblReportFooter);
            this.groupReports.Controls.Add(this.lblReportHeader);
            this.groupReports.Controls.Add(this.lblShowAmountsIn);
            this.groupReports.Controls.Add(this.lblDateFormat);
            this.groupReports.Controls.Add(this.lblAmountFormat);
            this.groupReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupReports.Location = new System.Drawing.Point(8, 8);
            this.groupReports.Name = "groupReports";
            this.groupReports.Size = new System.Drawing.Size(1336, 611);
            this.groupReports.TabIndex = 0;
            this.groupReports.TabStop = false;
            this.groupReports.Text = "Report Presentation";
            // 
            // txtDigitalSignature
            // 
            this.txtDigitalSignature.Location = new System.Drawing.Point(257, 259);
            this.txtDigitalSignature.Name = "txtDigitalSignature";
            this.txtDigitalSignature.Size = new System.Drawing.Size(650, 29);
            this.txtDigitalSignature.TabIndex = 0;
            // 
            // txtReportFooter
            // 
            this.txtReportFooter.Location = new System.Drawing.Point(257, 177);
            this.txtReportFooter.Multiline = true;
            this.txtReportFooter.Name = "txtReportFooter";
            this.txtReportFooter.Size = new System.Drawing.Size(650, 70);
            this.txtReportFooter.TabIndex = 1;
            // 
            // txtReportHeader
            // 
            this.txtReportHeader.Location = new System.Drawing.Point(257, 141);
            this.txtReportHeader.Name = "txtReportHeader";
            this.txtReportHeader.Size = new System.Drawing.Size(650, 29);
            this.txtReportHeader.TabIndex = 2;
            // 
            // cmbShowAmountsIn
            // 
            this.cmbShowAmountsIn.Location = new System.Drawing.Point(257, 105);
            this.cmbShowAmountsIn.Name = "cmbShowAmountsIn";
            this.cmbShowAmountsIn.Size = new System.Drawing.Size(250, 29);
            this.cmbShowAmountsIn.TabIndex = 3;
            // 
            // cmbReportDateFormat
            // 
            this.cmbReportDateFormat.Location = new System.Drawing.Point(257, 69);
            this.cmbReportDateFormat.Name = "cmbReportDateFormat";
            this.cmbReportDateFormat.Size = new System.Drawing.Size(250, 29);
            this.cmbReportDateFormat.TabIndex = 4;
            // 
            // cmbAmountFormat
            // 
            this.cmbAmountFormat.Location = new System.Drawing.Point(257, 33);
            this.cmbAmountFormat.Name = "cmbAmountFormat";
            this.cmbAmountFormat.Size = new System.Drawing.Size(250, 29);
            this.cmbAmountFormat.TabIndex = 5;
            // 
            // lblDigitalSignature
            // 
            this.lblDigitalSignature.AutoSize = true;
            this.lblDigitalSignature.Location = new System.Drawing.Point(25, 262);
            this.lblDigitalSignature.Name = "lblDigitalSignature";
            this.lblDigitalSignature.Size = new System.Drawing.Size(229, 23);
            this.lblDigitalSignature.TabIndex = 6;
            this.lblDigitalSignature.Text = "Digital signature placeholder";
            // 
            // lblReportFooter
            // 
            this.lblReportFooter.AutoSize = true;
            this.lblReportFooter.Location = new System.Drawing.Point(25, 180);
            this.lblReportFooter.Name = "lblReportFooter";
            this.lblReportFooter.Size = new System.Drawing.Size(112, 23);
            this.lblReportFooter.TabIndex = 7;
            this.lblReportFooter.Text = "Report footer";
            // 
            // lblReportHeader
            // 
            this.lblReportHeader.AutoSize = true;
            this.lblReportHeader.Location = new System.Drawing.Point(25, 144);
            this.lblReportHeader.Name = "lblReportHeader";
            this.lblReportHeader.Size = new System.Drawing.Size(119, 23);
            this.lblReportHeader.TabIndex = 8;
            this.lblReportHeader.Text = "Report header";
            // 
            // lblShowAmountsIn
            // 
            this.lblShowAmountsIn.AutoSize = true;
            this.lblShowAmountsIn.Location = new System.Drawing.Point(25, 108);
            this.lblShowAmountsIn.Name = "lblShowAmountsIn";
            this.lblShowAmountsIn.Size = new System.Drawing.Size(142, 23);
            this.lblShowAmountsIn.TabIndex = 9;
            this.lblShowAmountsIn.Text = "Show amounts in";
            // 
            // lblDateFormat
            // 
            this.lblDateFormat.AutoSize = true;
            this.lblDateFormat.Location = new System.Drawing.Point(25, 72);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.Size = new System.Drawing.Size(102, 23);
            this.lblDateFormat.TabIndex = 10;
            this.lblDateFormat.Text = "Date format";
            // 
            // lblAmountFormat
            // 
            this.lblAmountFormat.AutoSize = true;
            this.lblAmountFormat.Location = new System.Drawing.Point(25, 36);
            this.lblAmountFormat.Name = "lblAmountFormat";
            this.lblAmountFormat.Size = new System.Drawing.Size(128, 23);
            this.lblAmountFormat.TabIndex = 11;
            this.lblAmountFormat.Text = "Amount format";
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.btnResetDefaults);
            this.panelBottom.Controls.Add(this.btnSaveSettings);
            this.panelBottom.Location = new System.Drawing.Point(12, 707);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1360, 45);
            this.panelBottom.TabIndex = 2;
            // 
            // btnResetDefaults
            // 
            this.btnResetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetDefaults.Location = new System.Drawing.Point(1126, 7);
            this.btnResetDefaults.Name = "btnResetDefaults";
            this.btnResetDefaults.Size = new System.Drawing.Size(110, 33);
            this.btnResetDefaults.TabIndex = 1;
            this.btnResetDefaults.Text = "Reset Defaults";
            this.btnResetDefaults.UseVisualStyleBackColor = true;
            this.btnResetDefaults.Click += new System.EventHandler(this.btnResetDefaults_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Location = new System.Drawing.Point(1242, 7);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(110, 33);
            this.btnSaveSettings.TabIndex = 0;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // frm_accounting_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 761);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Name = "frm_accounting_settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Accounting Settings";
            this.Load += new System.EventHandler(this.frm_accounting_settings_Load);
            this.tabMain.ResumeLayout(false);
            this.tabCompany.ResumeLayout(false);
            this.groupCompanyHeader.ResumeLayout(false);
            this.groupCompanyHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoPreview)).EndInit();
            this.tabDefaults.ResumeLayout(false);
            this.tblDefaults.ResumeLayout(false);
            this.tabVoucher.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridVoucher)).EndInit();
            this.tabTax.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridWhtRates)).EndInit();
            this.groupTaxTop.ResumeLayout(false);
            this.groupTaxTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTaxRate)).EndInit();
            this.tabPosting.ResumeLayout(false);
            this.groupPosting.ResumeLayout(false);
            this.groupPosting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numApprovalThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBackdatingDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBudgetWarningPct)).EndInit();
            this.tabReports.ResumeLayout(false);
            this.groupReports.ResumeLayout(false);
            this.groupReports.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabCompany;
        private System.Windows.Forms.TabPage tabDefaults;
        private System.Windows.Forms.TabPage tabVoucher;
        private System.Windows.Forms.TabPage tabTax;
        private System.Windows.Forms.TabPage tabPosting;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.GroupBox groupCompanyHeader;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblLegalName;
        private System.Windows.Forms.Label lblRegistration;
        private System.Windows.Forms.Label lblNtnVat;
        private System.Windows.Forms.Label lblStrn;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.Label lblFyStart;
        private System.Windows.Forms.Label lblFyEnd;
        private System.Windows.Forms.Label lblBaseCurrency;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtLegalName;
        private System.Windows.Forms.TextBox txtRegistrationNo;
        private System.Windows.Forms.TextBox txtNtnVat;
        private System.Windows.Forms.TextBox txtStrn;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtWebsite;
        private System.Windows.Forms.ComboBox cmbFyStartMonth;
        private System.Windows.Forms.ComboBox cmbFyEndMonth;
        private System.Windows.Forms.ComboBox cmbBaseCurrency;
        private System.Windows.Forms.ComboBox cmbCountry;
        private System.Windows.Forms.PictureBox picLogoPreview;
        private System.Windows.Forms.Button btnBrowseLogo;
        private System.Windows.Forms.TextBox txtLogoPath;
        private System.Windows.Forms.Label lblCurrencyLockNote;

        private System.Windows.Forms.Button btnTestAutoPostingRules;
        private System.Windows.Forms.TableLayoutPanel tblDefaults;
        private System.Windows.Forms.Label lblSalesAr;
        private System.Windows.Forms.ComboBox cmbSalesAr;
        private System.Windows.Forms.Label lblSalesRevenue;
        private System.Windows.Forms.ComboBox cmbSalesRevenue;
        private System.Windows.Forms.Label lblSalesTaxOutput;
        private System.Windows.Forms.ComboBox cmbSalesTaxOutput;
        private System.Windows.Forms.Label lblPurchaseAp;
        private System.Windows.Forms.ComboBox cmbPurchaseAp;
        private System.Windows.Forms.Label lblPurchaseCogs;
        private System.Windows.Forms.ComboBox cmbPurchaseCogs;
        private System.Windows.Forms.Label lblPurchaseTaxInput;
        private System.Windows.Forms.ComboBox cmbPurchaseTaxInput;
        private System.Windows.Forms.Label lblDefaultExpense;
        private System.Windows.Forms.ComboBox cmbDefaultExpense;
        private System.Windows.Forms.Label lblDefaultCash;
        private System.Windows.Forms.ComboBox cmbDefaultCash;
        private System.Windows.Forms.Label lblDefaultBank;
        private System.Windows.Forms.ComboBox cmbDefaultBank;
        private System.Windows.Forms.Label lblSalaryExpense;
        private System.Windows.Forms.ComboBox cmbSalaryExpense;
        private System.Windows.Forms.Label lblSalaryPayable;
        private System.Windows.Forms.ComboBox cmbSalaryPayable;
        private System.Windows.Forms.Label lblInventoryAsset;
        private System.Windows.Forms.ComboBox cmbInventoryAsset;
        private System.Windows.Forms.Label lblInventoryCogs;
        private System.Windows.Forms.ComboBox cmbInventoryCogs;
        private System.Windows.Forms.Label lblInventoryAdjustment;
        private System.Windows.Forms.ComboBox cmbInventoryAdjustment;
        private System.Windows.Forms.Label lblFaAsset;
        private System.Windows.Forms.ComboBox cmbFaAsset;
        private System.Windows.Forms.Label lblFaAccumDep;
        private System.Windows.Forms.ComboBox cmbFaAccumDep;
        private System.Windows.Forms.Label lblFaDepExpense;
        private System.Windows.Forms.ComboBox cmbFaDepExpense;
        private System.Windows.Forms.Label lblInterBranchRec;
        private System.Windows.Forms.ComboBox cmbInterBranchRec;
        private System.Windows.Forms.Label lblInterBranchPay;
        private System.Windows.Forms.ComboBox cmbInterBranchPay;
        private System.Windows.Forms.Label lblOpeningEquity;
        private System.Windows.Forms.ComboBox cmbOpeningEquity;

        private System.Windows.Forms.DataGridView gridVoucher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoucherType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoucherPrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoucherBranchId;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVoucherFormat;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVoucherReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoucherStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoucherPreview;

        private System.Windows.Forms.GroupBox groupTaxTop;
        private System.Windows.Forms.NumericUpDown numSalesTaxRate;
        private System.Windows.Forms.ComboBox cmbTaxMode;
        private System.Windows.Forms.TextBox txtFbrNtn;
        private System.Windows.Forms.TextBox txtFbrStrn;
        private System.Windows.Forms.ComboBox cmbFilingFrequency;
        private System.Windows.Forms.Label lblSalesTaxRate;
        private System.Windows.Forms.Label lblTaxMode;
        private System.Windows.Forms.Label lblFbrNtn;
        private System.Windows.Forms.Label lblFbrStrn;
        private System.Windows.Forms.Label lblFilingFrequency;
        private System.Windows.Forms.DataGridView gridWhtRates;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhtId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhtType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaxSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhtDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhtRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffectiveFrom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsActive;

        private System.Windows.Forms.GroupBox groupPosting;
        private System.Windows.Forms.CheckBox chkAutoPostSales;
        private System.Windows.Forms.CheckBox chkAutoPostPurchases;
        private System.Windows.Forms.CheckBox chkAllowLockedPeriodPosting;
        private System.Windows.Forms.CheckBox chkRequireNarration;
        private System.Windows.Forms.NumericUpDown numBudgetWarningPct;
        private System.Windows.Forms.NumericUpDown numBackdatingDays;
        private System.Windows.Forms.NumericUpDown numApprovalThreshold;
        private System.Windows.Forms.Label lblBudgetWarning;
        private System.Windows.Forms.Label lblBackdatingDays;
        private System.Windows.Forms.Label lblApprovalThreshold;

        private System.Windows.Forms.GroupBox groupReports;
        private System.Windows.Forms.ComboBox cmbAmountFormat;
        private System.Windows.Forms.ComboBox cmbReportDateFormat;
        private System.Windows.Forms.ComboBox cmbShowAmountsIn;
        private System.Windows.Forms.TextBox txtReportHeader;
        private System.Windows.Forms.TextBox txtReportFooter;
        private System.Windows.Forms.TextBox txtDigitalSignature;
        private System.Windows.Forms.Label lblAmountFormat;
        private System.Windows.Forms.Label lblDateFormat;
        private System.Windows.Forms.Label lblShowAmountsIn;
        private System.Windows.Forms.Label lblReportHeader;
        private System.Windows.Forms.Label lblReportFooter;
        private System.Windows.Forms.Label lblDigitalSignature;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnResetDefaults;
    }
}
