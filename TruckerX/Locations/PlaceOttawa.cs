using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    class PlaceOttawa : BasePlace
    {
        public override string Name { get { return "Ottawa"; } }

        public override double Lattitude { get { return 45.249814; } }
        public override double Longtitude { get { return -76.0804321; } }

        public override double MapX { get { return 0.12; } }
        public override double MapY { get { return 0.1; } }

        public override List<BasePlace> GetConnections()
        {
            return new List<BasePlace>() { WorldData.GetPlaceByName("Winnipeg") };
        }
    }
}
