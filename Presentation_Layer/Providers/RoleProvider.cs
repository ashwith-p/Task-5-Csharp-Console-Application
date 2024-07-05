using EmployeeDirectory.Utilities.Helpers;
using System.Reflection;
using EmployeeDirectory.Interfaces;

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
            Console.WriteLine("1.Add Role");
            Console.WriteLine("2.Display All");
            Console.WriteLine("3. Go back");
            Console.Write("Enter the choice:", false);

            int choice = ConsoleHelpers.ConsoleIntegerInput();
            switch (choice)
            {
                case 1:
                    Console.WriteLine("--------------------------");
                    ReadRole();
                    break;
                case 2:
                    Console.WriteLine("--------------------------");
                    DisplayRoles();
                    break;
                case 3:
                    Console.WriteLine("--------------------------");
                    return;
                case -1:
                    Console.WriteLine("--------------------------");
                    break;
            }
            Console.WriteLine("Do you want to continue(Y/N)");
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
                Console.Write($"Enter the Value for {prop.Name}:", false);
                PropertyInfo property = type.GetProperty(prop.Name)!;
                if (prop.Name == nameof(Domain.DTO.Role.Department) )
                {
                    property.SetValue(Role, int.Parse(_employeeProvider.GetStaticValues(prop.Name)!));
                }
                else if(prop.Name == nameof(Domain.DTO.Role.Location))
                {
                    int? value = int.Parse(_employeeProvider.GetStaticValues(prop.Name)!);
                    List<int>? values = value != null?[(int)value]:null;
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
                    Console.Write($"Enter the Value for {errorList[i]}:", false);
                    PropertyInfo property = type.GetProperty(errorList[i])!;
                    if (errorList[i] == nameof(Domain.DTO.Employee.Department))
                    {
                        property.SetValue(Role, int.Parse(_employeeProvider.GetStaticValues(errorList[i])!));
                    }
                    else if (errorList[i] == nameof(Domain.DTO.Role.Location))
                    {
                        int? value = int.Parse(_employeeProvider.GetStaticValues(errorList[i])!);
                        List<int>? values = value != null ? [(int)value] : null;
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
                    if(prop.Name == nameof(Data.Models.RoleDetail.Location))
                    {
                        List<int> locations = (List<int>)prop.GetValue(roleInformation)!;
                        List<string> locationsString =locations.ConvertAll(x => _roleProvider.GetLocationById(x));
                        Console.WriteLine($"{prop.Name} :{ string.Join(',',locationsString)}");
                    }
                    else if(prop.Name == nameof(Data.Models.Role.Department))
                    {
                        Console.WriteLine($"{prop.Name} : {_roleProvider.GetDepartmentById((int)(prop.GetValue(roleInformation))!)}");
                    }
                    else
                    {
                        Console.WriteLine($"{prop.Name} : {prop.GetValue(roleInformation)}");
                    }
                   
                }
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("----------------------------------------------");
            }
        }


    }
}
