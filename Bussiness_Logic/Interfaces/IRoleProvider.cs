using Domain.DTO;

namespace Domain.Interfaces
{
    public interface IRoleProvider
    {
        public bool IsValidRole(string value, string Key);

        public List<string> SetData(Role role);

        public List<Role> GetRoleData();

        public string GetLocationById(int id);

        public string GetDepartmentById(int id);

        public void AddRole(Role Role);

    }
}
