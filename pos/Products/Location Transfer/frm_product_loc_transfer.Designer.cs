namespace pos
{
    partial class frm_product_loc_transfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_product_loc_transfer));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_ar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transfer_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmb_from_locations = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_to_locations = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_date = new System.Windows.Forms.DateTimePicker();
            this.txt_ref_no = new System.Windows.Forms.TextBox();
            this.rb_by_name = new System.Windows.Forms.RadioButton();
            this.rb_by_code = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
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
            this.name,
            this.name_ar,
            this.qty,
            this.transfer_qty});
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
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
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.qty.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            // 
            // transfer_qty
            // 
            this.transfer_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.transfer_qty.DataPropertyName = "transfer_qty";
            resources.ApplyResources(this.transfer_qty, "transfer_qty");
            this.transfer_qty.Name = "transfer_qty";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.cmb_from_locations);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmb_to_locations);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.txt_date);
            this.panel1.Controls.Add(this.txt_ref_no);
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
            // cmb_from_locations
            // 
            resources.ApplyResources(this.cmb_from_locations, "cmb_from_locations");
            this.cmb_from_locations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_from_locations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_from_locations.FormattingEnabled = true;
            this.cmb_from_locations.Name = "cmb_from_locations";
            this.cmb_from_locations.SelectedIndexChanged += new System.EventHandler(this.cmb_from_locations_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmb_to_locations
            // 
            resources.ApplyResources(this.cmb_to_locations, "cmb_to_locations");
            this.cmb_to_locations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_to_locations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_to_locations.FormattingEnabled = true;
            this.cmb_to_locations.Name = "cmb_to_locations";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
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
            // frm_product_loc_transfer
            // 
            this.AcceptButton = this.btn_search;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.grid_search_products);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_product_loc_transfer";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_product_loc_transfer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_product_loc_transfer_KeyDown);
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
        private System.Windows.Forms.ComboBox cmb_from_locations;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_to_locations;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_ar;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn transfer_qty;
    }
}