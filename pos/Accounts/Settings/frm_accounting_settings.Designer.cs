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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_accounting_settings));
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabCompany = new System.Windows.Forms.TabPage();
            this.groupCompanyHeader = new System.Windows.Forms.GroupBox();
            this.chk_use_zatca_e_invoice = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_buildingNumber = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txt_countryName = new System.Windows.Forms.TextBox();
            this.txt_postalCode = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txt_cityName = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txt_citySubdivisionName = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txt_StreetName = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
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
            this.groupBox3.SuspendLayout();
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
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // tabMain
            // 
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.Controls.Add(this.tabCompany);
            this.tabMain.Controls.Add(this.tabDefaults);
            this.tabMain.Controls.Add(this.tabVoucher);
            this.tabMain.Controls.Add(this.tabTax);
            this.tabMain.Controls.Add(this.tabPosting);
            this.tabMain.Controls.Add(this.tabReports);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            // 
            // tabCompany
            // 
            resources.ApplyResources(this.tabCompany, "tabCompany");
            this.tabCompany.Controls.Add(this.groupCompanyHeader);
            this.tabCompany.Name = "tabCompany";
            this.tabCompany.UseVisualStyleBackColor = true;
            // 
            // groupCompanyHeader
            // 
            resources.ApplyResources(this.groupCompanyHeader, "groupCompanyHeader");
            this.groupCompanyHeader.Controls.Add(this.chk_use_zatca_e_invoice);
            this.groupCompanyHeader.Controls.Add(this.groupBox3);
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
            this.groupCompanyHeader.Name = "groupCompanyHeader";
            this.groupCompanyHeader.TabStop = false;
            // 
            // chk_use_zatca_e_invoice
            // 
            resources.ApplyResources(this.chk_use_zatca_e_invoice, "chk_use_zatca_e_invoice");
            this.chk_use_zatca_e_invoice.Name = "chk_use_zatca_e_invoice";
            this.chk_use_zatca_e_invoice.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.txt_buildingNumber);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.txt_countryName);
            this.groupBox3.Controls.Add(this.txt_postalCode);
            this.groupBox3.Controls.Add(this.label24);
            this.groupBox3.Controls.Add(this.txt_cityName);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.txt_citySubdivisionName);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.txt_StreetName);
            this.groupBox3.Controls.Add(this.label27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // txt_buildingNumber
            // 
            resources.ApplyResources(this.txt_buildingNumber, "txt_buildingNumber");
            this.txt_buildingNumber.Name = "txt_buildingNumber";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // txt_countryName
            // 
            resources.ApplyResources(this.txt_countryName, "txt_countryName");
            this.txt_countryName.Name = "txt_countryName";
            // 
            // txt_postalCode
            // 
            resources.ApplyResources(this.txt_postalCode, "txt_postalCode");
            this.txt_postalCode.Name = "txt_postalCode";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // txt_cityName
            // 
            resources.ApplyResources(this.txt_cityName, "txt_cityName");
            this.txt_cityName.Name = "txt_cityName";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // txt_citySubdivisionName
            // 
            resources.ApplyResources(this.txt_citySubdivisionName, "txt_citySubdivisionName");
            this.txt_citySubdivisionName.Name = "txt_citySubdivisionName";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // txt_StreetName
            // 
            resources.ApplyResources(this.txt_StreetName, "txt_StreetName");
            this.txt_StreetName.Name = "txt_StreetName";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // lblCurrencyLockNote
            // 
            resources.ApplyResources(this.lblCurrencyLockNote, "lblCurrencyLockNote");
            this.lblCurrencyLockNote.ForeColor = System.Drawing.Color.Firebrick;
            this.lblCurrencyLockNote.Name = "lblCurrencyLockNote";
            // 
            // txtLogoPath
            // 
            resources.ApplyResources(this.txtLogoPath, "txtLogoPath");
            this.txtLogoPath.Name = "txtLogoPath";
            // 
            // btnBrowseLogo
            // 
            resources.ApplyResources(this.btnBrowseLogo, "btnBrowseLogo");
            this.btnBrowseLogo.Name = "btnBrowseLogo";
            this.btnBrowseLogo.UseVisualStyleBackColor = true;
            this.btnBrowseLogo.Click += new System.EventHandler(this.btnBrowseLogo_Click);
            // 
            // picLogoPreview
            // 
            resources.ApplyResources(this.picLogoPreview, "picLogoPreview");
            this.picLogoPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogoPreview.Name = "picLogoPreview";
            this.picLogoPreview.TabStop = false;
            // 
            // cmbCountry
            // 
            resources.ApplyResources(this.cmbCountry, "cmbCountry");
            this.cmbCountry.FormattingEnabled = true;
            this.cmbCountry.Name = "cmbCountry";
            // 
            // cmbBaseCurrency
            // 
            resources.ApplyResources(this.cmbBaseCurrency, "cmbBaseCurrency");
            this.cmbBaseCurrency.FormattingEnabled = true;
            this.cmbBaseCurrency.Name = "cmbBaseCurrency";
            // 
            // cmbFyEndMonth
            // 
            resources.ApplyResources(this.cmbFyEndMonth, "cmbFyEndMonth");
            this.cmbFyEndMonth.FormattingEnabled = true;
            this.cmbFyEndMonth.Name = "cmbFyEndMonth";
            // 
            // cmbFyStartMonth
            // 
            resources.ApplyResources(this.cmbFyStartMonth, "cmbFyStartMonth");
            this.cmbFyStartMonth.FormattingEnabled = true;
            this.cmbFyStartMonth.Name = "cmbFyStartMonth";
            this.cmbFyStartMonth.SelectedIndexChanged += new System.EventHandler(this.cmbFyStartMonth_SelectedIndexChanged);
            // 
            // txtWebsite
            // 
            resources.ApplyResources(this.txtWebsite, "txtWebsite");
            this.txtWebsite.Name = "txtWebsite";
            // 
            // txtEmail
            // 
            resources.ApplyResources(this.txtEmail, "txtEmail");
            this.txtEmail.Name = "txtEmail";
            // 
            // txtPhone
            // 
            resources.ApplyResources(this.txtPhone, "txtPhone");
            this.txtPhone.Name = "txtPhone";
            // 
            // txtAddress
            // 
            resources.ApplyResources(this.txtAddress, "txtAddress");
            this.txtAddress.Name = "txtAddress";
            // 
            // txtStrn
            // 
            resources.ApplyResources(this.txtStrn, "txtStrn");
            this.txtStrn.Name = "txtStrn";
            // 
            // txtNtnVat
            // 
            resources.ApplyResources(this.txtNtnVat, "txtNtnVat");
            this.txtNtnVat.Name = "txtNtnVat";
            // 
            // txtRegistrationNo
            // 
            resources.ApplyResources(this.txtRegistrationNo, "txtRegistrationNo");
            this.txtRegistrationNo.Name = "txtRegistrationNo";
            // 
            // txtLegalName
            // 
            resources.ApplyResources(this.txtLegalName, "txtLegalName");
            this.txtLegalName.Name = "txtLegalName";
            // 
            // txtCompanyName
            // 
            resources.ApplyResources(this.txtCompanyName, "txtCompanyName");
            this.txtCompanyName.Name = "txtCompanyName";
            // 
            // lblCountry
            // 
            resources.ApplyResources(this.lblCountry, "lblCountry");
            this.lblCountry.Name = "lblCountry";
            // 
            // lblBaseCurrency
            // 
            resources.ApplyResources(this.lblBaseCurrency, "lblBaseCurrency");
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            // 
            // lblFyEnd
            // 
            resources.ApplyResources(this.lblFyEnd, "lblFyEnd");
            this.lblFyEnd.Name = "lblFyEnd";
            // 
            // lblFyStart
            // 
            resources.ApplyResources(this.lblFyStart, "lblFyStart");
            this.lblFyStart.Name = "lblFyStart";
            // 
            // lblWebsite
            // 
            resources.ApplyResources(this.lblWebsite, "lblWebsite");
            this.lblWebsite.Name = "lblWebsite";
            // 
            // lblEmail
            // 
            resources.ApplyResources(this.lblEmail, "lblEmail");
            this.lblEmail.Name = "lblEmail";
            // 
            // lblPhone
            // 
            resources.ApplyResources(this.lblPhone, "lblPhone");
            this.lblPhone.Name = "lblPhone";
            // 
            // lblAddress
            // 
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.Name = "lblAddress";
            // 
            // lblStrn
            // 
            resources.ApplyResources(this.lblStrn, "lblStrn");
            this.lblStrn.Name = "lblStrn";
            // 
            // lblNtnVat
            // 
            resources.ApplyResources(this.lblNtnVat, "lblNtnVat");
            this.lblNtnVat.Name = "lblNtnVat";
            // 
            // lblRegistration
            // 
            resources.ApplyResources(this.lblRegistration, "lblRegistration");
            this.lblRegistration.Name = "lblRegistration";
            // 
            // lblLegalName
            // 
            resources.ApplyResources(this.lblLegalName, "lblLegalName");
            this.lblLegalName.Name = "lblLegalName";
            // 
            // lblCompanyName
            // 
            resources.ApplyResources(this.lblCompanyName, "lblCompanyName");
            this.lblCompanyName.Name = "lblCompanyName";
            // 
            // tabDefaults
            // 
            resources.ApplyResources(this.tabDefaults, "tabDefaults");
            this.tabDefaults.Controls.Add(this.btnTestAutoPostingRules);
            this.tabDefaults.Controls.Add(this.tblDefaults);
            this.tabDefaults.Name = "tabDefaults";
            this.tabDefaults.UseVisualStyleBackColor = true;
            // 
            // btnTestAutoPostingRules
            // 
            resources.ApplyResources(this.btnTestAutoPostingRules, "btnTestAutoPostingRules");
            this.btnTestAutoPostingRules.Name = "btnTestAutoPostingRules";
            this.btnTestAutoPostingRules.UseVisualStyleBackColor = true;
            this.btnTestAutoPostingRules.Click += new System.EventHandler(this.btnTestAutoPostingRules_Click);
            // 
            // tblDefaults
            // 
            resources.ApplyResources(this.tblDefaults, "tblDefaults");
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
            this.tblDefaults.Name = "tblDefaults";
            // 
            // lblSalesAr
            // 
            resources.ApplyResources(this.lblSalesAr, "lblSalesAr");
            this.lblSalesAr.Name = "lblSalesAr";
            // 
            // cmbSalesAr
            // 
            resources.ApplyResources(this.cmbSalesAr, "cmbSalesAr");
            this.cmbSalesAr.FormattingEnabled = true;
            this.cmbSalesAr.Name = "cmbSalesAr";
            // 
            // lblSalesRevenue
            // 
            resources.ApplyResources(this.lblSalesRevenue, "lblSalesRevenue");
            this.lblSalesRevenue.Name = "lblSalesRevenue";
            // 
            // cmbSalesRevenue
            // 
            resources.ApplyResources(this.cmbSalesRevenue, "cmbSalesRevenue");
            this.cmbSalesRevenue.FormattingEnabled = true;
            this.cmbSalesRevenue.Name = "cmbSalesRevenue";
            // 
            // lblSalesTaxOutput
            // 
            resources.ApplyResources(this.lblSalesTaxOutput, "lblSalesTaxOutput");
            this.lblSalesTaxOutput.Name = "lblSalesTaxOutput";
            // 
            // cmbSalesTaxOutput
            // 
            resources.ApplyResources(this.cmbSalesTaxOutput, "cmbSalesTaxOutput");
            this.cmbSalesTaxOutput.FormattingEnabled = true;
            this.cmbSalesTaxOutput.Name = "cmbSalesTaxOutput";
            // 
            // lblPurchaseAp
            // 
            resources.ApplyResources(this.lblPurchaseAp, "lblPurchaseAp");
            this.lblPurchaseAp.Name = "lblPurchaseAp";
            // 
            // cmbPurchaseAp
            // 
            resources.ApplyResources(this.cmbPurchaseAp, "cmbPurchaseAp");
            this.cmbPurchaseAp.FormattingEnabled = true;
            this.cmbPurchaseAp.Name = "cmbPurchaseAp";
            // 
            // lblPurchaseCogs
            // 
            resources.ApplyResources(this.lblPurchaseCogs, "lblPurchaseCogs");
            this.lblPurchaseCogs.Name = "lblPurchaseCogs";
            // 
            // cmbPurchaseCogs
            // 
            resources.ApplyResources(this.cmbPurchaseCogs, "cmbPurchaseCogs");
            this.cmbPurchaseCogs.FormattingEnabled = true;
            this.cmbPurchaseCogs.Name = "cmbPurchaseCogs";
            // 
            // lblPurchaseTaxInput
            // 
            resources.ApplyResources(this.lblPurchaseTaxInput, "lblPurchaseTaxInput");
            this.lblPurchaseTaxInput.Name = "lblPurchaseTaxInput";
            // 
            // cmbPurchaseTaxInput
            // 
            resources.ApplyResources(this.cmbPurchaseTaxInput, "cmbPurchaseTaxInput");
            this.cmbPurchaseTaxInput.FormattingEnabled = true;
            this.cmbPurchaseTaxInput.Name = "cmbPurchaseTaxInput";
            // 
            // lblDefaultExpense
            // 
            resources.ApplyResources(this.lblDefaultExpense, "lblDefaultExpense");
            this.lblDefaultExpense.Name = "lblDefaultExpense";
            // 
            // cmbDefaultExpense
            // 
            resources.ApplyResources(this.cmbDefaultExpense, "cmbDefaultExpense");
            this.cmbDefaultExpense.FormattingEnabled = true;
            this.cmbDefaultExpense.Name = "cmbDefaultExpense";
            // 
            // lblDefaultCash
            // 
            resources.ApplyResources(this.lblDefaultCash, "lblDefaultCash");
            this.lblDefaultCash.Name = "lblDefaultCash";
            // 
            // cmbDefaultCash
            // 
            resources.ApplyResources(this.cmbDefaultCash, "cmbDefaultCash");
            this.cmbDefaultCash.FormattingEnabled = true;
            this.cmbDefaultCash.Name = "cmbDefaultCash";
            // 
            // lblDefaultBank
            // 
            resources.ApplyResources(this.lblDefaultBank, "lblDefaultBank");
            this.lblDefaultBank.Name = "lblDefaultBank";
            // 
            // cmbDefaultBank
            // 
            resources.ApplyResources(this.cmbDefaultBank, "cmbDefaultBank");
            this.cmbDefaultBank.FormattingEnabled = true;
            this.cmbDefaultBank.Name = "cmbDefaultBank";
            // 
            // lblSalaryExpense
            // 
            resources.ApplyResources(this.lblSalaryExpense, "lblSalaryExpense");
            this.lblSalaryExpense.Name = "lblSalaryExpense";
            // 
            // cmbSalaryExpense
            // 
            resources.ApplyResources(this.cmbSalaryExpense, "cmbSalaryExpense");
            this.cmbSalaryExpense.FormattingEnabled = true;
            this.cmbSalaryExpense.Name = "cmbSalaryExpense";
            // 
            // lblSalaryPayable
            // 
            resources.ApplyResources(this.lblSalaryPayable, "lblSalaryPayable");
            this.lblSalaryPayable.Name = "lblSalaryPayable";
            // 
            // cmbSalaryPayable
            // 
            resources.ApplyResources(this.cmbSalaryPayable, "cmbSalaryPayable");
            this.cmbSalaryPayable.FormattingEnabled = true;
            this.cmbSalaryPayable.Name = "cmbSalaryPayable";
            // 
            // lblInventoryAsset
            // 
            resources.ApplyResources(this.lblInventoryAsset, "lblInventoryAsset");
            this.lblInventoryAsset.Name = "lblInventoryAsset";
            // 
            // cmbInventoryAsset
            // 
            resources.ApplyResources(this.cmbInventoryAsset, "cmbInventoryAsset");
            this.cmbInventoryAsset.FormattingEnabled = true;
            this.cmbInventoryAsset.Name = "cmbInventoryAsset";
            // 
            // lblInventoryCogs
            // 
            resources.ApplyResources(this.lblInventoryCogs, "lblInventoryCogs");
            this.lblInventoryCogs.Name = "lblInventoryCogs";
            // 
            // cmbInventoryCogs
            // 
            resources.ApplyResources(this.cmbInventoryCogs, "cmbInventoryCogs");
            this.cmbInventoryCogs.FormattingEnabled = true;
            this.cmbInventoryCogs.Name = "cmbInventoryCogs";
            // 
            // lblInventoryAdjustment
            // 
            resources.ApplyResources(this.lblInventoryAdjustment, "lblInventoryAdjustment");
            this.lblInventoryAdjustment.Name = "lblInventoryAdjustment";
            // 
            // cmbInventoryAdjustment
            // 
            resources.ApplyResources(this.cmbInventoryAdjustment, "cmbInventoryAdjustment");
            this.cmbInventoryAdjustment.FormattingEnabled = true;
            this.cmbInventoryAdjustment.Name = "cmbInventoryAdjustment";
            // 
            // lblFaAsset
            // 
            resources.ApplyResources(this.lblFaAsset, "lblFaAsset");
            this.lblFaAsset.Name = "lblFaAsset";
            // 
            // cmbFaAsset
            // 
            resources.ApplyResources(this.cmbFaAsset, "cmbFaAsset");
            this.cmbFaAsset.FormattingEnabled = true;
            this.cmbFaAsset.Name = "cmbFaAsset";
            // 
            // lblFaAccumDep
            // 
            resources.ApplyResources(this.lblFaAccumDep, "lblFaAccumDep");
            this.lblFaAccumDep.Name = "lblFaAccumDep";
            // 
            // cmbFaAccumDep
            // 
            resources.ApplyResources(this.cmbFaAccumDep, "cmbFaAccumDep");
            this.cmbFaAccumDep.FormattingEnabled = true;
            this.cmbFaAccumDep.Name = "cmbFaAccumDep";
            // 
            // lblFaDepExpense
            // 
            resources.ApplyResources(this.lblFaDepExpense, "lblFaDepExpense");
            this.lblFaDepExpense.Name = "lblFaDepExpense";
            // 
            // cmbFaDepExpense
            // 
            resources.ApplyResources(this.cmbFaDepExpense, "cmbFaDepExpense");
            this.cmbFaDepExpense.FormattingEnabled = true;
            this.cmbFaDepExpense.Name = "cmbFaDepExpense";
            // 
            // lblInterBranchRec
            // 
            resources.ApplyResources(this.lblInterBranchRec, "lblInterBranchRec");
            this.lblInterBranchRec.Name = "lblInterBranchRec";
            // 
            // cmbInterBranchRec
            // 
            resources.ApplyResources(this.cmbInterBranchRec, "cmbInterBranchRec");
            this.cmbInterBranchRec.FormattingEnabled = true;
            this.cmbInterBranchRec.Name = "cmbInterBranchRec";
            // 
            // lblInterBranchPay
            // 
            resources.ApplyResources(this.lblInterBranchPay, "lblInterBranchPay");
            this.lblInterBranchPay.Name = "lblInterBranchPay";
            // 
            // cmbInterBranchPay
            // 
            resources.ApplyResources(this.cmbInterBranchPay, "cmbInterBranchPay");
            this.cmbInterBranchPay.FormattingEnabled = true;
            this.cmbInterBranchPay.Name = "cmbInterBranchPay";
            // 
            // lblOpeningEquity
            // 
            resources.ApplyResources(this.lblOpeningEquity, "lblOpeningEquity");
            this.lblOpeningEquity.Name = "lblOpeningEquity";
            // 
            // cmbOpeningEquity
            // 
            resources.ApplyResources(this.cmbOpeningEquity, "cmbOpeningEquity");
            this.cmbOpeningEquity.FormattingEnabled = true;
            this.cmbOpeningEquity.Name = "cmbOpeningEquity";
            // 
            // tabVoucher
            // 
            resources.ApplyResources(this.tabVoucher, "tabVoucher");
            this.tabVoucher.Controls.Add(this.gridVoucher);
            this.tabVoucher.Name = "tabVoucher";
            this.tabVoucher.UseVisualStyleBackColor = true;
            // 
            // gridVoucher
            // 
            resources.ApplyResources(this.gridVoucher, "gridVoucher");
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
            this.gridVoucher.Name = "gridVoucher";
            this.gridVoucher.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridVoucher_CellEndEdit);
            // 
            // colVoucherType
            // 
            resources.ApplyResources(this.colVoucherType, "colVoucherType");
            this.colVoucherType.Name = "colVoucherType";
            this.colVoucherType.ReadOnly = true;
            // 
            // colVoucherPrefix
            // 
            resources.ApplyResources(this.colVoucherPrefix, "colVoucherPrefix");
            this.colVoucherPrefix.Name = "colVoucherPrefix";
            // 
            // colVoucherBranchId
            // 
            resources.ApplyResources(this.colVoucherBranchId, "colVoucherBranchId");
            this.colVoucherBranchId.Name = "colVoucherBranchId";
            this.colVoucherBranchId.ReadOnly = true;
            // 
            // colVoucherFormat
            // 
            resources.ApplyResources(this.colVoucherFormat, "colVoucherFormat");
            this.colVoucherFormat.Items.AddRange(new object[] {
            "YYYY-NNNN",
            "YY-NNNN",
            "NNNN",
            "YYYYMMDD-NNNN",
            "YYYY-MM-DD-NNNN",
            "YYYYMMDD-YYYY-NNNN",
            "YYYY-MM-DD-YYYY-NNNN"});
            this.colVoucherFormat.Name = "colVoucherFormat";
            // 
            // colVoucherReset
            // 
            resources.ApplyResources(this.colVoucherReset, "colVoucherReset");
            this.colVoucherReset.Items.AddRange(new object[] {
            "Daily",
            "Annually",
            "Never",
            "Per Financial Year"});
            this.colVoucherReset.Name = "colVoucherReset";
            // 
            // colVoucherStart
            // 
            resources.ApplyResources(this.colVoucherStart, "colVoucherStart");
            this.colVoucherStart.Name = "colVoucherStart";
            // 
            // colVoucherPreview
            // 
            resources.ApplyResources(this.colVoucherPreview, "colVoucherPreview");
            this.colVoucherPreview.Name = "colVoucherPreview";
            this.colVoucherPreview.ReadOnly = true;
            // 
            // tabTax
            // 
            resources.ApplyResources(this.tabTax, "tabTax");
            this.tabTax.Controls.Add(this.gridWhtRates);
            this.tabTax.Controls.Add(this.groupTaxTop);
            this.tabTax.Name = "tabTax";
            this.tabTax.UseVisualStyleBackColor = true;
            // 
            // gridWhtRates
            // 
            resources.ApplyResources(this.gridWhtRates, "gridWhtRates");
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
            this.gridWhtRates.Name = "gridWhtRates";
            // 
            // colWhtId
            // 
            resources.ApplyResources(this.colWhtId, "colWhtId");
            this.colWhtId.Name = "colWhtId";
            // 
            // colWhtType
            // 
            resources.ApplyResources(this.colWhtType, "colWhtType");
            this.colWhtType.Name = "colWhtType";
            // 
            // colTaxSection
            // 
            resources.ApplyResources(this.colTaxSection, "colTaxSection");
            this.colTaxSection.Name = "colTaxSection";
            // 
            // colWhtDescription
            // 
            resources.ApplyResources(this.colWhtDescription, "colWhtDescription");
            this.colWhtDescription.Name = "colWhtDescription";
            // 
            // colWhtRate
            // 
            resources.ApplyResources(this.colWhtRate, "colWhtRate");
            this.colWhtRate.Name = "colWhtRate";
            // 
            // colEffectiveFrom
            // 
            resources.ApplyResources(this.colEffectiveFrom, "colEffectiveFrom");
            this.colEffectiveFrom.Name = "colEffectiveFrom";
            // 
            // colIsActive
            // 
            resources.ApplyResources(this.colIsActive, "colIsActive");
            this.colIsActive.Name = "colIsActive";
            // 
            // groupTaxTop
            // 
            resources.ApplyResources(this.groupTaxTop, "groupTaxTop");
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
            this.groupTaxTop.Name = "groupTaxTop";
            this.groupTaxTop.TabStop = false;
            // 
            // cmbFilingFrequency
            // 
            resources.ApplyResources(this.cmbFilingFrequency, "cmbFilingFrequency");
            this.cmbFilingFrequency.Name = "cmbFilingFrequency";
            // 
            // txtFbrStrn
            // 
            resources.ApplyResources(this.txtFbrStrn, "txtFbrStrn");
            this.txtFbrStrn.Name = "txtFbrStrn";
            // 
            // txtFbrNtn
            // 
            resources.ApplyResources(this.txtFbrNtn, "txtFbrNtn");
            this.txtFbrNtn.Name = "txtFbrNtn";
            // 
            // cmbTaxMode
            // 
            resources.ApplyResources(this.cmbTaxMode, "cmbTaxMode");
            this.cmbTaxMode.Name = "cmbTaxMode";
            // 
            // numSalesTaxRate
            // 
            resources.ApplyResources(this.numSalesTaxRate, "numSalesTaxRate");
            this.numSalesTaxRate.DecimalPlaces = 2;
            this.numSalesTaxRate.Name = "numSalesTaxRate";
            // 
            // lblFilingFrequency
            // 
            resources.ApplyResources(this.lblFilingFrequency, "lblFilingFrequency");
            this.lblFilingFrequency.Name = "lblFilingFrequency";
            // 
            // lblFbrStrn
            // 
            resources.ApplyResources(this.lblFbrStrn, "lblFbrStrn");
            this.lblFbrStrn.Name = "lblFbrStrn";
            // 
            // lblFbrNtn
            // 
            resources.ApplyResources(this.lblFbrNtn, "lblFbrNtn");
            this.lblFbrNtn.Name = "lblFbrNtn";
            // 
            // lblTaxMode
            // 
            resources.ApplyResources(this.lblTaxMode, "lblTaxMode");
            this.lblTaxMode.Name = "lblTaxMode";
            // 
            // lblSalesTaxRate
            // 
            resources.ApplyResources(this.lblSalesTaxRate, "lblSalesTaxRate");
            this.lblSalesTaxRate.Name = "lblSalesTaxRate";
            // 
            // tabPosting
            // 
            resources.ApplyResources(this.tabPosting, "tabPosting");
            this.tabPosting.Controls.Add(this.groupPosting);
            this.tabPosting.Name = "tabPosting";
            this.tabPosting.UseVisualStyleBackColor = true;
            // 
            // groupPosting
            // 
            resources.ApplyResources(this.groupPosting, "groupPosting");
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
            this.groupPosting.Name = "groupPosting";
            this.groupPosting.TabStop = false;
            // 
            // numApprovalThreshold
            // 
            resources.ApplyResources(this.numApprovalThreshold, "numApprovalThreshold");
            this.numApprovalThreshold.DecimalPlaces = 2;
            this.numApprovalThreshold.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numApprovalThreshold.Name = "numApprovalThreshold";
            // 
            // numBackdatingDays
            // 
            resources.ApplyResources(this.numBackdatingDays, "numBackdatingDays");
            this.numBackdatingDays.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numBackdatingDays.Name = "numBackdatingDays";
            // 
            // numBudgetWarningPct
            // 
            resources.ApplyResources(this.numBudgetWarningPct, "numBudgetWarningPct");
            this.numBudgetWarningPct.Name = "numBudgetWarningPct";
            // 
            // chkRequireNarration
            // 
            resources.ApplyResources(this.chkRequireNarration, "chkRequireNarration");
            this.chkRequireNarration.Name = "chkRequireNarration";
            // 
            // chkAllowLockedPeriodPosting
            // 
            resources.ApplyResources(this.chkAllowLockedPeriodPosting, "chkAllowLockedPeriodPosting");
            this.chkAllowLockedPeriodPosting.Name = "chkAllowLockedPeriodPosting";
            // 
            // chkAutoPostPurchases
            // 
            resources.ApplyResources(this.chkAutoPostPurchases, "chkAutoPostPurchases");
            this.chkAutoPostPurchases.Name = "chkAutoPostPurchases";
            // 
            // chkAutoPostSales
            // 
            resources.ApplyResources(this.chkAutoPostSales, "chkAutoPostSales");
            this.chkAutoPostSales.Name = "chkAutoPostSales";
            // 
            // lblApprovalThreshold
            // 
            resources.ApplyResources(this.lblApprovalThreshold, "lblApprovalThreshold");
            this.lblApprovalThreshold.Name = "lblApprovalThreshold";
            // 
            // lblBackdatingDays
            // 
            resources.ApplyResources(this.lblBackdatingDays, "lblBackdatingDays");
            this.lblBackdatingDays.Name = "lblBackdatingDays";
            // 
            // lblBudgetWarning
            // 
            resources.ApplyResources(this.lblBudgetWarning, "lblBudgetWarning");
            this.lblBudgetWarning.Name = "lblBudgetWarning";
            // 
            // tabReports
            // 
            resources.ApplyResources(this.tabReports, "tabReports");
            this.tabReports.Controls.Add(this.groupReports);
            this.tabReports.Name = "tabReports";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // groupReports
            // 
            resources.ApplyResources(this.groupReports, "groupReports");
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
            this.groupReports.Name = "groupReports";
            this.groupReports.TabStop = false;
            // 
            // txtDigitalSignature
            // 
            resources.ApplyResources(this.txtDigitalSignature, "txtDigitalSignature");
            this.txtDigitalSignature.Name = "txtDigitalSignature";
            // 
            // txtReportFooter
            // 
            resources.ApplyResources(this.txtReportFooter, "txtReportFooter");
            this.txtReportFooter.Name = "txtReportFooter";
            // 
            // txtReportHeader
            // 
            resources.ApplyResources(this.txtReportHeader, "txtReportHeader");
            this.txtReportHeader.Name = "txtReportHeader";
            // 
            // cmbShowAmountsIn
            // 
            resources.ApplyResources(this.cmbShowAmountsIn, "cmbShowAmountsIn");
            this.cmbShowAmountsIn.Name = "cmbShowAmountsIn";
            // 
            // cmbReportDateFormat
            // 
            resources.ApplyResources(this.cmbReportDateFormat, "cmbReportDateFormat");
            this.cmbReportDateFormat.Name = "cmbReportDateFormat";
            // 
            // cmbAmountFormat
            // 
            resources.ApplyResources(this.cmbAmountFormat, "cmbAmountFormat");
            this.cmbAmountFormat.Name = "cmbAmountFormat";
            // 
            // lblDigitalSignature
            // 
            resources.ApplyResources(this.lblDigitalSignature, "lblDigitalSignature");
            this.lblDigitalSignature.Name = "lblDigitalSignature";
            // 
            // lblReportFooter
            // 
            resources.ApplyResources(this.lblReportFooter, "lblReportFooter");
            this.lblReportFooter.Name = "lblReportFooter";
            // 
            // lblReportHeader
            // 
            resources.ApplyResources(this.lblReportHeader, "lblReportHeader");
            this.lblReportHeader.Name = "lblReportHeader";
            // 
            // lblShowAmountsIn
            // 
            resources.ApplyResources(this.lblShowAmountsIn, "lblShowAmountsIn");
            this.lblShowAmountsIn.Name = "lblShowAmountsIn";
            // 
            // lblDateFormat
            // 
            resources.ApplyResources(this.lblDateFormat, "lblDateFormat");
            this.lblDateFormat.Name = "lblDateFormat";
            // 
            // lblAmountFormat
            // 
            resources.ApplyResources(this.lblAmountFormat, "lblAmountFormat");
            this.lblAmountFormat.Name = "lblAmountFormat";
            // 
            // panelBottom
            // 
            resources.ApplyResources(this.panelBottom, "panelBottom");
            this.panelBottom.Controls.Add(this.btnResetDefaults);
            this.panelBottom.Controls.Add(this.btnSaveSettings);
            this.panelBottom.Name = "panelBottom";
            // 
            // btnResetDefaults
            // 
            resources.ApplyResources(this.btnResetDefaults, "btnResetDefaults");
            this.btnResetDefaults.Name = "btnResetDefaults";
            this.btnResetDefaults.UseVisualStyleBackColor = true;
            this.btnResetDefaults.Click += new System.EventHandler(this.btnResetDefaults_Click);
            // 
            // btnSaveSettings
            // 
            resources.ApplyResources(this.btnSaveSettings, "btnSaveSettings");
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // frm_accounting_settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lblTitle);
            this.Name = "frm_accounting_settings";
            this.Load += new System.EventHandler(this.frm_accounting_settings_Load);
            this.tabMain.ResumeLayout(false);
            this.tabCompany.ResumeLayout(false);
            this.groupCompanyHeader.ResumeLayout(false);
            this.groupCompanyHeader.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txt_buildingNumber;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txt_countryName;
        private System.Windows.Forms.TextBox txt_postalCode;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txt_cityName;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txt_citySubdivisionName;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txt_StreetName;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chk_use_zatca_e_invoice;
    }
}
