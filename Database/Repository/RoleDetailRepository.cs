using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;

namespace Data.Provider
{
    public class RoleDetailsRepository(AshwithEmployeeDirectoryContext context) : IRoleDetailRepository
    {
        private readonly AshwithEmployeeDirectoryContext _context = context;

        public void Add(RoleDetail roleDetail)
        {
            _context.RoleDetails.Add(roleDetail);
            _context.SaveChanges();
        }
    }
}
