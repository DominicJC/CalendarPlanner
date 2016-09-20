/*
 * Class provides boolean checks for displaying events in the calendar.
 */ 

using System;

using Planner.Objects;

namespace Planner
{
    internal class CalendarEventDisplay
    {
        private Event _events;
        private DateTime _selectedDate;

        public CalendarEventDisplay(Event events, DateTime selectedDate)
        {
            _events = events;
            _selectedDate = selectedDate;
        }

        public bool DoesEventStartAndEndToday()
        {
            return _events.StartDate.Day == _events.EndDate.Day;
        }

        public bool DoesEventStartThisMonth()
        {
            return _events.StartDate.Month == _selectedDate.Month;
        }

        public bool DoesEventEndThisMonth()
        {
            return _events.EndDate.Month == _selectedDate.Month;
        }

        public bool DoesEventStartThisYear()
        {
            return _events.StartDate.Year == _selectedDate.Year;
        }

        public bool DoesEventEndThisYear()
        {
            return _events.EndDate.Year == _selectedDate.Year;
        }

        public bool DoesEventLastTheEntireMonth()
        {
            return (_events.StartDate.DayOfYear < _selectedDate.DayOfYear) && (_events.EndDate.DayOfYear > _selectedDate.DayOfYear);
        }
    }
}
