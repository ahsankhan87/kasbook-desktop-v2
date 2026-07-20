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
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
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
            this.panelTop.Controls.Add(this.btnCancel);
            this.panelTop.Controls.Add(this.btnSelect);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.lblSearch);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12);
            this.panelTop.Size = new System.Drawing.Size(884, 58);
            this.panelTop.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(797, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(716, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 28);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(68, 17);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(642, 22);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(15, 20);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(47, 16);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search";
            // 
            // gridAccounts
            // 
            this.gridAccounts.AllowUserToAddRows = false;
            this.gridAccounts.AllowUserToDeleteRows = false;
            this.gridAccounts.AllowUserToResizeRows = false;
            this.gridAccounts.BackgroundColor = System.Drawing.Color.White;
            this.gridAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colName,
            this.colName2});
            this.gridAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAccounts.Location = new System.Drawing.Point(0, 58);
            this.gridAccounts.MultiSelect = false;
            this.gridAccounts.Name = "gridAccounts";
            this.gridAccounts.ReadOnly = true;
            this.gridAccounts.RowHeadersVisible = false;
            this.gridAccounts.RowTemplate.Height = 28;
            this.gridAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAccounts.Size = new System.Drawing.Size(884, 403);
            this.gridAccounts.TabIndex = 1;
            this.gridAccounts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridAccounts_CellDoubleClick);
            this.gridAccounts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridAccounts_KeyDown);
            // 
            // colCode
            // 
            this.colCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCode.DataPropertyName = "code";
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 140;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "name";
            this.colName.HeaderText = "Account Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colName2
            // 
            this.colName2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName2.DataPropertyName = "name_2";
            this.colName2.HeaderText = "Arabic Name";
            this.colName2.Name = "colName2";
            this.colName2.ReadOnly = true;
            // 
            // frm_account_lookup
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.gridAccounts);
            this.Controls.Add(this.panelTop);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_account_lookup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Account";
            this.Load += new System.EventHandler(this.frm_account_lookup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_account_lookup_KeyDown);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccounts)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
