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
            this.lbl_title = new System.Windows.Forms.Label();
            this.btn_new = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.grid_zatca_csids = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_generatePCSID = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cert_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.csr_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cert_base64 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.activate = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_zatca_csids)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_title.Location = new System.Drawing.Point(14, 23);
            this.lbl_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(285, 25);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Manage ZATCA Credentials";
            // 
            // btn_new
            // 
            this.btn_new.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_new.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_new.Location = new System.Drawing.Point(19, 14);
            this.btn_new.Margin = new System.Windows.Forms.Padding(4);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(100, 28);
            this.btn_new.TabIndex = 1;
            this.btn_new.Text = "Add New";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel2.Controls.Add(this.lbl_title);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(862, 62);
            this.panel2.TabIndex = 3;
            // 
            // btn_refresh
            // 
            this.btn_refresh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_refresh.Location = new System.Drawing.Point(127, 14);
            this.btn_refresh.Margin = new System.Windows.Forms.Padding(4);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(100, 28);
            this.btn_refresh.TabIndex = 4;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // grid_zatca_csids
            // 
            this.grid_zatca_csids.AllowUserToAddRows = false;
            this.grid_zatca_csids.AllowUserToDeleteRows = false;
            this.grid_zatca_csids.AllowUserToOrderColumns = true;
            this.grid_zatca_csids.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_zatca_csids.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_zatca_csids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_zatca_csids.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.mode,
            this.cert_type,
            this.csr_text,
            this.cert_base64,
            this.otp,
            this.status,
            this.activate});
            this.grid_zatca_csids.Location = new System.Drawing.Point(0, 50);
            this.grid_zatca_csids.Margin = new System.Windows.Forms.Padding(4);
            this.grid_zatca_csids.Name = "grid_zatca_csids";
            this.grid_zatca_csids.ReadOnly = true;
            this.grid_zatca_csids.RowHeadersWidth = 51;
            this.grid_zatca_csids.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_zatca_csids.Size = new System.Drawing.Size(855, 401);
            this.grid_zatca_csids.TabIndex = 6;
            this.grid_zatca_csids.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_fiscal_years_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btn_generatePCSID);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.grid_zatca_csids);
            this.panel1.Controls.Add(this.btn_new);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(862, 455);
            this.panel1.TabIndex = 2;
            // 
            // btn_generatePCSID
            // 
            this.btn_generatePCSID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_generatePCSID.Location = new System.Drawing.Point(235, 14);
            this.btn_generatePCSID.Margin = new System.Windows.Forms.Padding(4);
            this.btn_generatePCSID.Name = "btn_generatePCSID";
            this.btn_generatePCSID.Size = new System.Drawing.Size(145, 28);
            this.btn_generatePCSID.TabIndex = 4;
            this.btn_generatePCSID.Text = "Generate PCSID";
            this.btn_generatePCSID.UseVisualStyleBackColor = true;
            this.btn_generatePCSID.Click += new System.EventHandler(this.btn_generatePCSID_Click);
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
            // mode
            // 
            this.mode.DataPropertyName = "mode";
            this.mode.HeaderText = "Mode";
            this.mode.MinimumWidth = 6;
            this.mode.Name = "mode";
            this.mode.ReadOnly = true;
            // 
            // cert_type
            // 
            this.cert_type.DataPropertyName = "cert_type";
            this.cert_type.HeaderText = "Type";
            this.cert_type.MinimumWidth = 6;
            this.cert_type.Name = "cert_type";
            this.cert_type.ReadOnly = true;
            // 
            // csr_text
            // 
            this.csr_text.DataPropertyName = "csr_text";
            this.csr_text.HeaderText = "CSR";
            this.csr_text.MinimumWidth = 6;
            this.csr_text.Name = "csr_text";
            this.csr_text.ReadOnly = true;
            // 
            // cert_base64
            // 
            this.cert_base64.DataPropertyName = "cert_base64";
            this.cert_base64.HeaderText = "Cert base64";
            this.cert_base64.MinimumWidth = 6;
            this.cert_base64.Name = "cert_base64";
            this.cert_base64.ReadOnly = true;
            // 
            // otp
            // 
            this.otp.DataPropertyName = "otp";
            this.otp.HeaderText = "OTP";
            this.otp.MinimumWidth = 6;
            this.otp.Name = "otp";
            this.otp.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "Status";
            this.status.MinimumWidth = 6;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // activate
            // 
            this.activate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.activate.HeaderText = "Activate";
            this.activate.MinimumWidth = 6;
            this.activate.Name = "activate";
            this.activate.ReadOnly = true;
            this.activate.Width = 61;
            // 
            // ShowZatcaCSID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 517);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ShowZatcaCSID";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowZatcaCSIDs";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn cert_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn csr_text;
        private System.Windows.Forms.DataGridViewTextBoxColumn cert_base64;
        private System.Windows.Forms.DataGridViewTextBoxColumn otp;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status;
        private System.Windows.Forms.DataGridViewImageColumn activate;
    }
}