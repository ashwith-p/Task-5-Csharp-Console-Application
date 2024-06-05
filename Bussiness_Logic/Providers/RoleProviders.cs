using Domain.DTO;
using Data.Interfaces;
using System.Reflection;
using Domain.Interfaces;

using DataEmployee = Data.Models.Employee;

namespace Domain.Providers
{
    public class RoleProviders(IEmployeeProvider employeeObj, IRoleDataProvider roleDataProvider,
        ILocationProvider locationProvider, IDepartmentProvider departmentProvider, IRoleDetailProvider roleDetailProvider) : IRoleProvider
    {
        private readonly IRoleDataProvider _roleDataProvider = roleDataProvider;
        private readonly IEmployeeProvider _employeeObj = employeeObj;
        private readonly ILocationProvider _locationProvider = locationProvider;
        private readonly IDepartmentProvider _departmentProvider = departmentProvider;
        private readonly IRoleDetailProvider _roleDetailProvider = roleDetailProvider;

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

        public string GetLocationById(int id)
        {
            return _locationProvider.GetLocationById(id).Name;
        }

        public string GetDepartmentById(int id)
        {
            return _departmentProvider.GetDepartment(id)!.Name;
        }

        public List<DTO.Role> GetRoleData()
        {
            List<DTO.Role> roleDTOs = [];
            List<Data.Models.Role> roles=_roleDataProvider.GetRoles().ToList();
            foreach (Data.Models.Role role in roles)
            {
                DTO.Role roleDTO = new()
                {
                    Name = role.Name,
                    Description = role.Description,
                    Location = _locationProvider.GetLocationIdsByRole(role.Id).ToList(),
                    Department =role.DepartmentId,
                };
                roleDTOs.Add(roleDTO);
            }
            return roleDTOs;
        }

        public void AddRole(DTO.Role Role)
        {
            int roleId = new Random().Next(100, 999);
            Data.Models.Role role = new()
            {
                Name = Role.Name,
                Description = Role.Description,
                DepartmentId = Role.Department,
                Department = _departmentProvider.GetDepartment(Role.Department)!,
                Employees = _roleDataProvider.GetEmployeesByRole(roleId).ToList(),
            };
            _roleDataProvider.Add(role);
            Data.Models.RoleDetail roleDetail = new() { 
                RoleId = role.Id, 
                LocationId = Role.Location[0]
                ,Role=_roleDataProvider.GetRoleById(role.Id),
                Location= _locationProvider.GetLocationById(Role.Location[0]) };
            
            _roleDetailProvider.Add(roleDetail);
        }
    }
}
