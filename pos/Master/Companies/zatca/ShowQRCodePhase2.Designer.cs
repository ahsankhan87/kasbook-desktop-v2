
namespace pos.Master.Companies.zatca
{
    partial class ShowQRCodePhase2
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
            this.BtnClose = new System.Windows.Forms.Button();
            this.pictureBoxQRCode = new System.Windows.Forms.PictureBox();
            this.BtnDownload = new System.Windows.Forms.Button();
            this.LabelInvoiceNo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnClose.Location = new System.Drawing.Point(245, 317);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(66, 27);
            this.BtnClose.TabIndex = 1;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // pictureBoxQRCode
            // 
            this.pictureBoxQRCode.Location = new System.Drawing.Point(27, 53);
            this.pictureBoxQRCode.Name = "pictureBoxQRCode";
            this.pictureBoxQRCode.Size = new System.Drawing.Size(284, 258);
            this.pictureBoxQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxQRCode.TabIndex = 3;
            this.pictureBoxQRCode.TabStop = false;
            // 
            // BtnDownload
            // 
            this.BtnDownload.Location = new System.Drawing.Point(154, 317);
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(86, 27);
            this.BtnDownload.TabIndex = 1;
            this.BtnDownload.Text = "Download";
            this.BtnDownload.UseVisualStyleBackColor = true;
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // LabelInvoiceNo
            // 
            this.LabelInvoiceNo.AutoSize = true;
            this.LabelInvoiceNo.Location = new System.Drawing.Point(24, 23);
            this.LabelInvoiceNo.Name = "LabelInvoiceNo";
            this.LabelInvoiceNo.Size = new System.Drawing.Size(42, 17);
            this.LabelInvoiceNo.TabIndex = 4;
            this.LabelInvoiceNo.Text = "label1";
            // 
            // ShowQRCodePhase2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnClose;
            this.ClientSize = new System.Drawing.Size(344, 356);
            this.Controls.Add(this.LabelInvoiceNo);
            this.Controls.Add(this.pictureBoxQRCode);
            this.Controls.Add(this.BtnDownload);
            this.Controls.Add(this.BtnClose);
            this.Name = "ShowQRCodePhase2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show QRCode Phase2";
            this.Load += new System.EventHandler(this.ShowQRCodePhase2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.PictureBox pictureBoxQRCode;
        private System.Windows.Forms.Button BtnDownload;
        private System.Windows.Forms.Label LabelInvoiceNo;
    }
}