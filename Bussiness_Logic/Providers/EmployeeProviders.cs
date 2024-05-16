using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using BLL.DTO;
using DLL.Interfaces;
using DLL.Exceptions;
using BLL.Interfaces;

//
namespace BLL.Providers
{
    public class EmployeeProviders : IEmployeeProvider
    {

        private readonly IDatabaseOperations _databaseObj;

        public EmployeeProviders(IDatabaseOperations databaseObj)
        {
            _databaseObj = databaseObj;
        }

        public string[] GetStaticData(string name)
        {
            //change to model
            Dictionary<string, string[]> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\StaticData.json")) ?? [];
            return keyValuePairs[name];
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
            DLL.Model.Employee employee = new()
            {
                Id = id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                DateOfBirth = emp.DateOfBirth,
                Manager = emp.Manager,
                MobileNumber = emp.MobileNumber,
                JobTitle = emp.JobTitle,
                JoiningDate = emp.JoiningDate,
                Department = emp.Department,
                Email = emp.Email,
                Location = emp.Location,
                Project = emp.Project,
            };
            _databaseObj.AddData(employee);

        }

        public string CreateID()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new();
            return Convert.ToString(rdm.Next(min,max));
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

        public void SetEmployeeCollection(List<DLL.Model.Employee> employee)
        {
            _databaseObj.SetData(employee);
        }

        public DTO.Employee? FindEmployee(string email)
        {
            DLL.Model.Employee? employee=_databaseObj.FindEmployee(email);
            if(employee == null)
            {
                return null;
            }
            return new DTO.Employee(employee);

        }

        public List<DTO.Employee>? GetEmployeesInformation()
        {
            List<DTO.Employee> data = []; 
            List<DLL.Model.Employee> list = _databaseObj.GetInformation<DLL.Model.Employee>();
            foreach (DLL.Model.Employee employee in list)
            {
                data.Add(new DTO.Employee(employee));
            }
            return data;
        }

        public DTO.Employee? GetEmployee(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                DTO.Employee? index = FindEmployee(email);
                return index ?? throw new EmployeeIdNotFoundException();
            }
            return null;
        }

        public void EditEmployee(DTO.Employee emp, Dictionary<int, string> pair, int choice, string value)
        {
            Type prop = typeof(DLL.Model.Employee);
            DLL.Model.Employee? employee = _databaseObj.FindEmployee(emp.Email);
            _databaseObj.DeleteData(emp.Email);
            PropertyInfo propertyValue = prop.GetProperty(pair[choice])!;
            propertyValue.SetValue(employee, value);
            List<DLL.Model.Employee> employees = _databaseObj.GetInformation<DLL.Model.Employee>();
            employees.Add(employee!);
            _databaseObj.SerializeJSONData(employees);
        }
    }
}