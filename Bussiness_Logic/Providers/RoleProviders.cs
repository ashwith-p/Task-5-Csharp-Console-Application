using Domain.DTO;
using Data.Interfaces;
using Data.Models;
using System.Reflection;
using Domain.Interfaces;

namespace Domain.Providers
{
    public class RoleProviders : IRoleProvider
    {
        private readonly IRoleDataProvider _roleDataProvider;
        private readonly IEmployeeProvider _employeeObj;
        private readonly ILocationProvider _locationProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly IRoleDetailProvider _roleDetailProvider;

        public RoleProviders(IEmployeeProvider employeeObj,IRoleDataProvider roleDataProvider,
            ILocationProvider locationProvider,IDepartmentProvider departmentProvider,IRoleDetailProvider roleDetailProvider)
        {
            _employeeObj = employeeObj;
            _roleDataProvider = roleDataProvider;
            _locationProvider = locationProvider;
            _departmentProvider = departmentProvider;
            _roleDetailProvider = roleDetailProvider;
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
            List<Data.Models.Role> roles=_roleDataProvider.GetRoles().ToList();
            foreach (Data.Models.Role role in roles)
            {
                DTO.Role roleDTO = new()
                {
                    Name = role.Name,
                    Description = role.Description,
                    Location = _locationProvider.GetLocationsByRole(role.Name).ToList(),
                    Department = _departmentProvider.GetDepartment(role.DepartmentId)!.Name,
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
                Id = roleId,
                Name = Role.Name,
                Description = Role.Description,
                DepartmentId = _departmentProvider.GetDepartmentByName(Role.Department).Id,
                Department = _departmentProvider.GetDepartmentByName(Role.Department),
                Employees = _roleDataProvider.GetEmployeesByRole(roleId).ToList(),
            };
            _roleDataProvider.Add(role);
            Data.Models.RoleDetail roleDetail = new() { 
                RoleId = role.Id, 
                LocationId = _locationProvider.GetLocationByName(Role.Location[0]).Id
                ,Role=_roleDataProvider.GetRoleById(roleId),
                Location= _locationProvider.GetLocationByName(Role.Location[0]) };
            
            _roleDetailProvider.Add(roleDetail);
        }
    }
}
