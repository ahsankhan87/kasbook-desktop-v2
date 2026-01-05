namespace pos.Sales
{
    partial class frm_pos_sale
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnLang = new System.Windows.Forms.Button();
            this.lblClock = new System.Windows.Forms.Label();
            this.lblBranch = new System.Windows.Forms.Label();
            this.lblBranchCaption = new System.Windows.Forms.Label();
            this.lblCashier = new System.Windows.Forms.Label();
            this.lblCashierCaption = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.flpTiles = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearchFocus = new System.Windows.Forms.Button();
            this.btnBarcodeFocus = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.gridCart = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaxRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlCartButtons = new System.Windows.Forms.Panel();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.btnQtyMinus = new System.Windows.Forms.Button();
            this.btnQtyPlus = new System.Windows.Forms.Button();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.btnNewSale = new System.Windows.Forms.Button();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.lblGrandCaption = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblDiscountCaption = new System.Windows.Forms.Label();
            this.lblTax = new System.Windows.Forms.Label();
            this.lblTaxCaption = new System.Windows.Forms.Label();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lblSubtotalCaption = new System.Windows.Forms.Label();
            this.pnlPay = new System.Windows.Forms.Panel();
            this.btnPayMixed = new System.Windows.Forms.Button();
            this.btnPayCard = new System.Windows.Forms.Button();
            this.btnPayCash = new System.Windows.Forms.Button();
            this.btnRecall = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCart)).BeginInit();
            this.pnlCartButtons.SuspendLayout();
            this.pnlTotals.SuspendLayout();
            this.pnlPay.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(31)))), ((int)(((byte)(39)))));
            this.pnlTop.Controls.Add(this.btnLang);
            this.pnlTop.Controls.Add(this.lblClock);
            this.pnlTop.Controls.Add(this.lblBranch);
            this.pnlTop.Controls.Add(this.lblBranchCaption);
            this.pnlTop.Controls.Add(this.lblCashier);
            this.pnlTop.Controls.Add(this.lblCashierCaption);
            this.pnlTop.Controls.Add(this.lblHeader);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1366, 56);
            this.pnlTop.TabIndex = 0;
            // 
            // btnLang
            // 
            this.btnLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.btnLang.FlatAppearance.BorderSize = 0;
            this.btnLang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLang.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLang.ForeColor = System.Drawing.Color.White;
            this.btnLang.Location = new System.Drawing.Point(1294, 10);
            this.btnLang.Name = "btnLang";
            this.btnLang.Size = new System.Drawing.Size(60, 36);
            this.btnLang.TabIndex = 6;
            this.btnLang.Text = "AR";
            this.btnLang.UseVisualStyleBackColor = false;
            this.btnLang.Click += new System.EventHandler(this.btnLang_Click);
            // 
            // lblClock
            // 
            this.lblClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblClock.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblClock.Location = new System.Drawing.Point(1040, 16);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(177, 25);
            this.lblClock.TabIndex = 5;
            this.lblClock.Text = "01 Jan 2026  12:00";
            // 
            // lblBranch
            // 
            this.lblBranch.AutoSize = true;
            this.lblBranch.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblBranch.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblBranch.Location = new System.Drawing.Point(428, 16);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.Size = new System.Drawing.Size(23, 25);
            this.lblBranch.TabIndex = 4;
            this.lblBranch.Text = "0";
            // 
            // lblBranchCaption
            // 
            this.lblBranchCaption.AutoSize = true;
            this.lblBranchCaption.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBranchCaption.ForeColor = System.Drawing.Color.Gray;
            this.lblBranchCaption.Location = new System.Drawing.Point(372, 18);
            this.lblBranchCaption.Name = "lblBranchCaption";
            this.lblBranchCaption.Size = new System.Drawing.Size(67, 23);
            this.lblBranchCaption.TabIndex = 3;
            this.lblBranchCaption.Text = "Branch:";
            // 
            // lblCashier
            // 
            this.lblCashier.AutoSize = true;
            this.lblCashier.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCashier.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCashier.Location = new System.Drawing.Point(293, 16);
            this.lblCashier.Name = "lblCashier";
            this.lblCashier.Size = new System.Drawing.Size(23, 25);
            this.lblCashier.TabIndex = 2;
            this.lblCashier.Text = "0";
            // 
            // lblCashierCaption
            // 
            this.lblCashierCaption.AutoSize = true;
            this.lblCashierCaption.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCashierCaption.ForeColor = System.Drawing.Color.Gray;
            this.lblCashierCaption.Location = new System.Drawing.Point(235, 18);
            this.lblCashierCaption.Name = "lblCashierCaption";
            this.lblCashierCaption.Size = new System.Drawing.Size(70, 23);
            this.lblCashierCaption.TabIndex = 1;
            this.lblCashierCaption.Text = "Cashier:";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(12, 12);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(179, 37);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Point of Sale";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 56);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1366, 712);
            this.pnlMain.TabIndex = 1;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.pnlLeft);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlRight);
            this.splitMain.Size = new System.Drawing.Size(1366, 712);
            this.splitMain.SplitterDistance = 835;
            this.splitMain.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.flpTiles);
            this.pnlLeft.Controls.Add(this.pnlSearch);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(835, 712);
            this.pnlLeft.TabIndex = 0;
            // 
            // flpTiles
            // 
            this.flpTiles.AutoScroll = true;
            this.flpTiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.flpTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTiles.Location = new System.Drawing.Point(0, 88);
            this.flpTiles.Name = "flpTiles";
            this.flpTiles.Padding = new System.Windows.Forms.Padding(10);
            this.flpTiles.Size = new System.Drawing.Size(835, 624);
            this.flpTiles.TabIndex = 1;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.btnSearchFocus);
            this.pnlSearch.Controls.Add(this.btnBarcodeFocus);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.txtBarcode);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Controls.Add(this.lblBarcode);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(835, 88);
            this.pnlSearch.TabIndex = 0;
            // 
            // btnSearchFocus
            // 
            this.btnSearchFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(120)))), ((int)(((byte)(198)))));
            this.btnSearchFocus.FlatAppearance.BorderSize = 0;
            this.btnSearchFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchFocus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearchFocus.ForeColor = System.Drawing.Color.White;
            this.btnSearchFocus.Location = new System.Drawing.Point(734, 46);
            this.btnSearchFocus.Name = "btnSearchFocus";
            this.btnSearchFocus.Size = new System.Drawing.Size(89, 34);
            this.btnSearchFocus.TabIndex = 5;
            this.btnSearchFocus.Text = "Search";
            this.btnSearchFocus.UseVisualStyleBackColor = false;
            this.btnSearchFocus.Click += new System.EventHandler(this.btnSearchFocus_Click);
            // 
            // btnBarcodeFocus
            // 
            this.btnBarcodeFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBarcodeFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(91)))));
            this.btnBarcodeFocus.FlatAppearance.BorderSize = 0;
            this.btnBarcodeFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBarcodeFocus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBarcodeFocus.ForeColor = System.Drawing.Color.White;
            this.btnBarcodeFocus.Location = new System.Drawing.Point(734, 6);
            this.btnBarcodeFocus.Name = "btnBarcodeFocus";
            this.btnBarcodeFocus.Size = new System.Drawing.Size(89, 34);
            this.btnBarcodeFocus.TabIndex = 4;
            this.btnBarcodeFocus.Text = "Barcode";
            this.btnBarcodeFocus.UseVisualStyleBackColor = false;
            this.btnBarcodeFocus.Click += new System.EventHandler(this.btnBarcodeFocus_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtSearch.Location = new System.Drawing.Point(112, 47);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(610, 39);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtBarcode.Location = new System.Drawing.Point(112, 7);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(610, 39);
            this.txtBarcode.TabIndex = 2;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.lblSearch.Location = new System.Drawing.Point(12, 53);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(80, 28);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Search:";
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.lblBarcode.Location = new System.Drawing.Point(12, 13);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(94, 28);
            this.lblBarcode.TabIndex = 0;
            this.lblBarcode.Text = "Barcode:";
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.gridCart);
            this.pnlRight.Controls.Add(this.pnlCartButtons);
            this.pnlRight.Controls.Add(this.pnlTotals);
            this.pnlRight.Controls.Add(this.pnlPay);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(0, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(527, 712);
            this.pnlRight.TabIndex = 0;
            // 
            // gridCart
            // 
            this.gridCart.AllowUserToAddRows = false;
            this.gridCart.AllowUserToDeleteRows = false;
            this.gridCart.AllowUserToResizeRows = false;
            this.gridCart.BackgroundColor = System.Drawing.Color.White;
            this.gridCart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridCart.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(31)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(31)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridCart.ColumnHeadersHeight = 38;
            this.gridCart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colItemNumber,
            this.colName,
            this.colQty,
            this.colPrice,
            this.colTaxRate,
            this.colTotal});
            this.gridCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCart.EnableHeadersVisualStyles = false;
            this.gridCart.Location = new System.Drawing.Point(0, 0);
            this.gridCart.MultiSelect = false;
            this.gridCart.Name = "gridCart";
            this.gridCart.ReadOnly = true;
            this.gridCart.RowHeadersVisible = false;
            this.gridCart.RowHeadersWidth = 51;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCart.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.gridCart.RowTemplate.Height = 38;
            this.gridCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCart.Size = new System.Drawing.Size(527, 354);
            this.gridCart.TabIndex = 0;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "Code";
            this.colCode.MinimumWidth = 6;
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 90;
            // 
            // colItemNumber
            // 
            this.colItemNumber.HeaderText = "ItemNumber";
            this.colItemNumber.MinimumWidth = 6;
            this.colItemNumber.Name = "colItemNumber";
            this.colItemNumber.ReadOnly = true;
            this.colItemNumber.Visible = false;
            this.colItemNumber.Width = 125;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.HeaderText = "Item";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colQty
            // 
            this.colQty.HeaderText = "Qty";
            this.colQty.MinimumWidth = 6;
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            this.colQty.Width = 60;
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "Price";
            this.colPrice.MinimumWidth = 6;
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            this.colPrice.Width = 80;
            // 
            // colTaxRate
            // 
            this.colTaxRate.HeaderText = "TaxRate";
            this.colTaxRate.MinimumWidth = 6;
            this.colTaxRate.Name = "colTaxRate";
            this.colTaxRate.ReadOnly = true;
            this.colTaxRate.Visible = false;
            this.colTaxRate.Width = 125;
            // 
            // colTotal
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotal.DefaultCellStyle = dataGridViewCellStyle5;
            this.colTotal.HeaderText = "Total";
            this.colTotal.MinimumWidth = 6;
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            this.colTotal.Width = 90;
            // 
            // pnlCartButtons
            // 
            this.pnlCartButtons.BackColor = System.Drawing.Color.White;
            this.pnlCartButtons.Controls.Add(this.btnDiscount);
            this.pnlCartButtons.Controls.Add(this.btnQtyMinus);
            this.pnlCartButtons.Controls.Add(this.btnQtyPlus);
            this.pnlCartButtons.Controls.Add(this.btnRemoveLine);
            this.pnlCartButtons.Controls.Add(this.btnNewSale);
            this.pnlCartButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCartButtons.Location = new System.Drawing.Point(0, 354);
            this.pnlCartButtons.Name = "pnlCartButtons";
            this.pnlCartButtons.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCartButtons.Size = new System.Drawing.Size(527, 84);
            this.pnlCartButtons.TabIndex = 1;
            // 
            // btnDiscount
            // 
            this.btnDiscount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnDiscount.FlatAppearance.BorderSize = 0;
            this.btnDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDiscount.ForeColor = System.Drawing.Color.Black;
            this.btnDiscount.Location = new System.Drawing.Point(409, 12);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(105, 60);
            this.btnDiscount.TabIndex = 4;
            this.btnDiscount.Text = "Discount";
            this.btnDiscount.UseVisualStyleBackColor = false;
            // 
            // btnQtyMinus
            // 
            this.btnQtyMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(98)))), ((int)(((byte)(146)))));
            this.btnQtyMinus.FlatAppearance.BorderSize = 0;
            this.btnQtyMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQtyMinus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnQtyMinus.ForeColor = System.Drawing.Color.White;
            this.btnQtyMinus.Location = new System.Drawing.Point(272, 12);
            this.btnQtyMinus.Name = "btnQtyMinus";
            this.btnQtyMinus.Size = new System.Drawing.Size(66, 60);
            this.btnQtyMinus.TabIndex = 3;
            this.btnQtyMinus.Text = "- Qty";
            this.btnQtyMinus.UseVisualStyleBackColor = false;
            this.btnQtyMinus.Click += new System.EventHandler(this.btnQtyMinus_Click);
            // 
            // btnQtyPlus
            // 
            this.btnQtyPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(91)))));
            this.btnQtyPlus.FlatAppearance.BorderSize = 0;
            this.btnQtyPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQtyPlus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnQtyPlus.ForeColor = System.Drawing.Color.White;
            this.btnQtyPlus.Location = new System.Drawing.Point(340, 12);
            this.btnQtyPlus.Name = "btnQtyPlus";
            this.btnQtyPlus.Size = new System.Drawing.Size(66, 60);
            this.btnQtyPlus.TabIndex = 2;
            this.btnQtyPlus.Text = "+ Qty";
            this.btnQtyPlus.UseVisualStyleBackColor = false;
            this.btnQtyPlus.Click += new System.EventHandler(this.btnQtyPlus_Click);
            // 
            // btnRemoveLine
            // 
            this.btnRemoveLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(83)))), ((int)(((byte)(80)))));
            this.btnRemoveLine.FlatAppearance.BorderSize = 0;
            this.btnRemoveLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveLine.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRemoveLine.ForeColor = System.Drawing.Color.White;
            this.btnRemoveLine.Location = new System.Drawing.Point(141, 12);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(128, 60);
            this.btnRemoveLine.TabIndex = 1;
            this.btnRemoveLine.Text = "Remove";
            this.btnRemoveLine.UseVisualStyleBackColor = false;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            // 
            // btnNewSale
            // 
            this.btnNewSale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(120)))), ((int)(((byte)(198)))));
            this.btnNewSale.FlatAppearance.BorderSize = 0;
            this.btnNewSale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewSale.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnNewSale.ForeColor = System.Drawing.Color.White;
            this.btnNewSale.Location = new System.Drawing.Point(10, 12);
            this.btnNewSale.Name = "btnNewSale";
            this.btnNewSale.Size = new System.Drawing.Size(128, 60);
            this.btnNewSale.TabIndex = 0;
            this.btnNewSale.Text = "New";
            this.btnNewSale.UseVisualStyleBackColor = false;
            this.btnNewSale.Click += new System.EventHandler(this.btnNewSale_Click);
            // 
            // pnlTotals
            // 
            this.pnlTotals.BackColor = System.Drawing.Color.White;
            this.pnlTotals.Controls.Add(this.lblGrandTotal);
            this.pnlTotals.Controls.Add(this.lblGrandCaption);
            this.pnlTotals.Controls.Add(this.lblDiscount);
            this.pnlTotals.Controls.Add(this.lblDiscountCaption);
            this.pnlTotals.Controls.Add(this.lblTax);
            this.pnlTotals.Controls.Add(this.lblTaxCaption);
            this.pnlTotals.Controls.Add(this.lblSubtotal);
            this.pnlTotals.Controls.Add(this.lblSubtotalCaption);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTotals.Location = new System.Drawing.Point(0, 438);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Padding = new System.Windows.Forms.Padding(16);
            this.pnlTotals.Size = new System.Drawing.Size(527, 140);
            this.pnlTotals.TabIndex = 2;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGrandTotal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(91)))));
            this.lblGrandTotal.Location = new System.Drawing.Point(330, 88);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(180, 42);
            this.lblGrandTotal.TabIndex = 7;
            this.lblGrandTotal.Text = "0.00";
            this.lblGrandTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGrandCaption
            // 
            this.lblGrandCaption.AutoSize = true;
            this.lblGrandCaption.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblGrandCaption.Location = new System.Drawing.Point(14, 96);
            this.lblGrandCaption.Name = "lblGrandCaption";
            this.lblGrandCaption.Size = new System.Drawing.Size(70, 32);
            this.lblGrandCaption.TabIndex = 6;
            this.lblGrandCaption.Text = "Total";
            // 
            // lblDiscount
            // 
            this.lblDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDiscount.Location = new System.Drawing.Point(330, 56);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(180, 25);
            this.lblDiscount.TabIndex = 5;
            this.lblDiscount.Text = "0.00";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDiscountCaption
            // 
            this.lblDiscountCaption.AutoSize = true;
            this.lblDiscountCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDiscountCaption.ForeColor = System.Drawing.Color.DimGray;
            this.lblDiscountCaption.Location = new System.Drawing.Point(16, 58);
            this.lblDiscountCaption.Name = "lblDiscountCaption";
            this.lblDiscountCaption.Size = new System.Drawing.Size(96, 28);
            this.lblDiscountCaption.TabIndex = 4;
            this.lblDiscountCaption.Text = "Discount";
            // 
            // lblTax
            // 
            this.lblTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTax.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTax.Location = new System.Drawing.Point(330, 32);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(180, 25);
            this.lblTax.TabIndex = 3;
            this.lblTax.Text = "0.00";
            this.lblTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxCaption
            // 
            this.lblTaxCaption.AutoSize = true;
            this.lblTaxCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTaxCaption.ForeColor = System.Drawing.Color.DimGray;
            this.lblTaxCaption.Location = new System.Drawing.Point(16, 34);
            this.lblTaxCaption.Name = "lblTaxCaption";
            this.lblTaxCaption.Size = new System.Drawing.Size(44, 28);
            this.lblTaxCaption.TabIndex = 2;
            this.lblTaxCaption.Text = "Tax";
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSubtotal.Location = new System.Drawing.Point(330, 8);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(180, 25);
            this.lblSubtotal.TabIndex = 1;
            this.lblSubtotal.Text = "0.00";
            this.lblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubtotalCaption
            // 
            this.lblSubtotalCaption.AutoSize = true;
            this.lblSubtotalCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubtotalCaption.ForeColor = System.Drawing.Color.DimGray;
            this.lblSubtotalCaption.Location = new System.Drawing.Point(16, 10);
            this.lblSubtotalCaption.Name = "lblSubtotalCaption";
            this.lblSubtotalCaption.Size = new System.Drawing.Size(92, 28);
            this.lblSubtotalCaption.TabIndex = 0;
            this.lblSubtotalCaption.Text = "Subtotal";
            // 
            // pnlPay
            // 
            this.pnlPay.BackColor = System.Drawing.Color.White;
            this.pnlPay.Controls.Add(this.btnPayMixed);
            this.pnlPay.Controls.Add(this.btnPayCard);
            this.pnlPay.Controls.Add(this.btnPayCash);
            this.pnlPay.Controls.Add(this.btnRecall);
            this.pnlPay.Controls.Add(this.btnHold);
            this.pnlPay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPay.Location = new System.Drawing.Point(0, 578);
            this.pnlPay.Name = "pnlPay";
            this.pnlPay.Padding = new System.Windows.Forms.Padding(10);
            this.pnlPay.Size = new System.Drawing.Size(527, 134);
            this.pnlPay.TabIndex = 3;
            // 
            // btnPayMixed
            // 
            this.btnPayMixed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnPayMixed.FlatAppearance.BorderSize = 0;
            this.btnPayMixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayMixed.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnPayMixed.ForeColor = System.Drawing.Color.Black;
            this.btnPayMixed.Location = new System.Drawing.Point(354, 62);
            this.btnPayMixed.Name = "btnPayMixed";
            this.btnPayMixed.Size = new System.Drawing.Size(160, 60);
            this.btnPayMixed.TabIndex = 4;
            this.btnPayMixed.Text = "Mixed";
            this.btnPayMixed.UseVisualStyleBackColor = false;
            this.btnPayMixed.Click += new System.EventHandler(this.btnPayMixed_Click);
            // 
            // btnPayCard
            // 
            this.btnPayCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(120)))), ((int)(((byte)(198)))));
            this.btnPayCard.FlatAppearance.BorderSize = 0;
            this.btnPayCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayCard.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnPayCard.ForeColor = System.Drawing.Color.White;
            this.btnPayCard.Location = new System.Drawing.Point(182, 62);
            this.btnPayCard.Name = "btnPayCard";
            this.btnPayCard.Size = new System.Drawing.Size(160, 60);
            this.btnPayCard.TabIndex = 3;
            this.btnPayCard.Text = "Card";
            this.btnPayCard.UseVisualStyleBackColor = false;
            this.btnPayCard.Click += new System.EventHandler(this.btnPayCard_Click);
            // 
            // btnPayCash
            // 
            this.btnPayCash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(91)))));
            this.btnPayCash.FlatAppearance.BorderSize = 0;
            this.btnPayCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayCash.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnPayCash.ForeColor = System.Drawing.Color.White;
            this.btnPayCash.Location = new System.Drawing.Point(10, 62);
            this.btnPayCash.Name = "btnPayCash";
            this.btnPayCash.Size = new System.Drawing.Size(160, 60);
            this.btnPayCash.TabIndex = 2;
            this.btnPayCash.Text = "Cash";
            this.btnPayCash.UseVisualStyleBackColor = false;
            this.btnPayCash.Click += new System.EventHandler(this.btnPayCash_Click);
            // 
            // btnRecall
            // 
            this.btnRecall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.btnRecall.FlatAppearance.BorderSize = 0;
            this.btnRecall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecall.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRecall.ForeColor = System.Drawing.Color.White;
            this.btnRecall.Location = new System.Drawing.Point(270, 10);
            this.btnRecall.Name = "btnRecall";
            this.btnRecall.Size = new System.Drawing.Size(244, 44);
            this.btnRecall.TabIndex = 1;
            this.btnRecall.Text = "Recall";
            this.btnRecall.UseVisualStyleBackColor = false;
            // 
            // btnHold
            // 
            this.btnHold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.btnHold.FlatAppearance.BorderSize = 0;
            this.btnHold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHold.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnHold.ForeColor = System.Drawing.Color.White;
            this.btnHold.Location = new System.Drawing.Point(10, 10);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(244, 44);
            this.btnHold.TabIndex = 0;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = false;
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // frm_pos_sale
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_pos_sale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POS";
            this.Load += new System.EventHandler(this.frm_pos_sale_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCart)).EndInit();
            this.pnlCartButtons.ResumeLayout(false);
            this.pnlTotals.ResumeLayout(false);
            this.pnlTotals.PerformLayout();
            this.pnlPay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnLang;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Label lblBranch;
        private System.Windows.Forms.Label lblBranchCaption;
        private System.Windows.Forms.Label lblCashier;
        private System.Windows.Forms.Label lblCashierCaption;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Button btnSearchFocus;
        private System.Windows.Forms.Button btnBarcodeFocus;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.FlowLayoutPanel flpTiles;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.DataGridView gridCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaxRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.Panel pnlCartButtons;
        private System.Windows.Forms.Button btnDiscount;
        private System.Windows.Forms.Button btnQtyMinus;
        private System.Windows.Forms.Button btnQtyPlus;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Button btnNewSale;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Label lblGrandCaption;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblDiscountCaption;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblTaxCaption;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblSubtotalCaption;
        private System.Windows.Forms.Panel pnlPay;
        private System.Windows.Forms.Button btnPayMixed;
        private System.Windows.Forms.Button btnPayCard;
        private System.Windows.Forms.Button btnPayCash;
        private System.Windows.Forms.Button btnRecall;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Timer timerClock;
    }
}