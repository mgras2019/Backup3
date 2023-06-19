using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public static class CSPPOStatusDeQueue
    {
        [Function("CSPPOStatusDeQueue")]
        public async static Task Run([QueueTrigger("updatepostatus", Connection = "StorageConnection")] string myQueueItem, FunctionContext context)
        {
            var logger = context.GetLogger("CSPPOStatusDeQueue");

            try
            {
                logger.LogInformation("Fetching access token");
                var token = await GetAccessTokenAsync();
                var serviceUrl = Environment.GetEnvironmentVariable("CSPUpdateQuoteStatusEndPoint");

                var request = Newtonsoft.Json.JsonConvert.DeserializeObject<QuoteApiRequest>(myQueueItem);

                logger.LogInformation($"Queue Items QuoteID - {request.QuoteID}, RevisionID - {request.RevisionID}");
                var httpClient = new HttpClient();
                StringContent requestContent = new StringContent(myQueueItem, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));
                httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiBaseUrl"));
                System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

                logger.LogInformation($"making http Call - {serviceUrl}");
                var response = await httpClient.PostAsync(serviceUrl, requestContent);

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
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

        private static async Task<string> GetAccessTokenAsync()
        {
            var httpclient = new HttpClient();
            string tokenEndpointUri = Environment.GetEnvironmentVariable("AuthUrl");

            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id",Environment.GetEnvironmentVariable("ClientId")),
                new KeyValuePair<string, string>("client_secret",Environment.GetEnvironmentVariable("ClientSecret")),
                new KeyValuePair<string, string>("directory_id",Environment.GetEnvironmentVariable("DirectoryID")),
                new KeyValuePair<string, string>("resource", Environment.GetEnvironmentVariable("ResourceUrl"))
                }
            );

            var httpResponseMessage = await httpclient.PostAsync(tokenEndpointUri, content);

            string accessToken;
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
    }
}
