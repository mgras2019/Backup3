namespace Microsoft.CAP.HRDDI.RetryFailedQuoteFlagging
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Worker;

    public class RetryFlagging
    {
        private readonly HttpClient HttpClient;

        private readonly HttpClient TokenHttpClientClient;

        private readonly System.Net.Http.IHttpClientFactory HttpClientFactory;

        public RetryFlagging(System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            this.HttpClient = httpClientFactory.CreateClient();
            this.TokenHttpClientClient = httpClientFactory.CreateClient("tokenClient");
        }

        [Function("RetryFlaggingForFailedQuotes")]
        public void Run([TimerTrigger("%TimeInterval%"
#if DEBUG
    , RunOnStartup=true
#endif
            )] TimerInfo myTimer, FunctionContext context)
        {
            var log = context.GetLogger("RetryFlaggingForFailedQuotes");
            try
            {
                if (!myTimer.IsPastDue)
                {
                    log.LogInformation($"Function executed at: {DateTime.Now}");
                    var result = RetryFlaggingForFailedQuotesAsync().Result;
                }
                else
                {
                    log.LogInformation($"Function is Past Due at: {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                log.LogInformation($"Function Exception : {ex.Message}{ex.InnerException.Message}");
                throw;
            }

            log.LogInformation($"Function completed at: {DateTime.Now}");
        }

        private async Task<string> GetAccessTokenAsync()
        {
            string accessToken = string.Empty;
            string tokenEndpointUri = Environment.GetEnvironmentVariable("AuthUrl");

            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id",Environment.GetEnvironmentVariable("ClientId")),
                new KeyValuePair<string, string>("client_secret",Environment.GetEnvironmentVariable("ClientSecret")),
                new KeyValuePair<string, string>("directory_id",Environment.GetEnvironmentVariable("TenantId")),
                new KeyValuePair<string, string>("resource", Environment.GetEnvironmentVariable("ResourceUrl"))
                }
            );

            var httpResponseMessage = TokenHttpClientClient.PostAsync(tokenEndpointUri, content).Result;

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                JObject responseJSON = JObject.Parse(responseString);
                accessToken = responseJSON["access_token"].Value<string>();
            }
            else
            {
                throw new Exception(httpResponseMessage.StatusCode.ToString());
            }

            return accessToken;
        }

        private async Task<bool> RetryFlaggingForFailedQuotesAsync()
        {
            string accessToken = GetAccessTokenAsync().Result;
            Uri uri = new Uri($"{Environment.GetEnvironmentVariable("HRDDIApiBaseUrl")}{Environment.GetEnvironmentVariable("HRDDIEndPoint")}");

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpClient.Timeout = TimeSpan.FromMilliseconds(600000);
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var httpResponseMessage = await HttpClient.PostAsync(uri, requestContent);

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(httpResponseMessage.StatusCode.ToString());
            }

            return await Task.FromResult(true);
        }
    }

}