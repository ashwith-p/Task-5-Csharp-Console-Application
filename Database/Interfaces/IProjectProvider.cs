using Data.Models;

namespace Data.Interfaces
{
    public interface IProjectProvider
    {
        public Project? GetProject(int? id);
    }
}
