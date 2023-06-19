using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public class ScreeningNotification
    {
        public IHttpClientFactory HttpClientFactory;

        public ScreeningNotification(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
        }

        [Function(nameof(ScreeningNotification))]
        public async Task RunAsync([TimerTrigger("%ScreeningTimeInterval%")] TimerInfo myTimer,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ScreeningNotification");
            var pamServiceUrl = Environment.GetEnvironmentVariable("ScreeningNotificationPAMEndPoint");
            var dneServiceUrl = Environment.GetEnvironmentVariable("ScreeningNotificationDNEEndPoint");
            var executedTasks = new List<Task<HttpResponseMessage>>();
            var pamTask = this.ExecuteQuoteAPI(pamServiceUrl, logger);
            var dneTask = this.ExecuteQuoteAPI(dneServiceUrl, logger);
            executedTasks.Add(pamTask);
            executedTasks.Add(dneTask);
            await Task.WhenAll(executedTasks);
        }

        private async Task<HttpResponseMessage> ExecuteQuoteAPI(string serviceUrl, ILogger logger)
        {
            var token = await this.GetAccessToken();
            using var httpClient = this.HttpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("ApiBaseUrl")}");
            var data = new StringContent(string.Empty);
            httpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var response = await httpClient.PostAsync(serviceUrl, data);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($" Response - {serviceUrl} - {await response.Content.ReadAsStringAsync()}");
            }

            return response;
        }

        private async Task<string> GetAccessToken()
        {
            Helper helper = new(HttpClientFactory);
            return await helper.GetAccessTokenAsync();
        }
    }
}