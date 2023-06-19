namespace Microsoft.IPG.HRDDIntegrated.Reporting
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Entities;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Enums;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Report Generator
    /// </summary>
    public class ReportGenerator
    {
        private readonly HttpClient HttpClient;

        private readonly System.Net.Http.IHttpClientFactory HttpClientFactory;

        public ReportGenerator(System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            this.HttpClient = this.HttpClientFactory.CreateClient();
        }

        [Function("ExportToExcel")]
        public async Task Run([QueueTrigger("%ExcelExportQueue%", Connection = "AzureStorageConnection")] string message, FunctionContext context)
        {
            // Call to get the logger
            var log = context.GetLogger("ExportToExcel");
            log.LogInformation($"C# Queue trigger function processed: {message}");

            // TODO: SignalR notifications to UI
            ExcelExportRequest exportRequest = JsonConvert.DeserializeObject<ExcelExportRequest>(message);

            DealService dealService = new DealService();
            bool isUploaded = false;

            //When filtertype is 0 , generate excel for all quotes
            if (exportRequest.DealRequest.FilterInput.FilterType == 0)
            {
                isUploaded = await dealService.GenerateExcelReportAllQuotes(exportRequest, log);
            }
            // Failed MS Quote Records - Flagged or Unflagged
            else if (exportRequest.DealRequest.FilterInput.FilterType == 21 || exportRequest.DealRequest.FilterInput.FilterType == 22)
            {
                isUploaded = await dealService.GenerateExcelReportforMSQuoteFailedQuotes(exportRequest, log);
            }
            // CRM Failed Quotes Records
            else if (exportRequest.DealRequest.FilterInput.FilterType == 24)
            {
                isUploaded = await dealService.GenerateExcelReportforCRMFailedQuotes(exportRequest, log);
            }
            // Pilot Yes or No Records
            else if (exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotY || exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotN)
            {
                isUploaded = await dealService.GenerateExcelReportforPilotQuotes(exportRequest, log);
            }
            else
            {
                isUploaded = await dealService.GenerateExcelReport(exportRequest, log);
            }

            string blobname = $"{exportRequest.JobId}.csv";
            string blobUrl = $"{Environment.GetEnvironmentVariable("ExcelBlobBaseUrl")}/{blobname}";

            if (isUploaded)
            {
                Dictionary<string, string> msgParams = new Dictionary<string, string>();
                if (exportRequest.DealRequest.FilterInput.FilterType == 0)
                {
                    msgParams.Add("FLG_UNFLG", "All Deals");
                }
                else if (exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotN || exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotY)
                {
                    msgParams.Add("FLG_UNFLG", exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PilotY ? "Pilot Deals" : "Non Pilot Deals");
                }
                else if (exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.DNE)
                {
                    msgParams.Add("FLG_UNFLG", "TNA Deals");
                }
                else if (exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.PAM)
                {
                    msgParams.Add("FLG_UNFLG", "PAM Deals");
                }
                else if (exportRequest.DealRequest.FilterInput.FilterType == (int)QuoteHeaders.SOE)
                {
                    msgParams.Add("FLG_UNFLG", "SOE Deals");
                }
                else
                {
                    msgParams.Add("FLG_UNFLG", exportRequest.DealRequest.IsUnflaggedDeal ? "Unflagged Deals" : "Flagged Deals");
                }
                msgParams.Add("USER_NAME", exportRequest.DealRequest.CurrentUser.Name);
                msgParams.Add("NAVURL", blobUrl);
                msgParams.Add("RPT_FILTERS", ConvertToHtmlBulletList(exportRequest.DealRequest.FilterInput));

                string subject = "HRDD - High Risk Deals Desk Excel Report";
                ApplicationEmailMessageRequest applicationEmailMessageRequest = new ApplicationEmailMessageRequest()
                {
                    EmailMessages = new List<MailMessageRequest>()
                    {
                       new MailMessageRequest()
                       {
                            To = exportRequest.DealRequest.CurrentUser.Email,
                            Priority = MailPriority.High,
                            Subject = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("Environment")) 
                                    ? subject
                                    : String.Concat(Environment.GetEnvironmentVariable("Environment"), subject),
                            TemplateId = "EXCEL_DOWNLOAD",
                            TemplateData = JsonConvert.SerializeObject(msgParams)
                       }
                    }
                };
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //send email notification
                var response = await SendMessageNotification(applicationEmailMessageRequest.EmailMessages);
                stopwatch.Stop();
                log.LogInformation("ReportGenerator - Time taken for SendMessageNotification : " + stopwatch.Elapsed.TotalMinutes + " Mins");
            }
        }

        /// <summary>
        /// Convert the filter to commma separated value
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private string ConvertToHtmlBulletList(FilterInput filter)
        {
            StringBuilder sb = new StringBuilder();
            if (filter.QuoteID != null && filter.QuoteID != 0)
            {
                sb.Append(string.Format("<li>Quote Id: {0}</li>", filter.QuoteID));
            }
            if (filter.SelectedArea != null)
            {
                sb.Append(string.Format("<li>Area: {0}</li>", CommaFormat(filter.SelectedArea)));
            }
            if (filter.SelectedSalesLocation != null)
            {
                sb.Append(string.Format("<li>Sales Location: {0}</li>", filter.SelectedSalesLocation.Replace("~|~", ", ")));
            }
            if (filter.CustomerName != null)
            {
                sb.Append(string.Format("<li>Customer : {0}</li>", filter.CustomerName));
            }
            if (filter.PartnerName != null)
            {
                sb.Append(string.Format("<li>Partner : {0}</li>", filter.PartnerName));
            }
            //Commented as we do not want to display the state ids
            //if (filter.SelectedState != null )
            //{
            //    sb.Append(string.Format("<li>State : {0}</li>", filter.SelectedState));
            //}
            if (filter.AssignedTo != null)
            {
                sb.Append(string.Format("<li>Assigned To : {0}</li>", filter.AssignedTo));
            }
            if (filter.MlPrediction != null)
            {
                sb.Append(string.Format("<li>ML Prediction : {0}</li>", CommaFormat(filter.MlPrediction)));
            }
            if (filter.QuarterFirstFlagged != null)
            {
                sb.Append(string.Format("<li>Quarter First Flagged : {0}</li>", CommaFormat(filter.QuarterFirstFlagged)));
            }
            if (filter.ManuallyFlaggedCategory != null)
            {
                sb.Append(string.Format("<li>Manually Flagged Category : {0}</li>", CommaFormat(filter.ManuallyFlaggedCategory)));
            }
            if (filter.TradeScreenStatus != null)
            {
                sb.Append(string.Format("<li>Trade Screening Status : {0}</li>", CommaFormat(filter.TradeScreenStatus)));
            }
            if (filter.RiskScoreFbi != null)
            {
                sb.Append(string.Format("<li>Risk Score (FBI) : {0}</li>", filter.RiskScoreFbi));
            }
            if (filter.RiskScoreUst != null)
            {
                sb.Append(string.Format("<li>Risk Score(UST) : {0}</li>", filter.RiskScoreUst));
            }
            if (filter.LandingProbability != null)
            {
                sb.Append(string.Format("<li>Landing Probability : {0}</li>", filter.LandingProbability));
            }
            if (filter.Segmentation != null)
            {
                sb.Append(string.Format("<li>Segmentation : {0}</li>", CommaFormat(filter.Segmentation)));
            }
            if (filter.InScope != null)
            {
                sb.Append(string.Format("<li>InScope : {0}</li>", CommaFormat(filter.InScope)));
            }
            if (sb.Length == 0)
            {
                sb.Append("None");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Send message notification to the service adapter 
        /// </summary>
        /// <param name="emailMessages"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendMessageNotification(List<MailMessageRequest> emailMessages)
        {
            var token = await AcquireToken(Environment.GetEnvironmentVariable("CAPEmailResource"));

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("EmailServiceUrl"));
            HttpClient.DefaultRequestHeaders.Add("Authorization", String.Concat("Bearer ", token));

            var serializedRequest = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(emailMessages)).ConfigureAwait(false);
            var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
            return await HttpClient.PostAsync(Environment.GetEnvironmentVariable("CAPSendEmailEndpoint"), httpContent);
        }

        /// <summary>
        ///  Acquire application token.
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        private async Task<string> AcquireToken(string resourceUrl)
        {
            string authority = Environment.GetEnvironmentVariable("Instance") + Environment.GetEnvironmentVariable("Domain");
            var clientCredential = new ClientCredential(Environment.GetEnvironmentVariable("ClientId"), Environment.GetEnvironmentVariable("ClientSecret"));
            AuthenticationContext context = new AuthenticationContext(authority, false);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(resourceUrl, clientCredential);

            return authenticationResult.AccessToken;
        }

        /// <summary>
        /// Making Comma format readable
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string CommaFormat(string filter)
        {
            return filter.Replace(",", ", ");
        }
    }


}
