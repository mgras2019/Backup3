using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public class CRMSyncQueue
    {
        public IHttpClientFactory HttpClientFactory;
        public HttpClient HttpClient;

        public CRMSyncQueue(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            HttpClient = this.HttpClientFactory.CreateClient();
        }

        [Function("CRMSyncQueue")]
        public async Task RunAsync([TimerTrigger("%CRMSyncAddToQueueTimeInterval%")] TimerInfo myTimer, FunctionContext context)
        {
            if (!myTimer.IsPastDue)
            {
                var log = context.GetLogger("CRMSyncQueue");

                try
                {
                    log.LogInformation("CRMSyncQueue triggered a request");
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpClient.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("ApiBaseUrl")}");
                    var serviceUrl = Environment.GetEnvironmentVariable("AddCRMDetailsToQueueEndpoint");
                    StringContent requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                    Helper helper = new Helper(HttpClientFactory);
                    var token = await helper.GetAccessTokenAsync();
                    log.LogInformation("Fetched token successfully");

                    HttpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));
                    System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

                    var response = await HttpClient.PostAsync(serviceUrl, requestContent);
                    log.LogInformation($"Triggered HTTP POST call to {serviceUrl}");

                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        log.LogError($" Response - {await response.Content.ReadAsStringAsync()}");
                        throw new ArgumentException($"Failed Calling API {Environment.GetEnvironmentVariable("CRMSyncAddToQueueEndpoint")}, response - {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception exception)
                {
                    log.LogError($"Exception occured  - {exception.Message} {exception?.InnerException?.Message}");
                }
            }

        }
    }

}
