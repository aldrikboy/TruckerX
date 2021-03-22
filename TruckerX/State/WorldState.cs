using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.TransportableItems;

namespace TruckerX.State
{
    [Serializable]
    public class WorldState
    {
        public static List<PlaceState> OwnedPlaces { get; set; }

        public static void CreateDefault()
        {
            OwnedPlaces = new List<PlaceState>();
            OwnedPlaces.Add(new PlaceState(WorldData.GetPlaceByName("Winnipeg")));

            var winnipeg = GetStateForPlace(WorldData.GetPlaceByName("Winnipeg"));
            var employees = winnipeg.Employees;
            for (int i = 0; i < 7; i++) employees.Add(EmployeeState.GenerateNew());

            var jobs = winnipeg.AvailableJobs;
            jobs.Add(new JobOffer(1, winnipeg.Place, WorldData.GetPlaceByName("Ottawa"), TransportableItem.Bananas, 400, new List<Weekday>() { Weekday.Monday, Weekday.Wednesday }));
        }

        public static bool PlaceOwned(BasePlace place)
        {
            foreach(var item in OwnedPlaces)
            {
                if (item.Place.GetType() == place.GetType())
                {
                    return true;
                }
            }
            return false;
        }

        public static PlaceState GetStateForPlace(BasePlace place)
        {
            foreach (var item in OwnedPlaces)
            {
                if (item.Place.GetType() == place.GetType())
                {
                    return item;
                }
            }
            throw new Exception("Place is not owned yet");
        }
    }

    public class PlaceState
    {
        public BasePlace Place { get; set; }
        public List<EmployeeState> Employees { get; set; }
        public List<JobOffer> AvailableJobs { get; set; }

        public PlaceState(BasePlace place)
        {
            Employees = new List<EmployeeState>();
            AvailableJobs = new List<JobOffer>();
            Place = place;
        }

        public int ActiveEmployeeCount()
        {
            int count = 0;
            foreach(var item in Employees)
            {
                if (!item.OnPaidLeave) count++;
            }
            return count;
        }
    }
}
