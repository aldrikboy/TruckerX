using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Trucks.Manufacturers
{
    public class ManManufacturer : BaseTruckManufacturer
    {
        public override string Name => "MAN";

        public override List<BaseTruck> GetTrucks()
        {
            return new List<BaseTruck>() { new Truck_TGX_D38() };
        }
    }

    public class Truck_TGX_D38 : BaseTruck
    {
        public override string Name => "TGX D38";
        public override decimal Price => 130_000;
        public override float LiterPer100Km => 27.5f;
    }
}
