using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    class PlaceSudbury : BasePlace
    {
        public override string Name { get { return "Sudbury"; } }

        public override double Lattitude { get { return 46.584065; } }
        public override double Longtitude { get { return -81.3592996; } }

        public override double MapX { get { return 0.261; } }
        public override double MapY { get { return 0.404; } }

        public override PlaceSize Size => PlaceSize.Medium;
    }
}
