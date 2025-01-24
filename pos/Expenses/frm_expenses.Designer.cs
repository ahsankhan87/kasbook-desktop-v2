
namespace pos.Expenses
{
    partial class frm_expenses
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
            this.grid_expenses = new System.Windows.Forms.DataGridView();
            this.account_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_delete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmb_account_code = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_cash_account = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_vat_account = new System.Windows.Forms.ComboBox();
            this.txt_sale_date = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid_expenses)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid_expenses
            // 
            this.grid_expenses.AllowUserToAddRows = false;
            this.grid_expenses.AllowUserToDeleteRows = false;
            this.grid_expenses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_expenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_expenses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.account_code,
            this.account,
            this.amount,
            this.vat,
            this.description,
            this.total,
            this.btn_delete});
            this.grid_expenses.Location = new System.Drawing.Point(-1, 153);
            this.grid_expenses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid_expenses.Name = "grid_expenses";
            this.grid_expenses.RowHeadersWidth = 51;
            this.grid_expenses.Size = new System.Drawing.Size(1083, 648);
            this.grid_expenses.TabIndex = 0;
            this.grid_expenses.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_expenses_CellContentClick);
            this.grid_expenses.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_expenses_CellEndEdit);
            this.grid_expenses.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_expenses_KeyDown);
            // 
            // account_code
            // 
            this.account_code.DataPropertyName = "account_code";
            this.account_code.HeaderText = "account_code";
            this.account_code.MinimumWidth = 6;
            this.account_code.Name = "account_code";
            this.account_code.Visible = false;
            this.account_code.Width = 125;
            // 
            // account
            // 
            this.account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.account.DataPropertyName = "account";
            this.account.HeaderText = "Account";
            this.account.MinimumWidth = 6;
            this.account.Name = "account";
            // 
            // amount
            // 
            this.amount.HeaderText = "Amount";
            this.amount.MinimumWidth = 6;
            this.amount.Name = "amount";
            this.amount.Width = 125;
            // 
            // vat
            // 
            this.vat.HeaderText = "VAT";
            this.vat.MinimumWidth = 6;
            this.vat.Name = "vat";
            this.vat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.vat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.vat.Width = 125;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.MinimumWidth = 6;
            this.description.Name = "description";
            this.description.Width = 125;
            // 
            // total
            // 
            this.total.HeaderText = "Total";
            this.total.MinimumWidth = 6;
            this.total.Name = "total";
            this.total.Width = 125;
            // 
            // btn_delete
            // 
            this.btn_delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btn_delete.HeaderText = "Delete";
            this.btn_delete.MinimumWidth = 6;
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btn_delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btn_delete.Width = 78;
            // 
            // cmb_account_code
            // 
            this.cmb_account_code.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_account_code.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_account_code.FormattingEnabled = true;
            this.cmb_account_code.Location = new System.Drawing.Point(138, 22);
            this.cmb_account_code.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_account_code.Name = "cmb_account_code";
            this.cmb_account_code.Size = new System.Drawing.Size(304, 24);
            this.cmb_account_code.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(7, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Select Account";
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(342, 56);
            this.btn_add.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(100, 47);
            this.btn_add.TabIndex = 14;
            this.btn_add.Text = "Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Location = new System.Drawing.Point(965, 80);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(100, 28);
            this.btn_close.TabIndex = 15;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(965, 15);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(100, 58);
            this.btn_save.TabIndex = 15;
            this.btn_save.Text = "Save (F3)";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(10, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Select Cash Account";
            // 
            // cmb_cash_account
            // 
            this.cmb_cash_account.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_cash_account.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_cash_account.FormattingEnabled = true;
            this.cmb_cash_account.Location = new System.Drawing.Point(152, 48);
            this.cmb_cash_account.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_cash_account.Name = "cmb_cash_account";
            this.cmb_cash_account.Size = new System.Drawing.Size(281, 24);
            this.cmb_cash_account.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(10, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Select VAT Account";
            // 
            // cmb_vat_account
            // 
            this.cmb_vat_account.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_vat_account.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_vat_account.FormattingEnabled = true;
            this.cmb_vat_account.Location = new System.Drawing.Point(152, 76);
            this.cmb_vat_account.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_vat_account.Name = "cmb_vat_account";
            this.cmb_vat_account.Size = new System.Drawing.Size(281, 24);
            this.cmb_vat_account.TabIndex = 12;
            // 
            // txt_sale_date
            // 
            this.txt_sale_date.CustomFormat = "";
            this.txt_sale_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_sale_date.Location = new System.Drawing.Point(152, 22);
            this.txt_sale_date.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_sale_date.Name = "txt_sale_date";
            this.txt_sale_date.Size = new System.Drawing.Size(281, 22);
            this.txt_sale_date.TabIndex = 55;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(10, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 17);
            this.label3.TabIndex = 56;
            this.label3.Text = "Date:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmb_account_code);
            this.groupBox1.Controls.Add(this.btn_add);
            this.groupBox1.Location = new System.Drawing.Point(14, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 111);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Expense";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_sale_date);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmb_vat_account);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmb_cash_account);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(504, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(454, 118);
            this.groupBox2.TabIndex = 58;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "GL Acounts";
            // 
            // frm_expenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(1081, 801);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.grid_expenses);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frm_expenses";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expenses";
            this.Load += new System.EventHandler(this.frm_expenses_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_expenses_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_expenses)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_expenses;
        private System.Windows.Forms.ComboBox cmb_account_code;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn account_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewComboBoxColumn vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewCheckBoxColumn btn_delete;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_cash_account;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmb_vat_account;
        private System.Windows.Forms.DateTimePicker txt_sale_date;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}