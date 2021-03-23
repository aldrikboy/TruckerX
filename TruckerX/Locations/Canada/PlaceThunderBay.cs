using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Locations
{
    class PlaceThunderBay : BasePlace
    {
        public override string Name { get { return "Thunder Bay"; } }

        public override double Lattitude { get { return 48.4311848; } }
        public override double Longtitude { get { return -89.4259172; } }

        public override double MapX { get { return 0.240; } }
        public override double MapY { get { return 0.393; } }

        public override PlaceSize Size => PlaceSize.Small;
    }
}
