namespace pos
{
    partial class frm_journal_entries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_journal_entries));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_new = new System.Windows.Forms.Button();
            this.txt_entry_date = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_invoice_no = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_journal = new System.Windows.Forms.DataGridView();
            this.account = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.debit_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credit_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_cr_total = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_dr_total = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_journal)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_new);
            this.panel1.Controls.Add(this.txt_entry_date);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_save);
            this.panel1.Controls.Add(this.txt_invoice_no);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btn_new
            // 
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // txt_entry_date
            // 
            this.txt_entry_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.txt_entry_date, "txt_entry_date");
            this.txt_entry_date.Name = "txt_entry_date";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // txt_invoice_no
            // 
            resources.ApplyResources(this.txt_invoice_no, "txt_invoice_no");
            this.txt_invoice_no.Name = "txt_invoice_no";
            this.txt_invoice_no.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_journal);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // grid_journal
            // 
            resources.ApplyResources(this.grid_journal, "grid_journal");
            this.grid_journal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_journal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.account,
            this.debit_amount,
            this.credit_amount,
            this.description});
            this.grid_journal.Name = "grid_journal";
            this.grid_journal.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_journal_CellEndEdit);
            this.grid_journal.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_journal_EditingControlShowing);
            // 
            // account
            // 
            this.account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.account, "account");
            this.account.Name = "account";
            // 
            // debit_amount
            // 
            this.debit_amount.DataPropertyName = "debit_amount";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.debit_amount.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.debit_amount, "debit_amount");
            this.debit_amount.Name = "debit_amount";
            // 
            // credit_amount
            // 
            this.credit_amount.DataPropertyName = "credit_amount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.credit_amount.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.credit_amount, "credit_amount");
            this.credit_amount.Name = "credit_amount";
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            // 
            // txt_cr_total
            // 
            resources.ApplyResources(this.txt_cr_total, "txt_cr_total");
            this.txt_cr_total.Name = "txt_cr_total";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txt_dr_total
            // 
            resources.ApplyResources(this.txt_dr_total, "txt_dr_total");
            this.txt_dr_total.Name = "txt_dr_total";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txt_cr_total);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txt_dr_total);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // frm_journal_entries
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_journal_entries";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_journal_entries_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_journal_entries_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_journal)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker txt_entry_date;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_invoice_no;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_journal;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.TextBox txt_cr_total;
        private System.Windows.Forms.TextBox txt_dr_total;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewComboBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn debit_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn credit_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
    }
}