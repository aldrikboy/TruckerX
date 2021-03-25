using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;
using TruckerX.TransportableItems;

namespace TruckerX.Locations
{
    public static class WorldData
    {
        public static List<BaseCountry> Countries { get; set; } = new List<BaseCountry>();

        public static void CreateData()
        {
            Countries.Add(new CountryCanada());

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

    public abstract class BaseCountry
    {
        public abstract string Name { get; }
        public List<BasePlace> Places { get; }
        public abstract float TrafficMultiplier { get; }
        public abstract float DetourMultiplier { get; }

        public abstract void MakeConnections();

        public abstract Currency Currency { get; }

        protected void Connect(string p1, string p2)
        {
            var pp1 = WorldData.GetPlaceByName(p1);
            var pp2 = WorldData.GetPlaceByName(p2);
            pp1.Connections.Add(pp2);
            pp2.Connections.Add(pp1);
        }

        public BaseCountry(List<BasePlace> places)
        {
            Places = places;   
            foreach(var place in Places)
            {
                place.Country = this;
            }
        }
    }

    public enum PlaceSize
    {
        Small = 3,
        Medium = 2,
        Large = 1,
    }

    public abstract class BasePlace
    {
        public abstract PlaceSize Size { get; }

        public abstract string Name { get; }
        public abstract double Longtitude { get; }
        public abstract double Lattitude { get; }

        public abstract double MapX { get; }
        public abstract double MapY { get; }

        public List<BasePlace> Connections { get; set; }

        public BaseCountry Country { get; set; }

        public BasePlace()
        {
            Connections = new List<BasePlace>();
        }
    }
}
