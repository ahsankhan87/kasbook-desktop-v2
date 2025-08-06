namespace pos
{
    partial class frm_updateCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_updateCompany));
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_address = new System.Windows.Forms.TextBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_vat_no = new System.Windows.Forms.TextBox();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.txt_contact_no = new System.Windows.Forms.TextBox();
            this.txt_currency_id = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_image = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmb_payable_acc_id = new System.Windows.Forms.ComboBox();
            this.cmb_cash_acc_id = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmb_receivable_acc_id = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmb_tax_acc_id = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmb_commission_acc_id = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cmb_item_variance_acc_id = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.cmb_purchases_disc_acc_id = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cmb_sales_disc_acc_id = new System.Windows.Forms.ComboBox();
            this.cmb_purchases_return_acc_id = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cmb_sales_return_acc_id = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmb_purchases_acc_id = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmb_inventory_acc_id = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmb_sales_acc_id = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chk_use_zatca_e_invoice = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // lbl_header_title
            // 
            resources.ApplyResources(this.lbl_header_title, "lbl_header_title");
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Name = "lbl_header_title";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lbl_header_title);
            this.panel1.Controls.Add(this.btn_cancel);
            this.panel1.Controls.Add(this.btn_save);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lblExpiryDate);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lblExpiryDate
            // 
            resources.ApplyResources(this.lblExpiryDate, "lblExpiryDate");
            this.lblExpiryDate.ForeColor = System.Drawing.Color.Brown;
            this.lblExpiryDate.Name = "lblExpiryDate";
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chk_use_zatca_e_invoice);
            this.groupBox2.Controls.Add(this.txt_address);
            this.groupBox2.Controls.Add(this.txt_name);
            this.groupBox2.Controls.Add(this.txt_vat_no);
            this.groupBox2.Controls.Add(this.txt_email);
            this.groupBox2.Controls.Add(this.txt_contact_no);
            this.groupBox2.Controls.Add(this.txt_currency_id);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txt_image);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txt_id);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txt_address
            // 
            resources.ApplyResources(this.txt_address, "txt_address");
            this.txt_address.Name = "txt_address";
            // 
            // txt_name
            // 
            resources.ApplyResources(this.txt_name, "txt_name");
            this.txt_name.Name = "txt_name";
            // 
            // txt_vat_no
            // 
            resources.ApplyResources(this.txt_vat_no, "txt_vat_no");
            this.txt_vat_no.Name = "txt_vat_no";
            // 
            // txt_email
            // 
            resources.ApplyResources(this.txt_email, "txt_email");
            this.txt_email.Name = "txt_email";
            // 
            // txt_contact_no
            // 
            resources.ApplyResources(this.txt_contact_no, "txt_contact_no");
            this.txt_contact_no.Name = "txt_contact_no";
            // 
            // txt_currency_id
            // 
            resources.ApplyResources(this.txt_currency_id, "txt_currency_id");
            this.txt_currency_id.Name = "txt_currency_id";
            this.txt_currency_id.ReadOnly = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // txt_image
            // 
            resources.ApplyResources(this.txt_image, "txt_image");
            this.txt_image.Name = "txt_image";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txt_id
            // 
            resources.ApplyResources(this.txt_id, "txt_id");
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_payable_acc_id);
            this.groupBox1.Controls.Add(this.cmb_cash_acc_id);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cmb_receivable_acc_id);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmb_tax_acc_id);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmb_commission_acc_id);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.cmb_item_variance_acc_id);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.cmb_purchases_disc_acc_id);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.cmb_sales_disc_acc_id);
            this.groupBox1.Controls.Add(this.cmb_purchases_return_acc_id);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.cmb_sales_return_acc_id);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.cmb_purchases_acc_id);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cmb_inventory_acc_id);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cmb_sales_acc_id);
            this.groupBox1.Controls.Add(this.label13);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmb_payable_acc_id
            // 
            this.cmb_payable_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_payable_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_payable_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_payable_acc_id, "cmb_payable_acc_id");
            this.cmb_payable_acc_id.Name = "cmb_payable_acc_id";
            // 
            // cmb_cash_acc_id
            // 
            this.cmb_cash_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_cash_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_cash_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_cash_acc_id, "cmb_cash_acc_id");
            this.cmb_cash_acc_id.Name = "cmb_cash_acc_id";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmb_receivable_acc_id
            // 
            this.cmb_receivable_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_receivable_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_receivable_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_receivable_acc_id, "cmb_receivable_acc_id");
            this.cmb_receivable_acc_id.Name = "cmb_receivable_acc_id";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmb_tax_acc_id
            // 
            this.cmb_tax_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_tax_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_tax_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_tax_acc_id, "cmb_tax_acc_id");
            this.cmb_tax_acc_id.Name = "cmb_tax_acc_id";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmb_commission_acc_id
            // 
            this.cmb_commission_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_commission_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_commission_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_commission_acc_id, "cmb_commission_acc_id");
            this.cmb_commission_acc_id.Name = "cmb_commission_acc_id";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // cmb_item_variance_acc_id
            // 
            this.cmb_item_variance_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_item_variance_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_item_variance_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_item_variance_acc_id, "cmb_item_variance_acc_id");
            this.cmb_item_variance_acc_id.Name = "cmb_item_variance_acc_id";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // cmb_purchases_disc_acc_id
            // 
            this.cmb_purchases_disc_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_purchases_disc_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_purchases_disc_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_purchases_disc_acc_id, "cmb_purchases_disc_acc_id");
            this.cmb_purchases_disc_acc_id.Name = "cmb_purchases_disc_acc_id";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // cmb_sales_disc_acc_id
            // 
            this.cmb_sales_disc_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_sales_disc_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_sales_disc_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_sales_disc_acc_id, "cmb_sales_disc_acc_id");
            this.cmb_sales_disc_acc_id.Name = "cmb_sales_disc_acc_id";
            // 
            // cmb_purchases_return_acc_id
            // 
            this.cmb_purchases_return_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_purchases_return_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_purchases_return_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_purchases_return_acc_id, "cmb_purchases_return_acc_id");
            this.cmb_purchases_return_acc_id.Name = "cmb_purchases_return_acc_id";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // cmb_sales_return_acc_id
            // 
            this.cmb_sales_return_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_sales_return_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_sales_return_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_sales_return_acc_id, "cmb_sales_return_acc_id");
            this.cmb_sales_return_acc_id.Name = "cmb_sales_return_acc_id";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // cmb_purchases_acc_id
            // 
            this.cmb_purchases_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_purchases_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_purchases_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_purchases_acc_id, "cmb_purchases_acc_id");
            this.cmb_purchases_acc_id.Name = "cmb_purchases_acc_id";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // cmb_inventory_acc_id
            // 
            this.cmb_inventory_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_inventory_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_inventory_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_inventory_acc_id, "cmb_inventory_acc_id");
            this.cmb_inventory_acc_id.Name = "cmb_inventory_acc_id";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // cmb_sales_acc_id
            // 
            this.cmb_sales_acc_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_sales_acc_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_sales_acc_id.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_sales_acc_id, "cmb_sales_acc_id");
            this.cmb_sales_acc_id.Name = "cmb_sales_acc_id";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // chk_use_zatca_e_invoice
            // 
            resources.ApplyResources(this.chk_use_zatca_e_invoice, "chk_use_zatca_e_invoice");
            this.chk_use_zatca_e_invoice.Name = "chk_use_zatca_e_invoice";
            this.chk_use_zatca_e_invoice.UseVisualStyleBackColor = true;
            // 
            // frm_updateCompany
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "frm_updateCompany";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_updateCompany_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_updateCompany_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_address;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_vat_no;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.TextBox txt_contact_no;
        private System.Windows.Forms.TextBox txt_currency_id;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_image;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmb_payable_acc_id;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmb_receivable_acc_id;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmb_cash_acc_id;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmb_tax_acc_id;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmb_sales_disc_acc_id;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cmb_sales_return_acc_id;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmb_purchases_acc_id;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmb_inventory_acc_id;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmb_sales_acc_id;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmb_purchases_disc_acc_id;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cmb_purchases_return_acc_id;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cmb_commission_acc_id;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cmb_item_variance_acc_id;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblExpiryDate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chk_use_zatca_e_invoice;
    }
}