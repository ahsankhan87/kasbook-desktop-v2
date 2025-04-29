namespace pos
{
    partial class frm_trialbalance_report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_trialbalance_report));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_trialbalance_report = new System.Windows.Forms.DataGridView();
            this.AccountName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClosingBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_trialbalance_report)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.CmbCondition);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.btn_close);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.txt_to_date);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txt_from_date);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
            // 
            // CmbCondition
            // 
            resources.ApplyResources(this.CmbCondition, "CmbCondition");
            this.CmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCondition.FormattingEnabled = true;
            this.CmbCondition.Name = "CmbCondition";
            this.CmbCondition.SelectedIndexChanged += new System.EventHandler(this.CmbCondition_SelectedIndexChanged);
            // 
            // btn_print
            // 
            resources.ApplyResources(this.btn_print, "btn_print");
            this.btn_print.Name = "btn_print";
            this.btn_print.UseVisualStyleBackColor = true;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txt_to_date
            // 
            resources.ApplyResources(this.txt_to_date, "txt_to_date");
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Name = "txt_to_date";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txt_from_date
            // 
            resources.ApplyResources(this.txt_from_date, "txt_from_date");
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_from_date.Name = "txt_from_date";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.grid_trialbalance_report);
            this.panel2.Name = "panel2";
            // 
            // grid_trialbalance_report
            // 
            resources.ApplyResources(this.grid_trialbalance_report, "grid_trialbalance_report");
            this.grid_trialbalance_report.AllowUserToAddRows = false;
            this.grid_trialbalance_report.AllowUserToDeleteRows = false;
            this.grid_trialbalance_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_trialbalance_report.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AccountName,
            this.TotalDebit,
            this.TotalCredit,
            this.ClosingBalance,
            this.AccountID});
            this.grid_trialbalance_report.Name = "grid_trialbalance_report";
            this.grid_trialbalance_report.ReadOnly = true;
            this.grid_trialbalance_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_trialbalance_report.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_trialbalance_report_CellDoubleClick);
            // 
            // AccountName
            // 
            this.AccountName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AccountName.DataPropertyName = "AccountName";
            resources.ApplyResources(this.AccountName, "AccountName");
            this.AccountName.Name = "AccountName";
            this.AccountName.ReadOnly = true;
            // 
            // TotalDebit
            // 
            this.TotalDebit.DataPropertyName = "TotalDebit";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.TotalDebit.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.TotalDebit, "TotalDebit");
            this.TotalDebit.Name = "TotalDebit";
            this.TotalDebit.ReadOnly = true;
            // 
            // TotalCredit
            // 
            this.TotalCredit.DataPropertyName = "TotalCredit";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.TotalCredit.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.TotalCredit, "TotalCredit");
            this.TotalCredit.Name = "TotalCredit";
            this.TotalCredit.ReadOnly = true;
            // 
            // ClosingBalance
            // 
            this.ClosingBalance.DataPropertyName = "ClosingBalance";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.ClosingBalance.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.ClosingBalance, "ClosingBalance");
            this.ClosingBalance.Name = "ClosingBalance";
            this.ClosingBalance.ReadOnly = true;
            // 
            // AccountID
            // 
            this.AccountID.DataPropertyName = "AccountID";
            resources.ApplyResources(this.AccountID, "AccountID");
            this.AccountID.Name = "AccountID";
            this.AccountID.ReadOnly = true;
            // 
            // frm_trialbalance_report
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_trialbalance_report";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_trialbalance_report_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_trialbalance_report_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_trialbalance_report)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_trialbalance_report;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.ComboBox CmbCondition;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClosingBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountID;
    }
}