using RestSharp;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;


public class ZatcaAuth
{
    private static readonly HttpClient client = new HttpClient();
    private const string Server = "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal"; // Compliance CSID (Certificate)
   
    // Token response class
    public class AuthenticationResponse
    {
        [JsonProperty("binarySecurityToken")]
        public string BinarySecurityToken { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("requestID")]
        public string requestID { get; set; }

    }

    public static async Task<AuthenticationResponse> GetComplianceCSIDAsync()
    {
        string api = "/compliance";
        string apiLink = Server + api;

        // Prepare the request body
        var requestBody = new
        {
            csr = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURSBSRVFVRVNULS0tLS0KTUlJQ0ZUQ0NBYndDQVFBd2RURUxNQWtHQTFVRUJoTUNVMEV4RmpBVUJnTlZCQXNNRFZKcGVXRmthQ0JDY21GdQpZMmd4SmpBa0JnTlZCQW9NSFUxaGVHbHRkVzBnVTNCbFpXUWdWR1ZqYUNCVGRYQndiSGtnVEZSRU1TWXdKQVlEClZRUUREQjFVVTFRdE9EZzJORE14TVRRMUxUTTVPVGs1T1RrNU9Ua3dNREF3TXpCV01CQUdCeXFHU000OUFnRUcKQlN1QkJBQUtBMElBQktGZ2ltdEVtdlJTQkswenI5TGdKQXRWU0NsOFZQWno2Y2RyNVgrTW9USG84dkhOTmx5Vwo1UTZ1N1Q4bmFQSnF0R29UakpqYVBJTUo0dTE3ZFNrL1ZIaWdnZWN3Z2VRR0NTcUdTSWIzRFFFSkRqR0IxakNCCjB6QWhCZ2tyQmdFRUFZSTNGQUlFRkF3U1drRlVRMEV0UTI5a1pTMVRhV2R1YVc1bk1JR3RCZ05WSFJFRWdhVXcKZ2FLa2daOHdnWnd4T3pBNUJnTlZCQVFNTWpFdFZGTlVmREl0VkZOVWZETXRaV1F5TW1ZeFpEZ3RaVFpoTWkweApNVEU0TFRsaU5UZ3RaRGxoT0dZeE1XVTBORFZtTVI4d0hRWUtDWkltaVpQeUxHUUJBUXdQTXprNU9UazVPVGs1Ck9UQXdNREF6TVEwd0N3WURWUVFNREFReE1UQXdNUkV3RHdZRFZRUWFEQWhTVWxKRU1qa3lPVEVhTUJnR0ExVUUKRHd3UlUzVndjR3g1SUdGamRHbDJhWFJwWlhNd0NnWUlLb1pJemowRUF3SURSd0F3UkFJZ1NHVDBxQkJ6TFJHOApJS09melI1L085S0VicHA4bWc3V2VqUlllZkNZN3VRQ0lGWjB0U216MzAybmYvdGo0V2FxbVYwN01qZVVkVnVvClJJckpLYkxtUWZTNwotLS0tLUVORCBDRVJUSUZJQ0FURSBSRVFVRVNULS0tLS0K",
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("OTP", "12345");
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
    public static async Task<AuthenticationResponse> GetProductionCSIDAsync(string compliance_request_id, string authorizationToken)
    {
        string api = "/production/csids";
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
        
        client.DefaultRequestHeaders.Add("Accept-Language", "en");
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
        client.DefaultRequestHeaders.Add("Authorization", authorizationToken);

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
