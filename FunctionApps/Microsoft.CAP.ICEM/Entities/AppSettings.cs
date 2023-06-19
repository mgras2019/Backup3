using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.PAM.ICEM.Entities
{
  public  class AppSettings
    {
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string ICEMResourceUrl { get; set; }

        public string ICEMBaseAddress { get; set; }
        public string RequestorTeamsEndpoint { get; set; }
        public string PartnerTypesEndpoint { get; set; }
        public string PartnerCountriesEndpoint { get; set; }
        public string ReasonsForEscalationEndpoint { get; set; }
        public string InvestigationTeamsEndpoint { get; set; }

        public string CreatePAMEndpoint { get; set; }
        public string PAMInvestigationTeamCode { get; set; }
        public string DefaultReasonForEscalation { get; set; }
        public string EAProgramCode { get; set; }
        public string OPENProgramCode { get; set; }

        public string RequestorTeamCode { get; set; }
    }
}
