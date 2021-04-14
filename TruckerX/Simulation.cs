using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Messaging;
using TruckerX.State;
using TruckerX.Trucks;

namespace TruckerX
{
    public class ActiveJob
    {
        public ScheduledJob Job { get; set; }
        public ShipTimeAssignment Assignment { get; set; }
        public BaseTruck UsedTruck { get; set; }
        public EmployeeState Employee { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime EndDate { get; set; }

        public ActiveJob(ScheduledJob job, ShipTimeAssignment assignment, DateTime shipTime, EmployeeState employee)
        {
            this.UsedTruck = assignment.AssignedEmployee.AssignedTruck;
            this.Job = job;
            this.Employee = employee;
            this.ShipDate = shipTime;
            this.Assignment = assignment;
            this.EndDate = ShipDate.AddHours(job.Job.GetTravelTime());
        }

        public Vector2 GetCurrentWorldLocation(float zoom)
        {
            float currentPercentage = GetCompletionPercentage();
            return Job.Job.ProgressPercentageToWorldLocation(currentPercentage, zoom);
        }

        public float GetCompletionPercentage()
        {
            long totalTickCount = EndDate.Ticks - ShipDate.Ticks;
            long elapsedTicks = Simulation.simulation.Time.Ticks - ShipDate.Ticks;
            float currentPercentage = (elapsedTicks / (float)totalTickCount);
            return currentPercentage;
        }

        public decimal GasPrice()
        {
            return (decimal)((Job.Job.GetDistanceInKm() / 100.0f) * UsedTruck.LiterPer100Km * Job.Job.From.Country.GasPricePerLiter);
        }
    }

    public class Simulation
    {
        public static Simulation simulation;

        public List<ActiveJob> ActiveJobs { get; set; } = new List<ActiveJob>();

        public decimal Money = 0.0M; // In USD
        public DateTime Time { get; set; }
        private const float minutesPerSecond = 450;
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
                    var finalPlace = WorldState.GetStateForPlace(job.Job.Job.To);

                    job.Job.Job.JobWasCompletedSuccesfully();

                    // Stay at location if owned + setting to stay there is enabled, or if it is the original location.
                    // else return to original location.
                    if (WorldState.PlaceOwned(finalDestination) && (job.Assignment.StayAtLocation || job.Employee.OriginalLocation == finalDestination))
                    {
                        ActiveJobs.RemoveAt(i);
                        i--;

                        // Employee is now in the employee list of the destination place.
                        job.Employee.CurrentJob = null;
                        finalPlace.AddEmployee(job.Employee);
                        continue;
                    }
                    else
                    {
                        ActiveJobs.RemoveAt(i);
                        i--;

                        // If the final stop place is not owned, make driver go back to original location.
                        var scheduledJob = new ScheduledJob(job.Job.Job.Reverse(), job.Job.ShipTimes);
                        ActiveJob returnJob = new ActiveJob(scheduledJob, job.Assignment, GetNextLeavingTimeslot(), job.Employee);
                        job.Employee.CurrentJob = returnJob;
                        ActiveJobs.Add(returnJob);

                        finalPlace.EmployeeStateChanged();
                        continue;
                    }
                }
            }
        }

        private void applyJobFactors(PlaceState place, JobOffer offer)
        {
            if (offer.TrustFactor < 1.0f)
            {
                foreach(var dock in place.Docks)
                {
                    var schedule = dock.Schedule;
                    for (int i = 0; i < schedule.Jobs.Count; i++)
                    {
                        var item = schedule.Jobs[i];
                        if (item.Job.Id == offer.Id)
                        {
                            MessageLog.AddError(string.Format("{0} has cancelled your contract to ship {1} from {2} to {3}",
                               item.Job.Company,
                               item.Job.Item.Name,
                               item.Job.From.Name,
                               item.Job.To.Name));

                            schedule.Jobs.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        private void StartJob(PlaceState place, ShipTimeAssignment assignee, ScheduledJob job)
        {
            if (!place.Employees.Contains(assignee.AssignedEmployee))
            {
                job.Job.JobWasCompletedUnsuccesfully();

                MessageLog.AddError(string.Format("{0} {1} missed their trip to {2} from {3} at {4}",
                    assignee.AssignedEmployee.Name,
                    assignee.AssignedEmployee.Id,
                    job.Job.To.Name, job.Job.From.Name, assignee.Time.ToString("h':'m''")));
                return;
            }

            var employee = assignee.AssignedEmployee;
            if (employee.AssignedTruck == null)
            {
                MessageLog.AddError(string.Format("{0} {1} does not have a truck to drive to {2} from {3} at {4}",
                    assignee.AssignedEmployee.Name,
                    assignee.AssignedEmployee.Id,
                    job.Job.To.Name, job.Job.From.Name, assignee.Time.ToString("h':'m''")));
                return;
            }
            
            if (employee.CurrentJob == null)
            {
                employee.CurrentLocation = null;
                place.Employees.Remove(assignee.AssignedEmployee);
                var activeJob = new ActiveJob(job, assignee, Time - TimeSpan.FromMinutes(Time.Minute % 15), employee);
                Money -= activeJob.GasPrice();

                employee.CurrentJob = activeJob;
                ActiveJobs.Add(activeJob);

                MessageLog.AddInfo(string.Format("{0} left {1}, driving to {2}", assignee.AssignedEmployee.Name, job.Job.From.Name, job.Job.To.Name));
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

                    for (int i = 0; i < dock.Schedule.Jobs.Count; i++)
                        applyJobFactors(place, dock.Schedule.Jobs[i].Job);
                }
            }
        }

        int prevMinutes = 0;
        public void Update(GameTime gameTime)
        {
            PayoutSalaries();
            UpdateHappyness();
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

        private void UpdateHappyness()
        {
            foreach(var place in WorldState.OwnedPlaces)
            {
                foreach(var employee in place.Employees)
                {
                    var placeRevenue = place.GetWeeklyRevenue() * 4;
                    var hoursPerWeek = place.GetWeeklyHours(employee);
                    employee.Happyness = 1.0f;

                    double hoursOverworked = hoursPerWeek - 70;
                    if (hoursOverworked > 0) employee.Happyness -= (float)hoursOverworked / 25.0f;

                    const int minPlaceRevenue = 50_000;
                    float percentageSalary = 1.0f;
                    if (placeRevenue > minPlaceRevenue)
                    {
                        float revStart = (float)placeRevenue - minPlaceRevenue;
                        int totalDiff = 500_000 - minPlaceRevenue;
                        percentageSalary = revStart / totalDiff;
                        if (percentageSalary > 1.0f) percentageSalary = 1.0f;

                        decimal expectedSalary = (decimal)((float)EmployeeState.BasePay + (((float)EmployeeState.MaxPay - (float)EmployeeState.BasePay) * percentageSalary));
                        if (employee.Salary < expectedSalary)
                        {
                            float underpayPercentage = (float)(expectedSalary - employee.Salary) / (float)(EmployeeState.MaxPay - EmployeeState.BasePay);
                            employee.Happyness -= underpayPercentage*2.0f;
                        }
                    }
                }
            }
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
