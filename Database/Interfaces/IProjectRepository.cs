using Data.Models;

namespace Data.Interfaces
{
    public interface IProjectRepository
    {
        public Project? GetProject(int? id);

        public IEnumerable<Project> GetAllProjects();
    }
}
