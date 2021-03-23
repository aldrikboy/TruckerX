using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    public class PlaceWinnipeg : BasePlace
    {
        public override string Name { get { return "Winnipeg"; } }

        public override double Lattitude { get { return 49.8537377; } }
        public override double Longtitude { get { return -97.2923062; } }

        public override double MapX { get { return 0.221; } }
        public override double MapY { get { return 0.379; } }

        public override PlaceSize Size => PlaceSize.Large;
    }
}
