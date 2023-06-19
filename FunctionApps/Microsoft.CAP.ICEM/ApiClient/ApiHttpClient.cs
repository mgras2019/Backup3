using Microsoft.CAP.ICEM.ApiClient.Interface;
using Microsoft.CAP.PAM.ICEM.Entities;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.ApiClient
{
    public class ApiHttpClient : BaseClient, IApiHttpClient
    {
        /// <summary>
        /// The base address
        /// </summary>
        private readonly AppSettings _appSetting;
        public ApiHttpClient(AppSettings appSettings, System.Net.Http.IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            this._appSetting = appSettings;
            HttpClient.BaseAddress = new Uri(appSettings.ICEMBaseAddress);
        }

        private async Task<string> acquireToken()
        {
            string token = string.Empty;
            try
            {
                string authority = _appSetting.Instance + _appSetting.Domain;


                IConfidentialClientApplication app;
                app = ConfidentialClientApplicationBuilder.Create(_appSetting.ClientId)
                                          .WithClientSecret(_appSetting.ClientSecret)
                                          .WithAuthority(new Uri(authority))
                                          .Build();
                string[] scopes = new string[] { _appSetting.ICEMResourceUrl };

                Identity.Client.AuthenticationResult result = null;
                result = await app.AcquireTokenForClient(scopes)
                     .ExecuteAsync();
                token = result.AccessToken;
            }
            catch(MsalServiceException msalex)
            {
                throw msalex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

            return token;
        }

        /// <summary>
        /// Posts the asynchronous with HTTPContent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPoint">The end point.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<ApiResponse<T>> PostAsync<T>(string endPoint, HttpContent content)
        {

            await this.PrepareHttpClient();
            var url = $"{this._appSetting.ICEMBaseAddress}{endPoint}";
            var responseMessage = await HttpClient.PostAsync(url, content);

            var apiResponse = await this.PopulateResponse<T>(responseMessage);
            return apiResponse;
        }


        public async Task<ApiResponse<T>> GetAsync<T>(string endPoint)
        {
            await this.PrepareHttpClient();

            var url = $"{this._appSetting.ICEMBaseAddress}{endPoint}";
            var responseMessage = await HttpClient.GetAsync(url);
            var apiResponse = await this.PopulateResponse<T>(responseMessage);

            return apiResponse;
        }


        /// <summary>
        /// Add Authorization
        /// </summary>
        /// <param name="value">the value</param>
        public void AddAuthorization(AuthenticationHeaderValue value)
        {
            this.HttpClient.DefaultRequestHeaders.Authorization = value;
        }

        /// <summary>
        /// Prepares the HTTP client.
        /// </summary>
        private async Task<bool> PrepareHttpClient()
        {
            // Add oauth-token 
            this.AddAuthorization(new AuthenticationHeaderValue("Bearer", await acquireToken()));
            return true;

        }

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <param name="endPoint">The endPoint.</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// return response
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ApiResponse<T>> PostAsync<T>(string endPoint, string content)
        {
            
             await   this.PrepareHttpClient();
            

            var url = $"{this._appSetting.ICEMBaseAddress}{endPoint}";
            var responseMessage = await HttpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

            var apiResponse = await this.PopulateResponse<T>(responseMessage);
            return apiResponse;
        }


    }
}
