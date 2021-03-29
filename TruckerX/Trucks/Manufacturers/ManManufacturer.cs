using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Trucks.Manufacturers
{
    public class ManManufacturer : BaseTruckManufacturer
    {
        public override string Name => "MAN";

        public override List<Truck> GetTrucks()
        {
            return new List<Truck>() { new Truck("TGX D38", 130_000, 27.5f) };
        }
    }
}
