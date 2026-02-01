namespace pos
{
    partial class frm_purchase_return
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_purchase_return));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid_purchase_return = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnableQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loc_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packet_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_close = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbReturnReason = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_return = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_taxes_title = new System.Windows.Forms.Label();
            this.txt_invoice_no = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_purchase_return)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.grid_purchase_return);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // grid_purchase_return
            // 
            this.grid_purchase_return.AllowUserToAddRows = false;
            this.grid_purchase_return.AllowUserToDeleteRows = false;
            this.grid_purchase_return.AllowUserToOrderColumns = true;
            this.grid_purchase_return.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_purchase_return.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_purchase_return.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.chk,
            this.invoice_no,
            this.item_code,
            this.product_name,
            this.quantity,
            this.ReturnedQty,
            this.ReturnableQty,
            this.ReturnQty,
            this.unit_price,
            this.cost_price,
            this.discount_value,
            this.vat,
            this.total,
            this.loc_code,
            this.tax_id,
            this.tax_rate,
            this.item_id,
            this.packet_qty,
            this.item_number});
            resources.ApplyResources(this.grid_purchase_return, "grid_purchase_return");
            this.grid_purchase_return.Name = "grid_purchase_return";
            this.grid_purchase_return.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid_purchase_return.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grid_purchase_return_CellValidating);
            this.grid_purchase_return.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_purchase_return_CellValueChanged);
            this.grid_purchase_return.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_purchase_return_CurrentCellDirtyStateChanged);
            this.grid_purchase_return.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grid_purchase_return_DataError);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // chk
            // 
            this.chk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.chk, "chk");
            this.chk.Name = "chk";
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            this.invoice_no.FillWeight = 112.5211F;
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // item_code
            // 
            this.item_code.DataPropertyName = "item_code";
            resources.ApplyResources(this.item_code, "item_code");
            this.item_code.Name = "item_code";
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product_name.DataPropertyName = "product_name";
            this.product_name.FillWeight = 112.5211F;
            resources.ApplyResources(this.product_name, "product_name");
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            // 
            // quantity
            // 
            this.quantity.DataPropertyName = "quantity";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = null;
            this.quantity.DefaultCellStyle = dataGridViewCellStyle10;
            this.quantity.FillWeight = 112.5211F;
            resources.ApplyResources(this.quantity, "quantity");
            this.quantity.Name = "quantity";
            this.quantity.ReadOnly = true;
            // 
            // ReturnedQty
            // 
            this.ReturnedQty.DataPropertyName = "ReturnedQty";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            this.ReturnedQty.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.ReturnedQty, "ReturnedQty");
            this.ReturnedQty.Name = "ReturnedQty";
            // 
            // ReturnableQty
            // 
            this.ReturnableQty.DataPropertyName = "ReturnableQty";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            dataGridViewCellStyle12.NullValue = null;
            this.ReturnableQty.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.ReturnableQty, "ReturnableQty");
            this.ReturnableQty.Name = "ReturnableQty";
            // 
            // ReturnQty
            // 
            this.ReturnQty.DataPropertyName = "ReturnQty";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle13.Format = "N2";
            dataGridViewCellStyle13.NullValue = null;
            this.ReturnQty.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.ReturnQty, "ReturnQty");
            this.ReturnQty.Name = "ReturnQty";
            // 
            // unit_price
            // 
            this.unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N2";
            dataGridViewCellStyle14.NullValue = null;
            this.unit_price.DefaultCellStyle = dataGridViewCellStyle14;
            this.unit_price.FillWeight = 112.5211F;
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // cost_price
            // 
            this.cost_price.DataPropertyName = "cost_price";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N2";
            dataGridViewCellStyle15.NullValue = null;
            this.cost_price.DefaultCellStyle = dataGridViewCellStyle15;
            resources.ApplyResources(this.cost_price, "cost_price");
            this.cost_price.Name = "cost_price";
            this.cost_price.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.NullValue = null;
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle16;
            this.discount_value.FillWeight = 112.5211F;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // vat
            // 
            this.vat.DataPropertyName = "vat";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "N2";
            dataGridViewCellStyle17.NullValue = null;
            this.vat.DefaultCellStyle = dataGridViewCellStyle17;
            resources.ApplyResources(this.vat, "vat");
            this.vat.Name = "vat";
            this.vat.ReadOnly = true;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N2";
            dataGridViewCellStyle18.NullValue = null;
            this.total.DefaultCellStyle = dataGridViewCellStyle18;
            this.total.FillWeight = 112.5211F;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // loc_code
            // 
            this.loc_code.DataPropertyName = "loc_code";
            resources.ApplyResources(this.loc_code, "loc_code");
            this.loc_code.Name = "loc_code";
            // 
            // tax_id
            // 
            this.tax_id.DataPropertyName = "tax_id";
            resources.ApplyResources(this.tax_id, "tax_id");
            this.tax_id.Name = "tax_id";
            // 
            // tax_rate
            // 
            this.tax_rate.DataPropertyName = "tax_rate";
            resources.ApplyResources(this.tax_rate, "tax_rate");
            this.tax_rate.Name = "tax_rate";
            // 
            // item_id
            // 
            this.item_id.DataPropertyName = "item_id";
            resources.ApplyResources(this.item_id, "item_id");
            this.item_id.Name = "item_id";
            // 
            // packet_qty
            // 
            this.packet_qty.DataPropertyName = "packet_qty";
            resources.ApplyResources(this.packet_qty, "packet_qty");
            this.packet_qty.Name = "packet_qty";
            // 
            // item_number
            // 
            this.item_number.DataPropertyName = "item_number";
            resources.ApplyResources(this.item_number, "item_number");
            this.item_number.Name = "item_number";
            // 
            // txt_close
            // 
            resources.ApplyResources(this.txt_close, "txt_close");
            this.txt_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.txt_close.Name = "txt_close";
            this.txt_close.UseVisualStyleBackColor = true;
            this.txt_close.Click += new System.EventHandler(this.txt_close_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel2.Controls.Add(this.cmbReturnReason);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btn_return);
            this.panel2.Controls.Add(this.btn_search);
            this.panel2.Controls.Add(this.txt_close);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbl_taxes_title);
            this.panel2.Controls.Add(this.txt_invoice_no);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // cmbReturnReason
            // 
            this.cmbReturnReason.FormattingEnabled = true;
            resources.ApplyResources(this.cmbReturnReason, "cmbReturnReason");
            this.cmbReturnReason.Name = "cmbReturnReason";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // btn_return
            // 
            resources.ApplyResources(this.btn_return, "btn_return");
            this.btn_return.Name = "btn_return";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_return_Click);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // lbl_taxes_title
            // 
            resources.ApplyResources(this.lbl_taxes_title, "lbl_taxes_title");
            this.lbl_taxes_title.ForeColor = System.Drawing.Color.White;
            this.lbl_taxes_title.Name = "lbl_taxes_title";
            // 
            // txt_invoice_no
            // 
            resources.ApplyResources(this.txt_invoice_no, "txt_invoice_no");
            this.txt_invoice_no.Name = "txt_invoice_no";
            // 
            // frm_purchase_return
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.txt_close;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "frm_purchase_return";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_purchase_return_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_purchase_return_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_purchase_return)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_purchase_return;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button txt_close;
        private System.Windows.Forms.TextBox txt_invoice_no;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.ComboBox cmbReturnReason;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_taxes_title;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnableQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn loc_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn packet_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_number;
    }
}

