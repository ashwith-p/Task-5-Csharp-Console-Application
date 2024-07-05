using Data.Models;

namespace Data.Interfaces
{
    public interface IDepartmentRepository
    {
        public Department? GetDepartment(int id);

        public IEnumerable<Department> GetAllDepartment();
        
     }
}
