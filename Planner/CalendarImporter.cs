/*
 * Class imports calendar data using the data layer.
 */ 

using System;
using System.Collections.Generic;

using System.Windows.Forms;
using Planner.Objects;
using System.Threading.Tasks;

namespace Planner
{
    internal class CalendarImporter
    {
        private List<Person> _birthdays = new List<Person>();
        private List<Event> _events = new List<Event>();
        private List<Event> _homePageEvents = new List<Event>();

        public List<Person> Birthdays
        {
            get
            {
                return _birthdays;
            }
        }

        public List<Event> Events
        {
            get
            {
                return _events;
            }
        }

        public List<Event> HomePageEvents
        {
            get
            {
                return _homePageEvents;
            }
        }

        public CalendarImporter()
        {
            GetBirthdays();
            GetEvents();
        }

        public CalendarImporter(int id)
        {

        }

         public void GetBirthdays()
        {
            try
            {
                _birthdays = DataLayer.DataLayer.GetBirthdays();
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error: " + e, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void GetEvents()
        {
            try
            {
                _events = DataLayer.DataLayer.GetEvents();
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error: " + e, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public Event GetEvent(int id)
        {
            try
            {
                return DataLayer.DataLayer.GetEvent(id);
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error: " + e, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        public void GetHomePageEvents(DateTime start, DateTime end)
        {
            try
            {
                _homePageEvents = DataLayer.DataLayer.GetHomePageEvents(start, end);
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error: " + e, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
