using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Provider
{
    public class RoleDetailsProvider:IRoleDetailProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        public RoleDetailsProvider(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }
        
        public IEnumerable<RoleDetail> GetRoleDetails()
        {
            return _context.RoleDetails;
        }

        public void Add(RoleDetail roleDetail)
        {
            _context.RoleDetails.Add(roleDetail);
            _context.SaveChanges();
        }
    }
}
