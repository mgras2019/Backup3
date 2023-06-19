namespace Rightspot.Integrations.Domain.Entities
{
    public class StudentEntity
    {
        public int Id
        { get; set; }

        public string? RollNumber
        { get; set; }

        public string? SchoolCode
        { get; set; }

        public string? InstituteCode
        { get; set; }

        public string? FirstName 
        { get; set; }

        public string? LastName 
        { get; set; }

        public char? MiddleInitial 
        { get; set; }

        public DateTime DateOfBirth
        { get; set; } 

        public char Gender 
        { get; set; }

        public char Class
        { get; set; }

        public char Section
        { get; set; }

        public string? Photo 
        { get; set; }

        public string? LastUpdatedOn
        { get; set; }

        public string? LastUpdatedBy
        { get; set; }
    }
}
