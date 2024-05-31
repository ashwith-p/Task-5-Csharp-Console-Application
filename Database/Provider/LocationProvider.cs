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
    public class LocationProvider:ILocationProvider
    {
        private readonly AshwithEmployeeDirectoryContext _context;
        public LocationProvider(AshwithEmployeeDirectoryContext context)
        {
            _context = context;
        }
        public Location? GetLocation(int id)
        {
            return _context.Locations.Where(s => s.Id == id).FirstOrDefault();
        }
        public IEnumerable<Location> GetLocations()
        {
            return _context.Locations;
        }
        public Location GetLocationByName(string name)
        {
            return _context.Locations.Where(s => s.Name == name).First();
        }

        public IEnumerable<string> GetLocationsByRole(string role)
        {
            try
            {
                int id = _context.Roles.Where(s => s.Name == role).Select(s => s.Id).FirstOrDefault();
                return (from location in _context.Locations
                        join roleDetail in _context.RoleDetails on location.Id equals roleDetail.LocationId
                        join roles in _context.Roles on roleDetail.RoleId equals roles.Id
                        where roles.Id.Equals(id)
                        select location.Name);
            }
            catch { return []; }
        }

        public Location GetLocationById(int id)
        {
            return _context.Locations.Where(s => s.Id == id).First();
        }
    }
}
