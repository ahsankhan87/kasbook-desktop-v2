namespace pos.Accounts
{
    partial class frm_import_data
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabChartOfAccounts = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnImportCOA = new System.Windows.Forms.Button();
            this.btnUploadCOA = new System.Windows.Forms.Button();
            this.btnDownloadCOATemplate = new System.Windows.Forms.Button();
            this.gridCOA = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tabOpeningBalance = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblOBBalance = new System.Windows.Forms.Label();
            this.btnPostOB = new System.Windows.Forms.Button();
            this.btnUploadOB = new System.Windows.Forms.Button();
            this.btnDownloadOBTemplate = new System.Windows.Forms.Button();
            this.gridOB = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpOBDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPartyBalances = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnImportParty = new System.Windows.Forms.Button();
            this.btnUploadParty = new System.Windows.Forms.Button();
            this.btnDownloadPartyTemplate = new System.Windows.Forms.Button();
            this.gridParty = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.rbCustomers = new System.Windows.Forms.RadioButton();
            this.rbSuppliers = new System.Windows.Forms.RadioButton();
            this.tabJournalHistory = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblJournalSummary = new System.Windows.Forms.Label();
            this.btnImportJournal = new System.Windows.Forms.Button();
            this.btnUploadJournal = new System.Windows.Forms.Button();
            this.btnDownloadJournalTemplate = new System.Windows.Forms.Button();
            this.gridJournal = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.tabImportHistory = new System.Windows.Forms.TabPage();
            this.btnRollback = new System.Windows.Forms.Button();
            this.btnRefreshHistory = new System.Windows.Forms.Button();
            this.gridHistory = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.chkDryRun = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabChartOfAccounts.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCOA)).BeginInit();
            this.tabOpeningBalance.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOB)).BeginInit();
            this.tabPartyBalances.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridParty)).BeginInit();
            this.tabJournalHistory.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridJournal)).BeginInit();
            this.tabImportHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabChartOfAccounts);
            this.tabControl1.Controls.Add(this.tabOpeningBalance);
            this.tabControl1.Controls.Add(this.tabPartyBalances);
            this.tabControl1.Controls.Add(this.tabJournalHistory);
            this.tabControl1.Controls.Add(this.tabImportHistory);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1160, 586);
            this.tabControl1.TabIndex = 0;
            // 
            // tabChartOfAccounts
            // 
            this.tabChartOfAccounts.Controls.Add(this.panel1);
            this.tabChartOfAccounts.Controls.Add(this.gridCOA);
            this.tabChartOfAccounts.Controls.Add(this.label1);
            this.tabChartOfAccounts.Location = new System.Drawing.Point(4, 26);
            this.tabChartOfAccounts.Name = "tabChartOfAccounts";
            this.tabChartOfAccounts.Padding = new System.Windows.Forms.Padding(3);
            this.tabChartOfAccounts.Size = new System.Drawing.Size(1152, 556);
            this.tabChartOfAccounts.TabIndex = 0;
            this.tabChartOfAccounts.Text = "1. Chart of Accounts";
            this.tabChartOfAccounts.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel1.Controls.Add(this.btnImportCOA);
            this.panel1.Controls.Add(this.btnUploadCOA);
            this.panel1.Controls.Add(this.btnDownloadCOATemplate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 31);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(1146, 60);
            this.panel1.TabIndex = 2;
            // 
            // btnImportCOA
            // 
            this.btnImportCOA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCOA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportCOA.Enabled = false;
            this.btnImportCOA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportCOA.ForeColor = System.Drawing.Color.White;
            this.btnImportCOA.Location = new System.Drawing.Point(1026, 13);
            this.btnImportCOA.Name = "btnImportCOA";
            this.btnImportCOA.Size = new System.Drawing.Size(110, 35);
            this.btnImportCOA.TabIndex = 2;
            this.btnImportCOA.Text = "Import";
            this.btnImportCOA.UseVisualStyleBackColor = false;
            this.btnImportCOA.Click += new System.EventHandler(this.btnImportCOA_Click);
            // 
            // btnUploadCOA
            // 
            this.btnUploadCOA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadCOA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadCOA.ForeColor = System.Drawing.Color.White;
            this.btnUploadCOA.Location = new System.Drawing.Point(220, 13);
            this.btnUploadCOA.Name = "btnUploadCOA";
            this.btnUploadCOA.Size = new System.Drawing.Size(200, 35);
            this.btnUploadCOA.TabIndex = 1;
            this.btnUploadCOA.Text = "📁 Upload File";
            this.btnUploadCOA.UseVisualStyleBackColor = false;
            this.btnUploadCOA.Click += new System.EventHandler(this.btnUploadCOA_Click);
            // 
            // btnDownloadCOATemplate
            // 
            this.btnDownloadCOATemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadCOATemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadCOATemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadCOATemplate.Location = new System.Drawing.Point(13, 13);
            this.btnDownloadCOATemplate.Name = "btnDownloadCOATemplate";
            this.btnDownloadCOATemplate.Size = new System.Drawing.Size(200, 35);
            this.btnDownloadCOATemplate.TabIndex = 0;
            this.btnDownloadCOATemplate.Text = "⬇ Download Template";
            this.btnDownloadCOATemplate.UseVisualStyleBackColor = false;
            this.btnDownloadCOATemplate.Click += new System.EventHandler(this.btnDownloadCOATemplate_Click);
            // 
            // gridCOA
            // 
            this.gridCOA.AllowUserToAddRows = false;
            this.gridCOA.AllowUserToDeleteRows = false;
            this.gridCOA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCOA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridCOA.BackgroundColor = System.Drawing.Color.White;
            this.gridCOA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCOA.Location = new System.Drawing.Point(6, 97);
            this.gridCOA.Name = "gridCOA";
            this.gridCOA.ReadOnly = true;
            this.gridCOA.Size = new System.Drawing.Size(1140, 453);
            this.gridCOA.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(1146, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Import Chart of Accounts from Excel. Template includes: Account Code, Name, Type" +
    ", Parent Code, Opening Balance, Bank Details.";
            // 
            // tabOpeningBalance
            // 
            this.tabOpeningBalance.Controls.Add(this.panel2);
            this.tabOpeningBalance.Controls.Add(this.gridOB);
            this.tabOpeningBalance.Controls.Add(this.label2);
            this.tabOpeningBalance.Controls.Add(this.dtpOBDate);
            this.tabOpeningBalance.Controls.Add(this.label3);
            this.tabOpeningBalance.Location = new System.Drawing.Point(4, 26);
            this.tabOpeningBalance.Name = "tabOpeningBalance";
            this.tabOpeningBalance.Padding = new System.Windows.Forms.Padding(3);
            this.tabOpeningBalance.Size = new System.Drawing.Size(1152, 556);
            this.tabOpeningBalance.TabIndex = 1;
            this.tabOpeningBalance.Text = "2. Opening Balances";
            this.tabOpeningBalance.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel2.Controls.Add(this.lblOBBalance);
            this.panel2.Controls.Add(this.btnPostOB);
            this.panel2.Controls.Add(this.btnUploadOB);
            this.panel2.Controls.Add(this.btnDownloadOBTemplate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 64);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(1146, 60);
            this.panel2.TabIndex = 3;
            // 
            // lblOBBalance
            // 
            this.lblOBBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOBBalance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblOBBalance.Location = new System.Drawing.Point(650, 13);
            this.lblOBBalance.Name = "lblOBBalance";
            this.lblOBBalance.Size = new System.Drawing.Size(370, 35);
            this.lblOBBalance.TabIndex = 3;
            this.lblOBBalance.Text = "Dr: 0.00 | Cr: 0.00 | Diff: 0.00";
            this.lblOBBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPostOB
            // 
            this.btnPostOB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostOB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnPostOB.Enabled = false;
            this.btnPostOB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPostOB.ForeColor = System.Drawing.Color.White;
            this.btnPostOB.Location = new System.Drawing.Point(1026, 13);
            this.btnPostOB.Name = "btnPostOB";
            this.btnPostOB.Size = new System.Drawing.Size(110, 35);
            this.btnPostOB.TabIndex = 2;
            this.btnPostOB.Text = "Post";
            this.btnPostOB.UseVisualStyleBackColor = false;
            this.btnPostOB.Click += new System.EventHandler(this.btnPostOB_Click);
            // 
            // btnUploadOB
            // 
            this.btnUploadOB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadOB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadOB.ForeColor = System.Drawing.Color.White;
            this.btnUploadOB.Location = new System.Drawing.Point(220, 13);
            this.btnUploadOB.Name = "btnUploadOB";
            this.btnUploadOB.Size = new System.Drawing.Size(200, 35);
            this.btnUploadOB.TabIndex = 1;
            this.btnUploadOB.Text = "📁 Upload File";
            this.btnUploadOB.UseVisualStyleBackColor = false;
            this.btnUploadOB.Click += new System.EventHandler(this.btnUploadOB_Click);
            // 
            // btnDownloadOBTemplate
            // 
            this.btnDownloadOBTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadOBTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadOBTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadOBTemplate.Location = new System.Drawing.Point(13, 13);
            this.btnDownloadOBTemplate.Name = "btnDownloadOBTemplate";
            this.btnDownloadOBTemplate.Size = new System.Drawing.Size(200, 35);
            this.btnDownloadOBTemplate.TabIndex = 0;
            this.btnDownloadOBTemplate.Text = "⬇ Download Template";
            this.btnDownloadOBTemplate.UseVisualStyleBackColor = false;
            this.btnDownloadOBTemplate.Click += new System.EventHandler(this.btnDownloadOBTemplate_Click);
            // 
            // gridOB
            // 
            this.gridOB.AllowUserToAddRows = false;
            this.gridOB.AllowUserToDeleteRows = false;
            this.gridOB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridOB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridOB.BackgroundColor = System.Drawing.Color.White;
            this.gridOB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOB.Location = new System.Drawing.Point(6, 130);
            this.gridOB.Name = "gridOB";
            this.gridOB.ReadOnly = true;
            this.gridOB.Size = new System.Drawing.Size(1140, 420);
            this.gridOB.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(5);
            this.label2.Size = new System.Drawing.Size(1146, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Import opening balances for existing accounts. Total Debit must equal Total Cred" +
    "it.";
            // 
            // dtpOBDate
            // 
            this.dtpOBDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpOBDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOBDate.Location = new System.Drawing.Point(150, 34);
            this.dtpOBDate.Name = "dtpOBDate";
            this.dtpOBDate.Size = new System.Drawing.Size(150, 25);
            this.dtpOBDate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Opening Balance Date:";
            // 
            // tabPartyBalances
            // 
            this.tabPartyBalances.Controls.Add(this.panel3);
            this.tabPartyBalances.Controls.Add(this.gridParty);
            this.tabPartyBalances.Controls.Add(this.label4);
            this.tabPartyBalances.Controls.Add(this.rbCustomers);
            this.tabPartyBalances.Controls.Add(this.rbSuppliers);
            this.tabPartyBalances.Location = new System.Drawing.Point(4, 26);
            this.tabPartyBalances.Name = "tabPartyBalances";
            this.tabPartyBalances.Size = new System.Drawing.Size(1152, 556);
            this.tabPartyBalances.TabIndex = 2;
            this.tabPartyBalances.Text = "3. Customer/Supplier Balances";
            this.tabPartyBalances.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel3.Controls.Add(this.btnImportParty);
            this.panel3.Controls.Add(this.btnUploadParty);
            this.panel3.Controls.Add(this.btnDownloadPartyTemplate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 64);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(1152, 60);
            this.panel3.TabIndex = 4;
            // 
            // btnImportParty
            // 
            this.btnImportParty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportParty.Enabled = false;
            this.btnImportParty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportParty.ForeColor = System.Drawing.Color.White;
            this.btnImportParty.Location = new System.Drawing.Point(1032, 13);
            this.btnImportParty.Name = "btnImportParty";
            this.btnImportParty.Size = new System.Drawing.Size(110, 35);
            this.btnImportParty.TabIndex = 2;
            this.btnImportParty.Text = "Import";
            this.btnImportParty.UseVisualStyleBackColor = false;
            // 
            // btnUploadParty
            // 
            this.btnUploadParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadParty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadParty.ForeColor = System.Drawing.Color.White;
            this.btnUploadParty.Location = new System.Drawing.Point(220, 13);
            this.btnUploadParty.Name = "btnUploadParty";
            this.btnUploadParty.Size = new System.Drawing.Size(200, 35);
            this.btnUploadParty.TabIndex = 1;
            this.btnUploadParty.Text = "📁 Upload File";
            this.btnUploadParty.UseVisualStyleBackColor = false;
            // 
            // btnDownloadPartyTemplate
            // 
            this.btnDownloadPartyTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadPartyTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadPartyTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadPartyTemplate.Location = new System.Drawing.Point(13, 13);
            this.btnDownloadPartyTemplate.Name = "btnDownloadPartyTemplate";
            this.btnDownloadPartyTemplate.Size = new System.Drawing.Size(200, 35);
            this.btnDownloadPartyTemplate.TabIndex = 0;
            this.btnDownloadPartyTemplate.Text = "⬇ Download Template";
            this.btnDownloadPartyTemplate.UseVisualStyleBackColor = false;
            // 
            // gridParty
            // 
            this.gridParty.AllowUserToAddRows = false;
            this.gridParty.AllowUserToDeleteRows = false;
            this.gridParty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridParty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridParty.BackgroundColor = System.Drawing.Color.White;
            this.gridParty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridParty.Location = new System.Drawing.Point(6, 130);
            this.gridParty.Name = "gridParty";
            this.gridParty.ReadOnly = true;
            this.gridParty.Size = new System.Drawing.Size(1140, 420);
            this.gridParty.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(5);
            this.label4.Size = new System.Drawing.Size(1152, 28);
            this.label4.TabIndex = 2;
            this.label4.Text = "Import outstanding customer or supplier balances as opening invoices.";
            // 
            // rbCustomers
            // 
            this.rbCustomers.AutoSize = true;
            this.rbCustomers.Checked = true;
            this.rbCustomers.Location = new System.Drawing.Point(13, 37);
            this.rbCustomers.Name = "rbCustomers";
            this.rbCustomers.Size = new System.Drawing.Size(90, 21);
            this.rbCustomers.TabIndex = 5;
            this.rbCustomers.TabStop = true;
            this.rbCustomers.Text = "Customers";
            this.rbCustomers.UseVisualStyleBackColor = true;
            // 
            // rbSuppliers
            // 
            this.rbSuppliers.AutoSize = true;
            this.rbSuppliers.Location = new System.Drawing.Point(120, 37);
            this.rbSuppliers.Name = "rbSuppliers";
            this.rbSuppliers.Size = new System.Drawing.Size(81, 21);
            this.rbSuppliers.TabIndex = 6;
            this.rbSuppliers.Text = "Suppliers";
            this.rbSuppliers.UseVisualStyleBackColor = true;
            // 
            // tabJournalHistory
            // 
            this.tabJournalHistory.Controls.Add(this.panel4);
            this.tabJournalHistory.Controls.Add(this.gridJournal);
            this.tabJournalHistory.Controls.Add(this.label5);
            this.tabJournalHistory.Location = new System.Drawing.Point(4, 26);
            this.tabJournalHistory.Name = "tabJournalHistory";
            this.tabJournalHistory.Size = new System.Drawing.Size(1152, 556);
            this.tabJournalHistory.TabIndex = 3;
            this.tabJournalHistory.Text = "4. Historical Journals";
            this.tabJournalHistory.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel4.Controls.Add(this.lblJournalSummary);
            this.panel4.Controls.Add(this.btnImportJournal);
            this.panel4.Controls.Add(this.btnUploadJournal);
            this.panel4.Controls.Add(this.btnDownloadJournalTemplate);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 28);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(10);
            this.panel4.Size = new System.Drawing.Size(1152, 60);
            this.panel4.TabIndex = 4;
            // 
            // lblJournalSummary
            // 
            this.lblJournalSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblJournalSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblJournalSummary.Location = new System.Drawing.Point(656, 13);
            this.lblJournalSummary.Name = "lblJournalSummary";
            this.lblJournalSummary.Size = new System.Drawing.Size(370, 35);
            this.lblJournalSummary.TabIndex = 3;
            this.lblJournalSummary.Text = "Vouchers: 0 | Entries: 0 | Balanced: 0";
            this.lblJournalSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnImportJournal
            // 
            this.btnImportJournal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportJournal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportJournal.Enabled = false;
            this.btnImportJournal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportJournal.ForeColor = System.Drawing.Color.White;
            this.btnImportJournal.Location = new System.Drawing.Point(1032, 13);
            this.btnImportJournal.Name = "btnImportJournal";
            this.btnImportJournal.Size = new System.Drawing.Size(110, 35);
            this.btnImportJournal.TabIndex = 2;
            this.btnImportJournal.Text = "Import";
            this.btnImportJournal.UseVisualStyleBackColor = false;
            this.btnImportJournal.Click += new System.EventHandler(this.btnImportJournal_Click);
            // 
            // btnUploadJournal
            // 
            this.btnUploadJournal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadJournal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadJournal.ForeColor = System.Drawing.Color.White;
            this.btnUploadJournal.Location = new System.Drawing.Point(220, 13);
            this.btnUploadJournal.Name = "btnUploadJournal";
            this.btnUploadJournal.Size = new System.Drawing.Size(200, 35);
            this.btnUploadJournal.TabIndex = 1;
            this.btnUploadJournal.Text = "📁 Upload File";
            this.btnUploadJournal.UseVisualStyleBackColor = false;
            this.btnUploadJournal.Click += new System.EventHandler(this.btnUploadJournal_Click);
            // 
            // btnDownloadJournalTemplate
            // 
            this.btnDownloadJournalTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadJournalTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadJournalTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadJournalTemplate.Location = new System.Drawing.Point(13, 13);
            this.btnDownloadJournalTemplate.Name = "btnDownloadJournalTemplate";
            this.btnDownloadJournalTemplate.Size = new System.Drawing.Size(200, 35);
            this.btnDownloadJournalTemplate.TabIndex = 0;
            this.btnDownloadJournalTemplate.Text = "⬇ Download Template";
            this.btnDownloadJournalTemplate.UseVisualStyleBackColor = false;
            this.btnDownloadJournalTemplate.Click += new System.EventHandler(this.btnDownloadJournalTemplate_Click);
            // 
            // gridJournal
            // 
            this.gridJournal.AllowUserToAddRows = false;
            this.gridJournal.AllowUserToDeleteRows = false;
            this.gridJournal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridJournal.BackgroundColor = System.Drawing.Color.White;
            this.gridJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJournal.Location = new System.Drawing.Point(6, 94);
            this.gridJournal.Name = "gridJournal";
            this.gridJournal.ReadOnly = true;
            this.gridJournal.Size = new System.Drawing.Size(1140, 456);
            this.gridJournal.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(5);
            this.label5.Size = new System.Drawing.Size(1152, 28);
            this.label5.TabIndex = 2;
            this.label5.Text = "Import historical journal entries from old system. Each voucher must be balanced" +
    ".";
            // 
            // tabImportHistory
            // 
            this.tabImportHistory.Controls.Add(this.btnRollback);
            this.tabImportHistory.Controls.Add(this.btnRefreshHistory);
            this.tabImportHistory.Controls.Add(this.gridHistory);
            this.tabImportHistory.Controls.Add(this.label6);
            this.tabImportHistory.Location = new System.Drawing.Point(4, 26);
            this.tabImportHistory.Name = "tabImportHistory";
            this.tabImportHistory.Size = new System.Drawing.Size(1152, 556);
            this.tabImportHistory.TabIndex = 4;
            this.tabImportHistory.Text = "📊 Import History & Rollback";
            this.tabImportHistory.UseVisualStyleBackColor = true;
            // 
            // btnRollback
            // 
            this.btnRollback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRollback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnRollback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRollback.ForeColor = System.Drawing.Color.White;
            this.btnRollback.Location = new System.Drawing.Point(1026, 34);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(120, 35);
            this.btnRollback.TabIndex = 3;
            this.btnRollback.Text = "Rollback Selected";
            this.btnRollback.UseVisualStyleBackColor = false;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // btnRefreshHistory
            // 
            this.btnRefreshHistory.Location = new System.Drawing.Point(6, 34);
            this.btnRefreshHistory.Name = "btnRefreshHistory";
            this.btnRefreshHistory.Size = new System.Drawing.Size(120, 35);
            this.btnRefreshHistory.TabIndex = 2;
            this.btnRefreshHistory.Text = "🔄 Refresh";
            this.btnRefreshHistory.UseVisualStyleBackColor = true;
            this.btnRefreshHistory.Click += new System.EventHandler(this.btnRefreshHistory_Click);
            // 
            // gridHistory
            // 
            this.gridHistory.AllowUserToAddRows = false;
            this.gridHistory.AllowUserToDeleteRows = false;
            this.gridHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridHistory.BackgroundColor = System.Drawing.Color.White;
            this.gridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridHistory.Location = new System.Drawing.Point(6, 75);
            this.gridHistory.MultiSelect = false;
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.ReadOnly = true;
            this.gridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridHistory.Size = new System.Drawing.Size(1140, 475);
            this.gridHistory.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(5);
            this.label6.Size = new System.Drawing.Size(1152, 28);
            this.label6.TabIndex = 0;
            this.label6.Text = "View import history and rollback imports within 24 hours.";
            // 
            // chkDryRun
            // 
            this.chkDryRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDryRun.AutoSize = true;
            this.chkDryRun.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.chkDryRun.Location = new System.Drawing.Point(16, 609);
            this.chkDryRun.Name = "chkDryRun";
            this.chkDryRun.Size = new System.Drawing.Size(288, 21);
            this.chkDryRun.TabIndex = 1;
            this.chkDryRun.Text = "🛡 Dry Run Mode (validate only, don\'t import)";
            this.chkDryRun.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(1062, 604);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frm_import_data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 651);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkDryRun);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "frm_import_data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opening Balance & Historical Data Import";
            this.Load += new System.EventHandler(this.frm_import_data_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabChartOfAccounts.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCOA)).EndInit();
            this.tabOpeningBalance.ResumeLayout(false);
            this.tabOpeningBalance.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOB)).EndInit();
            this.tabPartyBalances.ResumeLayout(false);
            this.tabPartyBalances.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridParty)).EndInit();
            this.tabJournalHistory.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridJournal)).EndInit();
            this.tabImportHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabChartOfAccounts;
        private System.Windows.Forms.TabPage tabOpeningBalance;
        private System.Windows.Forms.TabPage tabPartyBalances;
        private System.Windows.Forms.TabPage tabJournalHistory;
        private System.Windows.Forms.TabPage tabImportHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gridCOA;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDownloadCOATemplate;
        private System.Windows.Forms.Button btnUploadCOA;
        private System.Windows.Forms.Button btnImportCOA;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPostOB;
        private System.Windows.Forms.Button btnUploadOB;
        private System.Windows.Forms.Button btnDownloadOBTemplate;
        private System.Windows.Forms.DataGridView gridOB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpOBDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOBBalance;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnImportParty;
        private System.Windows.Forms.Button btnUploadParty;
        private System.Windows.Forms.Button btnDownloadPartyTemplate;
        private System.Windows.Forms.DataGridView gridParty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbCustomers;
        private System.Windows.Forms.RadioButton rbSuppliers;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblJournalSummary;
        private System.Windows.Forms.Button btnImportJournal;
        private System.Windows.Forms.Button btnUploadJournal;
        private System.Windows.Forms.Button btnDownloadJournalTemplate;
        private System.Windows.Forms.DataGridView gridJournal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRefreshHistory;
        private System.Windows.Forms.DataGridView gridHistory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkDryRun;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRollback;
    }
}
