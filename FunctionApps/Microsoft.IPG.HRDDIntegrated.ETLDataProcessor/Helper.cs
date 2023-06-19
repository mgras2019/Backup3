namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Helper
    {
        private readonly HttpClient TokenHttpClientClient;

        private readonly IHttpClientFactory HttpClientFactory;

        public Helper(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            this.TokenHttpClientClient = httpClientFactory.CreateClient("tokenClient");
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessTokenAsync()
        {
            HttpResponseMessage res;
            string tokenEndpointUri = Environment.GetEnvironmentVariable("AuthUrl");

            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id",Environment.GetEnvironmentVariable("ClientID")),
            new KeyValuePair<string, string>("client_secret",Environment.GetEnvironmentVariable("ClientSecret")),
            new KeyValuePair<string, string>("directory_id",Environment.GetEnvironmentVariable("DirectoryID")),
            new KeyValuePair<string, string>("resource", Environment.GetEnvironmentVariable("ResourceUrl"))
            }
            );
           
            try
            {
                res = TokenHttpClientClient.PostAsync(tokenEndpointUri, content).Result;
            }
            catch (Exception)
            {
                throw;
            }

            string tokenstr = await res.Content.ReadAsStringAsync();
            JObject tokenjson = JObject.Parse(tokenstr);
            string token = tokenjson["access_token"].Value<string>();
            return token;
        }
    }
}
