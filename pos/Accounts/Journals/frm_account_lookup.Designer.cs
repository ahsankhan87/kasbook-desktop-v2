namespace pos
{
    partial class frm_account_lookup
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView gridAccounts;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_account_lookup));
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.gridAccounts = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            resources.ApplyResources(this.panelTop, "panelTop");
            this.panelTop.Controls.Add(this.btnCancel);
            this.panelTop.Controls.Add(this.btnSelect);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.lblSearch);
            this.panelTop.Name = "panelTop";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // lblSearch
            // 
            resources.ApplyResources(this.lblSearch, "lblSearch");
            this.lblSearch.Name = "lblSearch";
            // 
            // gridAccounts
            // 
            resources.ApplyResources(this.gridAccounts, "gridAccounts");
            this.gridAccounts.AllowUserToAddRows = false;
            this.gridAccounts.AllowUserToDeleteRows = false;
            this.gridAccounts.AllowUserToResizeRows = false;
            this.gridAccounts.BackgroundColor = System.Drawing.Color.White;
            this.gridAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colName,
            this.colName2});
            this.gridAccounts.MultiSelect = false;
            this.gridAccounts.Name = "gridAccounts";
            this.gridAccounts.ReadOnly = true;
            this.gridAccounts.RowHeadersVisible = false;
            this.gridAccounts.RowTemplate.Height = 28;
            this.gridAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAccounts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridAccounts_CellDoubleClick);
            this.gridAccounts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridAccounts_KeyDown);
            // 
            // colCode
            // 
            this.colCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCode.DataPropertyName = "code";
            resources.ApplyResources(this.colCode, "colCode");
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "name";
            resources.ApplyResources(this.colName, "colName");
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colName2
            // 
            this.colName2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName2.DataPropertyName = "name_2";
            resources.ApplyResources(this.colName2, "colName2");
            this.colName2.Name = "colName2";
            this.colName2.ReadOnly = true;
            // 
            // frm_account_lookup
            // 
            this.AcceptButton = this.btnSelect;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.gridAccounts);
            this.Controls.Add(this.panelTop);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_account_lookup";
            this.Load += new System.EventHandler(this.frm_account_lookup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_account_lookup_KeyDown);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccounts)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName2;
    }
}
