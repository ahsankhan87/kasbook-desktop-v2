namespace pos
{
    partial class frm_productsMovements
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_productsMovements));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_productName = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trans_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid_search_products
            // 
            this.grid_search_products.AllowUserToAddRows = false;
            this.grid_search_products.AllowUserToDeleteRows = false;
            this.grid_search_products.AllowUserToOrderColumns = true;
            this.grid_search_products.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid_search_products.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            resources.ApplyResources(this.grid_search_products, "grid_search_products");
            this.grid_search_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid_search_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.qty,
            this.balanceQty,
            this.cost_price,
            this.unit_price,
            this.loc,
            this.description,
            this.supplier,
            this.customer,
            this.trans_date,
            this.username});
            this.grid_search_products.EnableHeadersVisualStyles = false;
            this.grid_search_products.MultiSelect = false;
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.ReadOnly = true;
            this.grid_search_products.RowHeadersVisible = false;
            //this.grid_search_products.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.grid_search_products.RowTemplate.DefaultCellStyle.Format = "N2";
            this.grid_search_products.RowTemplate.Height = 30;
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_productName);
            this.panel1.Controls.Add(this.btn_close);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // lbl_productName
            // 
            resources.ApplyResources(this.lbl_productName, "lbl_productName");
            this.lbl_productName.Name = "lbl_productName";
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.invoice_no.ContextMenuStrip = this.contextMenuStrip1;
            this.invoice_no.DataPropertyName = "invoice_no";
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.qty.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // balanceQty
            // 
            this.balanceQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.balanceQty.DataPropertyName = "balanceQty";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.balanceQty.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.balanceQty, "balanceQty");
            this.balanceQty.Name = "balanceQty";
            this.balanceQty.ReadOnly = true;
            // 
            // cost_price
            // 
            this.cost_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cost_price.DataPropertyName = "cost_price";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.cost_price.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cost_price, "cost_price");
            this.cost_price.Name = "cost_price";
            this.cost_price.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.unit_price.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // loc
            // 
            this.loc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.loc.DataPropertyName = "loc";
            resources.ApplyResources(this.loc, "loc");
            this.loc.Name = "loc";
            this.loc.ReadOnly = true;
            // 
            // description
            // 
            this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.description.DataPropertyName = "description";
            this.description.FillWeight = 20F;
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // supplier
            // 
            this.supplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier.DataPropertyName = "supplier";
            this.supplier.FillWeight = 30F;
            resources.ApplyResources(this.supplier, "supplier");
            this.supplier.Name = "supplier";
            this.supplier.ReadOnly = true;
            // 
            // customer
            // 
            this.customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.customer.DataPropertyName = "customer";
            this.customer.FillWeight = 30F;
            resources.ApplyResources(this.customer, "customer");
            this.customer.Name = "customer";
            this.customer.ReadOnly = true;
            // 
            // trans_date
            // 
            this.trans_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.trans_date.DataPropertyName = "trans_date";
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.trans_date.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.trans_date, "trans_date");
            this.trans_date.Name = "trans_date";
            this.trans_date.ReadOnly = true;
            // 
            // username
            // 
            this.username.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.username.DataPropertyName = "username";
            this.username.FillWeight = 20F;
            resources.ApplyResources(this.username, "username");
            this.username.Name = "username";
            this.username.ReadOnly = true;
            // 
            // frm_productsMovements
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.grid_search_products);
            this.Controls.Add(this.panel1);
            this.Name = "frm_productsMovements";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_productsMovements_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView grid_search_products;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Label lbl_productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn loc;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn trans_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn username;
    }
}
