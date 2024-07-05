using Data.Interfaces;
using Data.Models;
using EmployeeDirectory;

namespace Data.Provider
{
    public class LocationRepository(AshwithEmployeeDirectoryContext context) : ILocationRepository
    {
        private readonly AshwithEmployeeDirectoryContext _context = context;

        public Location? GetLocation(int id)
        {
            return _context.Locations.Where(s => s.Id == id).FirstOrDefault();
        }

        public IEnumerable<string> GetLocationsByRole(int role)
        {
            try
            {
                return (from location in _context.Locations
                        join roleDetail in _context.RoleDetails on location.Id equals roleDetail.LocationId
                        join roles in _context.Roles on roleDetail.RoleId equals roles.Id
                        where roles.Id.Equals(role)
                        select location.Name);
            }
            catch { return []; }
        }

        public IEnumerable<int> GetLocationIdsByRole(int role)
        {
            try
            {
                return (from location in _context.Locations
                        join roleDetail in _context.RoleDetails on location.Id equals roleDetail.LocationId
                        join roles in _context.Roles on roleDetail.RoleId equals roles.Id
                        where roles.Id.Equals(role)
                        select location.Id);
            }
            catch { return []; }
        }

        public Location GetLocationById(int id)
        {
            return _context.Locations.Where(s => s.Id == id).First();
        }
    }
}
