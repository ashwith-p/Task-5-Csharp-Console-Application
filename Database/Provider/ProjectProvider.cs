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
    public class ProjectProvider:IProjectProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        public ProjectProvider(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }
        public Project? GetProject(int? id)
        {
            if (id == null) return null;
            return _context.Projects.Where(s => s.Id == id).FirstOrDefault();
        }
        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects;
        }
        public Project? GetProjectByName(string? name)
        {
            try
            {
                return _context.Projects.Where(s => s.Name == name).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
