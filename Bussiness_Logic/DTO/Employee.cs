
using Microsoft.EntityFrameworkCore.Storage;
using Data;
using Data.Models;
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

        public string Department { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string? Manager { get; set; } 

        public string? Project { get; set; }

        public Employee()
        {}

        public Employee(Data.Models.Employee emp,IDepartmentProvider departmentProvider,ILocationProvider locationProvider,IRoleDataProvider roleDataProvider,IProjectProvider projectProvider)
        {
            
            this.FirstName = emp.FirstName;
            this.LastName = emp.LastName;
            this.DateOfBirth = emp.DateOfBirth==null?null:emp.DateOfBirth.ToString();
            this.Email = emp.Email;
            this.MobileNumber = emp.MobileNumber==null?null:emp.MobileNumber.ToString();
            this.Project = emp.ProjectId!=null?projectProvider.GetProject(emp.ProjectId)!.Name:null;
            this.Department = departmentProvider.GetDepartment(emp.DepartmentId)!.Name;
            this.Manager = emp.Manager!=null?emp.Manager.FirstName+' '+emp.Manager.LastName:null;
            this.Location = locationProvider.GetLocationById(emp.LocationId).Name;
            this.JobTitle = roleDataProvider.GetRoleById(emp.RoleId).Name;
            this.JoiningDate = emp.JoiningDate.ToString();
        }
    }

}

