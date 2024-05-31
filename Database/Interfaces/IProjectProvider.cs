using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IProjectProvider
    {
        public Project? GetProject(int? id);

        public IEnumerable<Project> GetProjects();

        public Project? GetProjectByName(string? name);
    }
}
