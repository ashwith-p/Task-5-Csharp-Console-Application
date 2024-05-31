using EmployeeDirectory.Utilities.Helpers;
using Domain.DTO;
using System.Reflection;
using Domain.Providers;
using EmployeeDirectory.Interfaces;
using Data.Models;

namespace EmployeeDirectory.Providers
{
    internal class RoleProvider : IRoleProvider
    {
        private readonly Domain.Interfaces.IRoleProvider _roleProvider;
        private readonly IEmployeeProvider _employeeProvider;

        public RoleProvider(Domain.Interfaces.IRoleProvider roleProvider, IEmployeeProvider employeeProvider)
        {
            _roleProvider = roleProvider;
            _employeeProvider = employeeProvider;
        }

        public void RoleDisplayOperations()
        {
            ConsoleHelpers.ConsoleOutput("1.Add Role");
            ConsoleHelpers.ConsoleOutput("2.Display All");
            ConsoleHelpers.ConsoleOutput("3. Go back");
            ConsoleHelpers.ConsoleOutput("Enter the choice:", false);

            int choice = ConsoleHelpers.ConsoleIntegerInput();
            switch (choice)
            {
                case 1:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    ReadRole();
                    break;
                case 2:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    DisplayRoles();
                    break;
                case 3:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    return;
                case -1:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    break;
            }
            ConsoleHelpers.ConsoleOutput("Do you want to continue(Y/N)");
            var select = Console.ReadLine();
            if (select?.ToLower() == "n")
            {
                return;
            }
            Console.Clear();
            RoleDisplayOperations();
        }

        public void ReadRole()
        {

            Domain.DTO.Role Role = new();
            Type type = typeof(Domain.DTO.Role);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                ConsoleHelpers.ConsoleOutput($"Enter the Value for {prop.Name}:", false);
                PropertyInfo property = type.GetProperty(prop.Name)!;
                if (prop.Name == nameof(Domain.DTO.Role.Department) )
                {
                    property.SetValue(Role, _employeeProvider.GetStaticValues(prop.Name));
                }
                else if(prop.Name == nameof(Domain.DTO.Role.Location))
                {
                    string? value = _employeeProvider.GetStaticValues(prop.Name);
                    List<string>? values = value!=null?[value]:null;
                    property.SetValue(Role, values);
                }
                else
                {
                    property.SetValue(Role, Console.ReadLine() ?? string.Empty);
                }
            }
            List<string> errorList = _roleProvider.SetData(Role);
            while (errorList.Count > 0)
            {
                for (int i = 0; i < errorList.Count; i++)
                {
                    ConsoleHelpers.ConsoleOutput($"Enter the Value for {errorList[i]}:", false);
                    PropertyInfo property = type.GetProperty(errorList[i])!;
                    if (errorList[i] == nameof(Domain.DTO.Employee.Department))
                    {
                        property.SetValue(Role, _employeeProvider.GetStaticValues(errorList[i]));
                    }
                    else if (errorList[i] == nameof(Domain.DTO.Role.Location))
                    {
                        string? value = _employeeProvider.GetStaticValues(errorList[i]);
                        List<string>? values = value != null ? [value] : null;
                        property.SetValue(Role, values);
                    }
                    else
                    {
                        property.SetValue(Role, Console.ReadLine() ?? string.Empty);
                    }
                }
                errorList = _roleProvider.SetData(Role);
            }
            _roleProvider.AddRole(Role);
        }

        public void DisplayRoles()
        {
            List<Domain.DTO.Role> roles = _roleProvider.GetRoleData();
            foreach (Domain.DTO.Role roleInformation in roles)
            {
                Type type = typeof(Domain.DTO.Role);
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if(prop.Name == nameof(RoleDetail.Location))
                    {
                        List<string> locations = (List<string>)prop.GetValue(roleInformation)!;
                        ConsoleHelpers.ConsoleOutput($"{prop.Name} :{ string.Join(',',locations)}");
                    }
                    else
                    {
                        ConsoleHelpers.ConsoleOutput($"{prop.Name} : {prop.GetValue(roleInformation)}");
                    }
                   
                }
                ConsoleHelpers.ConsoleOutput("----------------------------------------------");
                ConsoleHelpers.ConsoleOutput("----------------------------------------------");
            }
        }


    }
}
