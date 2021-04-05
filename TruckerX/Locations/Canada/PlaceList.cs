using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.TransportableItems;

namespace TruckerX.Locations.Canada
{
    class PlaceMontreal : BasePlace
    {
        public override string Name { get { return "Montreal"; } }

        public override double Lattitude { get { return 45.5579564; } }
        public override double Longtitude { get { return -73.8703839; } }

        public override double MapX { get { return 0.281; } }
        public override double MapY { get { return 0.414; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }

    class PlaceNipigon : BasePlace
    {
        public override string Name { get { return "Nipigon"; } }

        public override double Lattitude { get { return 49.0022436; } }
        public override double Longtitude { get { return -88.259918; } }

        public override double MapX { get { return 0.244; } }
        public override double MapY { get { return 0.389; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    class PlaceOttawa : BasePlace
    {
        public override string Name { get { return "Ottawa"; } }

        public override double Lattitude { get { return 45.249814; } }
        public override double Longtitude { get { return -76.0804321; } }

        public override double MapX { get { return 0.275; } }
        public override double MapY { get { return 0.415; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 600_000M;
        public override decimal DockPrice => 200_000M;
    }

    class PlaceQuebec : BasePlace
    {
        public override string Name { get { return "Quebec"; } }

        public override double Lattitude { get { return 46.856283; } }
        public override double Longtitude { get { return -71.4817738; } }

        public override double MapX { get { return 0.288; } }
        public override double MapY { get { return 0.407; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }

    class PlaceSudbury : BasePlace
    {
        public override string Name { get { return "Sudbury"; } }

        public override double Lattitude { get { return 46.584065; } }
        public override double Longtitude { get { return -81.3592996; } }

        public override double MapX { get { return 0.261; } }
        public override double MapY { get { return 0.404; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 600_000M;
        public override decimal DockPrice => 200_000M;
    }

    class PlaceThunderBay : BasePlace
    {
        public override string Name { get { return "Thunder Bay"; } }

        public override double Lattitude { get { return 48.4311848; } }
        public override double Longtitude { get { return -89.4259172; } }

        public override double MapX { get { return 0.240; } }
        public override double MapY { get { return 0.393; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    class PlaceToronto : BasePlace
    {
        public override string Name { get { return "Toronto"; } }

        public override double Lattitude { get { return 43.7181557; } }
        public override double Longtitude { get { return -79.6582408; } }

        public override double MapX { get { return 0.265; } }
        public override double MapY { get { return 0.423; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }

    public class PlaceWinnipeg : BasePlace
    {
        public override string Name { get { return "Winnipeg"; } }

        public override double Lattitude { get { return 49.8537377; } }
        public override double Longtitude { get { return -97.2923062; } }

        public override double MapX { get { return 0.221; } }
        public override double MapY { get { return 0.379; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }

    public class PlaceRegina : BasePlace
    {
        public override string Name { get { return "Regina"; } }

        public override double Lattitude { get { return 50.4585732; } }
        public override double Longtitude { get { return -104.7054482; } }

        public override double MapX { get { return 0.200; } }
        public override double MapY { get { return 0.372; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 600_000M;
        public override decimal DockPrice => 200_000M;
    }

    public class PlaceSaskatoon : BasePlace
    {
        public override string Name { get { return "Saskatoon"; } }

        public override double Lattitude { get { return 52.1504533; } }
        public override double Longtitude { get { return -106.8044928; } }

        public override double MapX { get { return 0.197; } }
        public override double MapY { get { return 0.359; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 600_000M;
        public override decimal DockPrice => 200_000M;
    }

    public class PlaceEdmonton : BasePlace
    {
        public override string Name { get { return "Edmonton"; } }

        public override double Lattitude { get { return 53.5557125; } }
        public override double Longtitude { get { return -113.632803; } }

        public override double MapX { get { return 0.174; } }
        public override double MapY { get { return 0.347; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }

    public class PlaceCalgary : BasePlace
    {
        public override string Name { get { return "Calgary"; } }

        public override double Lattitude { get { return 51.0275396; } }
        public override double Longtitude { get { return -114.2279158; } }

        public override double MapX { get { return 0.173; } }
        public override double MapY { get { return 0.364; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }

    public class PlaceVancouver : BasePlace
    {
        public override string Name { get { return "Vancouver"; } }

        public override double Lattitude { get { return 49.2577143; } }
        public override double Longtitude { get { return -123.1939432; } }

        public override double MapX { get { return 0.150; } }
        public override double MapY { get { return 0.373; } }

        public override PlaceSize Size => PlaceSize.Large;

        public override decimal GaragePrice => 1_000_000M;
        public override decimal DockPrice => 500_000M;
    }
    /// <summary>
    /// 
    /// </summary>
    public class PlaceBrandon : BasePlace
    {
        public override string Name { get { return "Brandon"; } }

        public override double Lattitude { get { return 49.8636371; } }
        public override double Longtitude { get { return -99.9417297; } }

        public override double MapX { get { return 0.214; } }
        public override double MapY { get { return 0.378; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceYorkton : BasePlace
    {
        public override string Name { get { return "Yorkton"; } }

        public override double Lattitude { get { return 51.2138531; } }
        public override double Longtitude { get { return -102.491999; } }

        public override double MapX { get { return 0.210; } }
        public override double MapY { get { return 0.370; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceDawsonCreek : BasePlace
    {
        public override string Name { get { return "Dawson Creek"; } }

        public override double Lattitude { get { return 55.7595916; } }
        public override double Longtitude { get { return -120.2532268; } }

        public override double MapX { get { return 0.163; } }
        public override double MapY { get { return 0.328; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlacePrinceGeorge : BasePlace
    {
        public override string Name { get { return "Prince George"; } }

        public override double Lattitude { get { return 53.926817; } }
        public override double Longtitude { get { return -122.8924989; } }

        public override double MapX { get { return 0.159; } }
        public override double MapY { get { return 0.339; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceKamloops : BasePlace
    {
        public override string Name { get { return "Kamloops"; } }

        public override double Lattitude { get { return 50.741671; } }
        public override double Longtitude { get { return -120.4388103; } }

        public override double MapX { get { return 0.158; } }
        public override double MapY { get { return 0.362; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceFortMcMurray : BasePlace
    {
        public override string Name { get { return "Fort McMurray"; } }

        public override double Lattitude { get { return 56.6970068; } }
        public override double Longtitude { get { return -111.4878523; } }

        public override double MapX { get { return 0.192; } }
        public override double MapY { get { return 0.324; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceSagvenay : BasePlace
    {
        public override string Name { get { return "Sagvenay"; } }

        public override double Lattitude { get { return 48.378383; } }
        public override double Longtitude { get { return -71.2736968; } }

        public override double MapX { get { return 0.290; } }
        public override double MapY { get { return 0.396; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceSaintJohns : BasePlace
    {
        public override string Name { get { return "Sait John's"; } }

        public override double Lattitude { get { return 47.4818262; } }
        public override double Longtitude { get { return -52.9677826; } }

        public override double MapX { get { return 0.339; } }
        public override double MapY { get { return 0.403; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceGander : BasePlace
    {
        public override string Name { get { return "Gander"; } }

        public override double Lattitude { get { return 48.9439597; } }
        public override double Longtitude { get { return -54.6795712; } }

        public override double MapX { get { return 0.334; } }
        public override double MapY { get { return 0.398; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceSallysCove : BasePlace
    {
        public override string Name { get { return "Sally's Cove"; } }

        public override double Lattitude { get { return 49.7304707; } }
        public override double Longtitude { get { return -57.95413; } }

        public override double MapX { get { return 0.327; } }
        public override double MapY { get { return 0.392; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceStAnthony : BasePlace
    {
        public override string Name { get { return "St. Anthony"; } }

        public override double Lattitude { get { return 51.376658; } }
        public override double Longtitude { get { return -55.6414953; } }

        public override double MapX { get { return 0.334; } }
        public override double MapY { get { return 0.382; } }

        public override PlaceSize Size => PlaceSize.Medium;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }

    public class PlaceStAndrews : BasePlace
    {
        public override string Name { get { return "St. Andrews"; } }

        public override double Lattitude { get { return 47.7776121; } }
        public override double Longtitude { get { return -59.2820118; } }

        public override double MapX { get { return 0.323; } }
        public override double MapY { get { return 0.403; } }

        public override PlaceSize Size => PlaceSize.Small;

        public override decimal GaragePrice => 300_000M;
        public override decimal DockPrice => 100_000M;
    }
}
