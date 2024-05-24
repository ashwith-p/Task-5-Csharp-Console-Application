using Data.Model;

namespace Data
{
    public class ErrorMessages
    {
        public readonly Dictionary<string, string> messages = new Dictionary<string, string>{

            { nameof(Employee.FirstName), "The FirstName should be minimum of 4 characters" },
            { nameof(Employee.LastName), "The LastName should be minimum of 4 characters" },
            { nameof(Employee.DateOfBirth), "The Date of birth should be of format dd/mm/yyyy and age should be minimum of 18 years" },
            { nameof(Employee.Email), "Email should be of format xyz@xyx.com"},
            { nameof(Employee.MobileNumber),"The Mobile number should be 10 digits and starting with 9,8,7,6" },
            { nameof(Employee.JoiningDate), "The joining date should be of format dd/mm/yyyy" },
            { nameof(Employee.Location), "The Location should be minimum of 4 characters"},
            { nameof(Employee.Department), "Enter the valid input for the department choose any one from below."},
            { nameof(Employee.JobTitle), "The JobTitle should be minimum of 4 characters"},
            { nameof(Employee.Manager), "The Manager should be minimum of 4 characters"}
        };
    }
}
