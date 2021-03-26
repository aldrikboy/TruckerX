using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.State
{
    public class ShipTimeAssignment
    {
        public TimeSpan Time { get; set; }
        public EmployeeState AssignedEmployee { get; set; }

        public ShipTimeAssignment(TimeSpan time, EmployeeState assignedEmployee)
        {
            Time = time;
            AssignedEmployee = assignedEmployee;
        }
    }

    public class ScheduledJob
    {
        public JobOffer Job { get; set; }
        public Dictionary<Weekday, ShipTimeAssignment> ShipTimes { get; set; } = new Dictionary<Weekday, ShipTimeAssignment>();

        public ScheduledJob(JobOffer job)
        {
            Job = job;
        }

        public bool AllShipTimesHaveAssignees()
        {
            foreach(var item in ShipTimes)
            {
                if (item.Value.AssignedEmployee == null) return false;
            }
            return true;
        }

        public ScheduledJob(JobOffer job, Dictionary<Weekday, ShipTimeAssignment> shipTime) : this(job)
        {
            ShipTimes = shipTime;
            if (job.IsReturnDrive) return;
            foreach(var item in ShipTimes)
            {
                if (!job.ShipDays.Contains(item.Key)) throw new Exception("Shipday and time assigned to job that shouldn't ship on that day.");
            }
        }
    }

    public class PlaceSchedule
    {
        public List<ScheduledJob> Jobs { get; set; } = new List<ScheduledJob>();
        public int StartHour { get; } = 6;
        public int EndHour { get; } = 18;

        public bool CanScheduleJob(Weekday day, TimeSpan time, JobOffer toReplace = null)
        {
            foreach(var item in Jobs)
            {
                if (toReplace != null && item.Job.Id == toReplace.Id) continue;
                foreach(var plannedTime in item.ShipTimes)
                {
                    if (plannedTime.Key == day && plannedTime.Value.Time == time)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void RemoveJobById(int id)
        {
            foreach(var job in Jobs)
            {
                if (job.Job.Id == id)
                {
                    Jobs.Remove(job);
                    return;
                }
            }
        }
    }
}
