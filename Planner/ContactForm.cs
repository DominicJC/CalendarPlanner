using System;
using System.ComponentModel;
using System.Windows.Forms;

using Planner.Objects;

namespace Planner
{
    public delegate void UpdateContactHandler();

    public partial class ContactForm : Form
    {
        // New update contact event handler.
        public event UpdateContactHandler OnUpdateContactHandler;

        // New person instance
        public Person Person;

        // Input validation bool. If set to true don't allow ok button click.
        private bool _blockOkIfValidationFails;

        // Contacts Importer declaration.
        private ContactsImporter _contactsImportPage = new ContactsImporter();

        // Int represents position in contact list
        private int _contactListIndex;

        // Bool indicates new contact if true, existing contact if false.
        private bool _newContact;

        // Constructor with nil parameters.
        public ContactForm()
        {
            InitializeComponent();
            // Make sure user can't set date in the future.
            dtpDob.MaxDate = DateTime.Now;
            btnDelete.Enabled = false;
            btnDelete.Visible = false;
            Person = new Person();
            _newContact = true;
            FormClosing += ContactForm_FormClosing;
        }

        // Constructor with an integer parameter.
        public ContactForm(int contactListIndex)
        {
            InitializeComponent();
            // Make sure user can't set date in the future.
            dtpDob.MaxDate = DateTime.Now;
            Person = new Person();
            _newContact = false;
            _contactListIndex = contactListIndex;
            DisplayContact();
            FormClosing += ContactForm_FormClosing;
        }

        // Displays currently selected contact.
        private void DisplayContact()
        {
            if (_contactsImportPage.Contacts[_contactListIndex] == null) return;
            txtFirstName.Text = _contactsImportPage.Contacts[_contactListIndex].FirstName;
            txtLastName.Text = _contactsImportPage.Contacts[_contactListIndex].LastName;
            dtpDob.Value = _contactsImportPage.Contacts[_contactListIndex].Dob;
            chbDisplay.Checked = _contactsImportPage.Contacts[_contactListIndex].Display;
            txtAddress.Text = _contactsImportPage.Contacts[_contactListIndex].Address;
            txtSuburb.Text = _contactsImportPage.Contacts[_contactListIndex].Suburb;
            txtTown.Text = _contactsImportPage.Contacts[_contactListIndex].Town;
            txtCountry.Text = _contactsImportPage.Contacts[_contactListIndex].Country;
            txtPostCode.Text = _contactsImportPage.Contacts[_contactListIndex].Postcode;
            chbUser.Checked = _contactsImportPage.Contacts[_contactListIndex].IsUser;
        }

        // Fills person fields.
        private void NewPerson()
        {
            Person.FirstName = txtFirstName.Text;
            Person.LastName = txtLastName.Text;
            Person.Dob = dtpDob.Value.Date;
            Person.Display = chbDisplay.Checked;
            Person.Address = txtAddress.Text;
            Person.Suburb = txtSuburb.Text;
            Person.Town = txtTown.Text;
            Person.Country = txtCountry.Text;
            Person.Postcode = txtPostCode.Text;
            Person.IsUser = chbUser.Checked;
        }

        // Fills person fields for existing contact.
        private void ExistingPerson()
        {
            Person.Id = _contactsImportPage.Contacts[_contactListIndex].Id;
            NewPerson();
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

        // Validate that a first name has been entered.
        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                errorProvider.SetError(txtFirstName, "Please enter a first name");
                e.Cancel = _blockOkIfValidationFails;
            }

            else errorProvider.SetError(txtFirstName, "");
        }

        // Validate that a surname has been entered.
        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                errorProvider.SetError(txtLastName, "Please enter a surname");
                e.Cancel = _blockOkIfValidationFails;
            }

            else errorProvider.SetError(txtLastName, "");
        }

        // Validate that an address has been entered.
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                errorProvider.SetError(txtAddress, "Please enter an address");
                e.Cancel = _blockOkIfValidationFails;
            }

            else errorProvider.SetError(txtAddress, "");
        }

        // Validate that a suburb has been entered.
        private void txtSuburb_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSuburb.Text))
            {
                errorProvider.SetError(txtSuburb, "Please enter a suburb");
                e.Cancel = _blockOkIfValidationFails;
            }

            else errorProvider.SetError(txtSuburb, "");
        }

        // Validate that a town has been entered.
        private void txtTown_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTown.Text))
            {
                errorProvider.SetError(txtTown, "Please enter a town");
                e.Cancel = _blockOkIfValidationFails;
            }

            else errorProvider.SetError(txtTown, "");
        }

        // Validate that a country has been entered.
        private void txtCountry_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCountry.Text))
            {
                errorProvider.SetError(txtCountry, "Please enter a country");
                e.Cancel = _blockOkIfValidationFails;
            }

            else errorProvider.SetError(txtCountry, "");
        }

        // Raise update contact event.
        private void RaiseContactUpdate()
        {
            if (OnUpdateContactHandler != null)
            {
                OnUpdateContactHandler();
            }
        }

        // Don't allow validation to prevent form closing.
        private void ContactForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
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
                if (_newContact)
                {
                    if (!IsFormValid()) return;
                    NewPerson();
                    DataLayer.DataLayer.AddPerson(Person);
                    MessageBox.Show(@"Added contact: " + Person.FirstName, @"Added Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    if (!IsFormValid()) return;
                    ExistingPerson();
                    DataLayer.DataLayer.UpdatePerson(Person);
                    MessageBox.Show(@"Updated contact: " + Person.Id + @" " + Person.FirstName, @"Updated Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete selected contact.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(@"Are you sure you want to delete this contact?", @"Delete Contact", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result != DialogResult.OK) return;
                ExistingPerson();
                DataLayer.DataLayer.DeletePerson(Person);
                MessageBox.Show(@"Deleted contact: " + Person.Id + @" " + Person.FirstName + @" " + Person.LastName, @"Deleted Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Calls update contact event when form closes.
        private void ContactForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RaiseContactUpdate();
        }

    }
    
}
