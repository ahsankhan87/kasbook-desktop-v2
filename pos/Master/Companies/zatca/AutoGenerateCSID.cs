using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts;
using Zatca.EInvoice.SDK.Contracts.Models;

namespace pos.Master.Companies.zatca
{
    public partial class AutoGenerateCSID : Form
    {
        //private Mode mode { get; set; }

        public AutoGenerateCSID()
        {
            InitializeComponent();
            label12.Text = "Mobile : +923459079213";
            label13.Text = "Copyright ©. All rights reserved. Developed by Ahsan Khan (khybersoft.com)";
            fillcontrols();
        }

        private void AutoGenerateCSR_Load(object sender, EventArgs e)
        {
            
            Zatca.EInvoice.SDK.CsrGenerator csrGenerator = new CsrGenerator();
            //csrGenerator.GenerateCsr();

            FillInvoiceTypes();
            btn_csr_save.Visible = false;
            btn_privatekey_save.Visible = false;
            btn_secretkey_save.Visible = false;
            btn_publickey_save.Visible = false;
            btn_info.Visible = false;
        }


        private void fillcontrols()
        {
            GeneralBLL objBLL = new GeneralBLL();

            string keyword = "TOP 1 *";
            string table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {       
                string vatNo = dr["vat_no"].ToString();
                string prefix = rdb_simulation.Checked ? "TST" : rdb_production.Checked ? "PRD" : "TST";
                string commonName = $"{prefix}-{vatNo}";
                txt_commonName.Text = commonName;
                txt_organizationName.Text = dr["name"].ToString();
                txt_organizationUnitName.Text = UsersModal.logged_in_branch_name; // "Riyadh Branch";
                txt_countryName.Text = dr["CountryName"].ToString(); 
                txt_serialNumber.Text = $"1-{prefix}|2-{prefix}|3-" + Guid.NewGuid().ToString();
                txt_organizationIdentifier.Text = vatNo;
                txt_location.Text = dr["address"].ToString();
                txt_industry.Text = "Auto Parts";
            }
        }
        //private void btn_csid_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        btn_csid.Enabled = false;
        //        btn_csid.Text = "Generating...";
        //        btn_csid.Cursor = Cursors.WaitCursor;
        //        btn_csid.Refresh();
        //        Task.Run(() => GenerateCSRAsync()).Wait();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        btn_csid.Enabled = true;
        //        btn_csid.Text = "Generate CSID انشاء المفتاح العام";
        //        btn_csid.Cursor = Cursors.Default;
        //    }

        //}
        private async void btn_csid_Click(object sender, EventArgs e)
        {
            try
            {
                btn_csid.Enabled = false;
                btn_csid.Text = "Generating...";
                btn_csid.Cursor = Cursors.WaitCursor;
                btn_csid.Refresh();
                await GenerateCSRAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_csid.Enabled = true;
                btn_csid.Text = "Generate CSID انشاء المفتاح العام";
                btn_csid.Cursor = Cursors.Default;
            }
        }
        private async Task GenerateCSRAsync()
        {
            try
            {
                var csrGenerator = new CsrGenerator();

                // Example values; replace with your actual data
                string commonName = txt_commonName.Text; // "TST-2050012095-300589284900003";
                string serialNumber = txt_serialNumber.Text; // "1-TST|2-TST|3-" + Guid.NewGuid().ToString();
                string organizationIdentifier = txt_organizationIdentifier.Text; // "300589284900003"; // ✅ valid
                string organizationUnit = txt_organizationName.Text; // "Main Branch";
                string organizationUnitName = txt_organizationUnitName.Text; // "Riyadh Branch"; // actual 10-digit TIN
                string countryName = txt_countryName.Text; //"SA";
                string invoiceType = cmb_invoicetype.SelectedValue.ToString(); // "1100";
                string locationAddress = txt_location.Text; // "Makka";
                string industryCategory = txt_industry.Text; // "Medical Laboratories";
                                                             // Validate required fields
                if (string.IsNullOrWhiteSpace(commonName) ||
                    string.IsNullOrWhiteSpace(serialNumber) ||
                    string.IsNullOrWhiteSpace(organizationIdentifier) ||
                    string.IsNullOrWhiteSpace(organizationUnit) ||
                    string.IsNullOrWhiteSpace(organizationUnitName) ||
                    string.IsNullOrWhiteSpace(countryName))
                {
                    MessageBox.Show("Please fill all required fields before generating CSR.");
                    return;
                }

                // Instantiate DTO with all parameters (no public setters)
                var csrDto = new CsrGenerationDto(
                    commonName,
                    serialNumber,
                    organizationIdentifier,
                    organizationUnit,
                    organizationUnitName,
                    countryName,
                    invoiceType,
                    locationAddress,
                    industryCategory
                );

                bool pemFormat = false; // set to false if you want base64 only
                EnvironmentType environment = rdb_simulation.Checked ? EnvironmentType.Simulation :
                                              rdb_production.Checked ? EnvironmentType.Production :
                                              EnvironmentType.NonProduction;

                var result = csrGenerator.GenerateCsr(csrDto, environment, pemFormat);

                if (result.IsValid)
                {
                    txt_csr.Text = result.Csr;
                    txt_privatekey.Text = result.PrivateKey;

                    btn_csr_save.Visible = true;
                    btn_privatekey_save.Visible = true;
                    string otp = txt_otp.Text.Trim(); // from Fatoora Portal
                    if (string.IsNullOrWhiteSpace(otp))
                    {
                        MessageBox.Show("OTP is required from Fatoora Portal for CSID request.");
                        return;
                    }

                    var authDetails = await ZatcaAuth.GetComplianceCSIDAsync(txt_csr.Text.Trim(), environment.ToString(), otp);
                    // Check if CSID was successfully retrieved
                    if (authDetails == null || string.IsNullOrWhiteSpace(authDetails.BinarySecurityToken) || string.IsNullOrWhiteSpace(authDetails.Secret))
                    {
                        // If csid is null or does not contain required fields, show error
                        MessageBox.Show("Failed to retrieve CSID. Please check your CSR and OTP.");
                        return;
                    }
                    string binarySecurityToken = authDetails.BinarySecurityToken;
                    string secret = authDetails.Secret;
                    string requestID = authDetails.RequestID;
                    txt_publickey.Text = binarySecurityToken ?? "";
                    txt_secret.Text = secret ?? "";
                    txt_compliance_request_id.Text = requestID ?? "";

                    string authorizationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authDetails.BinarySecurityToken}:{authDetails.Secret}"));

                    //var ProductionCSIDResponse = await ZatcaAuth.GetProductionCSIDAsync(authDetails.RequestID, authorizationToken, environment.ToString());
                    //string binarySecurityToken1 = ProductionCSIDResponse.BinarySecurityToken;
                    //string secret1 = ProductionCSIDResponse.Secret;
                    //string requestID1 = ProductionCSIDResponse.RequestID;

                    // If csid is not null, assign values to textboxes 
                    // and make buttons visible
                    //txt_publickey.Text = binarySecurityToken1 ?? "";
                    //txt_secret.Text = secret1 ?? "";
                    //txt_compliance_request_id.Text = requestID1 ?? "";

                    btn_publickey_save.Visible = true;
                    btn_secretkey_save.Visible = true;
                    btn_info.Visible = true;
                    MessageBox.Show("CSID generated successfully.");

                }
                else
                {
                    string message = string.Join(Environment.NewLine, result.ErrorMessages);
                    MessageBox.Show("Failed to generate CSID. " + Environment.NewLine + message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("CSID Error: \n" + ex.Message);
            }
            
        }

        private void FillInvoiceTypes()
        {
            Dictionary<string, string> types = new Dictionary<string, string>()
            {
                {"Standard & Simplified Invoices ** فاتورة ضريبية & مبسطة ","1100" },
                {"Standard Invoices Only ** فاتورة ضريبية فقط ","1000" },
                {"Simplified Invoices Only ** فاتورة مبسطة فقط ","0100" }
            };

            cmb_invoicetype.DataSource = new BindingSource(types, null);
            cmb_invoicetype.DisplayMember = "Key";
            cmb_invoicetype.ValueMember = "Value";
        }
        private void btn_csr_save_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "csr files (*.csr)|*.csr";
                saveFileDialog1.FileName = "csr.csr";
                DialogResult result = saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    File.WriteAllText(filename, txt_csr.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }

        }

        private void btn_privatekey_save_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "PEM files (*.pem)|*.pem";
                saveFileDialog1.FileName = "private_key.pem";
                DialogResult result = saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;

                    string keyBase64 = txt_privatekey.Text.Trim();
                    //string pem = "-----BEGIN EC PRIVATE KEY-----\n" +
                    //             BreakLines(keyBase64) +
                    //             "\n-----END EC PRIVATE KEY-----";

                    File.WriteAllText(filename, keyBase64);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving private key: " + ex.Message);
            }
        }


       
        private void btn_refresh1_Click(object sender, EventArgs e)
        {
            RefreshSerialNumber();
        }

        private void btn_refresh2_Click(object sender, EventArgs e)
        {
            RefreshSerialNumber();
        }
        private void RefreshSerialNumber()
        {
            string prefix = rdb_simulation.Checked ? "TST" : rdb_production.Checked ? "PRD" : "TST";
            string orgName = txt_organizationName.Text;
            txt_serialNumber.Text = $"1-{prefix}|2-{orgName}|3-{Guid.NewGuid()}";
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

                    string certBase64 = txt_publickey.Text.Trim();
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
                    string secretkey = txt_secret.Text;
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
            if (!string.IsNullOrEmpty(txt_publickey.Text))
            {
                GetCertInfo();
                
            }

        }
       
        private void GetCertInfo()
        {
            if (!string.IsNullOrEmpty(txt_publickey.Text))
            {
                string pemText = txt_publickey.Text.Trim();

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
        private void BtnGenerateCSR_Click(object sender, EventArgs e)
        {
           GenerateCSRAsync();
        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                EnvironmentType mode = rdb_simulation.Checked ? EnvironmentType.Simulation :
                                            rdb_production.Checked ? EnvironmentType.Production :
                                            EnvironmentType.NonProduction;
                // Save CSID credentials to database
                int result = ZatcaInvoiceGenerator.UpsertZatcaCredentials("CSID", mode.ToString(),
                    txt_publickey.Text.Trim(), txt_privatekey.Text.Trim(), txt_secret.Text.Trim(), txt_csr.Text.Trim(),
                    txt_otp.Text, txt_compliance_request_id.Text);
                if (result > 0)
                {
                    MessageBox.Show("Updated successfully", "Zatca Credentials", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving certificate: " + ex.Message);
            }
           
        }

        private void rdb_simulation_CheckedChanged(object sender, EventArgs e)
        {
            fillcontrols();
        }

        private void rdb_production_CheckedChanged(object sender, EventArgs e)
        {
            fillcontrols();
        }

        
    }
}
