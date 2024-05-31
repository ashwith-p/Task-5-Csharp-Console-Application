using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDepartmentProvider
    {
        public Department? GetDepartment(int id);

        public IEnumerable<Department> GetDepartments();

        public Department GetDepartmentByName(string name);

        public Department GetDepartmentById(int id);
    }
}
