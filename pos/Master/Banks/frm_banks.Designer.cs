
namespace pos.Master.Banks
{
    partial class frm_banks
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_trans_refresh = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entry_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.account_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.debit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grid_banks_transactions = new System.Windows.Forms.DataGridView();
            this.credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_blank = new System.Windows.Forms.Button();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.btn_delete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_accountNo = new System.Windows.Forms.TextBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_holderName = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.Detail = new System.Windows.Forms.TabPage();
            this.cmb_GL_account_code = new System.Windows.Forms.ComboBox();
            this.txt_bankBranch = new System.Windows.Forms.TextBox();
            this.lbl_edit_status = new System.Windows.Forms.Label();
            this.Transactions = new System.Windows.Forms.TabPage();
            this.btn_payment = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_bank_name = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grid_banks_transactions)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.Detail.SuspendLayout();
            this.Transactions.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_trans_refresh
            // 
            this.btn_trans_refresh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_trans_refresh.Location = new System.Drawing.Point(7, 12);
            this.btn_trans_refresh.Margin = new System.Windows.Forms.Padding(4);
            this.btn_trans_refresh.Name = "btn_trans_refresh";
            this.btn_trans_refresh.Size = new System.Drawing.Size(100, 28);
            this.btn_trans_refresh.TabIndex = 6;
            this.btn_trans_refresh.Text = "Refresh";
            this.btn_trans_refresh.UseVisualStyleBackColor = true;
            this.btn_trans_refresh.Click += new System.EventHandler(this.btn_trans_refresh_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            this.invoice_no.HeaderText = "Invoice No.";
            this.invoice_no.MinimumWidth = 6;
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // entry_date
            // 
            this.entry_date.DataPropertyName = "entry_date";
            this.entry_date.HeaderText = "Entry Date";
            this.entry_date.MinimumWidth = 6;
            this.entry_date.Name = "entry_date";
            this.entry_date.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(77, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "GL Account";
            // 
            // account_name
            // 
            this.account_name.DataPropertyName = "account_name";
            this.account_name.HeaderText = "Account";
            this.account_name.MinimumWidth = 6;
            this.account_name.Name = "account_name";
            this.account_name.ReadOnly = true;
            // 
            // debit
            // 
            this.debit.DataPropertyName = "debit";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            this.debit.DefaultCellStyle = dataGridViewCellStyle1;
            this.debit.HeaderText = "Debit";
            this.debit.MinimumWidth = 6;
            this.debit.Name = "debit";
            this.debit.ReadOnly = true;
            // 
            // grid_banks_transactions
            // 
            this.grid_banks_transactions.AllowUserToAddRows = false;
            this.grid_banks_transactions.AllowUserToDeleteRows = false;
            this.grid_banks_transactions.AllowUserToOrderColumns = true;
            this.grid_banks_transactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_banks_transactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_banks_transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_banks_transactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.entry_date,
            this.account_name,
            this.debit,
            this.credit,
            this.balance,
            this.description});
            this.grid_banks_transactions.Location = new System.Drawing.Point(3, 48);
            this.grid_banks_transactions.Margin = new System.Windows.Forms.Padding(4);
            this.grid_banks_transactions.Name = "grid_banks_transactions";
            this.grid_banks_transactions.ReadOnly = true;
            this.grid_banks_transactions.RowHeadersWidth = 51;
            this.grid_banks_transactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_banks_transactions.Size = new System.Drawing.Size(684, 373);
            this.grid_banks_transactions.TabIndex = 1;
            // 
            // credit
            // 
            this.credit.DataPropertyName = "credit";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.credit.DefaultCellStyle = dataGridViewCellStyle2;
            this.credit.HeaderText = "Credit";
            this.credit.MinimumWidth = 6;
            this.credit.Name = "credit";
            this.credit.ReadOnly = true;
            // 
            // balance
            // 
            this.balance.DataPropertyName = "balance";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.balance.DefaultCellStyle = dataGridViewCellStyle3;
            this.balance.HeaderText = "Balance";
            this.balance.MinimumWidth = 6;
            this.balance.Name = "balance";
            this.balance.ReadOnly = true;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "Description";
            this.description.MinimumWidth = 6;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // btn_blank
            // 
            this.btn_blank.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btn_blank.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_blank.Location = new System.Drawing.Point(20, 23);
            this.btn_blank.Margin = new System.Windows.Forms.Padding(4);
            this.btn_blank.Name = "btn_blank";
            this.btn_blank.Size = new System.Drawing.Size(100, 28);
            this.btn_blank.TabIndex = 0;
            this.btn_blank.Text = "Blank All";
            this.btn_blank.UseVisualStyleBackColor = true;
            this.btn_blank.Click += new System.EventHandler(this.btn_blank_Click);
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(172, 92);
            this.txt_name.Margin = new System.Windows.Forms.Padding(4);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(333, 22);
            this.txt_name.TabIndex = 1;
            // 
            // btn_delete
            // 
            this.btn_delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btn_delete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_delete.Location = new System.Drawing.Point(20, 207);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(4);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(100, 28);
            this.btn_delete.TabIndex = 4;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(77, 192);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Bank Branch";
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(172, 28);
            this.txt_id.Margin = new System.Windows.Forms.Padding(4);
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.Size = new System.Drawing.Size(333, 22);
            this.txt_id.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(77, 160);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Holder Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(77, 128);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Account No.";
            // 
            // txt_accountNo
            // 
            this.txt_accountNo.Location = new System.Drawing.Point(172, 124);
            this.txt_accountNo.Margin = new System.Windows.Forms.Padding(4);
            this.txt_accountNo.Name = "txt_accountNo";
            this.txt_accountNo.Size = new System.Drawing.Size(333, 22);
            this.txt_accountNo.TabIndex = 2;
            // 
            // btn_refresh
            // 
            this.btn_refresh.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btn_refresh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_refresh.Location = new System.Drawing.Point(20, 149);
            this.btn_refresh.Margin = new System.Windows.Forms.Padding(4);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(100, 28);
            this.btn_refresh.TabIndex = 3;
            this.btn_refresh.Text = "Refresh (F5)";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(77, 96);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bank Name";
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btn_save.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_save.Location = new System.Drawing.Point(20, 76);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(100, 28);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "Create (F3)";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_save);
            this.groupBox2.Controls.Add(this.btn_refresh);
            this.groupBox2.Controls.Add(this.btn_delete);
            this.groupBox2.Controls.Add(this.btn_update);
            this.groupBox2.Controls.Add(this.btn_blank);
            this.groupBox2.Controls.Add(this.btn_cancel);
            this.groupBox2.Location = new System.Drawing.Point(13, 82);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(136, 330);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Action";
            // 
            // btn_update
            // 
            this.btn_update.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btn_update.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_update.Location = new System.Drawing.Point(20, 113);
            this.btn_update.Margin = new System.Windows.Forms.Padding(4);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(100, 28);
            this.btn_update.TabIndex = 2;
            this.btn_update.Text = "Update (F4)";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_cancel.Location = new System.Drawing.Point(20, 242);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(100, 28);
            this.btn_cancel.TabIndex = 11;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_holderName
            // 
            this.txt_holderName.Location = new System.Drawing.Point(172, 156);
            this.txt_holderName.Margin = new System.Windows.Forms.Padding(4);
            this.txt_holderName.Name = "txt_holderName";
            this.txt_holderName.Size = new System.Drawing.Size(333, 22);
            this.txt_holderName.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(702, 58);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(161, 456);
            this.panel3.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(77, 32);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "ID";
            // 
            // Detail
            // 
            this.Detail.Controls.Add(this.cmb_GL_account_code);
            this.Detail.Controls.Add(this.label1);
            this.Detail.Controls.Add(this.label5);
            this.Detail.Controls.Add(this.txt_id);
            this.Detail.Controls.Add(this.label3);
            this.Detail.Controls.Add(this.txt_name);
            this.Detail.Controls.Add(this.label7);
            this.Detail.Controls.Add(this.txt_accountNo);
            this.Detail.Controls.Add(this.label2);
            this.Detail.Controls.Add(this.txt_holderName);
            this.Detail.Controls.Add(this.label8);
            this.Detail.Controls.Add(this.txt_bankBranch);
            this.Detail.Controls.Add(this.lbl_edit_status);
            this.Detail.Location = new System.Drawing.Point(4, 25);
            this.Detail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Detail.Name = "Detail";
            this.Detail.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Detail.Size = new System.Drawing.Size(694, 427);
            this.Detail.TabIndex = 0;
            this.Detail.Text = "Detail";
            this.Detail.UseVisualStyleBackColor = true;
            // 
            // cmb_GL_account_code
            // 
            this.cmb_GL_account_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_GL_account_code.FormattingEnabled = true;
            this.cmb_GL_account_code.Location = new System.Drawing.Point(172, 60);
            this.cmb_GL_account_code.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_GL_account_code.Name = "cmb_GL_account_code";
            this.cmb_GL_account_code.Size = new System.Drawing.Size(333, 24);
            this.cmb_GL_account_code.TabIndex = 10;
            // 
            // txt_bankBranch
            // 
            this.txt_bankBranch.Location = new System.Drawing.Point(172, 188);
            this.txt_bankBranch.Margin = new System.Windows.Forms.Padding(4);
            this.txt_bankBranch.Name = "txt_bankBranch";
            this.txt_bankBranch.Size = new System.Drawing.Size(333, 22);
            this.txt_bankBranch.TabIndex = 5;
            // 
            // lbl_edit_status
            // 
            this.lbl_edit_status.AutoSize = true;
            this.lbl_edit_status.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_edit_status.Location = new System.Drawing.Point(1, 401);
            this.lbl_edit_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_edit_status.Name = "lbl_edit_status";
            this.lbl_edit_status.Size = new System.Drawing.Size(99, 17);
            this.lbl_edit_status.TabIndex = 9;
            this.lbl_edit_status.Text = "lbl_edit_status";
            this.lbl_edit_status.Visible = false;
            // 
            // Transactions
            // 
            this.Transactions.Controls.Add(this.grid_banks_transactions);
            this.Transactions.Controls.Add(this.btn_payment);
            this.Transactions.Controls.Add(this.btn_trans_refresh);
            this.Transactions.Location = new System.Drawing.Point(4, 25);
            this.Transactions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Transactions.Name = "Transactions";
            this.Transactions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Transactions.Size = new System.Drawing.Size(694, 427);
            this.Transactions.TabIndex = 1;
            this.Transactions.Text = "Transactions";
            this.Transactions.UseVisualStyleBackColor = true;
            // 
            // btn_payment
            // 
            this.btn_payment.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_payment.Location = new System.Drawing.Point(115, 12);
            this.btn_payment.Margin = new System.Windows.Forms.Padding(4);
            this.btn_payment.Name = "btn_payment";
            this.btn_payment.Size = new System.Drawing.Size(100, 28);
            this.btn_payment.TabIndex = 5;
            this.btn_payment.Text = "Payment";
            this.btn_payment.UseVisualStyleBackColor = true;
            this.btn_payment.Click += new System.EventHandler(this.btn_payment_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Detail);
            this.tabControl1.Controls.Add(this.Transactions);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 456);
            this.tabControl1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 58);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(702, 456);
            this.panel2.TabIndex = 5;
            // 
            // lbl_bank_name
            // 
            this.lbl_bank_name.AutoSize = true;
            this.lbl_bank_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_bank_name.ForeColor = System.Drawing.Color.White;
            this.lbl_bank_name.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_bank_name.Location = new System.Drawing.Point(512, 17);
            this.lbl_bank_name.Name = "lbl_bank_name";
            this.lbl_bank_name.Size = new System.Drawing.Size(88, 17);
            this.lbl_bank_name.TabIndex = 14;
            this.lbl_bank_name.Text = "Bank name";
            this.lbl_bank_name.Visible = false;
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(177, 15);
            this.txt_search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(232, 22);
            this.txt_search.TabIndex = 0;
            // 
            // btn_search
            // 
            this.btn_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_search.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_search.Location = new System.Drawing.Point(415, 15);
            this.btn_search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(72, 26);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label21.Location = new System.Drawing.Point(47, 17);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(89, 17);
            this.label21.TabIndex = 13;
            this.label21.Text = "Bank Search";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_bank_name);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(863, 58);
            this.panel1.TabIndex = 4;
            // 
            // frm_banks
            // 
            this.AcceptButton = this.btn_search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(863, 514);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frm_banks";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Banks";
            this.Load += new System.EventHandler(this.frm_banks_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_banks_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_banks_transactions)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.Detail.ResumeLayout(false);
            this.Detail.PerformLayout();
            this.Transactions.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_trans_refresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn entry_date;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn account_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn debit;
        private System.Windows.Forms.DataGridView grid_banks_transactions;
        private System.Windows.Forms.DataGridViewTextBoxColumn credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.Button btn_blank;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_accountNo;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_holderName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage Detail;
        private System.Windows.Forms.TextBox txt_bankBranch;
        private System.Windows.Forms.Label lbl_edit_status;
        private System.Windows.Forms.TabPage Transactions;
        private System.Windows.Forms.Button btn_payment;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_bank_name;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmb_GL_account_code;
    }
}