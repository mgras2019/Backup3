using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.PAM.ICEM.Entities
{
    public class PAMICEMRequest : PAMRequest
    {
        public PAMICEMRequest(AppSettings appSettings, PAMModules module)
        {
            this.CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            this.ModifiedOn = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            this.Reasonforescalation = appSettings.DefaultReasonForEscalation;
            this.RequestorTeam = appSettings.RequestorTeamCode;
            this.InvestigatingTeam = appSettings.PAMInvestigationTeamCode;
            switch (module)
            {
                case PAMModules.EA:
                    this.PartnerType = appSettings.EAProgramCode;
                    break;
                case PAMModules.OPEN:
                    this.PartnerType = appSettings.OPENProgramCode;
                    break;
                default:
                    break;
            }

        }

        public string CountryId { get; set; }
        public string RegionId { get; set; }
        public string InvestigatingTeam { get; private set; }
        public new string PartnerType { get; private set; }
        public string Reasonforescalation { get; private set; }
        public string Region { get; set; }
        public string RequestorTeam { get; private set; }
        public string CreatedOn { get; private set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; private set; }
    }

    public class PAMICEMRequestData
    {
        public PAMICEMRequest data { get; set; }
    }
}
