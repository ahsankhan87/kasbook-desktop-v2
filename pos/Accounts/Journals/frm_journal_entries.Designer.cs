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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_entry_date = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.btn_save = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn_new = new System.Windows.Forms.Button();
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1299, 85);
            this.panel1.TabIndex = 0;
            // 
            // txt_entry_date
            // 
            this.txt_entry_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_entry_date.Location = new System.Drawing.Point(109, 33);
            this.txt_entry_date.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_entry_date.Name = "txt_entry_date";
            this.txt_entry_date.Size = new System.Drawing.Size(176, 26);
            this.txt_entry_date.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Entry Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(296, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Invoice No:";
            // 
            // txt_invoice_no
            // 
            this.txt_invoice_no.Location = new System.Drawing.Point(398, 33);
            this.txt_invoice_no.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_invoice_no.Name = "txt_invoice_no";
            this.txt_invoice_no.ReadOnly = true;
            this.txt_invoice_no.Size = new System.Drawing.Size(180, 26);
            this.txt_invoice_no.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_journal);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 85);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1299, 588);
            this.panel2.TabIndex = 0;
            // 
            // grid_journal
            // 
            this.grid_journal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_journal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_journal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.account,
            this.debit_amount,
            this.credit_amount,
            this.description});
            this.grid_journal.Location = new System.Drawing.Point(0, 5);
            this.grid_journal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grid_journal.Name = "grid_journal";
            this.grid_journal.Size = new System.Drawing.Size(1299, 583);
            this.grid_journal.TabIndex = 0;
            this.grid_journal.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_journal_CellEndEdit);
            this.grid_journal.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_journal_EditingControlShowing);
            // 
            // account
            // 
            this.account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.account.HeaderText = "Particulars / Accounts";
            this.account.Name = "account";
            // 
            // debit_amount
            // 
            this.debit_amount.DataPropertyName = "debit_amount";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.debit_amount.DefaultCellStyle = dataGridViewCellStyle5;
            this.debit_amount.HeaderText = "Debit";
            this.debit_amount.Name = "debit_amount";
            // 
            // credit_amount
            // 
            this.credit_amount.DataPropertyName = "credit_amount";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.credit_amount.DefaultCellStyle = dataGridViewCellStyle6;
            this.credit_amount.HeaderText = "Credit";
            this.credit_amount.Name = "credit_amount";
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "Description";
            this.description.MinimumWidth = 10;
            this.description.Name = "description";
            this.description.Width = 300;
            // 
            // txt_cr_total
            // 
            this.txt_cr_total.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_cr_total.Location = new System.Drawing.Point(758, 5);
            this.txt_cr_total.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_cr_total.Name = "txt_cr_total";
            this.txt_cr_total.Size = new System.Drawing.Size(132, 26);
            this.txt_cr_total.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(553, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Total:";
            // 
            // txt_dr_total
            // 
            this.txt_dr_total.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_dr_total.Location = new System.Drawing.Point(623, 5);
            this.txt_dr_total.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_dr_total.Name = "txt_dr_total";
            this.txt_dr_total.Size = new System.Drawing.Size(132, 26);
            this.txt_dr_total.TabIndex = 1;
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Location = new System.Drawing.Point(1174, 18);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(112, 57);
            this.btn_save.TabIndex = 5;
            this.btn_save.Text = "Save (F3)";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txt_cr_total);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txt_dr_total);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 673);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1299, 74);
            this.panel3.TabIndex = 6;
            // 
            // btn_new
            // 
            this.btn_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_new.Location = new System.Drawing.Point(1053, 18);
            this.btn_new.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(112, 57);
            this.btn_new.TabIndex = 5;
            this.btn_new.Text = "New (F1)";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // frm_journal_entries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 747);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frm_journal_entries";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Journal Entries";
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