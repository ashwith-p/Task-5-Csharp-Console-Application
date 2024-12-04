using System.Text.Json;
using DLL.Model;
using DLL.Interfaces;
using DLL.Exceptions;

namespace DLL
{
    public class DatabaseOperations:IDatabaseOperations
    {
        public void SerializeJSONData<T>(List<T> Information)
        {
            string jsonString = JsonSerializer.Serialize(Information);
            if(typeof(T)==typeof(Employee))
            {
                File.WriteAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\EmployeeData.json", jsonString);
            }
            else if(typeof(T)==typeof(Role))
            {
                File.WriteAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\RoleData.json", jsonString);
            }
        }

        public List<T> GetInformation<T>()
        {
            string jsonData=string.Empty;
            if (typeof(T) == typeof(Employee))
            {
                jsonData=File.ReadAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\EmployeeData.json");
            }
            else if (typeof(T) == typeof(Role))
            {
                jsonData=File.ReadAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\RoleData.json");
            }
            if (string.IsNullOrEmpty(jsonData))
            {
                return [];
            }
            else
            {
                return JsonSerializer.Deserialize<List<T>>(jsonData)!;
            }
        }

        public void AddData<T>(T data)
        {
            List<T> dataList=GetInformation<T>();
            dataList.Add(data);
            SerializeJSONData(dataList);
        }

        public bool DeleteData(string email)
        {
            List<Employee> dataList = GetInformation<Employee>();
            Employee? index = dataList.FirstOrDefault(e => e.Email == email) ?? throw new EmployeeIdNotFoundException();
            bool result = dataList.Remove(index);
            SerializeJSONData(dataList);
            return result;
        }

        public void SetData(List<Employee> data)
        {
            if (data != null)
            {
                SerializeJSONData(data);
            }
        }

        public Employee? FindEmployee(string email)
        {
            if (GetInformation<Employee>() != null)
            {
                Employee? employee = GetInformation<Employee>().FirstOrDefault(e => e.Email == email);
                return employee;
            }
            return null;

        }
    }
}
