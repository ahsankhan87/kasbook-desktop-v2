
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_banks_search));
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_search_banks = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GLAccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bankBranch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_banks)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_search_banks);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
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
            resources.ApplyResources(this.grid_search_banks, "grid_search_banks");
            this.grid_search_banks.Name = "grid_search_banks";
            this.grid_search_banks.ReadOnly = true;
            this.grid_search_banks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_search_banks.DoubleClick += new System.EventHandler(this.grid_search_customers_DoubleClick);
            this.grid_search_banks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_search_customers_KeyDown);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // GLAccountID
            // 
            this.GLAccountID.DataPropertyName = "GLAccountID";
            resources.ApplyResources(this.GLAccountID, "GLAccountID");
            this.GLAccountID.Name = "GLAccountID";
            this.GLAccountID.ReadOnly = true;
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // accountNo
            // 
            this.accountNo.DataPropertyName = "accountNo";
            resources.ApplyResources(this.accountNo, "accountNo");
            this.accountNo.Name = "accountNo";
            this.accountNo.ReadOnly = true;
            // 
            // holderName
            // 
            this.holderName.DataPropertyName = "holderName";
            resources.ApplyResources(this.holderName, "holderName");
            this.holderName.Name = "holderName";
            this.holderName.ReadOnly = true;
            // 
            // bankBranch
            // 
            this.bankBranch.DataPropertyName = "bankBranch";
            resources.ApplyResources(this.bankBranch, "bankBranch");
            this.bankBranch.Name = "bankBranch";
            this.bankBranch.ReadOnly = true;
            // 
            // date_created
            // 
            this.date_created.DataPropertyName = "date_created";
            resources.ApplyResources(this.date_created, "date_created");
            this.date_created.Name = "date_created";
            this.date_created.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn_cancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_search_KeyUp);
            // 
            // btn_ok
            // 
            this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_ok, "btn_ok");
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_banks_search
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frm_banks_search";
            this.ShowIcon = false;
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