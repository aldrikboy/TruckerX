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

        public override double MapX { get { return 0.275; } }
        public override double MapY { get { return 0.415; } }

        public override PlaceSize Size => PlaceSize.Medium;
    }
}
