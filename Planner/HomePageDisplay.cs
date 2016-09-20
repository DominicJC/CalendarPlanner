/*
 * Class displays events & birthdays coming up in the next week on the homepage.
 */ 

using System;
using System.Collections.Generic;

using System.Windows.Forms;

using Planner.Objects;

namespace Planner
{
    internal class HomePageDisplay
    {
        private List<Event> _events;

        public void DisplayEventsOnHomePage(CalendarImporter calendarImportPage, DateTime date, Label[] homePageLabelArray , EventDisplay display)
        {
            calendarImportPage.GetHomePageEvents(date, date.AddDays(7));
            _events = calendarImportPage.HomePageEvents;

            foreach (var ev in _events)
            {
                int i = FirstDayOfEvent(ev.StartDate, date); 

                homePageLabelArray[i].Controls.Add(display.SetEventDisplay(ev));
                        
                if (ev.EndDate < (date.AddDays(6)))
                {
                    int span = ev.EndDate.DayOfYear - ev.StartDate.DayOfYear + 2;

                    for (int j = i + 1; j < ((i + span) -1); j++)
                    {
                         homePageLabelArray[j].Controls.Add(display.SetEventDisplay(ev));
                    }
                }
                else if (ev.EndDate >= (date.AddDays(6)))
                {
                    for (int j = i + 1; j < (homePageLabelArray.Length); j++)
                    {
                        homePageLabelArray[j].Controls.Add(display.SetEventDisplay(ev));
                    }
                }
            }
        }

        private static int FirstDayOfEvent(DateTime startDate, DateTime date)
        {
            int i = 0;

            if (startDate.Day == date.Day)
            {
                i = 0;
            }
            else if (startDate.Day == (date.Day + 1))
            {
                i = 1;
            }
            else if (startDate.Day == (date.Day + 2))
            {
                i = 2;
            }
            else if (startDate.Day == (date.Day + 3))
            {
                i = 3;
            }
            else if (startDate.Day == (date.Day + 4))
            {
                i = 4;
            }
            else if (startDate.Day == (date.Day + 5))
            {
                i = 5;
            }
            else if (startDate.Day == (date.Day + 6))
            {
                i = 6;
            }

            return i;
        }

        public void DisplayBirthdaysOnHomePage(List<Person> birthdays, DateTime date, Label[] homePageLabelArray)
        {
            foreach (var day in birthdays)
            {
                if (!day.Display) continue;
                if (day.Dob.Month != date.Month) continue;
                if (day.Dob.Day < date.Day || day.Dob.Day >= (date.Day + 7)) continue;
                int i = 0;

                if (day.Dob.Day == date.Day)
                {
                    i = 0;
                }
                else if (day.Dob.Day == (date.Day + 1))
                {
                    i = 1;
                }
                else if (day.Dob.Day == (date.Day + 2))
                {
                    i = 2;
                }
                else if (day.Dob.Day == (date.Day + 3))
                {
                    i = 3;
                }
                else if (day.Dob.Day == (date.Day + 4))
                {
                    i = 4;
                }
                else if (day.Dob.Day == (date.Day + 5))
                {
                    i = 5;
                }
                else if (day.Dob.Day == (date.Day + 6))
                {
                    i = 6;
                }


                homePageLabelArray[i].Text = @"Birthday: " + day.FirstName + @" " + day.LastName;
            }
        }
    }
}
