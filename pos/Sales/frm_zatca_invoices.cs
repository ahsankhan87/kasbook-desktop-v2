using com.sun.corba.se.spi.ior;
using com.sun.security.ntlm;
using java.security;
using java.util;
using Newtonsoft.Json;
using pos.Master.Companies.zatca;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;

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
        private async void NewComplianceCheckButton_Click(object sender, EventArgs e)
        {
            await ZatcaInvoiceReporting_NEWAsync(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString());
        }

        public async Task ZatcaInvoiceReporting_NEWAsync(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
            try
            {
                if (!UsersModal.useZatcaEInvoice)
                {
                    MessageBox.Show("ZATCA E-Invoice is not enabled.");
                    return;
                }

                // 1️⃣ Generate unsigned XML
                XmlDocument ublXml = ZatcaHelper.GenerateUBLXMLInvoice(invoiceNo);

                // 2️⃣ Read Issue Date & Time (FINAL)
                XmlNamespaceManager ns = new XmlNamespaceManager(ublXml.NameTable);
                ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

                string issueDate = ublXml.SelectSingleNode("//cbc:IssueDate", ns)?.InnerText;
                string issueTime = ublXml.SelectSingleNode("//cbc:IssueTime", ns)?.InnerText;

                if (string.IsNullOrEmpty(issueDate) || string.IsNullOrEmpty(issueTime))
                    throw new Exception("IssueDate or IssueTime missing in XML.");

                // 3️⃣ Extract totals & seller info (use your existing logic)
                // Get invoice data for QR code
                DataSet ds = salesBLL.GetSaleAndItemsDataSet(invoiceNo);
                DataRow invoice = ds.Tables["Sale"].Rows[0];

                // Calculate totals for QR code
                decimal totalWithVat = Convert.ToDecimal(invoice["total_amount"]) + Convert.ToDecimal(invoice["total_tax"]) - Convert.ToDecimal(invoice["discount_value"]);
                decimal vatAmount = Convert.ToDecimal(invoice["total_tax"]);

                // Get seller info
                string sellerName = "";
                string sellerVAT = "";
                GeneralBLL objBLL = new GeneralBLL();
                DataTable companies_dt = objBLL.GetRecord("TOP 1 *", "pos_companies");
                if (companies_dt.Rows.Count > 0)
                {
                    sellerName = companies_dt.Rows[0]["name"].ToString();
                    sellerVAT = companies_dt.Rows[0]["vat_no"].ToString();
                }


                // 6️⃣ Load ZATCA credentials
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                DataRow PCSID = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(
                    Convert.ToInt32(activeZatcaCredential["id"])
                );
                string env = activeZatcaCredential["mode"].ToString();
                string privateKey = activeZatcaCredential["private_key"].ToString();
                string certBase64 = PCSID["cert_base64"].ToString();
                string PCSIDCertificate = PCSID["cert_base64"].ToString();
                string secret = PCSID["secret_key"].ToString();
                string PCSID_authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PCSIDCertificate}:{secret}"));

                string decodedCert = Encoding.UTF8.GetString(Convert.FromBase64String(certBase64));

                // 7️⃣ SIGN XML
                var signer = new EInvoiceSigner();
                SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey);

                if (!signResult.IsValid)
                    throw new Exception(signResult.ErrorMessage);

                // Tag-6
                var hashGen = new EInvoiceHashGenerator();
                string invoiceHashBase64 =
                    hashGen.GenerateEInvoiceHashing(signResult.SignedEInvoice).Hash;
                //string invoiceHashBase64 = signResult.Steps[1].ResultedValue.ToString(); 
                //ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, invoiceHashBase64);

                // Tag-7,8,9
                var sigData = ZatcaHelper.ExtractSignatureData(signResult.SignedEInvoice, PCSIDCertificate);

                string ecdsaSignatureBase64 = sigData.SignatureValueBase64;
                string ecdsaPublicKeyBase64 = sigData.PublicKeyBase64;
                string certificateSignatureBase64 = sigData.CertificateSignatureBase64;

                // 4️⃣ Generate ZATCA-SAFE QR (Tag-3 fixed)
                string qrBase64 = ZatcaPhase2QrGenerator.GenerateQrBase64(
                    sellerName,
                    sellerVAT,
                    issueDate,
                    issueTime,
                    totalWithVat,
                    vatAmount,
                    invoiceHashBase64,
                    ecdsaSignatureBase64,
                    ecdsaPublicKeyBase64,
                    certificateSignatureBase64
                );

                // 5️⃣ Insert QR AFTER signing (correct for Phase-2)
                ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

                // 8️⃣ Save signed XML
                string ublFolder = Path.Combine(Application.StartupPath, "UBL");
                Directory.CreateDirectory(ublFolder);

                string path = Path.Combine(ublFolder, invoiceNo + "_signed.xml");
                signResult.SignedEInvoice.Save(path);

                // 9️⃣ Save QR
                salesBLL.UpdateZatcaQrCode(invoiceNo, Convert.FromBase64String(qrBase64));
                salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", path, null);

                // Validate QR contents
                var tlvs = ZatcaPhase2QrGenerator.DecodeZatcaQr(qrBase64);
                //ZatcaPhase2QrGenerator.ValidateZatcaQr(tlvs);
                //MessageBox.Show("QR Tag-8: " + tlvs.First(t => t.Tag == 8).Value);
                //MessageBox.Show("QR Tag-9: " + tlvs.First(t => t.Tag == 9).Value);

                // 10. Generate request payload
                var requestGenerator = new RequestGenerator();
                var requestResult = requestGenerator.GenerateRequest(signResult.SignedEInvoice);

                object requestBody = new
                {
                    invoiceHash = invoiceHashBase64, //requestResult.InvoiceRequest.InvoiceHash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };
                
                // 3. Call ZATCA Reporting API
                var response = await ZatcaHelper.CallSingleInvoiceReportingAsync(requestBody, PCSID_authorization, env);
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

                // 4. Save the ZATCA status to the database
                ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                       invoiceNo,
                       requestResult.InvoiceRequest.Uuid,
                       invoiceHashBase64, ///requestResult.InvoiceRequest.InvoiceHash,
                       requestResult.InvoiceRequest.Invoice,
                       zatcaStatus,
                       env,
                       responseContent.ToString()
                       );
                
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
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

                XmlDocument ublXml = ZatcaHelper.LoadSignedXMLInvoice(invoiceNo);

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

        private void btnViewResponse_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null) return;
            string response = gridZatcaInvoices.CurrentRow.Cells["zatca_message"].Value.ToString();
            MessageBox.Show("\n" + response, "ZATCA Response");
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

                //if (status.ToLower() == "signed" || status.ToLower() == "cleared" || status.ToLower() == "reported")
                //{
                //    MessageBox.Show("This invoice is already signed or cleared/reported to ZATCA.", "Zatca Invoice Sign", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

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
                await ZatcaHelper.ZatcaInvoiceClearanceAsync(invoiceNo);
                LoadZatcaInvoices(); // Optionally reload the invoices grid
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
                await ZatcaHelper.ZatcaInvoiceReportingAsync(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString());
                LoadZatcaInvoices(); // Optionally reload the invoices grid
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