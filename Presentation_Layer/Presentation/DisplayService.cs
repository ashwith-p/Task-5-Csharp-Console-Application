﻿using System.Reflection;
using Employee_Directory_Console_app.Utilities.TypedEnums;
using Employee_Directory_Console_app.Models;
using Employee_Directory_Console_app.Utilities.Helpers;
using Employee_Directory_Console_app.BussinesLogic;

namespace Employee_Directory_Console_app.Display
{
    public class DisplayService
    {
        private static readonly EmployeeDetailsDelegate[] employeeDetailsDelegates = new EmployeeDetailsDelegate[2];
        public static EmployeeDetailsDelegate[] FunctionsArray = employeeDetailsDelegates;
        public DisplayService()
        {
            EmployeeOperations EmployeeOperationsObj = new();
            
            FunctionsArray[0] = new EmployeeDetailsDelegate(GetEmployeeDetails); //name,done
            FunctionsArray[1] = new EmployeeDetailsDelegate(GetStaticValues);
        }
        public delegate string EmployeeDetailsDelegate(EmployeeOperations obj, string choice,string message); //naming,done
        public void Init()
        {
            ConsoleHelpers.ConsoleOutput("1.Employee Managament");
            ConsoleHelpers.ConsoleOutput("2.Roles Managament");
            ConsoleHelpers.ConsoleOutput("3.Exit");
            Console.Write("Enter the choice:");
            int choice = int.Parse(ConsoleHelpers.ConsoleIntegerInput());
            _ = new DisplayService();
            switch (choice)
            {
                case 1:
                    ConsoleHelpers.ConsoleOutput("---------------");
                    EmployeeDisplayOperations();
                    break;
                case 2:
                    ConsoleHelpers.ConsoleOutput("----------------");
                    RoleDisplayOperations();
                    break;
                case 3:
                    return;

                case -1:
                    ConsoleHelpers.ConsoleOutput("------------------");
                    break;

            }
            Init();
        }

        public static void EmployeeDisplayOperations()
        {
            EmployeeOperations employeeObj = new(); //name,doone
            ConsoleHelpers.ConsoleOutput("1.Add Employee");
            ConsoleHelpers.ConsoleOutput("2.Display All");
            ConsoleHelpers.ConsoleOutput("3.Display One");
            ConsoleHelpers.ConsoleOutput("4.Edit Employee");
            ConsoleHelpers.ConsoleOutput("5.Delete Employee");
            ConsoleHelpers.ConsoleOutput("6.Go back");
            Console.Write("Enter the choice:");
            int choice = int.Parse(ConsoleHelpers.ConsoleIntegerInput());
            switch (choice)
            {
                case 1:
                    ConsoleHelpers.ConsoleOutput("-------------------------");
                    AddEmployee(employeeObj);
                    break;
                case 2:
                    ConsoleHelpers.ConsoleOutput("---------------------------");
                    DisplayEmployee(employeeObj, DisplayType.DisplayAll);
                    break;
                case 3:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    DisplayEmployee(employeeObj, DisplayType.DisplayOne);
                    break;
                case 4:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    EditEmployee(employeeObj);
                    break;
                case 5:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    Console.Write("Enter the Id:");
                    if (employeeObj.DeleteEmployee(Console.ReadLine() ?? ""))
                    {
                        ConsoleHelpers.ConsoleOutput("Deleted Successfully");
                    }
                    else
                    {
                        ConsoleHelpers.ConsoleOutput("InValid Employee Number");
                    }
                    break;
                case 6:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    return;
                case -1:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    break;
            }
            EmployeeDisplayOperations();
        }
        public static void RoleDisplayOperations()
        {
            ConsoleHelpers.ConsoleOutput("1.Add Role");
            ConsoleHelpers.ConsoleOutput("2.Display All");
            ConsoleHelpers.ConsoleOutput("3. Go back");
            Console.Write("Enter the choice:");
            RoleOperations role = new();
            int choice = int.Parse(ConsoleHelpers.ConsoleIntegerInput());
            switch (choice)
            {
                case 1:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    AddRole(role);
                    break;
                case 2:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    DisplayRoles(role);
                    break;
                case 3:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    return;
                case -1:
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                    break;
            }
            RoleDisplayOperations();
        }
        public static void AddEmployee(EmployeeOperations employeeOperationsObj)
        {
            Employee employee = new ();
            Employee? isIdUnique=new() ; //name && bool
            while (isIdUnique != null)
            {
                employee.Id = GetEmployeeDetails(employeeOperationsObj,nameof(Employee.Id), "Enter the Id:");
                isIdUnique = employeeOperationsObj.FindEmployee(employee.Id);
                if (isIdUnique != null)
                {
                    ConsoleHelpers.ConsoleOutput("Employee Number already Exists");
                }
            }
            employee.FirstName = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.FirstName), "Enter the FirstName:");
            employee.LastName = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.LastName), "Enter the LastName:");
            employee.DateOfBirth = GetEmployeeDetails(employeeOperationsObj,nameof(Employee.DateOfBirth), "Enter the Date of Birth:");
            employee.MobileNumber = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.MobileNumber), "Enter the Mobile number:");
            employee.Email = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.Email), "Enter the Email:");
            employee.JoiningDate = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.JoiningDate), "Enter the Joining date:");
            employee.Department = GetStaticValues(employeeOperationsObj, nameof(Employee.Department),"Enter the Department:");
            employee.JobTitle = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.JobTitle), "Enter the Job Title:");
            employee.Location = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.Location), "Enter the job Location:");
            employee.Manager = GetEmployeeDetails(employeeOperationsObj, nameof(Employee.Manager), "Enter the Manager:");
            employee.Project = GetStaticValues(employeeOperationsObj,nameof(Employee.Project),"Enter the project:");
            employeeOperationsObj.SetEmployeeCollection(employee);

        }
        public static string GetEmployeeDetails(EmployeeOperations employeeOperationsObj, string choice, string? message = null)
        {
            
            string value = "";
            bool status = false;
            while (!status)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Console.Write(message);
                }
                else
                {
                    ConsoleHelpers.ConsoleOutput("Enter the Value:", false);
                }
                value = Console.ReadLine() ?? "";
                status = EmployeeOperations.IsValidEmployee(value, choice);
                if (!status)
                {
                    ConsoleHelpers.ConsoleOutput("Enter valid input");
                    ConsoleHelpers.ConsoleOutput("--------------------------");
                }
            }
            return value;
        }
        public static string GetStaticValues(EmployeeOperations employeeOperationsObj, string choice, string message)
        {
            string[] staticData;
            staticData = EmployeeOperations.GetStaticData(choice);
            string value=string.Empty;
            bool status = false;
            while (!status)
            {
                ConsoleHelpers.ConsoleOutput(message);
                for (int i = 0; i < staticData.Length; i++)
                {
                    Console.WriteLine($"{i + 1}.{staticData[i]}");
                }
                
                string input = Console.ReadLine() ?? "";
                if(choice == nameof(Employee.Project) && input.Length==0)
                {
                    break;
                }
                status = EmployeeOperations.IsValidEmployee(input, choice);
                if (status)
                {
                    value = staticData[int.Parse(input) - 1];
                }
            }
            return value;
        }
        public static void DisplayEmployee(EmployeeOperations obj, DisplayType displayOption)
        {
            string Id =string.Empty;
            if (displayOption == DisplayType.DisplayOne)
            {
                Id = GetEmployeeDetails(new EmployeeOperations(), nameof(Employee.Id));
            }
            List<Employee> employees = obj.GetEmployeesInformation(Id);
            if (employees.Count == 0 )
            {
                if (displayOption == DisplayType.DisplayOne)
                {   Console.WriteLine("Employee Number does not Exist");
                    Console.WriteLine("");
                }
                else
                {
                    ConsoleHelpers.ConsoleOutput("No Employees ");
                }
            }

            foreach (Employee employee in employees)
            {
                Type type = typeof(Employee);
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    Console.WriteLine($"{prop.Name} : {prop.GetValue(employee)}");

                }
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("----------------------------------------------");
            }
        }
        public static void EditEmployee(EmployeeOperations employeeObj)
        {
            string Number;
            while (true)
            {
                Number = GetEmployeeDetails(employeeObj, nameof(Employee.Id));
                if (employeeObj.FindEmployee(Number) != null)
                {
                    break;
                }
            }
            Employee employee = employeeObj.FindEmployee(Number)!;
            bool status = false;
            while (!status)
            {
                Type type = typeof(Employee);
                int i = 1;
                Dictionary<int, string> pair = [];
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    Console.WriteLine($"{i} {prop.Name}");
                    pair[i] = prop.Name;
                    i++;
                }
                Console.Write("Enter the choise:");
                int choice = int.Parse(Console.ReadLine() ?? "0");
                if (choice > 0 && choice <= 12)
                { 
                    string Value = "";
                    if (choice==10 || choice==12)
                    {
                        Value = FunctionsArray[1](employeeObj, pair[choice], $"Enter the {pair[choice]}");
                    }
                    else
                    {
                        Value = FunctionsArray[0](employeeObj, pair[choice],string.Empty);
                    }
                    EmployeeOperations.EditEmployee(employee, pair, choice, Value);
                    employeeObj.DeleteEmployee(Number);
                    employeeObj.SetEmployeeCollection(employee);
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
        public static void AddRole(RoleOperations role)
        {
            bool flag = false;
            string roleName = "";
            while (!flag)
            {
                Console.Write("Enter the RoleName:");
                roleName = Console.ReadLine() ?? "";
                flag = EmployeeOperations.IsValidName(roleName);
            }
            string location = GetEmployeeDetails(new EmployeeOperations(), nameof(Role.Location),"Enter the Location:");
            Console.Write("Enter the RoleDescription:");
            string roleDescription = Console.ReadLine() ?? "";
            string department = GetStaticValues(new EmployeeOperations(), nameof(Role.Department),"Enter the Department");
            Dictionary<string, string> keyValuePairs = new()
            {
                ["RoleName"] = roleName,
                ["Location"] = location,
                ["Department"] = department,
                ["RoleDescription"] = roleDescription
            };
            role.SetData(keyValuePairs);
        }
        public static void DisplayRoles(RoleOperations role)
        {
            List<Role> roles = role.GetRoleData();
            foreach(Role roleInformation in roles)
            {
                Type type = typeof(Role);
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    Console.WriteLine($"{prop.Name} : {prop.GetValue(roleInformation)}");
                }
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("----------------------------------------------");
            }
        }
    }
}