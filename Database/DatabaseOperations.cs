using System.Text.Json;
using Data.Models;
using Data.Interfaces;
using Data.Exceptions;
using System.Data.SqlClient;
using System.Data;
using EmployeeDirectory;

namespace Data
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly AshwithEmployeeDirectoryContext _context;

        public DatabaseOperations(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }
        public IEnumerable<string> GetStaticData(string name)
        {
            try
            {
                if(name==nameof(Department))
                {
                    return _context.Departments.Select(s => s.Name);
                }
                else if(name==nameof(RoleDetail.Location))
                {
                    return _context.Locations.Select(s => s.Name);
                }
                else
                {
                    return _context.Projects.Select(s => s.Name);
                }
            }
            catch (Exception)
            {
                return [];
            }
        }

    }
}
