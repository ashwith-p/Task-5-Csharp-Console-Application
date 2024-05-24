using Domain.DTO;
using Data.Interfaces;
using Data.Model;
using System.Reflection;
using Domain.Interfaces;

namespace Domain.Providers
{
    public class RoleProviders : IRoleProvider
    {

        private readonly IDatabaseOperations _databaseObj;

        private readonly IEmployeeProvider _employeeObj;

        public RoleProviders(IDatabaseOperations databaseObj, IEmployeeProvider employeeObj)
        {
            _databaseObj = databaseObj;
            _employeeObj = employeeObj;
        }

        public bool IsValidRole(string value, string key)
        {
            string data = value.Trim();
            switch (key)
            {
                case nameof(DTO.Role.Name):
                    return _employeeObj.IsValidName(data);
                case nameof(DTO.Role.Description):
                    return true;
                case nameof(DTO.Role.Location):
                case nameof(DTO.Role.Department):
                    if (string.IsNullOrEmpty(data))
                    {
                        return false;
                    }
                    return true;
                default: return false;
            }
        }

        public List<string> SetData(DTO.Role role)
        {
            List<string> errorList = [];
            Type type = typeof(DTO.Role);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (!IsValidRole(Convert.ToString(prop.GetValue(role))!, prop.Name))
                {
                    errorList.Add(prop.Name);
                }
            }
            return errorList;
        }

        public List<DTO.Role> GetRoleData()
        {
            List<DTO.Role> roleDTOs = [];
            List<Data.Model.Role> roles = _databaseObj.GetRoles();
            foreach (Data.Model.Role role in roles)
            {
                DTO.Role roleDTO = new()
                {
                    Name = role.Name,
                    Description = role.Description,
                    Location = role.Location,
                    Department = role.Department,
                };
                roleDTOs.Add(roleDTO);
            }
            return roleDTOs;
        }



        public void AddRole(DTO.Role Role)
        {
            Data.Model.Role role = new()
            {
                Id = new Random().Next(100, 999),
                Name = Role.Name,
                Description = Role.Description,
                Location = Role.Location,
                Department = Role.Department,
            };
            _databaseObj.AddRole(role);
        }
    }
}
