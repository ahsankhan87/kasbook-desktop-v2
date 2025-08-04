
namespace pos.Sales
{
    partial class ZatcaInvoiceApp
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
            this.btnSubmitInvoice = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.pbQRCode = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtPrivateKey = new System.Windows.Forms.TextBox();
            this.txtCsr = new System.Windows.Forms.TextBox();
            this.Priavateket = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSubmitInvoice
            // 
            this.btnSubmitInvoice.Location = new System.Drawing.Point(470, 254);
            this.btnSubmitInvoice.Name = "btnSubmitInvoice";
            this.btnSubmitInvoice.Size = new System.Drawing.Size(133, 48);
            this.btnSubmitInvoice.TabIndex = 0;
            this.btnSubmitInvoice.Text = "btnSubmitInvoice ";
            this.btnSubmitInvoice.UseVisualStyleBackColor = true;
            this.btnSubmitInvoice.Click += new System.EventHandler(this.btnSubmitInvoice_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(452, 12);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(336, 214);
            this.txtResult.TabIndex = 1;
            // 
            // pbQRCode
            // 
            this.pbQRCode.Location = new System.Drawing.Point(16, 12);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(420, 303);
            this.pbQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbQRCode.TabIndex = 2;
            this.pbQRCode.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(613, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(552, 561);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 48);
            this.button2.TabIndex = 0;
            this.button2.Text = "Save CSR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(413, 561);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 48);
            this.button3.TabIndex = 0;
            this.button3.Text = "Generate CSR";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtPrivateKey
            // 
            this.txtPrivateKey.Location = new System.Drawing.Point(12, 341);
            this.txtPrivateKey.Multiline = true;
            this.txtPrivateKey.Name = "txtPrivateKey";
            this.txtPrivateKey.Size = new System.Drawing.Size(260, 214);
            this.txtPrivateKey.TabIndex = 1;
            // 
            // txtCsr
            // 
            this.txtCsr.Location = new System.Drawing.Point(289, 341);
            this.txtCsr.Multiline = true;
            this.txtCsr.Name = "txtCsr";
            this.txtCsr.Size = new System.Drawing.Size(396, 214);
            this.txtCsr.TabIndex = 1;
            // 
            // Priavateket
            // 
            this.Priavateket.AutoSize = true;
            this.Priavateket.Location = new System.Drawing.Point(13, 318);
            this.Priavateket.Name = "Priavateket";
            this.Priavateket.Size = new System.Drawing.Size(78, 17);
            this.Priavateket.TabIndex = 3;
            this.Priavateket.Text = "Private key";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "CSR";
            // 
            // ZatcaInvoiceApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 633);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Priavateket);
            this.Controls.Add(this.pbQRCode);
            this.Controls.Add(this.txtCsr);
            this.Controls.Add(this.txtPrivateKey);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSubmitInvoice);
            this.Name = "ZatcaInvoiceApp";
            this.Text = "ZatcaInvoiceApp";
            this.Load += new System.EventHandler(this.ZatcaInvoiceApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmitInvoice;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.PictureBox pbQRCode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtPrivateKey;
        private System.Windows.Forms.TextBox txtCsr;
        private System.Windows.Forms.Label Priavateket;
        private System.Windows.Forms.Label label1;
    }
}