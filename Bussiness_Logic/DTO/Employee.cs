

using Data.Interfaces;
namespace Domain.DTO
{
    public class Employee
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? DateOfBirth { get; set; } 

        public string Email { get; set; } = string.Empty;

        public string? MobileNumber { get; set; } 

        public string JoiningDate { get; set; } = string.Empty;

        public int  Department { get; set; } 

        public int JobTitle { get; set; }

        public int Location { get; set; } 

        public string? Manager { get; set; } 

        public int? Project { get; set; }

        public Employee()
        {}

        public Employee(Data.Models.Employee emp,IDepartmentRepository departmentProvider,ILocationRepository locationProvider,IRoleRepository roleDataProvider,IProjectRepository projectProvider)
        {
            
            this.FirstName = emp.FirstName;
            this.LastName = emp.LastName;
            this.DateOfBirth = emp.DateOfBirth?.ToString();
            this.Email = emp.Email;
            this.MobileNumber = emp.MobileNumber?.ToString();
            this.Project = emp.ProjectId!=null?projectProvider.GetProject(emp.ProjectId)!.Id:null;
            this.Department = departmentProvider.GetDepartment(emp.DepartmentId)!.Id;
            this.Manager = emp.Manager!=null?emp.Manager.FirstName+' '+emp.Manager.LastName:null;
            this.Location = locationProvider.GetLocationById(emp.LocationId).Id;
            this.JobTitle = roleDataProvider.GetRoleById(emp.RoleId).Id;
            this.JoiningDate = emp.JoiningDate.ToString();
        }
    }

}

