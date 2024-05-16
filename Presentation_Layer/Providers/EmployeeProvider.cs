using System.Reflection;
using BLL.DTO;
using EmployeeDirectory.Utilities.Helpers;
using EmployeeDirectory.Interfaces;
using BLL.Providers;
using DLL.Exceptions;
using DLL;


namespace EmployeeDirectory.Providers
{
    public class EmployeeProvider : IEmployeeProvider
    {
        
        private readonly BLL.Interfaces.IEmployeeProvider _provider;
        private readonly ErrorMessages _errorMessages = new();
        public EmployeeProvider(BLL.Interfaces.IEmployeeProvider provider)
        {
            _provider = provider;
        }

        public void EmployeeDisplayOperations()
        {
            
            ConsoleHelpers.ConsoleOutput("1.Add Employee");
            ConsoleHelpers.ConsoleOutput("2.Display All");
            ConsoleHelpers.ConsoleOutput("3.Display One");
            ConsoleHelpers.ConsoleOutput("4.Edit Employee");
            ConsoleHelpers.ConsoleOutput("5.Delete Employee");
            ConsoleHelpers.ConsoleOutput("6.Go back");
            ConsoleHelpers.ConsoleOutput("Enter the choice:",false);
            int choice = ConsoleHelpers.ConsoleIntegerInput();
            
            switch (choice)
            {

                case 1:
                    ConsoleHelpers.ConsoleOutput("-------------------------");
                    ReadEmployee();
                    break;
                case 2:
                    ConsoleHelpers.ConsoleOutput("---------------------------");
                    DisplayAll();
                    break;
                case 3:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    GetEmployeeByEmail();
                    break;
                case 4:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    EditEmployeeInputService();
                    break;
                case 5:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    ConsoleHelpers.ConsoleOutput("Enter the EmailId:",false);
                    try
                    {
                        if(_provider.DeleteEmployee(Console.ReadLine() ?? ""))
                        {
                            ConsoleHelpers.ConsoleOutput("Deleted Successfully");
                        }
                    }
                    catch(DLL.Exceptions.EmployeeIdNotFoundException )
                    {
                        ConsoleHelpers.ConsoleOutput("Employee does not Exist\n");
                    }
                    
                    break;
                case 6:
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
            EmployeeDisplayOperations();
        }

        public void ReadEmployee()
        {
            Employee employee = new();
            Type type = typeof(Employee);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                ConsoleHelpers.ConsoleOutput($"Enter the Value for {prop.Name}:",false);
                PropertyInfo property = type.GetProperty(prop.Name)!;
                if (prop.Name == nameof(Employee.Department) || prop.Name == nameof(Employee.Project))
                {
                    property.SetValue(employee, GetStaticValues(prop.Name));
                }
                else
                {
                    property.SetValue(employee, Console.ReadLine() ?? string.Empty);
                }
            }
            List<string> inCorrect = _provider.Validator(employee);
            while (inCorrect.Count > 0)
            {
                for (int i = 0; i < inCorrect.Count; i++)
                {
                    ConsoleHelpers.ConsoleOutput(_errorMessages.messages[inCorrect[i]]);
                    ConsoleHelpers.ConsoleOutput($"Enter the Value for {inCorrect[i]}:",false);
                    PropertyInfo property = type.GetProperty(inCorrect[i])!;
                    if (inCorrect[i] == nameof(Employee.Department) || inCorrect[i] == nameof(Employee.Project))
                    {
                        property.SetValue(employee, GetStaticValues(inCorrect[i]));
                    }
                    else
                    {
                        property.SetValue(employee, Console.ReadLine() ?? string.Empty);
                    }
                }
                inCorrect = _provider.Validator(employee);
            }
            _provider.AddEmployee(employee);
        }

        /// <summary>
        /// Reads input for Edit Employee
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="message"></param>
        /// <returns>The user Entered Value after validation</returns>
        public string GetInputData(string choice, string? message = null)
        {
            string value = "";
            bool status = false;
            while (!status)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    ConsoleHelpers.ConsoleOutput(message,false);
                }
                else
                {
                    ConsoleHelpers.ConsoleOutput("Enter the Value:", false);
                }
                value = Console.ReadLine() ?? "";
                status = _provider.IsValidEmployee(value, choice);
                if (!status)
                {
                    ConsoleHelpers.ConsoleOutput("Enter valid input");
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                }
            }
            return value;
        }

        /// <summary>
        /// Gets static data from json
        /// </summary>
        /// <param name="_provider"></param>
        /// <param name="choice"></param>
        /// <param name="message"></param>
        /// <returns>Static data for projects and departments.</returns>
        public string GetStaticValues(string choice, string message = "")
        {
            string[] staticData;
            staticData = _provider.GetStaticData(choice);
            string value = string.Empty;
            ConsoleHelpers.ConsoleOutput("");
            for (int i = 0; i < staticData.Length; i++)
            {
                ConsoleHelpers.ConsoleOutput($"{i + 1}.{staticData[i]}");
            }
            string input = Console.ReadLine() ?? string.Empty;
            if (choice == nameof(Employee.Project) && input.Length == 0)
            {
                return value;
            }
            if (int.TryParse(input, out int x))
            {
                if (x > 0 && x <= staticData.Length)
                {
                    value = staticData[int.Parse(input) - 1];
                }

            }
            return value;
        }

        public void DisplayAll()
        {
            try
            {
                List<Employee> employees = _provider.GetEmployeesInformation() ?? [];
                foreach (Employee employee in employees)
                {
                    Type type = typeof(Employee);
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        ConsoleHelpers.ConsoleOutput($"{prop.Name} : {prop.GetValue(employee)}");
                    }
                    ConsoleHelpers.ConsoleOutput("----------------------------------------------");
                    ConsoleHelpers.ConsoleOutput("----------------------------------------------");
                }

            }
            catch (DLL.Exceptions.EmployeeIdNotFoundException)
            {
                ConsoleHelpers.ConsoleOutput("Employee does not Exist\n");
            }
        }

        public void GetEmployeeByEmail()
        {
            ConsoleHelpers.ConsoleOutput("Enter the EmailID:",false);
            string email = Console.ReadLine() ?? string.Empty;
            try
            {
                Employee employee = _provider.GetEmployee(email) ?? throw new EmployeeIdNotFoundException();
                Type type = typeof(Employee);
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    ConsoleHelpers.ConsoleOutput($"{prop.Name} : {prop.GetValue(employee)}");
                }
                ConsoleHelpers.ConsoleOutput("----------------------------------------------");
            }
            catch (EmployeeIdNotFoundException)
            {
                ConsoleHelpers.ConsoleOutput("Employee does not Exist\n");
            }
        }

        public void EditEmployeeInputService()
        {
            try
            {
                ConsoleHelpers.ConsoleOutput("Enter the Email ID:",false);
                string Email = Console.ReadLine()??string.Empty;
                Employee employee = (_provider.FindEmployee(Email) ?? throw new EmployeeIdNotFoundException()) ?? throw new DLL.Exceptions.EmployeeIdNotFoundException();
                bool status = false;
                while (!status)
                {
                    Type type = typeof(Employee);
                    int i = 1;
                    Dictionary<int, string> pair = [];
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        ConsoleHelpers.ConsoleOutput($"{i} {prop.Name}");
                        pair[i] = prop.Name;
                        i++;
                    }
                    ConsoleHelpers.ConsoleOutput("Enter the choice:",false);
                    int choice = ConsoleHelpers.ConsoleIntegerInput();
                    if (choice > 0 && choice <= 12)
                    {
                        string Value = "";
                        if (choice == 10 || choice == 12)
                        {
                            Value = GetStaticValues(pair[choice], $"Enter the {pair[choice]}");
                        }
                        else
                        {
                            Value = GetInputData(pair[choice], string.Empty);
                        }
                        _provider.EditEmployee(employee, pair, choice, Value);
                    }
                    else
                    {
                        ConsoleHelpers.ConsoleOutput("Enter Valid Input:");
                    }
                    ConsoleHelpers.ConsoleOutput("Do you want to continue(Y/N)");
                    var select = Console.ReadLine();
                    if (select?.ToLower() == "n")
                    {
                        status = true;
                    }
                }
            }
            catch (DLL.Exceptions.EmployeeIdNotFoundException)
            {
                ConsoleHelpers.ConsoleOutput("Employee does not Exist\n");
            }
        }
    }
}