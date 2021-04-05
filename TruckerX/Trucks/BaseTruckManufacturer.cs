using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Trucks.Manufacturers;

namespace TruckerX.Trucks
{
    public abstract class BaseTruckManufacturer
    {
        public abstract string Name { get; }
        public List<BaseTruck> Trucks { get; }

        public static ManManufacturer ManManufacturer = new ManManufacturer();

        public BaseTruckManufacturer()
        {
            Trucks = GetTrucks();
        }

        public abstract List<BaseTruck> GetTrucks();
    }
}
