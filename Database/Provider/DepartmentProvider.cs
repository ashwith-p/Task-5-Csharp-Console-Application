using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Provider
{
    public class DepartmentProvider:IDepartmentProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        public DepartmentProvider(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }
        public Department? GetDepartment(int id)
        {
            return _context.Departments.Where(s=>s.Id == id).FirstOrDefault();
        }
    }
}
