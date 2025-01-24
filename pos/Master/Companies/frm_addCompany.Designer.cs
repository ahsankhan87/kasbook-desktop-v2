namespace pos
{
    partial class frm_addCompany
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
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_edit_status = new System.Windows.Forms.Label();
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
            this.cmb_payable_acc_id = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmb_receivable_acc_id = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmb_cash_acc_id = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_tax_acc_id = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmb_sales_disc_acc_id = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cmb_sales_return_acc_id = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmb_cos_acc_id = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmb_inventory_acc_id = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmb_sales_acc_id = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(124, 66);
            this.txt_name.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(333, 22);
            this.txt_name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(805, 468);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(100, 28);
            this.btn_save.TabIndex = 7;
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
            this.lbl_header_title.Size = new System.Drawing.Size(197, 25);
            this.lbl_header_title.TabIndex = 3;
            this.lbl_header_title.Text = "Add New Company";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_header_title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1049, 58);
            this.panel1.TabIndex = 6;
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(912, 468);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(100, 28);
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(124, 34);
            this.txt_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.Size = new System.Drawing.Size(333, 22);
            this.txt_id.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "ID:";
            // 
            // lbl_edit_status
            // 
            this.lbl_edit_status.AutoSize = true;
            this.lbl_edit_status.Location = new System.Drawing.Point(544, 468);
            this.lbl_edit_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_edit_status.Name = "lbl_edit_status";
            this.lbl_edit_status.Size = new System.Drawing.Size(99, 17);
            this.lbl_edit_status.TabIndex = 9;
            this.lbl_edit_status.Text = "lbl_edit_status";
            this.lbl_edit_status.Visible = false;
            // 
            // txt_vat_no
            // 
            this.txt_vat_no.Location = new System.Drawing.Point(124, 98);
            this.txt_vat_no.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_vat_no.Name = "txt_vat_no";
            this.txt_vat_no.Size = new System.Drawing.Size(333, 22);
            this.txt_vat_no.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "VAT No:";
            // 
            // txt_email
            // 
            this.txt_email.Location = new System.Drawing.Point(124, 130);
            this.txt_email.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(333, 22);
            this.txt_email.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 134);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Email:";
            // 
            // txt_contact_no
            // 
            this.txt_contact_no.Location = new System.Drawing.Point(124, 162);
            this.txt_contact_no.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_contact_no.Name = "txt_contact_no";
            this.txt_contact_no.Size = new System.Drawing.Size(333, 22);
            this.txt_contact_no.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Contact No:";
            // 
            // txt_currency_id
            // 
            this.txt_currency_id.Location = new System.Drawing.Point(124, 194);
            this.txt_currency_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_currency_id.Name = "txt_currency_id";
            this.txt_currency_id.ReadOnly = true;
            this.txt_currency_id.Size = new System.Drawing.Size(333, 22);
            this.txt_currency_id.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 198);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Currency:";
            // 
            // txt_image
            // 
            this.txt_image.Location = new System.Drawing.Point(124, 226);
            this.txt_image.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_image.Name = "txt_image";
            this.txt_image.Size = new System.Drawing.Size(333, 22);
            this.txt_image.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 230);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Image:";
            // 
            // txt_address
            // 
            this.txt_address.Location = new System.Drawing.Point(124, 258);
            this.txt_address.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_address.Multiline = true;
            this.txt_address.Name = "txt_address";
            this.txt_address.Size = new System.Drawing.Size(333, 117);
            this.txt_address.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 262);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Address:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_payable_acc_id);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cmb_receivable_acc_id);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmb_cash_acc_id);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmb_tax_acc_id);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmb_sales_disc_acc_id);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.cmb_sales_return_acc_id);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.cmb_cos_acc_id);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cmb_inventory_acc_id);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cmb_sales_acc_id);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Location = new System.Drawing.Point(513, 79);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(515, 342);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GL Accounts";
            // 
            // cmb_payable_acc_id
            // 
            this.cmb_payable_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_payable_acc_id.FormattingEnabled = true;
            this.cmb_payable_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_payable_acc_id.Location = new System.Drawing.Point(172, 292);
            this.cmb_payable_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_payable_acc_id.Name = "cmb_payable_acc_id";
            this.cmb_payable_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_payable_acc_id.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 295);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(118, 17);
            this.label12.TabIndex = 18;
            this.label12.Text = "Payable Account:";
            // 
            // cmb_receivable_acc_id
            // 
            this.cmb_receivable_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_receivable_acc_id.FormattingEnabled = true;
            this.cmb_receivable_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_receivable_acc_id.Location = new System.Drawing.Point(172, 258);
            this.cmb_receivable_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_receivable_acc_id.Name = "cmb_receivable_acc_id";
            this.cmb_receivable_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_receivable_acc_id.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 262);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 17);
            this.label10.TabIndex = 14;
            this.label10.Text = "Receivable Account :";
            // 
            // cmb_cash_acc_id
            // 
            this.cmb_cash_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_cash_acc_id.FormattingEnabled = true;
            this.cmb_cash_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_cash_acc_id.Location = new System.Drawing.Point(172, 224);
            this.cmb_cash_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_cash_acc_id.Name = "cmb_cash_acc_id";
            this.cmb_cash_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_cash_acc_id.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 228);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 17);
            this.label11.TabIndex = 15;
            this.label11.Text = "Cash Account:";
            // 
            // cmb_tax_acc_id
            // 
            this.cmb_tax_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_tax_acc_id.FormattingEnabled = true;
            this.cmb_tax_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_tax_acc_id.Location = new System.Drawing.Point(172, 192);
            this.cmb_tax_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_tax_acc_id.Name = "cmb_tax_acc_id";
            this.cmb_tax_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_tax_acc_id.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 196);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 17);
            this.label9.TabIndex = 12;
            this.label9.Text = "Tax GL Account:";
            // 
            // cmb_sales_disc_acc_id
            // 
            this.cmb_sales_disc_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sales_disc_acc_id.FormattingEnabled = true;
            this.cmb_sales_disc_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_sales_disc_acc_id.Location = new System.Drawing.Point(172, 159);
            this.cmb_sales_disc_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_sales_disc_acc_id.Name = "cmb_sales_disc_acc_id";
            this.cmb_sales_disc_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_sales_disc_acc_id.TabIndex = 3;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 165);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(161, 17);
            this.label17.TabIndex = 1;
            this.label17.Text = "Sales Discount Account:";
            // 
            // cmb_sales_return_acc_id
            // 
            this.cmb_sales_return_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sales_return_acc_id.FormattingEnabled = true;
            this.cmb_sales_return_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_sales_return_acc_id.Location = new System.Drawing.Point(172, 124);
            this.cmb_sales_return_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_sales_return_acc_id.Name = "cmb_sales_return_acc_id";
            this.cmb_sales_return_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_sales_return_acc_id.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 130);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(149, 17);
            this.label16.TabIndex = 1;
            this.label16.Text = "Sales Return Account:";
            // 
            // cmb_cos_acc_id
            // 
            this.cmb_cos_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_cos_acc_id.FormattingEnabled = true;
            this.cmb_cos_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_cos_acc_id.Location = new System.Drawing.Point(172, 90);
            this.cmb_cos_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_cos_acc_id.Name = "cmb_cos_acc_id";
            this.cmb_cos_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_cos_acc_id.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 96);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(119, 17);
            this.label15.TabIndex = 1;
            this.label15.Text = "C.O.G.S Account:";
            // 
            // cmb_inventory_acc_id
            // 
            this.cmb_inventory_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_inventory_acc_id.FormattingEnabled = true;
            this.cmb_inventory_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_inventory_acc_id.Location = new System.Drawing.Point(172, 55);
            this.cmb_inventory_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_inventory_acc_id.Name = "cmb_inventory_acc_id";
            this.cmb_inventory_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_inventory_acc_id.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 62);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(125, 17);
            this.label14.TabIndex = 1;
            this.label14.Text = "Inventory Account:";
            // 
            // cmb_sales_acc_id
            // 
            this.cmb_sales_acc_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sales_acc_id.FormattingEnabled = true;
            this.cmb_sales_acc_id.Items.AddRange(new object[] {
            "Purchased",
            "Service"});
            this.cmb_sales_acc_id.Location = new System.Drawing.Point(172, 21);
            this.cmb_sales_acc_id.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_sales_acc_id.Name = "cmb_sales_acc_id";
            this.cmb_sales_acc_id.Size = new System.Drawing.Size(333, 24);
            this.cmb_sales_acc_id.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 27);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "Sales Account:";
            // 
            // groupBox2
            // 
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
            this.groupBox2.Location = new System.Drawing.Point(16, 79);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(489, 417);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Company Detail";
            // 
            // frm_addCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(1049, 526);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_edit_status);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "frm_addCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New Company";
            this.Load += new System.EventHandler(this.frm_addCompany_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_addCompany_KeyDown);
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

        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_edit_status;
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
        private System.Windows.Forms.ComboBox cmb_sales_disc_acc_id;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cmb_sales_return_acc_id;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmb_cos_acc_id;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmb_inventory_acc_id;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmb_sales_acc_id;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmb_tax_acc_id;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmb_receivable_acc_id;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmb_cash_acc_id;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmb_payable_acc_id;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}