using System.Text.Json;
using Employee_Directory_Console_app.Models;

namespace Employee_Directory_Console_app.BussinesLogic
{
    public class RoleOperations
    {
        public List<Role> Rolesinformation;
        public RoleOperations() {
            string jsonData = File.ReadAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\RoleData.json");
            if (string.IsNullOrEmpty(jsonData))
            {
                Rolesinformation = [];
            }
            else
            {
                Rolesinformation = JsonSerializer.Deserialize<List<Role>>(jsonData)!;
            }
        }
        public void SetData(Dictionary<string, string> keyValues)
        {
            Role Role = new()
            {
                Name = keyValues["RoleName"],
                Description = keyValues["RoleDescription"],
                Location = keyValues["Location"],
                Department = keyValues["Department"]
            };
            Rolesinformation.Add(Role);
            SerializeJSONdata(Rolesinformation);
        }
        public static void SerializeJSONdata(List<Role> Rolesinformation)
        {
            string jsonString = JsonSerializer.Serialize(Rolesinformation);
            File.WriteAllText("C:\\Users\\ashwith.p\\source\\repos\\ashwith-p\\Task-5-Csharp-Console-Application\\Database\\RoleData.json", jsonString);
        }
        public List<Role> GetRoleData()
        {
            return Rolesinformation;
        }
    }
}
