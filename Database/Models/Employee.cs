
namespace Data.Model
{
    public class Employee
    {
        public string Id { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; } = string.Empty;

        public long? MobileNumber { get; set; }

        public DateTime JoiningDate { get; set; } 

        public string Location { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string? Manager { get; set; }

        public string? Project { get; set; }

    }
}
