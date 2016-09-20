using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Planner.Objects;

using System.Threading.Tasks;

namespace Planner
{
    public delegate void UpdateCalendarHandler();

    public partial class EventForm : Form
    {
        public event UpdateCalendarHandler OnUpdateCalendarHandler;

        // Lists to bind to the forms combo boxes.
        private readonly List<string> _eventTypes = new List<string> { "Holiday", "Appointment", "Event", "Other" };
        private Dictionary<int, string> _user;

        // New event instance
        private Event _events;

        // New edit event instance.
        private Event _editEvent = new Event();
               
        // Input validation bool. If set to true don't allow ok button click.
        private bool _blockOkIfValidationFails;

        // New event bool. True for new events, false for edit existing event.
        private bool _newRecord;

        // Constructor with nil parameters.
        public EventForm()
        {
            
        }

        // Constructor with one parameter.
        public EventForm(object sender)
        {
            Setup();
            PopulateFormExisting(sender);
        }

        // Constructor with three parameters.
        public EventForm(int day, DateTime selectedDate, bool newEvent)
        {
            Setup();
            PopulateFormNew(day, selectedDate);
        }

        // Performs initial page setup.
        private void Setup()
        {
            InitializeComponent();
            _user = DataLayer.DataLayer.GetUser();
            PopulateCombos();
            // Make sure user can't set date in the past.
            dtpDateFrom.MinDate = DateTime.Now.AddDays(-1);
            dtpDateTo.MinDate = DateTime.Now.AddDays(-1);
            _events = new Event();
            FormClosing += EventForm_FormClosing;
        }

        // Populate combo boxes with lists.
        private void PopulateCombos()
        {
            cmbEventType.DataSource = _eventTypes;
            cmbEventType.SelectedIndex = 0;

            cmbPerson.DataSource = new BindingSource(_user, null);
            cmbPerson.SelectedIndex = 0;
        }

        // Populates form when looking at existing event.
        private void PopulateFormExisting(object sender)
        {
            _newRecord = false;
            btnDelete.Enabled = true;
            btnDelete.Visible = true;
            int id;
            Label editEventLabel = (Label)sender;
            if (int.TryParse(editEventLabel.Name, out id))
            {
                id = int.Parse(editEventLabel.Name);
                CalendarImporter calImport = new CalendarImporter();
                _editEvent = calImport.GetEvent(id);
                cmbEventType.Text = _editEvent.EventType;
                txtEvent.Text = _editEvent.EventName;
                dtpDateFrom.MinDate = _editEvent.StartDate.AddYears(-1);
                dtpDateFrom.Value = _editEvent.StartDate;
                dtpDateTo.MinDate = _editEvent.EndDate.AddYears(-1);
                dtpDateTo.Value = _editEvent.EndDate;
                if (!string.IsNullOrEmpty(_editEvent.Notes))
                {
                    txtNotes.Text = _editEvent.Notes;
                }
            }
            
        }

        // Populates form when loading a new event.
        private void PopulateFormNew(int day, DateTime selectedDate)
        {
            _newRecord = true;
            btnDelete.Enabled = false;
            btnDelete.Visible = false;
            dtpDateFrom.Value = new DateTime(selectedDate.Year, selectedDate.Month, day);
            dtpDateTo.Value = new DateTime(selectedDate.Year, selectedDate.Month, day);
        }

        // Updates existing event with new details
        private void EditedEventMethod()
        {
            _editEvent.EventType = cmbEventType.SelectedValue.ToString();
            _editEvent.EventName = txtEvent.Text;
            _editEvent.StartDate = dtpDateFrom.Value.Date;
            _editEvent.EndDate = dtpDateTo.Value.Date;
            _editEvent.User = cmbPerson.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(txtNotes.Text))
            {
                _editEvent.Notes = txtNotes.Text;
            }
        }

        // Fills event fields.
        private void NewEvent()
        {
            _events.EventType = cmbEventType.SelectedValue.ToString();
            _events.EventName = txtEvent.Text;
            _events.StartDate = dtpDateFrom.Value.Date;
            _events.EndDate = dtpDateTo.Value.Date;
            _events.User = cmbPerson.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(txtNotes.Text))
            {
                _events.Notes = txtNotes.Text;
            }
        }

        // Checks all fields are valid.
        private bool IsFormValid()
        {
            try
            {
                _blockOkIfValidationFails = true;

                foreach (Control control in Controls)
                {
                    control.Focus();

                    if (!Validate())
                    {
                        return false;
                    }
                }
            }
            finally
            {
                _blockOkIfValidationFails = false;
                Validate();
            }

            return true;
        }

        // Validate that the event ends after it starts.
        private void dtpDateTo_Validating(object sender, CancelEventArgs e)
        {
            if (dtpDateTo.Value.Date < dtpDateFrom.Value.Date)
            {
                errorProvider.SetError(dtpDateTo, "End date must not be before the start date.");
                e.Cancel = _blockOkIfValidationFails;
            }
            else errorProvider.SetError(dtpDateTo, "");
        }
        
        // Validate that a event name has been entered.
        private void txtEvent_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEvent.Text))
            {
                errorProvider.SetError(txtEvent, "Please enter an event name");
                e.Cancel = _blockOkIfValidationFails;
            }
            else errorProvider.SetError(txtEvent, "");
        }

        // Don't allow validation to prevent form closing.
        private void EventForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;          
        }

        private void RaiseCalendarUpdate()
        {
            if (OnUpdateCalendarHandler != null)
            {
                OnUpdateCalendarHandler();
            }
        }

        // Cancel form.
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(@"Are you sure you want to cancel?", @"Cancel Event", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                Close();
            }
        }

        // Final validation and submit form.
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (_newRecord)
                {
                    if (!IsFormValid()) return;
                    NewEvent();
                    DataLayer.DataLayer.AddEvent(_events);
                    MessageBox.Show(@"Added event: " + _events.EventName, @"Added Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    if (!IsFormValid()) return;
                    EditedEventMethod();
                    DataLayer.DataLayer.UpdateEvent(_editEvent);
                    MessageBox.Show(@"Updated event: " + _editEvent.Id + @" " + _editEvent.EventType + @" " + _editEvent.EventName, @"Updated Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete selected record.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(@"Are you sure you want to delete this event?", @"Delete Record", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result != DialogResult.OK) return;
                DataLayer.DataLayer.DeleteEvent(_editEvent);
                MessageBox.Show(@"Deleted Event: " + _editEvent.Id + @" " + _editEvent.EventType, @"Deleted Event", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EventForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RaiseCalendarUpdate();
        }

    }
}
