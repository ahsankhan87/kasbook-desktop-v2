namespace pos.Master.Companies.zatca
{
    partial class GeneratePCSID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneratePCSID));
            this.btn_publickey_save = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.txt_publickey = new System.Windows.Forms.TextBox();
            this.btn_secretkey_save = new System.Windows.Forms.Button();
            this.txt_compliance_request_id = new System.Windows.Forms.TextBox();
            this.txt_secret = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.Btn_save = new System.Windows.Forms.Button();
            this.btn_info = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_production_secretkey = new System.Windows.Forms.TextBox();
            this.txt_production_publickey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_generate_pcsid = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lbl_mode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_publickey_save
            // 
            this.btn_publickey_save.Location = new System.Drawing.Point(729, 486);
            this.btn_publickey_save.Name = "btn_publickey_save";
            this.btn_publickey_save.Size = new System.Drawing.Size(123, 27);
            this.btn_publickey_save.TabIndex = 125;
            this.btn_publickey_save.Text = "Save to File حفظ";
            this.btn_publickey_save.UseVisualStyleBackColor = true;
            this.btn_publickey_save.Visible = false;
            this.btn_publickey_save.Click += new System.EventHandler(this.btn_publickey_save_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(14, 67);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(138, 16);
            this.label25.TabIndex = 124;
            this.label25.Text = "PublicKey المفتاح العام *";
            // 
            // txt_publickey
            // 
            this.txt_publickey.Location = new System.Drawing.Point(15, 86);
            this.txt_publickey.Multiline = true;
            this.txt_publickey.Name = "txt_publickey";
            this.txt_publickey.ReadOnly = true;
            this.txt_publickey.Size = new System.Drawing.Size(669, 73);
            this.txt_publickey.TabIndex = 123;
            // 
            // btn_secretkey_save
            // 
            this.btn_secretkey_save.Location = new System.Drawing.Point(715, 537);
            this.btn_secretkey_save.Name = "btn_secretkey_save";
            this.btn_secretkey_save.Size = new System.Drawing.Size(137, 27);
            this.btn_secretkey_save.TabIndex = 122;
            this.btn_secretkey_save.Text = "Save to File حفظ";
            this.btn_secretkey_save.UseVisualStyleBackColor = true;
            this.btn_secretkey_save.Visible = false;
            this.btn_secretkey_save.Click += new System.EventHandler(this.btn_secretkey_save_Click);
            // 
            // txt_compliance_request_id
            // 
            this.txt_compliance_request_id.BackColor = System.Drawing.SystemColors.Window;
            this.txt_compliance_request_id.Location = new System.Drawing.Point(17, 235);
            this.txt_compliance_request_id.Name = "txt_compliance_request_id";
            this.txt_compliance_request_id.ReadOnly = true;
            this.txt_compliance_request_id.Size = new System.Drawing.Size(665, 22);
            this.txt_compliance_request_id.TabIndex = 120;
            // 
            // txt_secret
            // 
            this.txt_secret.BackColor = System.Drawing.SystemColors.Window;
            this.txt_secret.Location = new System.Drawing.Point(17, 185);
            this.txt_secret.Name = "txt_secret";
            this.txt_secret.ReadOnly = true;
            this.txt_secret.Size = new System.Drawing.Size(665, 22);
            this.txt_secret.TabIndex = 121;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 166);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 16);
            this.label14.TabIndex = 119;
            this.label14.Text = "Secret الرقم السرى *";
            // 
            // Btn_save
            // 
            this.Btn_save.Location = new System.Drawing.Point(263, 619);
            this.Btn_save.Name = "Btn_save";
            this.Btn_save.Size = new System.Drawing.Size(273, 47);
            this.Btn_save.TabIndex = 118;
            this.Btn_save.Text = "Save All Production Certificates  حفظ ";
            this.Btn_save.UseVisualStyleBackColor = true;
            this.Btn_save.Click += new System.EventHandler(this.Btn_save_Click);
            // 
            // btn_info
            // 
            this.btn_info.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_info.BackgroundImage")));
            this.btn_info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_info.Location = new System.Drawing.Point(694, 486);
            this.btn_info.Name = "btn_info";
            this.btn_info.Size = new System.Drawing.Size(35, 27);
            this.btn_info.TabIndex = 126;
            this.btn_info.UseVisualStyleBackColor = true;
            this.btn_info.Visible = false;
            this.btn_info.Click += new System.EventHandler(this.btn_info_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 16);
            this.label1.TabIndex = 119;
            this.label1.Text = "Compliance Request ID *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 25);
            this.label2.TabIndex = 124;
            this.label2.Text = "CSID Information";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 537);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 16);
            this.label3.TabIndex = 119;
            this.label3.Text = "Secret الرقم السرى *";
            // 
            // txt_production_secretkey
            // 
            this.txt_production_secretkey.BackColor = System.Drawing.SystemColors.Window;
            this.txt_production_secretkey.Location = new System.Drawing.Point(17, 556);
            this.txt_production_secretkey.Name = "txt_production_secretkey";
            this.txt_production_secretkey.ReadOnly = true;
            this.txt_production_secretkey.Size = new System.Drawing.Size(665, 22);
            this.txt_production_secretkey.TabIndex = 121;
            // 
            // txt_production_publickey
            // 
            this.txt_production_publickey.Location = new System.Drawing.Point(15, 457);
            this.txt_production_publickey.Multiline = true;
            this.txt_production_publickey.Name = "txt_production_publickey";
            this.txt_production_publickey.ReadOnly = true;
            this.txt_production_publickey.Size = new System.Drawing.Size(669, 73);
            this.txt_production_publickey.TabIndex = 123;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 438);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 16);
            this.label5.TabIndex = 124;
            this.label5.Text = "PublicKey المفتاح العام *";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 388);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(286, 25);
            this.label6.TabIndex = 124;
            this.label6.Text = "Production CSID Information";
            // 
            // btn_generate_pcsid
            // 
            this.btn_generate_pcsid.Location = new System.Drawing.Point(204, 290);
            this.btn_generate_pcsid.Name = "btn_generate_pcsid";
            this.btn_generate_pcsid.Size = new System.Drawing.Size(412, 44);
            this.btn_generate_pcsid.TabIndex = 127;
            this.btn_generate_pcsid.Text = "Generate Production CSID انشاء المفتاح للإنتاج";
            this.btn_generate_pcsid.UseVisualStyleBackColor = true;
            this.btn_generate_pcsid.Click += new System.EventHandler(this.btn_csid_Click);
            // 
            // lbl_mode
            // 
            this.lbl_mode.AutoSize = true;
            this.lbl_mode.Location = new System.Drawing.Point(225, 30);
            this.lbl_mode.Name = "lbl_mode";
            this.lbl_mode.Size = new System.Drawing.Size(63, 16);
            this.lbl_mode.TabIndex = 124;
            this.lbl_mode.Text = "lbl_mode";
            // 
            // GeneratePCSID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 714);
            this.Controls.Add(this.btn_generate_pcsid);
            this.Controls.Add(this.btn_info);
            this.Controls.Add(this.btn_publickey_save);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_mode);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.txt_production_publickey);
            this.Controls.Add(this.txt_publickey);
            this.Controls.Add(this.btn_secretkey_save);
            this.Controls.Add(this.txt_production_secretkey);
            this.Controls.Add(this.txt_compliance_request_id);
            this.Controls.Add(this.txt_secret);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Btn_save);
            this.Name = "GeneratePCSID";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate PCSID";
            this.Load += new System.EventHandler(this.GeneratePCSID_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_info;
        private System.Windows.Forms.Button btn_publickey_save;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txt_publickey;
        private System.Windows.Forms.Button btn_secretkey_save;
        private System.Windows.Forms.TextBox txt_compliance_request_id;
        private System.Windows.Forms.TextBox txt_secret;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button Btn_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_production_secretkey;
        private System.Windows.Forms.TextBox txt_production_publickey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_generate_pcsid;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lbl_mode;
    }
}