using Data.Models;

namespace Data.Interfaces
{
    public interface IDepartmentProvider
    {
        public Department? GetDepartment(int id);
    }
}
