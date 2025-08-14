using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ZatcaComplianceApi
{
    private static readonly HttpClient client = new HttpClient();
    private const string ComplianceUrl = "https://gw-fatoora.zatca.gov.sa/e-invoicing/compliance/invoices";

    public static async Task<string> SubmitInvoiceAsync(string accessToken, string invoiceHash, string uuid, string invoiceBase64)
    {
        // Prepare the request body
        var requestBody = new
        {
            invoiceHash = invoiceHash,
            uuid = uuid,
            invoice = invoiceBase64
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Set the headers
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        client.DefaultRequestHeaders.Add("Accept-Language", "en");
        client.DefaultRequestHeaders.Add("Accept-Version", "V2");

        // Send the POST request
        HttpResponseMessage response = await client.PostAsync(ComplianceUrl, content);

        string responseContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return $"Invoice submitted successfully: {responseContent}";
        }
        else
        {
            throw new Exception($"Error submitting invoice: {response.StatusCode} - {responseContent}");
        }
    }
}
