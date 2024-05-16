using DLL.Model;

namespace DLL.Interfaces
{
    public interface IDatabaseOperations
    {
        public void SerializeJSONData<T>(List<T> Information);

        public List<T> GetInformation<T>();

        public void AddData<T>(T data);

        public bool DeleteData(string email);

        public void SetData(List<Employee> data);

        public Employee? FindEmployee(string email);
    }
}
