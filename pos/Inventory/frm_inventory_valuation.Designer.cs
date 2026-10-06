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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_inventory_valuation));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.chkShowNegativeStock = new System.Windows.Forms.CheckBox();
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
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            // 
            // btnSettings
            // 
            resources.ApplyResources(this.btnSettings, "btnSettings");
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lblFormTitle
            // 
            resources.ApplyResources(this.lblFormTitle, "lblFormTitle");
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Name = "lblFormTitle";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabValuation);
            this.tabMain.Controls.Add(this.tabCogs);
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            // 
            // tabValuation
            // 
            this.tabValuation.Controls.Add(this.panelValBody);
            this.tabValuation.Controls.Add(this.panelSummary);
            this.tabValuation.Controls.Add(this.panelFilters);
            resources.ApplyResources(this.tabValuation, "tabValuation");
            this.tabValuation.Name = "tabValuation";
            // 
            // panelValBody
            // 
            this.panelValBody.Controls.Add(this.gridValuation);
            resources.ApplyResources(this.panelValBody, "panelValBody");
            this.panelValBody.Name = "panelValBody";
            // 
            // gridValuation
            // 
            this.gridValuation.AllowUserToAddRows = false;
            resources.ApplyResources(this.gridValuation, "gridValuation");
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
            this.gridValuation.MultiSelect = false;
            this.gridValuation.Name = "gridValuation";
            this.gridValuation.ReadOnly = true;
            this.gridValuation.RowHeadersVisible = false;
            this.gridValuation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridValuation.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridValuation_CellFormatting);
            // 
            // colCode
            // 
            resources.ApplyResources(this.colCode, "colCode");
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colName
            // 
            resources.ApplyResources(this.colName, "colName");
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colCategory
            // 
            resources.ApplyResources(this.colCategory, "colCategory");
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colQty
            // 
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colQty.DefaultCellStyle = dataGridViewCellStyle34;
            resources.ApplyResources(this.colQty, "colQty");
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            // 
            // colUnitCost
            // 
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colUnitCost.DefaultCellStyle = dataGridViewCellStyle35;
            resources.ApplyResources(this.colUnitCost, "colUnitCost");
            this.colUnitCost.Name = "colUnitCost";
            this.colUnitCost.ReadOnly = true;
            // 
            // colTotalValue
            // 
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalValue.DefaultCellStyle = dataGridViewCellStyle36;
            resources.ApplyResources(this.colTotalValue, "colTotalValue");
            this.colTotalValue.Name = "colTotalValue";
            this.colTotalValue.ReadOnly = true;
            // 
            // colLastPurchDate
            // 
            resources.ApplyResources(this.colLastPurchDate, "colLastPurchDate");
            this.colLastPurchDate.Name = "colLastPurchDate";
            this.colLastPurchDate.ReadOnly = true;
            // 
            // colLastPurchCost
            // 
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colLastPurchCost.DefaultCellStyle = dataGridViewCellStyle37;
            resources.ApplyResources(this.colLastPurchCost, "colLastPurchCost");
            this.colLastPurchCost.Name = "colLastPurchCost";
            this.colLastPurchCost.ReadOnly = true;
            // 
            // colReorderLevel
            // 
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colReorderLevel.DefaultCellStyle = dataGridViewCellStyle38;
            resources.ApplyResources(this.colReorderLevel, "colReorderLevel");
            this.colReorderLevel.Name = "colReorderLevel";
            this.colReorderLevel.ReadOnly = true;
            // 
            // colStatus
            // 
            resources.ApplyResources(this.colStatus, "colStatus");
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
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
            resources.ApplyResources(this.panelSummary, "panelSummary");
            this.panelSummary.Name = "panelSummary";
            // 
            // lblTotalValue
            // 
            resources.ApplyResources(this.lblTotalValue, "lblTotalValue");
            this.lblTotalValue.Name = "lblTotalValue";
            // 
            // lblTotalValueVal
            // 
            resources.ApplyResources(this.lblTotalValueVal, "lblTotalValueVal");
            this.lblTotalValueVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblTotalValueVal.Name = "lblTotalValueVal";
            // 
            // lblTotalSku
            // 
            resources.ApplyResources(this.lblTotalSku, "lblTotalSku");
            this.lblTotalSku.Name = "lblTotalSku";
            // 
            // lblTotalSkuVal
            // 
            resources.ApplyResources(this.lblTotalSkuVal, "lblTotalSkuVal");
            this.lblTotalSkuVal.Name = "lblTotalSkuVal";
            // 
            // lblTotalQty
            // 
            resources.ApplyResources(this.lblTotalQty, "lblTotalQty");
            this.lblTotalQty.Name = "lblTotalQty";
            // 
            // lblTotalQtyVal
            // 
            resources.ApplyResources(this.lblTotalQtyVal, "lblTotalQtyVal");
            this.lblTotalQtyVal.Name = "lblTotalQtyVal";
            // 
            // lblAvgCost
            // 
            resources.ApplyResources(this.lblAvgCost, "lblAvgCost");
            this.lblAvgCost.Name = "lblAvgCost";
            // 
            // lblAvgCostVal
            // 
            resources.ApplyResources(this.lblAvgCostVal, "lblAvgCostVal");
            this.lblAvgCostVal.Name = "lblAvgCostVal";
            // 
            // panelChart
            // 
            resources.ApplyResources(this.panelChart, "panelChart");
            this.panelChart.BackColor = System.Drawing.Color.White;
            this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChart.Name = "panelChart";
            this.panelChart.Paint += new System.Windows.Forms.PaintEventHandler(this.panelChart_Paint);
            // 
            // lblChartTitle
            // 
            resources.ApplyResources(this.lblChartTitle, "lblChartTitle");
            this.lblChartTitle.Name = "lblChartTitle";
            // 
            // panelChartSuppliers
            // 
            resources.ApplyResources(this.panelChartSuppliers, "panelChartSuppliers");
            this.panelChartSuppliers.BackColor = System.Drawing.Color.White;
            this.panelChartSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChartSuppliers.Name = "panelChartSuppliers";
            this.panelChartSuppliers.Paint += new System.Windows.Forms.PaintEventHandler(this.panelChartSuppliers_Paint);
            // 
            // lblChartSupplierTitle
            // 
            resources.ApplyResources(this.lblChartSupplierTitle, "lblChartSupplierTitle");
            this.lblChartSupplierTitle.Name = "lblChartSupplierTitle";
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
            this.panelFilters.Controls.Add(this.chkShowNegativeStock);
            this.panelFilters.Controls.Add(this.chkShowZero);
            this.panelFilters.Controls.Add(this.btnCalculate);
            this.panelFilters.Controls.Add(this.btnSnapshot);
            this.panelFilters.Controls.Add(this.btnExport);
            this.panelFilters.Controls.Add(this.progressBar);
            resources.ApplyResources(this.panelFilters, "panelFilters");
            this.panelFilters.Name = "panelFilters";
            // 
            // lblAsOfDate
            // 
            resources.ApplyResources(this.lblAsOfDate, "lblAsOfDate");
            this.lblAsOfDate.Name = "lblAsOfDate";
            // 
            // dtpAsOfDate
            // 
            this.dtpAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpAsOfDate, "dtpAsOfDate");
            this.dtpAsOfDate.Name = "dtpAsOfDate";
            // 
            // lblCategory
            // 
            resources.ApplyResources(this.lblCategory, "lblCategory");
            this.lblCategory.Name = "lblCategory";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbCategory, "cmbCategory");
            this.cmbCategory.Name = "cmbCategory";
            // 
            // lblBrand
            // 
            resources.ApplyResources(this.lblBrand, "lblBrand");
            this.lblBrand.Name = "lblBrand";
            // 
            // cmbBrand
            // 
            this.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbBrand, "cmbBrand");
            this.cmbBrand.Name = "cmbBrand";
            // 
            // lblSupplier
            // 
            resources.ApplyResources(this.lblSupplier, "lblSupplier");
            this.lblSupplier.Name = "lblSupplier";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbSupplier, "cmbSupplier");
            this.cmbSupplier.Name = "cmbSupplier";
            // 
            // lblLocation
            // 
            resources.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.Name = "cmbLocation";
            // 
            // chkShowNegativeStock
            // 
            resources.ApplyResources(this.chkShowNegativeStock, "chkShowNegativeStock");
            this.chkShowNegativeStock.Name = "chkShowNegativeStock";
            this.chkShowNegativeStock.CheckedChanged += new System.EventHandler(this.chkShowNegativeStock_CheckedChanged);
            // 
            // chkShowZero
            // 
            resources.ApplyResources(this.chkShowZero, "chkShowZero");
            this.chkShowZero.Name = "chkShowZero";
            this.chkShowZero.CheckedChanged += new System.EventHandler(this.chkShowZero_CheckedChanged);
            // 
            // btnCalculate
            // 
            resources.ApplyResources(this.btnCalculate, "btnCalculate");
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnSnapshot
            // 
            resources.ApplyResources(this.btnSnapshot, "btnSnapshot");
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // tabCogs
            // 
            this.tabCogs.Controls.Add(this.panelCogsBody);
            this.tabCogs.Controls.Add(this.panelCogsTotals);
            this.tabCogs.Controls.Add(this.panelCogsFilters);
            resources.ApplyResources(this.tabCogs, "tabCogs");
            this.tabCogs.Name = "tabCogs";
            // 
            // panelCogsBody
            // 
            this.panelCogsBody.Controls.Add(this.gridCogs);
            resources.ApplyResources(this.panelCogsBody, "panelCogsBody");
            this.panelCogsBody.Name = "panelCogsBody";
            // 
            // gridCogs
            // 
            this.gridCogs.AllowUserToAddRows = false;
            resources.ApplyResources(this.gridCogs, "gridCogs");
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
            this.gridCogs.Name = "gridCogs";
            this.gridCogs.ReadOnly = true;
            this.gridCogs.RowHeadersVisible = false;
            this.gridCogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCogs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCogs_CellFormatting);
            // 
            // colCCode
            // 
            resources.ApplyResources(this.colCCode, "colCCode");
            this.colCCode.Name = "colCCode";
            this.colCCode.ReadOnly = true;
            // 
            // colCName
            // 
            resources.ApplyResources(this.colCName, "colCName");
            this.colCName.Name = "colCName";
            this.colCName.ReadOnly = true;
            // 
            // colCCat
            // 
            resources.ApplyResources(this.colCCat, "colCCat");
            this.colCCat.Name = "colCCat";
            this.colCCat.ReadOnly = true;
            // 
            // colSoldQty
            // 
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSoldQty.DefaultCellStyle = dataGridViewCellStyle39;
            resources.ApplyResources(this.colSoldQty, "colSoldQty");
            this.colSoldQty.Name = "colSoldQty";
            this.colSoldQty.ReadOnly = true;
            // 
            // colCostPerUnit
            // 
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colCostPerUnit.DefaultCellStyle = dataGridViewCellStyle40;
            resources.ApplyResources(this.colCostPerUnit, "colCostPerUnit");
            this.colCostPerUnit.Name = "colCostPerUnit";
            this.colCostPerUnit.ReadOnly = true;
            // 
            // colTotalCogs
            // 
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalCogs.DefaultCellStyle = dataGridViewCellStyle41;
            resources.ApplyResources(this.colTotalCogs, "colTotalCogs");
            this.colTotalCogs.Name = "colTotalCogs";
            this.colTotalCogs.ReadOnly = true;
            // 
            // colSalesVal
            // 
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSalesVal.DefaultCellStyle = dataGridViewCellStyle42;
            resources.ApplyResources(this.colSalesVal, "colSalesVal");
            this.colSalesVal.Name = "colSalesVal";
            this.colSalesVal.ReadOnly = true;
            // 
            // colGrossMargin
            // 
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGrossMargin.DefaultCellStyle = dataGridViewCellStyle43;
            resources.ApplyResources(this.colGrossMargin, "colGrossMargin");
            this.colGrossMargin.Name = "colGrossMargin";
            this.colGrossMargin.ReadOnly = true;
            // 
            // colVariance
            // 
            dataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colVariance.DefaultCellStyle = dataGridViewCellStyle44;
            resources.ApplyResources(this.colVariance, "colVariance");
            this.colVariance.Name = "colVariance";
            this.colVariance.ReadOnly = true;
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
            resources.ApplyResources(this.panelCogsTotals, "panelCogsTotals");
            this.panelCogsTotals.Name = "panelCogsTotals";
            // 
            // lblTotalCogs
            // 
            resources.ApplyResources(this.lblTotalCogs, "lblTotalCogs");
            this.lblTotalCogs.Name = "lblTotalCogs";
            // 
            // lblTotalCogsVal
            // 
            resources.ApplyResources(this.lblTotalCogsVal, "lblTotalCogsVal");
            this.lblTotalCogsVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(52)))), ((int)(((byte)(56)))));
            this.lblTotalCogsVal.Name = "lblTotalCogsVal";
            // 
            // lblTotalSales
            // 
            resources.ApplyResources(this.lblTotalSales, "lblTotalSales");
            this.lblTotalSales.Name = "lblTotalSales";
            // 
            // lblTotalSalesVal
            // 
            resources.ApplyResources(this.lblTotalSalesVal, "lblTotalSalesVal");
            this.lblTotalSalesVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(16)))));
            this.lblTotalSalesVal.Name = "lblTotalSalesVal";
            // 
            // lblGrossMargin
            // 
            resources.ApplyResources(this.lblGrossMargin, "lblGrossMargin");
            this.lblGrossMargin.Name = "lblGrossMargin";
            // 
            // lblGrossMarginVal
            // 
            resources.ApplyResources(this.lblGrossMarginVal, "lblGrossMarginVal");
            this.lblGrossMarginVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.lblGrossMarginVal.Name = "lblGrossMarginVal";
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
            resources.ApplyResources(this.panelCogsFilters, "panelCogsFilters");
            this.panelCogsFilters.Name = "panelCogsFilters";
            // 
            // lblFromDate
            // 
            resources.ApplyResources(this.lblFromDate, "lblFromDate");
            this.lblFromDate.Name = "lblFromDate";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Value = new System.DateTime(2026, 8, 1, 0, 0, 0, 0);
            // 
            // lblToDate
            // 
            resources.ApplyResources(this.lblToDate, "lblToDate");
            this.lblToDate.Name = "lblToDate";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            // 
            // btnCalcCogs
            // 
            resources.ApplyResources(this.btnCalcCogs, "btnCalcCogs");
            this.btnCalcCogs.Name = "btnCalcCogs";
            this.btnCalcCogs.Click += new System.EventHandler(this.btnCalcCogs_Click);
            // 
            // btnPostCogs
            // 
            resources.ApplyResources(this.btnPostCogs, "btnPostCogs");
            this.btnPostCogs.Name = "btnPostCogs";
            this.btnPostCogs.Click += new System.EventHandler(this.btnPostCogs_Click);
            // 
            // frm_inventory_valuation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "frm_inventory_valuation";
            this.ShowIcon = false;
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
        private System.Windows.Forms.CheckBox chkShowNegativeStock;
    }
}
