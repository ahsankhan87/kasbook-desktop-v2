namespace pos
{
    partial class frm_product_adjustment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_product_adjustment));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_date = new System.Windows.Forms.DateTimePicker();
            this.txt_ref_no = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rb_by_name = new System.Windows.Forms.RadioButton();
            this.rb_by_code = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_ar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adjustment_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avg_cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_search_KeyDown);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // grid_search_products
            // 
            resources.ApplyResources(this.grid_search_products, "grid_search_products");
            this.grid_search_products.AllowUserToAddRows = false;
            this.grid_search_products.AllowUserToDeleteRows = false;
            this.grid_search_products.AllowUserToOrderColumns = true;
            this.grid_search_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_search_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.category,
            this.name,
            this.name_ar,
            this.location_code,
            this.qty,
            this.adjustment_qty,
            this.avg_cost,
            this.unit_price,
            this.btn_delete,
            this.description,
            this.item_type,
            this.item_number});
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid_search_products.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_search_products_CellContentClick);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.txt_date);
            this.panel1.Controls.Add(this.txt_ref_no);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.rb_by_name);
            this.panel1.Controls.Add(this.rb_by_code);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_close);
            this.panel1.Controls.Add(this.btn_update);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Name = "panel1";
            // 
            // txt_date
            // 
            resources.ApplyResources(this.txt_date, "txt_date");
            this.txt_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_date.Name = "txt_date";
            // 
            // txt_ref_no
            // 
            resources.ApplyResources(this.txt_ref_no, "txt_ref_no");
            this.txt_ref_no.Name = "txt_ref_no";
            this.txt_ref_no.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // rb_by_name
            // 
            resources.ApplyResources(this.rb_by_name, "rb_by_name");
            this.rb_by_name.Name = "rb_by_name";
            this.rb_by_name.UseVisualStyleBackColor = true;
            // 
            // rb_by_code
            // 
            resources.ApplyResources(this.rb_by_code, "rb_by_code");
            this.rb_by_code.Checked = true;
            this.rb_by_code.Name = "rb_by_code";
            this.rb_by_code.TabStop = true;
            this.rb_by_code.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_update
            // 
            resources.ApplyResources(this.btn_update, "btn_update");
            this.btn_update.Name = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.code.DataPropertyName = "code";
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // category
            // 
            this.category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.category, "category");
            this.category.Name = "category";
            this.category.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // name_ar
            // 
            this.name_ar.DataPropertyName = "name_ar";
            resources.ApplyResources(this.name_ar, "name_ar");
            this.name_ar.Name = "name_ar";
            this.name_ar.ReadOnly = true;
            // 
            // location_code
            // 
            this.location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.location_code, "location_code");
            this.location_code.Name = "location_code";
            this.location_code.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.qty.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // adjustment_qty
            // 
            this.adjustment_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = "0";
            this.adjustment_qty.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.adjustment_qty, "adjustment_qty");
            this.adjustment_qty.Name = "adjustment_qty";
            // 
            // avg_cost
            // 
            this.avg_cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.avg_cost.DataPropertyName = "avg_cost";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.avg_cost.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.avg_cost, "avg_cost");
            this.avg_cost.Name = "avg_cost";
            this.avg_cost.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unit_price.DataPropertyName = "unit_price";
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // btn_delete
            // 
            this.btn_delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // item_type
            // 
            this.item_type.DataPropertyName = "item_type";
            resources.ApplyResources(this.item_type, "item_type");
            this.item_type.Name = "item_type";
            this.item_type.ReadOnly = true;
            // 
            // item_number
            // 
            this.item_number.DataPropertyName = "item_number";
            resources.ApplyResources(this.item_number, "item_number");
            this.item_number.Name = "item_number";
            // 
            // frm_product_adjustment
            // 
            this.AcceptButton = this.btn_search;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.grid_search_products);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_product_adjustment";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_product_adjustment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_product_adjustment_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.DataGridView grid_search_products;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.RadioButton rb_by_name;
        private System.Windows.Forms.RadioButton rb_by_code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txt_date;
        private System.Windows.Forms.TextBox txt_ref_no;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_ar;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn adjustment_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn avg_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewButtonColumn btn_delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_number;
    }
}