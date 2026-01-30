
namespace pos.Products.ICT
{
    partial class frm_release_ict
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_release_ict));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_ict = new System.Windows.Forms.DataGridView();
            this.source_branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiving_branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source_branch_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.destination_branch_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty_requested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty_released = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_release_qty = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid_ict)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_ict
            // 
            this.grid_ict.AllowUserToAddRows = false;
            this.grid_ict.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grid_ict, "grid_ict");
            this.grid_ict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_ict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.source_branch,
            this.id,
            this.receiving_branch,
            this.source_branch_id,
            this.destination_branch_id,
            this.item_code,
            this.product_name,
            this.qty_requested,
            this.qty_released,
            this.qty,
            this.chk,
            this.item_number});
            this.grid_ict.Name = "grid_ict";
            // 
            // source_branch
            // 
            this.source_branch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.source_branch.DataPropertyName = "source_branch";
            resources.ApplyResources(this.source_branch, "source_branch");
            this.source_branch.Name = "source_branch";
            this.source_branch.ReadOnly = true;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // receiving_branch
            // 
            this.receiving_branch.DataPropertyName = "receiving_branch";
            resources.ApplyResources(this.receiving_branch, "receiving_branch");
            this.receiving_branch.Name = "receiving_branch";
            // 
            // source_branch_id
            // 
            this.source_branch_id.DataPropertyName = "source_branch_id";
            resources.ApplyResources(this.source_branch_id, "source_branch_id");
            this.source_branch_id.Name = "source_branch_id";
            // 
            // destination_branch_id
            // 
            this.destination_branch_id.DataPropertyName = "destination_branch_id";
            resources.ApplyResources(this.destination_branch_id, "destination_branch_id");
            this.destination_branch_id.Name = "destination_branch_id";
            // 
            // item_code
            // 
            this.item_code.DataPropertyName = "item_code";
            resources.ApplyResources(this.item_code, "item_code");
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product_name.DataPropertyName = "product_name";
            resources.ApplyResources(this.product_name, "product_name");
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            // 
            // qty_requested
            // 
            this.qty_requested.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty_requested.DataPropertyName = "requested_qty";
            resources.ApplyResources(this.qty_requested, "qty_requested");
            this.qty_requested.Name = "qty_requested";
            this.qty_requested.ReadOnly = true;
            // 
            // qty_released
            // 
            this.qty_released.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty_released.DataPropertyName = "released_qty";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.qty_released.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.qty_released, "qty_released");
            this.qty_released.Name = "qty_released";
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "quantity_transferred";
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // chk
            // 
            resources.ApplyResources(this.chk, "chk");
            this.chk.Name = "chk";
            // 
            // item_number
            // 
            this.item_number.DataPropertyName = "item_number";
            resources.ApplyResources(this.item_number, "item_number");
            this.item_number.Name = "item_number";
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_release_qty
            // 
            resources.ApplyResources(this.btn_release_qty, "btn_release_qty");
            this.btn_release_qty.Name = "btn_release_qty";
            this.btn_release_qty.UseVisualStyleBackColor = true;
            this.btn_release_qty.Click += new System.EventHandler(this.btn_release_qty_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // frm_release_ict
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_release_qty);
            this.Controls.Add(this.grid_ict);
            this.Name = "frm_release_ict";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_release_ict_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_release_ict_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_ict)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_ict;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_release_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiving_branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn source_branch_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn destination_branch_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty_requested;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty_released;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_number;
        private System.Windows.Forms.Label label1;
    }
}