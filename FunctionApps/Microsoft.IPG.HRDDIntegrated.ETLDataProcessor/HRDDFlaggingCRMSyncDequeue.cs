using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public class HRDDFlaggingCRMSyncDequeue
    {
        public IHttpClientFactory HttpClientFactory;
        public HttpClient HttpClient;

        public HRDDFlaggingCRMSyncDequeue(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            HttpClient = this.HttpClientFactory.CreateClient();
        }

        [Function("HRDDFlaggingCRMSyncDequeue")]
        public async Task RunAsync([QueueTrigger("CRMCaseOrchestration", Connection = "StorageConnection")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("HRDDFlaggingCRMSyncDequeue");

            try
            {
                logger.LogInformation("Fetching access token");
                Helper helper = new(HttpClientFactory);
                var token = await helper.GetAccessTokenAsync();
                var serviceUrl = Environment.GetEnvironmentVariable("HRDDFlaggingCRMSyncDequeueEndpoint");

                var httpClient = new HttpClient();
                StringContent requestContent = new StringContent(myQueueItem, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));
                httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiBaseUrl"));
                System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

                logger.LogInformation($"making http Call - {serviceUrl}");
                var response = await httpClient.PostAsync(serviceUrl, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"API call response - {await response.Content.ReadAsStringAsync()}");
                    throw new ArgumentException($"API call failed, Response : {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception exception)
            {
                logger.LogError($"Exception occured - {exception.Message}{exception?.InnerException?.Message}");
                throw new ArgumentException($"Exception occured, exeption detail : {exception.Message}{exception?.InnerException?.Message}");
            }
        }
    }
}
