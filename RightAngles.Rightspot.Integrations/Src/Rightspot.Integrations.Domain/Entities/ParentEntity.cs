namespace Rightspot.Integrations.Domain.Entities
{
    public class ParentEntity
    {
        public string? Id
        { get; set; }

        public string? Name
        { get; set; }

        public char Email
        { get; set; }

        public char Address
        { get; set; }

        public char HomePhone
        { get; set; }

        public char MobileNumber
        { get; set; }

        public string? PhotoBlobUrl
        { get; set; }

        public bool IsPrimary
        { get; set; }

        public string? StudentId
        { get; set; }

        public string? LastUpdatedOn
        { get; set; }

        public string? LastUpdatedBy
        { get; set; }
    }
}
