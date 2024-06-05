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

        public IEnumerable<string> GetLocationsByRole(int role);

        public Location GetLocationById(int id);

        public IEnumerable<int> GetLocationIdsByRole(int role);
    }


}
