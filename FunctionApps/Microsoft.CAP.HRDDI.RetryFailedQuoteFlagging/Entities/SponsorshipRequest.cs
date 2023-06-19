namespace Microsoft.IPG.HRDDIntegrated.CSP.Entities.Sponsorship
{
    public class SponsorshipRequest
    {
        public string RequestID { get; set; }

        public string RegistrationID { get; set; }

        public int ReviewId { get; set; }

        public string Comment { get; set; }

        public string ReviewStatus { get; set; }

        public string ReviewedBy { get; set; }

        public string ReviewedOn { get; set; }
    }
}