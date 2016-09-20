/*
 * Class displays the event as a label. Called from the CalendarPageDisplay class.
 * When label is double clicked a new event form is called and event data passed to the form for editing.
 * This class subscribes to a calendar update event that is fired when the event form closes. This then
 * fires an event that is subscribed to by the homepage and the calendar is updated from there.
 */

using System.Drawing;
using System.Windows.Forms;
using Planner.Objects;

namespace Planner
{
    internal class EventDisplay
    {
        public event UpdateCalendarHandler OnUpdateCalendarHandler;

        public Label eventDisplay { get; set; }

        public Label SetEventDisplay(Event currentEvent)
        {
            eventDisplay = new Label
            {
                Name = currentEvent.Id.ToString(),
                AutoSize = false,
                Size = new Size(150, 20),
                BackColor = SetColour(currentEvent.EventType),
                Text = currentEvent.EventType + @": " + currentEvent.EventName,
                TextAlign = ContentAlignment.TopCenter
            };
            eventDisplay.MouseDoubleClick += eventDisplay_MouseDoubleClick;
            
            return eventDisplay;
        }

        public Color SetColour(string eventType)
        {
            Color colour;

            switch (eventType)
            {
                case "Holiday":
                    colour = Color.FromArgb(120, 240, 140);//Green
                    break;
                case "Appointment":
                    colour = Color.FromArgb(240, 120, 120);//Red
                    break;
                case "Event":
                    colour = Color.FromArgb(220, 150, 250);//Purple
                    break;
                default:
                    colour = Color.FromArgb(240, 200, 140);//Orange
                    break;
            }

            return colour;
        }

        private void UpdateCalendar()
        {
            if (OnUpdateCalendarHandler != null)
            {
                OnUpdateCalendarHandler();
            }
        }

        private void eventDisplay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EventForm eventFormInstance = new EventForm(sender);
            eventFormInstance.OnUpdateCalendarHandler += UpdateCalendar;
            eventFormInstance.Visible = true;
        }
    }
}
