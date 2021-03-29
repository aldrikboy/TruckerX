using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Trucks
{
    // https://www.0-60specs.com/5-most-popular-18-wheeler-semi-trucks/
    public class Truck
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public float LiterPer100Km { get; set; }

        public Truck(string name, decimal price, float literPer100Km)
        {
            Name = name;
            Price = price;
            LiterPer100Km = literPer100Km;
        }
    }
}
