using System;
using System.Collections.Generic;
using System.Text;

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
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public bool HasDiploma { get; set; }
        public decimal Salary { get; set; }
        public bool OnPaidLeave { get; set; }
        public JobTitle Job { get; set; }

        public static EmployeeState GenerateNew()
        {
            var rand = new Random();
            string[] names = { "John Johnson", "Jan Jansen", "Pete Peterson", "Joe Mama", "Hans Klok", "Mo Lester", "Mike Ockitch" };
            return new EmployeeState() {
                Job = (JobTitle)rand.Next((int)JobTitle.Driver, (int)JobTitle.Mechanic+1),
                Name = names[rand.Next(0, names.Length)], 
                Age = rand.Next(18, 66), 
                Gender = (Gender)rand.Next(0, 2), 
                HasDiploma = true, 
                HireDate = DateTime.Now, 
                Salary = rand.Next(1600, 2500), 
                OnPaidLeave = false 
            };
        }
    }
}
