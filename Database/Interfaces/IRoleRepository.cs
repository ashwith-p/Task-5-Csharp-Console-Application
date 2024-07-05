using Data.Models;

namespace Data.Interfaces
{
    public interface IRoleRepository
    {
        public IEnumerable<Role> GetRoles();

        public IEnumerable<Employee> GetEmployeesByRole(int id);

        public Role GetRoleById(int id);

        public void Add(Role role);

        public IEnumerable<string> GetRoleNamesByDepartment(int department);
    }
}
