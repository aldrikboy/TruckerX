using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Locations
{
    public class CountryCanada : BaseCountry
    {
        public override string Name => "Canada";

        public override float TrafficMultiplier => 1.0f;
        public override float DetourMultiplier => 1.4f;

        public CountryCanada() : base(new List<BasePlace>() { new PlaceOttawa(), new PlaceWinnipeg() })
        {

        }
    }
}
