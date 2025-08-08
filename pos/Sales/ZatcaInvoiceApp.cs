using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using QRCoder;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Sales
{
    public partial class ZatcaInvoiceApp : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private string csrPem;
        private string privateKeyPem;
        byte[] _qr_code;
        string _qr_code_1;

        public ZatcaInvoiceApp(string qr_code_1, byte[] qr_code = null)
        {
            _qr_code = qr_code;
            _qr_code_1 = qr_code_1;
            InitializeComponent();
        }
        public ZatcaInvoiceApp()
        {
            InitializeComponent();
        }
        private void ZatcaInvoiceApp_Load(object sender, EventArgs e)
        {
            ShowQRCode(_qr_code);
            ShowQRCode1(_qr_code_1);
        }
        private void btnSubmitInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                // Step 1: Generate Digital Signature (Simulated with Hashing)
                string invoiceData = GenerateInvoiceData();
                string digitalSignature = GenerateDigitalSignature(invoiceData);

                // Step 2: Generate QR Code from Invoice Data
                string qrData = GenerateQRCode(invoiceData);
                //ShowQRCode(qrData);
                //ShowQRCode(GenerateQRCode(invoiceData));//GIVE DATA TO FUNCTION AND GET QRCODE

                // Step 3: Send Invoice to ZATCA Compliance API
                //string response = SendInvoiceToZATCA(invoiceData, digitalSignature);
                _ = ComplianceInvoice(invoiceData, digitalSignature);
                
                //txtResult.Text = response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        // Generates dummy invoice data
        private string GenerateInvoiceData()
        {
            //string SallerName = gethexstring(1, "Khybersoft"); //Tag1
            //string VATReg = gethexstring(2, "300420598700003"); //Tag2
            //string DateTimeStr = gethexstring(3, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")); //Tage3
            //string TotalAmt = gethexstring(4, "1800.00"); //Tag4
            //string VatAmt = gethexstring(5, "300.00"); //Tag5
            //string qrcode_String = SallerName + VATReg + DateTimeStr + TotalAmt + VatAmt;

            //return qrcode_String;

            var invoice = new
            {
                InvoiceNumber = "INV123456",
                IssueDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Seller = new { Name = "Khybersoft", VATNumber = "300420598700003" },
                Buyer = new { Name = "Ahsan Khan", VATNumber = "9876543210" },
                LineItems = new[]
                {
                    new { Description = "Product A", Quantity = 2, UnitPrice = 500.00, VATRate = 0.15, VATAmount = 150.00 },
                    new { Description = "Product B", Quantity = 1, UnitPrice = 1000.00, VATRate = 0.15, VATAmount = 150.00 }
                },
                TotalExcludingVAT = 1500.00,
                TotalVAT = 300.00,
                TotalIncludingVAT = 1800.00
            };

            //// Serialize invoice data to JSON
            return JsonConvert.SerializeObject(invoice);
        }
        static string gethexstring(Int32 TagNo, string TagValue)
        {

            string decString = TagValue;
            byte[] bytes = Encoding.UTF8.GetBytes(decString);
            string hexString = BitConverter.ToString(bytes);

            string StrTagNo = String.Format("0{0:X}", TagNo);
            String TagNoVal = StrTagNo.Substring(StrTagNo.Length - 2, 2);

            string StrTagValue_Length = String.Format("0{0:X}", bytes.Length);
            String TagValue_LengthVal = StrTagValue_Length.Substring(StrTagValue_Length.Length - 2, 2);


            hexString = TagNoVal + TagValue_LengthVal + hexString.Replace("-", "");
            return hexString;
        }

        // Generates a digital signature (for illustration, using SHA256 hash)
        private string GenerateDigitalSignature(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashBytes); // Simulated signature
            }
        }

        // Generates a QR code from the invoice data
        private string GenerateQRCode(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] qrHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(qrHash); // QR data content
            }
        }

        // Display the QR code in the PictureBox
        private void ShowQRCode1(string qrData)
        {
            if (!string.IsNullOrEmpty(qrData))
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                pbQRCode.Image = qrCodeImage;
            }
        }
        public void ShowQRCode(byte[] qrBytes)
        {
            try
            {
                if(qrBytes != null)
                {
                    string base64String = Convert.ToBase64String(qrBytes);

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    pbQRCode.Image = qrCodeImage;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid QR image data: " + ex.Message);
            }
        }
        public static string HexToBase64(string strInput)
        {
            try
            {
                var bytes = new byte[strInput.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(strInput.Substring(i * 2, 2), 16);
                }
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        // Sends invoice data to ZATCA Compliance API
        private string SendInvoiceToZATCA(string invoiceData, string digitalSignature)
        {
            var client = new RestClient("https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices");
            var request = new RestRequest(Method.Post.ToString());
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            // Prepare request body (with invoice data and digital signature)
            var body = new
            {
                Invoice = invoiceData,
                Signature = digitalSignature // Simulated digital signature
            };

            string jsonBody = JsonConvert.SerializeObject(body);
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            // Return API response
            return response.IsSuccessful ? "Invoice submitted successfully: " + response.Content : "Error: " + response.Content;
        }

        private async Task ComplianceInvoice(string invoiceData, string invoiceHash)
        {
            try
            {
                
                var authDetails = await ZatcaAuth.GetComplianceCSIDAsync("alsdjfadj","Simulation");
                string binarySecurityToken = authDetails.BinarySecurityToken;
                string secret = authDetails.Secret;
                
                string apiUrl = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/compliance/invoices";

                // Request body in JSON format (example)
                var requestBody = new
                {
                    invoiceHash = invoiceHash,
                    uuid = Guid.NewGuid().ToString(), 
                    invoice = invoiceData, 
                };

                // Convert the request body to JSON
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Set required headers
                // Create the authorization token in the required format
                
                string authorizationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{binarySecurityToken}:{secret}"));
                
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept-Language", "en");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {authorizationToken}");
                
                // Send POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Handle the response
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject(responseContent);
                    txtResult.Text = tokenResponse.ToString();
                    MessageBox.Show("Response: " + tokenResponse);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed : {response.ReasonPhrase} - {errorContent}");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Step 1: Authenticate and get the binarySecurityToken and secret
                var authDetails = await ZatcaAuth.GetComplianceCSIDAsync("alsdjfadj","Simulation");
                string binarySecurityToken = authDetails.BinarySecurityToken;
                string secret = authDetails.Secret;
                string requestID = authDetails.RequestID;
                string authorizationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{binarySecurityToken}:{secret}"));

                //// Step 2: Prepare invoice data
                //string invoiceXml = ZatcaInvoice.GenerateInvoiceXml();
                //string invoiceHash = ZatcaInvoice.CalculateInvoiceHash(invoiceXml);
                //string invoiceBase64 = ZatcaInvoice.EncodeInvoiceToBase64(invoiceXml);
                //string uuid = Guid.NewGuid().ToString();

                // Step 3: Submit the invoice
                var response = await ZatcaAuth.GetProductionCSIDAsync(requestID, authorizationToken,"Simulation");
                string binarySecurityToken1 = response.BinarySecurityToken;
                string secret1 = response.Secret;
                string requestID1 = response.RequestID;
                // Display the response
                MessageBox.Show("Token: "+binarySecurityToken1, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            // Save CSR and Private Key to files
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*";

            // Save CSR
            saveFileDialog.Title = "Save CSR";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, csrPem);
            }

            // Save Private Key
            saveFileDialog.Title = "Save Private Key";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, privateKeyPem);
            }

            MessageBox.Show("CSR and Private Key saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string commonName = "khybersoft";
                string organization = "IT company";
                string organizationalUnit = "10 Digital";
                string country = "SA";
                string registeredAddress = "SA";
                string businessCategory = "IT Market";
                string VATNumber = "300420598700003";
                string email = "";
                string type = "1100";
                //1100 = Standard & Simplified
                //1000 = Standard
                //0100 = Simplified



                //GenerateCsrAndPrivateKey(commonName, organization, organizationalUnit, country, email, registeredAddress, businessCategory, VATNumber, type, out csrPem, out privateKeyPem);
                GeneratePkcs10ECDSA(commonName, organizationalUnit, organization, country, VATNumber, "", out csrPem, out privateKeyPem);
                // Display CSR and Private Key in the textboxes
                txtCsr.Text = csrPem;
                txtPrivateKey.Text = privateKeyPem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void GenerateCsrAndPrivateKey(
            string commonName, string organization, string organizationalUnit, string country, string email, string registeredAddress, string businessCategory, string VATNumber, string type,
            out string csrPem, out string privateKeyPem)
        {
            // Generate the RSA Key Pair (Public and Private Key)
            var keyGen = new RsaKeyPairGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048)); // 2048-bit RSA
            AsymmetricCipherKeyPair keyPair = keyGen.GenerateKeyPair();

            // Create Distinguished Name (DN) for the CSR using X509Name
            var distinguishedName = new X509Name($"CN={commonName}, O={organization}, OU={organizationalUnit}, C={country}, E={email}");
            
            // Generate the CSR
            var csrGenerator = new Pkcs10CertificationRequest(
                PkcsObjectIdentifiers.Pkcs9AtEmailAddress.Id,
                distinguishedName,
                keyPair.Public,
                null,
                keyPair.Private);

            // Convert CSR to PEM format (string)
            StringWriter csrStringWriter = new StringWriter();
            PemWriter csrPemWriter = new PemWriter(csrStringWriter);
            csrPemWriter.WriteObject(csrGenerator);
            csrPemWriter.Writer.Flush();
            csrPem = csrStringWriter.ToString();  // The CSR in PEM format

            // Convert Private Key to PEM format (string)
            StringWriter privateKeyStringWriter = new StringWriter();
            PemWriter privateKeyPemWriter = new PemWriter(privateKeyStringWriter);
            privateKeyPemWriter.WriteObject(keyPair.Private);
            privateKeyPemWriter.Writer.Flush();
            privateKeyPem = privateKeyStringWriter.ToString();  // The Private Key in PEM format
        }

        public static void GeneratePkcs10ECDSA(string commonName, string organizationUnitName, string organizationName, string country, string VATNumber,
            string sanDirName, out string csrPem, out string privateKeyPem)
        {
            try
            {
                // Create ECDSA key pair generator
                var ecKeyPairGenerator = new ECKeyPairGenerator();
                var genParam = new ECKeyGenerationParameters(SecObjectIdentifiers.SecP256k1, new SecureRandom());
                ecKeyPairGenerator.Init(genParam);
                AsymmetricCipherKeyPair ecKeyPair = ecKeyPairGenerator.GenerateKeyPair();

                // Subject Name
                var subjectAttrs = new List<DerObjectIdentifier>() {X509Name.OrganizationIdentifier, X509Name.C, X509Name.OU, X509Name.O, X509Name.CN };
                var subjectValues = new List<string>() { VATNumber, country, organizationUnitName, organizationName, commonName };
                var subject = new X509Name(subjectAttrs.ToArray(), subjectValues.ToArray());

                // SAN
                var sanAttrs = new List<DerObjectIdentifier>() { X509Name.SerialNumber,  X509Name.UID, X509Name.T, new DerObjectIdentifier("2.5.4.26"), X509Name.BusinessCategory };
                var sanValues = new List<string>() { "1-TST|2-TST|3-ed22f1d8-e6a2-1118-9b58-d9a8f11e445f", VATNumber, "1100", "Zatca 3", "Food Business3" };
                var san = new X509Name(sanAttrs.ToArray(), sanValues.ToArray());

                // Extensions
                var extensionsDictionary = new Dictionary<DerObjectIdentifier, X509Extension>()
{
                {
                    new DerObjectIdentifier("1.3.6.1.4.1.311.20.2"),
                    new X509Extension(false, new DerOctetString(new DerPrintableString("TSTZATCA-Code-Signing")))
                        },
                    {
                    X509Extensions.SubjectAlternativeName,
                    new X509Extension(false, new DerOctetString(new DerSequence(new DerTaggedObject(4, san))))
                },
            };
                var extensions = new X509Extensions(extensionsDictionary);
                var attribute = new AttributePkcs(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest, new DerSet(extensions));
                var extensionsSet = new DerSet(attribute);

                // Create CSR using keys, subject, extensions 
                var pkcs10CertificationRequest = new Pkcs10CertificationRequest(
                    "SHA256withECDSA",
                    subject,
                    ecKeyPair.Public,
                    extensionsSet,
                    ecKeyPair.Private);
                
                //return withe base64 endoced
                var csr = Convert.ToBase64String(pkcs10CertificationRequest.GetEncoded());

                // Convert CSR to PEM format (string)
                StringWriter csrStringWriter = new StringWriter();
                PemWriter csrPemWriter = new PemWriter(csrStringWriter);
                csrPemWriter.WriteObject(pkcs10CertificationRequest);
                csrPemWriter.Writer.Flush();
                csrPem = csrStringWriter.ToString();  // The CSR in PEM format

                // Convert Private Key to PEM format (string)
                StringWriter privateKeyStringWriter = new StringWriter();
                PemWriter privateKeyPemWriter = new PemWriter(privateKeyStringWriter);
                privateKeyPemWriter.WriteObject(ecKeyPair.Private);
                privateKeyPemWriter.Writer.Flush();
                privateKeyPem = privateKeyStringWriter.ToString();  // The Private Key in PEM format
            }
            catch (Exception ex)
            {
                // Handle errors as needed
                throw new Exception(ex.Message);
                
            }
        }

       
    }
   
}
