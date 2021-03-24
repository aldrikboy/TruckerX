using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.State
{
    public class ScheduledJob
    {
        public JobOffer Job { get; set; }
        public Dictionary<Weekday,TimeSpan> ShipTime { get; set; } = new Dictionary<Weekday, TimeSpan>();

        public ScheduledJob(JobOffer job)
        {
            Job = job;
        }

        public ScheduledJob(JobOffer job, Dictionary<Weekday, TimeSpan> shipTime) : this(job)
        {
            ShipTime = shipTime;
            foreach(var item in ShipTime)
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

        public bool CanScheduleJob(Weekday day, TimeSpan time)
        {
            foreach(var item in Jobs)
            {
                foreach(var plannedTime in item.ShipTime)
                {
                    if (plannedTime.Key == day && plannedTime.Value == time)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
