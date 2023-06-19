namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class CSPDealsSystemFlagging
    {
        public readonly HttpClient httpClient;
        public readonly IHttpClientFactory httpClientFactory;

        public CSPDealsSystemFlagging(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpClient = httpClientFactory.CreateClient();
        }

        [Function("CSPDealsSystemFlagging")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            var log = executionContext.GetLogger("CSPDealsSystemFlagging");
            try
            {
                await UpdateFlaggedRecords(log);
                log.LogInformation("CSPDealsSystemFlagging trigger function processed a request.");
                var result = req.CreateResponse(HttpStatusCode.OK);
                result.Body = new MemoryStream(Encoding.UTF8.GetBytes("Triggered Successfully"));
                return result;
            }
            catch (Exception exception)
            {
                log.LogError($"CSPDealsSystemFlagging trigger function exception occured - {exception.Message}{ exception.InnerException?.Message}");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private async Task UpdateFlaggedRecords(ILogger log)
        {
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("ApiBaseUrl")}");
            var serviceUrl = Environment.GetEnvironmentVariable("CSPUpdateFlaggingEndPoint");
            StringContent requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            Helper helper = new Helper(this.httpClientFactory);
            var token = await helper.GetAccessTokenAsync();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));

            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var response = await this.httpClient.PostAsync(serviceUrl, requestContent);

            if (response.StatusCode.ToString() == "BadRequest" || response.StatusCode.ToString() == "InternalServerError" || response.StatusCode.ToString() == "unauthorized" || response.StatusCode.ToString() == "Request Timeout"
                 || response.StatusCode.ToString() == "Generic internal server error")
            {
                log.LogInformation($"CSPDealsSystemFlagging Timer trigger function response: {response.StatusCode},{await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
