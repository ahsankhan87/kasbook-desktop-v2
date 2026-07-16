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
            this.panelHeader      = new System.Windows.Forms.Panel();
            this.lblFormTitle     = new System.Windows.Forms.Label();
            this.btnSettings      = new System.Windows.Forms.Button();
            this.tabMain          = new System.Windows.Forms.TabControl();
            this.tabValuation     = new System.Windows.Forms.TabPage();
            this.tabCogs          = new System.Windows.Forms.TabPage();

            // ---- Valuation tab controls ----
            this.panelFilters     = new System.Windows.Forms.Panel();
            this.lblAsOfDate      = new System.Windows.Forms.Label();
            this.dtpAsOfDate      = new System.Windows.Forms.DateTimePicker();
            this.lblCategory      = new System.Windows.Forms.Label();
            this.cmbCategory      = new System.Windows.Forms.ComboBox();
            this.lblBrand         = new System.Windows.Forms.Label();
            this.cmbBrand         = new System.Windows.Forms.ComboBox();
            this.chkShowZero      = new System.Windows.Forms.CheckBox();
            this.btnCalculate     = new System.Windows.Forms.Button();
            this.btnSnapshot      = new System.Windows.Forms.Button();
            this.btnExport        = new System.Windows.Forms.Button();
            this.progressBar      = new System.Windows.Forms.ProgressBar();

            this.panelValBody     = new System.Windows.Forms.Panel();
            this.gridValuation    = new System.Windows.Forms.DataGridView();
            this.panelSummary     = new System.Windows.Forms.Panel();
            this.lblTotalValue    = new System.Windows.Forms.Label();
            this.lblTotalValueVal = new System.Windows.Forms.Label();
            this.lblTotalSku      = new System.Windows.Forms.Label();
            this.lblTotalSkuVal   = new System.Windows.Forms.Label();
            this.lblTotalQty      = new System.Windows.Forms.Label();
            this.lblTotalQtyVal   = new System.Windows.Forms.Label();
            this.lblAvgCost       = new System.Windows.Forms.Label();
            this.lblAvgCostVal    = new System.Windows.Forms.Label();
            this.panelChart       = new System.Windows.Forms.Panel();
            this.lblChartTitle    = new System.Windows.Forms.Label();

            // ---- COGS tab controls ----
            this.panelCogsFilters  = new System.Windows.Forms.Panel();
            this.lblFromDate       = new System.Windows.Forms.Label();
            this.dtpFromDate       = new System.Windows.Forms.DateTimePicker();
            this.lblToDate         = new System.Windows.Forms.Label();
            this.dtpToDate         = new System.Windows.Forms.DateTimePicker();
            this.btnCalcCogs       = new System.Windows.Forms.Button();
            this.btnPostCogs       = new System.Windows.Forms.Button();
            this.panelCogsBody     = new System.Windows.Forms.Panel();
            this.gridCogs          = new System.Windows.Forms.DataGridView();
            this.panelCogsTotals   = new System.Windows.Forms.Panel();
            this.lblTotalCogs      = new System.Windows.Forms.Label();
            this.lblTotalCogsVal   = new System.Windows.Forms.Label();
            this.lblTotalSales     = new System.Windows.Forms.Label();
            this.lblTotalSalesVal  = new System.Windows.Forms.Label();
            this.lblGrossMargin    = new System.Windows.Forms.Label();
            this.lblGrossMarginVal = new System.Windows.Forms.Label();

            // Grid columns — Valuation
            this.colCode           = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName           = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty            = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalValue     = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastPurchDate  = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastPurchCost  = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReorderLevel   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus         = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // Grid columns — COGS
            this.colCCode          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCName          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCCat           = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoldQty        = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostPerUnit    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalCogs      = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesVal       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrossMargin    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVariance       = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.panelHeader.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabValuation.SuspendLayout();
            this.tabCogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridValuation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCogs)).BeginInit();
            this.SuspendLayout();

            // ================================================================
            // panelHeader
            // ================================================================
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 120, 212);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 50;
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Controls.Add(this.btnSettings);
            this.panelHeader.Controls.Add(this.lblFormTitle);

            this.lblFormTitle.AutoSize = false;
            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblFormTitle.Text = "Inventory Valuation & COGS";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.btnSettings.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSettings.Location = new System.Drawing.Point(1090, 10);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(110, 30);
            this.btnSettings.Text = "⚙ Settings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);

            // ================================================================
            // tabMain
            // ================================================================
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Name = "tabMain";
            this.tabMain.TabPages.Add(this.tabValuation);
            this.tabMain.TabPages.Add(this.tabCogs);

            // ================================================================
            // tabValuation
            // ================================================================
            this.tabValuation.Name = "tabValuation";
            this.tabValuation.Text = "  Inventory Valuation  ";
            this.tabValuation.Padding = new System.Windows.Forms.Padding(0);

            // panelFilters (Valuation)
            this.panelFilters.BackColor = System.Drawing.Color.FromArgb(250, 249, 248);
            this.panelFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Height = 52;
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);

            this.lblAsOfDate.AutoSize = true;
            this.lblAsOfDate.Location = new System.Drawing.Point(10, 16);
            this.lblAsOfDate.Name = "lblAsOfDate";
            this.lblAsOfDate.Text = "As of Date:";

            this.dtpAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAsOfDate.Location = new System.Drawing.Point(78, 12);
            this.dtpAsOfDate.Name = "dtpAsOfDate";
            this.dtpAsOfDate.Size = new System.Drawing.Size(110, 24);

            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(204, 16);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Text = "Category:";

            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Location = new System.Drawing.Point(266, 12);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(160, 24);

            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(438, 16);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Text = "Brand:";

            this.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrand.Location = new System.Drawing.Point(484, 12);
            this.cmbBrand.Name = "cmbBrand";
            this.cmbBrand.Size = new System.Drawing.Size(140, 24);

            this.chkShowZero.AutoSize = true;
            this.chkShowZero.Location = new System.Drawing.Point(638, 14);
            this.chkShowZero.Name = "chkShowZero";
            this.chkShowZero.Text = "Show Zero Stock";

            this.btnCalculate.Location = new System.Drawing.Point(778, 10);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(130, 30);
            this.btnCalculate.Text = "▶ Calculate Valuation";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);

            this.btnSnapshot.Location = new System.Drawing.Point(918, 10);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(110, 30);
            this.btnSnapshot.Text = "📷 Snapshot";
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);

            this.btnExport.Location = new System.Drawing.Point(1038, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 30);
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            this.progressBar.Location = new System.Drawing.Point(10, 42);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(400, 6);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.Visible = false;

            this.panelFilters.Controls.Add(this.lblAsOfDate);
            this.panelFilters.Controls.Add(this.dtpAsOfDate);
            this.panelFilters.Controls.Add(this.lblCategory);
            this.panelFilters.Controls.Add(this.cmbCategory);
            this.panelFilters.Controls.Add(this.lblBrand);
            this.panelFilters.Controls.Add(this.cmbBrand);
            this.panelFilters.Controls.Add(this.chkShowZero);
            this.panelFilters.Controls.Add(this.btnCalculate);
            this.panelFilters.Controls.Add(this.btnSnapshot);
            this.panelFilters.Controls.Add(this.btnExport);
            this.panelFilters.Controls.Add(this.progressBar);

            // panelSummary (below grid)
            this.panelSummary.BackColor = System.Drawing.Color.FromArgb(243, 242, 241);
            this.panelSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSummary.Height = 200;
            this.panelSummary.Name = "panelSummary";

            // summary labels (left half)
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalValue.Location = new System.Drawing.Point(12, 12);
            this.lblTotalValue.Text = "Total Inventory Value (PKR):";
            this.lblTotalValueVal.AutoSize = true;
            this.lblTotalValueVal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTotalValueVal.ForeColor = System.Drawing.Color.FromArgb(0, 120, 212);
            this.lblTotalValueVal.Location = new System.Drawing.Point(12, 30);
            this.lblTotalValueVal.Name = "lblTotalValueVal";
            this.lblTotalValueVal.Text = "0.00";

            this.lblTotalSku.AutoSize = true;
            this.lblTotalSku.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalSku.Location = new System.Drawing.Point(200, 12);
            this.lblTotalSku.Text = "Total SKUs:";
            this.lblTotalSkuVal.AutoSize = true;
            this.lblTotalSkuVal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTotalSkuVal.Location = new System.Drawing.Point(200, 30);
            this.lblTotalSkuVal.Name = "lblTotalSkuVal";
            this.lblTotalSkuVal.Text = "0";

            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalQty.Location = new System.Drawing.Point(340, 12);
            this.lblTotalQty.Text = "Total Qty:";
            this.lblTotalQtyVal.AutoSize = true;
            this.lblTotalQtyVal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTotalQtyVal.Location = new System.Drawing.Point(340, 30);
            this.lblTotalQtyVal.Name = "lblTotalQtyVal";
            this.lblTotalQtyVal.Text = "0";

            this.lblAvgCost.AutoSize = true;
            this.lblAvgCost.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAvgCost.Location = new System.Drawing.Point(480, 12);
            this.lblAvgCost.Text = "Avg Cost/Unit:";
            this.lblAvgCostVal.AutoSize = true;
            this.lblAvgCostVal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblAvgCostVal.Location = new System.Drawing.Point(480, 30);
            this.lblAvgCostVal.Name = "lblAvgCostVal";
            this.lblAvgCostVal.Text = "0.00";

            // chart panel (right half of summary)
            this.panelChart.BackColor = System.Drawing.Color.White;
            this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChart.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.panelChart.Location = new System.Drawing.Point(660, 8);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(460, 180);
            this.panelChart.Paint += new System.Windows.Forms.PaintEventHandler(this.panelChart_Paint);

            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblChartTitle.Location = new System.Drawing.Point(660, 190);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Text = "Top 5 Categories by Value";

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

            // panelValBody (contains grid)
            this.panelValBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelValBody.Name = "panelValBody";
            this.panelValBody.Controls.Add(this.gridValuation);

            // ---- gridValuation columns ----
            this.colCode.HeaderText          = "Code";           this.colCode.Name          = "colCode";          this.colCode.Width          = 90;
            this.colName.HeaderText          = "Product Name";   this.colName.Name          = "colName";          this.colName.Width          = 220;
            this.colCategory.HeaderText      = "Category";       this.colCategory.Name      = "colCategory";      this.colCategory.Width      = 110;
            this.colQty.HeaderText           = "Qty on Hand";    this.colQty.Name           = "colQty";           this.colQty.Width           = 90;  this.colQty.DefaultCellStyle.Alignment           = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colUnitCost.HeaderText      = "Unit Cost";      this.colUnitCost.Name      = "colUnitCost";      this.colUnitCost.Width      = 100; this.colUnitCost.DefaultCellStyle.Alignment      = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalValue.HeaderText    = "Total Value";    this.colTotalValue.Name    = "colTotalValue";    this.colTotalValue.Width    = 120; this.colTotalValue.DefaultCellStyle.Alignment    = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colLastPurchDate.HeaderText = "Last Purchase";  this.colLastPurchDate.Name = "colLastPurchDate"; this.colLastPurchDate.Width = 100;
            this.colLastPurchCost.HeaderText = "Last P. Cost";   this.colLastPurchCost.Name = "colLastPurchCost"; this.colLastPurchCost.Width = 90;  this.colLastPurchCost.DefaultCellStyle.Alignment  = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colReorderLevel.HeaderText  = "Reorder Lvl";   this.colReorderLevel.Name  = "colReorderLevel";  this.colReorderLevel.Width  = 90;  this.colReorderLevel.DefaultCellStyle.Alignment  = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colStatus.HeaderText        = "Status";         this.colStatus.Name        = "colStatus";        this.colStatus.Width        = 110;

            this.gridValuation.Columns.AddRange(
                this.colCode, this.colName, this.colCategory, this.colQty,
                this.colUnitCost, this.colTotalValue, this.colLastPurchDate,
                this.colLastPurchCost, this.colReorderLevel, this.colStatus);

            this.gridValuation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridValuation.Name = "gridValuation";
            this.gridValuation.AutoGenerateColumns = false;
            this.gridValuation.ReadOnly = true;
            this.gridValuation.AllowUserToAddRows = false;
            this.gridValuation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridValuation.MultiSelect = false;
            this.gridValuation.RowHeadersVisible = false;
            this.gridValuation.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridValuation_CellFormatting);

            this.tabValuation.Controls.Add(this.panelValBody);
            this.tabValuation.Controls.Add(this.panelSummary);
            this.tabValuation.Controls.Add(this.panelFilters);

            // ================================================================
            // tabCogs
            // ================================================================
            this.tabCogs.Name = "tabCogs";
            this.tabCogs.Text = "  COGS Calculation  ";

            // panelCogsFilters
            this.panelCogsFilters.BackColor = System.Drawing.Color.FromArgb(250, 249, 248);
            this.panelCogsFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCogsFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCogsFilters.Height = 52;
            this.panelCogsFilters.Name = "panelCogsFilters";

            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(10, 16);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Text = "From:";

            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(48, 12);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(110, 24);
            this.dtpFromDate.Value = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(170, 16);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Text = "To:";

            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(192, 12);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(110, 24);

            this.btnCalcCogs.Location = new System.Drawing.Point(316, 10);
            this.btnCalcCogs.Name = "btnCalcCogs";
            this.btnCalcCogs.Size = new System.Drawing.Size(130, 30);
            this.btnCalcCogs.Text = "▶ Calculate COGS";
            this.btnCalcCogs.Click += new System.EventHandler(this.btnCalcCogs_Click);

            this.btnPostCogs.Location = new System.Drawing.Point(458, 10);
            this.btnPostCogs.Name = "btnPostCogs";
            this.btnPostCogs.Size = new System.Drawing.Size(130, 30);
            this.btnPostCogs.Text = "Post COGS Entry";
            this.btnPostCogs.Enabled = false;
            this.btnPostCogs.Click += new System.EventHandler(this.btnPostCogs_Click);

            this.panelCogsFilters.Controls.Add(this.lblFromDate);
            this.panelCogsFilters.Controls.Add(this.dtpFromDate);
            this.panelCogsFilters.Controls.Add(this.lblToDate);
            this.panelCogsFilters.Controls.Add(this.dtpToDate);
            this.panelCogsFilters.Controls.Add(this.btnCalcCogs);
            this.panelCogsFilters.Controls.Add(this.btnPostCogs);

            // panelCogsTotals
            this.panelCogsTotals.BackColor = System.Drawing.Color.FromArgb(243, 242, 241);
            this.panelCogsTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCogsTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCogsTotals.Height = 60;
            this.panelCogsTotals.Name = "panelCogsTotals";

            this.lblTotalCogs.AutoSize = true;
            this.lblTotalCogs.Location = new System.Drawing.Point(12, 12);
            this.lblTotalCogs.Name = "lblTotalCogs";
            this.lblTotalCogs.Text = "Grand Total COGS:";

            this.lblTotalCogsVal.AutoSize = true;
            this.lblTotalCogsVal.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblTotalCogsVal.ForeColor = System.Drawing.Color.FromArgb(209, 52, 56);
            this.lblTotalCogsVal.Location = new System.Drawing.Point(140, 8);
            this.lblTotalCogsVal.Name = "lblTotalCogsVal";
            this.lblTotalCogsVal.Text = "0.00";

            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Location = new System.Drawing.Point(340, 12);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Text = "Total Sales Revenue:";

            this.lblTotalSalesVal.AutoSize = true;
            this.lblTotalSalesVal.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblTotalSalesVal.ForeColor = System.Drawing.Color.FromArgb(16, 124, 16);
            this.lblTotalSalesVal.Location = new System.Drawing.Point(480, 8);
            this.lblTotalSalesVal.Name = "lblTotalSalesVal";
            this.lblTotalSalesVal.Text = "0.00";

            this.lblGrossMargin.AutoSize = true;
            this.lblGrossMargin.Location = new System.Drawing.Point(680, 12);
            this.lblGrossMargin.Name = "lblGrossMargin";
            this.lblGrossMargin.Text = "Gross Margin:";

            this.lblGrossMarginVal.AutoSize = true;
            this.lblGrossMarginVal.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblGrossMarginVal.ForeColor = System.Drawing.Color.FromArgb(0, 120, 212);
            this.lblGrossMarginVal.Location = new System.Drawing.Point(788, 8);
            this.lblGrossMarginVal.Name = "lblGrossMarginVal";
            this.lblGrossMarginVal.Text = "0.00 (0%)";

            this.panelCogsTotals.Controls.Add(this.lblTotalCogs);
            this.panelCogsTotals.Controls.Add(this.lblTotalCogsVal);
            this.panelCogsTotals.Controls.Add(this.lblTotalSales);
            this.panelCogsTotals.Controls.Add(this.lblTotalSalesVal);
            this.panelCogsTotals.Controls.Add(this.lblGrossMargin);
            this.panelCogsTotals.Controls.Add(this.lblGrossMarginVal);

            // panelCogsBody
            this.panelCogsBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCogsBody.Name = "panelCogsBody";
            this.panelCogsBody.Controls.Add(this.gridCogs);

            // ---- gridCogs columns ----
            this.colCCode.HeaderText       = "Code";             this.colCCode.Name       = "colCCode";       this.colCCode.Width       = 90;
            this.colCName.HeaderText       = "Product Name";     this.colCName.Name       = "colCName";       this.colCName.Width       = 200;
            this.colCCat.HeaderText        = "Category";         this.colCCat.Name        = "colCCat";        this.colCCat.Width        = 100;
            this.colSoldQty.HeaderText     = "Units Sold";       this.colSoldQty.Name     = "colSoldQty";     this.colSoldQty.Width     = 90;  this.colSoldQty.DefaultCellStyle.Alignment     = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colCostPerUnit.HeaderText = "Cost/Unit";        this.colCostPerUnit.Name = "colCostPerUnit"; this.colCostPerUnit.Width = 100; this.colCostPerUnit.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalCogs.HeaderText   = "Total COGS";       this.colTotalCogs.Name   = "colTotalCogs";   this.colTotalCogs.Width   = 120; this.colTotalCogs.DefaultCellStyle.Alignment   = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSalesVal.HeaderText    = "Sales Value";      this.colSalesVal.Name    = "colSalesVal";    this.colSalesVal.Width    = 120; this.colSalesVal.DefaultCellStyle.Alignment    = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGrossMargin.HeaderText = "Gross Margin";     this.colGrossMargin.Name = "colGrossMargin"; this.colGrossMargin.Width = 110; this.colGrossMargin.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colVariance.HeaderText    = "Recon. Variance";  this.colVariance.Name    = "colVariance";    this.colVariance.Width    = 120; this.colVariance.DefaultCellStyle.Alignment    = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            this.gridCogs.Columns.AddRange(
                this.colCCode, this.colCName, this.colCCat, this.colSoldQty,
                this.colCostPerUnit, this.colTotalCogs, this.colSalesVal,
                this.colGrossMargin, this.colVariance);

            this.gridCogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCogs.Name = "gridCogs";
            this.gridCogs.AutoGenerateColumns = false;
            this.gridCogs.ReadOnly = true;
            this.gridCogs.AllowUserToAddRows = false;
            this.gridCogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCogs.RowHeadersVisible = false;
            this.gridCogs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCogs_CellFormatting);

            this.tabCogs.Controls.Add(this.panelCogsBody);
            this.tabCogs.Controls.Add(this.panelCogsTotals);
            this.tabCogs.Controls.Add(this.panelCogsFilters);

            // ================================================================
            // frm_inventory_valuation
            // ================================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 760);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "frm_inventory_valuation";
            this.Text = "Inventory Valuation & COGS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_inventory_valuation_Load);
            this.panelHeader.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabValuation.ResumeLayout(false);
            this.tabCogs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridValuation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCogs)).EndInit();
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
