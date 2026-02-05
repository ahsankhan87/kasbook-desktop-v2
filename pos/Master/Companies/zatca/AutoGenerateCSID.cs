using pos.UI;
using pos.UI.Busy;
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
using ZATCA.EInvoice.SDK;
using ZATCA.EInvoice.SDK.Contracts;
using ZATCA.EInvoice.SDK.Contracts.Models;

namespace pos.Master.Companies.zatca
{
    public partial class AutoGenerateCSID : Form
    {
        public AutoGenerateCSID()
        {
            InitializeComponent();
            label12.Text = "Mobile : +923459079213";
            label13.Text = "Copyright ©. All rights reserved. Developed by Ahsan Khan (khybersoft.com)";
            fillcontrols();
        }

        private void AutoGenerateCSR_Load(object sender, EventArgs e)
        {
            ZATCA.EInvoice.SDK.CsrGenerator csrGenerator = new CsrGenerator();

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
                txt_organizationUnitName.Text = UsersModal.logged_in_branch_name;
                txt_countryName.Text = dr["CountryName"].ToString();
                txt_serialNumber.Text = $"1-{prefix}|2-{prefix}|3-" + Guid.NewGuid().ToString();
                txt_organizationIdentifier.Text = vatNo;
                txt_location.Text = dr["address"].ToString();
                txt_industry.Text = "Auto Parts";
            }
        }

        private async void btn_csid_Click(object sender, EventArgs e)
        {
            btn_csid.Enabled = false;
            btn_csid.Text = UiMessages.T("Generating...", "جارٍ الإنشاء...");
            btn_csid.Cursor = Cursors.WaitCursor;
            btn_csid.Refresh();

            using (BusyScope.Show(this, UiMessages.T("Generating CSID...", "جارٍ إنشاء CSID...")))
            {
                try
                {
                    await GenerateCSRAsync();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "An error occurred while generating CSID.\n" + ex.Message,
                        "حدث خطأ أثناء إنشاء CSID.\n" + ex.Message,
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                }
                finally
                {
                    btn_csid.Enabled = true;
                    btn_csid.Text = UiMessages.T("Generate CSID", "إنشاء CSID");
                    btn_csid.Cursor = Cursors.Default;
                }
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
                string organizationName = txt_organizationName.Text; // "Main Branch";
                string organizationUnitName = txt_organizationUnitName.Text; // "Riyadh Branch"; // actual 10-digit TIN
                string countryName = txt_countryName.Text; //"SA";
                string invoiceType = cmb_invoicetype.SelectedValue.ToString(); // "1100";
                string locationAddress = txt_location.Text; // "Makka";
                string industryCategory = txt_industry.Text; // "Medical Laboratories";
                                                             // Validate required fields
                if (string.IsNullOrWhiteSpace(commonName) ||
                    string.IsNullOrWhiteSpace(serialNumber) ||
                    string.IsNullOrWhiteSpace(organizationIdentifier) ||
                    string.IsNullOrWhiteSpace(organizationName) ||
                    string.IsNullOrWhiteSpace(organizationUnitName) ||
                    string.IsNullOrWhiteSpace(countryName) ||
                    string.IsNullOrWhiteSpace(invoiceType))
                {
                    UiMessages.ShowWarning(
                        "Please fill in all required fields before generating CSR.",
                        "يرجى تعبئة جميع الحقول المطلوبة قبل إنشاء CSR.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                var csrDto = new CsrGenerationDto(
                    commonName,
                    serialNumber,
                    organizationIdentifier,
                    organizationUnitName,
                    organizationName,
                    countryName,
                    invoiceType,
                    locationAddress,
                    industryCategory
                );

                bool pemFormat = false;
                EnvironmentType environment = rdb_simulation.Checked ? EnvironmentType.Simulation :
                                              rdb_production.Checked ? EnvironmentType.Production :
                                              EnvironmentType.NonProduction;

                // Generate CSR + key
                var result = csrGenerator.GenerateCsr(csrDto, environment, pemFormat);

                if (!result.IsValid)
                {
                    string message = (result.ErrorMessages != null && result.ErrorMessages.Any())
                        ? string.Join(Environment.NewLine, result.ErrorMessages)
                        : "Unknown error.";

                    UiMessages.ShowError(
                        "CSR generation failed.\n" + message,
                        "فشل إنشاء CSR.\n" + message,
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                txt_csr.Text = result.Csr;
                txt_privatekey.Text = result.PrivateKey;

                btn_csr_save.Visible = true;
                btn_privatekey_save.Visible = true;

                string otp = txt_otp.Text.Trim();
                if (string.IsNullOrWhiteSpace(otp))
                {
                    UiMessages.ShowWarning(
                        "OTP is required. Please copy it from the Fatoora portal and paste it here.",
                        "رمز التحقق (OTP) مطلوب. يرجى نسخه من بوابة فاتورة ولصقه هنا.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                // Request compliance CSID
                var authDetails = await ZatcaAuth.GetComplianceCSIDAsync(txt_csr.Text.Trim(), environment.ToString(), otp);

                if (authDetails == null ||
                    string.IsNullOrWhiteSpace(authDetails.BinarySecurityToken) ||
                    string.IsNullOrWhiteSpace(authDetails.Secret))
                {
                    UiMessages.ShowError(
                        "CSID request failed. Please verify CSR and OTP, then try again.",
                        "فشل طلب CSID. يرجى التحقق من CSR و OTP ثم المحاولة مرة أخرى.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                txt_publickey.Text = authDetails.BinarySecurityToken ?? "";
                txt_secret.Text = authDetails.Secret ?? "";
                txt_compliance_request_id.Text = authDetails.RequestID ?? "";

                btn_publickey_save.Visible = true;
                btn_secretkey_save.Visible = true;
                btn_info.Visible = true;

                UiMessages.ShowInfo(
                    "CSID generated successfully.",
                    "تم إنشاء CSID بنجاح.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "CSID generation error.\n" + ex.Message,
                    "حدث خطأ أثناء إنشاء CSID.\n" + ex.Message,
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
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
                using (BusyScope.Show(this, UiMessages.T("Saving CSR...", "جارٍ حفظ CSR...")))
                {
                    saveFileDialog1.Filter = "CSR files (*.csr)|*.csr";
                    saveFileDialog1.FileName = "csr.csr";
                    DialogResult result = saveFileDialog1.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string filename = saveFileDialog1.FileName;
                        File.WriteAllText(filename, txt_csr.Text.Trim());

                        UiMessages.ShowInfo(
                            "CSR file saved successfully.",
                            "تم حفظ ملف CSR بنجاح.",
                            captionEn: "ZATCA",
                            captionAr: "زاتكا");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Unable to save CSR file.\n" + ex.Message,
                    "تعذر حفظ ملف CSR.\n" + ex.Message,
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }

        }

        private void btn_privatekey_save_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Saving private key...", "جارٍ حفظ المفتاح الخاص...")))
                {
                    saveFileDialog1.Filter = "PEM files (*.pem)|*.pem";
                    saveFileDialog1.FileName = "private_key.pem";
                    DialogResult result = saveFileDialog1.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string filename = saveFileDialog1.FileName;
                        string keyBase64 = txt_privatekey.Text.Trim();
                        File.WriteAllText(filename, keyBase64);

                        UiMessages.ShowInfo(
                            "Private key saved successfully.",
                            "تم حفظ المفتاح الخاص بنجاح.",
                            captionEn: "ZATCA",
                            captionAr: "زاتكا");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Unable to save the private key.\n" + ex.Message,
                    "تعذر حفظ المفتاح الخاص.\n" + ex.Message,
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
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
                using (BusyScope.Show(this, UiMessages.T("Saving certificate...", "جارٍ حفظ الشهادة...")))
                {
                    saveFileDialog1.Filter = "PEM files (*.pem)|*.pem";
                    saveFileDialog1.FileName = "cert.pem";
                    DialogResult result = saveFileDialog1.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string filename = saveFileDialog1.FileName;
                        string certBase64 = txt_publickey.Text.Trim();
                        File.WriteAllText(filename, certBase64);

                        UiMessages.ShowInfo(
                            "Certificate saved successfully.",
                            "تم حفظ الشهادة بنجاح.",
                            captionEn: "ZATCA",
                            captionAr: "زاتكا");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Unable to save the certificate.\n" + ex.Message,
                    "تعذر حفظ الشهادة.\n" + ex.Message,
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }
        }

        private void btn_secretkey_save_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Saving secret key...", "جارٍ حفظ المفتاح السري...")))
                {
                    saveFileDialog1.Filter = "Secret key files (*.txt)|*.txt";
                    saveFileDialog1.FileName = "secret.txt";
                    DialogResult result = saveFileDialog1.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string filename = saveFileDialog1.FileName;
                        string secretkey = txt_secret.Text;
                        File.WriteAllText(filename, secretkey);

                        UiMessages.ShowInfo(
                            "Secret key saved successfully.",
                            "تم حفظ المفتاح السري بنجاح.",
                            captionEn: "ZATCA",
                            captionAr: "زاتكا");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Unable to save the secret key.\n" + ex.Message,
                    "تعذر حفظ المفتاح السري.\n" + ex.Message,
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
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
                try
                {
                    string pemText = txt_publickey.Text.Trim();

                    string base64 = pemText
                        .Replace("-----BEGIN CERTIFICATE-----", "")
                        .Replace("-----END CERTIFICATE-----", "")
                        .Replace("\r", "")
                        .Replace("\n", "")
                        .Trim();

                    byte[] certBytes = Convert.FromBase64String(base64);

                    var certificate = new X509Certificate2(certBytes);

                    string info = "";
                    info = UiMessages.T("Subject: ", "الموضوع: ") + certificate.Subject + "\n";
                    info += UiMessages.T("Issuer: ", "المصدر: ") + certificate.Issuer + "\n";
                    info += UiMessages.T("Valid From: ", "صالح من: ") + certificate.NotBefore + "\n";
                    info += UiMessages.T("Valid To: ", "صالح حتى: ") + certificate.NotAfter + "\n";

                    MessageBox.Show(info, UiMessages.T("Certificate Information", "معلومات الشهادة"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Unable to read certificate information.\n" + ex.Message,
                        "تعذر قراءة معلومات الشهادة.\n" + ex.Message,
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                }
            }
        }

        private void BtnGenerateCSR_Click(object sender, EventArgs e)
        {
            // fire-and-forget UI event
            _ = GenerateCSRAsync();
        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Saving credentials...", "جارٍ حفظ بيانات الاعتماد...")))
            {
                try
                {
                    EnvironmentType mode = rdb_simulation.Checked ? EnvironmentType.Simulation :
                                                rdb_production.Checked ? EnvironmentType.Production :
                                                EnvironmentType.NonProduction;

                    int result = ZatcaInvoiceGenerator.UpsertZatcaCredentials(
                        "CSID",
                        mode.ToString(),
                        txt_publickey.Text.Trim(),
                        txt_privatekey.Text.Trim(),
                        txt_secret.Text.Trim(),
                        txt_csr.Text.Trim(),
                        txt_otp.Text,
                        txt_compliance_request_id.Text);

                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Credentials saved successfully.",
                            "تم حفظ بيانات الاعتماد بنجاح.",
                            captionEn: "ZATCA Credentials",
                            captionAr: "بيانات اعتماد زاتكا");
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "No changes were saved. Please verify the entered details.",
                            "لم يتم حفظ أي تغييرات. يرجى التحقق من البيانات المدخلة.",
                            captionEn: "ZATCA Credentials",
                            captionAr: "بيانات اعتماد زاتكا");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Error while saving credentials.\n" + ex.Message,
                        "حدث خطأ أثناء حفظ بيانات الاعتماد.\n" + ex.Message,
                        captionEn: "ZATCA Credentials",
                        captionAr: "بيانات اعتماد زاتكا");
                }

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
