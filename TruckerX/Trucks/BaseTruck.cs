using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.State;

namespace TruckerX.Trucks
{
    // https://www.0-60specs.com/5-most-popular-18-wheeler-semi-trucks/
    public abstract class BaseTruck : ICloneable
    {
        public string Id { get; }
        public abstract string Name { get; }
        public abstract decimal Price { get; }
        public abstract float LiterPer100Km { get; }
        public abstract int HorsePower { get; }
        public EmployeeState Assignee { get; set; }

        public BaseTruck()
        {
            Id = "#" + String.Format("{0:000000}", WorldState.FreeId);
        }

        public object Clone()
        {
            var item = (BaseTruck)this.MemberwiseClone();
            item.Assignee = null;
            return item;
        }
    }
}
