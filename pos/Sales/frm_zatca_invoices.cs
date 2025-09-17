using com.sun.corba.se.spi.ior;
using com.sun.security.ntlm;
using Newtonsoft.Json;
using pos.Master.Companies.zatca;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.IO;
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
            StatusDDL();
            InvoiceSubTypeDDL();
            InvoiceTypeDDL();
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

            string invoiceNo = txtInvoiceNo.Text.Trim();
            string type = cmbType.SelectedValue?.ToString();
            string subtype = cmbSubtype.SelectedValue?.ToString();
            string status = cmb_status.SelectedValue?.ToString();
            DateTime? fromdate = dtpFromDate.Checked ? dtpFromDate.Value.Date : (DateTime?)null;
            DateTime? todate = dtpToDate.Checked ? dtpToDate.Value.Date : (DateTime?)null;

            DataTable dt = new SalesBLL().SearchInvoices(invoiceNo,fromdate,type,subtype,todate,status); // Implement this method in your BLL
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
                    MessageBox.Show("No active ZATCA CSID credentials found. Please configure them first.", "ZATCA CSID");
                    return;
                }
                string env = activeZatcaCredential["mode"].ToString();

                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(env))
                {
                    MessageBox.Show("No active ZATCA CSID credentials found. Please configure them first.", "ZATCA CSID");
                    return;
                }

                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(ublXml);
                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    //ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                    //    invoiceNo,
                    //    requestResult.InvoiceRequest.Uuid,
                    //    hashResult.Hash,
                    //    requestResult.InvoiceRequest.Invoice,
                    //    zatcaStatus,
                    //    env,
                    //    responseContent.ToString()
                    //    );

                    // Optionally, you can save the response content to the database or process it further
                    // Show a success message

                    MessageBox.Show($"ZATCA status for invoice {invoiceNo}: {zatcaStatus}", "Zatca Compliance Checks", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MessageBox.Show($"ZATCA Response ({env}):\n{jsonResponse}", "Zatca Response", MessageBoxButtons.OK);
                    //LoadZatcaInvoices();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"Invoice {invoiceNo} has been successfully cleared by ZATCA.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"ZATCA clearance status for invoice {invoiceNo}: {zatcaStatus}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Show the response in a message box or process it as needed
                // For example, you can show the response in a message box
                MessageBox.Show($"ZATCA Response:\n{response}", "Zatca Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Alternatively, you can log the response or save it to a file/database
                LoadZatcaInvoices(); // Optionally reload the invoices grid

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var response = await ZatcaHelper.CallSingleInvoiceReportingAsync(requestBody, PCSID_credentials, env);
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
                    MessageBox.Show($"Invoice {invoiceNo} has been successfully reported by ZATCA.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"ZATCA clearance status for invoice {invoiceNo}: {zatcaStatus}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Show the response in a message box or process it as needed
                // For example, you can show the response in a message box
                MessageBox.Show($"ZATCA Response:\n{response}", "Zatca Response");
                // Alternatively, you can log the response or save it to a file/database
                LoadZatcaInvoices(); // Optionally reload the invoices grid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewResponse_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string response = gridZatcaInvoices.CurrentRow.Cells["zatca_message"].Value.ToString();
            MessageBox.Show("\n" + response, "ZATCA Response");
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

        private async void btn_signInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridZatcaInvoices.CurrentRow == null) return;
                string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
                string account = gridZatcaInvoices.CurrentRow.Cells["account"].Value.ToString();
                string prevInvoiceNo = gridZatcaInvoices.CurrentRow.Cells["prevInvoiceNo"].Value.ToString();
                string status = gridZatcaInvoices.CurrentRow.Cells["zatca_status"].Value.ToString();
                DateTime prevSaleDate = (string.IsNullOrEmpty(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value.ToString()) ? DateTime.Now : Convert.ToDateTime(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value.ToString()));

                btn_signInvoice.Enabled = false;
                btn_signInvoice.Text = "Checking...";
                btn_signInvoice.Cursor = Cursors.WaitCursor;
                btn_signInvoice.Refresh();

                if (status.ToLower() == "signed" || status.ToLower() == "cleared" || status.ToLower() == "reported")
                {
                    MessageBox.Show("This invoice is already signed or cleared/reported to ZATCA.", "Zatca Invoice Sign", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //sign the credit note invoice to ZATCA
                if (UsersModal.useZatcaEInvoice == true)
                {
                    DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                    if (activeZatcaCredential == null)
                    {
                        MessageBox.Show("No active ZATCA CSID/credentials found. Please configure them first.");
                    }

                    // Retrieve PCSID credentials from the database using the credentialId
                    DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                    if (PCSID_dataRow == null)
                    {
                        //Sign Invoice with CSID instead of Production CSID
                        if (account.ToLower() == "sale")
                        {
                            ZatcaHelper.SignInvoiceToZatca(invoiceNo);
                        }
                        else if (account.ToLower() == "return")
                        {
                            ZatcaHelper.SignCreditNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);

                        }
                        else if (account.ToLower() == "debit")
                        {
                            ZatcaHelper.SignDebitNoteToZatca(invoiceNo, prevInvoiceNo, prevSaleDate);

                        }
                        else
                        {
                            MessageBox.Show("Invalid account type for signing invoice.");

                        }
                    }
                    else
                    {
                        //If PCSID exist then sign it 
                        if (account.ToLower() == "sale")
                        {
                            await ZatcaHelper.PCSID_SignInvoiceToZatcaAsync(invoiceNo);
                        }
                        else if (account.ToLower() == "return")
                        {
                            await ZatcaHelper.PCSID_SignCreditNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
                        }
                        else if (account.ToLower() == "debit")
                        {
                            await ZatcaHelper.PCSID_SignDebitNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
                        }
                    }

                }
                //////
                ///

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_signInvoice.Enabled = true;
                btn_signInvoice.Text = "Sign Invoice";
                btn_signInvoice.Cursor = Cursors.Default;
            }
            LoadZatcaInvoices();
        }

        private async void btn_Invoice_clearance_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridZatcaInvoices.CurrentRow == null) return;
                string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
                string account = gridZatcaInvoices.CurrentRow.Cells["account"].Value.ToString();
                string invoice_subtype = gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value.ToString();

                if (invoice_subtype.ToLower() == "simplified") // Simplified Invoice
                {
                    MessageBox.Show("Simplified invoices cannot be cleared. Please use the reporting option instead.", "ZATCA Clearance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrEmpty(invoiceNo) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(invoice_subtype))
                {
                    MessageBox.Show("Please ensure all required fields are filled.");
                    return;
                }   

                btn_Invoice_clearance.Enabled = false;
                btn_Invoice_clearance.Text = "Checking...";
                btn_Invoice_clearance.Cursor = Cursors.WaitCursor;
                btn_Invoice_clearance.Refresh();
                await ZatcaInvoiceClearanceAsync(invoiceNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_Invoice_clearance.Enabled = true;
                btn_Invoice_clearance.Text = "Clearance";
                btn_Invoice_clearance.Cursor = Cursors.Default;
            }
        }

        private async void btn_invoice_report_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridZatcaInvoices.CurrentRow == null) return;
                string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
                string account = gridZatcaInvoices.CurrentRow.Cells["account"].Value.ToString();
                string invoice_subtype = gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value.ToString();

                if (invoice_subtype.ToLower() == "standard") // 01=Standard Invoice
                {
                    MessageBox.Show("Standard invoices cannot be reported. Please use the clearance option instead.", "ZATCA Reporting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
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
                btn_invoice_report.Enabled = false;
                btn_invoice_report.Text = "Checking...";
                btn_invoice_report.Cursor = Cursors.WaitCursor;
                btn_invoice_report.Refresh();
                await ZatcaInvoiceReportingAsync(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn_invoice_report.Enabled = true;
                btn_invoice_report.Text = "Reporting";
                btn_invoice_report.Cursor = Cursors.Default;
            }
        }

        private void InvoiceTypeDDL()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow _row_0 = dt.NewRow();
            _row_0["id"] = "";
            _row_0["name"] = "All";
            dt.Rows.Add(_row_0);

            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "Sale";
            _row_1["name"] = "Sale";
            dt.Rows.Add(_row_1);

            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "Return";
            _row_2["name"] = "Credit Note";
            dt.Rows.Add(_row_2);

            DataRow _row_3 = dt.NewRow();
            _row_3["id"] = "Debit";
            _row_3["name"] = "Debit Note";
            dt.Rows.Add(_row_3);

            cmbType.DisplayMember = "name";
            cmbType.ValueMember = "id";
            cmbType.DataSource = dt;

            cmbType.SelectedIndex = 0; // Set default selection to "Sale"
        }

        private void InvoiceSubTypeDDL()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow _row_0 = dt.NewRow();
            _row_0["id"] = "";
            _row_0["name"] = "All";
            dt.Rows.Add(_row_0);

            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "02";
            _row_1["name"] = "Simplified";
            dt.Rows.Add(_row_1);

            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "01";
            _row_2["name"] = "Standard";

            dt.Rows.Add(_row_2);

            cmbSubtype.DisplayMember = "name";
            cmbSubtype.ValueMember = "id";
            cmbSubtype.DataSource = dt;

            cmbSubtype.SelectedIndex = 0; // Set default selection to "Standard"
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string invoiceNo = txtInvoiceNo.Text.Trim();
                string type = cmbType.SelectedValue?.ToString();
                string subtype = cmbSubtype.SelectedValue?.ToString();
                string status = cmb_status.SelectedValue?.ToString();
                DateTime? fromdate = dtpFromDate.Checked ? dtpFromDate.Value.Date : (DateTime?)null;
                DateTime? todate = dtpToDate.Checked ? dtpToDate.Value.Date : (DateTime?)null;

                var results = new SalesBLL().SearchInvoices(invoiceNo, fromdate, type, subtype, todate, status);
                gridZatcaInvoices.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching invoices: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnDownloadUBL_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null)
            {
                MessageBox.Show("Please select an invoice to download.");
                return;
            }

            string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();
            DownloadUBLInvoice(invoiceNo);
        }
        private void DownloadUBLInvoice(string invoiceNo)
        {
            if (gridZatcaInvoices.CurrentRow == null)
            {
                MessageBox.Show("Please select an invoice to download.");
                return;
            }

            // Example column names: adjust as per your DataGridView
            string vat = gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value.ToString();
            DateTime issueDate = Convert.ToDateTime(gridZatcaInvoices.CurrentRow.Cells["sale_time"].Value);
            string irn = System.Text.RegularExpressions.Regex.Replace(invoiceNo, "[^a-zA-Z0-9]", "-");
            
            string datePart = issueDate.ToString("yyyyMMdd");
            string timePart = issueDate.ToString("HHmmss");
            string fileName = $"{vat}_{datePart}T{timePart}_{irn}.xml";

           
            // Replace with your actual logic to get UBL XML content
            XmlDocument ublXmlDoc = GetUBLXmlByInvoiceNo(invoiceNo);

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "XML Files (*.xml)|*.xml";
                sfd.FileName = fileName;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ublXmlDoc.Save(sfd.FileName);
                    MessageBox.Show("UBL Invoice downloaded successfully.","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }
        private XmlDocument GetUBLXmlByInvoiceNo(string invoiceNo)
        {
            string path = Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");

            if (!File.Exists(path))
                throw new FileNotFoundException("XML/UBL invoice not found.");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true; // important for signed XML
            xmlDoc.Load(path); // this reads the file and parses it into an XmlDocument
            return xmlDoc;
        }

        private void StatusDDL()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_0 = dt.NewRow();
            _row_0["id"] = "";
            _row_0["name"] = "All";
            dt.Rows.Add(_row_0);
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "Signed";
            _row_1["name"] = "Signed";
            dt.Rows.Add(_row_1);
            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "Cleared";
            _row_2["name"] = "Cleared";
            dt.Rows.Add(_row_2);
            DataRow _row_3 = dt.NewRow();
            _row_3["id"] = "Reported";
            _row_3["name"] = "Reported";
            dt.Rows.Add(_row_3);
            DataRow _row_4 = dt.NewRow();
            _row_4["id"] = "Pending";
            _row_4["name"] = "Pending";
            dt.Rows.Add(_row_4);
            DataRow _row_5 = dt.NewRow();
            _row_5["id"] = "Failed";
            _row_5["name"] = "Failed";
            dt.Rows.Add(_row_5);

            cmb_status.DisplayMember = "name";
            cmb_status.ValueMember = "id";
            cmb_status.DataSource = dt;
            cmb_status.SelectedIndex = 0; // Set default selection to "All"
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}