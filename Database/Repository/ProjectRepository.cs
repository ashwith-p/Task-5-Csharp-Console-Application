using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;

namespace Data.Provider
{
    public class ProjectRepository(AshwithEmployeeDirectoryContext context) : IProjectRepository
    {
        private readonly AshwithEmployeeDirectoryContext _context = context;

        public Project? GetProject(int? id)
        {
            if (id == null) return null;
            return _context.Projects.Where(s => s.Id == id).FirstOrDefault();
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Projects;
        }
    }
}
