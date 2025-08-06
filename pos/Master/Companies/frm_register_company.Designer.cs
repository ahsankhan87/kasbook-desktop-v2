namespace pos
{
    partial class frm_register_company
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
            this.txt_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_vat_no = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_contact_no = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_currency_id = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_image = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_address = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_full_name = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.cmb_user_role = new System.Windows.Forms.ComboBox();
            this.cmb_lang = new System.Windows.Forms.ComboBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.cmb_branches = new System.Windows.Forms.ComboBox();
            this.txt_confirm_pwd = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl_pwd = new System.Windows.Forms.Label();
            this.lbl_cpwd = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtSubscriptionKey = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtSystemID = new System.Windows.Forms.TextBox();
            this.chk_use_zatca_e_invoice = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(127, 36);
            this.txt_name.Margin = new System.Windows.Forms.Padding(4);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(333, 22);
            this.txt_name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name*:";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(821, 511);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(100, 28);
            this.btn_save.TabIndex = 15;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // lbl_header_title
            // 
            this.lbl_header_title.AutoSize = true;
            this.lbl_header_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Location = new System.Drawing.Point(25, 17);
            this.lbl_header_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_header_title.Name = "lbl_header_title";
            this.lbl_header_title.Size = new System.Drawing.Size(224, 25);
            this.lbl_header_title.TabIndex = 3;
            this.lbl_header_title.Text = "Company Registration";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_header_title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1049, 58);
            this.panel1.TabIndex = 6;
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(928, 511);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(100, 28);
            this.btn_cancel.TabIndex = 17;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_vat_no
            // 
            this.txt_vat_no.Location = new System.Drawing.Point(127, 68);
            this.txt_vat_no.Margin = new System.Windows.Forms.Padding(4);
            this.txt_vat_no.Name = "txt_vat_no";
            this.txt_vat_no.Size = new System.Drawing.Size(333, 22);
            this.txt_vat_no.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "VAT No:";
            // 
            // txt_email
            // 
            this.txt_email.Location = new System.Drawing.Point(127, 100);
            this.txt_email.Margin = new System.Windows.Forms.Padding(4);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(333, 22);
            this.txt_email.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 103);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Email:";
            // 
            // txt_contact_no
            // 
            this.txt_contact_no.Location = new System.Drawing.Point(127, 132);
            this.txt_contact_no.Margin = new System.Windows.Forms.Padding(4);
            this.txt_contact_no.Name = "txt_contact_no";
            this.txt_contact_no.Size = new System.Drawing.Size(333, 22);
            this.txt_contact_no.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 135);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Contact No:";
            // 
            // txt_currency_id
            // 
            this.txt_currency_id.Location = new System.Drawing.Point(127, 284);
            this.txt_currency_id.Margin = new System.Windows.Forms.Padding(4);
            this.txt_currency_id.Name = "txt_currency_id";
            this.txt_currency_id.ReadOnly = true;
            this.txt_currency_id.Size = new System.Drawing.Size(333, 22);
            this.txt_currency_id.TabIndex = 4;
            this.txt_currency_id.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 289);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Currency:";
            this.label5.Visible = false;
            // 
            // txt_image
            // 
            this.txt_image.Location = new System.Drawing.Point(127, 316);
            this.txt_image.Margin = new System.Windows.Forms.Padding(4);
            this.txt_image.Name = "txt_image";
            this.txt_image.Size = new System.Drawing.Size(333, 22);
            this.txt_image.TabIndex = 5;
            this.txt_image.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 321);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Image:";
            this.label6.Visible = false;
            // 
            // txt_address
            // 
            this.txt_address.Location = new System.Drawing.Point(127, 161);
            this.txt_address.Margin = new System.Windows.Forms.Padding(4);
            this.txt_address.Multiline = true;
            this.txt_address.Name = "txt_address";
            this.txt_address.Size = new System.Drawing.Size(333, 117);
            this.txt_address.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 165);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "Address:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txt_full_name);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txt_username);
            this.groupBox1.Controls.Add(this.cmb_user_role);
            this.groupBox1.Controls.Add(this.cmb_lang);
            this.groupBox1.Controls.Add(this.txt_password);
            this.groupBox1.Controls.Add(this.cmb_branches);
            this.groupBox1.Controls.Add(this.txt_confirm_pwd);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.lbl_pwd);
            this.groupBox1.Controls.Add(this.lbl_cpwd);
            this.groupBox1.Location = new System.Drawing.Point(513, 79);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(515, 258);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Detail";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(28, 132);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 16);
            this.label9.TabIndex = 22;
            this.label9.Text = "User Role*:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(28, 101);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 16);
            this.label10.TabIndex = 23;
            this.label10.Text = "App Language:";
            // 
            // txt_full_name
            // 
            this.txt_full_name.Location = new System.Drawing.Point(157, 36);
            this.txt_full_name.Margin = new System.Windows.Forms.Padding(4);
            this.txt_full_name.Name = "txt_full_name";
            this.txt_full_name.Size = new System.Drawing.Size(333, 22);
            this.txt_full_name.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(28, 70);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 16);
            this.label11.TabIndex = 24;
            this.label11.Text = "Branch*:";
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(157, 158);
            this.txt_username.Margin = new System.Windows.Forms.Padding(4);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(333, 22);
            this.txt_username.TabIndex = 11;
            // 
            // cmb_user_role
            // 
            this.cmb_user_role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_user_role.FormattingEnabled = true;
            this.cmb_user_role.Location = new System.Drawing.Point(157, 127);
            this.cmb_user_role.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_user_role.Name = "cmb_user_role";
            this.cmb_user_role.Size = new System.Drawing.Size(333, 24);
            this.cmb_user_role.TabIndex = 10;
            // 
            // cmb_lang
            // 
            this.cmb_lang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_lang.FormattingEnabled = true;
            this.cmb_lang.Location = new System.Drawing.Point(157, 97);
            this.cmb_lang.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_lang.Name = "cmb_lang";
            this.cmb_lang.Size = new System.Drawing.Size(333, 24);
            this.cmb_lang.TabIndex = 9;
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(157, 188);
            this.txt_password.Margin = new System.Windows.Forms.Padding(4);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(333, 22);
            this.txt_password.TabIndex = 12;
            this.txt_password.UseSystemPasswordChar = true;
            // 
            // cmb_branches
            // 
            this.cmb_branches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_branches.FormattingEnabled = true;
            this.cmb_branches.Location = new System.Drawing.Point(157, 66);
            this.cmb_branches.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_branches.Name = "cmb_branches";
            this.cmb_branches.Size = new System.Drawing.Size(333, 24);
            this.cmb_branches.TabIndex = 8;
            // 
            // txt_confirm_pwd
            // 
            this.txt_confirm_pwd.Location = new System.Drawing.Point(157, 218);
            this.txt_confirm_pwd.Margin = new System.Windows.Forms.Padding(4);
            this.txt_confirm_pwd.Name = "txt_confirm_pwd";
            this.txt_confirm_pwd.Size = new System.Drawing.Size(333, 22);
            this.txt_confirm_pwd.TabIndex = 13;
            this.txt_confirm_pwd.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(28, 39);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 16);
            this.label12.TabIndex = 12;
            this.label12.Text = "Full Name:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(28, 160);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 16);
            this.label13.TabIndex = 13;
            this.label13.Text = "Username*:";
            // 
            // lbl_pwd
            // 
            this.lbl_pwd.AutoSize = true;
            this.lbl_pwd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_pwd.Location = new System.Drawing.Point(28, 191);
            this.lbl_pwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_pwd.Name = "lbl_pwd";
            this.lbl_pwd.Size = new System.Drawing.Size(75, 16);
            this.lbl_pwd.TabIndex = 14;
            this.lbl_pwd.Text = "Password*:";
            // 
            // lbl_cpwd
            // 
            this.lbl_cpwd.AutoSize = true;
            this.lbl_cpwd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_cpwd.Location = new System.Drawing.Point(28, 220);
            this.lbl_cpwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_cpwd.Name = "lbl_cpwd";
            this.lbl_cpwd.Size = new System.Drawing.Size(118, 16);
            this.lbl_cpwd.TabIndex = 15;
            this.lbl_cpwd.Text = "Confirm Password:";
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
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txt_image);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(16, 79);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(489, 398);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Company Detail";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 480);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(161, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Note: * are required fields.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtSubscriptionKey);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtSystemID);
            this.groupBox3.Location = new System.Drawing.Point(513, 345);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(515, 132);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Subscription";
            // 
            // txtSubscriptionKey
            // 
            this.txtSubscriptionKey.Location = new System.Drawing.Point(157, 55);
            this.txtSubscriptionKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtSubscriptionKey.Multiline = true;
            this.txtSubscriptionKey.Name = "txtSubscriptionKey";
            this.txtSubscriptionKey.Size = new System.Drawing.Size(333, 68);
            this.txtSubscriptionKey.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 23);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 16);
            this.label15.TabIndex = 1;
            this.label15.Text = "System ID:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(28, 59);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 16);
            this.label14.TabIndex = 1;
            this.label14.Text = "Key:";
            // 
            // txtSystemID
            // 
            this.txtSystemID.Location = new System.Drawing.Point(157, 20);
            this.txtSystemID.Margin = new System.Windows.Forms.Padding(4);
            this.txtSystemID.Name = "txtSystemID";
            this.txtSystemID.ReadOnly = true;
            this.txtSystemID.Size = new System.Drawing.Size(333, 22);
            this.txtSystemID.TabIndex = 11;
            // 
            // chk_use_zatca_e_invoice
            // 
            this.chk_use_zatca_e_invoice.AutoSize = true;
            this.chk_use_zatca_e_invoice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chk_use_zatca_e_invoice.Location = new System.Drawing.Point(35, 358);
            this.chk_use_zatca_e_invoice.Name = "chk_use_zatca_e_invoice";
            this.chk_use_zatca_e_invoice.Size = new System.Drawing.Size(146, 20);
            this.chk_use_zatca_e_invoice.TabIndex = 113;
            this.chk_use_zatca_e_invoice.Text = "Use Zatca EInvoice";
            this.chk_use_zatca_e_invoice.UseVisualStyleBackColor = true;
            // 
            // frm_register_company
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(1049, 554);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frm_register_company";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Company Registration";
            this.Load += new System.EventHandler(this.frm_register_company_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_register_company_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_vat_no;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_contact_no;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_currency_id;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_image;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_address;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_full_name;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.ComboBox cmb_user_role;
        private System.Windows.Forms.ComboBox cmb_lang;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.ComboBox cmb_branches;
        private System.Windows.Forms.TextBox txt_confirm_pwd;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbl_pwd;
        private System.Windows.Forms.Label lbl_cpwd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSubscriptionKey;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtSystemID;
        private System.Windows.Forms.CheckBox chk_use_zatca_e_invoice;
    }
}