using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Planner.Objects;

namespace Planner
{
    internal class CalendarPageDisplay
    {
        public void AddDayOfMonthGroupBoxes(CalendarDayDisplay calendarDay, DateTime date, DateTime selectedDate, 
                            TableLayoutPanel pnlCalendarTable)
        {
            int days = CalendarUtility.NumberOfDaysInMonth(selectedDate);
            int column = CalendarUtility.FirstDayOfMonthDisplay(CalendarUtility.FirstOfMonth(selectedDate));
            int row = 0;

            for (int i = 1; i < (days + 1); i++, column++)
            {

                pnlCalendarTable.Controls.Add(calendarDay.AddDayOfTheMonthGroupBoxes(i, selectedDate), column, row);
                if (column != 8) continue;
                column = 1;
                row++;
            }

        }

        public void DisplayEventsInCalendar(List<Event> events, DateTime selectedDate, Label[] labelArray, EventDisplay display)
        {
            foreach (Event ev in events)
            {
                CalendarEventDisplay calendarEvent = new CalendarEventDisplay(ev, selectedDate);

                if ((calendarEvent.DoesEventStartThisYear() && !calendarEvent.DoesEventEndThisYear()) 
                    || (!calendarEvent.DoesEventStartThisYear() && calendarEvent.DoesEventEndThisYear()))
                {
                    MultiYearEvent(ev, calendarEvent, selectedDate, labelArray, display);
                }
                else if (calendarEvent.DoesEventStartThisYear() && calendarEvent.DoesEventEndThisYear())
                {
                    SingleYearEvent(ev, calendarEvent, selectedDate, labelArray, display);
                }
            }
        }

        public void MultiYearEvent(Event ev, CalendarEventDisplay calendarEvent, DateTime selectedDate, 
                                    Label[] labelArray, EventDisplay display)
        {
            if (calendarEvent.DoesEventStartThisYear())
            {
                if (calendarEvent.DoesEventStartThisMonth())
                {
                    for (int i = (ev.StartDate.Day - 1); i < labelArray.Length; i++)
                    {
                        labelArray[i].Controls.Add(display.SetEventDisplay(ev));
                    }
                }
                else
                {
                    foreach (Label t in labelArray)
                    {
                        t.Controls.Add(display.SetEventDisplay(ev));
                    }
                }
            }
            else if(calendarEvent.DoesEventEndThisYear())
            {
                if (calendarEvent.DoesEventEndThisMonth())
                {
                    for (int i = 0; i < (ev.EndDate.Day); i++)
                    {
                        labelArray[i].Controls.Add(display.SetEventDisplay(ev));
                    }
                }
                else
                {
                    foreach (Label t in labelArray)
                    {
                        t.Controls.Add(display.SetEventDisplay(ev));
                    }
                }
            }
        }

        public void SingleYearEvent(Event ev, CalendarEventDisplay calendarEvent, DateTime selectedDate, 
                                    Label[] labelArray, EventDisplay display)
        {
            if (calendarEvent.DoesEventStartThisMonth() && calendarEvent.DoesEventEndThisMonth())
            {
                if (calendarEvent.DoesEventStartAndEndToday())
                {
                    labelArray[ev.StartDate.Day - 1].Controls.Add(display.SetEventDisplay(ev));
                }
                else
                {
                    for (int i = (ev.StartDate.Day - 1); i < (ev.EndDate.Day); i++)
                    {
                        labelArray[i].Controls.Add(display.SetEventDisplay(ev));
                    }
                }
            }
            else if (calendarEvent.DoesEventStartThisMonth() && !calendarEvent.DoesEventEndThisMonth())
            {
                for (int i = (ev.StartDate.Day - 1); i < labelArray.Length; i++)
                {
                    labelArray[i].Controls.Add(display.SetEventDisplay(ev));
                }
            }
            else if (!calendarEvent.DoesEventStartThisMonth() && calendarEvent.DoesEventEndThisMonth())
            {
                for (int i = 0; i < (ev.EndDate.Day); i++)
                {
                    labelArray[i].Controls.Add(display.SetEventDisplay(ev));
                }
            }
            else if (calendarEvent.DoesEventLastTheEntireMonth())
            {
                foreach (Label t in labelArray)
                {
                    t.Controls.Add(display.SetEventDisplay(ev));
                }
            }
        }

        public void DisplayBirthdaysInCalendar(List<Person> birthdays, DateTime selectedDate, Label[] labelArray)
        {
            foreach (var day in birthdays.Where(day => day.Display).Where(day => day.Dob.Month == selectedDate.Month))
            {
                labelArray[day.Dob.Day - 1].Text += @"Birthday: " + day.FirstName 
                                                    + @" " + day.LastName + @" ";
            }
        }

        public void ChangeColourOfCurrentDay(DateTime date, DateTime selectedDate, Label[] calendarPageLabelArray, GroupBox[] calendarPageGroupBoxArray)
        {
            if (selectedDate.Month != date.Month || selectedDate.Year != date.Year) return;
            calendarPageLabelArray[date.Day - 1].BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            calendarPageGroupBoxArray[date.Day - 1].BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

    }
}
