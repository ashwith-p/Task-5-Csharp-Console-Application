using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IRoleProvider
    {
        public bool IsValidRole(string value, string Key);

        public List<string> SetData(Role role);

        public List<Role> GetRoleData();

        public void AddRole(Role Role);

    }
}
