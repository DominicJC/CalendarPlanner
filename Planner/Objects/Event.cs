using System;

namespace Planner.Objects
{
    public class Event
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string User { get; set; }
        public string Notes { get; set; }
    }
}
