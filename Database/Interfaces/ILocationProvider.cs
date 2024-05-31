using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ILocationProvider
    {
        public Location? GetLocation(int id);

        public IEnumerable<Location> GetLocations();

        public Location GetLocationByName(string name);

        public IEnumerable<string> GetLocationsByRole(string role);

        public Location GetLocationById(int id);
    }
}
