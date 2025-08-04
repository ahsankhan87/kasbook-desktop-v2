using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Master.Companies.zatca
{
    public partial class ShowQRCodePhase2 : Form
    {
        byte[] _qr_code = null;
        string _invoiceNo = "";

        public ShowQRCodePhase2(byte[] qr_code,string invoiceNo ="")
        {
            _qr_code = qr_code;
            _invoiceNo = invoiceNo;
            InitializeComponent();
        }
        public ShowQRCodePhase2()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowQRCodePhase2_Load(object sender, EventArgs e)
        {
            LabelInvoiceNo.Text = _invoiceNo;
            ShowQRCode(_qr_code);
        }
        public void ShowQRCode(byte[] qrBytes)
        {
            try
            {
                if (qrBytes != null)
                {
                    string base64String = Convert.ToBase64String(qrBytes);

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    pictureBoxQRCode.Image = qrCodeImage;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid QR image data: " + ex.Message);
            }
        }
        public void SaveByteArrayToImage(byte[] imageData, string defaultFilename)
        {
            string base64String = Convert.ToBase64String(imageData);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(20);

            byte[] byteImage;
            using(Bitmap bmp = qrCode.GetGraphic(5))
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteImage = ms.ToArray();
                    
                }
            }
           
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png";
                saveFileDialog.Title = "Save QR Code";
                saveFileDialog.FileName = defaultFilename;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, byteImage);
                }
            }
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                SaveByteArrayToImage(_qr_code, _invoiceNo + "_phase2_qr.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid: " + ex.Message);
            }
        }
    }
}
