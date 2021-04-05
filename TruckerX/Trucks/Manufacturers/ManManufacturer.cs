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
            return new List<BaseTruck>() { new Truck_TGX_D38(), new Truck_TGS_6X4() };
        }
    }

    public class Truck_TGX_D38 : BaseTruck
    {
        public override string Name => "TGX D38";
        public override decimal Price => 130_000;
        public override float LiterPer100Km => 27.5f;
        public override int HorsePower => 640;
    }

    public class Truck_TGS_6X4 : BaseTruck
    {
        public override string Name => "TGS 6X4";
        public override decimal Price => 120_000;
        public override float LiterPer100Km => 33.5f;
        public override int HorsePower => 540;
    }
}
