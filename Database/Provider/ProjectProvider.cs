using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;

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
        
    }
}
