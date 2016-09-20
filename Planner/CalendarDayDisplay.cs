/*
 * Class displays the days of the month on the calendar with a label inside a group box. 
 * Called from the home page. Each instance represents a day.
 * Double clicking the label opens a new event form and subscribes to a calendar update event that is fired when 
 * the new event form closes. This then fires an event that is subscribed to by the homepage and the calendar is
 * updated from there.
 */ 

using System;

using System.Windows.Forms;
using System.Drawing;

namespace Planner
{
    internal class CalendarDayDisplay
    {
        public event UpdateCalendarHandler OnUpdateCalendarHandler;
        private GroupBox _dayOfTheMonthGroupBox;
        private Label _dayOfTheMonthLabel;
        private DateTime _selectedDate;

        public GroupBox AddDayOfTheMonthGroupBoxes(int date, DateTime currentlySelectedDate)
        {
            _dayOfTheMonthGroupBox = new GroupBox
            {
                Name = "box" + date,
                Text = date + CalendarUtility.AddDateSuffix(date),
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlDarkDark,
                Font = new Font("Segoe UI", 8.25F),
                Margin = new Padding(5)
            };
            _dayOfTheMonthGroupBox.Controls.Add(AddDayOfTheMonthLabels(_dayOfTheMonthGroupBox));
            _selectedDate = currentlySelectedDate;
            return _dayOfTheMonthGroupBox;
        }

        private Label AddDayOfTheMonthLabels(GroupBox box)
        {
            _dayOfTheMonthLabel = new Label
            {
                Name = "lbl" + box.Name,
                Location = new Point(10, 20),
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Control,
                Font = new Font("Segoe UI", 8.25F),
                TextAlign = ContentAlignment.BottomLeft
            };
            _dayOfTheMonthLabel.MouseDoubleClick += lab_MouseDoubleClick;
            return _dayOfTheMonthLabel;
        }

        private bool IsDateBeforeCurrentDate(int day)
        {
            DateTime current = DateTime.Now.AddDays(-1);
            DateTime compare = new DateTime(_selectedDate.Year, _selectedDate.Month, day);

            return compare < current;
        }

        private void UpdateCalendar()
        {
            if (OnUpdateCalendarHandler != null)
            {
                OnUpdateCalendarHandler();
            }
        }

        private void lab_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Label eventClick = (Label)sender;
            int day = int.Parse(eventClick.Name.Remove(0, 6));
            if (IsDateBeforeCurrentDate(day))
            {
                MessageBox.Show(@"You cannot set an event in the past.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                const bool newEvent = true;
                EventForm eventFormInstance = new EventForm(day, _selectedDate, newEvent);
                eventFormInstance.OnUpdateCalendarHandler += UpdateCalendar;
                eventFormInstance.Visible = true;
            }
        }
    }
}
