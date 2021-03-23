using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    class PlaceQuebec : BasePlace
    {
        public override string Name { get { return "Quebec"; } }

        public override double Lattitude { get { return 46.856283; } }
        public override double Longtitude { get { return -71.4817738; } }

        public override double MapX { get { return 0.288; } }
        public override double MapY { get { return 0.407; } }

        public override PlaceSize Size => PlaceSize.Large;
    }
}
