using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.State
{
    public class ScheduledJob
    {
        public JobOffer Job { get; set; }
        public Weekday Day { get; set; }
        public TimeSpan ShipTime { get; set; }
        public EmployeeState Employee { get; set; }
    }

    public class PlaceSchedule
    {
        public List<ScheduledJob> Jobs { get; set; }
    }
}
