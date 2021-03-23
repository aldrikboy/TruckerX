using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Locations
{
    class PlaceNipigon : BasePlace
    {
        public override string Name { get { return "Nipigon"; } }

        public override double Lattitude { get { return 49.0022436; } }
        public override double Longtitude { get { return -88.259918; } }

        public override double MapX { get { return 0.244; } }
        public override double MapY { get { return 0.389; } }

        public override PlaceSize Size => PlaceSize.Small;
    }
}
