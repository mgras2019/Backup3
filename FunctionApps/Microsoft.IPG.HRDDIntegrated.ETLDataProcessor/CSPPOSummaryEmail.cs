using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public class CSPPOSummaryEmail
    {
        private readonly HttpClient HttpClient;

        private readonly IHttpClientFactory HttpClientFactory;

        public CSPPOSummaryEmail(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            this.HttpClient = HttpClientFactory.CreateClient();
        }

        [Function("CSPPOSummaryEmail")]
        public void Run([TimerTrigger("%timeintervalCSPPOSummaryTrigger%"
            
#if DEBUG
    , RunOnStartup=true
#endif
            )] TimerInfo myTimer, FunctionContext context)
        {
            var log = context.GetLogger("CSPPOSummaryEmail");
            if (!myTimer.IsPastDue)
            {
                log.LogInformation($"CSP PO Summary Email function execution started at: {DateTime.Now}");
                Helper helper = new Helper(HttpClientFactory);

                string token = helper.GetAccessTokenAsync().Result;
                var serviceUrl = Environment.GetEnvironmentVariable("CSPPOSummaryEmailEndpoint");
                _ = TriggerPOSummaryEmailsAsync(token, serviceUrl, log);
                log.LogInformation($"CSP PO Summary Email Timer trigger function execution completed at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"CSP PO Summary Email function skipped because of running later: {DateTime.Now}");
            }
        }

        /// <summary>
        /// APIs the call.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="serviceUrl">The service URL.</param>
        private async Task TriggerPOSummaryEmailsAsync(string token, string serviceUrl, ILogger log)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("ApiBaseUrl")}{serviceUrl}");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.Timeout = TimeSpan.FromMilliseconds(200000000);

                System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var response = await httpClient.GetAsync(serviceUrl);

                if (response.StatusCode.ToString() == "BadRequest" || response.StatusCode.ToString() == "InternalServerError" || response.StatusCode.ToString() == "unauthorized" || response.StatusCode.ToString() == "Request Timeout"
                     || response.StatusCode.ToString() == "Generic internal server error")
                {
                    log.LogInformation($"CSP PO Summary Email Timer trigger function response: {response.StatusCode.ToString()},{await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"CSP PO Summary Email Timer trigger function exception: {ex.Message},{ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
