using Microsoft.CAP.ICEM.ApiClient;
using Microsoft.CAP.PAM.ICEM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.Business.Interface
{
   public interface IMetadataService
    {
       public  Task<ApiResponse<List<RequestorTeam>>> GetRequestorTeams();
        public  Task<ApiResponse<List<PartnerType>>> GetPartnerTypes();
        public Task<ApiResponse<List<PartnerCountry>>> GetPartnerCountries();
        public Task<ApiResponse<List<ReasonsForEscalation>>> GetReasonsForEscalation();
        public Task<ApiResponse<List<InvestigationTeam>>> GetInvestigationTeams();
    }
}
