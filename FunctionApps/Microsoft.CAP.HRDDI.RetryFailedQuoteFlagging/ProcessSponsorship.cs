using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.IPG.HRDDIntegrated.CSP.Entities.Sponsorship;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.CAP.HRDDI.RetryFailedQuoteFlagging
{
    public class ProcessSponsorship
    {
        [Function("ProcessSponsorship")]
        public static async Task Run([QueueTrigger("%SponsorshipQueue%", Connection = "AzureStorageConnection")] SponsorshipRequest sponsorshipRequestItem, FunctionContext context)
        {
            var logger = context.GetLogger("ProcessSponsorship");
            logger.LogInformation($"ProcessSponsorship Started for RegistrationId:{sponsorshipRequestItem.RegistrationID}");
            var token = await GetAccessTokenAsync();
            var serviceUrl = Environment.GetEnvironmentVariable("CSPProcessSponsorshipRequestEndPoint");

            var httpClient = new HttpClient();
            StringContent requestContent = new(JsonConvert.SerializeObject(sponsorshipRequestItem), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Concat("Bearer ", token));
            httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("HRDDIApiBaseUrl"));

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            logger.LogInformation($"ProcessSponsorship Calling Hrddi APi for RegistrationId :{sponsorshipRequestItem.RegistrationID}");
            var response = await httpClient.PostAsync(serviceUrl, requestContent);

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                logger.LogInformation($"ProcessSponsorship Calling Hrddi APi Error{response.Content.ReadAsStringAsync()} for :{sponsorshipRequestItem.RegistrationID}");
                throw new ArgumentException($"API call failed, Response : {response.Content.ReadAsStringAsync()}");
            }

            logger.LogInformation($"ProcessSponsorship Ended for RegistrationId:{sponsorshipRequestItem.RegistrationID}");
        }

        private static async Task<string> GetAccessTokenAsync()
        {
            var httpclient = new HttpClient();
            string tokenEndpointUri = Environment.GetEnvironmentVariable("AuthUrl");

            FormUrlEncodedContent content = new(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id",Environment.GetEnvironmentVariable("ClientId")),
                new KeyValuePair<string, string>("client_secret",Environment.GetEnvironmentVariable("ClientSecret")),
                new KeyValuePair<string, string>("directory_id",Environment.GetEnvironmentVariable("TenantId")),
                new KeyValuePair<string, string>("resource", Environment.GetEnvironmentVariable("ResourceUrl"))
                }
            );

            var httpResponseMessage = await httpclient.PostAsync(tokenEndpointUri, content);

            string accessToken;
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseJSON = JObject.Parse(responseString);
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