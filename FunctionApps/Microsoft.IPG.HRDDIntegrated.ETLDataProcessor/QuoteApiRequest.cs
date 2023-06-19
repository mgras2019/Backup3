using System;

namespace Microsoft.IPG.HRDDIntegrated.ETLDataProcessor
{
    public class QuoteApiRequest
    {
        public Guid Id { get; set; }
        public string QuoteID { get; set; }
        public int RevisionID { get; set; }
    }
}
