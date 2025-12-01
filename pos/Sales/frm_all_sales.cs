using Newtonsoft.Json;
using pos.Master.Companies.zatca;
using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;

namespace pos
{
    public partial class frm_all_sales : Form
    {
        public SalesBLL objSalesBLL = new SalesBLL();

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_all_sales()
        {
            InitializeComponent();

            // Ensure user identity exists; hydrate claims from DB
            if (_currentUser == null)
            {
                var parsedRole = SystemRole.Viewer;
                System.Enum.TryParse(UsersModal.logged_in_user_role, true, out parsedRole);
                AppSecurityContext.SetUser(new UserIdentity
                {
                    UserId = UsersModal.logged_in_userid,
                    BranchId = UsersModal.logged_in_branch_id,
                    Username = UsersModal.logged_in_username,
                    Role = parsedRole
                });
                _currentUser = AppSecurityContext.User;
            }

            // On load, refresh claims from DB and apply permission attributes
            this.Load += (s, e) =>
            {
                AppSecurityContext.RefreshUserClaims();
                RequirePermissionAttribute.Apply(this, _currentUser, _auth);
            };
        }

        private void frm_all_sales_Load(object sender, EventArgs e)
        {
            load_all_sales_grid();

            // Disable/hide actions based on DB-backed permissions
            Btn_PrintPOS80.Enabled = _auth.HasPermission(_currentUser, Permissions.Sales_Print);
            btn_print_invoice.Enabled = _auth.HasPermission(_currentUser, Permissions.Sales_Print);
            // Add further UI elements here as needed, e.g. delete/report buttons/menus.
        }

        public void load_all_sales_grid()
        {
            try
            {
                grid_all_sales.DataSource = null;

                //bind data in data grid view  
                grid_all_sales.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
                // String table = "pos_all_sales";
                grid_all_sales.DataSource = objSalesBLL.GetAllSales();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_sales_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {

                String condition = txt_search.Text;
                if (!string.IsNullOrEmpty(condition))
                {
                    grid_all_sales.DataSource = objSalesBLL.SearchRecord(condition);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void grid_all_sales_KeyDown(object sender, KeyEventArgs e)
        {
            if (grid_all_sales.RowCount > 0 && e.KeyCode == Keys.Enter)
            {
                var sale_id = grid_all_sales.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

                load_sales_items_detail(Convert.ToInt32(sale_id), invoice_no);
            }
        }

        private void grid_all_sales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sale_id = grid_all_sales.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

            load_sales_items_detail(Convert.ToInt32(sale_id), invoice_no);
        }

        private void load_sales_items_detail(int sale_id, string invoice_no)
        {
            frm_sales_detail frm_sales_detail_obj = new frm_sales_detail(sale_id, invoice_no);
            //frm_sales_detail_obj.load_sales_detail_grid();
            frm_sales_detail_obj.ShowDialog();
        }

        private void frm_all_sales_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public DataTable load_sales_receipt()
        {
            //DataTable dt = new DataTable();
            if (grid_all_sales.Rows.Count > 0)
            {
                var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                //bind data in data grid view  
                return objSalesBLL.SaleReceipt(invoice_no);
            }
            return null;

        }

        private void btn_print_Click(object sender, EventArgs e)
        {

            using (frm_sales_receipt obj = new frm_sales_receipt(load_sales_receipt()))
            {
                obj.ShowDialog();
            }

        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_sales.Rows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Print invoice with product code?", "Print Invoice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                bool isPrintInvoiceWithCode = false;
                if (result1 == DialogResult.Yes)
                {
                    isPrintInvoiceWithCode = true;
                }

                string sale_time = grid_all_sales.CurrentRow.Cells["sale_time"].Value.ToString();
                string invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                double netTotal = Convert.ToDouble(grid_all_sales.CurrentRow.Cells["total"].Value.ToString());
                double totalTax = Convert.ToDouble(grid_all_sales.CurrentRow.Cells["total_tax"].Value.ToString());
                string Zetca_qrcode = grid_all_sales.CurrentRow.Cells["Zetca_qrcode"].Value.ToString();

                //pos.Reports.Sales.New.SaleInvoiceReport saleInvoiceReport = new Reports.Sales.New.SaleInvoiceReport();
                //saleInvoiceReport.LoadReport(invoice_no, sale_time, netTotal,totalTax,false, isPrintInvoiceWithCode, Zetca_qrcode);
                //saleInvoiceReport.Show();

                using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(), false, isPrintInvoiceWithCode))
                {
                    obj.ShowDialog();
                }
            }
        }

        private void grid_all_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_all_sales.Columns[e.ColumnIndex].Name;
                if (name == "detail")
                {
                    var sale_id = grid_all_sales.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                    var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

                    load_sales_items_detail(Convert.ToInt32(sale_id), invoice_no);

                }
                if (name == "btn_delete" && !_auth.HasPermission(_currentUser, Permissions.Sales_Delete))
                {
                    MessageBox.Show("You don't have permission to delete sales.");
                    return;
                }
                if (name == "btn_send_zatca" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_Sign))
                {
                    MessageBox.Show("You don't have permission to sign invoices to ZATCA.");
                    return;
                }
                if (name == "btn_report_to_zatca" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_Report))
                {
                    MessageBox.Show("You don't have permission to report invoices to ZATCA.");
                    return;
                }
                if (name == "btn_download_ubl" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_DownloadUBL))
                {
                    MessageBox.Show("You don't have permission to download UBL.");
                    return;
                }
                if (name == "btn_show_qrcode" && !_auth.HasPermission(_currentUser, Permissions.Sales_Zatca_Qr_Show))
                {
                    MessageBox.Show("You don't have permission to view QR code.");
                    return;
                }
                if (name == "btn_delete")
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

                        int qresult = objSalesBLL.DeleteSales(invoice_no);
                        if (qresult > 0)
                        {
                            MessageBox.Show(invoice_no + " deleted successfully", "Delete Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_all_sales_grid();
                        }
                        else
                        {
                            MessageBox.Show(invoice_no + " not deleted, please try again", "Delete Sales", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
                if (name == "btn_send_zatca")
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                    int saleId = Convert.ToInt32(grid_all_sales.CurrentRow.Cells["id"].Value);
                    SignInvoiceToZatca(saleId, invoiceNo);
                }
                if (name == "btn_zatca_compliance_check")
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                    int saleId = Convert.ToInt32(grid_all_sales.CurrentRow.Cells["id"].Value);
                    ZatcaComplianceCheck(saleId, invoiceNo);
                }
                if (name == "btn_download_ubl")
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                    DownloadUBL(invoiceNo);
                }
                if (name == "btn_report_to_zatca")
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                    ReportInvoiceToZatcaAsync(invoiceNo);
                }
                if (name == "btn_show_qrcode")
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ZatcaComplianceCheck(int saleId, string invoiceNo)
        {
            try
            {
                if (UsersModal.useZatcaEInvoice == false)
                {
                    //MessageBox.Show("ZATCA E-Invoice is not enabled for this branch. Please enable it in profile/settings.");
                    return;
                }

                // 1. Get signed XML first
                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);
                //ublXml.Save("UBL\\debug_ubl" + invoiceNo + ".xml");

                // Check if ZATCA credentials are configured
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID found. Please configure them first.");
                    return;
                }

                // 3. Get Certificates
                string cert = activeZatcaCredential["cert_base64"].ToString(); // GetPublicKeyFromFile();
                string secret = activeZatcaCredential["secret_key"].ToString(); // GetSecretFromFile();

                //string cert = GetPublicKeyFromFile(); // CSID token / binarySecurityToken
                //string secret = GetSecretFromFile();

                var invoiceRequest = new RequestGenerator();
                RequestResult requestResult = invoiceRequest.GenerateRequest(ublXml);


                if (requestResult.IsValid)
                {

                    using (var client = new HttpClient())
                    {
                        // Choose endpoint
                        string url;
                        switch (activeZatcaCredential["mode"].ToString())
                        {
                            case "Production":
                                url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices";
                                break;
                            case "Simulation":
                                url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/simulation/invoices";
                                break;
                            default:
                                url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices";
                                break;
                        }

                        var invoiceHash = new EInvoiceHashGenerator();
                        var hashResult = invoiceHash.GenerateEInvoiceHashing(ublXml);

                        // Prepare the request body
                        var requestBody = new
                        {
                            invoiceHash = hashResult.Hash,
                            uuid = requestResult.InvoiceRequest.Uuid,
                            invoice = requestResult.InvoiceRequest.Invoice
                        };

                        var jsonBody = JsonConvert.SerializeObject(requestBody);
                        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));

                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
                        client.DefaultRequestHeaders.Add("accept", "application/json");
                        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        client.DefaultRequestHeaders.Add("Accept-Language", "EN");

                        var response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var resultString = await response.Content.ReadAsStringAsync();

                            // Parse the JSON
                            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(resultString);

                            //ShowZatcaResponse(jsonResponse);

                            string zatcaStatus = (GetInvoiceTypeCode(invoiceNo) == "01" ? jsonResponse?.clearanceStatus : jsonResponse?.reportingStatus); //01=Standard Invoice
                                                                                                                                                          // Save successful submission
                            ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                                invoiceNo,
                                requestResult.InvoiceRequest.Uuid,
                                hashResult.Hash,
                                requestResult.InvoiceRequest.Invoice,
                                zatcaStatus,
                                resultString.ToString());

                            //ShowZatcaResponse(jsonResponse);
                            MessageBox.Show($"Successfully submitted to ZATCA:\n{jsonResponse}");


                        }
                        else
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();

                            var error = JsonConvert.DeserializeObject<dynamic>(responseContent);
                            string errorDetails = error?.validationResults?.errorMessages?[0]?.message ?? responseContent;

                            //MessageBox.Show($"Successfully submitted to ZATCA:\n{responseContent}");

                            string zatcaStatus = (GetInvoiceTypeCode(invoiceNo) == "01" ? error?.clearanceStatus : error?.reportingStatus); //01=Standard Invoice

                            objSalesBLL.UpdateZatcaStatus(invoiceNo, zatcaStatus, null, errorDetails);
                            throw new Exception($"ZATCA Error: {errorDetails}");

                            //MessageBox.Show($"Failed to get invoiceRequest: {response.ReasonPhrase} - {error}");
                            //return;
                        }
                    }
                    load_all_sales_grid();
                }
                else
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending to ZATCA:\n" + ex.Message);
                //objSalesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        public XmlDocument GenerateUBLXMLInvoice(string invoiceNo)
        {

            // 2. Generate UBL XML (helper function — I’ll help build this)
            DataSet ds = objSalesBLL.GetSaleAndItemsDataSet(invoiceNo);
            //XmlDocument ublXml = pos.Master.Companies.zatca.ZatcaHelper.BuildUblXml(ds);

            // Create generator instance
            var generator = new ZatcaInvoiceGenerator();

            // Generate XML document
            XmlDocument ublXml = generator.GenerateZatcaInvoiceXmlDocument(ds, invoiceNo);

            return ublXml;

        }

        private void SignInvoiceToZatca(int saleId, string invoiceNo)
        {
            try
            {
                if (UsersModal.useZatcaEInvoice == false)
                {
                    MessageBox.Show("ZATCA E-Invoice is not enabled for this branch. Please enable it in profile/settings.");
                    return;
                }
                // 1. Get sale data
                XmlDocument ublXml = GenerateUBLXMLInvoice(invoiceNo);
                //ublXml.Save("UBL\\unsigned_ubl_"+ invoiceNo + ".xml");

                // Check if ZATCA credentials are configured
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID found. Please configure them first.");
                    return;
                }

                // 3. Sign XML
                //string cert = GetPublicKeyFromFile(); // CSID token / binarySecurityToken
                //string privateKey = GetPrivateKeyFromFile();
                string cert = activeZatcaCredential["cert_base64"].ToString(); // GetPublicKeyFromFile();
                string secret = activeZatcaCredential["secret_key"].ToString(); // GetSecretFromFile();
                string privateKey = activeZatcaCredential["private_key"].ToString();  //GetPrivateKeyFromFile();

                byte[] bytes = Convert.FromBase64String(cert);
                string decodedCert = Encoding.UTF8.GetString(bytes);

                //XmlDocument ublXml = LoadSampleUBL();
                //ublXml.Save("UBL\\debug_ubl1.xml");

                var signer = new EInvoiceSigner();
                SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey);

                //ShowSignResult(signResult);

                if (signResult.IsValid)
                {
                    // Make sure "UBL" folder exists
                    string ublFolder = Path.Combine(Application.StartupPath, "UBL");
                    if (!Directory.Exists(ublFolder))
                        Directory.CreateDirectory(ublFolder);

                    //var invoiceHash = new EInvoiceHashGenerator();
                    //var hashResult = invoiceHash.GenerateEInvoiceHashing(signResult.SignedEInvoice);

                    //Get Invoice Hash for Next PIH
                    var SignedInvoiceHash = GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

                    // Save signed XML
                    string ublPath = Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");
                    signResult.SignedEInvoice.Save(ublPath);
                    //signResult.SaveSignedEInvoice(ublPath);

                    //EInvoiceValidator eInvoiceValidator = new EInvoiceValidator();
                    //var resultValidator = eInvoiceValidator.ValidateEInvoice(signResult.SignedEInvoice, cert, secret);

                    //if (!resultValidator.IsValid)
                    //{
                    //    var failedSteps = resultValidator.ValidationSteps
                    //       .Where(step => !step.IsValid)
                    //       .Select(step => $"{step.ValidationStepName}: {step.ErrorMessages[0]}")
                    //       .ToList();

                    //    string fullError = failedSteps.Any()
                    //        ? string.Join("\n\n", failedSteps)
                    //        : resultValidator.ValidationSteps[0].ErrorMessages[0] ?? "Signing failed with unknown error.";

                    //    MessageBox.Show("Zatca Invoice Validator results:\n\n" + fullError);
                    //}



                    //Get QRCode from SignedInvoice
                    var Base64QrCode = GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    objSalesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
                    //MessageBox.Show($"Base64 QRCode : {Base64QrCode}\n");

                    ////GetRequestApi Payload
                    //RequestGenerator RequestGenerator = new RequestGenerator();
                    //RequestResult RequestResult = RequestGenerator.GenerateRequest(signResult.SignedEInvoice);

                    //if (RequestResult.IsValid)
                    //{
                    //    var jsonPath = Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_ApiRequestPayload.json");
                    //    RequestResult.SaveRequestToFile(jsonPath);
                    //    //MessageBox.Show($"Request Api Payload : \n{ RequestResult.InvoiceRequest.Serialize()}");
                    //}

                    // Save base64 string in DB (optional)
                    objSalesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

                    MessageBox.Show($"Invoice signed by Zatca and saved.");

                }
                else
                {
                    var failedSteps = signResult.Steps
                    .Where(step => !step.IsValid)
                    .Select(step => $"{step.StepName}: {step.Exception.Message}")
                    .ToList();

                    string fullError = failedSteps.Any()
                        ? string.Join("\n", failedSteps)
                        : signResult.ErrorMessage ?? "Signing failed with unknown error.";

                    MessageBox.Show("Signing failed:\n" + fullError);

                    MessageBox.Show("Signing failed:\n" + string.Join("\n", signResult.ErrorMessage));
                    objSalesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

                load_all_sales_grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                objSalesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        private async void ReportInvoiceToZatcaAsync(string invoiceNo)
        {
            try
            {
                // 1. Get signed XML first
                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                // 2. Prepare request
                RequestGenerator requestGenerator = new RequestGenerator();
                RequestResult requestResult = requestGenerator.GenerateRequest(ublXml);

                if (!requestResult.IsValid)
                {
                    MessageBox.Show("Failed to generate request: " + requestResult.ErrorMessages);
                    return;
                }

                // 3. Prepare credentials
                string cert = ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, "Simulation"); // GetPublicKeyFromFile();
                string secret = ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, "Simulation"); // GetSecretFromFile();
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert}:{secret}"));

                // 4. Prepare request body
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

                    // 5. Send request
                    string url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices";
                    var content = new StringContent(JsonConvert.SerializeObject(requestBody),
                        Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);

                    // 6. Handle response
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string errorDetails = error?.validationResults?.errorMessages?[0]?.message ?? responseContent;
                        throw new Exception($"ZATCA Error: {errorDetails}");
                    }

                    // 7. Save successful submission
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string zatcaStatus = jsonResponse?.clearanceStatus;
                    ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                        invoiceNo,
                        requestResult.InvoiceRequest.Uuid,
                        hashResult.Hash,
                        requestResult.InvoiceRequest.Invoice,
                        zatcaStatus,
                        responseContent.ToString());

                    //ShowZatcaResponse(jsonResponse);
                    MessageBox.Show($"Successfully submitted to ZATCA:\n{jsonResponse}");
                    load_all_sales_grid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}");
                objSalesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        private void InsertQrIntoXml(XmlDocument xmlDoc, string qrBase64)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            // Remove any existing QR tag
            var existingQrNode = xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference[cbc:ID='QR']", ns);
            if (existingQrNode != null)
                existingQrNode.ParentNode.RemoveChild(existingQrNode);

            // Create QR node
            XmlElement qrDocRef = xmlDoc.CreateElement("cac", "AdditionalDocumentReference", ns.LookupNamespace("cac"));

            XmlElement id = xmlDoc.CreateElement("cbc", "ID", ns.LookupNamespace("cbc"));
            id.InnerText = "QR";

            XmlElement attachment = xmlDoc.CreateElement("cac", "Attachment", ns.LookupNamespace("cac"));
            XmlElement embedded = xmlDoc.CreateElement("cbc", "EmbeddedDocumentBinaryObject", ns.LookupNamespace("cbc"));
            embedded.SetAttribute("mimeCode", "text/plain");
            embedded.InnerText = qrBase64;

            attachment.AppendChild(embedded);
            qrDocRef.AppendChild(id);
            qrDocRef.AppendChild(attachment);

            // Find the PIH node
            XmlNode pihNode = xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference[cbc:ID='PIH']", ns);

            if (pihNode != null && pihNode.ParentNode != null)
            {
                // Insert after PIH
                pihNode.ParentNode.InsertAfter(qrDocRef, pihNode);
            }
            else
            {
                // fallback: insert before first InvoiceLine
                XmlNode invoiceLine = xmlDoc.SelectSingleNode("//cac:InvoiceLine", ns);
                if (invoiceLine != null)
                {
                    invoiceLine.ParentNode.InsertBefore(qrDocRef, invoiceLine);
                }
                else
                {
                    // fallback to root
                    xmlDoc.DocumentElement.AppendChild(qrDocRef);
                }
            }
        }

        public void InsertInvoiceHashToSignedXml(XmlDocument signedXml, string hashBase64)
        {
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(signedXml.NameTable);
            nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            XmlNode invoiceNode = signedXml.SelectSingleNode("//cac:AccountingSupplierParty", nsMgr);
            if (invoiceNode == null) throw new Exception("Unable to find insertion point for PIH block.");

            // Remove any existing PIH tag
            var existingQrNode = signedXml.SelectSingleNode("//cac:AdditionalDocumentReference[cbc:ID='PIH']", nsMgr);
            if (existingQrNode != null)
                existingQrNode.ParentNode.RemoveChild(existingQrNode);

            // Create AdditionalDocumentReference for PIH
            XmlElement docRef = signedXml.CreateElement("cac", "AdditionalDocumentReference", nsMgr.LookupNamespace("cac"));

            XmlElement id = signedXml.CreateElement("cbc", "ID", nsMgr.LookupNamespace("cbc"));
            id.InnerText = "PIH";
            docRef.AppendChild(id);

            XmlElement attachment = signedXml.CreateElement("cac", "Attachment", nsMgr.LookupNamespace("cac"));
            XmlElement embeddedObj = signedXml.CreateElement("cbc", "EmbeddedDocumentBinaryObject", nsMgr.LookupNamespace("cbc"));
            embeddedObj.SetAttribute("mimeCode", "text/plain");
            embeddedObj.InnerText = hashBase64;
            attachment.AppendChild(embeddedObj);

            docRef.AppendChild(attachment);

            // Insert after ICV block (if exists), or after last AdditionalDocumentReference
            var allDocRefs = signedXml.SelectNodes("//cac:AdditionalDocumentReference", nsMgr);
            XmlNode lastDocRef = allDocRefs[allDocRefs.Count - 1];
            lastDocRef.ParentNode.InsertAfter(docRef, lastDocRef);
        }

        public static string GetBase64QrCode(SignResult signResult)
        {
            XmlDocument xmlDoc = new XmlDocument() { PreserveWhitespace = true };
            xmlDoc.LoadXml(signResult.SignedEInvoice.OuterXml);


            XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsManager.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            // Find the QR code <cbc:EmbeddedDocumentBinaryObject>
            XmlNode qrNode = xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference[cbc:ID='QR']//cbc:EmbeddedDocumentBinaryObject", nsManager);
            return qrNode.InnerText.ToString();
        }
        public static string GetInvoiceHash(SignResult signResult)
        {
            var step = signResult.Steps
                .FirstOrDefault(s => s.StepName == "Generate EInvoice Hash");
            return step?.ResultedValue ?? "Invoice Hash not found";
        }
        public static string GetInvoiceTypeCode(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
            return salesBLL.GetInvoiceTypeCode(invoiceNo);
        }
        public static void ShowSignResult(SignResult signResult)
        {
            StringBuilder resultMessage = new StringBuilder();

            foreach (var step in signResult.Steps)
            {
                resultMessage.AppendLine($"Step: {step.StepName}");
                resultMessage.AppendLine($"  Status: {(step.IsValid ? "Valid" : "Invalid")}");

                if (step.ErrorMessages.Any())
                {
                    resultMessage.AppendLine("  Errors:");
                    foreach (var error in step.ErrorMessages)
                    {
                        resultMessage.AppendLine($"    - {error}");
                    }
                }

                if (step.WarningMessages.Any())
                {
                    resultMessage.AppendLine("  Warnings:");
                    foreach (var warning in step.WarningMessages)
                    {
                        resultMessage.AppendLine($"    - {warning}");
                    }
                }

                resultMessage.AppendLine(); // blank line between steps
            }

            resultMessage.AppendLine($"Overall Sign Result: {(signResult.IsValid ? "Valid" : "Invalid")}");

            // Show all at once in a message box
            MessageBox.Show(resultMessage.ToString(), "Sign Result", MessageBoxButtons.OK,
                signResult.IsValid ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        public void ShowZatcaResponse(dynamic response)
        {
            var info = response?.validationResults?.infoMessages;
            var warnings = response?.validationResults?.warningMessages;
            var errors = response?.validationResults?.errorMessages;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("✅ Invoice submitted to ZATCA successfully.\n");

            if (info != null)
            {
                sb.AppendLine("ℹ️ Info Messages:");
                foreach (var msg in info)
                {
                    sb.AppendLine($"- {msg.code}: {msg.message}");
                }
            }

            if (warnings != null && warnings.Count > 0)
            {
                sb.AppendLine("\n⚠️ Warnings:");
                foreach (var msg in warnings)
                {
                    sb.AppendLine($"- {msg.code}: {msg.message}");
                }
            }

            if (errors != null && errors.Count > 0)
            {
                sb.AppendLine("\n❌ Errors:");
                foreach (var msg in errors)
                {
                    sb.AppendLine($"- {msg.code}: {msg.message}");
                }
            }

            MessageBox.Show(sb.ToString(), "ZATCA Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DownloadUBL(string invoiceNo)
        {
            string path = objSalesBLL.GetUblPath(invoiceNo);
            if (File.Exists(path))
                Process.Start("explorer.exe", "/select,\"" + path + "\"");
            else
                MessageBox.Show("UBL XML file not found.");
        }
        private XmlDocument LoadSignedXMLInvoice(string invoiceNo)
        {
            string path = Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");

            if (!File.Exists(path))
                throw new FileNotFoundException("XML/UBL invoice not found.");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true; // important for signed XML
            xmlDoc.Load(path); // this reads the file and parses it into an XmlDocument
            return xmlDoc;
        }

        private string GetPublicKeyFromFile()
        {
            string path = Path.Combine(Application.StartupPath, "certs", "cert.pem");
            if (!File.Exists(path)) throw new FileNotFoundException("Public certificate not found.");
            return File.ReadAllText(path).Trim();
        }

        private string GetPrivateKeyFromFile()
        {
            string path = Path.Combine(Application.StartupPath, "certs", "private_key.pem");
            if (!File.Exists(path)) throw new FileNotFoundException("Private key not found.");
            return File.ReadAllText(path).Trim();
        }
        private string GetSecretFromFile()
        {
            string path = Path.Combine(Application.StartupPath, "certs", "secret.txt");
            if (!File.Exists(path)) throw new FileNotFoundException("Secret certificate not found.");
            return File.ReadAllText(path).Trim();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString());

        }

        private void BtnCustomerNameChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_all_sales.Rows.Count > 0)
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                    pos.Customers.CustomerNameChange customerNameChange = new Customers.CustomerNameChange(invoiceNo);
                    customerNameChange.ShowDialog();
                    load_all_sales_grid();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void BtnUBLInvoice_Click(object sender, EventArgs e)
        {
            string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
            Sales.EInvoice eInvoice = new Sales.EInvoice();
            string xmlContent = eInvoice.CreateUBLInvoice(invoiceNo);

            SaveXmlToFile(xmlContent, invoiceNo);
        }
        private void SaveXmlToFile(string xmlContent, string invoiceNo)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                saveFileDialog.Title = "Save UBL XML File";
                saveFileDialog.FileName = invoiceNo + ".xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Save the XML content to the selected file
                    File.WriteAllText(saveFileDialog.FileName, xmlContent);
                    System.Windows.MessageBox.Show("XML file saved successfully.");
                }
            }
        }

        private async void btnSendWhatsApp_Click(object sender, EventArgs e)
        {
            //SendInvoiceToWhatsApp();
            string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
            if (string.IsNullOrEmpty(invoiceNo))
            {
                MessageBox.Show("Please select a valid invoice.");
                return;
            }
            frm_send_whatsapp _frm_Send_Whatsapp = new frm_send_whatsapp(invoiceNo);
            _frm_Send_Whatsapp.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmPOS frmPOS = new frmPOS();
            frmPOS.ShowDialog();
        }

        private void Btn_PrintPOS80_Click(object sender, EventArgs e)
        {
            if (grid_all_sales.Rows.Count > 0)
            {
                string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(), false, false, true))
                {
                    obj.ShowDialog();
                }
            }
        }
    }
}
