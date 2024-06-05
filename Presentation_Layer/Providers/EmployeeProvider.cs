using System.Reflection;
using Domain.DTO;
using EmployeeDirectory.Utilities.Helpers;
using EmployeeDirectory.Interfaces;
using Data.Exceptions;
using Data;


namespace EmployeeDirectory.Providers
{
    public class EmployeeProvider(Domain.Interfaces.IEmployeeProvider provider) : IEmployeeProvider
    {

        private readonly Domain.Interfaces.IEmployeeProvider _provider = provider;
        private readonly ErrorMessages _errorMessages = new();

        public void EmployeeDisplayOperations()
        {

            ConsoleHelpers.ConsoleOutput("1.Add Employee");
            ConsoleHelpers.ConsoleOutput("2.Display All");
            ConsoleHelpers.ConsoleOutput("3.Display One");
            ConsoleHelpers.ConsoleOutput("4.Edit Employee");
            ConsoleHelpers.ConsoleOutput("5.Delete Employee");
            ConsoleHelpers.ConsoleOutput("6.Go back");
            ConsoleHelpers.ConsoleOutput("Enter the choice:", false);
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
                    GetEmployeeById();
                    break;
                case 4:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    EditEmployeeInputService();
                    break;
                case 5:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    ConsoleHelpers.ConsoleOutput("Enter the EmailId:", false);
                    try
                    {
                        if (_provider.DeleteEmployee(Console.ReadLine() ?? ""))
                        {
                            ConsoleHelpers.ConsoleOutput("Deleted Successfully");
                        }
                    }
                    catch (EmployeeIdNotFoundException)
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
                ConsoleHelpers.ConsoleOutput($"Enter the Value for {prop.Name}:", false);
                PropertyInfo property = type.GetProperty(prop.Name)!;
                if (prop.Name == nameof(Employee.Department) || prop.Name == nameof(Employee.Project))
                {
                    string? id = GetStaticValues(prop.Name);
                    property.SetValue(employee, id==null?null:int.Parse(id));
                }
                else if (prop.Name == nameof(Employee.JobTitle))
                {
                    int? value = (int)employee.GetType().GetProperty(nameof(employee.Department))!.GetValue(employee)!;
                    property.SetValue(employee,int.Parse( GetStaticValues(prop.Name, string.Empty,value)!));
                }
                else if (prop.Name == nameof(Employee.Location))
                {
                    int? value =(int) employee.GetType().GetProperty(nameof(employee.JobTitle))!.GetValue(employee)!;
                    property.SetValue(employee, int.Parse(GetStaticValues(prop.Name, string.Empty,value)!));
                }
                else if (prop.Name == nameof(Employee.Manager))
                {
                    string? managerId = GetStaticValues(prop.Name, string.Empty, 0);
                    property.SetValue(employee, managerId != null ? managerId![..6] : null);
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
                    ConsoleHelpers.ConsoleOutput($"Enter the Value for {inCorrect[i]}:", false);
                    PropertyInfo property = type.GetProperty(inCorrect[i])!;
                    if (inCorrect[i] == nameof(Employee.Department) || inCorrect[i] == nameof(Employee.Project))
                    {
                        string? id = GetStaticValues(inCorrect[i]);
                        property.SetValue(employee, id == null ? null : int.Parse(id));
                    }
                    else if (inCorrect[i] == nameof(Employee.JobTitle))
                    {
                        int? value = (int)employee.GetType().GetProperty(nameof(employee.Department))!.GetValue(employee)!;
                        property.SetValue(employee, int.Parse(GetStaticValues(inCorrect[i], string.Empty,value)!));
                    }
                    else if (inCorrect[i] == nameof(Employee.Location))
                    {
                        int? value = (int)employee.GetType().GetProperty(nameof(employee.JobTitle))!.GetValue(employee)!;
                        property.SetValue(employee, int.Parse(GetStaticValues(inCorrect[i], string.Empty,value)!));
                    }
                    else if (inCorrect[i] == nameof(Employee.Manager))
                    {
                        string? managerId = GetStaticValues(inCorrect[i], string.Empty, 0);
                        property.SetValue(employee, managerId != null ? managerId![..6] : null);
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
                    ConsoleHelpers.ConsoleOutput(message, false);
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
        public string? GetStaticValues(string choice, string message = "", int? staticValue = null)
        {
            List<string> staticData;
            staticData = _provider.GetStaticData(choice, staticValue);
            string value = string.Empty;
            ConsoleHelpers.ConsoleOutput("");
            for (int i = 0; i < staticData.Count; i++)
            {
                ConsoleHelpers.ConsoleOutput($"{i + 1}.{staticData[i]}");
            }
            string input = Console.ReadLine() ?? string.Empty;
            if ((choice == nameof(Employee.Project) || choice == nameof(Employee.Manager)) && input.Length == 0)
            {
                return null;
            }
            if (int.TryParse(input, out int x))
            {
                if (x > 0 && x <= staticData.Count)
                {
                    if (choice == nameof(Employee.Manager))
                        return staticData[x-1];
                    value = input;
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
                    this.PrintEmployee(employee);
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                }

            }
            catch (EmployeeIdNotFoundException)
            {
                ConsoleHelpers.ConsoleOutput("Employee does not Exist\n");
            }
        }

        private void PrintEmployee(Employee employee)
        {
            ConsoleHelpers.ConsoleOutput("FirstName:" + employee.FirstName);
            ConsoleHelpers.ConsoleOutput("LastName:" + employee.LastName);
            ConsoleHelpers.ConsoleOutput("DateOfBirth:" + employee.DateOfBirth);
            ConsoleHelpers.ConsoleOutput("Email ID:" + employee.Email);
            ConsoleHelpers.ConsoleOutput($"Phone Number:{employee.MobileNumber}");
            ConsoleHelpers.ConsoleOutput($"Joining Date:{employee.JoiningDate}");
            ConsoleHelpers.ConsoleOutput($"Department:{_provider.GetValueById(employee.Department,nameof(employee.Department))}");
            ConsoleHelpers.ConsoleOutput($"Location:{_provider.GetValueById(employee.Location,nameof(employee.Location))}");
            ConsoleHelpers.ConsoleOutput($"Job Title:{_provider.GetValueById(employee.JobTitle,nameof(employee.JobTitle))}");
            ConsoleHelpers.ConsoleOutput($"Manager:{employee.Manager}");
            ConsoleHelpers.ConsoleOutput($"Project:{_provider.GetValueById(employee.Project,nameof(employee.Project))}");
        }

        public void GetEmployeeById()
        {
            ConsoleHelpers.ConsoleOutput("Enter the Employee ID:", false);
            string id = Console.ReadLine() ?? string.Empty;
            try
            {
                Employee employee = _provider.GetEmployee(id) ?? throw new EmployeeIdNotFoundException();
                this.PrintEmployee(employee);
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
                ConsoleHelpers.ConsoleOutput("Enter the Employee ID:", false);
                string Email = Console.ReadLine() ?? string.Empty;
                Employee employee = _provider.GetEmployee(Email) ?? throw new EmployeeIdNotFoundException();
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
                    ConsoleHelpers.ConsoleOutput("Enter the choice:", false);
                    bool isValidInput = false;
                    int choice = ConsoleHelpers.ConsoleIntegerInput();
                    if (choice > 0 && choice <= pair.Count)
                    {
                        string data = string.Empty;
                        if (pair[choice] == nameof(Employee.Department))
                        {
                            ConsoleHelpers.ConsoleOutput("You cant edit the department");
                            break;
                        }
                        else if (pair[choice] == nameof(Employee.JobTitle))
                        {
                            int? value = (int)employee.GetType().GetProperty(nameof(employee.Department))!.GetValue(employee)!;
                            data = GetStaticValues(pair[choice], "Enter the value of" + pair[choice] + ':', value)!;
                        }
                        else if (pair[choice] == nameof(Employee.Location))
                        {
                            int? value =(int) employee.GetType().GetProperty(nameof(employee.JobTitle))!.GetValue(employee)!;
                            data = GetStaticValues(pair[choice], "Enter the value of" + pair[choice] + ':', value)!;
                        }
                        else if (pair[choice] == nameof(Employee.Manager))
                        {
                            data = GetStaticValues(pair[choice], "Enter the value of" + pair[choice] + ':', 0)!;
                        }
                        else if (pair[choice] == nameof(Employee.Project))
                        {
                            data = GetStaticValues(pair[choice], "Enter the value of" + pair[choice] + ':')!;
                        }
                        else
                        {
                            data = GetInputData(pair[choice], string.Empty);
                        }
                        isValidInput = _provider.IsValidEmployee(data, pair[choice]);
                        if (isValidInput)
                        {
                            _provider.EditEmployee(pair, choice, data, Email);
                        }
                        //_provider.EditEmployee(employee, pair, choice, data);
                    }
                    else
                    {
                        isValidInput = false;
                    }
                    if (!isValidInput)
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
            catch (EmployeeIdNotFoundException)
            {
                ConsoleHelpers.ConsoleOutput("Employee does not Exist\n");
            }
        }
    }
}