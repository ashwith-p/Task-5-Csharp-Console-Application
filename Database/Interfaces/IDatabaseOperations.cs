using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDatabaseOperations
    {
        

        public void AddRole(Role role);

        public bool DeleteData(string email);

        int? GetId(string table, string name);

        List<string> GetRoleNames(string department);

        List<string> GetStaticData(string role);

        public List<string> GetLocations(string role);

        public List<Role> GetRoles();

        public void AddEmployee(Employee employee);

        public List<Employee> GetEmployees(string? id = null);

        public void EditEmployeeData(Dictionary<int, string> pair, int choice, string value, string email);
    }
}
