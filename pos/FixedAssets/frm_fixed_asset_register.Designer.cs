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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.dgvAssets = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnAddAsset = new System.Windows.Forms.Button();
            this.btnImportAssets = new System.Windows.Forms.Button();
            this.btnManageCategories = new System.Windows.Forms.Button();
            this.btnManageLocations = new System.Windows.Forms.Button();
            this.btnEditAsset = new System.Windows.Forms.Button();
            this.btnDeleteAsset = new System.Windows.Forms.Button();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.btnSaveAssetInfo = new System.Windows.Forms.Button();
            this.btnRefreshGrid = new System.Windows.Forms.Button();
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
            this.pnlSummaryCard = new System.Windows.Forms.Panel();
            this.lblCostSummary = new System.Windows.Forms.Label();
            this.lblAssetNameSummary = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
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
            this.pnlSummaryCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.splitContainer);
            this.mainPanel.Controls.Add(this.pnlSummaryCard);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(6);
            this.mainPanel.Size = new System.Drawing.Size(1400, 862);
            this.mainPanel.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(6, 104);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.leftPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rightPanel);
            this.splitContainer.Size = new System.Drawing.Size(1388, 752);
            this.splitContainer.SplitterDistance = 820;
            this.splitContainer.TabIndex = 0;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.dgvAssets);
            this.leftPanel.Controls.Add(this.pnlToolbar);
            this.leftPanel.Controls.Add(this.pnlFilters);
            this.leftPanel.Controls.Add(this.txtSearch);
            this.leftPanel.Controls.Add(this.lblSearch);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Margin = new System.Windows.Forms.Padding(4);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(6);
            this.leftPanel.Size = new System.Drawing.Size(720, 752);
            this.leftPanel.TabIndex = 0;
            // 
            // dgvAssets
            // 
            this.dgvAssets.AllowUserToAddRows = false;
            this.dgvAssets.AllowUserToDeleteRows = false;
            this.dgvAssets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAssets.ColumnHeadersHeight = 29;
            this.dgvAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            this.dgvAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssets.Location = new System.Drawing.Point(6, 92);
            this.dgvAssets.Margin = new System.Windows.Forms.Padding(4);
            this.dgvAssets.MultiSelect = false;
            this.dgvAssets.Name = "dgvAssets";
            this.dgvAssets.ReadOnly = true;
            this.dgvAssets.RowHeadersWidth = 51;
            this.dgvAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssets.Size = new System.Drawing.Size(808, 611);
            this.dgvAssets.TabIndex = 0;
            this.dgvAssets.SelectionChanged += new System.EventHandler(this.DgvAssets_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Code";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 69;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 72;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Category";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 94;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Purchase Date";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 126;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Cost";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 65;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Book Value";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 104;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Status";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 76;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.Controls.Add(this.btnAddAsset);
            this.pnlToolbar.Controls.Add(this.btnImportAssets);
            this.pnlToolbar.Controls.Add(this.btnManageCategories);
            this.pnlToolbar.Controls.Add(this.btnManageLocations);
            this.pnlToolbar.Controls.Add(this.btnEditAsset);
            this.pnlToolbar.Controls.Add(this.btnDeleteAsset);
            this.pnlToolbar.Controls.Add(this.btnRefreshGrid);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlToolbar.Location = new System.Drawing.Point(6, 703);
            this.pnlToolbar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(808, 43);
            this.pnlToolbar.TabIndex = 1;
            // 
            // btnAddAsset
            // 
            this.btnAddAsset.Location = new System.Drawing.Point(6, 2);
            this.btnAddAsset.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddAsset.Name = "btnAddAsset";
            this.btnAddAsset.Size = new System.Drawing.Size(140, 37);
            this.btnAddAsset.TabIndex = 0;
            this.btnAddAsset.Text = "Add New Asset";
            this.btnAddAsset.Click += new System.EventHandler(this.BtnAddAsset_Click);
            // 
            // btnImportAssets
            // 
            this.btnImportAssets.Location = new System.Drawing.Point(146, 2);
            this.btnImportAssets.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportAssets.Name = "btnImportAssets";
            this.btnImportAssets.Size = new System.Drawing.Size(140, 37);
            this.btnImportAssets.TabIndex = 1;
            this.btnImportAssets.Text = "Import Assets";
            // 
            // btnManageCategories
            // 
            this.btnManageCategories.Location = new System.Drawing.Point(286, 2);
            this.btnManageCategories.Margin = new System.Windows.Forms.Padding(4);
            this.btnManageCategories.Name = "btnManageCategories";
            this.btnManageCategories.Size = new System.Drawing.Size(149, 37);
            this.btnManageCategories.TabIndex = 2;
            this.btnManageCategories.Text = "Manage Categories";
            this.btnManageCategories.Click += new System.EventHandler(this.BtnManageCategories_Click);
            // 
            // btnManageLocations
            // 
            this.btnManageLocations.Location = new System.Drawing.Point(529, 2);
            this.btnManageLocations.Margin = new System.Windows.Forms.Padding(4);
            this.btnManageLocations.Name = "btnManageLocations";
            this.btnManageLocations.Size = new System.Drawing.Size(140, 37);
            this.btnManageLocations.TabIndex = 3;
            this.btnManageLocations.Text = "Manage Locations";
            this.btnManageLocations.Click += new System.EventHandler(this.BtnManageLocations_Click);
            // 
            // btnEditAsset
            // 
            this.btnEditAsset.Location = new System.Drawing.Point(435, 2);
            this.btnEditAsset.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditAsset.Name = "btnEditAsset";
            this.btnEditAsset.Size = new System.Drawing.Size(94, 37);
            this.btnEditAsset.TabIndex = 4;
            this.btnEditAsset.Text = "Edit Asset";
            this.btnEditAsset.Click += new System.EventHandler(this.BtnEditAsset_Click);
            // 
            // btnDeleteAsset
            // 
            this.btnDeleteAsset.Location = new System.Drawing.Point(669, 2);
            this.btnDeleteAsset.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteAsset.Name = "btnDeleteAsset";
            this.btnDeleteAsset.Size = new System.Drawing.Size(105, 37);
            this.btnDeleteAsset.TabIndex = 5;
            this.btnDeleteAsset.Text = "Delete Asset";
            this.btnDeleteAsset.Click += new System.EventHandler(this.BtnDeleteAsset_Click);
            // 
            // btnRefreshGrid
            // 
            this.btnRefreshGrid.Location = new System.Drawing.Point(774, 2);
            this.btnRefreshGrid.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshGrid.Name = "btnRefreshGrid";
            this.btnRefreshGrid.Size = new System.Drawing.Size(94, 37);
            this.btnRefreshGrid.TabIndex = 6;
            this.btnRefreshGrid.Text = "Refresh";
            this.btnRefreshGrid.Click += new System.EventHandler(this.BtnRefreshGrid_Click);
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.lblCategory);
            this.pnlFilters.Controls.Add(this.ddlCategory);
            this.pnlFilters.Controls.Add(this.lblStatus);
            this.pnlFilters.Controls.Add(this.ddlStatus);
            this.pnlFilters.Controls.Add(this.lblLocation);
            this.pnlFilters.Controls.Add(this.ddlLocation);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(6, 55);
            this.pnlFilters.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(808, 37);
            this.pnlFilters.TabIndex = 2;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(0, 6);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(82, 28);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "Category:";
            // 
            // ddlCategory
            // 
            this.ddlCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategory.Location = new System.Drawing.Point(88, 4);
            this.ddlCategory.Margin = new System.Windows.Forms.Padding(4);
            this.ddlCategory.Name = "ddlCategory";
            this.ddlCategory.Size = new System.Drawing.Size(139, 24);
            this.ddlCategory.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(233, 6);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 28);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status:";
            // 
            // ddlStatus
            // 
            this.ddlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlStatus.Items.AddRange(new object[] {
            "All",
            "Active",
            "Disposed",
            "Under Repair",
            "Fully Depreciated"});
            this.ddlStatus.Location = new System.Drawing.Point(298, 4);
            this.ddlStatus.Margin = new System.Windows.Forms.Padding(4);
            this.ddlStatus.Name = "ddlStatus";
            this.ddlStatus.Size = new System.Drawing.Size(116, 24);
            this.ddlStatus.TabIndex = 3;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(420, 6);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(70, 28);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location:";
            // 
            // ddlLocation
            // 
            this.ddlLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLocation.Location = new System.Drawing.Point(496, 4);
            this.ddlLocation.Margin = new System.Windows.Forms.Padding(4);
            this.ddlLocation.Name = "ddlLocation";
            this.ddlLocation.Size = new System.Drawing.Size(139, 24);
            this.ddlLocation.TabIndex = 5;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(6, 31);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(808, 24);
            this.txtSearch.TabIndex = 3;
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSearch.Location = new System.Drawing.Point(6, 6);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(808, 25);
            this.lblSearch.TabIndex = 4;
            this.lblSearch.Text = "Search:";
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.tabControl);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(0, 0);
            this.rightPanel.Margin = new System.Windows.Forms.Padding(4);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(6);
            this.rightPanel.Size = new System.Drawing.Size(564, 752);
            this.rightPanel.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAssetInfo);
            this.tabControl.Controls.Add(this.tabDepreciationSetup);
            this.tabControl.Controls.Add(this.tabDepreciationHistory);
            this.tabControl.Controls.Add(this.tabDisposalRevaluation);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(6, 6);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(552, 740);
            this.tabControl.TabIndex = 0;
            // 
            // tabAssetInfo
            // 
            this.tabAssetInfo.Location = new System.Drawing.Point(4, 25);
            this.tabAssetInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tabAssetInfo.Name = "tabAssetInfo";
            this.tabAssetInfo.Padding = new System.Windows.Forms.Padding(12);
            this.tabAssetInfo.Size = new System.Drawing.Size(544, 711);
            this.tabAssetInfo.TabIndex = 0;
            this.tabAssetInfo.Text = "Asset Information";
            // 
            // tabDepreciationSetup
            // 
            this.tabDepreciationSetup.Location = new System.Drawing.Point(4, 25);
            this.tabDepreciationSetup.Margin = new System.Windows.Forms.Padding(4);
            this.tabDepreciationSetup.Name = "tabDepreciationSetup";
            this.tabDepreciationSetup.Padding = new System.Windows.Forms.Padding(12);
            this.tabDepreciationSetup.Size = new System.Drawing.Size(544, 711);
            this.tabDepreciationSetup.TabIndex = 1;
            this.tabDepreciationSetup.Text = "Depreciation Setup";
            // 
            // tabDepreciationHistory
            // 
            this.tabDepreciationHistory.Location = new System.Drawing.Point(4, 25);
            this.tabDepreciationHistory.Margin = new System.Windows.Forms.Padding(4);
            this.tabDepreciationHistory.Name = "tabDepreciationHistory";
            this.tabDepreciationHistory.Padding = new System.Windows.Forms.Padding(12);
            this.tabDepreciationHistory.Size = new System.Drawing.Size(544, 711);
            this.tabDepreciationHistory.TabIndex = 2;
            this.tabDepreciationHistory.Text = "Depreciation History";
            // 
            // tabDisposalRevaluation
            // 
            this.tabDisposalRevaluation.Location = new System.Drawing.Point(4, 25);
            this.tabDisposalRevaluation.Margin = new System.Windows.Forms.Padding(4);
            this.tabDisposalRevaluation.Name = "tabDisposalRevaluation";
            this.tabDisposalRevaluation.Padding = new System.Windows.Forms.Padding(12);
            this.tabDisposalRevaluation.Size = new System.Drawing.Size(544, 711);
            this.tabDisposalRevaluation.TabIndex = 3;
            this.tabDisposalRevaluation.Text = "Disposal / Revaluation";
            // 
            // pnlSummaryCard
            // 
            this.pnlSummaryCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlSummaryCard.Controls.Add(this.lblCostSummary);
            this.pnlSummaryCard.Controls.Add(this.lblAssetNameSummary);
            this.pnlSummaryCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummaryCard.ForeColor = System.Drawing.Color.White;
            this.pnlSummaryCard.Location = new System.Drawing.Point(6, 6);
            this.pnlSummaryCard.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSummaryCard.Name = "pnlSummaryCard";
            this.pnlSummaryCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlSummaryCard.Size = new System.Drawing.Size(1388, 98);
            this.pnlSummaryCard.TabIndex = 1;
            // 
            // lblCostSummary
            // 
            this.lblCostSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCostSummary.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCostSummary.Location = new System.Drawing.Point(12, 43);
            this.lblCostSummary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCostSummary.Name = "lblCostSummary";
            this.lblCostSummary.Size = new System.Drawing.Size(1364, 43);
            this.lblCostSummary.TabIndex = 0;
            this.lblCostSummary.Text = "Cost: PKR 0 | Book Value: PKR 0 | Age: 0 years | Depreciation to Date: PKR 0";
            // 
            // lblAssetNameSummary
            // 
            this.lblAssetNameSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAssetNameSummary.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAssetNameSummary.Location = new System.Drawing.Point(12, 12);
            this.lblAssetNameSummary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAssetNameSummary.Name = "lblAssetNameSummary";
            this.lblAssetNameSummary.Size = new System.Drawing.Size(1364, 31);
            this.lblAssetNameSummary.TabIndex = 1;
            this.lblAssetNameSummary.Text = "No Asset Selected";
            // 
            // frm_fixed_asset_register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 862);
            this.Controls.Add(this.mainPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frm_fixed_asset_register";
            this.Text = "Fixed Asset Register";
            this.Load += new System.EventHandler(this.frm_fixed_asset_register_Load);
            this.mainPanel.ResumeLayout(false);
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
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Asset Code:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtAssetCode = new System.Windows.Forms.TextBox() { ReadOnly = true, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtAssetCode); y += gap;

            // Asset Name
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Asset Name:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtAssetName = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtAssetName); y += gap;

            // Description
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Description:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtDescription = new System.Windows.Forms.TextBox() { Multiline = true, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 52) };
            scroll.Controls.Add(this.txtDescription); y += 58;

            // Category
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Category:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAssetCategory = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAssetCategory); y += gap;

            // Purchase Date
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Purchase Date:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtPurchaseDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtPurchaseDate); y += gap;

            // Supplier
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Supplier:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlSupplier = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlSupplier); y += gap;

            // Invoice No
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Invoice No:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtInvoiceNo = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtInvoiceNo); y += gap;

            // Purchase Cost
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Purchase Cost:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtCost = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtCost); y += gap;

            // Location
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Location:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAssetLocation = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAssetLocation); y += gap;

            // Serial Number
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Serial Number:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtSerialNumber = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtSerialNumber); y += gap;

            // Model Number
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Model Number:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtModelNumber = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtModelNumber); y += gap;

            // Status (display-only; not editable)
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Status:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h), Visible = false });
            this.ddlAssetStatus = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h), Visible = false, Enabled = false };
            this.ddlAssetStatus.Items.AddRange(new object[] { "Active", "Under Repair", "Disposed", "Fully Depreciated" });
            scroll.Controls.Add(this.ddlAssetStatus); y += gap;

            // Save Asset Info button (inside Asset Info tab)
            this.btnSaveAssetInfo = new System.Windows.Forms.Button() { Text = "Save Asset Info", Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 28) };
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
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Dep. Method:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDepMethod = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            this.ddlDepMethod.Items.AddRange(new object[] { "STRAIGHT_LINE", "REDUCING_BALANCE", "UNITS_OF_PRODUCTION" });
            scroll.Controls.Add(this.ddlDepMethod); y += gap;

            // Useful Life Years
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Useful Life (Years):", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.numUsefulLifeYears = new System.Windows.Forms.NumericUpDown() { Minimum = 0, Maximum = 100, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.numUsefulLifeYears); y += gap;

            // Useful Life Months
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Useful Life (Months):", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.numUsefulLifeMonths = new System.Windows.Forms.NumericUpDown() { Minimum = 0, Maximum = 1200, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.numUsefulLifeMonths); y += gap;

            // Residual Value
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Residual Value:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtResidualValue = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtResidualValue); y += gap;

            // Dep Rate
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Dep. Rate (%):", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtDepRate = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtDepRate); y += gap;

            // Dep Account
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Dep. Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDepAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDepAccount); y += gap;

            // Accum Dep Account
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Accum. Dep. Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlAccumDepAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlAccumDepAccount); y += gap;

            // Start Dep Date
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Start Dep. From:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtStartDepreciationDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtStartDepreciationDate); y += gap + 4;

            // Save Dep Setup button
            this.btnSaveDepSetup = new System.Windows.Forms.Button();
            this.btnSaveDepSetup.Text = "Save Depreciation Setup";
            this.btnSaveDepSetup.Location = new System.Drawing.Point(cx, y);
            this.btnSaveDepSetup.Size = new System.Drawing.Size(160, 28);
            this.btnSaveDepSetup.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnSaveDepSetup.ForeColor = System.Drawing.Color.White;
            this.btnSaveDepSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            scroll.Controls.Add(this.btnSaveDepSetup); y += 40;

            // Schedule label
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Depreciation Schedule Preview:", Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });
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
            this.lblCurrentWDV.Text = "Current Book Value (WDV): PKR 0.00";
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
            this.btnRunDepreciation.Text = "Run Depreciation";
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
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "── DISPOSAL ──", Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });
            y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Disposal Date:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtDisposalDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtDisposalDate); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Disposal Method:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalMethod = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            this.ddlDisposalMethod.Items.AddRange(new object[] { "Sale", "Write-Off", "Donation", "Scrapped" });
            scroll.Controls.Add(this.ddlDisposalMethod); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Receipt Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalReceiptAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalReceiptAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Asset Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalAssetAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalAssetAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Gain Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalGainAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalGainAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Loss Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlDisposalLossAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlDisposalLossAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Proceeds (PKR):", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtDisposalProceeds = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtDisposalProceeds); y += gap;

            this.lblGainLossDisplay = new System.Windows.Forms.Label() { Text = "Gain / Loss: PKR 0.00", Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) };
            scroll.Controls.Add(this.lblGainLossDisplay); y += gap;

            this.btnPostDisposal = new System.Windows.Forms.Button() { Text = "Post Disposal", Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 28) };
            scroll.Controls.Add(this.btnPostDisposal); y += 40;

            // --- REVALUATION ---
            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "── REVALUATION ──", Location = new System.Drawing.Point(lx, y), Size = new System.Drawing.Size(320, h), Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });
            y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Revaluation Date:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.dtRevaluationDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.dtRevaluationDate); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "New Revalued Amount:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.txtNewRevaluedAmount = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.txtNewRevaluedAmount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Asset Account:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlRevaluationAssetAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlRevaluationAssetAccount); y += gap;

            scroll.Controls.Add(new System.Windows.Forms.Label() { Text = "Revaluation Reserve:", Location = new System.Drawing.Point(lx, y + 3), Size = new System.Drawing.Size(148, h) });
            this.ddlRevaluationAccount = new System.Windows.Forms.ComboBox() { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, h) };
            scroll.Controls.Add(this.ddlRevaluationAccount); y += gap;

            this.btnPostRevaluation = new System.Windows.Forms.Button() { Text = "Post Revaluation", Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(w, 28) };
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel rightPanel;
    }
}
