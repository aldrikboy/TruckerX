using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    public static class WorldData
    {
        public static List<BaseCountry> Countries { get; set; } = new List<BaseCountry>();

        public static void MakeConnections()
        {
            foreach(var country in Countries)
            {
                country.MakeConnections();
            }
        }

        public static BasePlace GetPlaceByName(string name)
        {
            foreach(var country in Countries)
            {
                foreach(var place in country.Places)
                {
                    if (place.Name == name) return place;
                }
            }
            throw new Exception("Place does not exist.");
        }
    }


    public class BaseCountry
    {
        public string Name { get; set; }
        public List<BasePlace> Places { get; set; }
        public float TrafficMultiplier { get; set; }
        public float DetourMultiplier { get; set; }
        public float GasPricePerLiter { get; set; }

        [JsonIgnore]
        public Currency Currency { get; } = Currency.USD;

        public void MakeConnections()
        {
            foreach(var place in Places)
            {
                place.Country = this;
                foreach(var placeName in place.ConnectedTo)
                {
                    Connect(placeName, place.Name);
                }
            }
        }

        protected void Connect(string p1, string p2)
        {
            var pp1 = WorldData.GetPlaceByName(p1);
            var pp2 = WorldData.GetPlaceByName(p2);
            if (!pp1.Connections.Contains(pp2)) pp1.Connections.Add(pp2);
            if (!pp2.Connections.Contains(pp1)) pp2.Connections.Add(pp1);
        }

        public BaseCountry()
        {

        }
    }

    public enum PlaceSize
    {
        Small = 3,
        Medium = 2,
        Large = 1,
    }

    public class BasePlace
    {
        public PlaceSize Size { get; set; }

        public string Name { get; set; }
        public double Longtitude { get; set; }
        public double Lattitude { get; set; }

        public decimal GaragePrice { get; set; }
        public decimal DockPrice { get; set; }

        public List<string> ConnectedTo { get; set; }

        [JsonIgnore]
        public List<BasePlace> Connections { get; set; } = new List<BasePlace>();
        [JsonIgnore]
        public BaseCountry Country { get; set; }

        public BasePlace()
        {

        }
    }

#if false
public class BaseCountry
    {
        public string Name;
        public float TrafficMultiplier;
        public float DetourMultiplier;
        public float GasPricePerLiter;

        public Currency Currency { get; }
        public List<BasePlace> Places { get; } = new List<BasePlace>();
    }

    public enum PlaceSize
    {
        Small = 3,
        Medium = 2,
        Large = 1,
    }

    public class BasePlace
    {
        public PlaceSize Size { get; }

        public string Name;
        public double Longtitude;
        public double Lattitude;
        public decimal GaragePrice;
        public decimal DockPrice;
        public double MapX;
        public double MapY;
        public string CountryName;
        public string[] ConnectionNames;

        public List<BasePlace> Connections { get; set; } = new List<BasePlace>();
        public BaseCountry Country { get; set; }

        public BasePlace()
        {
          
        }

        public void ConnectToCountry()
        {
            foreach (var item in WorldData.Countries)
            {
                if (item.Name == CountryName)
                {
                    item.Places.Add(this);
                    Country = item;
                    return;
                }
            }
        }

        public void MakeConnections()
        {
            foreach(var item in ConnectionNames)
            {
                if (Connections.Any(e => e.Name == item)) continue;
                var place = WorldData.GetPlaceByName(item);
                place.Connections.Add(this);
                Connections.Add(place);
            }
        }

#endif
}
