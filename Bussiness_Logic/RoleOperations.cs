using System.Text.Json;
using Employee_Directory_Console_app.Models;
using Bussiness_Logic;

namespace Employee_Directory_Console_app.BussinesLogic
{
    public class RoleOperations
    {
        public List<Role> Rolesinformation;
        public RoleOperations() {
            Rolesinformation = DatabaseOperations.GetInformation<Role>();
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
            DatabaseOperations.SerializeJSONdata<Role>(Rolesinformation);
        }
        
        public List<Role> GetRoleData()
        {
            return Rolesinformation;
        }
    }
}
