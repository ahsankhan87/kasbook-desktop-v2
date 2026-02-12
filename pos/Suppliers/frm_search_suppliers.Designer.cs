namespace pos
{
    partial class frm_search_suppliers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_search_suppliers));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_search_suppliers = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.first_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_suppliers)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn_cancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_search_KeyUp);
            // 
            // btn_ok
            // 
            this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_ok, "btn_ok");
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_search_suppliers);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // grid_search_suppliers
            // 
            this.grid_search_suppliers.AllowUserToAddRows = false;
            this.grid_search_suppliers.AllowUserToDeleteRows = false;
            this.grid_search_suppliers.AllowUserToOrderColumns = true;
            this.grid_search_suppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_search_suppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_search_suppliers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.supplier_code,
            this.first_name,
            this.last_name,
            this.email,
            this.vat_no,
            this.contact_no,
            this.address});
            resources.ApplyResources(this.grid_search_suppliers, "grid_search_suppliers");
            this.grid_search_suppliers.Name = "grid_search_suppliers";
            this.grid_search_suppliers.ReadOnly = true;
            this.grid_search_suppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_search_suppliers.DoubleClick += new System.EventHandler(this.grid_search_suppliers_DoubleClick);
            this.grid_search_suppliers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_search_suppliers_KeyDown);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // supplier_code
            // 
            this.supplier_code.DataPropertyName = "supplier_code";
            resources.ApplyResources(this.supplier_code, "supplier_code");
            this.supplier_code.Name = "supplier_code";
            this.supplier_code.ReadOnly = true;
            // 
            // first_name
            // 
            this.first_name.DataPropertyName = "first_name";
            resources.ApplyResources(this.first_name, "first_name");
            this.first_name.Name = "first_name";
            this.first_name.ReadOnly = true;
            // 
            // last_name
            // 
            this.last_name.DataPropertyName = "last_name";
            resources.ApplyResources(this.last_name, "last_name");
            this.last_name.Name = "last_name";
            this.last_name.ReadOnly = true;
            // 
            // email
            // 
            this.email.DataPropertyName = "email";
            resources.ApplyResources(this.email, "email");
            this.email.Name = "email";
            this.email.ReadOnly = true;
            // 
            // vat_no
            // 
            this.vat_no.DataPropertyName = "vat_no";
            resources.ApplyResources(this.vat_no, "vat_no");
            this.vat_no.Name = "vat_no";
            this.vat_no.ReadOnly = true;
            // 
            // contact_no
            // 
            this.contact_no.DataPropertyName = "contact_no";
            resources.ApplyResources(this.contact_no, "contact_no");
            this.contact_no.Name = "contact_no";
            this.contact_no.ReadOnly = true;
            // 
            // address
            // 
            this.address.DataPropertyName = "address";
            resources.ApplyResources(this.address, "address");
            this.address.Name = "address";
            this.address.ReadOnly = true;
            // 
            // frm_search_suppliers
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_search_suppliers";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_search_suppliers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_search_suppliers_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_suppliers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_search_suppliers;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn first_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
    }
}