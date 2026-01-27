namespace pos.Master.Companies.zatca
{
    partial class ShowZatcaCSID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowZatcaCSID));
            this.lbl_title = new System.Windows.Forms.Label();
            this.btn_new = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.grid_zatca_csids = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cert_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.csr_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cert_base64 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.secret_key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.activate = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_info = new System.Windows.Forms.Button();
            this.btn_renew_PCSID = new System.Windows.Forms.Button();
            this.btn_generatePCSID = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_zatca_csids)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            resources.ApplyResources(this.lbl_title, "lbl_title");
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Name = "lbl_title";
            // 
            // btn_new
            // 
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel2.Controls.Add(this.lbl_title);
            this.panel2.Name = "panel2";
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // grid_zatca_csids
            // 
            resources.ApplyResources(this.grid_zatca_csids, "grid_zatca_csids");
            this.grid_zatca_csids.AllowUserToAddRows = false;
            this.grid_zatca_csids.AllowUserToDeleteRows = false;
            this.grid_zatca_csids.AllowUserToOrderColumns = true;
            this.grid_zatca_csids.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_zatca_csids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_zatca_csids.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.mode,
            this.cert_type,
            this.csr_text,
            this.cert_base64,
            this.secret_key,
            this.otp,
            this.status,
            this.activate});
            this.grid_zatca_csids.Name = "grid_zatca_csids";
            this.grid_zatca_csids.ReadOnly = true;
            this.grid_zatca_csids.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_zatca_csids.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_fiscal_years_CellContentClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // mode
            // 
            this.mode.DataPropertyName = "mode";
            resources.ApplyResources(this.mode, "mode");
            this.mode.Name = "mode";
            this.mode.ReadOnly = true;
            // 
            // cert_type
            // 
            this.cert_type.DataPropertyName = "cert_type";
            resources.ApplyResources(this.cert_type, "cert_type");
            this.cert_type.Name = "cert_type";
            this.cert_type.ReadOnly = true;
            // 
            // csr_text
            // 
            this.csr_text.DataPropertyName = "csr_text";
            resources.ApplyResources(this.csr_text, "csr_text");
            this.csr_text.Name = "csr_text";
            this.csr_text.ReadOnly = true;
            // 
            // cert_base64
            // 
            this.cert_base64.DataPropertyName = "cert_base64";
            resources.ApplyResources(this.cert_base64, "cert_base64");
            this.cert_base64.Name = "cert_base64";
            this.cert_base64.ReadOnly = true;
            // 
            // secret_key
            // 
            this.secret_key.DataPropertyName = "secret_key";
            resources.ApplyResources(this.secret_key, "secret_key");
            this.secret_key.Name = "secret_key";
            this.secret_key.ReadOnly = true;
            // 
            // otp
            // 
            this.otp.DataPropertyName = "otp";
            resources.ApplyResources(this.otp, "otp");
            this.otp.Name = "otp";
            this.otp.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            resources.ApplyResources(this.status, "status");
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // activate
            // 
            this.activate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.activate, "activate");
            this.activate.Name = "activate";
            this.activate.ReadOnly = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btn_info);
            this.panel1.Controls.Add(this.btn_renew_PCSID);
            this.panel1.Controls.Add(this.btn_generatePCSID);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.grid_zatca_csids);
            this.panel1.Controls.Add(this.btn_new);
            this.panel1.Name = "panel1";
            // 
            // btn_info
            // 
            resources.ApplyResources(this.btn_info, "btn_info");
            this.btn_info.Name = "btn_info";
            this.btn_info.UseVisualStyleBackColor = true;
            this.btn_info.Click += new System.EventHandler(this.btn_info_Click);
            // 
            // btn_renew_PCSID
            // 
            resources.ApplyResources(this.btn_renew_PCSID, "btn_renew_PCSID");
            this.btn_renew_PCSID.Name = "btn_renew_PCSID";
            this.btn_renew_PCSID.UseVisualStyleBackColor = true;
            this.btn_renew_PCSID.Click += new System.EventHandler(this.btn_renew_PCSID_Click);
            // 
            // btn_generatePCSID
            // 
            resources.ApplyResources(this.btn_generatePCSID, "btn_generatePCSID");
            this.btn_generatePCSID.Name = "btn_generatePCSID";
            this.btn_generatePCSID.UseVisualStyleBackColor = true;
            this.btn_generatePCSID.Click += new System.EventHandler(this.btn_generatePCSID_Click);
            // 
            // ShowZatcaCSID
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ShowZatcaCSID";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.ShowZatcaCSID_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_zatca_csids)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.DataGridView grid_zatca_csids;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_generatePCSID;
        private System.Windows.Forms.Button btn_renew_PCSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn cert_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn csr_text;
        private System.Windows.Forms.DataGridViewTextBoxColumn cert_base64;
        private System.Windows.Forms.DataGridViewTextBoxColumn secret_key;
        private System.Windows.Forms.DataGridViewTextBoxColumn otp;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status;
        private System.Windows.Forms.DataGridViewImageColumn activate;
        private System.Windows.Forms.Button btn_info;
    }
}