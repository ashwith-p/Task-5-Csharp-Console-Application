
using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;
namespace Data.Provider
{
    public class RoleRepository(AshwithEmployeeDirectoryContext context) : IRoleRepository
    {
        private readonly AshwithEmployeeDirectoryContext _context = context;

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }

        public IEnumerable<Employee> GetEmployeesByRole(int id)
        {
            return _context.Employees.Where(s=>s.RoleId==id);
        }

        public Role GetRoleById(int id)
        {
            try
            {
                return _context.Roles.Where(s => s.Id == id).First();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public IEnumerable<string> GetRoleNamesByDepartment(int department)
        {
            return _context.Roles.Where(s=>s.DepartmentId==department).Select(s=>s.Name);
        }
    }
}
