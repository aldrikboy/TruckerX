using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.Locations.Canada;

namespace TruckerX.Locations
{
#if false
    public class CountryCanada : BaseCountry
    {
        public override string Name => "Canada";

        public override float TrafficMultiplier => 1.0f;
        public override float DetourMultiplier => 1.3f;

        public override Currency Currency => Currency.CAD;
        public override float GasPricePerLiter => 1.219f;

        public CountryCanada() : base(typeof(PlaceCalgary).Namespace)
        {
            
        }

        public override void MakeConnections()
        {
            Connect("Calgary", "Kamloops");
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
            Connect("St. Andrews", "Sally's Cove");
            Connect("St. Anthony", "Sally's Cove");
            Connect("Gander", "Sally's Cove");
            Connect("Gander", "Sait John's");
            Connect("Quebec", "Sagvenay"); 
            Connect("Fort McMurray", "Edmonton");
            Connect("Fort McMurray", "Saskatoon");
            Connect("Dawson Creek", "Fort McMurray");
            Connect("Dawson Creek", "Edmonton");
            Connect("Dawson Creek", "Prince George");
            Connect("Prince George", "Edmonton");
            Connect("Kamloops", "Prince George");
            Connect("Kamloops", "Vancouver");
        }
    }
#endif
}
