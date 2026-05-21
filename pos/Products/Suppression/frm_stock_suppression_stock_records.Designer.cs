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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnPartNumber = new System.Windows.Forms.Button();
            this.btnWordSearch = new System.Windows.Forms.Button();
            this.gridStockRecords = new System.Windows.Forms.DataGridView();
            this.colPartNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridStockRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(13, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(399, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnPartNumber
            // 
            this.btnPartNumber.Location = new System.Drawing.Point(418, 13);
            this.btnPartNumber.Name = "btnPartNumber";
            this.btnPartNumber.Size = new System.Drawing.Size(96, 24);
            this.btnPartNumber.TabIndex = 1;
            this.btnPartNumber.Text = "Part number";
            this.btnPartNumber.UseVisualStyleBackColor = true;
            this.btnPartNumber.Click += new System.EventHandler(this.btnPartNumber_Click);
            // 
            // btnWordSearch
            // 
            this.btnWordSearch.Location = new System.Drawing.Point(520, 13);
            this.btnWordSearch.Name = "btnWordSearch";
            this.btnWordSearch.Size = new System.Drawing.Size(96, 24);
            this.btnWordSearch.TabIndex = 2;
            this.btnWordSearch.Text = "Word Search";
            this.btnWordSearch.UseVisualStyleBackColor = true;
            this.btnWordSearch.Click += new System.EventHandler(this.btnWordSearch_Click);
            // 
            // gridStockRecords
            // 
            this.gridStockRecords.AllowUserToAddRows = false;
            this.gridStockRecords.AllowUserToDeleteRows = false;
            this.gridStockRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridStockRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStockRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPartNumber,
            this.colDescription,
            this.colStock,
            this.colBin,
            this.colPrice});
            this.gridStockRecords.Location = new System.Drawing.Point(13, 46);
            this.gridStockRecords.MultiSelect = false;
            this.gridStockRecords.Name = "gridStockRecords";
            this.gridStockRecords.ReadOnly = true;
            this.gridStockRecords.RowHeadersVisible = false;
            this.gridStockRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStockRecords.Size = new System.Drawing.Size(785, 396);
            this.gridStockRecords.TabIndex = 3;
            this.gridStockRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridStockRecords_CellDoubleClick);
            // 
            // colPartNumber
            // 
            this.colPartNumber.DataPropertyName = "item_number";
            this.colPartNumber.HeaderText = "Part number";
            this.colPartNumber.Name = "colPartNumber";
            this.colPartNumber.ReadOnly = true;
            this.colPartNumber.Width = 140;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.DataPropertyName = "name";
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colStock
            // 
            this.colStock.DataPropertyName = "qty";
            this.colStock.HeaderText = "Stock";
            this.colStock.Name = "colStock";
            this.colStock.ReadOnly = true;
            this.colStock.Width = 70;
            // 
            // colBin
            // 
            this.colBin.DataPropertyName = "category";
            this.colBin.HeaderText = "Bin";
            this.colBin.Name = "colBin";
            this.colBin.ReadOnly = true;
            this.colBin.Width = 140;
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "unit_price";
            this.colPrice.HeaderText = "Price";
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            this.colPrice.Width = 80;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(642, 448);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(723, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 456);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Searching";
            // 
            // frm_stock_suppression_stock_records
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(810, 488);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gridStockRecords);
            this.Controls.Add(this.btnWordSearch);
            this.Controls.Add(this.btnPartNumber);
            this.Controls.Add(this.txtSearch);
            this.Name = "frm_stock_suppression_stock_records";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stock Records";
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
