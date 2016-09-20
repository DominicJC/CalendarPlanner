/*
 * Class imports contacts using the data layer and populates a list.
 */
 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Planner.Objects;

namespace Planner
{
    internal class ContactsImporter
    {
        private List<Person> _contacts = new List<Person>();

        public List<Person> Contacts
        {
            get
            {
                return _contacts;
            }
        }

        public ContactsImporter()
        {
            GetContacts();
        }

        private void GetContacts()
        {
            try
            {
                _contacts = DataLayer.DataLayer.GetContacts();
                _contacts.Sort((x, y) => string.CompareOrdinal(x.LastName, y.LastName));
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error: " + e, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
