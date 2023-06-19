using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.PAM.ICEM.Entities
{
    public class PAMRequest
    {
        [Required]
        public PAMModules PartnerType { get; set; }
        [Required]
        public string PartnerName { get; set; }
        [Required]
        public string PartnerCountry { get; set; }
        [Required]
        public string MPNID { get; set; }
        public string SupplierId { get; set; }
        [Required]
        public string TPID { get; set; }
        [Required]
        public string PartnerOneId { get; set; }
        [Required]
        public string PartnerOneSubId { get; set; }
        [Required]
        public string RequestorName { get; set; }
         [Required]
        public string RequestorEmail { get; set; }
        public string OnBehalfOfName { get; set; }
        public string OnBehalfOfEmail { get; set; }
    }

    public enum PAMModules
    {
        EA = 1,
        OPEN
    }
}
