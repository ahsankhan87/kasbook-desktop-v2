
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_banks));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.Btn_deposit = new System.Windows.Forms.Button();
            this.btn_payment = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_bank_name = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_bank_report = new System.Windows.Forms.Button();
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
            resources.ApplyResources(this.btn_trans_refresh, "btn_trans_refresh");
            this.btn_trans_refresh.Name = "btn_trans_refresh";
            this.btn_trans_refresh.UseVisualStyleBackColor = true;
            this.btn_trans_refresh.Click += new System.EventHandler(this.btn_trans_refresh_Click);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            this.debit.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.debit, "debit");
            this.debit.Name = "debit";
            this.debit.ReadOnly = true;
            // 
            // grid_banks_transactions
            // 
            this.grid_banks_transactions.AllowUserToAddRows = false;
            this.grid_banks_transactions.AllowUserToDeleteRows = false;
            this.grid_banks_transactions.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_banks_transactions, "grid_banks_transactions");
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
            this.grid_banks_transactions.Name = "grid_banks_transactions";
            this.grid_banks_transactions.ReadOnly = true;
            this.grid_banks_transactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // credit
            // 
            this.credit.DataPropertyName = "credit";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            this.credit.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.credit, "credit");
            this.credit.Name = "credit";
            this.credit.ReadOnly = true;
            // 
            // balance
            // 
            this.balance.DataPropertyName = "balance";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            this.balance.DefaultCellStyle = dataGridViewCellStyle12;
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
            // btn_blank
            // 
            resources.ApplyResources(this.btn_blank, "btn_blank");
            this.btn_blank.Name = "btn_blank";
            this.btn_blank.UseVisualStyleBackColor = true;
            this.btn_blank.Click += new System.EventHandler(this.btn_blank_Click);
            // 
            // txt_name
            // 
            resources.ApplyResources(this.txt_name, "txt_name");
            this.txt_name.Name = "txt_name";
            // 
            // btn_delete
            // 
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txt_id
            // 
            resources.ApplyResources(this.txt_id, "txt_id");
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
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
            // txt_accountNo
            // 
            resources.ApplyResources(this.txt_accountNo, "txt_accountNo");
            this.txt_accountNo.Name = "txt_accountNo";
            // 
            // btn_refresh
            // 
            this.btn_refresh.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
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
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btn_update
            // 
            resources.ApplyResources(this.btn_update, "btn_update");
            this.btn_update.Name = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_holderName
            // 
            resources.ApplyResources(this.txt_holderName, "txt_holderName");
            this.txt_holderName.Name = "txt_holderName";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
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
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Name = "Detail";
            this.Detail.UseVisualStyleBackColor = true;
            // 
            // cmb_GL_account_code
            // 
            this.cmb_GL_account_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_GL_account_code.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_GL_account_code, "cmb_GL_account_code");
            this.cmb_GL_account_code.Name = "cmb_GL_account_code";
            // 
            // txt_bankBranch
            // 
            resources.ApplyResources(this.txt_bankBranch, "txt_bankBranch");
            this.txt_bankBranch.Name = "txt_bankBranch";
            // 
            // lbl_edit_status
            // 
            resources.ApplyResources(this.lbl_edit_status, "lbl_edit_status");
            this.lbl_edit_status.Name = "lbl_edit_status";
            // 
            // Transactions
            // 
            this.Transactions.Controls.Add(this.Btn_bank_report);
            this.Transactions.Controls.Add(this.Btn_deposit);
            this.Transactions.Controls.Add(this.grid_banks_transactions);
            this.Transactions.Controls.Add(this.btn_payment);
            this.Transactions.Controls.Add(this.btn_trans_refresh);
            resources.ApplyResources(this.Transactions, "Transactions");
            this.Transactions.Name = "Transactions";
            this.Transactions.UseVisualStyleBackColor = true;
            // 
            // Btn_deposit
            // 
            resources.ApplyResources(this.Btn_deposit, "Btn_deposit");
            this.Btn_deposit.Name = "Btn_deposit";
            this.Btn_deposit.UseVisualStyleBackColor = true;
            this.Btn_deposit.Click += new System.EventHandler(this.Btn_deposit_Click);
            // 
            // btn_payment
            // 
            resources.ApplyResources(this.btn_payment, "btn_payment");
            this.btn_payment.Name = "btn_payment";
            this.btn_payment.UseVisualStyleBackColor = true;
            this.btn_payment.Click += new System.EventHandler(this.btn_payment_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Detail);
            this.tabControl1.Controls.Add(this.Transactions);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lbl_bank_name
            // 
            resources.ApplyResources(this.lbl_bank_name, "lbl_bank_name");
            this.lbl_bank_name.ForeColor = System.Drawing.Color.White;
            this.lbl_bank_name.Name = "lbl_bank_name";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            // 
            // btn_search
            // 
            this.btn_search.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.btn_search, "btn_search");
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
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_bank_name);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.label21);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Name = "panel1";
            // 
            // Btn_bank_report
            // 
            resources.ApplyResources(this.Btn_bank_report, "Btn_bank_report");
            this.Btn_bank_report.Name = "Btn_bank_report";
            this.Btn_bank_report.UseVisualStyleBackColor = true;
            this.Btn_bank_report.Click += new System.EventHandler(this.Btn_bank_report_Click);
            // 
            // frm_banks
            // 
            this.AcceptButton = this.btn_search;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_banks";
            this.ShowIcon = false;
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
        private System.Windows.Forms.Button Btn_deposit;
        private System.Windows.Forms.Button Btn_bank_report;
    }
}