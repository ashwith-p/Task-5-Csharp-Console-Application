using BLL.DTO;
using DLL.Model;

namespace BLL.Interfaces
{
    public interface IEmployeeProvider
    {
        public string[] GetStaticData(string name);

        public bool IsValidName(string name);

        public bool IsValidEmployee(string value, string type);

        public List<string> Validator(DTO.Employee employee);

        public void AddEmployee(DTO.Employee emp);

        public string CreateID();

        public bool DeleteEmployee(string email);

        public void SetEmployeeCollection(List<DLL.Model.Employee> employee);

        public List<DTO.Employee>? GetEmployeesInformation();

        public DTO.Employee? GetEmployee(string email);

        public void EditEmployee(DTO.Employee emp, Dictionary<int, string> pair, int choice, string value);

        public DTO.Employee? FindEmployee(string email);
    }
}
