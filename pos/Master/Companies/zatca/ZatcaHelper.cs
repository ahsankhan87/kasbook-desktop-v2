using Newtonsoft.Json;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;

namespace pos.Master.Companies.zatca
{
    public class ZatcaHelper
    {
        public static string GetZatcaUrl(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                throw new ArgumentException("Company ID cannot be null or empty.", nameof(companyId));
            }
            // Assuming the ZATCA URL format is something like this
            return $"https://zatca.gov.sa/{companyId}";
        }

        public static void SignInvoiceToZatca(string invoiceNo) //CSID Signing
        {
            SalesBLL salesBLL = new SalesBLL();
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
                    MessageBox.Show("No active ZATCA CSID/credentials found. Please configure them first.");
                    return;
                }

                // get CSID credentials from DB
                string cert = activeZatcaCredential["cert_base64"].ToString(); // ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetPublicKeyFromFile();
                string secret = activeZatcaCredential["secret_key"].ToString(); //  ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetSecretFromFile();
                string privateKey = activeZatcaCredential["private_key"].ToString(); // ZatcaInvoiceGenerator.GetPrivateKeyFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString());  //GetPrivateKeyFromFile();

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
                    var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

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
                    var Base64QrCode = ZatcaHelper.GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }
        public static void PCSID_SignCreditToZatcaAsync(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
        {
            SalesBLL salesBLL = new SalesBLL();
            try
            {
                if (UsersModal.useZatcaEInvoice == false)
                {
                    MessageBox.Show("ZATCA E-Invoice is not enabled for this branch. Please enable it in profile/settings.");
                    return;
                }
                // 1. Get sale data
                XmlDocument ublXml = GenerateUBLXMLCreditNote(invoiceNo, previousInvoiceNo, previousInvoiceDate);
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
                    var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

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
                    var Base64QrCode = ZatcaHelper.GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        public static void SignDebitNoteToZatca(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
        {
            SalesBLL salesBLL = new SalesBLL();
            try
            {
                if (UsersModal.useZatcaEInvoice == false)
                {
                    MessageBox.Show("ZATCA E-Invoice is not enabled for this branch. Please enable it in profile/settings.");
                    return;
                }
                // 1. Get sale data
                XmlDocument ublXml = GenerateUBLXMLDebitNote(invoiceNo, previousInvoiceNo, previousInvoiceDate);
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
                    var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

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
                    var Base64QrCode = ZatcaHelper.GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        // Change the method signature to static
        public static XmlDocument GenerateUBLXMLInvoice(string invoiceNo)
        {
            // 1. Generate UBL XML 
            SalesBLL salesBLL = new SalesBLL();
            DataSet ds = salesBLL.GetSaleAndItemsDataSet(invoiceNo);
            //XmlDocument ublXml = pos.Master.Companies.zatca.ZatcaHelper.BuildUblXml(ds);

            // Create generator instance
            var generator = new ZatcaInvoiceGenerator();

            // Generate XML document
            XmlDocument ublXml = generator.GenerateZatcaInvoiceXmlDocument(ds, invoiceNo);

            return ublXml;
        }
        public static XmlDocument GenerateUBLXMLCreditNote(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
        {
            // 1. Generate UBL XML 
            SalesBLL salesBLL = new SalesBLL();
            DataSet ds = salesBLL.GetSaleAndItemsDataSet(invoiceNo);
            //XmlDocument ublXml = pos.Master.Companies.zatca.ZatcaHelper.BuildUblXml(ds);

            // Create generator instance
            var generator = new ZatcaInvoiceGenerator();

            // Generate XML document
            XmlDocument ublXml = generator.GenerateZatcaCreditNoteXmlDocument(ds, invoiceNo, previousInvoiceNo, previousInvoiceDate);

            return ublXml;
        }
        public static XmlDocument GenerateUBLXMLDebitNote(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
        {
            // 1. Generate UBL XML 
            SalesBLL salesBLL = new SalesBLL();
            DataSet ds = salesBLL.GetSaleAndItemsDataSet(invoiceNo);
            //XmlDocument ublXml = pos.Master.Companies.zatca.ZatcaHelper.BuildUblXml(ds);

            // Create generator instance
            var generator = new ZatcaInvoiceGenerator();

            // Generate XML document
            XmlDocument ublXml = generator.GenerateZatcaDebitNoteXmlDocument(ds, invoiceNo, previousInvoiceNo, previousInvoiceDate);

            return ublXml;
        }
        /// <summary>
        /// Calls the ZATCA single invoice production clearance API.
        /// </summary>
        /// <param name="requestBody">The request object to be serialized as JSON.</param>
        /// <param name="base64Credentials">Base64 encoded "cert:secret" string.</param>
        /// <returns>API response as string.</returns>
        public static async Task<string> CallSingleInvoiceClearanceAsync(object requestBody, string base64Credentials,string env)
        {
            string url;
            switch (env)
            {
                case "Production":
                    url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/core/invoices/clearance/single";
                    break;
                case "Simulation":
                    url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation/invoices/clearance/single";
                    break;
                default:
                    url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/invoices/clearance/single";
                    break;
            }

            //const string url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation/invoices/clearance/single";
            //const string url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/invoices/clearance/single";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                client.DefaultRequestHeaders.Add("Accept-Language", "EN");

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"ZATCA API Error: {response.StatusCode} - {responseContent}");
                }

                return responseContent;
            }
        }

        /// <summary>
        /// Calls the ZATCA single invoice reporing API.
        /// </summary>
        /// <param name="requestBody">The request object to be serialized as JSON.</param>
        /// <param name="base64Credentials">Base64 encoded "cert:secret" string.</param>
        /// <returns>API response as string.</returns>
        public static async Task<string> CallSingleInvoiceReportingAsync(object requestBody, string base64Credentials,string env)
        {
            string url;
            switch (env)
            {
                case "Production":
                    url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/core/invoices/reporting/single";
                    break;
                case "Simulation":
                    url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation/invoices/reporting/single";
                    break;
                default:
                    url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/invoices/reporting/single";
                    break;
            }

            //const string url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/invoices/reporting/single";
            //const string url = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation/invoices/reporting/single";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                client.DefaultRequestHeaders.Add("Accept-Language", "EN");

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"ZATCA API Error: {response.StatusCode} - {responseContent}");
                }

                return responseContent;
            }
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
        public static void InsertInvoiceHashToSignedXml(XmlDocument signedXml, string hashBase64)
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
        public static void InsertQrIntoXml(XmlDocument xmlDoc, string qrBase64)
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

       
        public static string GetInvoiceHash(SignResult signResult)
        {
            var step = signResult.Steps
                .FirstOrDefault(s => s.StepName == "Generate EInvoice Hash");
            return step?.ResultedValue ?? "Invoice Hash not found";
        }

        public static async Task PCSID_SignInvoiceToZatcaAsync(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
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
                //string cert_CSID = activeZatcaCredential["cert_base64"].ToString(); // ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetPublicKeyFromFile();
                //string secret_CSID = activeZatcaCredential["secret_key"].ToString(); //ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetSecretFromFile();
                string privateKey_CSID = activeZatcaCredential["private_key"].ToString(); //ZatcaInvoiceGenerator.GetPrivateKeyFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString());  //GetPrivateKeyFromFile();
               // string complainceRequestID = activeZatcaCredential["compliance_request_id"].ToString(); //ZatcaInvoiceGenerator.GetComplainceRequestIDFromDb(UsersModal.logged_in_branch_id, _env);
               // string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert_CSID}:{secret_CSID}"));

                // Retrieve PCSID credentials from the database using the credentialId
                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                if (PCSID_dataRow == null)
                {
                    MessageBox.Show("No PCSID credentials found for the selected ZATCA CSID.");
                    return;
                }

                string PCISD_cert = PCSID_dataRow["cert_base64"].ToString();
                ////

                byte[] bytes = Convert.FromBase64String(PCISD_cert);
                string decodedCert = Encoding.UTF8.GetString(bytes);

                //XmlDocument ublXml = LoadSampleUBL();
                //ublXml.Save("UBL\\debug_ubl1.xml");

                var signer = new EInvoiceSigner();
                SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey_CSID);

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
                    var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

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
                    var Base64QrCode = ZatcaHelper.GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        public static async Task PCSID_SignCreditNoteToZatcaAsync(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
        {
            SalesBLL salesBLL = new SalesBLL();
            try
            {
                if (UsersModal.useZatcaEInvoice == false)
                {
                    MessageBox.Show("ZATCA E-Invoice is not enabled for this branch. Please enable it in profile/settings.");
                    return;
                }
                // 1. Get sale data
                XmlDocument ublXml = GenerateUBLXMLCreditNote(invoiceNo, previousInvoiceNo, previousInvoiceDate);
                //ublXml.Save("UBL\\unsigned_ubl_"+ invoiceNo + ".xml");

                // Check if ZATCA credentials are configured
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID found. Please configure them first.");
                    return;
                }

                // 3. Sign XML
                //string cert_CSID = activeZatcaCredential["cert_base64"].ToString(); // ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetPublicKeyFromFile();
                //string secret_CSID = activeZatcaCredential["secret_key"].ToString(); //ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetSecretFromFile();
                string privateKey_CSID = activeZatcaCredential["private_key"].ToString(); //ZatcaInvoiceGenerator.GetPrivateKeyFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString());  //GetPrivateKeyFromFile();
                                                                                          // string complainceRequestID = activeZatcaCredential["compliance_request_id"].ToString(); //ZatcaInvoiceGenerator.GetComplainceRequestIDFromDb(UsersModal.logged_in_branch_id, _env);
                                                                                          // string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert_CSID}:{secret_CSID}"));

                // Retrieve PCSID credentials from the database using the credentialId
                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                if (PCSID_dataRow == null)
                {
                    MessageBox.Show("No PCSID credentials found for the selected ZATCA CSID.");
                    return;
                }

                string PCISD_cert = PCSID_dataRow["cert_base64"].ToString();
                ////

                byte[] bytes = Convert.FromBase64String(PCISD_cert);
                string decodedCert = Encoding.UTF8.GetString(bytes);

                //XmlDocument ublXml = LoadSampleUBL();
                //ublXml.Save("UBL\\debug_ubl1.xml");

                var signer = new EInvoiceSigner();
                SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey_CSID);

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
                    var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

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
                    var Base64QrCode = ZatcaHelper.GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }

        public static async Task PCSID_SignDebitNoteToZatcaAsync(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
        {
            SalesBLL salesBLL = new SalesBLL();
            try
            {
                if (UsersModal.useZatcaEInvoice == false)
                {
                    MessageBox.Show("ZATCA E-Invoice is not enabled for this branch. Please enable it in profile/settings.");
                    return;
                }
                // 1. Get sale data
                XmlDocument ublXml = GenerateUBLXMLDebitNote(invoiceNo, previousInvoiceNo, previousInvoiceDate);
                //ublXml.Save("UBL\\unsigned_ubl_"+ invoiceNo + ".xml");

                // Check if ZATCA credentials are configured
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID found. Please configure them first.");
                    return;
                }

                // 3. Sign XML
                //string cert_CSID = activeZatcaCredential["cert_base64"].ToString(); // ZatcaInvoiceGenerator.GetCertFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetPublicKeyFromFile();
                //string secret_CSID = activeZatcaCredential["secret_key"].ToString(); //ZatcaInvoiceGenerator.GetSecretFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString()); // GetSecretFromFile();
                string privateKey_CSID = activeZatcaCredential["private_key"].ToString(); //ZatcaInvoiceGenerator.GetPrivateKeyFromDb(UsersModal.logged_in_branch_id, activeZatcaCredential["mode"].ToString());  //GetPrivateKeyFromFile();
                                                                                          // string complainceRequestID = activeZatcaCredential["compliance_request_id"].ToString(); //ZatcaInvoiceGenerator.GetComplainceRequestIDFromDb(UsersModal.logged_in_branch_id, _env);
                                                                                          // string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cert_CSID}:{secret_CSID}"));

                // Retrieve PCSID credentials from the database using the credentialId
                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                if (PCSID_dataRow == null)
                {
                    MessageBox.Show("No PCSID credentials found for the selected ZATCA CSID.");
                    return;
                }

                string PCISD_cert = PCSID_dataRow["cert_base64"].ToString();
                ////

                byte[] bytes = Convert.FromBase64String(PCISD_cert);
                string decodedCert = Encoding.UTF8.GetString(bytes);

                //XmlDocument ublXml = LoadSampleUBL();
                //ublXml.Save("UBL\\debug_ubl1.xml");

                var signer = new EInvoiceSigner();
                SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey_CSID);

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
                    var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;

                    // Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

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
                    var Base64QrCode = ZatcaHelper.GetBase64QrCode(signResult);
                    byte[] qrBytes = Convert.FromBase64String(Base64QrCode);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", ublPath, null);

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
                    salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, signResult.ErrorMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error signing to ZATCA:\n" + ex.Message);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }
    }
}
