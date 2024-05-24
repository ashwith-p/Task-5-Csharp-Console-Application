using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using Domain.DTO;
using Data.Interfaces;
using Data.Exceptions;
using Domain.Interfaces;
using Data.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

//
namespace Domain.Providers
{
    public class EmployeeProviders : IEmployeeProvider
    {

        private readonly IDatabaseOperations _databaseObj;

        public EmployeeProviders(IDatabaseOperations databaseObj)
        {
            _databaseObj = databaseObj;
        }

        public List<string> GetStaticData(string name, string? value = null)
        {
            if (value != null)
            {
                if (name == nameof(DTO.Employee.JobTitle))
                {
                    return _databaseObj.GetRoleNames(value);
                }
                else if (name == nameof(DTO.Employee.Location))
                {
                    return _databaseObj.GetLocations(value);
                }
                else
                {
                    return _databaseObj.GetStaticData(value);
                }
            }

            return _databaseObj.GetStaticData(name);
        }

        public bool IsValidName(string name)
        {
            return name.Length > 3;
        }

        public bool IsValidEmployee(string value, string type) //name,done
        {
            string data = value.Trim();

            switch (type)
            {

                case nameof(DTO.Employee.FirstName):
                    return IsValidName(data);
                case nameof(DTO.Employee.LastName):
                    return IsValidName(data);
                case nameof(DTO.Employee.DateOfBirth):
                    if (data.Length == 0)
                    {
                        return true;
                    }
                    if (!DateTime.TryParseExact(data, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dateOfBirth))
                    {
                        return false;
                    }
                    int age = DateTime.Now.Year - dateOfBirth.Year;
                    if (dateOfBirth.Date.AddYears(age) > DateTime.Now)
                        age--;
                    return age >= 18 && age <= 90;

                case nameof(DTO.Employee.Email):
                    return Regex.IsMatch(data, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                case nameof(DTO.Employee.MobileNumber):
                    if (data.Length == 0)
                    {
                        return true;
                    }
                    bool success = long.TryParse(data, out long _);
                    if (success && data.Length == 10)
                    {
                        if (int.Parse(data[0].ToString()) > 5)
                        {
                            return true;
                        }
                    }
                    return false;

                case nameof(DTO.Employee.JoiningDate):
                    if (!DateTime.TryParseExact(data, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime joiningDate))
                    {
                        return false;
                    }
                    TimeSpan span = DateTime.Now - joiningDate;
                    return span.TotalDays > 0;

                case nameof(DTO.Employee.Location):
                case nameof(DTO.Employee.JobTitle):
                case nameof(DTO.Employee.Department):
                    return IsValidName(data);
                case nameof(DTO.Employee.Manager):
                case nameof(DTO.Employee.Project):
                    if (string.IsNullOrEmpty(data))
                        return true;
                    return IsValidName(data);

            }
            return false;
        }

        public List<string> Validator(DTO.Employee employee)
        {
            List<string> inCorrect = [];
            Type type = typeof(DTO.Employee);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (!IsValidEmployee(Convert.ToString(prop.GetValue(employee))!, prop.Name))
                {
                    inCorrect.Add(prop.Name);
                }
            }
            return inCorrect;
        }

        public void AddEmployee(DTO.Employee emp)
        {
            string id = "TZ" + this.CreateID();
            Data.Model.Employee employee = new()
            {
                Id = id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                DateOfBirth = emp.DateOfBirth != null ? DateTime.ParseExact(emp.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                Manager = emp.Manager,
                MobileNumber = emp.MobileNumber != null ? long.Parse(emp.MobileNumber) : null,
                JobTitle = emp.JobTitle,
                JoiningDate = DateTime.ParseExact(emp.JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Department = emp.Department,
                Email = emp.Email,
                Location = emp.Location,
                Project = emp.Project,
            };
            _databaseObj.AddEmployee(employee);

        }

        public string CreateID()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new();
            return Convert.ToString(rdm.Next(min, max));
        }

        public bool DeleteEmployee(string email)
        {
            try
            {
                return _databaseObj.DeleteData(email);
            }
            catch (EmployeeIdNotFoundException)
            {
                throw new EmployeeIdNotFoundException();
            }

        }

        public List<DTO.Employee>? GetEmployeesInformation()
        {
            List<DTO.Employee> data = [];
            List<Data.Model.Employee> list = _databaseObj.GetEmployees();
            foreach (Data.Model.Employee employee in list)
            {
                data.Add(new DTO.Employee(employee));
            }
            return data;
        }

        public DTO.Employee? GetEmployee(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                List<Data.Model.Employee> list = _databaseObj.GetEmployees(id);
                return new DTO.Employee(list[0]) ?? throw new EmployeeIdNotFoundException();
            }
            return null;
        }

        public void EditEmployee(Dictionary<int, string> pair, int choice, string value, string email)
        {
            _databaseObj.EditEmployeeData(pair, choice, value, email);
        }
    }
}