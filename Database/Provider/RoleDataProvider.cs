using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;
namespace Data.Provider
{
    public class RoleDataProvider:IRoleDataProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        public RoleDataProvider(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }
        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }
        public Role GetRoleByName(string name)
        {
            return _context.Roles.Where(s=>s.Name==name).First();
        }

        public IEnumerable<Employee> GetEmployeesByRole(int id)
        {
            return _context.Employees.Where(s=>s.RoleId==id);
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.Where(s=>s.Id==id).First();
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public IEnumerable<string> GetRoleNamesByDepartment(string department)
        {
            return _context.Roles.Where(s=>s.DepartmentId==(_context.Departments.Where(d=>d.Name==department).Select(d=>d.Id).First())).Select(s=>s.Name);
        }
    }
}
