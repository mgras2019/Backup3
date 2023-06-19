using Microsoft.CAP.ICEM.ApiClient;
using Microsoft.CAP.ICEM.ApiClient.Interface;
using Microsoft.CAP.ICEM.Business.Interface;
using Microsoft.CAP.ICEM.Entities;
using Microsoft.CAP.PAM.ICEM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.Business.Service
{
    public class PAMRequestService : IPAMRequestService
    {

        private IApiHttpClient _apiHttpClient;
        private AppSettings _appSettings;
        private IMetadataService _metadataService;
        public PAMRequestService(AppSettings appSettings, IHttpClientFactory httpClientFactory, IMetadataService metadataService)
        {
            this._apiHttpClient = new ApiHttpClient(appSettings, httpClientFactory);
            this._appSettings = appSettings;
            _metadataService = metadataService;
        }

        public async Task<ApiResponse<PAMResponse>> Create(PAMRequest pAMRequest)
        {
            try
            {
                var partnerCountries = await _metadataService.GetPartnerCountries();

                

                if (partnerCountries != null && partnerCountries.Error == null && partnerCountries.Value != null && partnerCountries.Value.Count > 0
                  && pAMRequest!=null && !String.IsNullOrEmpty(pAMRequest.PartnerCountry) && partnerCountries.Value.Exists(c => c.CountryName.ToLower() == pAMRequest.PartnerCountry.ToLower()))
                {
                    var country = partnerCountries.Value.First(c => c.CountryName.ToLower() == pAMRequest.PartnerCountry.ToLower());
                    PAMICEMRequest pAMICEM = new PAMICEMRequest(this._appSettings, pAMRequest.PartnerType);
                    pAMICEM.CreatedBy = pAMRequest.RequestorName;
                    pAMICEM.ModifiedBy = pAMRequest.RequestorName;
                    pAMICEM.MPNID = pAMRequest.MPNID;
                    pAMICEM.OnBehalfOfEmail = pAMRequest.OnBehalfOfEmail;
                    pAMICEM.OnBehalfOfName = pAMRequest.OnBehalfOfName;
                    pAMICEM.CountryId = country.CountryId;
                    pAMICEM.PartnerName = pAMRequest.PartnerName;
                    pAMICEM.PartnerOneId = pAMRequest.PartnerOneId;
                    pAMICEM.PartnerOneSubId = pAMRequest.PartnerOneSubId;
                    pAMICEM.RequestorName = pAMRequest.RequestorName;
                    pAMICEM.RequestorEmail = pAMRequest.RequestorEmail;
                    pAMICEM.RegionId = country.RegionId;
                    pAMICEM.TPID = pAMRequest.TPID;

                    PAMICEMRequestData requestData = new PAMICEMRequestData();
                    requestData.data = pAMICEM;
                    return await _apiHttpClient.PostAsync<PAMResponse>(this._appSettings.CreatePAMEndpoint, JsonSerializer.Serialize(requestData));
                }
                else
                {
                    ApiResponse<PAMResponse> invalidCountry = new ApiResponse<PAMResponse>();
                    invalidCountry.Error = new ApiError() { Message = "PartnerCountry is invalid" };
                    return invalidCountry;
                }
            }
            catch (Exception ex)
            {

                ApiResponse<PAMResponse> exceptionError = new ApiResponse<PAMResponse>();
                exceptionError.Error = new ApiError() { Message = ex.Message };
                return exceptionError;
            }

        }
    }
}
