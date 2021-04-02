using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Trucks
{
    public abstract class BaseTruckManufacturer
    {
        public abstract string Name { get; }
        public List<BaseTruck> Trucks { get; }

        public BaseTruckManufacturer()
        {
            Trucks = GetTrucks();
        }

        public abstract List<BaseTruck> GetTrucks();
    }
}
