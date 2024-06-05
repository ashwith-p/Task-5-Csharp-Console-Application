﻿using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;
using Data.Exceptions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Data.Provider
{
    public class EmployeeDataProvider(AshwithEmployeeDirectoryContext context, IRoleDataProvider roleDataProvider, ILocationProvider locationProvider
            , IProjectProvider projectProvider) : IEmployeeDataProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context = context;
        private readonly IRoleDataProvider _roleDataProvider = roleDataProvider;
        private readonly IProjectProvider _projectProvider = projectProvider;
        private readonly ILocationProvider _locationProvider = locationProvider;

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
                id = int.Parse(value);
                pair[choice]=nameof(Employee.RoleId);
            }
            else if (pair[choice] == nameof(Employee.Project))
            {
                id = int.Parse(value);
                pair[choice] = nameof(Employee.ProjectId);
            }
            else if (pair[choice]==nameof(Employee.Location))
            {
                id = int.Parse(value);
                pair[choice]=nameof(Employee.LocationId);
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
                    propertyInfo!.SetValue(emp, value[..6]);
                }
                else if(pair[choice] == nameof(Employee.DateOfBirth) || pair[choice]== nameof(Employee.JoiningDate))
                {
                    propertyInfo!.SetValue(emp, DateOnly.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                else
                {
                    propertyInfo!.SetValue(emp, value);
                }
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
