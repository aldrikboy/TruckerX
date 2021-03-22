using System;
using System.Collections.Generic;
using System.Text;
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
                country.GetConnections();
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

        public void GetConnections()
        {
            foreach (var item in Places)
            {
                item.Connections = item.GetConnections();
            }
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

    public abstract class BasePlace
    {
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

        public abstract List<BasePlace> GetConnections();
    }
}
