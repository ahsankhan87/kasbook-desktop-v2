namespace pos
{
    partial class frm_customer_payment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_customer_payment));
            this.label1 = new System.Windows.Forms.Label();
            this.txt_total_amount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_description = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.txt_discount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_payment_date = new System.Windows.Forms.DateTimePicker();
            this.cmb_GL_account_code = new System.Windows.Forms.ComboBox();
            this.cmb_ref_account_code = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblGLAccountDebitName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_total_amount
            // 
            resources.ApplyResources(this.txt_total_amount, "txt_total_amount");
            this.txt_total_amount.Name = "txt_total_amount";
            this.txt_total_amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_total_amount_KeyPress);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txt_description
            // 
            resources.ApplyResources(this.txt_description, "txt_description");
            this.txt_description.Name = "txt_description";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
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
            // txt_discount
            // 
            resources.ApplyResources(this.txt_discount, "txt_discount");
            this.txt_discount.Name = "txt_discount";
            this.txt_discount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_discount_KeyPress);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_header_title);
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Name = "panel1";
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_payment_date
            // 
            resources.ApplyResources(this.txt_payment_date, "txt_payment_date");
            this.txt_payment_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_payment_date.Name = "txt_payment_date";
            // 
            // cmb_GL_account_code
            // 
            resources.ApplyResources(this.cmb_GL_account_code, "cmb_GL_account_code");
            this.cmb_GL_account_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_GL_account_code.FormattingEnabled = true;
            this.cmb_GL_account_code.Name = "cmb_GL_account_code";
            this.cmb_GL_account_code.SelectedIndexChanged += new System.EventHandler(this.cmb_cash_account_code_SelectedIndexChanged);
            // 
            // cmb_ref_account_code
            // 
            resources.ApplyResources(this.cmb_ref_account_code, "cmb_ref_account_code");
            this.cmb_ref_account_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ref_account_code.FormattingEnabled = true;
            this.cmb_ref_account_code.Name = "cmb_ref_account_code";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox1.Controls.Add(this.lblCustomerName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblGLAccountDebitName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblCustomerName
            // 
            resources.ApplyResources(this.lblCustomerName, "lblCustomerName");
            this.lblCustomerName.Name = "lblCustomerName";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblGLAccountDebitName
            // 
            resources.ApplyResources(this.lblGLAccountDebitName, "lblGLAccountDebitName");
            this.lblGLAccountDebitName.Name = "lblGLAccountDebitName";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmb_ref_account_code);
            this.groupBox2.Controls.Add(this.cmb_GL_account_code);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // frm_customer_payment
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txt_payment_date);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_description);
            this.Controls.Add(this.txt_discount);
            this.Controls.Add(this.txt_total_amount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frm_customer_payment";
            this.Load += new System.EventHandler(this.frm_customer_payment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_customer_payment_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_total_amount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_description;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.TextBox txt_discount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DateTimePicker txt_payment_date;
        private System.Windows.Forms.ComboBox cmb_GL_account_code;
        private System.Windows.Forms.ComboBox cmb_ref_account_code;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblGLAccountDebitName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}