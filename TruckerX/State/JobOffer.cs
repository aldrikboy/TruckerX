using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.TransportableItems;
using GeoCoordinatePortable;
using Microsoft.Xna.Framework;

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
        public string Company { get; set; }
        public BasePlace From { get; set; }
        public List<BasePlace> Connections { get; set; } = new List<BasePlace>();
        public BasePlace To { get; set; }
        public TransportableItem Item { get; set; }
        public decimal OfferedReward { get; set; }
        public List<Weekday> ShipDays { get; set; }

        public bool IsReturnDrive { get { return Id == -1; } }

        private double distance = -1; // We store this so we only have to calculate this once.

        public JobOffer(int id, string company, BasePlace from, List<BasePlace> connections, BasePlace to, TransportableItem item, decimal reward, List<Weekday> shipDays)
        {
            Id = id;
            From = from;
            To = to;
            Item = item;
            OfferedReward = reward;
            ShipDays = shipDays;
            Connections = connections;
            Company = company;
        }

        public JobOffer Reverse()
        {
            var reversedConnections = new List<BasePlace>();
            for (int i = Connections.Count-1; i >= 0; i--)
            {
                reversedConnections.Add(Connections[i]);
            }
            var reversePath = new JobOffer(-1, Company, this.To, reversedConnections, this.From, null, 0, null);
            return reversePath;
        }

        public Vector2 ProgressPercentageToWorldLocation(float percentage)
        {
            var allLocations = new List<BasePlace>();
            allLocations.AddRange(Connections);
            allLocations.Add(To);

            var looingForKm = GetDistanceInKm() * percentage;

            double currentKm;
            double prevKm;
            float percentageCompletedOfBetween2Connections;

            var prevPlace = From;
            var sCoord = new GeoCoordinate(From.Lattitude, From.Longtitude);
            double totalDistance = 0.0;
            double prevTotalDistance;
            foreach (var place in allLocations)
            {
                var coord = new GeoCoordinate(place.Lattitude, place.Longtitude);
                prevTotalDistance = totalDistance;
                totalDistance += sCoord.GetDistanceTo(coord);

                currentKm = Math.Round(totalDistance / 1000.0);
                prevKm = Math.Round(prevTotalDistance / 1000.0);
                if (currentKm > looingForKm) // Truck is between sCoord and coord.
                {
                    percentageCompletedOfBetween2Connections = (float)((looingForKm - prevKm)/(currentKm - prevKm));
                    return new Vector2((float)(prevPlace.MapX + ((place.MapX- prevPlace.MapX)*percentageCompletedOfBetween2Connections)), 
                        (float)(prevPlace.MapY + ((place.MapY - prevPlace.MapY)*percentageCompletedOfBetween2Connections)));
                }

                prevPlace = place;
                sCoord = coord;
            }

            throw new Exception("Invalid Job, it likely has no destination.");
        }

        public double GetDistanceInKm()
        {
            if (distance != -1) return distance;

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

            distance = Math.Round(totalDistance / 1000.0);
            return distance;
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
