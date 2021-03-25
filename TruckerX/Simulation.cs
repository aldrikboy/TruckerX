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

        public decimal Money = 0.0M; // In USD
        public DateTime Time { get; set; }
        private const float minutesPerSecond = 250;
        private bool quarterHalfReached = false;
        private bool salariesPaidOutThisMonth = false;

        public Simulation()
        {
            Time = DateTime.Now;
            simulation = this;
        }

        public DateTime GetNextLeavingTimeslot()
        {
            var minutesLeft = 15 - (Time.Minute % 15);
            return Time + TimeSpan.FromMinutes(minutesLeft);
        }

        private void StopCompletedJobs()
        {
            for (int i = 0; i < ActiveJobs.Count; i++)
            {
                var job = ActiveJobs[i];
                if (job.GetCompletionPercentage() >= 1.0f)
                {
                    Money += job.Job.Job.OfferedReward;
                    var finalDestination = job.Job.Job.To;
                    if (WorldState.PlaceOwned(finalDestination))
                    {
                        // Employee is now in the employee list of the destination place.
                        var finalPlace = WorldState.GetStateForPlace(job.Job.Job.To);
                        finalPlace.Employees.Add(job.Employee);
                        job.Employee.CurrentJob = null;
                        job.Employee.CurrentLocation = finalPlace.Place;
                    }
                    else
                    {
                        // If the final stop place is not owned, make driver go back to original location.
                        var scheduledJob = new ScheduledJob(job.Job.Job.Reverse(), job.Job.ShipTimes);
                        ActiveJob returnJob = new ActiveJob(scheduledJob, GetNextLeavingTimeslot(), job.Employee);
                        job.Employee.CurrentJob = returnJob;
                        ActiveJobs.Add(returnJob);
                    }

                    ActiveJobs.RemoveAt(i);
                    i--;
                }
            }
        }

        private void StartJob(PlaceState place, ShipTimeAssignment assignee, ScheduledJob job)
        {
            if (!place.Employees.Contains(assignee.AssignedEmployee))
            {
                // TODO: give error to player, employee is not at the correct location!
                //throw new Exception("Employee is not at correct location.");
                return;
            }
            var employee = assignee.AssignedEmployee;
            if (employee.CurrentJob == null)
            {
                employee.CurrentLocation = null;
                place.Employees.Remove(assignee.AssignedEmployee);
                var activeJob = new ActiveJob(job, Time - TimeSpan.FromMinutes(Time.Minute % 15), employee);
                employee.CurrentJob = activeJob;
                ActiveJobs.Add(activeJob);
            }
            else
            {
                // TODO: give error to player, employee is not available at the planned time!
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
                        foreach(var shipTime in plannedJob.ShipTimes)
                        {
                            if ((shipTime.Key == (Weekday)(Time.DayOfWeek-1) || shipTime.Key == Weekday.Sunday && Time.DayOfWeek == DayOfWeek.Sunday) 
                                && Time.Hour == shipTime.Value.Time.Hours && Time.Minute - (Time.Minute % 15) == shipTime.Value.Time.Minutes)
                            {
                                StartJob(place, shipTime.Value, plannedJob);
                            }
                        }
                    }
                }
            }
        }

        int prevMinutes = 0;
        public void Update(GameTime gameTime)
        {
            PayoutSalaries();
            Time += TimeSpan.FromMinutes((gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f) * minutesPerSecond);

            if (Time.Minute % 15 == 0 || (prevMinutes % 15) > (Time.Minute % 15))
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
            prevMinutes = Time.Minute;
        }

        private void PayoutSalaries()
        {
            if (Time.Day == 1)
            { 
                if (!salariesPaidOutThisMonth)
                {
                    salariesPaidOutThisMonth = true;
                    foreach(var place in WorldState.OwnedPlaces)
                    {
                        foreach(var employee in place.Employees)
                        {
                            Money -= employee.Salary;
                        }
                    }
                }
            }
            else
            {
                salariesPaidOutThisMonth = false;
            }
        }
    }
}
