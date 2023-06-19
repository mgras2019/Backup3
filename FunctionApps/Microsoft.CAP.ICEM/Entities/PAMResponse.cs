using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.Entities
{
   public class PAMResponse
    {
        public string id { get; set; }
        public object parentId { get; set; }
        public object parentInvestigationPlanId { get; set; }
        public string requestNumber { get; set; }
        public string title { get; set; }
        public object description { get; set; }
        public string reasonForEscalation { get; set; }
        public object requestType { get; set; }
        public string investigationGroup { get; set; }
        public string originatingTeam { get; set; }
        public string requestor { get; set; }
        public object intakeManager { get; set; }
        public object triageManager { get; set; }
        public object investigator { get; set; }
        public object agent { get; set; }
        public List<object> categories { get; set; }
        public List<object> subCategories { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
    }
}
