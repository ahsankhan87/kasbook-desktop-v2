using Newtonsoft.Json;
using pos.Master.Companies.zatca;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Zatca.EInvoice.SDK;

namespace pos.Sales
{
    public partial class frm_zatca_invoices : Form
    {
        private string _env = "NonProduction"; // Default to NonProduction, can be changed based on active credentials

        public frm_zatca_invoices()
        {
            InitializeComponent();
            
        }

        private void frm_zatca_invoices_Load(object sender, EventArgs e)
        {
            DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCredential();
           
            _env = activeZatcaCredential["mode"].ToString();
            LoadZatcaInvoices();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadZatcaInvoices();
        }

        private void LoadZatcaInvoices()
        {
            // Example: get all invoices with ZATCA status from your DB
            gridZatcaInvoices.AutoGenerateColumns = false;
            DataTable dt = new SalesBLL().GetAllSales(); // Implement this method in your BLL
            gridZatcaInvoices.DataSource = dt;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
           
            await SendOrReportToZatca(invoiceNo, _env, isReport: false);
        }

        private async void btnReport_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();

            await SendOrReportToZatca(invoiceNo, _env, isReport: true);
        }
        public async Task ZatcaInvoiceClearanceAsync(string invoiceNo)
        {
            try
            {
                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(_env))
                {
                    MessageBox.Show("No active ZATCA credentials found. Please configure them first.");
                    return;
                }

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                string cert_CSID = ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, _env);
                string secret_CSID = ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, _env);
                string complainceRequestID = ZatcaInvoiceGenerator.GetComplainceRequestIDFromDb(UsersModal.logged_in_branch_id, _env);
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert_CSID}:{secret_CSID}"));

                var ProductionCSIDResponse = await ZatcaAuth.GetProductionCSIDAsync(complainceRequestID, credentials, _env);
                string binarySecurityToken1 = ProductionCSIDResponse.BinarySecurityToken;
                string secret1 = ProductionCSIDResponse.Secret;
                string requestID1 = ProductionCSIDResponse.RequestID;

                //If csid is not null, assign values to textboxes
                // and make buttons visible
                string cert = binarySecurityToken1 ?? "";
                string secret = secret1 ?? "";
                string PCSID_credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));


                var invoiceHash = new EInvoiceHashGenerator();
                var hashResult = invoiceHash.GenerateEInvoiceHashing(ublXml);

                var requestBody = new
                {
                    invoiceHash = hashResult.Hash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };

                // Fix for CS4014 and IDE0058: Await the async call and handle the result
                var response = await ZatcaHelper.CallSingleInvoiceClearanceAsync(requestBody, PCSID_credentials);
                MessageBox.Show($"Successfully submitted to ZATCA ({_env}):\n{response}");
                // Optionally, you can process 'response' or show a message if needed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}");
            }
        }
        public async Task ZatcaInvoiceReportingAsync(string invoiceNo)
        {
            try
            {
                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(_env))
                {
                    MessageBox.Show("No active ZATCA credentials found. Please configure them first.");
                    return;
                }

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                string cert = ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, _env);
                string secret = ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, _env);
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));

                var invoiceHash = new EInvoiceHashGenerator();
                var hashResult = invoiceHash.GenerateEInvoiceHashing(ublXml);

                var requestBody = new
                {
                    invoiceHash = hashResult.Hash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };

                // Fix for CS4014 and IDE0058: Await the async call and handle the result
                var response = await ZatcaHelper.CallSingleInvoiceReportingAsync(requestBody, credentials);
                MessageBox.Show($"Successfully submitted to ZATCA ({_env}):\n{response}");
                // Optionally, you can process 'response' or show a message if needed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}");
            }
        }
        private async Task SendOrReportToZatca(string invoiceNo, string env, bool isReport)
        {
            try
            {
                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(_env))
                {
                    MessageBox.Show("No active ZATCA credentials found. Please configure them first.");
                    return;
                }

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                string cert = ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, _env);
                string secret = ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, _env);
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));

                var invoiceHash = new EInvoiceHashGenerator();
                var hashResult = invoiceHash.GenerateEInvoiceHashing(ublXml);

                var requestBody = new
                {
                    invoiceHash = hashResult.Hash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
                    client.DefaultRequestHeaders.Add("accept", "application/json");
                    client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                    client.DefaultRequestHeaders.Add("Accept-Language", "EN");

                    string url;
                    switch (_env)
                    {
                        case "Production":
                            url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices";
                            break;
                        case "Simulation":
                            url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation/compliance/invoices";
                            break;
                        default:
                            url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices";
                            break;
                    }

                    var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string errorDetails = error?.validationResults?.errorMessages?[0]?.message ?? responseContent;
                        throw new Exception($"ZATCA Error: {errorDetails}");
                    }

                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string zatcaStatus = (GetInvoiceTypeCode(invoiceNo) == "01" ? jsonResponse?.clearanceStatus : jsonResponse?.reportingStatus); //01=Standard Invoice
                    
                    ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                        invoiceNo,
                        requestResult.InvoiceRequest.Uuid,
                        hashResult.Hash,
                        requestResult.InvoiceRequest.Invoice,
                        zatcaStatus,
                        env,
                        responseContent.ToString()
                        );

                    MessageBox.Show($"Successfully submitted to ZATCA ({env}):\n{jsonResponse}");
                    //LoadZatcaInvoices();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}");
            }
        }

        private void btnViewResponse_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string response = gridZatcaInvoices.CurrentRow.Cells["zatca_message"].Value.ToString();
            MessageBox.Show("\n"+response, "ZATCA Response");
        }

        private XmlDocument LoadSignedXMLInvoice(string invoiceNo)
        {
            string path = System.IO.Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");
            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException("XML/UBL invoice not found.");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(path);
            return xmlDoc;
        }
        public static string GetInvoiceTypeCode(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
            return salesBLL.GetInvoiceTypeCode(invoiceNo);
        }

        private void btn_viewQR_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();

            SalesBLL objSalesBLL = new SalesBLL();
            DataTable dt = objSalesBLL.SearchRecord(invoiceNo);

            if (dt.Rows.Count > 0 && dt.Rows[0]["zatca_qrcode_phase2"] != DBNull.Value)
            {
                byte[] qrCodeBytes = (byte[])dt.Rows[0]["zatca_qrcode_phase2"];

                pos.Master.Companies.zatca.ShowQRCodePhase2 showQRCodePhase2 = new ShowQRCodePhase2(qrCodeBytes, invoiceNo);
                showQRCodePhase2.ShowDialog();
            }
            else
            {
                MessageBox.Show("No QR Code found for this invoice.");
            }
        }

        private void btn_signInvoice_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
            string account = gridZatcaInvoices.CurrentRow.Cells["account"].Value.ToString();
            string prevInvoiceNo = gridZatcaInvoices.CurrentRow.Cells["prevInvoiceNo"].Value.ToString();
            DateTime prevSaleDate = (string.IsNullOrEmpty(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value.ToString()) ? DateTime.Now : Convert.ToDateTime(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value.ToString()));

            if (account.ToLower() == "sale")
            {
                ZatcaHelper.SignInvoiceToZatca(invoiceNo);
            }else if (account.ToLower() == "return")
            {
                ZatcaHelper.SignCreditNoteToZatca(invoiceNo, prevInvoiceNo, prevSaleDate);
            }
            //LoadZatcaInvoices();
        }

        private void btn_Invoice_clearance_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
            ZatcaInvoiceClearanceAsync(invoiceNo);
        }

        private void btn_invoice_report_Click(object sender, EventArgs e)
        {
            if(gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
            ZatcaInvoiceReportingAsync(invoiceNo);
        }

        private void btn_PCSID_sign_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString(); 
            ZatcaHelper.PCSID_SignInvoiceToZatcaAsync(invoiceNo);
        }
    }
}