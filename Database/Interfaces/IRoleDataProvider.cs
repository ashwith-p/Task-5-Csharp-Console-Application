using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRoleDataProvider
    {
        public IEnumerable<Role> GetRoles();

        public Role GetRoleByName(string name);

        public IEnumerable<Employee> GetEmployeesByRole(int id);

        public Role GetRoleById(int id);

        public void Add(Role role);

        public IEnumerable<string> GetRoleNamesByDepartment(string department);
    }
}
