using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{ 
    class PlaceMontreal : BasePlace
    {
        public override string Name { get { return "Montreal"; } }

        public override double Lattitude { get { return 45.5579564; } }
        public override double Longtitude { get { return -73.8703839; } }

        public override double MapX { get { return 0.281; } }
        public override double MapY { get { return 0.414; } }

        public override PlaceSize Size => PlaceSize.Large;
    }
}
