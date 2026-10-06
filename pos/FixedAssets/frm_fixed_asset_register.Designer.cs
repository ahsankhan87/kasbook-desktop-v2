using pos.UI;

namespace pos.FixedAssets
{
    partial class frm_fixed_asset_register
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_fixed_asset_register));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.dgvAssets = new System.Windows.Forms.DataGridView();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnAddAsset = new System.Windows.Forms.Button();
            this.btnImportAssets = new System.Windows.Forms.Button();
            this.btnManageCategories = new System.Windows.Forms.Button();
            this.btnManageLocations = new System.Windows.Forms.Button();
            this.btnEditAsset = new System.Windows.Forms.Button();
            this.btnDeleteAsset = new System.Windows.Forms.Button();
            this.btnRefreshGrid = new System.Windows.Forms.Button();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblCategory = new System.Windows.Forms.Label();
            this.ddlCategory = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ddlStatus = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.ddlLocation = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAssetInfo = new System.Windows.Forms.TabPage();
            this.tabDepreciationSetup = new System.Windows.Forms.TabPage();
            this.tabDepreciationHistory = new System.Windows.Forms.TabPage();
            this.tabDisposalRevaluation = new System.Windows.Forms.TabPage();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.pnlSummaryCard = new System.Windows.Forms.Panel();
            this.lblCostSummary = new System.Windows.Forms.Label();
            this.lblAssetNameSummary = new System.Windows.Forms.Label();
            this.btnSaveAssetInfo = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).BeginInit();
            this.pnlToolbar.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.pnlSummaryCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
            this.splitContainer.Panel1.Controls.Add(this.leftPanel);
            // 
            // splitContainer.Panel2
            // 
            resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
            this.splitContainer.Panel2.Controls.Add(this.rightPanel);
            // 
            // leftPanel
            // 
            resources.ApplyResources(this.leftPanel, "leftPanel");
            this.leftPanel.Controls.Add(this.dgvAssets);
            this.leftPanel.Controls.Add(this.pnlToolbar);
            this.leftPanel.Controls.Add(this.pnlFilters);
            this.leftPanel.Controls.Add(this.txtSearch);
            this.leftPanel.Controls.Add(this.lblSearch);
            this.leftPanel.Name = "leftPanel";
            // 
            // dgvAssets
            // 
            resources.ApplyResources(this.dgvAssets, "dgvAssets");
            this.dgvAssets.AllowUserToAddRows = false;
            this.dgvAssets.AllowUserToDeleteRows = false;
            this.dgvAssets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            this.dgvAssets.MultiSelect = false;
            this.dgvAssets.Name = "dgvAssets";
            this.dgvAssets.ReadOnly = true;
            this.dgvAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssets.SelectionChanged += new System.EventHandler(this.DgvAssets_SelectionChanged);
            // 
            // pnlToolbar
            // 
            resources.ApplyResources(this.pnlToolbar, "pnlToolbar");
            this.pnlToolbar.Controls.Add(this.btnAddAsset);
            this.pnlToolbar.Controls.Add(this.btnImportAssets);
            this.pnlToolbar.Controls.Add(this.btnManageCategories);
            this.pnlToolbar.Controls.Add(this.btnManageLocations);
            this.pnlToolbar.Controls.Add(this.btnEditAsset);
            this.pnlToolbar.Controls.Add(this.btnDeleteAsset);
            this.pnlToolbar.Controls.Add(this.btnRefreshGrid);
            this.pnlToolbar.Name = "pnlToolbar";
            // 
            // btnAddAsset
            // 
            resources.ApplyResources(this.btnAddAsset, "btnAddAsset");
            this.btnAddAsset.Name = "btnAddAsset";
            this.btnAddAsset.Click += new System.EventHandler(this.BtnAddAsset_Click);
            // 
            // btnImportAssets
            // 
            resources.ApplyResources(this.btnImportAssets, "btnImportAssets");
            this.btnImportAssets.Name = "btnImportAssets";
            // 
            // btnManageCategories
            // 
            resources.ApplyResources(this.btnManageCategories, "btnManageCategories");
            this.btnManageCategories.Name = "btnManageCategories";
            this.btnManageCategories.Click += new System.EventHandler(this.BtnManageCategories_Click);
            // 
            // btnManageLocations
            // 
            resources.ApplyResources(this.btnManageLocations, "btnManageLocations");
            this.btnManageLocations.Name = "btnManageLocations";
            this.btnManageLocations.Click += new System.EventHandler(this.BtnManageLocations_Click);
            // 
            // btnEditAsset
            // 
            resources.ApplyResources(this.btnEditAsset, "btnEditAsset");
            this.btnEditAsset.Name = "btnEditAsset";
            this.btnEditAsset.Click += new System.EventHandler(this.BtnEditAsset_Click);
            // 
            // btnDeleteAsset
            // 
            resources.ApplyResources(this.btnDeleteAsset, "btnDeleteAsset");
            this.btnDeleteAsset.Name = "btnDeleteAsset";
            this.btnDeleteAsset.Click += new System.EventHandler(this.BtnDeleteAsset_Click);
            // 
            // btnRefreshGrid
            // 
            resources.ApplyResources(this.btnRefreshGrid, "btnRefreshGrid");
            this.btnRefreshGrid.Name = "btnRefreshGrid";
            this.btnRefreshGrid.Click += new System.EventHandler(this.BtnRefreshGrid_Click);
            // 
            // pnlFilters
            // 
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
            this.pnlFilters.Controls.Add(this.lblCategory);
            this.pnlFilters.Controls.Add(this.ddlCategory);
            this.pnlFilters.Controls.Add(this.lblStatus);
            this.pnlFilters.Controls.Add(this.ddlStatus);
            this.pnlFilters.Controls.Add(this.lblLocation);
            this.pnlFilters.Controls.Add(this.ddlLocation);
            this.pnlFilters.Name = "pnlFilters";
            // 
            // lblCategory
            // 
            resources.ApplyResources(this.lblCategory, "lblCategory");
            this.lblCategory.Name = "lblCategory";
            // 
            // ddlCategory
            // 
            resources.ApplyResources(this.ddlCategory, "ddlCategory");
            this.ddlCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategory.Name = "ddlCategory";
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // ddlStatus
            // 
            resources.ApplyResources(this.ddlStatus, "ddlStatus");
            this.ddlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlStatus.Items.AddRange(new object[] {
            resources.GetString("ddlStatus.Items"),
            resources.GetString("ddlStatus.Items1"),
            resources.GetString("ddlStatus.Items2"),
            resources.GetString("ddlStatus.Items3"),
            resources.GetString("ddlStatus.Items4")});
            this.ddlStatus.Name = "ddlStatus";
            // 
            // lblLocation
            // 
            resources.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            // 
            // ddlLocation
            // 
            resources.ApplyResources(this.ddlLocation, "ddlLocation");
            this.ddlLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLocation.Name = "ddlLocation";
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            // 
            // lblSearch
            // 
            resources.ApplyResources(this.lblSearch, "lblSearch");
            this.lblSearch.Name = "lblSearch";
            // 
            // rightPanel
            // 
            resources.ApplyResources(this.rightPanel, "rightPanel");
            this.rightPanel.Controls.Add(this.tabControl);
            this.rightPanel.Name = "rightPanel";
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.tabAssetInfo);
            this.tabControl.Controls.Add(this.tabDepreciationSetup);
            this.tabControl.Controls.Add(this.tabDepreciationHistory);
            this.tabControl.Controls.Add(this.tabDisposalRevaluation);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabAssetInfo
            // 
            resources.ApplyResources(this.tabAssetInfo, "tabAssetInfo");
            this.tabAssetInfo.Name = "tabAssetInfo";
            // 
            // tabDepreciationSetup
            // 
            resources.ApplyResources(this.tabDepreciationSetup, "tabDepreciationSetup");
            this.tabDepreciationSetup.Name = "tabDepreciationSetup";
            // 
            // tabDepreciationHistory
            // 
            resources.ApplyResources(this.tabDepreciationHistory, "tabDepreciationHistory");
            this.tabDepreciationHistory.Name = "tabDepreciationHistory";
            // 
            // tabDisposalRevaluation
            // 
            resources.ApplyResources(this.tabDisposalRevaluation, "tabDisposalRevaluation");
            this.tabDisposalRevaluation.Name = "tabDisposalRevaluation";
            // 
            // mainPanel
            // 
            resources.ApplyResources(this.mainPanel, "mainPanel");
            this.mainPanel.Controls.Add(this.splitContainer);
            this.mainPanel.Controls.Add(this.pnlSummaryCard);
            this.mainPanel.Name = "mainPanel";
            // 
            // pnlSummaryCard
            // 
            resources.ApplyResources(this.pnlSummaryCard, "pnlSummaryCard");
            this.pnlSummaryCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlSummaryCard.Controls.Add(this.lblCostSummary);
            this.pnlSummaryCard.Controls.Add(this.lblAssetNameSummary);
            this.pnlSummaryCard.ForeColor = System.Drawing.Color.White;
            this.pnlSummaryCard.Name = "pnlSummaryCard";
            // 
            // lblCostSummary
            // 
            resources.ApplyResources(this.lblCostSummary, "lblCostSummary");
            this.lblCostSummary.Name = "lblCostSummary";
            // 
            // lblAssetNameSummary
            // 
            resources.ApplyResources(this.lblAssetNameSummary, "lblAssetNameSummary");
            this.lblAssetNameSummary.Name = "lblAssetNameSummary";
            // 
            // btnSaveAssetInfo
            // 
            resources.ApplyResources(this.btnSaveAssetInfo, "btnSaveAssetInfo");
            this.btnSaveAssetInfo.Name = "btnSaveAssetInfo";
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // frm_fixed_asset_register
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Name = "frm_fixed_asset_register";
            this.Load += new System.EventHandler(this.frm_fixed_asset_register_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).EndInit();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlFilters.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.pnlSummaryCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void BuildAssetInfoTab(System.Windows.Forms.TabPage tab)
        {
            System.Windows.Forms.Panel scroll = new System.Windows.Forms.Panel();
            scroll.Dock = System.Windows.Forms.DockStyle.Fill;
            scroll.AutoScroll = true;
            scroll.Padding = new System.Windows.Forms.Padding(6);

            int lx = 6, cx = 160, w = 160, h = 24, gap = 32, y = 6;

            // Asset Code
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Asset Code:", "رمز الأصل:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtAssetCode = new System.Windows.Forms.TextBox() { ReadOnly = true, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtAssetCode); y += gap;

            // Asset Name
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Asset Name:", "اسم الأصل:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtAssetName = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtAssetName); y += gap;

            // Description
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Description:", "الوصف:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtDescription = new System.Windows.Forms.TextBox() { Multiline = true, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 52) };
            scroll.Controls.Add(this.txtDescription); y += 58;

            // Category
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Category:", "الفئة:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAssetCategory = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAssetCategory); y += gap;

            // Purchase Date
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Purchase Date:", "تاريخ الشراء:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtPurchaseDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtPurchaseDate); y += gap;

            // Supplier
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Supplier:", "المورد:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlSupplier = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlSupplier); y += gap;

            // Invoice No
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Invoice No:", "رقم الفاتورة:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtInvoiceNo = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtInvoiceNo); y += gap;

            // Purchase Cost
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Purchase Cost:", "تكلفة الشراء:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtCost = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtCost); y += gap;

            // Location
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Location:", "الموقع:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAssetLocation = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAssetLocation); y += gap;

            // Cost Center
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Cost Center:", "مركز التكلفة:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAssetCostCenter = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAssetCostCenter); y += gap;

            // Serial Number
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Serial Number:", "الرقم التسلسلي:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtSerialNumber = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtSerialNumber); y += gap;

            // Model Number
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Model Number:", "رقم الموديل:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtModelNumber = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtModelNumber); y += gap;

            // Status (display-only; not editable)
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Status:", "الحالة:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h), Visible = false });
            this.ddlAssetStatus = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h), Visible = false, Enabled = false };
            this.ddlAssetStatus.Items.AddRange(new object[] { "Active", "Under Repair", "Disposed", "Fully Depreciated" });
            scroll.Controls.Add(this.ddlAssetStatus); y += gap;

            // Save Asset Info button (inside Asset Info tab)
            this.btnSaveAssetInfo = new System.Windows.Forms.Button() { Text = UiMessages.T("Save Asset Info", "حفظ معلومات الأصل"), Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 28) };
            scroll.Controls.Add(this.btnSaveAssetInfo);

            tab.Controls.Add(scroll);
        }

        private void BuildDepreciationSetupTab(System.Windows.Forms.TabPage tab)
        {
            System.Windows.Forms.Panel scroll = new System.Windows.Forms.Panel();
            scroll.Dock = System.Windows.Forms.DockStyle.Fill;
            scroll.AutoScroll = true;
            scroll.Padding = new System.Windows.Forms.Padding(6);

            int lx = 6, cx = 160, w = 160, h = 24, gap = 32, y = 6;

            // Depreciation Method
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Dep. Method:", "طريقة الاستهلاك:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDepMethod = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            this.ddlDepMethod.Items.AddRange(new object[] { "STRAIGHT_LINE", "REDUCING_BALANCE", "UNITS_OF_PRODUCTION", "NO_DEPRECIATION" });
            scroll.Controls.Add(this.ddlDepMethod); y += gap;

            // Useful Life Years
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Useful Life (Years):", "العمر الإنتاجي (سنوات):"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.numUsefulLifeYears = new System.Windows.Forms.NumericUpDown() { Minimum = 0, Maximum = 100, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.numUsefulLifeYears); y += gap;

            // Useful Life Months
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Useful Life (Months):", "العمر الإنتاجي (أشهر):"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.numUsefulLifeMonths = new System.Windows.Forms.NumericUpDown() { Minimum = 0, Maximum = 11, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.numUsefulLifeMonths); y += gap;

            // Residual Value
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Residual Value:", "القيمة الباقية:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtResidualValue = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtResidualValue); y += gap;

            // Dep Rate
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Dep. Rate (%):", "معدل الاستهلاك (%):"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtDepRate = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtDepRate); y += gap;

            // Dep Account
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Dep. Account:", "حساب الاستهلاك:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDepAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDepAccount); y += gap;

            // Accum Dep Account
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Accum. Dep. Account:", "حساب مجمع الاستهلاك:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAccumDepAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAccumDepAccount); y += gap;

            // Start Dep Date
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Start Dep. From:", "بدء الاستهلاك من:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtStartDepreciationDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtStartDepreciationDate); y += gap + 4;

            // Save Dep Setup button
            this.btnSaveDepSetup = new System.Windows.Forms.Button();
            this.btnSaveDepSetup.Text = UiMessages.T("Save Depreciation Setup", "حفظ إعدادات الاستهلاك");
            this.btnSaveDepSetup.Location = new System.Drawing.Point(cx, y);
            this.btnSaveDepSetup.Size = new System.Drawing.Size(160, 28);
            this.btnSaveDepSetup.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnSaveDepSetup.ForeColor = System.Drawing.Color.White;
            this.btnSaveDepSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            scroll.Controls.Add(this.btnSaveDepSetup); y += 40;

            // Schedule label
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Depreciation Schedule Preview:", "معاينة جدول الاستهلاك:"), Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });
            y += gap;

            // Schedule grid
            this.dgvDepSchedule = new System.Windows.Forms.DataGridView();
            this.dgvDepSchedule.Location = new System.Drawing.Point(lx, y);
            this.dgvDepSchedule.Size = new System.Drawing.Size(320, 180);
            this.dgvDepSchedule.AllowUserToAddRows = false;
            this.dgvDepSchedule.ReadOnly = true;
            this.dgvDepSchedule.ColumnHeadersHeight = 24;
            this.dgvDepSchedule.Columns.Add("Year", "Period");
            this.dgvDepSchedule.Columns.Add("OpeningWDV", "Opening WDV");
            this.dgvDepSchedule.Columns.Add("DepAmount", "Depreciation");
            this.dgvDepSchedule.Columns.Add("AccumDep", "Accumulated");
            this.dgvDepSchedule.Columns.Add("ClosingWDV", "Closing WDV");
            scroll.Controls.Add(this.dgvDepSchedule);

            tab.Controls.Add(scroll);
        }

        private void BuildDepreciationHistoryTab(System.Windows.Forms.TabPage tab)
        {
            // WDV header bar — add first so Fill grid respects it
            System.Windows.Forms.Panel pnlWDVDisplay = new System.Windows.Forms.Panel();
            pnlWDVDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            pnlWDVDisplay.Height = 36;
            pnlWDVDisplay.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            pnlWDVDisplay.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);

            this.lblCurrentWDV = new System.Windows.Forms.Label();
            this.lblCurrentWDV.Text = UiMessages.T("Current Book Value (WDV): PKR 0.00", "القيمة الدفترية الحالية (WDV): درهم 0.00");
            this.lblCurrentWDV.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.lblCurrentWDV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentWDV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            pnlWDVDisplay.Controls.Add(this.lblCurrentWDV);

            // Run Depreciation button bar — add second
            System.Windows.Forms.Panel pnlButtons = new System.Windows.Forms.Panel();
            pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButtons.Height = 38;
            pnlButtons.Padding = new System.Windows.Forms.Padding(4);

            this.btnRunDepreciation = new System.Windows.Forms.Button();
            this.btnRunDepreciation.Text = UiMessages.T("Run Depreciation", "تشغيل الاستهلاك");
            this.btnRunDepreciation.Size = new System.Drawing.Size(150, 28);
            this.btnRunDepreciation.Location = new System.Drawing.Point(4, 4);
            pnlButtons.Controls.Add(this.btnRunDepreciation);

            // History grid — add last so Dock=Fill fills remaining space
            this.dgvDepreciationHistory = new System.Windows.Forms.DataGridView();
            this.dgvDepreciationHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDepreciationHistory.AllowUserToAddRows = false;
            this.dgvDepreciationHistory.ReadOnly = true;
            this.dgvDepreciationHistory.ColumnHeadersHeight = 24;
            this.dgvDepreciationHistory.Columns.Add("PeriodDate", "Period");
            this.dgvDepreciationHistory.Columns.Add("DepMethod", "Method");
            this.dgvDepreciationHistory.Columns.Add("DepAmount", "Amount");
            this.dgvDepreciationHistory.Columns.Add("WDVAfter", "WDV After");
            this.dgvDepreciationHistory.Columns.Add("VoucherNo", "Voucher No");

            tab.Controls.Add(this.dgvDepreciationHistory);
            tab.Controls.Add(pnlButtons);
            tab.Controls.Add(pnlWDVDisplay);
        }

        private void BuildDisposalRevaluationTab(System.Windows.Forms.TabPage tab)
        {
            System.Windows.Forms.Panel scroll = new System.Windows.Forms.Panel();
            scroll.Dock = System.Windows.Forms.DockStyle.Fill;
            scroll.AutoScroll = true;
            scroll.Padding = new System.Windows.Forms.Padding(6);

            int lx = 6, cx = 160, w = 160, h = 24, gap = 32, y = 6;

            // --- DISPOSAL ---
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("── DISPOSAL ──", "── الاستبعاد ──"), Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });
            y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Disposal Date:", "تاريخ الاستبعاد:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtDisposalDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtDisposalDate); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Disposal Method:", "طريقة الاستبعاد:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalMethod = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            this.ddlDisposalMethod.Items.AddRange(new object[] { "Sale", "Write-Off", "Donation", "Scrapped" });
            scroll.Controls.Add(this.ddlDisposalMethod); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Receipt Account:", "حساب التحصيل:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalReceiptAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalReceiptAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Asset Account:", "حساب الأصل:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalAssetAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalAssetAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Gain Account:", "حساب الأرباح:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalGainAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalGainAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Loss Account:", "حساب الخسائر:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalLossAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalLossAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Proceeds (PKR):", "العائد (درهم):"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtDisposalProceeds = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtDisposalProceeds); y += gap;

            this.lblGainLossDisplay = new System.Windows.Forms.Label() { Text = UiMessages.T("Gain / Loss: PKR 0.00", "الربح / الخسارة: درهم 0.00"), Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) };
            scroll.Controls.Add(this.lblGainLossDisplay); y += gap;

            this.btnPostDisposal = new System.Windows.Forms.Button() { Text = UiMessages.T("Post Disposal", "تسجيل الاستبعاد"), Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 28) };
            scroll.Controls.Add(this.btnPostDisposal); y += 40;

            // --- REVALUATION ---
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("── REVALUATION ──", "── إعادة التقييم ──"), Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });
            y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Revaluation Date:", "تاريخ إعادة التقييم:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtRevaluationDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtRevaluationDate); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("New Revalued Amount:", "المبلغ الجديد بعد إعادة التقييم:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtNewRevaluedAmount = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtNewRevaluedAmount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Asset Account:", "حساب الأصل:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlRevaluationAssetAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlRevaluationAssetAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = UiMessages.T("Revaluation Reserve:", "حساب احتياطي إعادة التقييم:"), Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlRevaluationAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlRevaluationAccount); y += gap;

            this.btnPostRevaluation = new System.Windows.Forms.Button() { Text = UiMessages.T("Post Revaluation", "تسجيل إعادة التقييم"), Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 28) };
            scroll.Controls.Add(this.btnPostRevaluation);

            tab.Controls.Add(scroll);
        }

        // Infrastructure controls
        private System.Windows.Forms.Panel pnlSummaryCard;
        private System.Windows.Forms.Label lblAssetNameSummary;
        private System.Windows.Forms.Label lblCostSummary;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvAssets;
        private System.Windows.Forms.Button btnAddAsset;
        private System.Windows.Forms.Button btnImportAssets;
        private System.Windows.Forms.Button btnManageCategories;
        private System.Windows.Forms.Button btnManageLocations;
        private System.Windows.Forms.Button btnEditAsset;
        private System.Windows.Forms.Button btnDeleteAsset;
        private System.Windows.Forms.Button btnRefreshGrid;
        private System.Windows.Forms.Button btnSaveAssetInfo;
        private System.Windows.Forms.ComboBox ddlCategory;
        private System.Windows.Forms.ComboBox ddlStatus;
        private System.Windows.Forms.ComboBox ddlLocation;
        private System.Windows.Forms.TextBox txtSearch;

        // Tab controls
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAssetInfo;
        private System.Windows.Forms.TabPage tabDepreciationSetup;
        private System.Windows.Forms.TabPage tabDepreciationHistory;
        private System.Windows.Forms.TabPage tabDisposalRevaluation;

        // Asset Info Tab
        private System.Windows.Forms.TextBox txtAssetCode;
        private System.Windows.Forms.TextBox txtAssetName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox ddlAssetCategory;
        private System.Windows.Forms.DateTimePicker dtPurchaseDate;
        private System.Windows.Forms.ComboBox ddlSupplier;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.ComboBox ddlAssetLocation;
        private System.Windows.Forms.ComboBox ddlAssetCostCenter;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.TextBox txtModelNumber;
        private System.Windows.Forms.ComboBox ddlAssetStatus;

        // Depreciation Setup Tab
        private System.Windows.Forms.ComboBox ddlDepMethod;
        private System.Windows.Forms.NumericUpDown numUsefulLifeYears;
        private System.Windows.Forms.NumericUpDown numUsefulLifeMonths;
        private System.Windows.Forms.TextBox txtResidualValue;
        private System.Windows.Forms.TextBox txtDepRate;
        private System.Windows.Forms.ComboBox ddlDepAccount;
        private System.Windows.Forms.ComboBox ddlAccumDepAccount;
        private System.Windows.Forms.DateTimePicker dtStartDepreciationDate;
        private System.Windows.Forms.Button btnSaveDepSetup;
        private System.Windows.Forms.DataGridView dgvDepSchedule;

        // Depreciation History Tab
        private System.Windows.Forms.Label lblCurrentWDV;
        private System.Windows.Forms.DataGridView dgvDepreciationHistory;
        private System.Windows.Forms.Button btnRunDepreciation;

        // Disposal / Revaluation Tab
        private System.Windows.Forms.DateTimePicker dtDisposalDate;
        private System.Windows.Forms.ComboBox ddlDisposalMethod;
        private System.Windows.Forms.ComboBox ddlDisposalReceiptAccount;
        private System.Windows.Forms.ComboBox ddlDisposalAssetAccount;
        private System.Windows.Forms.ComboBox ddlDisposalGainAccount;
        private System.Windows.Forms.ComboBox ddlDisposalLossAccount;
        private System.Windows.Forms.TextBox txtDisposalProceeds;
        private System.Windows.Forms.Label lblGainLossDisplay;
        private System.Windows.Forms.Button btnPostDisposal;
        private System.Windows.Forms.DateTimePicker dtRevaluationDate;
        private System.Windows.Forms.TextBox txtNewRevaluedAmount;
        private System.Windows.Forms.ComboBox ddlRevaluationAssetAccount;
        private System.Windows.Forms.ComboBox ddlRevaluationAccount;
        private System.Windows.Forms.Button btnPostRevaluation;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}
