using Data.Models;

namespace Data.Interfaces
{
    public interface IEmployeeRepository
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
