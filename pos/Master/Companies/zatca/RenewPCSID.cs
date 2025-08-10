using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Master.Companies.zatca
{
    public partial class RenewPCSID : Form
    {
        private int _csidId = 0;

        public RenewPCSID()
        {
            InitializeComponent();
        }
    
        public RenewPCSID(int csidId)
        {
            InitializeComponent();
            _csidId = csidId;
        }
        private void RenewPCSID_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCSID(_csidId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading PCSID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadCSID(int csidId)
        {
            // Logic to load CSID details based on csidId
            // This could involve fetching data from a database or an API
            // For example:
            // var csidDetails = GetCSIDDetails(csidId);
            // txtPCSID.Text = csidDetails.PCSID;
            // txtExpiryDate.Text = csidDetails.ExpiryDate.ToString("yyyy-MM-dd");
            try
            {
                // This method is intended to load the CSID grid data.
                DataRow dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByID(csidId);
                if (dataRow == null)
                {
                    MessageBox.Show("No CSID found for the provided ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dataRow["cert_type"].ToString() != "CSID")
                {
                    MessageBox.Show("Please select CSID to generate PCSID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dataRow["csr_text"] != DBNull.Value && dataRow["otp"] != DBNull.Value)
                {
                    txt_csr.Text = dataRow["csr_text"].ToString();
                    txt_otp.Text = dataRow["otp"].ToString();
                    lbl_mode.Text = dataRow["mode"].ToString();
                }
                else
                {
                    txt_csr.Text = string.Empty;
                    txt_otp.Text = string.Empty;
                    lbl_mode.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected async Task GeneratePCSIDAsync()
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(txt_csr.Text) || string.IsNullOrEmpty(txt_otp.Text))
                {
                    MessageBox.Show("Please fill in all fields before generating PCSID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //string authorizationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{txt_publickey.Text}:{txt_secret.Text}"));
                var ProductionCSIDResponse = await ZatcaAuth.RenewProductionCSIDAsync(txt_csr.Text, txt_otp.Text, lbl_mode.Text);
                string binarySecurityToken1 = ProductionCSIDResponse.BinarySecurityToken;
                string secret1 = ProductionCSIDResponse.Secret;
                string requestID1 = ProductionCSIDResponse.RequestID;

                if (string.IsNullOrEmpty(binarySecurityToken1) || string.IsNullOrEmpty(secret1) || string.IsNullOrEmpty(requestID1))
                {
                    MessageBox.Show("Failed to generate PCSID. Please check your inputs and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Clear previous values in the textboxes
                txt_production_publickey.Clear();
                txt_production_secretkey.Clear();
                // Assign the values from the response to the textboxes
                txt_production_publickey.Text = binarySecurityToken1;
                txt_production_secretkey.Text = secret1;


                // Make buttons visible
                btn_publickey_save.Visible = true;
                btn_secretkey_save.Visible = true;
                btn_info.Visible = true;
                MessageBox.Show("PCSID generated successfully.", "Generate PCSID", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btn_csid_Click(object sender, EventArgs e)
        {
            try
            {
                btn_generate_pcsid.Enabled = false;
                btn_generate_pcsid.Text = "Generating...";
                btn_generate_pcsid.Cursor = Cursors.WaitCursor;
                btn_generate_pcsid.Refresh();
                await GeneratePCSIDAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_generate_pcsid.Enabled = true;
                btn_generate_pcsid.Text = "Renew Production CSID انشاء المفتاح للإنتاج";
                btn_generate_pcsid.Cursor = Cursors.Default;
            }
        }

        private void btn_publickey_save_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "PEM files (*.pem)|*.pem";
                saveFileDialog1.FileName = "cert.pem";
                DialogResult result = saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;

                    string certBase64 = txt_production_publickey.Text.Trim();
                    //string pem = "-----BEGIN CERTIFICATE-----\n" +
                    //             BreakLines(certBase64) +
                    //             "\n-----END CERTIFICATE-----";

                    File.WriteAllText(filename, certBase64);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving certificate: " + ex.Message);
            }
        }


        private void btn_secretkey_save_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "secretkey files (*.txt)|*.txt";
                saveFileDialog1.FileName = "secret.txt";
                DialogResult result = saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    string secretkey = txt_production_secretkey.Text;
                    File.WriteAllText(filename, secretkey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }
        private string BreakLines(string base64, int lineLength = 64)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < base64.Length; i += lineLength)
            {
                int len = Math.Min(lineLength, base64.Length - i);
                sb.AppendLine(base64.Substring(i, len));
            }
            return sb.ToString().TrimEnd();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_production_publickey.Text))
            {
                GetCertInfo();

            }

        }

        private void GetCertInfo()
        {
            if (!string.IsNullOrEmpty(txt_production_publickey.Text))
            {
                string pemText = txt_production_publickey.Text.Trim();

                // Remove PEM headers
                string base64 = pemText
                    .Replace("-----BEGIN CERTIFICATE-----", "")
                    .Replace("-----END CERTIFICATE-----", "")
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();

                // Decode Base64
                byte[] certBytes = Convert.FromBase64String(base64);

                // Save to a .cer or .der file
                //File.WriteAllBytes(pemText, certBytes);

                // Optional: Load it into an X509Certificate2 object
                var certificate = new X509Certificate2(certBytes);

                // Print info
                string info = "";
                info = "Subject :" + certificate.Subject + "\n";
                info += "Issuer :" + certificate.Issuer + "\n";
                info += "Valid From :" + certificate.NotBefore + "\n";
                info += "Valid To :" + certificate.NotAfter + "\n";
                MessageBox.Show(info);

            }
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {

                // Save CSID credentials to database
                int result = ZatcaInvoiceGenerator.UpsertZatcaPCSIDCredentials("PCSID", lbl_mode.Text,
                    txt_production_publickey.Text.Trim(), txt_production_secretkey.Text.Trim(), _csidId);
                if (result > 0)
                {
                    MessageBox.Show("PCSID generated successfully.", "Generate PCSID", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving certificate: " + ex.Message);
            }
        }
    }
}
