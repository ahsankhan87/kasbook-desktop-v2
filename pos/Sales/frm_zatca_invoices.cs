using com.sun.security.ntlm;
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
        public frm_zatca_invoices()
        {
            InitializeComponent();
            
        }

        private void frm_zatca_invoices_Load(object sender, EventArgs e)
        {
           
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

        

        private async void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridZatcaInvoices.CurrentRow == null) return;

                // Get the selected invoice number from the grid
                if (gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value == null)
                {
                    MessageBox.Show("Please select a valid invoice.");
                    return;
                }
                // Assuming the invoice number is in the "invoice_no" column
                if (string.IsNullOrEmpty(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString()))
                {
                    MessageBox.Show("Selected invoice number is empty.");
                    return;
                }
            
                string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();

                btnComplianceChecks.Enabled = false;
                btnComplianceChecks.Text = "Checking...";
                btnComplianceChecks.Cursor = Cursors.WaitCursor;
                btnComplianceChecks.Refresh();
                await ZatcaInvoiceComplianceChecks(invoiceNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnComplianceChecks.Enabled = true;
                btnComplianceChecks.Text = "Compliance checks";
                btnComplianceChecks.Cursor = Cursors.Default;
            }
            
            
        }
        private async Task ZatcaInvoiceComplianceChecks(string invoiceNo)
        {
            try
            {
                if (invoiceNo == null) return;

                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID credentials found. Please configure them first.");
                    return;
                }
                string env = activeZatcaCredential["mode"].ToString();
               
                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(env))
                {
                    MessageBox.Show("No active ZATCA CSID credentials found. Please configure them first.");
                    return;
                }

                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                string cert = activeZatcaCredential["cert_base64"].ToString(); 
                string secret = activeZatcaCredential["secret_key"].ToString(); //ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, env);
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
                    switch (env)
                    {
                        case "Production":
                            url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/core/compliance/invoices";
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

        public async Task ZatcaInvoiceClearanceAsync(string invoiceNo)
        {
            try
            {
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID credentials found. Please configure them first.");
                    return;
                }

                string env = activeZatcaCredential["mode"].ToString();
                int credentialId = Convert.ToInt32(activeZatcaCredential["id"]);

                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(env))
                {
                    MessageBox.Show("No active ZATCA credentials found. Please configure them first.");
                    return;
                }

                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                // Retrieve PCSID credentials from the database using the credentialId
                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(credentialId);
                if (PCSID_dataRow == null)
                {
                    MessageBox.Show("No PCSID credentials found for the selected ZATCA credential.");
                    return;
                }

                string cert = PCSID_dataRow["cert_base64"].ToString();
                string secret = PCSID_dataRow["secret_key"].ToString();
                string PCSID_credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));
                ////

                var invoiceHash = new EInvoiceHashGenerator();
                var hashResult = invoiceHash.GenerateEInvoiceHashing(ublXml);

                var requestBody = new
                {
                    invoiceHash = hashResult.Hash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };

                // Fix for CS4014 and IDE0058: Await the async call and handle the result
                var response = await ZatcaHelper.CallSingleInvoiceClearanceAsync(requestBody, PCSID_credentials, env);

                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("No response received from ZATCA.");
                    return;
                }
               
                var responseContent = JsonConvert.DeserializeObject<dynamic>(response);
                if (responseContent == null)
                {
                    MessageBox.Show("Invalid response format from ZATCA.");
                    return;
                }

                string zatcaStatus = responseContent?.clearanceStatus;
                string clearedInvoice = responseContent?.clearedInvoice;

                if (string.IsNullOrEmpty(zatcaStatus))
                {
                    MessageBox.Show("ZATCA clearance status is empty or not found in the response.");
                    return;
                }

                // Save the ZATCA status to the database
                ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                       invoiceNo,
                       requestResult.InvoiceRequest.Uuid,
                       hashResult.Hash,
                       clearedInvoice,      //requestResult.InvoiceRequest.Invoice,
                       zatcaStatus,
                       env,
                       responseContent.ToString()
                       );
                // Optionally, you can save the response content to the database or process it further
                // Show a success message
                if (zatcaStatus == "CLEARED")
                {
                    MessageBox.Show($"Invoice {invoiceNo} has been successfully cleared by ZATCA.");
                }
                else
                {
                    MessageBox.Show($"ZATCA clearance status for invoice {invoiceNo}: {zatcaStatus}");
                }
                // Show the response in a message box or process it as needed
                // For example, you can show the response in a message box
                MessageBox.Show($"ZATCA Response:\n{response}");
                // Alternatively, you can log the response or save it to a file/database
                LoadZatcaInvoices(); // Optionally reload the invoices grid

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
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID credentials found. Please configure them first.");
                    return;
                }

                string env = activeZatcaCredential["mode"].ToString();
                int credentialId = Convert.ToInt32(activeZatcaCredential["id"]);

                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(env))
                {
                    MessageBox.Show("No active ZATCA credentials found. Please configure them first.");
                    return;
                }
                
                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                // Retrieve PCSID credentials from the database using the credentialId
                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(credentialId);
                if (PCSID_dataRow == null)
                {
                    MessageBox.Show("No PCSID credentials found for the selected ZATCA credential.");
                    return;
                }

                string cert = PCSID_dataRow["cert_base64"].ToString();
                string secret = PCSID_dataRow["secret_key"].ToString();
                string PCSID_credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));
                ////
                ///

                var invoiceHash = new EInvoiceHashGenerator();
                var hashResult = invoiceHash.GenerateEInvoiceHashing(ublXml);

                var requestBody = new
                {
                    invoiceHash = hashResult.Hash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };

                // Fix for CS4014 and IDE0058: Await the async call and handle the result
                var response = await ZatcaHelper.CallSingleInvoiceReportingAsync(requestBody, PCSID_credentials,env);
                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("No response received from ZATCA.");
                    return;
                }
                var responseContent = JsonConvert.DeserializeObject<dynamic>(response);
                if (responseContent == null)
                {
                    MessageBox.Show("Invalid response format from ZATCA.");
                    return;
                }
                string zatcaStatus = responseContent?.reportingStatus;

                if (string.IsNullOrEmpty(zatcaStatus))
                {
                    MessageBox.Show("ZATCA reporting status is empty or not found in the response.");
                    return;
                }

                // Save the ZATCA status to the database
                ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                       invoiceNo,
                       requestResult.InvoiceRequest.Uuid,
                       hashResult.Hash,
                       requestResult.InvoiceRequest.Invoice,
                       zatcaStatus,
                       env,
                       responseContent.ToString()
                       );
                // Optionally, you can save the response content to the database or process it further
                // Show a success message
                if (zatcaStatus == "REPORTED")
                {
                    MessageBox.Show($"Invoice {invoiceNo} has been successfully reported by ZATCA.");
                }
                else
                {
                    MessageBox.Show($"ZATCA clearance status for invoice {invoiceNo}: {zatcaStatus}");
                }
                // Show the response in a message box or process it as needed
                // For example, you can show the response in a message box
                MessageBox.Show($"ZATCA Response:\n{response}");
                // Alternatively, you can log the response or save it to a file/database
                LoadZatcaInvoices(); // Optionally reload the invoices grid
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
                ZatcaHelper.SignCreditNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
                
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

        private async void btn_PCSID_sign_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
            string account = gridZatcaInvoices.CurrentRow.Cells["account"].Value.ToString();
            string prevInvoiceNo = gridZatcaInvoices.CurrentRow.Cells["prevInvoiceNo"].Value.ToString();
            DateTime prevSaleDate = (string.IsNullOrEmpty(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value.ToString()) ? DateTime.Now : Convert.ToDateTime(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value.ToString()));

            if (account.ToLower() == "sale")
            {
                await ZatcaHelper.PCSID_SignInvoiceToZatcaAsync(invoiceNo);
            }
            else if (account.ToLower() == "return")
            {
                ZatcaHelper.PCSID_SignCreditNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
            }
        }
    }
}