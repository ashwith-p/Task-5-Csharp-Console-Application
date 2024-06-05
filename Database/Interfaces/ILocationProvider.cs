﻿using Data.Models;

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
