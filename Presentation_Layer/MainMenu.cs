
using EmployeeDirectory.Utilities.Helpers;
using EmployeeDirectory.Interfaces;

namespace EmployeeDirectory
{
    public class MainMenu(IEmployeeProvider employeeProvider, IRoleProvider roleProvider,AshwithEmployeeDirectoryContext context)
    {
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;
        private readonly IRoleProvider _roleProvider = roleProvider;
        private readonly AshwithEmployeeDirectoryContext _context= context;

        public void Init()
        {
            
            ConsoleHelpers.ConsoleOutput("1.Employee Management");
            ConsoleHelpers.ConsoleOutput("2.Roles Management");
            ConsoleHelpers.ConsoleOutput("3.Exit");
            Console.Write("Enter the choice:");
            int choice = ConsoleHelpers.ConsoleIntegerInput();
           
            switch (choice)
            {
                case 1:
                    ConsoleHelpers.ConsoleOutput("---------------");
                    _employeeProvider.EmployeeDisplayOperations();
                    break;
                case 2:
                    ConsoleHelpers.ConsoleOutput("----------------");
                    _roleProvider.RoleDisplayOperations();
                    break;
                case 3:
                    return;

                case -1:
                    ConsoleHelpers.ConsoleOutput("------------------");
                    break;
            }
            Console.Clear();
            Init();
        }
    }
}
