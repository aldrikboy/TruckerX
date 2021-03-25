using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.TransportableItems;

namespace TruckerX.State
{
    [Serializable]
    public static class WorldState
    {
        public static List<PlaceState> OwnedPlaces { get; set; }

        private static int freeEmployeeId = 1;
        public static int FreeId { get { return freeEmployeeId++; } }

        public static EmployeeState GetEmployeeById(string id)
        {
            foreach(var place in OwnedPlaces)
            {
                foreach(var employee in place.Employees)
                {
                    if (employee.Id == id) return employee;
                }
            }
            return null;
        }

        public static void CreateDefault()
        {
            OwnedPlaces = new List<PlaceState>();
            OwnedPlaces.Add(new PlaceState(WorldData.GetPlaceByName("Winnipeg")));

            var winnipeg = GetStateForPlace(WorldData.GetPlaceByName("Winnipeg"));
            var employees = winnipeg.Employees;
            for (int i = 0; i < 2; i++) employees.Add(EmployeeState.GenerateNew(winnipeg));

            var jobs = winnipeg.AvailableJobs;
            jobs.Add(new JobOffer(1, 
                "Good Foods Inc",
                winnipeg.Place, 
                new List<BasePlace>() { 
                    WorldData.GetPlaceByName("Thunder Bay"),
                    WorldData.GetPlaceByName("Nipigon"),
                    WorldData.GetPlaceByName("Sudbury"),
                },
                WorldData.GetPlaceByName("Ottawa"), 
                TransportableItem.Bananas, 
                400, 
                new List<Weekday>() { Weekday.Friday, Weekday.Saturday, Weekday.Sunday }));

            var existingJob = new JobOffer(2,
                "Nutty Inc",
                winnipeg.Place,
                new List<BasePlace>() {
                    WorldData.GetPlaceByName("Thunder Bay"),
                    WorldData.GetPlaceByName("Nipigon"),
                    WorldData.GetPlaceByName("Sudbury"),
                },
                WorldData.GetPlaceByName("Ottawa"),
                TransportableItem.Peanuts,
                350,
                new List<Weekday>() { Weekday.Monday, Weekday.Tuesday });
            winnipeg.Docks[0].Schedule.Jobs.Add(new ScheduledJob(existingJob,  
                new Dictionary<Weekday, ShipTimeAssignment>() { 
                    { Weekday.Monday, new ShipTimeAssignment(new TimeSpan(7, 15,0), WorldState.GetEmployeeById("#000001")) },
                    { Weekday.Tuesday, new ShipTimeAssignment(new TimeSpan(12, 30, 0), WorldState.GetEmployeeById("#000002")) } }));
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

    public class DockState
    {
        public PlaceSchedule Schedule { get; set; }
        public bool Unlocked { get; set; }

        public DockState(bool unlocked = false)
        {
            Schedule = new PlaceSchedule();
            Unlocked = unlocked;
        }
    }

    public class PlaceState
    {
        public BasePlace Place { get; set; }
        public List<EmployeeState> Employees { get; set; }
        public List<JobOffer> AvailableJobs { get; set; }
        public List<DockState> Docks { get; set; }

        public PlaceState(BasePlace place)
        {
            Employees = new List<EmployeeState>();
            AvailableJobs = new List<JobOffer>();          
            Docks = new List<DockState>();
            Docks.Add(new DockState(true));
            for (int i = 0; i < 6; i++) Docks.Add(new DockState(false));
            Place = place;
        }

        public void PlanJob(DockState dock, ScheduledJob job)
        {
            dock.Schedule.Jobs.Add(job);
            AvailableJobs.Remove(job.Job);
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
