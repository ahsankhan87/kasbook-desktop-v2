namespace pos
{
    partial class frm_customer_detail
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.tabProfile = new System.Windows.Forms.TabControl();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.chartMonthly = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableKpi = new System.Windows.Forms.TableLayoutPanel();
            this.pnlKpi1 = new System.Windows.Forms.Panel();
            this.lblLifetimeSalesValue = new System.Windows.Forms.Label();
            this.lblLifetimeSalesTitle = new System.Windows.Forms.Label();
            this.pnlKpi2 = new System.Windows.Forms.Panel();
            this.lblTotalPaidValue = new System.Windows.Forms.Label();
            this.lblTotalPaidTitle = new System.Windows.Forms.Label();
            this.pnlKpi3 = new System.Windows.Forms.Panel();
            this.lblOutstandingValue = new System.Windows.Forms.Label();
            this.lblOutstandingTitle = new System.Windows.Forms.Label();
            this.pnlKpi4 = new System.Windows.Forms.Panel();
            this.lblAvailableCreditValue = new System.Windows.Forms.Label();
            this.lblAvailableCreditTitle = new System.Windows.Forms.Label();
            this.pnlOverviewHeader = new System.Windows.Forms.Panel();
            this.chkAccountStatus = new System.Windows.Forms.CheckBox();
            this.lblHeaderCreditLimit = new System.Windows.Forms.Label();
            this.lblHeaderEmail = new System.Windows.Forms.Label();
            this.lblHeaderPhone = new System.Windows.Forms.Label();
            this.lblHeaderArea = new System.Windows.Forms.Label();
            this.lblHeaderCode = new System.Windows.Forms.Label();
            this.lblHeaderName = new System.Windows.Forms.Label();
            this.pnlAvatar = new System.Windows.Forms.Panel();
            this.tabLedger = new System.Windows.Forms.TabPage();
            this.gridLedger = new System.Windows.Forms.DataGridView();
            this.colLedgerDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLedgerFooter = new System.Windows.Forms.Panel();
            this.lblLedgerTotals = new System.Windows.Forms.Label();
            this.pnlLedgerFilter = new System.Windows.Forms.Panel();
            this.btnPrintLedger = new System.Windows.Forms.Button();
            this.btnLoadLedger = new System.Windows.Forms.Button();
            this.dtLedgerTo = new System.Windows.Forms.DateTimePicker();
            this.lblLedgerTo = new System.Windows.Forms.Label();
            this.dtLedgerFrom = new System.Windows.Forms.DateTimePicker();
            this.lblLedgerFrom = new System.Windows.Forms.Label();
            this.tabOutstanding = new System.Windows.Forms.TabPage();
            this.gridOutstanding = new System.Windows.Forms.DataGridView();
            this.colOutInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlOutstandingBottom = new System.Windows.Forms.Panel();
            this.btnReceivePayment = new System.Windows.Forms.Button();
            this.flowAging = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlAging1 = new System.Windows.Forms.Panel();
            this.lblAge0to30Value = new System.Windows.Forms.Label();
            this.lblAge0to30Title = new System.Windows.Forms.Label();
            this.pnlAging2 = new System.Windows.Forms.Panel();
            this.lblAge31to60Value = new System.Windows.Forms.Label();
            this.lblAge31to60Title = new System.Windows.Forms.Label();
            this.pnlAging3 = new System.Windows.Forms.Panel();
            this.lblAge61to90Value = new System.Windows.Forms.Label();
            this.lblAge61to90Title = new System.Windows.Forms.Label();
            this.pnlAging4 = new System.Windows.Forms.Panel();
            this.lblAge90PlusValue = new System.Windows.Forms.Label();
            this.lblAge90PlusTitle = new System.Windows.Forms.Label();
            this.tabNotes = new System.Windows.Forms.TabPage();
            this.gridNotes = new System.Windows.Forms.DataGridView();
            this.colNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNoteText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNoteBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlNotesTop = new System.Windows.Forms.Panel();
            this.btnReminder = new System.Windows.Forms.Button();
            this.btnAddNote = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.tabOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).BeginInit();
            this.tableKpi.SuspendLayout();
            this.pnlKpi1.SuspendLayout();
            this.pnlKpi2.SuspendLayout();
            this.pnlKpi3.SuspendLayout();
            this.pnlKpi4.SuspendLayout();
            this.pnlOverviewHeader.SuspendLayout();
            this.tabLedger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLedger)).BeginInit();
            this.pnlLedgerFooter.SuspendLayout();
            this.pnlLedgerFilter.SuspendLayout();
            this.tabOutstanding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOutstanding)).BeginInit();
            this.pnlOutstandingBottom.SuspendLayout();
            this.flowAging.SuspendLayout();
            this.pnlAging1.SuspendLayout();
            this.pnlAging2.SuspendLayout();
            this.pnlAging3.SuspendLayout();
            this.pnlAging4.SuspendLayout();
            this.tabNotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNotes)).BeginInit();
            this.pnlNotesTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lbl_title);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.panelTop.Size = new System.Drawing.Size(1024, 52);
            this.panelTop.TabIndex = 0;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_title.Location = new System.Drawing.Point(12, 13);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(194, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Individual Customer";
            // 
            // tabProfile
            // 
            this.tabProfile.Controls.Add(this.tabOverview);
            this.tabProfile.Controls.Add(this.tabLedger);
            this.tabProfile.Controls.Add(this.tabOutstanding);
            this.tabProfile.Controls.Add(this.tabNotes);
            this.tabProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProfile.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabProfile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabProfile.ItemSize = new System.Drawing.Size(180, 34);
            this.tabProfile.Location = new System.Drawing.Point(0, 52);
            this.tabProfile.Multiline = true;
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.SelectedIndex = 0;
            this.tabProfile.Size = new System.Drawing.Size(1024, 616);
            this.tabProfile.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabProfile.TabIndex = 1;
            this.tabProfile.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabProfile_DrawItem);
            // 
            // tabOverview
            // 
            this.tabOverview.BackColor = System.Drawing.Color.White;
            this.tabOverview.Controls.Add(this.chartMonthly);
            this.tabOverview.Controls.Add(this.tableKpi);
            this.tabOverview.Controls.Add(this.pnlOverviewHeader);
            this.tabOverview.Location = new System.Drawing.Point(4, 38);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Padding = new System.Windows.Forms.Padding(10);
            this.tabOverview.Size = new System.Drawing.Size(1016, 574);
            this.tabOverview.TabIndex = 0;
            this.tabOverview.Text = "Overview";
            // 
            // chartMonthly
            // 
            chartArea1.Name = "ChartArea1";
            this.chartMonthly.ChartAreas.Add(chartArea1);
            this.chartMonthly.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chartMonthly.Legends.Add(legend1);
            this.chartMonthly.Location = new System.Drawing.Point(10, 235);
            this.chartMonthly.Name = "chartMonthly";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            series1.Legend = "Legend1";
            series1.Name = "Monthly";
            series1.XValueMember = "month_label";
            series1.YValueMembers = "amount";
            this.chartMonthly.Series.Add(series1);
            this.chartMonthly.Size = new System.Drawing.Size(996, 329);
            this.chartMonthly.TabIndex = 2;
            this.chartMonthly.Text = "chart1";
            // 
            // tableKpi
            // 
            this.tableKpi.ColumnCount = 4;
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.Controls.Add(this.pnlKpi1, 0, 0);
            this.tableKpi.Controls.Add(this.pnlKpi2, 1, 0);
            this.tableKpi.Controls.Add(this.pnlKpi3, 2, 0);
            this.tableKpi.Controls.Add(this.pnlKpi4, 3, 0);
            this.tableKpi.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableKpi.Location = new System.Drawing.Point(10, 145);
            this.tableKpi.Name = "tableKpi";
            this.tableKpi.RowCount = 1;
            this.tableKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableKpi.Size = new System.Drawing.Size(996, 90);
            this.tableKpi.TabIndex = 1;
            // 
            // pnlKpi1
            // 
            this.pnlKpi1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi1.Controls.Add(this.lblLifetimeSalesValue);
            this.pnlKpi1.Controls.Add(this.lblLifetimeSalesTitle);
            this.pnlKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi1.Location = new System.Drawing.Point(3, 3);
            this.pnlKpi1.Name = "pnlKpi1";
            this.pnlKpi1.Size = new System.Drawing.Size(243, 84);
            this.pnlKpi1.TabIndex = 0;
            // 
            // lblLifetimeSalesValue
            // 
            this.lblLifetimeSalesValue.AutoSize = true;
            this.lblLifetimeSalesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblLifetimeSalesValue.Location = new System.Drawing.Point(13, 35);
            this.lblLifetimeSalesValue.Name = "lblLifetimeSalesValue";
            this.lblLifetimeSalesValue.Size = new System.Drawing.Size(56, 32);
            this.lblLifetimeSalesValue.TabIndex = 1;
            this.lblLifetimeSalesValue.Text = "0.00";
            // 
            // lblLifetimeSalesTitle
            // 
            this.lblLifetimeSalesTitle.AutoSize = true;
            this.lblLifetimeSalesTitle.Location = new System.Drawing.Point(16, 12);
            this.lblLifetimeSalesTitle.Name = "lblLifetimeSalesTitle";
            this.lblLifetimeSalesTitle.Size = new System.Drawing.Size(85, 20);
            this.lblLifetimeSalesTitle.TabIndex = 0;
            this.lblLifetimeSalesTitle.Text = "Lifetime Sales";
            // 
            // pnlKpi2
            // 
            this.pnlKpi2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi2.Controls.Add(this.lblTotalPaidValue);
            this.pnlKpi2.Controls.Add(this.lblTotalPaidTitle);
            this.pnlKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi2.Location = new System.Drawing.Point(252, 3);
            this.pnlKpi2.Name = "pnlKpi2";
            this.pnlKpi2.Size = new System.Drawing.Size(243, 84);
            this.pnlKpi2.TabIndex = 1;
            // 
            // lblTotalPaidValue
            // 
            this.lblTotalPaidValue.AutoSize = true;
            this.lblTotalPaidValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalPaidValue.Location = new System.Drawing.Point(14, 35);
            this.lblTotalPaidValue.Name = "lblTotalPaidValue";
            this.lblTotalPaidValue.Size = new System.Drawing.Size(56, 32);
            this.lblTotalPaidValue.TabIndex = 1;
            this.lblTotalPaidValue.Text = "0.00";
            // 
            // lblTotalPaidTitle
            // 
            this.lblTotalPaidTitle.AutoSize = true;
            this.lblTotalPaidTitle.Location = new System.Drawing.Point(17, 12);
            this.lblTotalPaidTitle.Name = "lblTotalPaidTitle";
            this.lblTotalPaidTitle.Size = new System.Drawing.Size(67, 20);
            this.lblTotalPaidTitle.TabIndex = 0;
            this.lblTotalPaidTitle.Text = "Total Paid";
            // 
            // pnlKpi3
            // 
            this.pnlKpi3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi3.Controls.Add(this.lblOutstandingValue);
            this.pnlKpi3.Controls.Add(this.lblOutstandingTitle);
            this.pnlKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi3.Location = new System.Drawing.Point(501, 3);
            this.pnlKpi3.Name = "pnlKpi3";
            this.pnlKpi3.Size = new System.Drawing.Size(243, 84);
            this.pnlKpi3.TabIndex = 2;
            // 
            // lblOutstandingValue
            // 
            this.lblOutstandingValue.AutoSize = true;
            this.lblOutstandingValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblOutstandingValue.ForeColor = System.Drawing.Color.Firebrick;
            this.lblOutstandingValue.Location = new System.Drawing.Point(14, 35);
            this.lblOutstandingValue.Name = "lblOutstandingValue";
            this.lblOutstandingValue.Size = new System.Drawing.Size(56, 32);
            this.lblOutstandingValue.TabIndex = 1;
            this.lblOutstandingValue.Text = "0.00";
            // 
            // lblOutstandingTitle
            // 
            this.lblOutstandingTitle.AutoSize = true;
            this.lblOutstandingTitle.Location = new System.Drawing.Point(17, 12);
            this.lblOutstandingTitle.Name = "lblOutstandingTitle";
            this.lblOutstandingTitle.Size = new System.Drawing.Size(89, 20);
            this.lblOutstandingTitle.TabIndex = 0;
            this.lblOutstandingTitle.Text = "Outstanding";
            // 
            // pnlKpi4
            // 
            this.pnlKpi4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi4.Controls.Add(this.lblAvailableCreditValue);
            this.pnlKpi4.Controls.Add(this.lblAvailableCreditTitle);
            this.pnlKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi4.Location = new System.Drawing.Point(750, 3);
            this.pnlKpi4.Name = "pnlKpi4";
            this.pnlKpi4.Size = new System.Drawing.Size(243, 84);
            this.pnlKpi4.TabIndex = 3;
            // 
            // lblAvailableCreditValue
            // 
            this.lblAvailableCreditValue.AutoSize = true;
            this.lblAvailableCreditValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblAvailableCreditValue.Location = new System.Drawing.Point(14, 35);
            this.lblAvailableCreditValue.Name = "lblAvailableCreditValue";
            this.lblAvailableCreditValue.Size = new System.Drawing.Size(56, 32);
            this.lblAvailableCreditValue.TabIndex = 1;
            this.lblAvailableCreditValue.Text = "0.00";
            // 
            // lblAvailableCreditTitle
            // 
            this.lblAvailableCreditTitle.AutoSize = true;
            this.lblAvailableCreditTitle.Location = new System.Drawing.Point(17, 12);
            this.lblAvailableCreditTitle.Name = "lblAvailableCreditTitle";
            this.lblAvailableCreditTitle.Size = new System.Drawing.Size(101, 20);
            this.lblAvailableCreditTitle.TabIndex = 0;
            this.lblAvailableCreditTitle.Text = "Available Credit";
            // 
            // pnlOverviewHeader
            // 
            this.pnlOverviewHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOverviewHeader.Controls.Add(this.chkAccountStatus);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderCreditLimit);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderEmail);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderPhone);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderArea);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderCode);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderName);
            this.pnlOverviewHeader.Controls.Add(this.pnlAvatar);
            this.pnlOverviewHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOverviewHeader.Location = new System.Drawing.Point(10, 10);
            this.pnlOverviewHeader.Name = "pnlOverviewHeader";
            this.pnlOverviewHeader.Size = new System.Drawing.Size(996, 135);
            this.pnlOverviewHeader.TabIndex = 0;
            // 
            // chkAccountStatus
            // 
            this.chkAccountStatus.AutoSize = true;
            this.chkAccountStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.chkAccountStatus.Location = new System.Drawing.Point(861, 17);
            this.chkAccountStatus.Name = "chkAccountStatus";
            this.chkAccountStatus.Size = new System.Drawing.Size(71, 24);
            this.chkAccountStatus.TabIndex = 7;
            this.chkAccountStatus.Text = "Active";
            this.chkAccountStatus.UseVisualStyleBackColor = true;
            this.chkAccountStatus.CheckedChanged += new System.EventHandler(this.chkAccountStatus_CheckedChanged);
            // 
            // lblHeaderCreditLimit
            // 
            this.lblHeaderCreditLimit.AutoSize = true;
            this.lblHeaderCreditLimit.Location = new System.Drawing.Point(147, 96);
            this.lblHeaderCreditLimit.Name = "lblHeaderCreditLimit";
            this.lblHeaderCreditLimit.Size = new System.Drawing.Size(112, 20);
            this.lblHeaderCreditLimit.TabIndex = 6;
            this.lblHeaderCreditLimit.Text = "Credit Limit: 0.00";
            // 
            // lblHeaderEmail
            // 
            this.lblHeaderEmail.AutoSize = true;
            this.lblHeaderEmail.Location = new System.Drawing.Point(147, 76);
            this.lblHeaderEmail.Name = "lblHeaderEmail";
            this.lblHeaderEmail.Size = new System.Drawing.Size(106, 20);
            this.lblHeaderEmail.TabIndex = 5;
            this.lblHeaderEmail.Text = "Email: no-email";
            // 
            // lblHeaderPhone
            // 
            this.lblHeaderPhone.AutoSize = true;
            this.lblHeaderPhone.Location = new System.Drawing.Point(147, 56);
            this.lblHeaderPhone.Name = "lblHeaderPhone";
            this.lblHeaderPhone.Size = new System.Drawing.Size(100, 20);
            this.lblHeaderPhone.TabIndex = 4;
            this.lblHeaderPhone.Text = "Phone: -";
            // 
            // lblHeaderArea
            // 
            this.lblHeaderArea.AutoSize = true;
            this.lblHeaderArea.Location = new System.Drawing.Point(420, 56);
            this.lblHeaderArea.Name = "lblHeaderArea";
            this.lblHeaderArea.Size = new System.Drawing.Size(60, 20);
            this.lblHeaderArea.TabIndex = 3;
            this.lblHeaderArea.Text = "Area: -";
            // 
            // lblHeaderCode
            // 
            this.lblHeaderCode.AutoSize = true;
            this.lblHeaderCode.Location = new System.Drawing.Point(420, 33);
            this.lblHeaderCode.Name = "lblHeaderCode";
            this.lblHeaderCode.Size = new System.Drawing.Size(95, 20);
            this.lblHeaderCode.TabIndex = 2;
            this.lblHeaderCode.Text = "Code: C00000";
            // 
            // lblHeaderName
            // 
            this.lblHeaderName.AutoSize = true;
            this.lblHeaderName.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeaderName.Location = new System.Drawing.Point(145, 22);
            this.lblHeaderName.Name = "lblHeaderName";
            this.lblHeaderName.Size = new System.Drawing.Size(165, 32);
            this.lblHeaderName.TabIndex = 1;
            this.lblHeaderName.Text = "Customer Name";
            // 
            // pnlAvatar
            // 
            this.pnlAvatar.Location = new System.Drawing.Point(17, 19);
            this.pnlAvatar.Name = "pnlAvatar";
            this.pnlAvatar.Size = new System.Drawing.Size(110, 95);
            this.pnlAvatar.TabIndex = 0;
            this.pnlAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlAvatar_Paint);
            // 
            // tabLedger
            // 
            this.tabLedger.BackColor = System.Drawing.Color.White;
            this.tabLedger.Controls.Add(this.gridLedger);
            this.tabLedger.Controls.Add(this.pnlLedgerFooter);
            this.tabLedger.Controls.Add(this.pnlLedgerFilter);
            this.tabLedger.Location = new System.Drawing.Point(4, 38);
            this.tabLedger.Name = "tabLedger";
            this.tabLedger.Padding = new System.Windows.Forms.Padding(8);
            this.tabLedger.Size = new System.Drawing.Size(1016, 574);
            this.tabLedger.TabIndex = 1;
            this.tabLedger.Text = "Transactions / Ledger";
            // 
            // gridLedger
            // 
            this.gridLedger.AllowUserToAddRows = false;
            this.gridLedger.AllowUserToDeleteRows = false;
            this.gridLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLedgerDate,
            this.colLedgerType,
            this.colLedgerRef,
            this.colLedgerDebit,
            this.colLedgerCredit,
            this.colLedgerBalance});
            this.gridLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLedger.Location = new System.Drawing.Point(8, 56);
            this.gridLedger.Name = "gridLedger";
            this.gridLedger.ReadOnly = true;
            this.gridLedger.RowHeadersVisible = false;
            this.gridLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLedger.Size = new System.Drawing.Size(1000, 478);
            this.gridLedger.TabIndex = 1;
            // 
            // colLedgerDate
            // 
            this.colLedgerDate.DataPropertyName = "entry_date";
            this.colLedgerDate.HeaderText = "Date";
            this.colLedgerDate.Name = "colLedgerDate";
            this.colLedgerDate.ReadOnly = true;
            // 
            // colLedgerType
            // 
            this.colLedgerType.DataPropertyName = "trans_type";
            this.colLedgerType.HeaderText = "Type";
            this.colLedgerType.Name = "colLedgerType";
            this.colLedgerType.ReadOnly = true;
            // 
            // colLedgerRef
            // 
            this.colLedgerRef.DataPropertyName = "reference_no";
            this.colLedgerRef.HeaderText = "Reference No";
            this.colLedgerRef.Name = "colLedgerRef";
            this.colLedgerRef.ReadOnly = true;
            // 
            // colLedgerDebit
            // 
            this.colLedgerDebit.DataPropertyName = "debit";
            this.colLedgerDebit.HeaderText = "Debit (Invoice Amount)";
            this.colLedgerDebit.Name = "colLedgerDebit";
            this.colLedgerDebit.ReadOnly = true;
            // 
            // colLedgerCredit
            // 
            this.colLedgerCredit.DataPropertyName = "credit";
            this.colLedgerCredit.HeaderText = "Credit (Payment)";
            this.colLedgerCredit.Name = "colLedgerCredit";
            this.colLedgerCredit.ReadOnly = true;
            // 
            // colLedgerBalance
            // 
            this.colLedgerBalance.DataPropertyName = "running_balance";
            this.colLedgerBalance.HeaderText = "Running Balance";
            this.colLedgerBalance.Name = "colLedgerBalance";
            this.colLedgerBalance.ReadOnly = true;
            // 
            // pnlLedgerFooter
            // 
            this.pnlLedgerFooter.Controls.Add(this.lblLedgerTotals);
            this.pnlLedgerFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLedgerFooter.Location = new System.Drawing.Point(8, 534);
            this.pnlLedgerFooter.Name = "pnlLedgerFooter";
            this.pnlLedgerFooter.Size = new System.Drawing.Size(1000, 32);
            this.pnlLedgerFooter.TabIndex = 2;
            // 
            // lblLedgerTotals
            // 
            this.lblLedgerTotals.AutoSize = true;
            this.lblLedgerTotals.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblLedgerTotals.Location = new System.Drawing.Point(10, 7);
            this.lblLedgerTotals.Name = "lblLedgerTotals";
            this.lblLedgerTotals.Size = new System.Drawing.Size(154, 20);
            this.lblLedgerTotals.TabIndex = 0;
            this.lblLedgerTotals.Text = "Totals: DR 0.00 | CR 0.00";
            // 
            // pnlLedgerFilter
            // 
            this.pnlLedgerFilter.Controls.Add(this.btnPrintLedger);
            this.pnlLedgerFilter.Controls.Add(this.btnLoadLedger);
            this.pnlLedgerFilter.Controls.Add(this.dtLedgerTo);
            this.pnlLedgerFilter.Controls.Add(this.lblLedgerTo);
            this.pnlLedgerFilter.Controls.Add(this.dtLedgerFrom);
            this.pnlLedgerFilter.Controls.Add(this.lblLedgerFrom);
            this.pnlLedgerFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLedgerFilter.Location = new System.Drawing.Point(8, 8);
            this.pnlLedgerFilter.Name = "pnlLedgerFilter";
            this.pnlLedgerFilter.Size = new System.Drawing.Size(1000, 48);
            this.pnlLedgerFilter.TabIndex = 0;
            // 
            // btnPrintLedger
            // 
            this.btnPrintLedger.Location = new System.Drawing.Point(707, 10);
            this.btnPrintLedger.Name = "btnPrintLedger";
            this.btnPrintLedger.Size = new System.Drawing.Size(120, 28);
            this.btnPrintLedger.TabIndex = 5;
            this.btnPrintLedger.Text = "Print Ledger";
            this.btnPrintLedger.UseVisualStyleBackColor = true;
            this.btnPrintLedger.Click += new System.EventHandler(this.btnPrintLedger_Click);
            // 
            // btnLoadLedger
            // 
            this.btnLoadLedger.Location = new System.Drawing.Point(581, 10);
            this.btnLoadLedger.Name = "btnLoadLedger";
            this.btnLoadLedger.Size = new System.Drawing.Size(120, 28);
            this.btnLoadLedger.TabIndex = 4;
            this.btnLoadLedger.Text = "Apply";
            this.btnLoadLedger.UseVisualStyleBackColor = true;
            this.btnLoadLedger.Click += new System.EventHandler(this.btnLoadLedger_Click);
            // 
            // dtLedgerTo
            // 
            this.dtLedgerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtLedgerTo.Location = new System.Drawing.Point(365, 13);
            this.dtLedgerTo.Name = "dtLedgerTo";
            this.dtLedgerTo.Size = new System.Drawing.Size(133, 27);
            this.dtLedgerTo.TabIndex = 3;
            // 
            // lblLedgerTo
            // 
            this.lblLedgerTo.AutoSize = true;
            this.lblLedgerTo.Location = new System.Drawing.Point(330, 16);
            this.lblLedgerTo.Name = "lblLedgerTo";
            this.lblLedgerTo.Size = new System.Drawing.Size(24, 20);
            this.lblLedgerTo.TabIndex = 2;
            this.lblLedgerTo.Text = "To";
            // 
            // dtLedgerFrom
            // 
            this.dtLedgerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtLedgerFrom.Location = new System.Drawing.Point(158, 13);
            this.dtLedgerFrom.Name = "dtLedgerFrom";
            this.dtLedgerFrom.Size = new System.Drawing.Size(133, 27);
            this.dtLedgerFrom.TabIndex = 1;
            // 
            // lblLedgerFrom
            // 
            this.lblLedgerFrom.AutoSize = true;
            this.lblLedgerFrom.Location = new System.Drawing.Point(110, 16);
            this.lblLedgerFrom.Name = "lblLedgerFrom";
            this.lblLedgerFrom.Size = new System.Drawing.Size(44, 20);
            this.lblLedgerFrom.TabIndex = 0;
            this.lblLedgerFrom.Text = "From";
            // 
            // tabOutstanding
            // 
            this.tabOutstanding.BackColor = System.Drawing.Color.White;
            this.tabOutstanding.Controls.Add(this.gridOutstanding);
            this.tabOutstanding.Controls.Add(this.pnlOutstandingBottom);
            this.tabOutstanding.Controls.Add(this.flowAging);
            this.tabOutstanding.Location = new System.Drawing.Point(4, 38);
            this.tabOutstanding.Name = "tabOutstanding";
            this.tabOutstanding.Padding = new System.Windows.Forms.Padding(8);
            this.tabOutstanding.Size = new System.Drawing.Size(1016, 574);
            this.tabOutstanding.TabIndex = 2;
            this.tabOutstanding.Text = "Outstanding Invoices";
            // 
            // gridOutstanding
            // 
            this.gridOutstanding.AllowUserToAddRows = false;
            this.gridOutstanding.AllowUserToDeleteRows = false;
            this.gridOutstanding.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridOutstanding.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOutstanding.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOutInvoice,
            this.colOutDate,
            this.colOutDueDate,
            this.colOutAmount,
            this.colOutPaid,
            this.colOutBalance,
            this.colOutDays});
            this.gridOutstanding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOutstanding.Location = new System.Drawing.Point(8, 80);
            this.gridOutstanding.Name = "gridOutstanding";
            this.gridOutstanding.ReadOnly = true;
            this.gridOutstanding.RowHeadersVisible = false;
            this.gridOutstanding.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOutstanding.Size = new System.Drawing.Size(1000, 446);
            this.gridOutstanding.TabIndex = 1;
            // 
            // colOutInvoice
            // 
            this.colOutInvoice.DataPropertyName = "invoice_no";
            this.colOutInvoice.HeaderText = "Invoice No";
            this.colOutInvoice.Name = "colOutInvoice";
            this.colOutInvoice.ReadOnly = true;
            // 
            // colOutDate
            // 
            this.colOutDate.DataPropertyName = "sale_date";
            this.colOutDate.HeaderText = "Date";
            this.colOutDate.Name = "colOutDate";
            this.colOutDate.ReadOnly = true;
            // 
            // colOutDueDate
            // 
            this.colOutDueDate.DataPropertyName = "due_date";
            this.colOutDueDate.HeaderText = "Due Date";
            this.colOutDueDate.Name = "colOutDueDate";
            this.colOutDueDate.ReadOnly = true;
            // 
            // colOutAmount
            // 
            this.colOutAmount.DataPropertyName = "amount";
            this.colOutAmount.HeaderText = "Amount";
            this.colOutAmount.Name = "colOutAmount";
            this.colOutAmount.ReadOnly = true;
            // 
            // colOutPaid
            // 
            this.colOutPaid.DataPropertyName = "paid";
            this.colOutPaid.HeaderText = "Paid";
            this.colOutPaid.Name = "colOutPaid";
            this.colOutPaid.ReadOnly = true;
            // 
            // colOutBalance
            // 
            this.colOutBalance.DataPropertyName = "balance";
            this.colOutBalance.HeaderText = "Balance";
            this.colOutBalance.Name = "colOutBalance";
            this.colOutBalance.ReadOnly = true;
            // 
            // colOutDays
            // 
            this.colOutDays.DataPropertyName = "days_overdue";
            this.colOutDays.HeaderText = "Days Overdue";
            this.colOutDays.Name = "colOutDays";
            this.colOutDays.ReadOnly = true;
            // 
            // pnlOutstandingBottom
            // 
            this.pnlOutstandingBottom.Controls.Add(this.btnReceivePayment);
            this.pnlOutstandingBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOutstandingBottom.Location = new System.Drawing.Point(8, 526);
            this.pnlOutstandingBottom.Name = "pnlOutstandingBottom";
            this.pnlOutstandingBottom.Size = new System.Drawing.Size(1000, 40);
            this.pnlOutstandingBottom.TabIndex = 2;
            // 
            // btnReceivePayment
            // 
            this.btnReceivePayment.Location = new System.Drawing.Point(10, 6);
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.Size = new System.Drawing.Size(152, 28);
            this.btnReceivePayment.TabIndex = 0;
            this.btnReceivePayment.Text = "Receive Payment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            this.btnReceivePayment.Click += new System.EventHandler(this.btnReceivePayment_Click);
            // 
            // flowAging
            // 
            this.flowAging.Controls.Add(this.pnlAging1);
            this.flowAging.Controls.Add(this.pnlAging2);
            this.flowAging.Controls.Add(this.pnlAging3);
            this.flowAging.Controls.Add(this.pnlAging4);
            this.flowAging.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowAging.Location = new System.Drawing.Point(8, 8);
            this.flowAging.Name = "flowAging";
            this.flowAging.Size = new System.Drawing.Size(1000, 72);
            this.flowAging.TabIndex = 0;
            // 
            // pnlAging1
            // 
            this.pnlAging1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.pnlAging1.Controls.Add(this.lblAge0to30Value);
            this.pnlAging1.Controls.Add(this.lblAge0to30Title);
            this.pnlAging1.Location = new System.Drawing.Point(3, 3);
            this.pnlAging1.Name = "pnlAging1";
            this.pnlAging1.Size = new System.Drawing.Size(235, 63);
            this.pnlAging1.TabIndex = 0;
            // 
            // lblAge0to30Value
            // 
            this.lblAge0to30Value.AutoSize = true;
            this.lblAge0to30Value.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAge0to30Value.Location = new System.Drawing.Point(14, 31);
            this.lblAge0to30Value.Name = "lblAge0to30Value";
            this.lblAge0to30Value.Size = new System.Drawing.Size(45, 23);
            this.lblAge0to30Value.TabIndex = 1;
            this.lblAge0to30Value.Text = "0.00";
            // 
            // lblAge0to30Title
            // 
            this.lblAge0to30Title.AutoSize = true;
            this.lblAge0to30Title.Location = new System.Drawing.Point(14, 9);
            this.lblAge0to30Title.Name = "lblAge0to30Title";
            this.lblAge0to30Title.Size = new System.Drawing.Size(65, 20);
            this.lblAge0to30Title.TabIndex = 0;
            this.lblAge0to30Title.Text = "0-30 days";
            // 
            // pnlAging2
            // 
            this.pnlAging2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(224)))));
            this.pnlAging2.Controls.Add(this.lblAge31to60Value);
            this.pnlAging2.Controls.Add(this.lblAge31to60Title);
            this.pnlAging2.Location = new System.Drawing.Point(244, 3);
            this.pnlAging2.Name = "pnlAging2";
            this.pnlAging2.Size = new System.Drawing.Size(235, 63);
            this.pnlAging2.TabIndex = 1;
            // 
            // lblAge31to60Value
            // 
            this.lblAge31to60Value.AutoSize = true;
            this.lblAge31to60Value.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAge31to60Value.Location = new System.Drawing.Point(14, 31);
            this.lblAge31to60Value.Name = "lblAge31to60Value";
            this.lblAge31to60Value.Size = new System.Drawing.Size(45, 23);
            this.lblAge31to60Value.TabIndex = 1;
            this.lblAge31to60Value.Text = "0.00";
            // 
            // lblAge31to60Title
            // 
            this.lblAge31to60Title.AutoSize = true;
            this.lblAge31to60Title.Location = new System.Drawing.Point(14, 9);
            this.lblAge31to60Title.Name = "lblAge31to60Title";
            this.lblAge31to60Title.Size = new System.Drawing.Size(72, 20);
            this.lblAge31to60Title.TabIndex = 0;
            this.lblAge31to60Title.Text = "31-60 days";
            // 
            // pnlAging3
            // 
            this.pnlAging3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(178)))));
            this.pnlAging3.Controls.Add(this.lblAge61to90Value);
            this.pnlAging3.Controls.Add(this.lblAge61to90Title);
            this.pnlAging3.Location = new System.Drawing.Point(485, 3);
            this.pnlAging3.Name = "pnlAging3";
            this.pnlAging3.Size = new System.Drawing.Size(235, 63);
            this.pnlAging3.TabIndex = 2;
            // 
            // lblAge61to90Value
            // 
            this.lblAge61to90Value.AutoSize = true;
            this.lblAge61to90Value.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAge61to90Value.Location = new System.Drawing.Point(14, 31);
            this.lblAge61to90Value.Name = "lblAge61to90Value";
            this.lblAge61to90Value.Size = new System.Drawing.Size(45, 23);
            this.lblAge61to90Value.TabIndex = 1;
            this.lblAge61to90Value.Text = "0.00";
            // 
            // lblAge61to90Title
            // 
            this.lblAge61to90Title.AutoSize = true;
            this.lblAge61to90Title.Location = new System.Drawing.Point(14, 9);
            this.lblAge61to90Title.Name = "lblAge61to90Title";
            this.lblAge61to90Title.Size = new System.Drawing.Size(72, 20);
            this.lblAge61to90Title.TabIndex = 0;
            this.lblAge61to90Title.Text = "61-90 days";
            // 
            // pnlAging4
            // 
            this.pnlAging4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.pnlAging4.Controls.Add(this.lblAge90PlusValue);
            this.pnlAging4.Controls.Add(this.lblAge90PlusTitle);
            this.pnlAging4.Location = new System.Drawing.Point(726, 3);
            this.pnlAging4.Name = "pnlAging4";
            this.pnlAging4.Size = new System.Drawing.Size(235, 63);
            this.pnlAging4.TabIndex = 3;
            // 
            // lblAge90PlusValue
            // 
            this.lblAge90PlusValue.AutoSize = true;
            this.lblAge90PlusValue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAge90PlusValue.Location = new System.Drawing.Point(14, 31);
            this.lblAge90PlusValue.Name = "lblAge90PlusValue";
            this.lblAge90PlusValue.Size = new System.Drawing.Size(45, 23);
            this.lblAge90PlusValue.TabIndex = 1;
            this.lblAge90PlusValue.Text = "0.00";
            // 
            // lblAge90PlusTitle
            // 
            this.lblAge90PlusTitle.AutoSize = true;
            this.lblAge90PlusTitle.Location = new System.Drawing.Point(14, 9);
            this.lblAge90PlusTitle.Name = "lblAge90PlusTitle";
            this.lblAge90PlusTitle.Size = new System.Drawing.Size(66, 20);
            this.lblAge90PlusTitle.TabIndex = 0;
            this.lblAge90PlusTitle.Text = "90+ days";
            // 
            // tabNotes
            // 
            this.tabNotes.BackColor = System.Drawing.Color.White;
            this.tabNotes.Controls.Add(this.gridNotes);
            this.tabNotes.Controls.Add(this.pnlNotesTop);
            this.tabNotes.Location = new System.Drawing.Point(4, 38);
            this.tabNotes.Name = "tabNotes";
            this.tabNotes.Padding = new System.Windows.Forms.Padding(8);
            this.tabNotes.Size = new System.Drawing.Size(1016, 574);
            this.tabNotes.TabIndex = 3;
            this.tabNotes.Text = "Notes & History";
            // 
            // gridNotes
            // 
            this.gridNotes.AllowUserToAddRows = false;
            this.gridNotes.AllowUserToDeleteRows = false;
            this.gridNotes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNoteDate,
            this.colNoteText,
            this.colNoteBy});
            this.gridNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNotes.Location = new System.Drawing.Point(8, 52);
            this.gridNotes.Name = "gridNotes";
            this.gridNotes.ReadOnly = true;
            this.gridNotes.RowHeadersVisible = false;
            this.gridNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridNotes.Size = new System.Drawing.Size(1000, 514);
            this.gridNotes.TabIndex = 1;
            // 
            // colNoteDate
            // 
            this.colNoteDate.DataPropertyName = "note_date";
            this.colNoteDate.FillWeight = 25F;
            this.colNoteDate.HeaderText = "Date";
            this.colNoteDate.Name = "colNoteDate";
            this.colNoteDate.ReadOnly = true;
            // 
            // colNoteText
            // 
            this.colNoteText.DataPropertyName = "note_text";
            this.colNoteText.FillWeight = 55F;
            this.colNoteText.HeaderText = "Note";
            this.colNoteText.Name = "colNoteText";
            this.colNoteText.ReadOnly = true;
            // 
            // colNoteBy
            // 
            this.colNoteBy.DataPropertyName = "added_by";
            this.colNoteBy.FillWeight = 20F;
            this.colNoteBy.HeaderText = "Added By";
            this.colNoteBy.Name = "colNoteBy";
            this.colNoteBy.ReadOnly = true;
            // 
            // pnlNotesTop
            // 
            this.pnlNotesTop.Controls.Add(this.btnReminder);
            this.pnlNotesTop.Controls.Add(this.btnAddNote);
            this.pnlNotesTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNotesTop.Location = new System.Drawing.Point(8, 8);
            this.pnlNotesTop.Name = "pnlNotesTop";
            this.pnlNotesTop.Size = new System.Drawing.Size(1000, 44);
            this.pnlNotesTop.TabIndex = 0;
            // 
            // btnReminder
            // 
            this.btnReminder.Location = new System.Drawing.Point(152, 8);
            this.btnReminder.Name = "btnReminder";
            this.btnReminder.Size = new System.Drawing.Size(181, 28);
            this.btnReminder.TabIndex = 1;
            this.btnReminder.Text = "SMS / WhatsApp Reminder";
            this.btnReminder.UseVisualStyleBackColor = true;
            this.btnReminder.Click += new System.EventHandler(this.btnReminder_Click);
            // 
            // btnAddNote
            // 
            this.btnAddNote.Location = new System.Drawing.Point(10, 8);
            this.btnAddNote.Name = "btnAddNote";
            this.btnAddNote.Size = new System.Drawing.Size(136, 28);
            this.btnAddNote.TabIndex = 0;
            this.btnAddNote.Text = "Add Note";
            this.btnAddNote.UseVisualStyleBackColor = true;
            this.btnAddNote.Click += new System.EventHandler(this.btnAddNote_Click);
            // 
            // frm_customer_detail
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 668);
            this.Controls.Add(this.tabProfile);
            this.Controls.Add(this.panelTop);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 620);
            this.Name = "frm_customer_detail";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Customer Profile";
            this.Load += new System.EventHandler(this.frm_customer_detail_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tabProfile.ResumeLayout(false);
            this.tabOverview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).EndInit();
            this.tableKpi.ResumeLayout(false);
            this.pnlKpi1.ResumeLayout(false);
            this.pnlKpi1.PerformLayout();
            this.pnlKpi2.ResumeLayout(false);
            this.pnlKpi2.PerformLayout();
            this.pnlKpi3.ResumeLayout(false);
            this.pnlKpi3.PerformLayout();
            this.pnlKpi4.ResumeLayout(false);
            this.pnlKpi4.PerformLayout();
            this.pnlOverviewHeader.ResumeLayout(false);
            this.pnlOverviewHeader.PerformLayout();
            this.tabLedger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLedger)).EndInit();
            this.pnlLedgerFooter.ResumeLayout(false);
            this.pnlLedgerFooter.PerformLayout();
            this.pnlLedgerFilter.ResumeLayout(false);
            this.pnlLedgerFilter.PerformLayout();
            this.tabOutstanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOutstanding)).EndInit();
            this.pnlOutstandingBottom.ResumeLayout(false);
            this.flowAging.ResumeLayout(false);
            this.pnlAging1.ResumeLayout(false);
            this.pnlAging1.PerformLayout();
            this.pnlAging2.ResumeLayout(false);
            this.pnlAging2.PerformLayout();
            this.pnlAging3.ResumeLayout(false);
            this.pnlAging3.PerformLayout();
            this.pnlAging4.ResumeLayout(false);
            this.pnlAging4.PerformLayout();
            this.tabNotes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNotes)).EndInit();
            this.pnlNotesTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.TabControl tabProfile;
        private System.Windows.Forms.TabPage tabOverview;
        private System.Windows.Forms.Panel pnlOverviewHeader;
        private System.Windows.Forms.Panel pnlAvatar;
        private System.Windows.Forms.Label lblHeaderName;
        private System.Windows.Forms.Label lblHeaderCode;
        private System.Windows.Forms.Label lblHeaderArea;
        private System.Windows.Forms.Label lblHeaderPhone;
        private System.Windows.Forms.Label lblHeaderEmail;
        private System.Windows.Forms.Label lblHeaderCreditLimit;
        private System.Windows.Forms.CheckBox chkAccountStatus;
        private System.Windows.Forms.TableLayoutPanel tableKpi;
        private System.Windows.Forms.Panel pnlKpi1;
        private System.Windows.Forms.Label lblLifetimeSalesValue;
        private System.Windows.Forms.Label lblLifetimeSalesTitle;
        private System.Windows.Forms.Panel pnlKpi2;
        private System.Windows.Forms.Label lblTotalPaidValue;
        private System.Windows.Forms.Label lblTotalPaidTitle;
        private System.Windows.Forms.Panel pnlKpi3;
        private System.Windows.Forms.Label lblOutstandingValue;
        private System.Windows.Forms.Label lblOutstandingTitle;
        private System.Windows.Forms.Panel pnlKpi4;
        private System.Windows.Forms.Label lblAvailableCreditValue;
        private System.Windows.Forms.Label lblAvailableCreditTitle;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthly;
        private System.Windows.Forms.TabPage tabLedger;
        private System.Windows.Forms.Panel pnlLedgerFilter;
        private System.Windows.Forms.DateTimePicker dtLedgerTo;
        private System.Windows.Forms.Label lblLedgerTo;
        private System.Windows.Forms.DateTimePicker dtLedgerFrom;
        private System.Windows.Forms.Label lblLedgerFrom;
        private System.Windows.Forms.Button btnLoadLedger;
        private System.Windows.Forms.Button btnPrintLedger;
        private System.Windows.Forms.DataGridView gridLedger;
        private System.Windows.Forms.Panel pnlLedgerFooter;
        private System.Windows.Forms.Label lblLedgerTotals;
        private System.Windows.Forms.TabPage tabOutstanding;
        private System.Windows.Forms.FlowLayoutPanel flowAging;
        private System.Windows.Forms.Panel pnlAging1;
        private System.Windows.Forms.Label lblAge0to30Value;
        private System.Windows.Forms.Label lblAge0to30Title;
        private System.Windows.Forms.Panel pnlAging2;
        private System.Windows.Forms.Label lblAge31to60Value;
        private System.Windows.Forms.Label lblAge31to60Title;
        private System.Windows.Forms.Panel pnlAging3;
        private System.Windows.Forms.Label lblAge61to90Value;
        private System.Windows.Forms.Label lblAge61to90Title;
        private System.Windows.Forms.Panel pnlAging4;
        private System.Windows.Forms.Label lblAge90PlusValue;
        private System.Windows.Forms.Label lblAge90PlusTitle;
        private System.Windows.Forms.DataGridView gridOutstanding;
        private System.Windows.Forms.Panel pnlOutstandingBottom;
        private System.Windows.Forms.Button btnReceivePayment;
        private System.Windows.Forms.TabPage tabNotes;
        private System.Windows.Forms.Panel pnlNotesTop;
        private System.Windows.Forms.Button btnAddNote;
        private System.Windows.Forms.Button btnReminder;
        private System.Windows.Forms.DataGridView gridNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNoteText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNoteBy;
    }
}

