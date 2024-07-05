using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;

namespace Data.Provider
{
    public class DepartmentRepository(AshwithEmployeeDirectoryContext context) : IDepartmentRepository
    {
        private readonly AshwithEmployeeDirectoryContext _context = context;

        public Department? GetDepartment(int id)
        {
            return _context.Departments.Where(s=>s.Id == id).FirstOrDefault();
        }

        public IEnumerable<Department> GetAllDepartment() {
            return _context.Departments;
        }
    }
}
