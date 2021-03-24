using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.TransportableItems;
using GeoCoordinatePortable;

namespace TruckerX.State
{
    public enum Weekday
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6,
    }

    public class JobOffer
    {
        public int Id { get; set; }
        public BasePlace From { get; set; }
        public List<BasePlace> Connections { get; set; } = new List<BasePlace>();
        public BasePlace To { get; set; }
        public TransportableItem Item { get; set; }
        public decimal OfferedReward { get; set; }
        public List<Weekday> ShipDays { get; set; }

        public JobOffer(int id, BasePlace from, List<BasePlace> connections, BasePlace to, TransportableItem item, decimal reward, List<Weekday> shipDays)
        {
            Id = id;
            From = from;
            To = to;
            Item = item;
            OfferedReward = reward;
            ShipDays = shipDays;
            Connections = connections;
        }

        public double GetDistanceInKm()
        {
            var sCoord = new GeoCoordinate(From.Lattitude, From.Longtitude);

            double totalDistance = 0.0;
            foreach(var place in Connections)
            {
                var coord = new GeoCoordinate(place.Lattitude, place.Longtitude);
                totalDistance += sCoord.GetDistanceTo(coord);
                sCoord = coord;
            }

            var eCoord = new GeoCoordinate(To.Lattitude, To.Longtitude);
            totalDistance += sCoord.GetDistanceTo(eCoord);

            return Math.Round(totalDistance / 1000.0);
        }

        private float GetDetourMultiplier()
        {
            float result = 0;
            result += From.Country.DetourMultiplier;
            foreach(var place in Connections)
            {
                result += place.Country.DetourMultiplier;
            }
            result += To.Country.DetourMultiplier;
            result /= (Connections.Count + 2);
            return result;
        }

        private float GetTrafficMultiplier()
        {
            float result = 0;
            result += From.Country.TrafficMultiplier;
            foreach (var place in Connections)
            {
                result += place.Country.TrafficMultiplier;
            }
            result += To.Country.TrafficMultiplier;
            result /= (Connections.Count + 2);
            return result;
        }

        public double GetTravelTime()
        {
            double totalDriveTime = GetDistanceInKm() / 100.0f; // Trucks drive 100km/h everywhere for now.
            float randomDelayMultiplier = GetDetourMultiplier() * GetTrafficMultiplier(); // Traffic + detour
            totalDriveTime *= randomDelayMultiplier;
            int pauseCount = (int)totalDriveTime / 4;
            totalDriveTime += (pauseCount * 0.25);  // 15min break every 4 hour drive.
            totalDriveTime = Math.Round(totalDriveTime, 2);
            return totalDriveTime;
        }
    }
}
