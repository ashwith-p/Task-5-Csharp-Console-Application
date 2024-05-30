
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

        public Employee(Data.Models.Employee emp)
        {
            
            this.FirstName = emp.FirstName;
            this.LastName = emp.LastName;
            this.DateOfBirth = emp.DateOfBirth==null?null:emp.DateOfBirth.ToString();
            this.Email = emp.Email;
            this.MobileNumber = emp.MobileNumber==null?null:emp.MobileNumber.ToString();
            this.Project = emp.Project!=null?emp.Project.Name:null;
            this.Department = emp.Department.Name;
            this.Manager = emp.Manager!=null?emp.Manager.FirstName+' '+emp.Manager.LastName:null;
            this.Location = emp.Location.Name;
            this.JobTitle = emp.Role.Name;
            this.JoiningDate = emp.JoiningDate.ToString();
        }
    }

}

