using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public class EATradeStatusNotification
    {
        public IHttpClientFactory HttpClientFactory;
        public HttpClient HttpClient;

        public EATradeStatusNotification(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            HttpClient = this.HttpClientFactory.CreateClient();
        }

        [Function("EATradeStatusNotification")]
        public async  Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("EATradeStatusNotification");

            if(Convert.ToBoolean(Environment.GetEnvironmentVariable("EnableTradeNotification")) == false)
            {
                logger.LogInformation("TradeNotification is disabled.");
                return req.CreateResponse(HttpStatusCode.OK);
            }
            
            logger.LogInformation("C# HTTP trigger function processed a request.");
            var content = await req.ReadAsStringAsync();

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("ApiBaseUrl")}");
            var serviceUrl = Environment.GetEnvironmentVariable("EATradeNotificationEndPoint");
            StringContent requestContent = new StringContent(content, Encoding.UTF8, "application/json");

            Helper helper = new(HttpClientFactory);
            var token = await helper.GetAccessTokenAsync();
            logger.LogInformation("Fetched token successfully");

            HttpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            var response = await HttpClient.PostAsync(serviceUrl, requestContent);
            logger.LogInformation($"Triggered HTTP POST call to {serviceUrl}");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($" Response - {await response.Content.ReadAsStringAsync()}");
                throw new ArgumentException($"Failed Calling API {Environment.GetEnvironmentVariable("EATradeNotificationEndPoint")}, response - {await response.Content.ReadAsStringAsync()}");
            }

            var result = req.CreateResponse(HttpStatusCode.OK);
            result.Body = new MemoryStream(Encoding.UTF8.GetBytes("Triggered Successfully"));
            return result;
        }
    }
}
