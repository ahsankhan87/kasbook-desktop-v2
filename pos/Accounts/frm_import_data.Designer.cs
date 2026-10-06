using pos.UI;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_import_data));
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
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabChartOfAccounts);
            this.tabControl1.Controls.Add(this.tabOpeningBalance);
            this.tabControl1.Controls.Add(this.tabPartyBalances);
            this.tabControl1.Controls.Add(this.tabJournalHistory);
            this.tabControl1.Controls.Add(this.tabImportHistory);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabChartOfAccounts
            // 
            resources.ApplyResources(this.tabChartOfAccounts, "tabChartOfAccounts");
            this.tabChartOfAccounts.Controls.Add(this.panel1);
            this.tabChartOfAccounts.Controls.Add(this.gridCOA);
            this.tabChartOfAccounts.Controls.Add(this.label1);
            this.tabChartOfAccounts.Name = "tabChartOfAccounts";
            this.tabChartOfAccounts.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel1.Controls.Add(this.btnImportCOA);
            this.panel1.Controls.Add(this.btnUploadCOA);
            this.panel1.Controls.Add(this.btnDownloadCOATemplate);
            this.panel1.Name = "panel1";
            // 
            // btnImportCOA
            // 
            resources.ApplyResources(this.btnImportCOA, "btnImportCOA");
            this.btnImportCOA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportCOA.ForeColor = System.Drawing.Color.White;
            this.btnImportCOA.Name = "btnImportCOA";
            this.btnImportCOA.UseVisualStyleBackColor = false;
            this.btnImportCOA.Click += new System.EventHandler(this.btnImportCOA_Click);
            // 
            // btnUploadCOA
            // 
            resources.ApplyResources(this.btnUploadCOA, "btnUploadCOA");
            this.btnUploadCOA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadCOA.ForeColor = System.Drawing.Color.White;
            this.btnUploadCOA.Name = "btnUploadCOA";
            this.btnUploadCOA.UseVisualStyleBackColor = false;
            this.btnUploadCOA.Click += new System.EventHandler(this.btnUploadCOA_Click);
            // 
            // btnDownloadCOATemplate
            // 
            resources.ApplyResources(this.btnDownloadCOATemplate, "btnDownloadCOATemplate");
            this.btnDownloadCOATemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadCOATemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadCOATemplate.Name = "btnDownloadCOATemplate";
            this.btnDownloadCOATemplate.UseVisualStyleBackColor = false;
            this.btnDownloadCOATemplate.Click += new System.EventHandler(this.btnDownloadCOATemplate_Click);
            // 
            // gridCOA
            // 
            resources.ApplyResources(this.gridCOA, "gridCOA");
            this.gridCOA.AllowUserToAddRows = false;
            this.gridCOA.AllowUserToDeleteRows = false;
            this.gridCOA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridCOA.BackgroundColor = System.Drawing.Color.White;
            this.gridCOA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCOA.Name = "gridCOA";
            this.gridCOA.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabOpeningBalance
            // 
            resources.ApplyResources(this.tabOpeningBalance, "tabOpeningBalance");
            this.tabOpeningBalance.Controls.Add(this.panel2);
            this.tabOpeningBalance.Controls.Add(this.gridOB);
            this.tabOpeningBalance.Controls.Add(this.label2);
            this.tabOpeningBalance.Controls.Add(this.dtpOBDate);
            this.tabOpeningBalance.Controls.Add(this.label3);
            this.tabOpeningBalance.Name = "tabOpeningBalance";
            this.tabOpeningBalance.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel2.Controls.Add(this.lblOBBalance);
            this.panel2.Controls.Add(this.btnPostOB);
            this.panel2.Controls.Add(this.btnUploadOB);
            this.panel2.Controls.Add(this.btnDownloadOBTemplate);
            this.panel2.Name = "panel2";
            // 
            // lblOBBalance
            // 
            resources.ApplyResources(this.lblOBBalance, "lblOBBalance");
            this.lblOBBalance.Name = "lblOBBalance";
            // 
            // btnPostOB
            // 
            resources.ApplyResources(this.btnPostOB, "btnPostOB");
            this.btnPostOB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnPostOB.ForeColor = System.Drawing.Color.White;
            this.btnPostOB.Name = "btnPostOB";
            this.btnPostOB.UseVisualStyleBackColor = false;
            this.btnPostOB.Click += new System.EventHandler(this.btnPostOB_Click);
            // 
            // btnUploadOB
            // 
            resources.ApplyResources(this.btnUploadOB, "btnUploadOB");
            this.btnUploadOB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadOB.ForeColor = System.Drawing.Color.White;
            this.btnUploadOB.Name = "btnUploadOB";
            this.btnUploadOB.UseVisualStyleBackColor = false;
            this.btnUploadOB.Click += new System.EventHandler(this.btnUploadOB_Click);
            // 
            // btnDownloadOBTemplate
            // 
            resources.ApplyResources(this.btnDownloadOBTemplate, "btnDownloadOBTemplate");
            this.btnDownloadOBTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadOBTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadOBTemplate.Name = "btnDownloadOBTemplate";
            this.btnDownloadOBTemplate.UseVisualStyleBackColor = false;
            this.btnDownloadOBTemplate.Click += new System.EventHandler(this.btnDownloadOBTemplate_Click);
            // 
            // gridOB
            // 
            resources.ApplyResources(this.gridOB, "gridOB");
            this.gridOB.AllowUserToAddRows = false;
            this.gridOB.AllowUserToDeleteRows = false;
            this.gridOB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridOB.BackgroundColor = System.Drawing.Color.White;
            this.gridOB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOB.Name = "gridOB";
            this.gridOB.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // dtpOBDate
            // 
            resources.ApplyResources(this.dtpOBDate, "dtpOBDate");
            this.dtpOBDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOBDate.Name = "dtpOBDate";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tabPartyBalances
            // 
            resources.ApplyResources(this.tabPartyBalances, "tabPartyBalances");
            this.tabPartyBalances.Controls.Add(this.panel3);
            this.tabPartyBalances.Controls.Add(this.gridParty);
            this.tabPartyBalances.Controls.Add(this.label4);
            this.tabPartyBalances.Controls.Add(this.rbCustomers);
            this.tabPartyBalances.Controls.Add(this.rbSuppliers);
            this.tabPartyBalances.Name = "tabPartyBalances";
            this.tabPartyBalances.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel3.Controls.Add(this.btnImportParty);
            this.panel3.Controls.Add(this.btnUploadParty);
            this.panel3.Controls.Add(this.btnDownloadPartyTemplate);
            this.panel3.Name = "panel3";
            // 
            // btnImportParty
            // 
            resources.ApplyResources(this.btnImportParty, "btnImportParty");
            this.btnImportParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportParty.ForeColor = System.Drawing.Color.White;
            this.btnImportParty.Name = "btnImportParty";
            this.btnImportParty.UseVisualStyleBackColor = false;
            // 
            // btnUploadParty
            // 
            resources.ApplyResources(this.btnUploadParty, "btnUploadParty");
            this.btnUploadParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadParty.ForeColor = System.Drawing.Color.White;
            this.btnUploadParty.Name = "btnUploadParty";
            this.btnUploadParty.UseVisualStyleBackColor = false;
            // 
            // btnDownloadPartyTemplate
            // 
            resources.ApplyResources(this.btnDownloadPartyTemplate, "btnDownloadPartyTemplate");
            this.btnDownloadPartyTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadPartyTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadPartyTemplate.Name = "btnDownloadPartyTemplate";
            this.btnDownloadPartyTemplate.UseVisualStyleBackColor = false;
            // 
            // gridParty
            // 
            resources.ApplyResources(this.gridParty, "gridParty");
            this.gridParty.AllowUserToAddRows = false;
            this.gridParty.AllowUserToDeleteRows = false;
            this.gridParty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridParty.BackgroundColor = System.Drawing.Color.White;
            this.gridParty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridParty.Name = "gridParty";
            this.gridParty.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // rbCustomers
            // 
            resources.ApplyResources(this.rbCustomers, "rbCustomers");
            this.rbCustomers.Checked = true;
            this.rbCustomers.Name = "rbCustomers";
            this.rbCustomers.TabStop = true;
            this.rbCustomers.UseVisualStyleBackColor = true;
            // 
            // rbSuppliers
            // 
            resources.ApplyResources(this.rbSuppliers, "rbSuppliers");
            this.rbSuppliers.Name = "rbSuppliers";
            this.rbSuppliers.UseVisualStyleBackColor = true;
            // 
            // tabJournalHistory
            // 
            resources.ApplyResources(this.tabJournalHistory, "tabJournalHistory");
            this.tabJournalHistory.Controls.Add(this.panel4);
            this.tabJournalHistory.Controls.Add(this.gridJournal);
            this.tabJournalHistory.Controls.Add(this.label5);
            this.tabJournalHistory.Name = "tabJournalHistory";
            this.tabJournalHistory.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel4.Controls.Add(this.lblJournalSummary);
            this.panel4.Controls.Add(this.btnImportJournal);
            this.panel4.Controls.Add(this.btnUploadJournal);
            this.panel4.Controls.Add(this.btnDownloadJournalTemplate);
            this.panel4.Name = "panel4";
            // 
            // lblJournalSummary
            // 
            resources.ApplyResources(this.lblJournalSummary, "lblJournalSummary");
            this.lblJournalSummary.Name = "lblJournalSummary";
            // 
            // btnImportJournal
            // 
            resources.ApplyResources(this.btnImportJournal, "btnImportJournal");
            this.btnImportJournal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnImportJournal.ForeColor = System.Drawing.Color.White;
            this.btnImportJournal.Name = "btnImportJournal";
            this.btnImportJournal.UseVisualStyleBackColor = false;
            this.btnImportJournal.Click += new System.EventHandler(this.btnImportJournal_Click);
            // 
            // btnUploadJournal
            // 
            resources.ApplyResources(this.btnUploadJournal, "btnUploadJournal");
            this.btnUploadJournal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnUploadJournal.ForeColor = System.Drawing.Color.White;
            this.btnUploadJournal.Name = "btnUploadJournal";
            this.btnUploadJournal.UseVisualStyleBackColor = false;
            this.btnUploadJournal.Click += new System.EventHandler(this.btnUploadJournal_Click);
            // 
            // btnDownloadJournalTemplate
            // 
            resources.ApplyResources(this.btnDownloadJournalTemplate, "btnDownloadJournalTemplate");
            this.btnDownloadJournalTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnDownloadJournalTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadJournalTemplate.Name = "btnDownloadJournalTemplate";
            this.btnDownloadJournalTemplate.UseVisualStyleBackColor = false;
            this.btnDownloadJournalTemplate.Click += new System.EventHandler(this.btnDownloadJournalTemplate_Click);
            // 
            // gridJournal
            // 
            resources.ApplyResources(this.gridJournal, "gridJournal");
            this.gridJournal.AllowUserToAddRows = false;
            this.gridJournal.AllowUserToDeleteRows = false;
            this.gridJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridJournal.BackgroundColor = System.Drawing.Color.White;
            this.gridJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJournal.Name = "gridJournal";
            this.gridJournal.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tabImportHistory
            // 
            resources.ApplyResources(this.tabImportHistory, "tabImportHistory");
            this.tabImportHistory.Controls.Add(this.btnRollback);
            this.tabImportHistory.Controls.Add(this.btnRefreshHistory);
            this.tabImportHistory.Controls.Add(this.gridHistory);
            this.tabImportHistory.Controls.Add(this.label6);
            this.tabImportHistory.Name = "tabImportHistory";
            this.tabImportHistory.UseVisualStyleBackColor = true;
            // 
            // btnRollback
            // 
            resources.ApplyResources(this.btnRollback, "btnRollback");
            this.btnRollback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnRollback.ForeColor = System.Drawing.Color.White;
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.UseVisualStyleBackColor = false;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // btnRefreshHistory
            // 
            resources.ApplyResources(this.btnRefreshHistory, "btnRefreshHistory");
            this.btnRefreshHistory.Name = "btnRefreshHistory";
            this.btnRefreshHistory.UseVisualStyleBackColor = true;
            this.btnRefreshHistory.Click += new System.EventHandler(this.btnRefreshHistory_Click);
            // 
            // gridHistory
            // 
            resources.ApplyResources(this.gridHistory, "gridHistory");
            this.gridHistory.AllowUserToAddRows = false;
            this.gridHistory.AllowUserToDeleteRows = false;
            this.gridHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridHistory.BackgroundColor = System.Drawing.Color.White;
            this.gridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridHistory.MultiSelect = false;
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.ReadOnly = true;
            this.gridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkDryRun
            // 
            resources.ApplyResources(this.chkDryRun, "chkDryRun");
            this.chkDryRun.Name = "chkDryRun";
            this.chkDryRun.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frm_import_data
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkDryRun);
            this.Controls.Add(this.tabControl1);
            this.Name = "frm_import_data";
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
