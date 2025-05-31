namespace pos
{
    partial class frm_addCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_addCustomer));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txt_first_name = new System.Windows.Forms.TextBox();
            this.txt_last_name = new System.Windows.Forms.TextBox();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.txt_address = new System.Windows.Forms.TextBox();
            this.txt_contact_no = new System.Windows.Forms.TextBox();
            this.txt_vatno = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_customer_name = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.lbl_edit_status = new System.Windows.Forms.Label();
            this.txt_credit_limit = new System.Windows.Forms.TextBox();
            this.txt_vin_no = new System.Windows.Forms.TextBox();
            this.txt_car_name = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Detail = new System.Windows.Forms.TabPage();
            this.Transactions = new System.Windows.Forms.TabPage();
            this.Btn_ledger_report = new System.Windows.Forms.Button();
            this.btn_payment = new System.Windows.Forms.Button();
            this.btn_trans_refresh = new System.Windows.Forms.Button();
            this.grid_customer_transactions = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entry_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.debit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_blank = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Detail.SuspendLayout();
            this.Transactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_customer_transactions)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_first_name
            // 
            resources.ApplyResources(this.txt_first_name, "txt_first_name");
            this.txt_first_name.Name = "txt_first_name";
            // 
            // txt_last_name
            // 
            resources.ApplyResources(this.txt_last_name, "txt_last_name");
            this.txt_last_name.Name = "txt_last_name";
            // 
            // txt_email
            // 
            resources.ApplyResources(this.txt_email, "txt_email");
            this.txt_email.Name = "txt_email";
            // 
            // txt_address
            // 
            resources.ApplyResources(this.txt_address, "txt_address");
            this.txt_address.Name = "txt_address";
            // 
            // txt_contact_no
            // 
            resources.ApplyResources(this.txt_contact_no, "txt_contact_no");
            this.txt_contact_no.Name = "txt_contact_no";
            // 
            // txt_vatno
            // 
            resources.ApplyResources(this.txt_vatno, "txt_vatno");
            this.txt_vatno.Name = "txt_vatno";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_customer_name);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.label21);
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Name = "panel1";
            // 
            // lbl_customer_name
            // 
            resources.ApplyResources(this.lbl_customer_name, "lbl_customer_name");
            this.lbl_customer_name.ForeColor = System.Drawing.Color.White;
            this.lbl_customer_name.Name = "lbl_customer_name";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Name = "label21";
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_id
            // 
            resources.ApplyResources(this.txt_id, "txt_id");
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            // 
            // lbl_edit_status
            // 
            resources.ApplyResources(this.lbl_edit_status, "lbl_edit_status");
            this.lbl_edit_status.Name = "lbl_edit_status";
            // 
            // txt_credit_limit
            // 
            resources.ApplyResources(this.txt_credit_limit, "txt_credit_limit");
            this.txt_credit_limit.Name = "txt_credit_limit";
            // 
            // txt_vin_no
            // 
            resources.ApplyResources(this.txt_vin_no, "txt_vin_no");
            this.txt_vin_no.Name = "txt_vin_no";
            // 
            // txt_car_name
            // 
            resources.ApplyResources(this.txt_car_name, "txt_car_name");
            this.txt_car_name.Name = "txt_car_name";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Name = "panel2";
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.Detail);
            this.tabControl1.Controls.Add(this.Transactions);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // Detail
            // 
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Controls.Add(this.label1);
            this.Detail.Controls.Add(this.txt_first_name);
            this.Detail.Controls.Add(this.label6);
            this.Detail.Controls.Add(this.txt_vin_no);
            this.Detail.Controls.Add(this.label5);
            this.Detail.Controls.Add(this.txt_car_name);
            this.Detail.Controls.Add(this.label4);
            this.Detail.Controls.Add(this.txt_id);
            this.Detail.Controls.Add(this.label3);
            this.Detail.Controls.Add(this.txt_last_name);
            this.Detail.Controls.Add(this.label7);
            this.Detail.Controls.Add(this.txt_vatno);
            this.Detail.Controls.Add(this.label2);
            this.Detail.Controls.Add(this.txt_email);
            this.Detail.Controls.Add(this.label8);
            this.Detail.Controls.Add(this.txt_address);
            this.Detail.Controls.Add(this.label10);
            this.Detail.Controls.Add(this.txt_contact_no);
            this.Detail.Controls.Add(this.label9);
            this.Detail.Controls.Add(this.txt_credit_limit);
            this.Detail.Controls.Add(this.lbl_edit_status);
            this.Detail.Name = "Detail";
            this.Detail.UseVisualStyleBackColor = true;
            // 
            // Transactions
            // 
            resources.ApplyResources(this.Transactions, "Transactions");
            this.Transactions.Controls.Add(this.Btn_ledger_report);
            this.Transactions.Controls.Add(this.btn_payment);
            this.Transactions.Controls.Add(this.btn_trans_refresh);
            this.Transactions.Controls.Add(this.grid_customer_transactions);
            this.Transactions.Name = "Transactions";
            this.Transactions.UseVisualStyleBackColor = true;
            // 
            // Btn_ledger_report
            // 
            resources.ApplyResources(this.Btn_ledger_report, "Btn_ledger_report");
            this.Btn_ledger_report.Name = "Btn_ledger_report";
            this.Btn_ledger_report.UseVisualStyleBackColor = true;
            this.Btn_ledger_report.Click += new System.EventHandler(this.Btn_ledger_report_Click);
            // 
            // btn_payment
            // 
            resources.ApplyResources(this.btn_payment, "btn_payment");
            this.btn_payment.Name = "btn_payment";
            this.btn_payment.UseVisualStyleBackColor = true;
            this.btn_payment.Click += new System.EventHandler(this.btn_payment_Click);
            // 
            // btn_trans_refresh
            // 
            resources.ApplyResources(this.btn_trans_refresh, "btn_trans_refresh");
            this.btn_trans_refresh.Name = "btn_trans_refresh";
            this.btn_trans_refresh.UseVisualStyleBackColor = true;
            this.btn_trans_refresh.Click += new System.EventHandler(this.btn_trans_refresh_Click);
            // 
            // grid_customer_transactions
            // 
            resources.ApplyResources(this.grid_customer_transactions, "grid_customer_transactions");
            this.grid_customer_transactions.AllowUserToAddRows = false;
            this.grid_customer_transactions.AllowUserToDeleteRows = false;
            this.grid_customer_transactions.AllowUserToOrderColumns = true;
            this.grid_customer_transactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_customer_transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_customer_transactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.entry_date,
            this.account_name,
            this.debit,
            this.credit,
            this.balance,
            this.description});
            this.grid_customer_transactions.Name = "grid_customer_transactions";
            this.grid_customer_transactions.ReadOnly = true;
            this.grid_customer_transactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // entry_date
            // 
            this.entry_date.DataPropertyName = "entry_date";
            resources.ApplyResources(this.entry_date, "entry_date");
            this.entry_date.Name = "entry_date";
            this.entry_date.ReadOnly = true;
            // 
            // account_name
            // 
            this.account_name.DataPropertyName = "account_name";
            resources.ApplyResources(this.account_name, "account_name");
            this.account_name.Name = "account_name";
            this.account_name.ReadOnly = true;
            // 
            // debit
            // 
            this.debit.DataPropertyName = "debit";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.debit.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.debit, "debit");
            this.debit.Name = "debit";
            this.debit.ReadOnly = true;
            // 
            // credit
            // 
            this.credit.DataPropertyName = "credit";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.credit.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.credit, "credit");
            this.credit.Name = "credit";
            this.credit.ReadOnly = true;
            // 
            // balance
            // 
            this.balance.DataPropertyName = "balance";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.balance.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.balance, "balance");
            this.balance.Name = "balance";
            this.balance.ReadOnly = true;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Name = "panel3";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btn_save);
            this.groupBox2.Controls.Add(this.btn_refresh);
            this.groupBox2.Controls.Add(this.btn_delete);
            this.groupBox2.Controls.Add(this.btn_update);
            this.groupBox2.Controls.Add(this.btn_blank);
            this.groupBox2.Controls.Add(this.btn_cancel);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            // btn_blank
            // 
            resources.ApplyResources(this.btn_blank, "btn_blank");
            this.btn_blank.Name = "btn_blank";
            this.btn_blank.UseVisualStyleBackColor = true;
            this.btn_blank.Click += new System.EventHandler(this.btn_blank_Click);
            // 
            // frm_addCustomer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "frm_addCustomer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_addCustomer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_addCustomer_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Detail.ResumeLayout(false);
            this.Detail.PerformLayout();
            this.Transactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_customer_transactions)).EndInit();
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_first_name;
        private System.Windows.Forms.TextBox txt_last_name;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.TextBox txt_address;
        private System.Windows.Forms.TextBox txt_contact_no;
        private System.Windows.Forms.TextBox txt_vatno;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label lbl_edit_status;
        private System.Windows.Forms.TextBox txt_credit_limit;
        private System.Windows.Forms.TextBox txt_vin_no;
        private System.Windows.Forms.TextBox txt_car_name;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Detail;
        private System.Windows.Forms.TabPage Transactions;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_blank;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataGridView grid_customer_transactions;
        private System.Windows.Forms.Button btn_payment;
        private System.Windows.Forms.Button btn_trans_refresh;
        private System.Windows.Forms.Label lbl_customer_name;
        private System.Windows.Forms.Button Btn_ledger_report;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn entry_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn account_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn debit;
        private System.Windows.Forms.DataGridViewTextBoxColumn credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
    }
}