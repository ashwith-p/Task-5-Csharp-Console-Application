using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using Domain.DTO;
using Data.Interfaces;
using Data.Exceptions;
using Domain.Interfaces;

//
namespace Domain.Providers
{
    public class EmployeeProviders : IEmployeeProvider
    {

        private readonly IDatabaseOperations _databaseObj;
        private readonly IEmployeeDataProvider _employeeDataProvider;
        private readonly IRoleDataProvider _roleDataProvider;
        private readonly IProjectProvider _projectProvider;
        private readonly ILocationProvider _locationProvider;
        private readonly IDepartmentProvider _departmentProvider;

        public EmployeeProviders(IDatabaseOperations databaseObj,IEmployeeDataProvider employeeDataProvider,IRoleDataProvider roleDataProvider,IDepartmentProvider departmentProvider
            ,ILocationProvider locationProvider,IProjectProvider projectProvider)
        {
            _databaseObj = databaseObj;
            _employeeDataProvider = employeeDataProvider;
            _roleDataProvider = roleDataProvider;
            _projectProvider = projectProvider;
            _locationProvider = locationProvider;
            _departmentProvider = departmentProvider;

        }

        public List<string> GetStaticData(string name, string? value = null)
        {
            if (value != null)
            {
                if (name == nameof(DTO.Employee.JobTitle))
                {
                    return _roleDataProvider.GetRoleNamesByDepartment(value).ToList();
                }
                else if (name == nameof(DTO.Employee.Location))
                {
                    return _locationProvider.GetLocationsByRole(value).ToList();
                }
                else
                {
                    return _employeeDataProvider.GetEmployeeNames().ToList();
                }
            }

            return _databaseObj.GetStaticData(name).ToList();
        }

        public bool IsValidName(string name)
        {
            return name.Length > 3;
        }

        public bool IsValidEmployee(string value, string type) 
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
            string? managerId = emp.Manager != null ? _employeeDataProvider.GetEmployee(emp.Manager) != null ? _employeeDataProvider.GetEmployee(emp.Manager)!.Id : null : null;
            Data.Models.Employee employee = new()
            {
                Id = id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                DateOfBirth = emp.DateOfBirth != null ? DateOnly.ParseExact(emp.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                ManagerId = managerId,
                MobileNumber = emp.MobileNumber != null ? long.Parse(emp.MobileNumber) : null,
                RoleId = _roleDataProvider.GetRoleByName(emp.JobTitle).Id,
                JoiningDate = DateOnly.ParseExact(emp.JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Department = _departmentProvider.GetDepartmentByName(emp.Department),
                Email = emp.Email,
                Location = _locationProvider.GetLocationByName(emp.Location),
                Project = _projectProvider.GetProjectByName(emp.Project),
                ProjectId= _projectProvider.GetProjectByName(emp.Project)!=null? _projectProvider.GetProjectByName(emp.Project)!.Id:null,
                DepartmentId= _departmentProvider.GetDepartmentByName(emp.Department).Id,
                LocationId= _locationProvider.GetLocationByName(emp.Location).Id,
                Role= _roleDataProvider.GetRoleByName(emp.JobTitle),
                Manager = emp.Manager != null ? _employeeDataProvider.GetEmployee(emp.Manager) != null ? _employeeDataProvider.GetEmployee(emp.Manager)!: null : null,
                InverseManager=_employeeDataProvider.GetEmployesUnderManager(managerId)!=null? _employeeDataProvider.GetEmployesUnderManager(managerId)!.ToList():null!,

            };
            _employeeDataProvider.Add(employee);

        }

        public string CreateID()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new();
            return Convert.ToString(rdm.Next(min, max));
        }

        public bool DeleteEmployee(string id)
        {
            try
            {
                return _employeeDataProvider.Delete(id);
            }
            catch (EmployeeIdNotFoundException)
            {
                throw new EmployeeIdNotFoundException();
            }

        }

        public List<DTO.Employee>? GetEmployeesInformation()
        {
            List<DTO.Employee> data = [];
            List<Data.Models.Employee> list = _employeeDataProvider.GetEmployees().ToList();
            foreach (Data.Models.Employee employee in list)
            {
                data.Add(new DTO.Employee(employee,_departmentProvider,_locationProvider,_roleDataProvider,_projectProvider));
            }
            return data;
        }

        public DTO.Employee? GetEmployee(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Data.Models.Employee employee = _employeeDataProvider.GetEmployee(id)!;
                return employee != null ? new DTO.Employee(employee, _departmentProvider, _locationProvider, _roleDataProvider, _projectProvider) : throw new EmployeeIdNotFoundException();
            }
            return null;

        }

        public void EditEmployee(Dictionary<int, string> pair, int choice, string value, string email)
        {
            _employeeDataProvider.EditEmployee(pair, choice, value, email);
        }
    }
}