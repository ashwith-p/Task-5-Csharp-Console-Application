using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRoleDetailProvider
    {
        public IEnumerable<RoleDetail> GetRoleDetails();

        public void Add(RoleDetail roleDetail);
    }
}
