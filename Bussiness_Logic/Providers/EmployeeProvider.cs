using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using Domain.DTO;
using Data.Interfaces;
using Data.Exceptions;
using Domain.Interfaces;
using DataEmployee = Data.Models.Employee;

//
namespace Domain.Providers
{
    public class EmployeeProvider( IEmployeeRepository employeeDataProvider, IRoleRepository roleDataProvider, IDepartmentRepository departmentProvider
            , ILocationRepository locationProvider, IProjectRepository projectProvider) : IEmployeeProvider
    {

        private readonly IEmployeeRepository _employeeDataProvider = employeeDataProvider;
        private readonly IRoleRepository _roleDataProvider = roleDataProvider;
        private readonly IProjectRepository _projectProvider = projectProvider;
        private readonly ILocationRepository _locationProvider = locationProvider;
        private readonly IDepartmentRepository _departmentProvider = departmentProvider;

        public List<string> GetStaticData(string name, int? value = null)
        {
            if (value != null)
            {
                if (name == nameof(DTO.Employee.JobTitle))
                {
                    return _roleDataProvider.GetRoleNamesByDepartment((int)value).ToList();
                }
                else if (name == nameof(DTO.Employee.Location))
                {
                    return _locationProvider.GetLocationsByRole((int)value).ToList();
                }
                else
                {
                    return _employeeDataProvider.GetEmployeeNames().ToList();
                }
            }

            else if(name == nameof(DTO.Employee.Department))
            {
                return _departmentProvider.GetAllDepartment().Select(dept=>dept.Name).ToList();
            }
            return _projectProvider.GetAllProjects().Select(project=>project.Name).ToList();
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
                    return data.Length!=0;
                case nameof(DTO.Employee.Manager):
                case nameof(DTO.Employee.Project):
                    if (string.IsNullOrEmpty(data))
                        return true;
                    return data.Length!=0;

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
            DataEmployee employee = new()
            {
                Id = id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                DateOfBirth = emp.DateOfBirth != null && emp.DateOfBirth != string.Empty ? DateOnly.ParseExact(emp.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null,

                ManagerId = managerId,
                MobileNumber = emp.MobileNumber != null && emp.MobileNumber != string.Empty ? long.Parse(emp.MobileNumber) : null,
                RoleId = emp.JobTitle,
                JoiningDate = DateOnly.ParseExact(emp.JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Department = _departmentProvider.GetDepartment(emp.Department)!,
                Email = emp.Email,
                Location = _locationProvider.GetLocation(emp.Location)!,
                DepartmentId = emp.Department,
                LocationId = emp.Location,
                Role = _roleDataProvider.GetRoleById(emp.JobTitle),
                Manager = emp.Manager != null ? _employeeDataProvider.GetEmployee(emp.Manager) != null ? _employeeDataProvider.GetEmployee(emp.Manager)! : null : null,
                InverseManager = _employeeDataProvider.GetEmployesUnderManager(managerId) != null ? _employeeDataProvider.GetEmployesUnderManager(managerId)!.ToList() : null!,
                Project = _projectProvider.GetProject(emp.Project)
            };
            if (employee.Project != null)
            {
                employee.ProjectId= emp.Project;
            }
            else
            {
                employee.ProjectId = null;
            }
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
            List<Employee> data = [];
            List<DataEmployee> list = _employeeDataProvider.GetEmployees().ToList();
            foreach (DataEmployee employee in list)
            {
                data.Add(new DTO.Employee(employee,_departmentProvider,_locationProvider,_roleDataProvider,_projectProvider));
            }
            return data;
        }

        public DTO.Employee? GetEmployee(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                DataEmployee employee = _employeeDataProvider.GetEmployee(id)!;
                return employee != null ? new DTO.Employee(employee, _departmentProvider, _locationProvider, _roleDataProvider, _projectProvider) : throw new EmployeeIdNotFoundException();
            }
            return null;

        }

        public string? GetValueById(int? id,string name)
        {
            
            return name switch
            {
                nameof(Employee.Location) => _locationProvider.GetLocation((int)id!)?.Name!,
                nameof(Employee.Department) => _departmentProvider.GetDepartment((int)id!)?.Name!,
                nameof(Employee.JobTitle) => _roleDataProvider.GetRoleById((int)id!)?.Name!,
                nameof(Employee.Project) => _projectProvider.GetProject(id)?.Name!,
                _ => string.Empty,
            }; ;
        }

        public void EditEmployee(Dictionary<int, string> pair, int choice, string value, string email)
        {
            _employeeDataProvider.EditEmployee(pair, choice, value, email);
        }
    }
}