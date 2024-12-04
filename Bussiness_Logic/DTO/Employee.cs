
namespace BLL.DTO
{
    public class Employee
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? DateOfBirth { get; set; } 

        public string Email { get; set; } = string.Empty;

        public string? MobileNumber { get; set; } 

        public string JoiningDate { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string? Manager { get; set; } 

        public string? Project { get; set; }

        public Employee()
        {}

        public Employee(DLL.Model.Employee emp )
        {
            this.FirstName = emp.FirstName;
            this.LastName = emp.LastName;
            this.DateOfBirth = emp.DateOfBirth;
            this.Email = emp.Email;
            this.MobileNumber = emp.MobileNumber;
            this.Project = emp.Project;
            this.Department = emp.Department;
            this.Manager = emp.Manager;
            this.Location = emp.Location;
            this.JobTitle = emp.JobTitle;
            this.JoiningDate = emp.JoiningDate;
        }
    }

}
