using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Zatca.EInvoice.SDK.Contracts.Models;


public class ZatcaAuth
{
    private static readonly HttpClient client = new HttpClient();
    //private const string Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal"; // Compliance CSID (Certificate)
   
    // Token response class
    public class AuthenticationResponse
    {
        [JsonProperty("binarySecurityToken")]
        public string BinarySecurityToken { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("requestID")]
        public string RequestID { get; set; }
        
        [JsonProperty("dispositionMessage")]
        public string DispositionMessage { get; set; }

    }

    public static async Task<AuthenticationResponse> GetComplianceCSIDAsync(string csr,string mode, string otp = "123456")
    {
        string api = "/compliance";
        string Server;
        switch (mode)
        {
            case "Simulation":
                Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation";
                break;
            case "Production":
                Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal";
                break;
            default:
                Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal";
                break;
        }
        string apiLink = Server + api;
        
        // Prepare the request body
        var requestBody = new
        {
            csr = csr
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("OTP", otp);
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
        
        // Send the POST request
        HttpResponseMessage response = await client.PostAsync(apiLink, content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
            return tokenResponse;
        }
        else
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get CSID: {response.ReasonPhrase} - {errorContent}");
        }
    }
    public static async Task<AuthenticationResponse> GetProductionCSIDAsync(string compliance_request_id, string authorizationToken,string mode)
    {
        string api = "/production/csids";
        string Server; 
        switch (mode)
        {
            case "Simulation":
                Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation";
                break;
            case "Production":
                Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal";
                break;
            default:
                Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal";
                break;
        }
        string apiLink = Server + api;

        // Prepare the request body
        var requestBody = new
        {
            compliance_request_id = compliance_request_id
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Clear();
        // Set required headers
        // Create the authorization token in the required format
        //string authorizationToken = $"Bearer {binarySecurityToken}:{secret}";
        
        client.DefaultRequestHeaders.Add("Accept-Language", "EN");
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {authorizationToken}");

        // Send the POST request
        HttpResponseMessage response = await client.PostAsync(apiLink, content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
            return tokenResponse;
        }
        else
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get Production CSID: {response.ReasonPhrase} - {errorContent}");
        }

        
    }

    
}
