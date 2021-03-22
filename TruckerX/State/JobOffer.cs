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
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    public class JobOffer
    {
        public int Id { get; set; }
        public BasePlace From { get; set; }
        public BasePlace To { get; set; }
        public TransportableItem Item { get; set; }
        public decimal OfferedReward { get; set; }
        public List<Weekday> ShipDays { get; set; }

        public JobOffer(int id, BasePlace from, BasePlace to, TransportableItem item, decimal reward, List<Weekday> shipDays)
        {
            Id = id;
            From = from;
            To = to;
            Item = item;
            OfferedReward = reward;
            ShipDays = shipDays;
        }

        public double GetDistanceInKm()
        {
            var sCoord = new GeoCoordinate(From.Lattitude, From.Longtitude);
            var eCoord = new GeoCoordinate(To.Lattitude, To.Longtitude);

            return Math.Round(sCoord.GetDistanceTo(eCoord) / 1000.0);
        }

        public double GetTravelTime(double Distance)
        {
            double totalDriveTime = Distance / 100.0f; // Trucks drive 100km/h everywhere for now.
            float randomDelayMultiplier = From.Country.DetourMultiplier * From.Country.TrafficMultiplier; // Traffic + detour
            totalDriveTime *= randomDelayMultiplier;
            int pauseCount = (int)totalDriveTime / 4;
            totalDriveTime += (pauseCount * 0.25);  // 15min break every 4 hour drive.
            totalDriveTime = Math.Round(totalDriveTime, 2);
            return totalDriveTime;
        }
    }
}
