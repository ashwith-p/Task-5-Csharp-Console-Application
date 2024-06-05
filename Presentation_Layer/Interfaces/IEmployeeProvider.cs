namespace EmployeeDirectory.Interfaces
{
    public interface IEmployeeProvider
    {
        
        public void EmployeeDisplayOperations();
        public void ReadEmployee();
        public string GetInputData(string choice, string? message = null);
        public string? GetStaticValues(string choice, string message = "", int? staticValue = null);
        public void DisplayAll();
        public void GetEmployeeById();
        public void EditEmployeeInputService();

    }
}
