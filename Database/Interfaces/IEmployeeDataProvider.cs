using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IEmployeeDataProvider
    {
        public Employee? GetEmployee(string id);

        public IEnumerable<Employee> GetEmployees();

        public IEnumerable<Employee>? GetEmployesUnderManager(string? id);

        public bool Delete(string id);

        public void Add(Employee employee);

        public IEnumerable<string> GetEmployeeNames();

        public void EditEmployee(Dictionary<int, string> pair, int choice, string value, string email);
    }
}
