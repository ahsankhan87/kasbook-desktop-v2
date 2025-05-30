﻿namespace pos
{
    partial class frm_suppliers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_suppliers));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.grid_suppliers = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.first_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_detail = new System.Windows.Forms.DataGridViewButtonColumn();
            this.vat_status = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btn_new = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_suppliers_title = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_suppliers)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.btn_delete);
            this.panel1.Controls.Add(this.btn_update);
            this.panel1.Controls.Add(this.grid_suppliers);
            this.panel1.Controls.Add(this.btn_new);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_search_KeyPress);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_delete
            // 
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_update
            // 
            resources.ApplyResources(this.btn_update, "btn_update");
            this.btn_update.Name = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // grid_suppliers
            // 
            this.grid_suppliers.AllowUserToAddRows = false;
            this.grid_suppliers.AllowUserToDeleteRows = false;
            this.grid_suppliers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_suppliers, "grid_suppliers");
            this.grid_suppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_suppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_suppliers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.first_name,
            this.last_name,
            this.email,
            this.vat_no,
            this.contact_no,
            this.address,
            this.date_created,
            this.btn_detail,
            this.vat_status});
            this.grid_suppliers.Name = "grid_suppliers";
            this.grid_suppliers.ReadOnly = true;
            this.grid_suppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_suppliers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_suppliers_CellContentClick);
            this.grid_suppliers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_suppliers_CellDoubleClick);
            this.grid_suppliers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_suppliers_KeyDown);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
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
            // date_created
            // 
            this.date_created.DataPropertyName = "date_created";
            resources.ApplyResources(this.date_created, "date_created");
            this.date_created.Name = "date_created";
            this.date_created.ReadOnly = true;
            // 
            // btn_detail
            // 
            this.btn_detail.DataPropertyName = "btn_detail";
            this.btn_detail.FillWeight = 50F;
            resources.ApplyResources(this.btn_detail, "btn_detail");
            this.btn_detail.Name = "btn_detail";
            this.btn_detail.ReadOnly = true;
            this.btn_detail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btn_detail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // vat_status
            // 
            this.vat_status.DataPropertyName = "vat_status";
            resources.ApplyResources(this.vat_status, "vat_status");
            this.vat_status.Name = "vat_status";
            this.vat_status.ReadOnly = true;
            // 
            // btn_new
            // 
            this.btn_new.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel2.Controls.Add(this.lbl_suppliers_title);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lbl_suppliers_title
            // 
            resources.ApplyResources(this.lbl_suppliers_title, "lbl_suppliers_title");
            this.lbl_suppliers_title.ForeColor = System.Drawing.Color.White;
            this.lbl_suppliers_title.Name = "lbl_suppliers_title";
            // 
            // frm_suppliers
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "frm_suppliers";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_suppliers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_suppliers_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_suppliers)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_suppliers;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_suppliers_title;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn first_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_created;
        private System.Windows.Forms.DataGridViewButtonColumn btn_detail;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vat_status;
    }
}

