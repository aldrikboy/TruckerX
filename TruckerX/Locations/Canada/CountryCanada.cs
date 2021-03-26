using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.Locations.Canada;

namespace TruckerX.Locations
{
    public class CountryCanada : BaseCountry
    {
        public override string Name => "Canada";

        public override float TrafficMultiplier => 1.0f;
        public override float DetourMultiplier => 1.3f;

        public override Currency Currency => Currency.CAD;

        public CountryCanada() : base(typeof(PlaceCalgary).Namespace)
        {
            
        }

        public override void MakeConnections()
        {
            Connect("Calgary", "Vancouver");
            Connect("Edmonton", "Saskatoon");
            Connect("Calgary", "Edmonton");
            Connect("Regina", "Calgary");
            Connect("Regina", "Yorkton");
            Connect("Regina", "Saskatoon");
            Connect("Yorkton", "Saskatoon");
            Connect("Brandon", "Yorkton");
            Connect("Brandon", "Regina");
            Connect("Winnipeg", "Brandon");
            Connect("Winnipeg", "Thunder Bay");
            Connect("Thunder Bay", "Nipigon");
            Connect("Nipigon", "Sudbury");
            Connect("Sudbury", "Ottawa");
            Connect("Sudbury", "Toronto");
            Connect("Ottawa", "Montreal");
            Connect("Ottawa", "Toronto");
            Connect("Montreal", "Quebec");
        }
    }
}
