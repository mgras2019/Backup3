namespace Microsoft.IPG.HRDDIntegrated.Open.SystemAutoManualFlaggingProcessor
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using Microsoft.IPG.HRDDIntegrated.ETLDataProcessor;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// SystemAutoManualFlaggingProcessor
    /// </summary>
    public class OpenDealSystemAutoManualFlagging
    {
        private readonly HttpClient HttpClient;

        private readonly IHttpClientFactory HttpClientFactory;

        public OpenDealSystemAutoManualFlagging(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            this.HttpClient = HttpClientFactory.CreateClient();
        }

        [Function("OpenDealSystemAutoManualFlagging")]
        public void Run([TimerTrigger("%timeinterval%"
            
#if DEBUG
    , RunOnStartup=true
#endif
            )] TimerInfo myTimer, FunctionContext context)
        {
            var log = context.GetLogger("OpenDealSystemAutoManualFlagging");
            if (!myTimer.IsPastDue)
            {
                log.LogInformation($"SystemAutoManualFlagging Timer trigger function execution started at: {DateTime.Now}");
                Helper helper = new Helper(HttpClientFactory);

                if (Convert.ToBoolean(Environment.GetEnvironmentVariable("SystemFlagCallEnabled")) == true)
                {
                    string token = helper.GetAccessTokenAsync().Result;
                    _ = UpdateFlaggedRecordsAsync(token, Environment.GetEnvironmentVariable("UpdateSystemFlaggedEndPoint"), log);
                }

                if (Convert.ToBoolean(Environment.GetEnvironmentVariable("AutoManualFlagCallEnabled")) == true)
                {
                    string token = helper.GetAccessTokenAsync().Result;
                    var serviceUrl = Environment.GetEnvironmentVariable("UpdateAutoManualFlaggingEndPoint") + "?authorization=" + token;
                    _ = UpdateFlaggedRecordsAsync(token, serviceUrl, log);
                }

                log.LogInformation($"SystemAutoManualFlagging Timer trigger function execution completed at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"SystemAutoManualFlagging Timer trigger function skipped because of running later: {DateTime.Now}");
            }
        }

        /// <summary>
        /// APIs the call.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="serviceUrl">The service URL.</param>
        private async Task UpdateFlaggedRecordsAsync(string token, string serviceUrl, ILogger log)
        {
            try
            {
                StringContent requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("ApiBaseUrl")}{serviceUrl}");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.Timeout = TimeSpan.FromMilliseconds(200000000);

                System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var response = await httpClient.PostAsync(serviceUrl, requestContent);

                if (response.StatusCode.ToString() == "BadRequest" || response.StatusCode.ToString() == "InternalServerError" || response.StatusCode.ToString() == "unauthorized" || response.StatusCode.ToString() == "Request Timeout"
                     || response.StatusCode.ToString() == "Generic internal server error")
                {
                    log.LogInformation($"SystemandAutoManualFlagging Timer trigger function response: {response.StatusCode.ToString()},{await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"SystemandAutoManualFlagging Timer trigger function exception: {ex.Message},{ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
