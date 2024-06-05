
namespace Domain.Interfaces
{
    public interface IEmployeeProvider
    {
        public List<string> GetStaticData(string name, int? value = null);

        public bool IsValidName(string name);

        public bool IsValidEmployee(string value, string type);

        public List<string> Validator(DTO.Employee employee);

        public void AddEmployee(DTO.Employee emp);

        public string CreateID();

        public bool DeleteEmployee(string email);

        public List<DTO.Employee>? GetEmployeesInformation();

        public DTO.Employee? GetEmployee(string email);

        public string? GetValueById(int? id, string name);

        public void EditEmployee(Dictionary<int, string> pair, int choice, string value, string email);

    }
}
