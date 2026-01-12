using Newtonsoft.Json;
using Org.BouncyCastle.X509;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
                    //var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    ////MessageBox.Show($"Invoice Hash : {SignedInvoiceHash}\n");
                    //ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    //var qrGen = new EInvoiceQRGenerator();
                    //QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    //string qrBase64 = qrResult.QR;


                    // Insert QR into Signed XML before submission
                   // ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

                    // Save signed XML
                    string ublPath = Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");
                    signResult.SignedEInvoice.Save(ublPath);
                    //signResult.SaveSignedEInvoice(ublPath);

                    EInvoiceValidator eInvoiceValidator = new EInvoiceValidator();
                    var resultValidator = eInvoiceValidator.ValidateEInvoice(signResult.SignedEInvoice, cert, secret);

                    if (!resultValidator.IsValid)
                    {
                        var failedSteps = resultValidator.ValidationSteps
                           .Where(step => !step.IsValid)
                           .Select(step => $"{step.ValidationStepName}: {step.ErrorMessages[0]}")
                           .ToList();

                        string fullError = failedSteps.Any()
                            ? string.Join("\n\n", failedSteps)
                            : resultValidator.ValidationSteps[0].ErrorMessages[0] ?? "Signing failed with unknown error.";

                        MessageBox.Show("Zatca Invoice Validator results:\n\n" + fullError);
                    }



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

                    //MessageBox.Show($"Invoice No: {invoiceNo} signed with ZATCA CSID.", "ZATCA Compliance CSID", MessageBoxButtons.OK);


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
        private static int ReadTlvLength(byte[] data, ref int index)
        {
            if (index >= data.Length)
                throw new FormatException("Invalid TLV: missing length.");

            int first = data[index++];

            // Short form
            if ((first & 0x80) == 0)
                return first;

            // Long form
            int count = first & 0x7F;
            if (count <= 0 || count > 4)
                throw new FormatException("Invalid TLV: unsupported length bytes.");

            if (index + count > data.Length)
                throw new FormatException("Invalid TLV: length exceeds buffer.");

            int len = 0;
            for (int i = 0; i < count; i++)
            {
                len = (len << 8) | data[index++];
            }

            return len;
        }

        private static Dictionary<int, byte[]> DecodeZatcaQrTlvRaw(string qrBase64)
        {
            if (string.IsNullOrWhiteSpace(qrBase64))
                throw new ArgumentException("QR base64 is empty.", nameof(qrBase64));

            byte[] data = Convert.FromBase64String(qrBase64);

            var result = new Dictionary<int, byte[]>();
            int i = 0;

            while (i < data.Length)
            {
                int tag = data[i++];

                int len = ReadTlvLength(data, ref i);
                if (i + len > data.Length)
                    throw new FormatException("Invalid TLV: value length exceeds buffer.");

                var valueBytes = new byte[len];
                Buffer.BlockCopy(data, i, valueBytes, 0, len);
                i += len;

                result[tag] = valueBytes;
            }

            return result;
        }

        private static void DebugQrTimestampFromBase64(string qrBase64)
        {
            var tlv = DecodeZatcaQrTlvRaw(qrBase64);

            string tag3 = null;
            if (tlv.ContainsKey(3))
                tag3 = Encoding.UTF8.GetString(tlv[3]);

            bool ok =
                !string.IsNullOrWhiteSpace(tag3) &&
                (Regex.IsMatch(tag3, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$") ||
                 Regex.IsMatch(tag3, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$"));

            string tags = string.Join(", ", tlv.Keys.OrderBy(k => k).Select(k => k.ToString()));

            MessageBox.Show(
                "TLV tags found: " + tags + "\n" +
                "QR Tag3 timestamp = " + (tag3 ?? "<missing>") + "\n" +
                "Valid format = " + ok,
                "QR Timestamp Debug");
        }

        public static void SignCreditNoteToZatcaAsync(string invoiceNo, string previousInvoiceNo, DateTime previousInvoiceDate)
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

                    MessageBox.Show($"Credit Note No: {invoiceNo} signed with ZATCA CSID.", "ZATCA Compliance CSID", MessageBoxButtons.OK);


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

                    MessageBox.Show($"Debit Note No: {invoiceNo} signed with ZATCA CSID.", "ZATCA Compliance CSID", MessageBoxButtons.OK);


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
            
            if (ds.Tables["Sale"].Rows.Count == 0)
                throw new Exception("Invoice not found: " + invoiceNo);

            // Create generator instance
            var generator = new ZatcaInvoiceGenerator();

            // Generate XML document
            XmlDocument ublXml = generator.GenerateZatcaInvoiceXmlDocument(ds, invoiceNo);
            ublXml.PreserveWhitespace = true;

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
            ublXml.PreserveWhitespace = true;

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
            ublXml.PreserveWhitespace = true;

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
        public static async Task ZatcaInvoiceClearanceAsync(string invoiceNo)
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


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async Task ZatcaInvoiceReportingAsync(string invoiceNo)
        {
            SalesBLL salesBLL = new SalesBLL();
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
                var hash = string.Empty;
                // Check if ZATCA credentials are configured
                if (string.IsNullOrEmpty(env))
                {
                    MessageBox.Show("No active ZATCA credentials found. Please configure them first.");
                    return;
                }

                XmlDocument ublXml = LoadSignedXMLInvoice(invoiceNo);

                
                // 2. Generate request payload
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

                // 1. IMPORTANT: compute hash from the exact XML that will be submitted
                var _GenerateInvoiceHash = new EInvoiceHashGenerator();
                hash = _GenerateInvoiceHash.GenerateEInvoiceHashing(ublXml).Hash;

                //hash = requestResult.InvoiceRequest.InvoiceHash;
                var requestBody = new
                {
                    // Use the hash we calculated from the same XmlDocument we are submitting
                    invoiceHash = hash,
                    uuid = requestResult.InvoiceRequest.Uuid,
                    invoice = requestResult.InvoiceRequest.Invoice
                };

                //  Fix for CS4014 and IDE0058: Await the async call and handle the result
                // 3. Call ZATCA Reporting API
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

                // 4. Save the ZATCA status to the database
                ZatcaInvoiceGenerator.SaveZatcaStatusToDatabase(
                       invoiceNo,
                       requestResult.InvoiceRequest.Uuid,
                       hash,
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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting to ZATCA:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                salesBLL.UpdateZatcaStatus(invoiceNo, "Failed", null, ex.Message);
            }
        }
        public static XmlDocument LoadSignedXMLInvoice(string invoiceNo)
        {
            string path = System.IO.Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");
            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException("XML/UBL invoice not found.");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(path);
            return xmlDoc;
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
                // 1. Generate XML invoice unsigned
                XmlDocument ublXml = GenerateUBLXMLInvoice(invoiceNo);
                //ublXml.Save("UBL\\unsigned_ubl_"+ invoiceNo + ".xml");

                // Check if ZATCA credentials are configured
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID found. Please configure them first.");
                    return;
                }

               
                //string cert_CSID = activeZatcaCredential["cert_base64"].ToString(); 
                //string secret_CSID = activeZatcaCredential["secret_key"].ToString();
                string privateKey_CSID = activeZatcaCredential["private_key"].ToString(); 
                                                                                          

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

                // 2. Sign XML with Zatca PCSID 
                var signer = new EInvoiceSigner();
                SignResult signResult = signer.SignDocument(ublXml, decodedCert, privateKey_CSID);

                //ShowSignResult(signResult);

                if (signResult.IsValid)
                {
                    // Make sure "UBL" folder exists
                    string ublFolder = Path.Combine(Application.StartupPath, "UBL");
                    if (!Directory.Exists(ublFolder))
                        Directory.CreateDirectory(ublFolder);

                    // Debug the QR already embedded by the signer (this is the one you should keep)
                    var embeddedQrBase64 = ZatcaHelper.GetBase64QrCode(signResult);
                    //DebugQrTimestampFromBase64(embeddedQrBase64);
                    //ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, embeddedQrBase64);

                    // 3. Compute Invoice Hash for submission
                    //var invoiceHash = new EInvoiceHashGenerator();
                    //var hashResult = invoiceHash.GenerateEInvoiceHashing(signResult.SignedEInvoice);

                    //// Get Invoice Hash for Next PIH
                    //var SignedInvoiceHash = ZatcaHelper.GetInvoiceHash(signResult);
                    //ZatcaHelper.InsertInvoiceHashToSignedXml(signResult.SignedEInvoice, SignedInvoiceHash);

                    // 5. Generate QR Code
                    var qrGen = new EInvoiceQRGenerator();
                    QRResult qrResult = qrGen.GenerateEInvoiceQRCode(signResult.SignedEInvoice);
                    string qrBase64 = qrResult.QR;
                    //Insert QR into Signed XML before submission
                    ZatcaHelper.InsertQrIntoXml(signResult.SignedEInvoice, qrBase64);

                    //// Debug Tag3 timestamp
                    //DebugQrTimestampFromBase64(qrBase64);


                    // 6. Save signed XML
                    string ublPath = Path.Combine(Application.StartupPath, "UBL", invoiceNo + "_signed.xml");
                    signResult.SignedEInvoice.Save(ublPath);
                    
                    // 7. Generate QR image for display/storage in db
                    byte[] qrBytes = Convert.FromBase64String(embeddedQrBase64);
                    salesBLL.UpdateZatcaQrCode(invoiceNo, qrBytes);
                    //MessageBox.Show($"Base64 QRCode : {Base64QrCode}\n");

                    EInvoiceValidator eInvoiceValidator = new EInvoiceValidator();
                    string pcsidCertBase64 = PCSID_dataRow["cert_base64"].ToString();
                    string pcsidSecret = PCSID_dataRow["secret_key"].ToString();

                    var resultValidator = eInvoiceValidator.ValidateEInvoice(
                        signResult.SignedEInvoice,
                        pcsidCertBase64,
                        pcsidSecret); 
                    
                    //var resultValidator = eInvoiceValidator.ValidateEInvoice(signResult.SignedEInvoice, decodedCert, privateKey_CSID);

                    if (!resultValidator.IsValid)
                    {
                        var failedSteps = resultValidator.ValidationSteps
                           .Where(step => !step.IsValid)
                           .Select(step => $"{step.ValidationStepName}: {step.ErrorMessages[0]}")
                           .ToList();

                        string fullError = failedSteps.Any()
                            ? string.Join("\n\n", failedSteps)
                            : resultValidator.ValidationSteps[0].ErrorMessages[0] ?? "Signing failed with unknown error.";

                        MessageBox.Show("Zatca Invoice Validator results:\n\n" + fullError);
                    }

                    
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

                    //MessageBox.Show($"Invoice No: {invoiceNo} signed with ZATCA PCSID.", "ZATCA Production CSID", MessageBoxButtons.OK,MessageBoxIcon.Information);

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

        public static async Task PCSID_SignInvoiceToZatcaAsync_NEW(string invoiceNo)
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
                XmlDocument ublXml = GenerateUBLXMLInvoice(invoiceNo);

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

                string privateKey = activeZatcaCredential["private_key"].ToString();
                string certBase64 = PCSID["cert_base64"].ToString();
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
                var sigData = ExtractSignatureData(signResult.SignedEInvoice,decodedCert);

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

                // 5️⃣ Insert QR into xml
                //ZatcaHelper.InsertQrIntoXml(ublXml, qrBase64);

                // 8️⃣ Save signed XML
                string ublFolder = Path.Combine(Application.StartupPath, "UBL");
                Directory.CreateDirectory(ublFolder);

                string path = Path.Combine(ublFolder, invoiceNo + "_signed.xml");
                signResult.SignedEInvoice.Save(path);

                // 9️⃣ Save QR
                salesBLL.UpdateZatcaQrCode(invoiceNo, Convert.FromBase64String(qrBase64));
                salesBLL.UpdateZatcaStatus(invoiceNo, "Signed", path, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                    MessageBox.Show($"Credit Note No: {invoiceNo} signed with ZATCA PCSID.", "ZATCA Production CSID", MessageBoxButtons.OK,MessageBoxIcon.Information);

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

                    MessageBox.Show($"Debit Note No: {invoiceNo} signed with ZATCA PCSID.", "ZATCA Production CSID", MessageBoxButtons.OK,MessageBoxIcon.Information);


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
       
        public static void UpdateQRCodeInSignedXml(XmlDocument xmlDoc, string qrBase64)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            // Find the QR node
            var qrNode = xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference[cbc:ID='QR']//cbc:EmbeddedDocumentBinaryObject", ns);
            if (qrNode != null)
            {
                qrNode.InnerText = qrBase64;
            }
            else
            {
                // Create if not found (should not happen)
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

                // Insert before PIH
                XmlNode pihNode = xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference[cbc:ID='PIH']", ns);
                if (pihNode != null)
                {
                    pihNode.ParentNode.InsertBefore(qrDocRef, pihNode);
                }
                else
                {
                    xmlDoc.DocumentElement.AppendChild(qrDocRef);
                }
            }

        }
        public static ZatcaSignatureData ExtractSignatureData(
    XmlDocument signedXml,
    string decodedPcSidCertBase64)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(signedXml.NameTable);
            ns.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            // Tag-7: Invoice signature value
            string tag7 =
                signedXml.SelectSingleNode("//ds:SignatureValue", ns)?.InnerText;

            // Tag-8: FULL certificate from signed XML
            string tag8 =
                signedXml.SelectSingleNode("//ds:X509Certificate", ns)?.InnerText;

            if (string.IsNullOrEmpty(tag7) || string.IsNullOrEmpty(tag8))
                throw new Exception("Tag-7 or Tag-8 missing in signed XML.");

            // Tag-9: PCSID certificate signature (FIXED)
            string tag9 = GetCertificateSignatureBase64(decodedPcSidCertBase64);

            return new ZatcaSignatureData
            {
                SignatureValueBase64 = tag7,          // Tag-7
                PublicKeyBase64 = tag8,               // Tag-8 (certificate)
                CertificateSignatureBase64 = tag9     // Tag-9 (cert signature)
            };
        }

        public static string GetCertificateSignatureBase64(string pcSidCertBase64)
        {
            if (string.IsNullOrWhiteSpace(pcSidCertBase64))
                throw new ArgumentException("Certificate is empty.", nameof(pcSidCertBase64));

            // Your DB value is base64 → PEM text
            string pemText = Encoding.UTF8.GetString(
                Convert.FromBase64String(pcSidCertBase64)
            );

            // Remove PEM headers
            string derBase64 = pemText
                .Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            byte[] certDer = Convert.FromBase64String(derBase64);

            // BouncyCastle parses X509 correctly
            var parser = new X509CertificateParser();
            var cert = parser.ReadCertificate(certDer);

            // ✅ THIS IS TAG-9 (certificate digital signature)
            byte[] signatureBytes = cert.GetSignature();

            return Convert.ToBase64String(signatureBytes);
        }


    }
}

