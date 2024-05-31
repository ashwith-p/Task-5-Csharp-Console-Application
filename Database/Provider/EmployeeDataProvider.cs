using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;
using Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Data.Provider
{
    public class EmployeeDataProvider:IEmployeeDataProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        private readonly IRoleDataProvider _roleDataProvider;
        private readonly IProjectProvider _projectProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ILocationProvider _locationProvider;
        public EmployeeDataProvider(AshwithEmployeeDirectoryContext context,IRoleDataProvider roleDataProvider,ILocationProvider locationProvider
            ,IDepartmentProvider departmentProvider,IProjectProvider projectProvider)
        {
            _context = context;
            _roleDataProvider = roleDataProvider;
            _locationProvider = locationProvider;
            _projectProvider = projectProvider;
            _departmentProvider = departmentProvider;
        }
        public Employee? GetEmployee(string id)
        {
            return _context.Employees.Where(s => s.Id == id).FirstOrDefault();
        }
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees;
        }
        public IEnumerable<Employee>? GetEmployesUnderManager(string? id)
        {
            try
            {
                return _context.Employees.Where(s => s.ManagerId == id);
            }
            catch (Exception )
            {
                return null;
            }
        }
        public void Add(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public bool Delete(string id)
        {
            try
            {
                _context.Employees.Remove(this.GetEmployee(id)??throw new EmployeeIdNotFoundException());
                _context.SaveChanges();
                return true;
            }
            catch (EmployeeIdNotFoundException )
            {
                return false;
            }
            
        }

        public IEnumerable<string> GetEmployeeNames()
        {
            return _context.Employees.Select(s => s.Id + ' ' + s.FirstName + ' ' + s.LastName);
        }

        public void EditEmployee(Dictionary<int, string> pair, int choice, string value, string employeeId)
        {
            int? id = null;
            bool flag = false;
            Employee emp = _context.Employees.Where(s => s.Id==employeeId).First();
            if (pair[choice]==nameof(Employee.Manager))
            {
                pair[choice] = nameof(Employee.ManagerId);
            }
            else if (pair[choice]=="JobTitle")
            {
                pair[choice]=nameof(Employee.RoleId);
            }
            if (pair[choice] == nameof(Employee.RoleId))
            {
                id = _roleDataProvider.GetRoleByName(value).Id;
            }
            else if (pair[choice] == nameof(Employee.Project))
            {
                id = _projectProvider.GetProjectByName(value) != null ? _projectProvider.GetProjectByName(value)!.Id : null;
                pair[choice] = nameof(Employee.ProjectId);
            }
            else if (pair[choice] == nameof(Employee.Location))
            {
                id = _locationProvider.GetLocationByName(value).Id;
            }
            else
            {
                flag = true;
            }
            var propertyInfo = typeof(Employee).GetProperty(pair[choice], BindingFlags.Public | BindingFlags.Instance);
            if (flag)
            {
                if (pair[choice] == nameof(Employee.ManagerId))
                {
                    propertyInfo!.SetValue(emp, value.Substring(0, 6));
                    Console.WriteLine(value.Substring(0,6).Length);
                }
                propertyInfo!.SetValue(emp, value);
            }
            else
            {
                propertyInfo!.SetValue(emp, id);
            }
            _context.Entry(emp).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
