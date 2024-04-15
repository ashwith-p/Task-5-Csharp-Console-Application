using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using Employee_Directory_Console_app.Models;
using Bussiness_Logic;

namespace Employee_Directory_Console_app.BussinesLogic
{
     public class EmployeeOperations
    {
        public List<Employee> EmployeeCollection { get; set; } //name,done

        

        public EmployeeOperations()
        {
            EmployeeCollection = DatabaseOperations.GetInformation<Employee>();
        }
        public static string[] GetStaticData(string name)
        {
            //change to model
            Dictionary<string, string[]> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\StaticData.json")) ?? [];
            return keyValuePairs[name];

        }
        public static bool IsValidName(string name)
        {
            return name.Length > 3;
        }
        public static bool IsEmployeeIdValid(string Id)
        {
            return Id.Length == 6 && Id[..2] == "TZ" && int.TryParse(Id[2..], out _);
        }
        public static bool IsValidEmployee(string value, string type) //name,done
        {
            string data = value.Trim();
            switch (type)
            {
                case nameof(Employee.Id):
                    return IsEmployeeIdValid(data);
                case nameof(Employee.FirstName):
                    return IsValidName(data);
                case nameof(Employee.LastName):
                    return IsValidName(data);
                case nameof(Employee.DateOfBirth):
                    if (data.Length == 0)
                    {
                        return true;
                    }
                    if (!DateTime.TryParseExact(data, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime _))
                    {
                        return false;
                    }

                    string[] format = data.Split('/');
                    DateTime dateofbirth = new(int.Parse(format[2]), int.Parse(format[1]), int.Parse(format[0]));
                    int age = DateTime.Now.Year - dateofbirth.Year;
                    if (dateofbirth.Date.AddYears(age) > DateTime.Now)
                        age--;
                    return (age >= 18 && age <= 90);

                case nameof(Employee.Email):
                    return Regex.IsMatch(data, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                case nameof(Employee.MobileNumber):
                    if (data.Length == 0)
                    {
                        return true;
                    }
                    return (data.Length == 10 && int.Parse(data[0].ToString()) > 5);

                case nameof(Employee.JoiningDate):
                    if (!DateTime.TryParseExact(data, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime _))
                    {
                        return false;
                    }
                    string[] Format = data.Split('/');
                    DateTime Joiningdate = new(int.Parse(Format[2]), int.Parse(Format[1]), int.Parse(Format[0]));
                    TimeSpan span = DateTime.Now - Joiningdate;
                    return (span.TotalDays > 0);


                case nameof(Employee.Location):
                    return IsValidName(data);
                case nameof(Employee.JobTitle):
                    return IsValidName(data);
                case nameof(Employee.Department):
                    if (int.TryParse(data, out int value1))
                    {
                        if (value1 > 0 && value1 < GetStaticData(type).Length + 1)
                        {
                            return true;
                        }
                    }
                    return false;

                case nameof(Employee.Manager):
                    return IsValidName(data);
                case nameof(Employee.Project):
                    if (int.TryParse(data, out int res))
                    {
                        if (res > 0 && res < GetStaticData(type).Length + 1)
                        {
                            return true;
                        }
                    }
                    return false;
            }
            return false;
        }

        public bool DeleteEmployee(string empNo)
        {
            if (empNo.Length == 6 && empNo[..2] == "TZ" && int.TryParse(empNo[2..], out _))
            {
                Employee? index = FindEmployee(empNo);
                if (index!=null)
                {
                    EmployeeCollection.Remove(index);
                    DatabaseOperations.SerializeJSONdata<Employee>(EmployeeCollection);
                    return true;
                }
            }
            return false;
        }

        public void SetEmployeeCollection(Employee employee)
        {
            EmployeeCollection.Add(employee);
            DatabaseOperations.SerializeJSONdata<Employee>(EmployeeCollection);
        }

        
        public Employee? FindEmployee(string empNo)
        {
            if (EmployeeCollection.Count <= 0)
            {
                return null;
            }
            else
            {
                Employee? employee = EmployeeCollection.FirstOrDefault(e => e.Id == empNo);
                return employee;
            }
        }
        public List<Employee> GetEmployeesInformation(string empNo)
        {
            if (string.IsNullOrEmpty(empNo))
            {
                return EmployeeCollection;
            }
            else
            {
                Employee? index = FindEmployee(empNo);
                if (index !=null)
                    return [index];
                return [];
            }
        }
        public static void EditEmployee(Employee emp, Dictionary<int, string> pair, int choice, string value)
        {

            PropertyInfo propertyInfo = typeof(Employee).GetProperty(pair[choice])!;
            propertyInfo.SetValue(emp, value);
        }
    }
}