using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.ApiClient
{
   public class BaseClient
    {
        /// <summary>
        /// <summary>
        /// Http client factory.
        /// </summary>
        private readonly IHttpClientFactory HttpClientFactory;

        /// <summary>
        /// The HTTP client
        /// </summary>
        protected readonly HttpClient HttpClient;

        public BaseClient(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
            this.HttpClient = HttpClientFactory.CreateClient();
        }

        /// <summary>
        /// Populates the response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpResponseMessage">The HTTP response message.</param>
        /// <returns></returns>
        public virtual async Task<ApiResponse<T>> PopulateResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var response = new ApiResponse<T>();
            try
            {
                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    response.Value = JsonConvert.DeserializeObject<T>(jsonString);
                }
                else if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest
                    || httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
                {
                    response.Error = new ApiError() { Message = jsonString };
                }
                else if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Error = new ApiError() { Message = "Unauthorized" };
                }
            }
            catch (UnauthorizedAccessException)
            {
                response.Error = new ApiError() { Message = "Unauthorized" };
            }
            catch (Exception ex)
            {
                response.Error = new ApiError() { Message = ex.Message };
            }

            return response;
        }
    }
}
