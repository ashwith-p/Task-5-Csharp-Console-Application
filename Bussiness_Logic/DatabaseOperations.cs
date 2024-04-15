using Employee_Directory_Console_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bussiness_Logic
{
    internal class DatabaseOperations
    {
        public static void SerializeJSONdata<T>(List<T> Information)
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
        public static List<T> GetInformation<T>()
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
    }
}
