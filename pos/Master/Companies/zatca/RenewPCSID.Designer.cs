namespace pos.Master.Companies.zatca
{
    partial class RenewPCSID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenewPCSID));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn_generate_pcsid = new System.Windows.Forms.Button();
            this.btn_publickey_save = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_mode = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.txt_production_publickey = new System.Windows.Forms.TextBox();
            this.txt_csr = new System.Windows.Forms.TextBox();
            this.btn_secretkey_save = new System.Windows.Forms.Button();
            this.txt_production_secretkey = new System.Windows.Forms.TextBox();
            this.txt_compliance_request_id = new System.Windows.Forms.TextBox();
            this.txt_otp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.Btn_save = new System.Windows.Forms.Button();
            this.btn_info = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_generate_pcsid
            // 
            this.btn_generate_pcsid.Location = new System.Drawing.Point(215, 309);
            this.btn_generate_pcsid.Name = "btn_generate_pcsid";
            this.btn_generate_pcsid.Size = new System.Drawing.Size(412, 44);
            this.btn_generate_pcsid.TabIndex = 145;
            this.btn_generate_pcsid.Text = "Generate Production CSID انشاء المفتاح للإنتاج";
            this.btn_generate_pcsid.UseVisualStyleBackColor = true;
            // 
            // btn_publickey_save
            // 
            this.btn_publickey_save.Location = new System.Drawing.Point(740, 505);
            this.btn_publickey_save.Name = "btn_publickey_save";
            this.btn_publickey_save.Size = new System.Drawing.Size(123, 27);
            this.btn_publickey_save.TabIndex = 143;
            this.btn_publickey_save.Text = "Save to File حفظ";
            this.btn_publickey_save.UseVisualStyleBackColor = true;
            this.btn_publickey_save.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 407);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(286, 25);
            this.label6.TabIndex = 142;
            this.label6.Text = "Production CSID Information";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 25);
            this.label2.TabIndex = 141;
            this.label2.Text = "CSID Information";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 457);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 16);
            this.label5.TabIndex = 140;
            this.label5.Text = "PublicKey المفتاح العام *";
            // 
            // lbl_mode
            // 
            this.lbl_mode.AutoSize = true;
            this.lbl_mode.Location = new System.Drawing.Point(236, 49);
            this.lbl_mode.Name = "lbl_mode";
            this.lbl_mode.Size = new System.Drawing.Size(63, 16);
            this.lbl_mode.TabIndex = 139;
            this.lbl_mode.Text = "lbl_mode";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(25, 86);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(138, 16);
            this.label25.TabIndex = 138;
            this.label25.Text = "PublicKey المفتاح العام *";
            // 
            // txt_production_publickey
            // 
            this.txt_production_publickey.Location = new System.Drawing.Point(26, 476);
            this.txt_production_publickey.Multiline = true;
            this.txt_production_publickey.Name = "txt_production_publickey";
            this.txt_production_publickey.ReadOnly = true;
            this.txt_production_publickey.Size = new System.Drawing.Size(669, 73);
            this.txt_production_publickey.TabIndex = 137;
            // 
            // txt_csr
            // 
            this.txt_csr.Location = new System.Drawing.Point(26, 105);
            this.txt_csr.Multiline = true;
            this.txt_csr.Name = "txt_csr";
            this.txt_csr.ReadOnly = true;
            this.txt_csr.Size = new System.Drawing.Size(669, 73);
            this.txt_csr.TabIndex = 136;
            // 
            // btn_secretkey_save
            // 
            this.btn_secretkey_save.Location = new System.Drawing.Point(726, 556);
            this.btn_secretkey_save.Name = "btn_secretkey_save";
            this.btn_secretkey_save.Size = new System.Drawing.Size(137, 27);
            this.btn_secretkey_save.TabIndex = 135;
            this.btn_secretkey_save.Text = "Save to File حفظ";
            this.btn_secretkey_save.UseVisualStyleBackColor = true;
            this.btn_secretkey_save.Visible = false;
            // 
            // txt_production_secretkey
            // 
            this.txt_production_secretkey.BackColor = System.Drawing.SystemColors.Window;
            this.txt_production_secretkey.Location = new System.Drawing.Point(28, 575);
            this.txt_production_secretkey.Name = "txt_production_secretkey";
            this.txt_production_secretkey.ReadOnly = true;
            this.txt_production_secretkey.Size = new System.Drawing.Size(665, 22);
            this.txt_production_secretkey.TabIndex = 134;
            // 
            // txt_compliance_request_id
            // 
            this.txt_compliance_request_id.BackColor = System.Drawing.SystemColors.Window;
            this.txt_compliance_request_id.Location = new System.Drawing.Point(28, 254);
            this.txt_compliance_request_id.Name = "txt_compliance_request_id";
            this.txt_compliance_request_id.ReadOnly = true;
            this.txt_compliance_request_id.Size = new System.Drawing.Size(665, 22);
            this.txt_compliance_request_id.TabIndex = 132;
            // 
            // txt_otp
            // 
            this.txt_otp.BackColor = System.Drawing.SystemColors.Window;
            this.txt_otp.Location = new System.Drawing.Point(28, 204);
            this.txt_otp.Name = "txt_otp";
            this.txt_otp.ReadOnly = true;
            this.txt_otp.Size = new System.Drawing.Size(665, 22);
            this.txt_otp.TabIndex = 133;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 556);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 16);
            this.label3.TabIndex = 130;
            this.label3.Text = "Secret الرقم السرى *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 16);
            this.label1.TabIndex = 129;
            this.label1.Text = "Compliance Request ID *";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 185);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 16);
            this.label14.TabIndex = 131;
            this.label14.Text = "Secret الرقم السرى *";
            // 
            // Btn_save
            // 
            this.Btn_save.Location = new System.Drawing.Point(274, 638);
            this.Btn_save.Name = "Btn_save";
            this.Btn_save.Size = new System.Drawing.Size(273, 47);
            this.Btn_save.TabIndex = 128;
            this.Btn_save.Text = "Save All Production Certificates  حفظ ";
            this.Btn_save.UseVisualStyleBackColor = true;
            // 
            // btn_info
            // 
            this.btn_info.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_info.BackgroundImage")));
            this.btn_info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_info.Location = new System.Drawing.Point(705, 505);
            this.btn_info.Name = "btn_info";
            this.btn_info.Size = new System.Drawing.Size(35, 27);
            this.btn_info.TabIndex = 144;
            this.btn_info.UseVisualStyleBackColor = true;
            this.btn_info.Visible = false;
            // 
            // RenewPCSID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 711);
            this.Controls.Add(this.btn_generate_pcsid);
            this.Controls.Add(this.btn_info);
            this.Controls.Add(this.btn_publickey_save);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_mode);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.txt_production_publickey);
            this.Controls.Add(this.txt_csr);
            this.Controls.Add(this.btn_secretkey_save);
            this.Controls.Add(this.txt_production_secretkey);
            this.Controls.Add(this.txt_compliance_request_id);
            this.Controls.Add(this.txt_otp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Btn_save);
            this.Name = "RenewPCSID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renew PCSID";
            this.Load += new System.EventHandler(this.RenewPCSID_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btn_generate_pcsid;
        private System.Windows.Forms.Button btn_info;
        private System.Windows.Forms.Button btn_publickey_save;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_mode;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txt_production_publickey;
        private System.Windows.Forms.TextBox txt_csr;
        private System.Windows.Forms.Button btn_secretkey_save;
        private System.Windows.Forms.TextBox txt_production_secretkey;
        private System.Windows.Forms.TextBox txt_compliance_request_id;
        private System.Windows.Forms.TextBox txt_otp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button Btn_save;
    }
}