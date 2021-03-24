using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.State;

namespace TruckerX
{
    public class ActiveJob
    {
        public ScheduledJob Job { get; set; }
        public EmployeeState Employee { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime EndDate { get; set; }

        public ActiveJob(ScheduledJob job, DateTime shipTime, EmployeeState employee)
        {
            this.Job = job;
            this.Employee = employee;
            this.ShipDate = shipTime;
            this.EndDate = ShipDate.AddHours(job.Job.GetTravelTime());
        }

        public Vector2 GetCurrentWorldLocation()
        {
            float currentPercentage = GetCompletionPercentage();
            return Job.Job.ProgressPercentageToWorldLocation(currentPercentage);
        }

        public float GetCompletionPercentage()
        {
            long totalTickCount = EndDate.Ticks - ShipDate.Ticks;
            long elapsedTicks = Simulation.simulation.Time.Ticks - ShipDate.Ticks;
            float currentPercentage = (elapsedTicks / (float)totalTickCount);
            return currentPercentage;
        }
    }

    public class Simulation
    {
        public static Simulation simulation;

        public List<ActiveJob> ActiveJobs { get; set; } = new List<ActiveJob>();

        public DateTime Time { get; set; }
        private const float minutesPerSecond = 15;
        private bool quarterHalfReached = false;

        public Simulation()
        {
            Time = DateTime.Now;
            simulation = this;
        }

        private EmployeeState GetEmployeeForNewJob(PlaceState place)
        {
            foreach(var employee in place.Employees)
            {
                if (employee.Job == JobTitle.Driver)
                    return employee; // We can add filters like rest time here later.
            }
            return null;
        }

        private void StopCompletedJobs()
        {
            for (int i = 0; i < ActiveJobs.Count; i++)
            {
                var job = ActiveJobs[i];
                if (job.GetCompletionPercentage() >= 1.0f)
                {
                    job.Employee.CurrentJob = null;
                    // TODO: If the final stop place is not owned, make driver go back to original location.
                    ActiveJobs.RemoveAt(i);
                    i--;
                }
            }
        }

        private void StartJob(PlaceState place, ScheduledJob job)
        {
            var employee = GetEmployeeForNewJob(place);
            if (employee != null)
            {
                var activeJob = new ActiveJob(job, Time, employee);
                employee.CurrentJob = activeJob;
                ActiveJobs.Add(activeJob);
            }
            else
            {
                // TODO: give error to player, a planned job could not be started because there are no available drivers!
            }
        }

        private void BeginPlannedJobs()
        {
            foreach (var place in WorldState.OwnedPlaces)
            {
                foreach (var dock in place.Docks)
                {
                    if (!dock.Unlocked) continue;
                    
                    foreach(var plannedJob in dock.Schedule.Jobs)
                    {
                        foreach(var shipTime in plannedJob.ShipTime)
                        {
                            if ((shipTime.Key == (Weekday)(Time.DayOfWeek-1) || shipTime.Key == Weekday.Sunday && Time.DayOfWeek == DayOfWeek.Sunday) 
                                && Time.Hour == shipTime.Value.Hours && Time.Minute == shipTime.Value.Minutes)
                            {
                                StartJob(place, plannedJob);
                            }
                        }
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            Time += TimeSpan.FromMinutes((gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f) * minutesPerSecond);

            if (Time.Minute % 15 == 0)
            {
                if (!quarterHalfReached)
                {
                    quarterHalfReached = true;
                    BeginPlannedJobs();
                }
            }
            else
            {
                quarterHalfReached = false;
            }
            StopCompletedJobs();
        }
    }
}
