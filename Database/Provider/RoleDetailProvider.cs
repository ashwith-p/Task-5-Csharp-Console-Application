using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;

namespace Data.Provider
{
    public class RoleDetailsProvider:IRoleDetailProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        public RoleDetailsProvider(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }

        public void Add(RoleDetail roleDetail)
        {
            _context.RoleDetails.Add(roleDetail);
            _context.SaveChanges();
        }
    }
}
