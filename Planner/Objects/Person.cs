using System;


namespace Planner.Objects
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public bool Display { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string Notes { get; set; }
        public bool IsUser { get; set; }
    }
}
