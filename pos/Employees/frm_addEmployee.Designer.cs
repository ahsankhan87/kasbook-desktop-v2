namespace pos
{
    partial class frm_addEmployee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_addEmployee));
            this.txt_first_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_last_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_address = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_contact_no = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.txt_vatno = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_edit_status = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_commission_percent = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.detail = new System.Windows.Forms.TabPage();
            this.commission = new System.Windows.Forms.TabPage();
            this.btn_payment = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.grid_commission = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entry_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.debit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.detail.SuspendLayout();
            this.commission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_commission)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_first_name
            // 
            resources.ApplyResources(this.txt_first_name, "txt_first_name");
            this.txt_first_name.Name = "txt_first_name";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_last_name
            // 
            resources.ApplyResources(this.txt_last_name, "txt_last_name");
            this.txt_last_name.Name = "txt_last_name";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txt_email
            // 
            resources.ApplyResources(this.txt_email, "txt_email");
            this.txt_email.Name = "txt_email";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txt_address
            // 
            resources.ApplyResources(this.txt_address, "txt_address");
            this.txt_address.Name = "txt_address";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txt_contact_no
            // 
            resources.ApplyResources(this.txt_contact_no, "txt_contact_no");
            this.txt_contact_no.Name = "txt_contact_no";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            // txt_vatno
            // 
            resources.ApplyResources(this.txt_vatno, "txt_vatno");
            this.txt_vatno.Name = "txt_vatno";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_header_title);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Name = "panel1";
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
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
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // lbl_edit_status
            // 
            resources.ApplyResources(this.lbl_edit_status, "lbl_edit_status");
            this.lbl_edit_status.Name = "lbl_edit_status";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txt_commission_percent
            // 
            resources.ApplyResources(this.txt_commission_percent, "txt_commission_percent");
            this.txt_commission_percent.Name = "txt_commission_percent";
            this.txt_commission_percent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_commission_percent_KeyPress);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.detail);
            this.tabControl1.Controls.Add(this.commission);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // detail
            // 
            this.detail.Controls.Add(this.txt_email);
            this.detail.Controls.Add(this.txt_first_name);
            this.detail.Controls.Add(this.txt_id);
            this.detail.Controls.Add(this.txt_last_name);
            this.detail.Controls.Add(this.txt_vatno);
            this.detail.Controls.Add(this.label6);
            this.detail.Controls.Add(this.txt_address);
            this.detail.Controls.Add(this.label5);
            this.detail.Controls.Add(this.txt_contact_no);
            this.detail.Controls.Add(this.label4);
            this.detail.Controls.Add(this.txt_commission_percent);
            this.detail.Controls.Add(this.label3);
            this.detail.Controls.Add(this.label1);
            this.detail.Controls.Add(this.label7);
            this.detail.Controls.Add(this.label8);
            this.detail.Controls.Add(this.label2);
            resources.ApplyResources(this.detail, "detail");
            this.detail.Name = "detail";
            this.detail.UseVisualStyleBackColor = true;
            // 
            // commission
            // 
            this.commission.Controls.Add(this.btn_payment);
            this.commission.Controls.Add(this.btn_refresh);
            this.commission.Controls.Add(this.grid_commission);
            resources.ApplyResources(this.commission, "commission");
            this.commission.Name = "commission";
            this.commission.UseVisualStyleBackColor = true;
            // 
            // btn_payment
            // 
            resources.ApplyResources(this.btn_payment, "btn_payment");
            this.btn_payment.Name = "btn_payment";
            this.btn_payment.UseVisualStyleBackColor = true;
            this.btn_payment.Click += new System.EventHandler(this.btn_payment_Click);
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // grid_commission
            // 
            this.grid_commission.AllowUserToAddRows = false;
            this.grid_commission.AllowUserToDeleteRows = false;
            this.grid_commission.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_commission, "grid_commission");
            this.grid_commission.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_commission.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_commission.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.entry_date,
            this.invoice_no,
            this.debit,
            this.credit,
            this.balance,
            this.description});
            this.grid_commission.Name = "grid_commission";
            this.grid_commission.ReadOnly = true;
            this.grid_commission.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // entry_date
            // 
            this.entry_date.DataPropertyName = "entry_date";
            resources.ApplyResources(this.entry_date, "entry_date");
            this.entry_date.Name = "entry_date";
            this.entry_date.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // debit
            // 
            this.debit.DataPropertyName = "debit";
            resources.ApplyResources(this.debit, "debit");
            this.debit.Name = "debit";
            this.debit.ReadOnly = true;
            // 
            // credit
            // 
            this.credit.DataPropertyName = "credit";
            resources.ApplyResources(this.credit, "credit");
            this.credit.Name = "credit";
            this.credit.ReadOnly = true;
            // 
            // balance
            // 
            this.balance.DataPropertyName = "balance";
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
            // frm_addEmployee
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lbl_edit_status);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frm_addEmployee";
            this.Load += new System.EventHandler(this.frm_addEmployee_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_addEmployee_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.detail.ResumeLayout(false);
            this.detail.PerformLayout();
            this.commission.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_commission)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_first_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_last_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_address;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_contact_no;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.TextBox txt_vatno;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_edit_status;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_commission_percent;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage detail;
        private System.Windows.Forms.TabPage commission;
        private System.Windows.Forms.Button btn_payment;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.DataGridView grid_commission;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn entry_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn debit;
        private System.Windows.Forms.DataGridViewTextBoxColumn credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
    }
}