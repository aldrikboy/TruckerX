using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.TransportableItems;
using TruckerX.Trucks;
using TruckerX.Trucks.Manufacturers;

namespace TruckerX.State
{
    [Serializable]
    public static class WorldState
    {
        public static List<PlaceState> OwnedPlaces { get; private set; }

        private static int freeEmployeeId = 1;
        public static int FreeId { get { return freeEmployeeId++; } }

        /// <summary>
        /// Returns all employees that were hired at the given location.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public static IEnumerable<EmployeeState> InternalEmployeesFromPlace(BasePlace place)
        {
            foreach (var ownedPlace in OwnedPlaces)
            {
                foreach (var employee in ownedPlace.Employees)
                {
                    if (employee.CurrentJob != null) continue;
                    if (employee.OriginalLocation == place) yield return employee;
                }
            }

            foreach (var job in Simulation.simulation.ActiveJobs)
            {
                if (job.Employee.OriginalLocation == place) yield return job.Employee;
            }
        }

        /// <summary>
        /// Return all employees that are at the given location but were not hired there.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public static IEnumerable<EmployeeState> ExternalEmployeesCurrentlyAt(BasePlace place)
        {
            foreach (var ownedPlace in OwnedPlaces)
            {
                foreach (var employee in ownedPlace.Employees)
                {
                    if (employee.CurrentLocation == place && employee.OriginalLocation != place) yield return employee;
                }
            }
        }

        public static EmployeeState GetEmployeeById(string id)
        {
            foreach(var place in OwnedPlaces)
            {
                foreach(var employee in place.Employees)
                {
                    if (employee.Id == id) return employee;
                }
            }
            if (Simulation.simulation == null) return null;
            foreach(var job in Simulation.simulation.ActiveJobs)
            {
                if (job.Employee.Id == id) return job.Employee;
            }
            return null;
        }

        public static void UnlockPlace(BasePlace place)
        {
            foreach (var p in OwnedPlaces)
            {
                if (p.Place.Name == place.Name) throw new Exception("Place is already unlocked.");
            }
            Simulation.simulation.Money -= place.GaragePrice;
            OwnedPlaces.Add(new PlaceState(place));
        }

        public static void CreateDefault()
        {
            OwnedPlaces = new List<PlaceState>();
            OwnedPlaces.Add(new PlaceState(WorldData.GetPlaceByName("Winnipeg")));
            OwnedPlaces.Add(new PlaceState(WorldData.GetPlaceByName("Quebec")));

            var winnipeg = GetStateForPlace(WorldData.GetPlaceByName("Winnipeg"));
            var employees = winnipeg.Employees;
            for (int i = 0; i < 2; i++)
            {
                var truck = new Truck_TGX_D38();
                winnipeg.Trucks.Add(truck);

                var employee = EmployeeState.GenerateNew(winnipeg);
                employee.AssignTruck(truck);
                employees.Add(employee);
            }

            var jobs = winnipeg.AvailableJobs;
            jobs.Add(new JobOffer(1, 
                "Good Foods Inc",
                winnipeg.Place, 
                WorldData.GetPlaceByName("Ottawa"), 
                TransportableItem.Bananas, 
                1100, 
                new List<Weekday>() { Weekday.Friday, Weekday.Saturday, Weekday.Sunday }));

            var existingJob = new JobOffer(2,
                "Nutty Inc",
                winnipeg.Place,
                WorldData.GetPlaceByName("Quebec"),
                TransportableItem.Peanuts,
                1350,
                new List<Weekday>() { Weekday.Monday, Weekday.Tuesday });
            winnipeg.Docks[0].Schedule.Jobs.Add(new ScheduledJob(existingJob,  
                new Dictionary<Weekday, ShipTimeAssignment>() { 
                    { Weekday.Monday, new ShipTimeAssignment(new TimeSpan(7, 15,0), WorldState.GetEmployeeById("#000002"), false) },
                    { Weekday.Tuesday, new ShipTimeAssignment(new TimeSpan(12, 30, 0), WorldState.GetEmployeeById("#000004"), false) } }));
        }

        public static bool PlaceOwned(BasePlace place)
        {
            foreach(var item in OwnedPlaces)
            {
                if (item.Place.Name == place.Name)
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
                if (item.Place.Name == place.Name)
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
        public List<BaseTruck> Trucks { get; set; }

        public event EventHandler OnEmployeeArrived;

        public void EmployeeStateChanged()
        {
            OnEmployeeArrived?.Invoke(null, null);
        }

        public void AddEmployee(EmployeeState employee)
        {
            employee.CurrentLocation = Place;
            this.Employees.Add(employee);
            EmployeeStateChanged();
        }

        public PlaceState(BasePlace place)
        {
            Trucks = new List<BaseTruck>();
            Employees = new List<EmployeeState>();
            AvailableJobs = new List<JobOffer>();          
            Docks = new List<DockState>();
            Docks.Add(new DockState(true));
            Docks.Add(new DockState(true));
            for (int i = 0; i < 5; i++) Docks.Add(new DockState(false));
            Place = place;
        }

        public void PlanJob(DockState dock, ScheduledJob job)
        {
            dock.Schedule.Jobs.Add(job);
            AvailableJobs.Remove(job.Job);
        }

        internal double GetWeeklyHours(EmployeeState employee)
        {
            double result = 0.0;
            foreach(var dock in Docks)
            {
                foreach(var job in dock.Schedule.Jobs)
                {
                    foreach(var assignment in job.ShipTimes)
                    {
                        if (assignment.Value.AssignedEmployee == employee)
                        {
                            result += job.Job.GetTravelTime();
                        }
                    }
                }
            }
            return result;
        }

        internal decimal GetWeeklyRevenue()
        {
            decimal result = 0.0M;
            foreach (var dock in Docks)
            {
                foreach (var job in dock.Schedule.Jobs)
                {
                    result += job.ShipTimes.Count * job.Job.OfferedReward;
                }
            }
            return result;
        }
    }
}
