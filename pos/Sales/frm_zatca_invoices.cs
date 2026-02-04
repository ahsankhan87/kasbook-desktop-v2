using com.sun.corba.se.spi.ior;
using com.sun.security.ntlm;
using java.security;
using java.util;
using Newtonsoft.Json;
using pos.Master.Companies.zatca;
using pos.UI;
using pos.UI.Busy;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;
using pos.Security.Authorization;
using AppPermissions = pos.Security.Authorization.Permissions;

namespace pos.Sales
{
    public partial class frm_zatca_invoices : Form
    {
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_zatca_invoices()
        {
            InitializeComponent();

        }

        private void frm_zatca_invoices_Load(object sender, EventArgs e)
        {
            StatusDDL();
            InvoiceSubTypeDDL();
            InvoiceTypeDDL();

            // keep action buttons in sync with current selection
            gridZatcaInvoices.SelectionChanged += gridZatcaInvoices_SelectionChanged;

            LoadZatcaInvoices();
            UpdateActionButtonsForSelection();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadZatcaInvoices();
            UpdateActionButtonsForSelection();
        }

        private void LoadZatcaInvoices()
        {
            using (BusyScope.Show(this, UiMessages.T("Loading invoices...", "جاري تحميل الفواتير...")))
            {
                try
                {
                    // Example: get all invoices with ZATCA status from your DB
                    gridZatcaInvoices.AutoGenerateColumns = false;

                    string invoiceNo = txtInvoiceNo.Text.Trim();
                    string type = cmbType.SelectedValue?.ToString();
                    string subtype = cmbSubtype.SelectedValue?.ToString();
                    string status = cmb_status.SelectedValue?.ToString();
                    DateTime? fromdate = dtpFromDate.Checked ? dtpFromDate.Value.Date : (DateTime?)null;
                    DateTime? todate = dtpToDate.Checked ? dtpToDate.Value.Date : (DateTime?)null;
                    bool showZatcaSkipInvoice = chk_ShowZatcaInvoice.Checked;

                    DataTable dt = new SalesBLL().SearchInvoices(invoiceNo, fromdate, type, subtype, todate, status, showZatcaSkipInvoice);
                    gridZatcaInvoices.DataSource = dt;
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
                }
                finally
                {
                    UpdateActionButtonsForSelection();
                }
            }
        }

        private void gridZatcaInvoices_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtonsForSelection();
        }

        private void UpdateActionButtonsForSelection()
        {
            // Default: disable both until we can clearly determine subtype
            btn_invoice_report.Enabled = false;
            btn_Invoice_clearance.Enabled = false;

            // Apply permissions for other buttons (independent of subtype)
            btn_signInvoice.Enabled = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Sign);
            btn_viewQR.Enabled = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Qr_Show);
            btnDownloadUBL.Enabled = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_DownloadUBL);
            btnComplianceChecks.Enabled = _auth.HasAny(_currentUser, AppPermissions.Sales_Zatca_Transmit, AppPermissions.Sales_Zatca_View);

            if (gridZatcaInvoices.CurrentRow == null)
                return;

            var subtypeObj = gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value;
            var subtype = Convert.ToString(subtypeObj);
            if (string.IsNullOrWhiteSpace(subtype))
                return;

            // Support both "01/02" codes and "standard/simplified" labels if returned by query
            var normalized = subtype.Trim().ToLowerInvariant();

            var isStandard = normalized == "01" || normalized == "standard";
            var isSimplified = normalized == "02" || normalized == "simplified";

            if (isStandard)
            {
                // Standard => Clearance only (if permitted)
                btn_Invoice_clearance.Enabled = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Clear);
                btn_invoice_report.Enabled = false;
            }
            else if (isSimplified)
            {
                // Simplified => Reporting only (if permitted)
                btn_invoice_report.Enabled = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Report);
                btn_Invoice_clearance.Enabled = false;
            }
        }

        private async void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridZatcaInvoices.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice from the list.",
                        "يرجى اختيار فاتورة من القائمة.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                if (gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value == null ||
                    string.IsNullOrWhiteSpace(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString()))
                {
                    UiMessages.ShowWarning(
                        "The selected row does not contain a valid invoice number.",
                        "السطر المحدد لا يحتوي على رقم فاتورة صحيح.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                string invoiceNo = gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value.ToString();

                btnComplianceChecks.Enabled = false;
                btnComplianceChecks.Text = UiMessages.T("Checking...", "جارٍ التحقق...");
                btnComplianceChecks.Cursor = Cursors.WaitCursor;
                btnComplianceChecks.Refresh();

                await ZatcaInvoiceComplianceChecks(invoiceNo);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
            finally
            {
                btnComplianceChecks.Enabled = true;
                btnComplianceChecks.Text = UiMessages.T("Compliance checks", "فحص المطابقة");
                btnComplianceChecks.Cursor = Cursors.Default;
            }
        }

        private async void NewComplianceCheckButton_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null)
            {
                UiMessages.ShowWarning(
                    "Please select an invoice from the list.",
                    "يرجى اختيار فاتورة من القائمة.",
                    "ZATCA",
                    "زاتكا");
                return;
            }

            var invoiceNo = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value);
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                UiMessages.ShowWarning(
                    "The selected row does not contain a valid invoice number.",
                    "السطر المحدد لا يحتوي على رقم فاتورة صحيح.",
                    "ZATCA",
                    "زاتكا");
                return;
            }

            await ZatcaInvoiceReporting_NEWAsync(invoiceNo);
        }

        public async Task ZatcaInvoiceReporting_NEWAsync(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
            try
            {
                if (!UsersModal.useZatcaEInvoice)
                {
                    UiMessages.ShowWarning(
                        "ZATCA E-Invoicing is not enabled in system settings.",
                        "ميزة الفوترة الإلكترونية (زاتكا) غير مفعلة في إعدادات النظام.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Preparing invoice for reporting...", "جارٍ تجهيز الفاتورة للإرسال...")))
                {
                    // 1️⃣ Generate unsigned XML
                    XmlDocument ublXml = ZatcaHelper.GenerateUBLXMLInvoice(invoiceNo);

                    // 2️⃣ Read Issue Date & Time
                    XmlNamespaceManager ns = new XmlNamespaceManager(ublXml.NameTable);
                    ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

                    string issueDate = ublXml.SelectSingleNode("//cbc:IssueDate", ns)?.InnerText;
                    string issueTime = ublXml.SelectSingleNode("//cbc:IssueTime", ns)?.InnerText;

                    if (string.IsNullOrEmpty(issueDate) || string.IsNullOrEmpty(issueTime))
                        throw new Exception("IssueDate or IssueTime is missing in the generated XML.");

                    // Get invoice data for QR code
                    DataSet ds = salesBLL.GetSaleAndItemsDataSet(invoiceNo);
                    DataRow invoice = ds.Tables["Sale"].Rows[0];

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

                    // Load ZATCA credentials
                    DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                    if (activeZatcaCredential == null)
                    {
                        UiMessages.ShowError(
                            "No active ZATCA credentials found. Please configure CSID/PCSID first.",
                            "لا توجد بيانات اعتماد زاتكا نشطة. يرجى إعداد CSID/PCSID أولاً.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }

                    DataRow PCSID = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                    if (PCSID == null)
                    {
                        UiMessages.ShowError(
                            "No PCSID credentials found under the active CSID. Please configure PCSID first.",
                            "لا توجد بيانات PCSID تحت CSID النشط. يرجى إعداد PCSID أولاً.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }

                    string env = activeZatcaCredential["mode"].ToString();
                    string privateKey = activeZatcaCredential["private_key"].ToString();
                    string certBase64 = PCSID["cert_base64"].ToString();
                    string PCSIDCertificate = PCSID["cert_base64"].ToString();
                    string secret = PCSID["secret_key"].ToString();
                    string PCSID_authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PCSIDCertificate}:{secret}"));

                    string decodedCert = Encoding.UTF8.GetString(Convert.FromBase64String(certBase64));

                    // SIGN XML
                    var signer = new EInvoiceSigner();
                    SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey);
                    if (!signResult.IsValid)
                        throw new Exception(signResult.ErrorMessage);

                    var hashGen = new EInvoiceHashGenerator();
                    string invoiceHashBase64 = hashGen.GenerateEInvoiceHashing(signResult.SignedEInvoice).Hash;

                    var sigData = ZatcaHelper.ExtractSignatureData(signResult.SignedEInvoice, PCSIDCertificate);

                    // Generate ZATCA-SAFE QR
                    string qrBase64 = ZatcaPhase2QrGenerator.GenerateQrBase64(
                        sellerName,
                        sellerVAT,
                        issueDate,
                        issueTime,
                        totalWithVat,
                        vatAmount,
                        invoiceHashBase64,
                        sigData.SignatureValueBase64,
                        sigData.PublicKeyBase64,
                        sigData.CertificateSignatureBase64
                    );

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase641 = qrResult.QR;

                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase641);

                    // Save signed XML
                    string ublFolder = Path.Combine(Application.StartupPath, "UBL");
                    Directory.CreateDirectory(ublFolder);

                    string path = Path.Combine(ublFolder, invoiceNo + "_signed.xml");
                    signResult.SignedEInvoice.Save(path);

                    // Save QR
                    salesBLL.UpdateZatcaQrCode(invoiceNo, Convert.FromBase64String(qrBase64));
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", path, null);

                    // Generate request payload
                    var requestGenerator = new RequestGenerator();
                    var requestResult = requestGenerator.GenerateRequest(signResult.SignedEInvoice);

                    object requestBody = new
                    {
                        invoiceHash = invoiceHashBase64,
                        uuid = requestResult.InvoiceRequest.Uuid,
                        invoice = requestResult.InvoiceRequest.Invoice
                    };

                    var response = await ZatcaHelper.CallSingleInvoiceReportingAsync(requestBody, PCSID_authorization, env);
                    if (string.IsNullOrEmpty(response))
                    {
                        UiMessages.ShowError(
                            "No response was received from ZATCA. Please try again.",
                            "لم يتم استلام أي رد من زاتكا. يرجى المحاولة مرة أخرى.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }

                    var responseContent = JsonConvert.DeserializeObject<dynamic>(response);
                    string zatcaStatus = responseContent?.reportingStatus;

                    if (string.IsNullOrEmpty(zatcaStatus))
                    {
                        UiMessages.ShowError(
                            "ZATCA response does not contain a reporting status.",
                            "رد زاتكا لا يحتوي على حالة الإرسال.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }

                    ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                        invoiceNo,
                        requestResult.InvoiceRequest.Uuid,
                        invoiceHashBase64,
                        requestResult.InvoiceRequest.Invoice,
                        zatcaStatus,
                        env,
                        responseContent.ToString()
                    );

                    UiMessages.ShowInfo(
                        $"Invoice {invoiceNo} was processed successfully. ZATCA Status: {zatcaStatus}",
                        $"تمت معالجة الفاتورة {invoiceNo} بنجاح. حالة زاتكا: {zatcaStatus}",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        public static string ExtractEcdsaSignature(XmlDocument signedXml)
        {
            try
            {
                XmlNamespaceManager ns = new XmlNamespaceManager(signedXml.NameTable);
                ns.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                ns.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.AddNamespace("sig", "urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2");
                ns.AddNamespace("sac", "urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2");

                // XPath: /Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sig:UBLDocumentSignatures/sac:SignatureInformation/ds:Signature/ds:SignatureValue
                XmlNode signatureNode = signedXml.SelectSingleNode(
                    "//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sig:UBLDocumentSignatures/sac:SignatureInformation/ds:Signature/ds:SignatureValue", ns);

                if (signatureNode != null)
                {
                    return signatureNode.InnerText.Trim();
                }

                // Alternative XPath (without sig: prefix)
                signatureNode = signedXml.SelectSingleNode(
                    "//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/*[local-name()='UBLDocumentSignatures']/*[local-name()='SignatureInformation']/ds:Signature/ds:SignatureValue", ns);

                return signatureNode?.InnerText.Trim() ?? "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting ECDSA signature: {ex.Message}");
                return "";
            }
        }

        public static string ExtractEcdsaPublicKeyThumbprint(XmlDocument signedXml)
        {
            try
            {
                XmlNamespaceManager ns = new XmlNamespaceManager(signedXml.NameTable);
                ns.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                ns.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

                // Extract the full certificate
                XmlNode certNode = signedXml.SelectSingleNode(
                    "//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/*[local-name()='UBLDocumentSignatures']/*[local-name()='SignatureInformation']/ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", ns);

                if (certNode != null)
                {
                    string cert = certNode.InnerText.Trim();
                    cert = Regex.Replace(cert, @"\s+", "");

                    // Generate SHA-256 thumbprint
                    return ZATCAQRCodeGenerator.GenerateCertificateThumbprint(cert);
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting ECDSA public key thumbprint: {ex.Message}");
                return "";
            }
        }

        public static string ExtractZatcaCertificateThumbprint(XmlDocument signedXml)
        {
            try
            {
                // Check if this is a simplified invoice
                XmlNamespaceManager ns = new XmlNamespaceManager(signedXml.NameTable);
                ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

                XmlNode invoiceTypeNode = signedXml.SelectSingleNode("//cbc:InvoiceTypeCode", ns);
                string invoiceType = invoiceTypeNode?.Attributes?["name"]?.Value;

                // For simplified invoices only
                if (invoiceType == "0100000" || invoiceType == "0200000")
                {
                    ns.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                    ns.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

                    // Extract ZATCA's CA certificate (second certificate in chain)
                    XmlNodeList certNodes = signedXml.SelectNodes(
                        "//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/*[local-name()='UBLDocumentSignatures']/*[local-name()='SignatureInformation']/ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", ns);

                    if (certNodes != null && certNodes.Count > 1)
                    {
                        string zatcaCert = certNodes[1].InnerText.Trim();
                        zatcaCert = Regex.Replace(zatcaCert, @"\s+", "");

                        // Generate SHA-256 thumbprint
                        return ZATCAQRCodeGenerator.GenerateCertificateThumbprint(zatcaCert);
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting ZATCA certificate thumbprint: {ex.Message}");
                return "";
            }
        }

        private async Task ZatcaInvoiceComplianceChecks(string invoiceNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceNo)) return;

                using (BusyScope.Show(this, UiMessages.T("Running compliance checks...", "جارٍ تنفيذ فحوصات المطابقة...")))
                {
                    DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                    if (activeZatcaCredential == null)
                    {
                        UiMessages.ShowWarning(
                            "No active ZATCA credentials found. Please configure CSID first.",
                            "لا توجد بيانات اعتماد زاتكا نشطة. يرجى إعداد CSID أولاً.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }
                    string env = activeZatcaCredential["mode"].ToString();

                    if (string.IsNullOrEmpty(env))
                    {
                        UiMessages.ShowWarning(
                            "No active ZATCA environment is configured.",
                            "لم يتم تحديد بيئة زاتكا النشطة.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }

                    XmlDocument ublXml = ZatcaHelper.LoadSignedXMLInvoice(invoiceNo);

                    var requestGenerator = new RequestGenerator();
                    var requestResult = requestGenerator.GenerateRequest(ublXml);
                    if (!requestResult.IsValid)
                    {
                        UiMessages.ShowError(
                            "Failed to generate compliance request: " + requestResult.ErrorMessages,
                            "فشل إنشاء طلب المطابقة: " + requestResult.ErrorMessages,
                            "ZATCA",
                            "زاتكا");
                        return;
                    }

                    string cert = activeZatcaCredential["cert_base64"].ToString();
                    string secret = activeZatcaCredential["secret_key"].ToString();
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
                        string zatcaStatus = (GetInvoiceTypeCode(invoiceNo) == "01" ? jsonResponse?.clearanceStatus : jsonResponse?.reportingStatus);

                        UiMessages.ShowInfo(
                            $"ZATCA status for invoice {invoiceNo}: {zatcaStatus}",
                            $"حالة زاتكا للفاتورة {invoiceNo}: {zatcaStatus}",
                            captionEn: "ZATCA",
                            captionAr: "زاتكا");

                        // Optional: show raw response (kept but professional label)
                        MessageBox.Show($"{UiMessages.T("ZATCA Response", "رد زاتكا")}:\n{jsonResponse}", UiMessages.T("ZATCA", "زاتكا"), MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error submitting to ZATCA:\n{ex.Message}",
                    $"حدث خطأ أثناء الإرسال إلى زاتكا:\n{ex.Message}",
                    "ZATCA",
                    "زاتكا");
            }
        }

        private void btnViewResponse_Click(object sender, EventArgs e)
        {
            if (gridZatcaInvoices.CurrentRow == null)
            {
                UiMessages.ShowWarning(
                    "Please select an invoice first.",
                    "يرجى اختيار فاتورة أولاً.",
                    "ZATCA",
                    "زاتكا");
                return;
            }

            string response = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["zatca_message"].Value);
            if (string.IsNullOrWhiteSpace(response))
            {
                UiMessages.ShowInfo(
                    "No response details are available for the selected invoice.",
                    "لا توجد تفاصيل رد متاحة للفاتورة المحددة.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
                return;
            }

            MessageBox.Show("\n" + response, UiMessages.T("ZATCA Response", "رد زاتكا"));
        }

        public static string GetInvoiceTypeCode(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
            return salesBLL.GetInvoiceTypeCode(invoiceNo);
        }

        private void btn_viewQR_Click(object sender, EventArgs e)
        {
            if (!_auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Qr_Show))
            {
                UiMessages.ShowWarning(
                    "You don't have permission to view QR code.",
                    "ليس لديك صلاحية لعرض رمز الاستجابة السريعة.",
                    "Permission Denied",
                    "تم رفض الصلاحية");
                return;
            }

            if (gridZatcaInvoices.CurrentRow == null)
            {
                UiMessages.ShowWarning(
                    "Please select an invoice first.",
                    "يرجى اختيار فاتورة أولاً.",
                    "ZATCA",
                    "زاتكا");
                return;
            }

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
                UiMessages.ShowInfo(
                    "No QR code is stored for the selected invoice.",
                    "لا يوجد رمز QR محفوظ للفاتورة المحددة.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }
        }

        private async void btn_signInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Sign))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to sign invoices for ZATCA.",
                        "ليس لديك صلاحية لتوقيع الفواتير لإرسالها إلى زاتكا.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }

                if (gridZatcaInvoices.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice first.",
                        "يرجى اختيار فاتورة أولاً.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                string invoiceNo = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value);
                string account = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["account"].Value);
                string prevInvoiceNo = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["prevInvoiceNo"].Value);
                DateTime prevSaleDate = (string.IsNullOrEmpty(Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value))
                    ? DateTime.Now
                    : Convert.ToDateTime(gridZatcaInvoices.CurrentRow.Cells["prevSaleDate"].Value));

                btn_signInvoice.Enabled = false;
                btn_signInvoice.Text = UiMessages.T("Processing...", "جارٍ المعالجة...");
                btn_signInvoice.Cursor = Cursors.WaitCursor;
                btn_signInvoice.Refresh();

                if (!UsersModal.useZatcaEInvoice)
                {
                    UiMessages.ShowWarning(
                        "ZATCA E-Invoicing is not enabled in system settings.",
                        "ميزة الفوترة الإلكترونية (زاتكا) غير مفعلة في إعدادات النظام.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    UiMessages.ShowError(
                        "No active ZATCA credentials found. Please configure CSID/PCSID first.",
                        "لا توجد بيانات اعتماد زاتكا نشطة. يرجى إعداد CSID/PCSID أولاً.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));

                if (PCSID_dataRow == null)
                {
                    // Sign using CSID
                    if (account != null && account.ToLower() == "sale")
                    {
                        ZatcaHelper.SignInvoiceToZatca(invoiceNo);
                    }
                    else if (account != null && account.ToLower() == "return")
                    {
                        ZatcaHelper.SignCreditNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
                    }
                    else if (account != null && account.ToLower() == "debit")
                    {
                        ZatcaHelper.SignDebitNoteToZatca(invoiceNo, prevInvoiceNo, prevSaleDate);
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Unsupported transaction type for signing.",
                            "نوع العملية غير مدعوم للتوقيع.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }
                }
                else
                {
                    // Sign using PCSID
                    if (account != null && account.ToLower() == "sale")
                    {
                        await ZatcaHelper.PCSID_SignInvoiceToZatcaAsync(invoiceNo);
                    }
                    else if (account != null && account.ToLower() == "return")
                    {
                        await ZatcaHelper.PCSID_SignCreditNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
                    }
                    else if (account != null && account.ToLower() == "debit")
                    {
                        await ZatcaHelper.PCSID_SignDebitNoteToZatcaAsync(invoiceNo, prevInvoiceNo, prevSaleDate);
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Unsupported transaction type for signing.",
                            "نوع العملية غير مدعوم للتوقيع.",
                            "ZATCA",
                            "زاتكا");
                        return;
                    }
                }

                UiMessages.ShowInfo(
                    $"Invoice {invoiceNo} was signed successfully.",
                    $"تم توقيع الفاتورة {invoiceNo} بنجاح.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
            finally
            {
                btn_signInvoice.Enabled = true;
                btn_signInvoice.Text = UiMessages.T("Sign Invoice", "توقيع الفاتورة");
                btn_signInvoice.Cursor = Cursors.Default;
            }

            LoadZatcaInvoices();
        }

        private async void btn_Invoice_clearance_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Clear))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to submit clearance to ZATCA.",
                        "ليس لديك صلاحية لإرسال الاعتماد إلى زاتكا.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }

                if (gridZatcaInvoices.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice first.",
                        "يرجى اختيار فاتورة أولاً.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                string invoiceNo = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value);
                string invoice_subtype = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value);

                if (invoice_subtype != null && invoice_subtype.ToLower() == "simplified")
                {
                    UiMessages.ShowInfo(
                        "Simplified invoices cannot be cleared. Please use Reporting instead.",
                        "لا يمكن إجراء الاعتماد (Clearance) للفواتير المبسطة. يرجى استخدام الإبلاغ (Reporting).",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                if (string.IsNullOrWhiteSpace(invoiceNo) || string.IsNullOrWhiteSpace(invoice_subtype))
                {
                    UiMessages.ShowWarning(
                        "Please ensure the selected invoice has complete details.",
                        "يرجى التأكد من اكتمال بيانات الفاتورة المحددة.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                btn_Invoice_clearance.Enabled = false;
                btn_Invoice_clearance.Text = UiMessages.T("Processing...", "جارٍ المعالجة...");
                btn_Invoice_clearance.Cursor = Cursors.WaitCursor;
                btn_Invoice_clearance.Refresh();

                await ZatcaHelper.ZatcaInvoiceClearanceAsync(invoiceNo);

                UiMessages.ShowInfo(
                    $"Clearance request submitted for invoice {invoiceNo}.",
                    $"تم إرسال طلب الاعتماد للفاتورة {invoiceNo}.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");

                LoadZatcaInvoices();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
            finally
            {
                btn_Invoice_clearance.Enabled = true;
                btn_Invoice_clearance.Text = UiMessages.T("Clearance", "اعتماد");
                btn_Invoice_clearance.Cursor = Cursors.Default;
            }
        }

        private async void btn_invoice_report_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Report))
                {
                    UiMessages.ShowWarning(
                        "You don't have permission to report invoices to ZATCA.",
                        "ليس لديك صلاحية لإرسال الفواتير (إبلاغ) إلى زاتكا.",
                        "Permission Denied",
                        "تم رفض الصلاحية");
                    return;
                }

                if (gridZatcaInvoices.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice first.",
                        "يرجى اختيار فاتورة أولاً.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                string invoiceNo = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value);
                string invoice_subtype = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value);

                if (invoice_subtype != null && invoice_subtype.ToLower() == "standard")
                {
                    UiMessages.ShowInfo(
                        "Standard invoices cannot be reported. Please use Clearance instead.",
                        "لا يمكن الإبلاغ (Reporting) للفواتير القياسية. يرجى استخدام الاعتماد (Clearance).",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                if (string.IsNullOrWhiteSpace(invoiceNo))
                {
                    UiMessages.ShowWarning(
                        "The selected row does not contain a valid invoice number.",
                        "السطر المحدد لا يحتوي على رقم فاتورة صحيح.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                btn_invoice_report.Enabled = false;
                btn_invoice_report.Text = UiMessages.T("Processing...", "جارٍ المعالجة...");
                btn_invoice_report.Cursor = Cursors.WaitCursor;
                btn_invoice_report.Refresh();

                await ZatcaHelper.ZatcaInvoiceReportingAsync(invoiceNo);

                UiMessages.ShowInfo(
                    $"Reporting request submitted for invoice {invoiceNo}.",
                    $"تم إرسال طلب الإبلاغ للفاتورة {invoiceNo}.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");

                LoadZatcaInvoices();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
            finally
            {
                btn_invoice_report.Enabled = true;
                btn_invoice_report.Text = UiMessages.T("Reporting", "إبلاغ");
                btn_invoice_report.Cursor = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Searching...", "جارٍ البحث...")))
            {
                try
                {
                    string invoiceNo = txtInvoiceNo.Text.Trim();
                    string type = cmbType.SelectedValue?.ToString();
                    string subtype = cmbSubtype.SelectedValue?.ToString();
                    string status = cmb_status.SelectedValue?.ToString();
                    DateTime? fromdate = dtpFromDate.Checked ? dtpFromDate.Value.Date : (DateTime?)null;
                    DateTime? todate = dtpToDate.Checked ? dtpToDate.Value.Date : (DateTime?)null;
                    bool showZatcaSkipInvoice = chk_ShowZatcaInvoice.Checked;

                    var results = new SalesBLL().SearchInvoices(invoiceNo, fromdate, type, subtype, todate, status, showZatcaSkipInvoice);
                    gridZatcaInvoices.DataSource = results;
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Error while searching invoices: " + ex.Message,
                        "حدث خطأ أثناء البحث عن الفواتير: " + ex.Message,
                        "ZATCA",
                        "زاتكا");
                }
            }
        }

        private void btnDownloadUBL_Click(object sender, EventArgs e)
        {
            if (!_auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_DownloadUBL))
            {
                UiMessages.ShowWarning(
                    "You don't have permission to download UBL.",
                    "ليس لديك صلاحية لتنزيل ملف UBL.",
                    "Permission Denied",
                    "تم رفض الص صلاحية");
                return;
            }

            if (gridZatcaInvoices.CurrentRow == null)
            {
                UiMessages.ShowWarning(
                    "Please select an invoice to download.",
                    "يرجى اختيار فاتورة لتنزيلها.",
                    "ZATCA",
                    "زاتكا");
                return;
            }

            string invoiceNo = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_no"].Value);
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                UiMessages.ShowWarning(
                    "The selected invoice number is not valid.",
                    "رقم الفاتورة المحدد غير صالح.",
                    "ZATCA",
                    "زاتكا");
                return;
            }

            DownloadUBLInvoice(invoiceNo);
        }

        private void DownloadUBLInvoice(string invoiceNo)
        {
            try
            {
                if (gridZatcaInvoices.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice to download.",
                        "يرجى اختيار فاتورة لتنزيلها.",
                        "ZATCA",
                        "زاتكا");
                    return;
                }

                string vat = Convert.ToString(gridZatcaInvoices.CurrentRow.Cells["invoice_subtype"].Value);
                DateTime issueDate = Convert.ToDateTime(gridZatcaInvoices.CurrentRow.Cells["sale_time"].Value);
                string irn = Regex.Replace(invoiceNo, "[^a-zA-Z0-9]", "-");

                string datePart = issueDate.ToString("yyyyMMdd");
                string timePart = issueDate.ToString("HHmmss");
                string fileName = $"{vat}_{datePart}T{timePart}_{irn}.xml";

                XmlDocument ublXmlDoc = GetUBLXmlByInvoiceNo(invoiceNo);

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "XML Files (*.xml)|*.xml";
                    sfd.FileName = fileName;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ublXmlDoc.Save(sfd.FileName);
                        UiMessages.ShowInfo(
                            "UBL file has been saved successfully.",
                            "تم حفظ ملف UBL بنجاح.",
                            captionEn: "ZATCA",
                            captionAr: "زاتكا");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                UiMessages.ShowError(
                    "Signed XML/UBL file was not found for the selected invoice.",
                    "لم يتم العثور على ملف XML/UBL الموقّع للفاتورة المحددة.",
                    "ZATCA",
                    "زاتكا");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
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
    }
}