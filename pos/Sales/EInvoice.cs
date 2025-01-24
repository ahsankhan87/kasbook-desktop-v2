using POS.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace pos.Sales
{
    [XmlRoot("Invoice", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
    public class Invoice
    {
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string InvoiceNumber { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string InvoiceDate { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal Amount { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string CustomerName { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ProfileID { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string IssueDate { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string IssueTime { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
        
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string UUID { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string InvoiceTypeCode { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string DocumentCurrencyCode { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string TaxCurrencyCode { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SupplierVATNumber { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public TaxTotal TaxTotal { get; set; }
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]

        public InvoiceLine InvoiceLine { get; set; }

    }

    public class TaxTotal
    {
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal TaxableAmount { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal TaxAmount { get; set; }

        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal TaxRate { get; set; }
    }

    public class InvoiceLine
    {
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public int LineID { get; set; }
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string Description { get; set; }
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal Quantity { get; set; }
        [XmlElement(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public decimal UnitPrice { get; set; }

        
    }

    class EInvoice
    {
        public string GenerateXMLInvoice(Invoice invoice)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Invoice));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, invoice);
                return textWriter.ToString();
            }
        }
        public string SignInvoice(string invoiceXML, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
            byte[] data = Encoding.UTF8.GetBytes(invoiceXML);
            byte[] signature = rsa.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
            return Convert.ToBase64String(signature);
        }
        public async Task<string> SendInvoiceToZatca(string xmlInvoice)
        {
            HttpClient client = new HttpClient();
            var content = new StringContent(xmlInvoice, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await client.PostAsync("https://api.zatca.gov.sa/invoice", content);
            return await response.Content.ReadAsStringAsync();
        }

        public string GenerateUBLXML(Invoice invoice)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            XmlSerializer serializer = new XmlSerializer(typeof(Invoice), "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false), // UTF-8 without BOM
                Indent = true
            };

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    serializer.Serialize(xmlWriter, invoice, namespaces);
                }

                string resultXml = Encoding.UTF8.GetString(memoryStream.ToArray());
                // Now 'resultXml' contains the correctly serialized XML with namespaces

                return resultXml;
            }
            

        }
        public string CreateUBLInvoice(string invoiceNo)
        {
            // Step 1: Fetch data from the database
            Invoice invoice = GetInvoiceData(invoiceNo);

            // Step 2: Populate invoice lines, tax totals, and other fields
            //invoice.InvoiceLines = GetInvoiceLines(invoiceId); // Fetch the lines from the database
            
            // Step 3: Generate UBL XML
            string ublXML = GenerateUBLXML(invoice);

            return ublXML;
        }
        public Invoice GetInvoiceData(string invoiceNo)
        {
            Invoice invoice = new Invoice();
            SalesBLL salesBLL = new SalesBLL();
            DataTable dt = salesBLL.SaleReceipt(invoiceNo);
            DataTable dtSalesItems = salesBLL.GetAllSalesItems(invoiceNo);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in dt.Rows)
                {
                    invoice.ProfileID = "reporting:1.0";  // Business process (BT-23)
                    invoice.ID = myProductView["invoice_no"].ToString();  // Business process (BT-23)
                    invoice.UUID = Guid.NewGuid().ToString();
                    invoice.IssueDate = myProductView["sale_date"].ToString(); // Issue date (BT-2)
                    invoice.IssueTime = "17:41:08";   // Issue time (KSA-25)
                    invoice.InvoiceTypeCode = "388";
                    invoice.DocumentCurrencyCode = "SAR";  // Invoice currency code (BT-5)
                    invoice.TaxCurrencyCode = "SAR";  // Tax currency code (BT-6)

                    invoice.InvoiceDate = myProductView["sale_date"].ToString();
                    invoice.InvoiceNumber = myProductView["invoice_no"].ToString();
                    
                    invoice.Amount = Math.Round(Convert.ToDecimal(myProductView["total_amount"]), 2);
                    invoice.CustomerName = myProductView["customer_name"].ToString();
                    // Add other fields as necessary
                    // Populate mandatory fields
                    
                    invoice.ID = invoiceNo;  // Invoice counter (KSA-16)
                    invoice.InvoiceTypeCode = "380";  // Invoice type code (BT-3)
                    
                    // Seller VAT registration number (BT-31)
                    invoice.SupplierVATNumber = "300420598700003";

                    // Tax Details from the same table
                    invoice.TaxTotal = new TaxTotal
                    {
                        TaxableAmount = Math.Round(Convert.ToDecimal(myProductView["total_tax"]), 2),
                        TaxAmount = Math.Round(Convert.ToDecimal(myProductView["total_tax"]), 2),
                        TaxRate = Math.Round(Convert.ToDecimal(myProductView["tax_rate"]), 2)
                    };

                }
            }
            if (dtSalesItems.Rows.Count > 0)
            {
                foreach (DataRow myProductView in dtSalesItems.Rows)
                {
                    invoice.InvoiceLine = new InvoiceLine
                    {
                        LineID = dtSalesItems.Rows.Count + 1,
                        Description = myProductView["product_name"].ToString(),
                        Quantity = Math.Round(Convert.ToDecimal(myProductView["quantity_sold"].ToString()),2),
                        UnitPrice = Math.Round(Convert.ToDecimal(myProductView["unit_price"].ToString()),2)

                    };
                }
            }

            return invoice;
        }

        public async Task<string> SendInvoiceToZATCA(string xmlInvoice)
        {
            try
            {
                // ZATCA API Test URL (Replace with actual endpoint provided by ZATCA)
                string apiUrl = "https://sandbox-api.zatca.gov.sa/invoice";

                // Create HttpClient
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/xml");

                    // Prepare content for the request (XML content)
                    HttpContent content = new StringContent(xmlInvoice, Encoding.UTF8, "application/xml");

                    // Make the POST request to ZATCA API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Read response content
                    string responseString = await response.Content.ReadAsStringAsync();

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        System.Windows.MessageBox.Show("Invoice submitted successfully to ZATCA.");
                        return responseString;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Error submitting invoice: " + response.ReasonPhrase);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Exception: " + ex.Message);
                return null;
            }
        }

    }
}
