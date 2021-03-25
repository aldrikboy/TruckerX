using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;

namespace TruckerX.Locations
{
    public class CountryCanada : BaseCountry
    {
        public override string Name => "Canada";

        public override float TrafficMultiplier => 1.0f;
        public override float DetourMultiplier => 1.4f;

        public override Currency Currency => Currency.CAD;

        public CountryCanada() : base(new List<BasePlace>() { 
            new PlaceOttawa(), new PlaceWinnipeg(), new PlaceThunderBay(), new PlaceNipigon(),
            new PlaceMontreal(), new PlaceQuebec(), new PlaceSudbury(), new PlaceToronto() })
        {

        }

        public override void MakeConnections()
        {
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
