
namespace pos.Products.ICT
{
    partial class frm_ict
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_ict = new System.Windows.Forms.DataGridView();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.source_branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiving_branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source_branch_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destination_branch_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty_requested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty_released = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid_ict)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_ict
            // 
            this.grid_ict.AllowUserToAddRows = false;
            this.grid_ict.AllowUserToDeleteRows = false;
            this.grid_ict.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_ict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_ict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.source_branch,
            this.receiving_branch,
            this.source_branch_id,
            this.destination_branch_id,
            this.item_code,
            this.product_name,
            this.qty_requested,
            this.qty_released,
            this.qty,
            this.chk});
            this.grid_ict.Location = new System.Drawing.Point(0, 60);
            this.grid_ict.Name = "grid_ict";
            this.grid_ict.RowHeadersWidth = 62;
            this.grid_ict.Size = new System.Drawing.Size(765, 385);
            this.grid_ict.TabIndex = 0;
            // 
            // btn_refresh
            // 
            this.btn_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_refresh.Location = new System.Drawing.Point(668, 5);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(87, 24);
            this.btn_refresh.TabIndex = 2;
            this.btn_refresh.Text = "Refresh (F5)";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.Location = new System.Drawing.Point(668, 30);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(87, 24);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // source_branch
            // 
            this.source_branch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.source_branch.DataPropertyName = "source_branch";
            this.source_branch.HeaderText = "Requesting Branch";
            this.source_branch.MinimumWidth = 8;
            this.source_branch.Name = "source_branch";
            this.source_branch.ReadOnly = true;
            this.source_branch.Width = 113;
            // 
            // receiving_branch
            // 
            this.receiving_branch.DataPropertyName = "receiving_branch";
            this.receiving_branch.HeaderText = "Receiving Branch";
            this.receiving_branch.MinimumWidth = 8;
            this.receiving_branch.Name = "receiving_branch";
            this.receiving_branch.Width = 150;
            // 
            // source_branch_id
            // 
            this.source_branch_id.DataPropertyName = "source_branch_id";
            this.source_branch_id.HeaderText = "source_branch_id";
            this.source_branch_id.MinimumWidth = 8;
            this.source_branch_id.Name = "source_branch_id";
            this.source_branch_id.Visible = false;
            this.source_branch_id.Width = 150;
            // 
            // destination_branch_id
            // 
            this.destination_branch_id.DataPropertyName = "destination_branch_id";
            this.destination_branch_id.HeaderText = "destination_branch_id";
            this.destination_branch_id.MinimumWidth = 8;
            this.destination_branch_id.Name = "destination_branch_id";
            this.destination_branch_id.Visible = false;
            this.destination_branch_id.Width = 150;
            // 
            // item_code
            // 
            this.item_code.DataPropertyName = "item_code";
            this.item_code.HeaderText = "Code";
            this.item_code.MinimumWidth = 8;
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            this.item_code.Width = 150;
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.product_name.DataPropertyName = "product_name";
            this.product_name.HeaderText = "Product";
            this.product_name.MinimumWidth = 8;
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            this.product_name.Width = 69;
            // 
            // qty_requested
            // 
            this.qty_requested.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty_requested.DataPropertyName = "requested_qty";
            this.qty_requested.HeaderText = "Qty Requested";
            this.qty_requested.MinimumWidth = 8;
            this.qty_requested.Name = "qty_requested";
            this.qty_requested.ReadOnly = true;
            this.qty_requested.Width = 95;
            // 
            // qty_released
            // 
            this.qty_released.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty_released.DataPropertyName = "released_qty";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.qty_released.DefaultCellStyle = dataGridViewCellStyle2;
            this.qty_released.HeaderText = "Qty Released";
            this.qty_released.MinimumWidth = 8;
            this.qty_released.Name = "qty_released";
            this.qty_released.Width = 88;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "quantity_transferred";
            this.qty.HeaderText = "Qty Transferred";
            this.qty.MinimumWidth = 8;
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 96;
            // 
            // chk
            // 
            this.chk.DataPropertyName = "transfer_status";
            this.chk.HeaderText = "Transfer Status";
            this.chk.MinimumWidth = 8;
            this.chk.Name = "chk";
            this.chk.ReadOnly = true;
            this.chk.Width = 150;
            // 
            // frm_ict
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 450);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.grid_ict);
            this.Name = "frm_ict";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inter Company Stock Transfer";
            this.Load += new System.EventHandler(this.frm_ict_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_ict_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_ict)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_ict;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiving_branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_branch_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn destination_branch_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty_requested;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty_released;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
    }
}