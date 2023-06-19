using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.ApiClient.Interface
{
    public interface IApiHttpClient
    {
        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <param name="endPoint">The endPoint.</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// return response
        /// </returns>
        Task<ApiResponse<T>> PostAsync<T>(string endPoint, string content);

        /// <summary>
        /// Get Asynchronous 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        Task<ApiResponse<T>> GetAsync<T>(string endPoint);


        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPoint">The end point.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<ApiResponse<T>> PostAsync<T>(string endPoint, HttpContent content);
    }
}
