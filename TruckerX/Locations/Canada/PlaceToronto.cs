using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    class PlaceToronto : BasePlace
    {
        public override string Name { get { return "Toronto"; } }

        public override double Lattitude { get { return 43.7181557; } }
        public override double Longtitude { get { return -79.6582408; } }

        public override double MapX { get { return 0.265; } }
        public override double MapY { get { return 0.423; } }

        public override PlaceSize Size => PlaceSize.Large;
    }
}
