/*
 * Utility class for calendar display.
 */ 

using System;

namespace Planner
{
    public static class CalendarUtility
    {
        public static string AddDateSuffix(int date)
        {
            switch (date)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static DayOfWeek FirstOfMonth(DateTime selectedDate)
        {
            DateTime getDay = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            return getDay.DayOfWeek;
        }

        public static int FirstDayOfMonthDisplay(DayOfWeek firstDayOfMonth)
        {
            int column = 0;

            switch (firstDayOfMonth)
            {
                case DayOfWeek.Monday:
                    column = 0;
                    break;
                case DayOfWeek.Tuesday:
                    column = 1;
                    break;
                case DayOfWeek.Wednesday:
                    column = 2;
                    break;
                case DayOfWeek.Thursday:
                    column = 3;
                    break;
                case DayOfWeek.Friday:
                    column = 4;
                    break;
                case DayOfWeek.Saturday:
                    column = 5;
                    break;
                case DayOfWeek.Sunday:
                    column = 6;
                    break;
            }

            return column;
        }

        public static int NumberOfDaysInMonth(DateTime selectedDate)
        {
            switch (selectedDate.Month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    return DateTime.IsLeapYear(selectedDate.Year) ? 29 : 28;
                default:
                    return 31;
            }
        }

        public static DateTime DateNextSunday(DateTime date)
        {
            switch (date.DayOfWeek)
            {       
                case DayOfWeek.Monday:
                    return date.AddDays(6);
                case DayOfWeek.Tuesday:
                    return date.AddDays(5);
                case DayOfWeek.Wednesday:
                    return date.AddDays(4);
                case DayOfWeek.Thursday:
                    return date.AddDays(3);
                case DayOfWeek.Friday:
                    return date.AddDays(2);
                case DayOfWeek.Saturday:
                    return date.AddDays(1);
                default:
                    return date;
            }
        }
    }
}
