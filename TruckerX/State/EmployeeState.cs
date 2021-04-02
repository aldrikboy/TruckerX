﻿using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.Trucks;

namespace TruckerX.State
{
    public enum Gender
    {
        Male,
        Female,
    }

    public enum JobTitle
    {
        Driver,

        // Add new jobs here

        Mechanic,
    }

    public class EmployeeState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public bool HasDiploma { get; set; }
        public decimal Salary { get; set; }
        public bool OnPaidLeave { get; set; }
        public JobTitle Job { get; set; }
        public ActiveJob CurrentJob { get; set; } = null;
        public BasePlace OriginalLocation { get; set; }
        public BasePlace CurrentLocation { get; set; } = null;
        public BaseTruck AssignedTruck { get; set; } = null;

        public void AssignTruck(BaseTruck truck)
        {
            AssignedTruck = truck;
            truck.Assignee = this;
        }

        public static EmployeeState GenerateNew(PlaceState place)
        {
            var rand = new Random();
            string[] names = { "John Johnson", "Jan Jansen", "Pete Peterson", "Joe Mama", "Hans Klok", "Mo Lester", "Mike Ockitch" };
            return new EmployeeState() {
                Id = "#" + String.Format("{0:000000}", WorldState.FreeId),
                Job = JobTitle.Driver,
                Name = names[rand.Next(0, names.Length)], 
                Age = rand.Next(18, 66), 
                Gender = (Gender)rand.Next(0, 2), 
                HasDiploma = true, 
                HireDate = DateTime.Now, 
                Salary = rand.Next(1600, 2500), 
                OnPaidLeave = false,
                OriginalLocation = place.Place,
                CurrentLocation = place.Place,
            };
        }
    }
}
