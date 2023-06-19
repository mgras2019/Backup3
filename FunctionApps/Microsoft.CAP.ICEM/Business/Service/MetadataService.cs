using Microsoft.CAP.ICEM.ApiClient;
using Microsoft.CAP.ICEM.ApiClient.Interface;
using Microsoft.CAP.ICEM.Business.Interface;
using Microsoft.CAP.PAM.ICEM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.Business.Service
{
    public class MetadataService : IMetadataService
    {
        private IApiHttpClient _apiHttpClient;
        private AppSettings _appSettings;
        public MetadataService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
        {
            this._apiHttpClient = new ApiHttpClient(appSettings, httpClientFactory);
            this._appSettings = appSettings;
        }
        public async Task<ApiResponse<List<InvestigationTeam>>> GetInvestigationTeams()
        {
            return await _apiHttpClient.GetAsync<List<InvestigationTeam>>(_appSettings.InvestigationTeamsEndpoint);

        }

        public async Task<ApiResponse<List<PartnerCountry>>> GetPartnerCountries()
        {
            return await _apiHttpClient.GetAsync<List<PartnerCountry>>(_appSettings.PartnerCountriesEndpoint);
        }

        public async Task<ApiResponse<List<PartnerType>>> GetPartnerTypes()
        {
            return await _apiHttpClient.GetAsync<List<PartnerType>>(_appSettings.PartnerTypesEndpoint);
        }

        public async Task<ApiResponse<List<ReasonsForEscalation>>> GetReasonsForEscalation()
        {
            return await _apiHttpClient.GetAsync<List<ReasonsForEscalation>>(_appSettings.PartnerTypesEndpoint);
        }

        public async Task<ApiResponse<List<RequestorTeam>>> GetRequestorTeams()
        {
            return await _apiHttpClient.GetAsync<List<RequestorTeam>>(_appSettings.RequestorTeamsEndpoint);
        }


    }
}
