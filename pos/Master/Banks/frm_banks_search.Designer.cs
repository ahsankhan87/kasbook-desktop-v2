
namespace pos.Master.Banks
{
    partial class frm_banks_search
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_search_banks = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GLAccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bankBranch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_banks)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_search_banks);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 69);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1001, 408);
            this.panel2.TabIndex = 5;
            // 
            // grid_search_banks
            // 
            this.grid_search_banks.AllowUserToAddRows = false;
            this.grid_search_banks.AllowUserToDeleteRows = false;
            this.grid_search_banks.AllowUserToOrderColumns = true;
            this.grid_search_banks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_search_banks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_search_banks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.GLAccountID,
            this.code,
            this.name,
            this.accountNo,
            this.holderName,
            this.bankBranch,
            this.date_created});
            this.grid_search_banks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_search_banks.Location = new System.Drawing.Point(0, 0);
            this.grid_search_banks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid_search_banks.Name = "grid_search_banks";
            this.grid_search_banks.ReadOnly = true;
            this.grid_search_banks.RowHeadersWidth = 51;
            this.grid_search_banks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_search_banks.Size = new System.Drawing.Size(1001, 408);
            this.grid_search_banks.TabIndex = 1;
            this.grid_search_banks.DoubleClick += new System.EventHandler(this.grid_search_customers_DoubleClick);
            this.grid_search_banks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_search_customers_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn_cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1001, 69);
            this.panel1.TabIndex = 4;
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(11, 14);
            this.txt_search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(612, 22);
            this.txt_search.TabIndex = 6;
            this.txt_search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_search_KeyUp);
            // 
            // btn_ok
            // 
            this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_ok.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_ok.Location = new System.Drawing.Point(628, 10);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(103, 30);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "OK (Enter)";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_cancel.Location = new System.Drawing.Point(734, 10);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(103, 30);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Cancel (Esc)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
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
            // GLAccountID
            // 
            this.GLAccountID.DataPropertyName = "GLAccountID";
            this.GLAccountID.HeaderText = "GL Account ID";
            this.GLAccountID.MinimumWidth = 6;
            this.GLAccountID.Name = "GLAccountID";
            this.GLAccountID.ReadOnly = true;
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "Bank Code";
            this.code.MinimumWidth = 6;
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "Bank Name";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // accountNo
            // 
            this.accountNo.DataPropertyName = "accountNo";
            this.accountNo.HeaderText = "Account No";
            this.accountNo.MinimumWidth = 6;
            this.accountNo.Name = "accountNo";
            this.accountNo.ReadOnly = true;
            // 
            // holderName
            // 
            this.holderName.DataPropertyName = "holderName";
            this.holderName.HeaderText = "Holder Name";
            this.holderName.MinimumWidth = 6;
            this.holderName.Name = "holderName";
            this.holderName.ReadOnly = true;
            // 
            // bankBranch
            // 
            this.bankBranch.DataPropertyName = "bankBranch";
            this.bankBranch.HeaderText = "Bank Branch";
            this.bankBranch.MinimumWidth = 6;
            this.bankBranch.Name = "bankBranch";
            this.bankBranch.ReadOnly = true;
            // 
            // date_created
            // 
            this.date_created.DataPropertyName = "date_created";
            this.date_created.HeaderText = "Date Created";
            this.date_created.MinimumWidth = 6;
            this.date_created.Name = "date_created";
            this.date_created.ReadOnly = true;
            // 
            // frm_banks_search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(1001, 477);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frm_banks_search";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Banks ";
            this.Load += new System.EventHandler(this.frm_banks_search_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_banks_search_KeyDown);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_banks)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_search_banks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn GLAccountID;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn holderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn bankBranch;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_created;
    }
}