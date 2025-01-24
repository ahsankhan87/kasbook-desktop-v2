using RestSharp;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Security.Cryptography;

public class ZatcaInvoice
{
    public static string GenerateInvoiceXml()
    {
        // Create your invoice XML according to ZATCA's UBL specifications
        string invoiceXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Invoice>\n  <!-- Your invoice data goes here -->\n</Invoice>";
        return invoiceXml;
    }

    public static string CalculateInvoiceHash(string invoiceXml)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] invoiceBytes = Encoding.UTF8.GetBytes(invoiceXml);
            byte[] hashBytes = sha256.ComputeHash(invoiceBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static string EncodeInvoiceToBase64(string invoiceXml)
    {
        byte[] invoiceBytes = Encoding.UTF8.GetBytes(invoiceXml);
        return Convert.ToBase64String(invoiceBytes);
    }

    public static void SendInvoice(string accessToken)
    {
        var client = new RestClient("https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal/invoices"); // ZATCA Sandbox URL
        var request = new RestRequest(Method.Post.ToString());
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");

        // Create invoice data object according to ZATCA's requirements
        var invoiceData = new
        {
            invoice_number = "INV123456",
            issue_date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            customer_name = "Test Customer",
            vat_amount = 100.00,
            total_amount = 1000.00,
            invoice_items = new[]
            {
                new { item_name = "Product A", item_price = 500.00, vat = 50.00 },
                new { item_name = "Product B", item_price = 500.00, vat = 50.00 }
            }
        };

        string jsonBody = JsonConvert.SerializeObject(invoiceData);
        request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response.IsSuccessful)
        {
            Console.WriteLine("Invoice submitted successfully: " + response.Content);
        }
        else
        {
            Console.WriteLine("Error submitting invoice: " + response.Content);
        }
    }
}
