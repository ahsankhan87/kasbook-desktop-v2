namespace pos.Products.Suppression
{
    partial class frm_stock_suppression_stock_records
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_stock_suppression_stock_records));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnPartNumber = new System.Windows.Forms.Button();
            this.btnWordSearch = new System.Windows.Forms.Button();
            this.gridStockRecords = new System.Windows.Forms.DataGridView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.colPartNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridStockRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnPartNumber
            // 
            resources.ApplyResources(this.btnPartNumber, "btnPartNumber");
            this.btnPartNumber.Name = "btnPartNumber";
            this.btnPartNumber.UseVisualStyleBackColor = true;
            this.btnPartNumber.Click += new System.EventHandler(this.btnPartNumber_Click);
            // 
            // btnWordSearch
            // 
            resources.ApplyResources(this.btnWordSearch, "btnWordSearch");
            this.btnWordSearch.Name = "btnWordSearch";
            this.btnWordSearch.UseVisualStyleBackColor = true;
            this.btnWordSearch.Click += new System.EventHandler(this.btnWordSearch_Click);
            // 
            // gridStockRecords
            // 
            resources.ApplyResources(this.gridStockRecords, "gridStockRecords");
            this.gridStockRecords.AllowUserToAddRows = false;
            this.gridStockRecords.AllowUserToDeleteRows = false;
            this.gridStockRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStockRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPartNumber,
            this.colDescription,
            this.colStock,
            this.colBin,
            this.colPrice});
            this.gridStockRecords.MultiSelect = false;
            this.gridStockRecords.Name = "gridStockRecords";
            this.gridStockRecords.ReadOnly = true;
            this.gridStockRecords.RowHeadersVisible = false;
            this.gridStockRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStockRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridStockRecords_CellDoubleClick);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // colPartNumber
            // 
            this.colPartNumber.DataPropertyName = "item_number";
            resources.ApplyResources(this.colPartNumber, "colPartNumber");
            this.colPartNumber.Name = "colPartNumber";
            this.colPartNumber.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.DataPropertyName = "name";
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colStock
            // 
            this.colStock.DataPropertyName = "qty";
            resources.ApplyResources(this.colStock, "colStock");
            this.colStock.Name = "colStock";
            this.colStock.ReadOnly = true;
            // 
            // colBin
            // 
            this.colBin.DataPropertyName = "category";
            resources.ApplyResources(this.colBin, "colBin");
            this.colBin.Name = "colBin";
            this.colBin.ReadOnly = true;
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "unit_price";
            resources.ApplyResources(this.colPrice, "colPrice");
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            // 
            // frm_stock_suppression_stock_records
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gridStockRecords);
            this.Controls.Add(this.btnWordSearch);
            this.Controls.Add(this.btnPartNumber);
            this.Controls.Add(this.txtSearch);
            this.Name = "frm_stock_suppression_stock_records";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_stock_suppression_stock_records_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridStockRecords)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnPartNumber;
        private System.Windows.Forms.Button btnWordSearch;
        private System.Windows.Forms.DataGridView gridStockRecords;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPartNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
    }
}
