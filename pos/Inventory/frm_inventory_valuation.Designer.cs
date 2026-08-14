namespace pos.Inventory
{
    partial class frm_inventory_valuation
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle53 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle54 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle55 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabValuation = new System.Windows.Forms.TabPage();
            this.panelValBody = new System.Windows.Forms.Panel();
            this.gridValuation = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastPurchDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastPurchCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReorderLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalValueVal = new System.Windows.Forms.Label();
            this.lblTotalSku = new System.Windows.Forms.Label();
            this.lblTotalSkuVal = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblTotalQtyVal = new System.Windows.Forms.Label();
            this.lblAvgCost = new System.Windows.Forms.Label();
            this.lblAvgCostVal = new System.Windows.Forms.Label();
            this.panelChart = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.panelChartSuppliers = new System.Windows.Forms.Panel();
            this.lblChartSupplierTitle = new System.Windows.Forms.Label();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.lblAsOfDate = new System.Windows.Forms.Label();
            this.dtpAsOfDate = new System.Windows.Forms.DateTimePicker();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.cmbBrand = new System.Windows.Forms.ComboBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.chkShowZero = new System.Windows.Forms.CheckBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabCogs = new System.Windows.Forms.TabPage();
            this.panelCogsBody = new System.Windows.Forms.Panel();
            this.gridCogs = new System.Windows.Forms.DataGridView();
            this.colCCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCCat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoldQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostPerUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalCogs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrossMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVariance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCogsTotals = new System.Windows.Forms.Panel();
            this.lblTotalCogs = new System.Windows.Forms.Label();
            this.lblTotalCogsVal = new System.Windows.Forms.Label();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.lblTotalSalesVal = new System.Windows.Forms.Label();
            this.lblGrossMargin = new System.Windows.Forms.Label();
            this.lblGrossMarginVal = new System.Windows.Forms.Label();
            this.panelCogsFilters = new System.Windows.Forms.Panel();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnCalcCogs = new System.Windows.Forms.Button();
            this.btnPostCogs = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabValuation.SuspendLayout();
            this.panelValBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridValuation)).BeginInit();
            this.panelSummary.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.tabCogs.SuspendLayout();
            this.panelCogsBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCogs)).BeginInit();
            this.panelCogsTotals.SuspendLayout();
            this.panelCogsFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.panelHeader.Controls.Add(this.btnSettings);
            this.panelHeader.Controls.Add(this.lblFormTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1220, 50);
            this.panelHeader.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(2110, 10);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(110, 30);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "⚙ Settings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(0, 0);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblFormTitle.Size = new System.Drawing.Size(1220, 50);
            this.lblFormTitle.TabIndex = 1;
            this.lblFormTitle.Text = "Inventory Valuation & COGS";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabValuation);
            this.tabMain.Controls.Add(this.tabCogs);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 50);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1220, 726);
            this.tabMain.TabIndex = 0;
            // 
            // tabValuation
            // 
            this.tabValuation.Controls.Add(this.panelValBody);
            this.tabValuation.Controls.Add(this.panelSummary);
            this.tabValuation.Controls.Add(this.panelFilters);
            this.tabValuation.Location = new System.Drawing.Point(4, 25);
            this.tabValuation.Name = "tabValuation";
            this.tabValuation.Size = new System.Drawing.Size(1212, 697);
            this.tabValuation.TabIndex = 0;
            this.tabValuation.Text = "  Inventory Valuation  ";
            // 
            // panelValBody
            // 
            this.panelValBody.Controls.Add(this.gridValuation);
            this.panelValBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelValBody.Location = new System.Drawing.Point(0, 52);
            this.panelValBody.Name = "panelValBody";
            this.panelValBody.Size = new System.Drawing.Size(1212, 441);
            this.panelValBody.TabIndex = 0;
            // 
            // gridValuation
            // 
            this.gridValuation.AllowUserToAddRows = false;
            this.gridValuation.ColumnHeadersHeight = 29;
            this.gridValuation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colName,
            this.colCategory,
            this.colQty,
            this.colUnitCost,
            this.colTotalValue,
            this.colLastPurchDate,
            this.colLastPurchCost,
            this.colReorderLevel,
            this.colStatus});
            this.gridValuation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridValuation.Location = new System.Drawing.Point(0, 0);
            this.gridValuation.MultiSelect = false;
            this.gridValuation.Name = "gridValuation";
            this.gridValuation.ReadOnly = true;
            this.gridValuation.RowHeadersVisible = false;
            this.gridValuation.RowHeadersWidth = 51;
            this.gridValuation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridValuation.Size = new System.Drawing.Size(1212, 441);
            this.gridValuation.TabIndex = 0;
            this.gridValuation.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridValuation_CellFormatting);
            // 
            // colCode
            // 
            this.colCode.HeaderText = "Code";
            this.colCode.MinimumWidth = 6;
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 90;
            // 
            // colName
            // 
            this.colName.HeaderText = "Product Name";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 220;
            // 
            // colCategory
            // 
            this.colCategory.HeaderText = "Category";
            this.colCategory.MinimumWidth = 6;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            this.colCategory.Width = 110;
            // 
            // colQty
            // 
            dataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colQty.DefaultCellStyle = dataGridViewCellStyle45;
            this.colQty.HeaderText = "Qty on Hand";
            this.colQty.MinimumWidth = 6;
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            this.colQty.Width = 90;
            // 
            // colUnitCost
            // 
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colUnitCost.DefaultCellStyle = dataGridViewCellStyle46;
            this.colUnitCost.HeaderText = "Unit Cost";
            this.colUnitCost.MinimumWidth = 6;
            this.colUnitCost.Name = "colUnitCost";
            this.colUnitCost.ReadOnly = true;
            this.colUnitCost.Width = 125;
            // 
            // colTotalValue
            // 
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalValue.DefaultCellStyle = dataGridViewCellStyle47;
            this.colTotalValue.HeaderText = "Total Value";
            this.colTotalValue.MinimumWidth = 6;
            this.colTotalValue.Name = "colTotalValue";
            this.colTotalValue.ReadOnly = true;
            this.colTotalValue.Width = 120;
            // 
            // colLastPurchDate
            // 
            this.colLastPurchDate.HeaderText = "Last Purchase";
            this.colLastPurchDate.MinimumWidth = 6;
            this.colLastPurchDate.Name = "colLastPurchDate";
            this.colLastPurchDate.ReadOnly = true;
            this.colLastPurchDate.Width = 125;
            // 
            // colLastPurchCost
            // 
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colLastPurchCost.DefaultCellStyle = dataGridViewCellStyle48;
            this.colLastPurchCost.HeaderText = "Last P. Cost";
            this.colLastPurchCost.MinimumWidth = 6;
            this.colLastPurchCost.Name = "colLastPurchCost";
            this.colLastPurchCost.ReadOnly = true;
            this.colLastPurchCost.Width = 90;
            // 
            // colReorderLevel
            // 
            dataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colReorderLevel.DefaultCellStyle = dataGridViewCellStyle49;
            this.colReorderLevel.HeaderText = "Reorder Lvl";
            this.colReorderLevel.MinimumWidth = 6;
            this.colReorderLevel.Name = "colReorderLevel";
            this.colReorderLevel.ReadOnly = true;
            this.colReorderLevel.Width = 90;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 110;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.panelSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSummary.Controls.Add(this.lblTotalValue);
            this.panelSummary.Controls.Add(this.lblTotalValueVal);
            this.panelSummary.Controls.Add(this.lblTotalSku);
            this.panelSummary.Controls.Add(this.lblTotalSkuVal);
            this.panelSummary.Controls.Add(this.lblTotalQty);
            this.panelSummary.Controls.Add(this.lblTotalQtyVal);
            this.panelSummary.Controls.Add(this.lblAvgCost);
            this.panelSummary.Controls.Add(this.lblAvgCostVal);
            this.panelSummary.Controls.Add(this.panelChart);
            this.panelSummary.Controls.Add(this.lblChartTitle);
            this.panelSummary.Controls.Add(this.panelChartSuppliers);
            this.panelSummary.Controls.Add(this.lblChartSupplierTitle);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSummary.Location = new System.Drawing.Point(0, 493);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1212, 204);
            this.panelSummary.TabIndex = 1;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalValue.Location = new System.Drawing.Point(12, 12);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(190, 20);
            this.lblTotalValue.TabIndex = 0;
            this.lblTotalValue.Text = "Total Inventory Value (PKR):";
            // 
            // lblTotalValueVal
            // 
            this.lblTotalValueVal.AutoSize = true;
            this.lblTotalValueVal.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalValueVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblTotalValueVal.Location = new System.Drawing.Point(12, 30);
            this.lblTotalValueVal.Name = "lblTotalValueVal";
            this.lblTotalValueVal.Size = new System.Drawing.Size(76, 41);
            this.lblTotalValueVal.TabIndex = 1;
            this.lblTotalValueVal.Text = "0.00";
            // 
            // lblTotalSku
            // 
            this.lblTotalSku.AutoSize = true;
            this.lblTotalSku.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalSku.Location = new System.Drawing.Point(200, 12);
            this.lblTotalSku.Name = "lblTotalSku";
            this.lblTotalSku.Size = new System.Drawing.Size(82, 20);
            this.lblTotalSku.TabIndex = 2;
            this.lblTotalSku.Text = "Total SKUs:";
            // 
            // lblTotalSkuVal
            // 
            this.lblTotalSkuVal.AutoSize = true;
            this.lblTotalSkuVal.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalSkuVal.Location = new System.Drawing.Point(200, 30);
            this.lblTotalSkuVal.Name = "lblTotalSkuVal";
            this.lblTotalSkuVal.Size = new System.Drawing.Size(35, 41);
            this.lblTotalSkuVal.TabIndex = 3;
            this.lblTotalSkuVal.Text = "0";
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalQty.Location = new System.Drawing.Point(340, 12);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(72, 20);
            this.lblTotalQty.TabIndex = 4;
            this.lblTotalQty.Text = "Total Qty:";
            // 
            // lblTotalQtyVal
            // 
            this.lblTotalQtyVal.AutoSize = true;
            this.lblTotalQtyVal.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalQtyVal.Location = new System.Drawing.Point(340, 30);
            this.lblTotalQtyVal.Name = "lblTotalQtyVal";
            this.lblTotalQtyVal.Size = new System.Drawing.Size(35, 41);
            this.lblTotalQtyVal.TabIndex = 5;
            this.lblTotalQtyVal.Text = "0";
            // 
            // lblAvgCost
            // 
            this.lblAvgCost.AutoSize = true;
            this.lblAvgCost.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAvgCost.Location = new System.Drawing.Point(480, 12);
            this.lblAvgCost.Name = "lblAvgCost";
            this.lblAvgCost.Size = new System.Drawing.Size(104, 20);
            this.lblAvgCost.TabIndex = 6;
            this.lblAvgCost.Text = "Avg Cost/Unit:";
            // 
            // lblAvgCostVal
            // 
            this.lblAvgCostVal.AutoSize = true;
            this.lblAvgCostVal.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblAvgCostVal.Location = new System.Drawing.Point(480, 30);
            this.lblAvgCostVal.Name = "lblAvgCostVal";
            this.lblAvgCostVal.Size = new System.Drawing.Size(76, 41);
            this.lblAvgCostVal.TabIndex = 7;
            this.lblAvgCostVal.Text = "0.00";
            // 
            // panelChart
            // 
            this.panelChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChart.BackColor = System.Drawing.Color.White;
            this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChart.Location = new System.Drawing.Point(630, 15);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(280, 184);
            this.panelChart.TabIndex = 8;
            this.panelChart.Paint += new System.Windows.Forms.PaintEventHandler(this.panelChart_Paint);
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblChartTitle.Location = new System.Drawing.Point(655, -1);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(185, 20);
            this.lblChartTitle.TabIndex = 9;
            this.lblChartTitle.Text = "Top 5 Categories by Value";
            // 
            // panelChartSuppliers
            // 
            this.panelChartSuppliers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChartSuppliers.BackColor = System.Drawing.Color.White;
            this.panelChartSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChartSuppliers.Location = new System.Drawing.Point(920, 15);
            this.panelChartSuppliers.Name = "panelChartSuppliers";
            this.panelChartSuppliers.Size = new System.Drawing.Size(280, 184);
            this.panelChartSuppliers.TabIndex = 10;
            this.panelChartSuppliers.Paint += new System.Windows.Forms.PaintEventHandler(this.panelChartSuppliers_Paint);
            // 
            // lblChartSupplierTitle
            // 
            this.lblChartSupplierTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChartSupplierTitle.AutoSize = true;
            this.lblChartSupplierTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblChartSupplierTitle.Location = new System.Drawing.Point(943, -2);
            this.lblChartSupplierTitle.Name = "lblChartSupplierTitle";
            this.lblChartSupplierTitle.Size = new System.Drawing.Size(247, 20);
            this.lblChartSupplierTitle.TabIndex = 11;
            this.lblChartSupplierTitle.Text = "Top 5 Suppliers by Inventory Value";
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(249)))), ((int)(((byte)(248)))));
            this.panelFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFilters.Controls.Add(this.lblAsOfDate);
            this.panelFilters.Controls.Add(this.dtpAsOfDate);
            this.panelFilters.Controls.Add(this.lblCategory);
            this.panelFilters.Controls.Add(this.cmbCategory);
            this.panelFilters.Controls.Add(this.lblBrand);
            this.panelFilters.Controls.Add(this.cmbBrand);
            this.panelFilters.Controls.Add(this.lblSupplier);
            this.panelFilters.Controls.Add(this.cmbSupplier);
            this.panelFilters.Controls.Add(this.lblLocation);
            this.panelFilters.Controls.Add(this.cmbLocation);
            this.panelFilters.Controls.Add(this.chkShowZero);
            this.panelFilters.Controls.Add(this.btnCalculate);
            this.panelFilters.Controls.Add(this.btnSnapshot);
            this.panelFilters.Controls.Add(this.btnExport);
            this.panelFilters.Controls.Add(this.progressBar);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this.panelFilters.Size = new System.Drawing.Size(1212, 52);
            this.panelFilters.TabIndex = 2;
            // 
            // lblAsOfDate
            // 
            this.lblAsOfDate.AutoSize = true;
            this.lblAsOfDate.Location = new System.Drawing.Point(6, 16);
            this.lblAsOfDate.Name = "lblAsOfDate";
            this.lblAsOfDate.Size = new System.Drawing.Size(76, 17);
            this.lblAsOfDate.TabIndex = 0;
            this.lblAsOfDate.Text = "As of Date:";
            // 
            // dtpAsOfDate
            // 
            this.dtpAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAsOfDate.Location = new System.Drawing.Point(78, 12);
            this.dtpAsOfDate.Name = "dtpAsOfDate";
            this.dtpAsOfDate.Size = new System.Drawing.Size(110, 24);
            this.dtpAsOfDate.TabIndex = 1;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(198, 16);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(70, 17);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Location = new System.Drawing.Point(266, 12);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(160, 24);
            this.cmbCategory.TabIndex = 3;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(438, 16);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(49, 17);
            this.lblBrand.TabIndex = 4;
            this.lblBrand.Text = "Brand:";
            // 
            // cmbBrand
            // 
            this.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrand.Location = new System.Drawing.Point(484, 12);
            this.cmbBrand.Name = "cmbBrand";
            this.cmbBrand.Size = new System.Drawing.Size(110, 24);
            this.cmbBrand.TabIndex = 5;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(600, 16);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(63, 17);
            this.lblSupplier.TabIndex = 6;
            this.lblSupplier.Text = "Supplier:";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.Location = new System.Drawing.Point(659, 12);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(120, 24);
            this.cmbSupplier.TabIndex = 7;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(785, 16);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(62, 17);
            this.lblLocation.TabIndex = 8;
            this.lblLocation.Text = "Location:";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.Location = new System.Drawing.Point(844, 12);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(120, 24);
            this.cmbLocation.TabIndex = 9;
            // 
            // chkShowZero
            // 
            this.chkShowZero.AutoSize = true;
            this.chkShowZero.Location = new System.Drawing.Point(970, 14);
            this.chkShowZero.Name = "chkShowZero";
            this.chkShowZero.Size = new System.Drawing.Size(135, 21);
            this.chkShowZero.TabIndex = 10;
            this.chkShowZero.Text = "Show Zero Stock";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(1110, 10);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(149, 30);
            this.btnCalculate.TabIndex = 11;
            this.btnCalculate.Text = "▶ Calculate Valuation";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Location = new System.Drawing.Point(1269, 10);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(110, 30);
            this.btnSnapshot.TabIndex = 12;
            this.btnSnapshot.Text = "📷 Snapshot";
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1389, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 30);
            this.btnExport.TabIndex = 13;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 42);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(400, 6);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 14;
            this.progressBar.Visible = false;
            // 
            // tabCogs
            // 
            this.tabCogs.Controls.Add(this.panelCogsBody);
            this.tabCogs.Controls.Add(this.panelCogsTotals);
            this.tabCogs.Controls.Add(this.panelCogsFilters);
            this.tabCogs.Location = new System.Drawing.Point(4, 25);
            this.tabCogs.Name = "tabCogs";
            this.tabCogs.Size = new System.Drawing.Size(1212, 681);
            this.tabCogs.TabIndex = 1;
            this.tabCogs.Text = "  COGS Calculation  ";
            // 
            // panelCogsBody
            // 
            this.panelCogsBody.Controls.Add(this.gridCogs);
            this.panelCogsBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCogsBody.Location = new System.Drawing.Point(0, 52);
            this.panelCogsBody.Name = "panelCogsBody";
            this.panelCogsBody.Size = new System.Drawing.Size(1212, 569);
            this.panelCogsBody.TabIndex = 0;
            // 
            // gridCogs
            // 
            this.gridCogs.AllowUserToAddRows = false;
            this.gridCogs.ColumnHeadersHeight = 29;
            this.gridCogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCCode,
            this.colCName,
            this.colCCat,
            this.colSoldQty,
            this.colCostPerUnit,
            this.colTotalCogs,
            this.colSalesVal,
            this.colGrossMargin,
            this.colVariance});
            this.gridCogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCogs.Location = new System.Drawing.Point(0, 0);
            this.gridCogs.Name = "gridCogs";
            this.gridCogs.ReadOnly = true;
            this.gridCogs.RowHeadersVisible = false;
            this.gridCogs.RowHeadersWidth = 51;
            this.gridCogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCogs.Size = new System.Drawing.Size(1212, 569);
            this.gridCogs.TabIndex = 0;
            this.gridCogs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCogs_CellFormatting);
            // 
            // colCCode
            // 
            this.colCCode.HeaderText = "Code";
            this.colCCode.MinimumWidth = 6;
            this.colCCode.Name = "colCCode";
            this.colCCode.ReadOnly = true;
            this.colCCode.Width = 90;
            // 
            // colCName
            // 
            this.colCName.HeaderText = "Product Name";
            this.colCName.MinimumWidth = 6;
            this.colCName.Name = "colCName";
            this.colCName.ReadOnly = true;
            this.colCName.Width = 200;
            // 
            // colCCat
            // 
            this.colCCat.HeaderText = "Category";
            this.colCCat.MinimumWidth = 6;
            this.colCCat.Name = "colCCat";
            this.colCCat.ReadOnly = true;
            this.colCCat.Width = 125;
            // 
            // colSoldQty
            // 
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSoldQty.DefaultCellStyle = dataGridViewCellStyle50;
            this.colSoldQty.HeaderText = "Units Sold";
            this.colSoldQty.MinimumWidth = 6;
            this.colSoldQty.Name = "colSoldQty";
            this.colSoldQty.ReadOnly = true;
            this.colSoldQty.Width = 90;
            // 
            // colCostPerUnit
            // 
            dataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colCostPerUnit.DefaultCellStyle = dataGridViewCellStyle51;
            this.colCostPerUnit.HeaderText = "Cost/Unit";
            this.colCostPerUnit.MinimumWidth = 6;
            this.colCostPerUnit.Name = "colCostPerUnit";
            this.colCostPerUnit.ReadOnly = true;
            this.colCostPerUnit.Width = 125;
            // 
            // colTotalCogs
            // 
            dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalCogs.DefaultCellStyle = dataGridViewCellStyle52;
            this.colTotalCogs.HeaderText = "Total COGS";
            this.colTotalCogs.MinimumWidth = 6;
            this.colTotalCogs.Name = "colTotalCogs";
            this.colTotalCogs.ReadOnly = true;
            this.colTotalCogs.Width = 120;
            // 
            // colSalesVal
            // 
            dataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSalesVal.DefaultCellStyle = dataGridViewCellStyle53;
            this.colSalesVal.HeaderText = "Sales Value";
            this.colSalesVal.MinimumWidth = 6;
            this.colSalesVal.Name = "colSalesVal";
            this.colSalesVal.ReadOnly = true;
            this.colSalesVal.Width = 120;
            // 
            // colGrossMargin
            // 
            dataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGrossMargin.DefaultCellStyle = dataGridViewCellStyle54;
            this.colGrossMargin.HeaderText = "Gross Margin";
            this.colGrossMargin.MinimumWidth = 6;
            this.colGrossMargin.Name = "colGrossMargin";
            this.colGrossMargin.ReadOnly = true;
            this.colGrossMargin.Width = 110;
            // 
            // colVariance
            // 
            dataGridViewCellStyle55.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colVariance.DefaultCellStyle = dataGridViewCellStyle55;
            this.colVariance.HeaderText = "Recon. Variance";
            this.colVariance.MinimumWidth = 6;
            this.colVariance.Name = "colVariance";
            this.colVariance.ReadOnly = true;
            this.colVariance.Width = 120;
            // 
            // panelCogsTotals
            // 
            this.panelCogsTotals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.panelCogsTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCogsTotals.Controls.Add(this.lblTotalCogs);
            this.panelCogsTotals.Controls.Add(this.lblTotalCogsVal);
            this.panelCogsTotals.Controls.Add(this.lblTotalSales);
            this.panelCogsTotals.Controls.Add(this.lblTotalSalesVal);
            this.panelCogsTotals.Controls.Add(this.lblGrossMargin);
            this.panelCogsTotals.Controls.Add(this.lblGrossMarginVal);
            this.panelCogsTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCogsTotals.Location = new System.Drawing.Point(0, 621);
            this.panelCogsTotals.Name = "panelCogsTotals";
            this.panelCogsTotals.Size = new System.Drawing.Size(1212, 60);
            this.panelCogsTotals.TabIndex = 1;
            // 
            // lblTotalCogs
            // 
            this.lblTotalCogs.AutoSize = true;
            this.lblTotalCogs.Location = new System.Drawing.Point(12, 12);
            this.lblTotalCogs.Name = "lblTotalCogs";
            this.lblTotalCogs.Size = new System.Drawing.Size(124, 17);
            this.lblTotalCogs.TabIndex = 0;
            this.lblTotalCogs.Text = "Grand Total COGS:";
            // 
            // lblTotalCogsVal
            // 
            this.lblTotalCogsVal.AutoSize = true;
            this.lblTotalCogsVal.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblTotalCogsVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(52)))), ((int)(((byte)(56)))));
            this.lblTotalCogsVal.Location = new System.Drawing.Point(140, 8);
            this.lblTotalCogsVal.Name = "lblTotalCogsVal";
            this.lblTotalCogsVal.Size = new System.Drawing.Size(54, 30);
            this.lblTotalCogsVal.TabIndex = 1;
            this.lblTotalCogsVal.Text = "0.00";
            // 
            // lblTotalSales
            // 
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Location = new System.Drawing.Point(340, 12);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(135, 17);
            this.lblTotalSales.TabIndex = 2;
            this.lblTotalSales.Text = "Total Sales Revenue:";
            // 
            // lblTotalSalesVal
            // 
            this.lblTotalSalesVal.AutoSize = true;
            this.lblTotalSalesVal.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblTotalSalesVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(16)))));
            this.lblTotalSalesVal.Location = new System.Drawing.Point(480, 8);
            this.lblTotalSalesVal.Name = "lblTotalSalesVal";
            this.lblTotalSalesVal.Size = new System.Drawing.Size(54, 30);
            this.lblTotalSalesVal.TabIndex = 3;
            this.lblTotalSalesVal.Text = "0.00";
            // 
            // lblGrossMargin
            // 
            this.lblGrossMargin.AutoSize = true;
            this.lblGrossMargin.Location = new System.Drawing.Point(680, 12);
            this.lblGrossMargin.Name = "lblGrossMargin";
            this.lblGrossMargin.Size = new System.Drawing.Size(91, 17);
            this.lblGrossMargin.TabIndex = 4;
            this.lblGrossMargin.Text = "Gross Margin:";
            // 
            // lblGrossMarginVal
            // 
            this.lblGrossMarginVal.AutoSize = true;
            this.lblGrossMarginVal.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblGrossMarginVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblGrossMarginVal.Location = new System.Drawing.Point(788, 8);
            this.lblGrossMarginVal.Name = "lblGrossMarginVal";
            this.lblGrossMarginVal.Size = new System.Drawing.Size(104, 30);
            this.lblGrossMarginVal.TabIndex = 5;
            this.lblGrossMarginVal.Text = "0.00 (0%)";
            // 
            // panelCogsFilters
            // 
            this.panelCogsFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(249)))), ((int)(((byte)(248)))));
            this.panelCogsFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCogsFilters.Controls.Add(this.lblFromDate);
            this.panelCogsFilters.Controls.Add(this.dtpFromDate);
            this.panelCogsFilters.Controls.Add(this.lblToDate);
            this.panelCogsFilters.Controls.Add(this.dtpToDate);
            this.panelCogsFilters.Controls.Add(this.btnCalcCogs);
            this.panelCogsFilters.Controls.Add(this.btnPostCogs);
            this.panelCogsFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCogsFilters.Location = new System.Drawing.Point(0, 0);
            this.panelCogsFilters.Name = "panelCogsFilters";
            this.panelCogsFilters.Size = new System.Drawing.Size(1212, 52);
            this.panelCogsFilters.TabIndex = 2;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(10, 16);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(45, 17);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(48, 12);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(110, 24);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.Value = new System.DateTime(2026, 8, 1, 0, 0, 0, 0);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(170, 16);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(29, 17);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(192, 12);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(110, 24);
            this.dtpToDate.TabIndex = 3;
            // 
            // btnCalcCogs
            // 
            this.btnCalcCogs.Location = new System.Drawing.Point(316, 10);
            this.btnCalcCogs.Name = "btnCalcCogs";
            this.btnCalcCogs.Size = new System.Drawing.Size(130, 30);
            this.btnCalcCogs.TabIndex = 4;
            this.btnCalcCogs.Text = "▶ Calculate COGS";
            this.btnCalcCogs.Click += new System.EventHandler(this.btnCalcCogs_Click);
            // 
            // btnPostCogs
            // 
            this.btnPostCogs.Enabled = false;
            this.btnPostCogs.Location = new System.Drawing.Point(458, 10);
            this.btnPostCogs.Name = "btnPostCogs";
            this.btnPostCogs.Size = new System.Drawing.Size(130, 30);
            this.btnPostCogs.TabIndex = 5;
            this.btnPostCogs.Text = "Post COGS Entry";
            this.btnPostCogs.Click += new System.EventHandler(this.btnPostCogs_Click);
            // 
            // frm_inventory_valuation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 776);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "frm_inventory_valuation";
            this.Text = "Inventory Valuation & COGS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_inventory_valuation_Load);
            this.panelHeader.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabValuation.ResumeLayout(false);
            this.panelValBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridValuation)).EndInit();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.tabCogs.ResumeLayout(false);
            this.panelCogsBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCogs)).EndInit();
            this.panelCogsTotals.ResumeLayout(false);
            this.panelCogsTotals.PerformLayout();
            this.panelCogsFilters.ResumeLayout(false);
            this.panelCogsFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // ---- Valuation tab ----
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabValuation;
        private System.Windows.Forms.TabPage tabCogs;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label lblAsOfDate;
        private System.Windows.Forms.DateTimePicker dtpAsOfDate;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.ComboBox cmbBrand;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.CheckBox chkShowZero;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnSnapshot;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panelValBody;
        private System.Windows.Forms.DataGridView gridValuation;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotalValueVal;
        private System.Windows.Forms.Label lblTotalSku;
        private System.Windows.Forms.Label lblTotalSkuVal;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblTotalQtyVal;
        private System.Windows.Forms.Label lblAvgCost;
        private System.Windows.Forms.Label lblAvgCostVal;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.Label lblChartTitle;
        // ---- COGS tab ----
        private System.Windows.Forms.Panel panelCogsFilters;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnCalcCogs;
        private System.Windows.Forms.Button btnPostCogs;
        private System.Windows.Forms.Panel panelCogsBody;
        private System.Windows.Forms.DataGridView gridCogs;
        private System.Windows.Forms.Panel panelCogsTotals;
        private System.Windows.Forms.Label lblTotalCogs;
        private System.Windows.Forms.Label lblTotalCogsVal;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label lblTotalSalesVal;
        private System.Windows.Forms.Label lblGrossMargin;
        private System.Windows.Forms.Label lblGrossMarginVal;
        private System.Windows.Forms.Panel panelChartSuppliers;
        private System.Windows.Forms.Label lblChartSupplierTitle;
        // ---- Grid columns ----
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastPurchDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastPurchCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReorderLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCCat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoldQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostPerUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalCogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrossMargin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVariance;
    }
}
