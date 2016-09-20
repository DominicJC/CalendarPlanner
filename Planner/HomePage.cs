/*
 * Main page for application.
 * On initial startup birthday & event lists are retrieved from the database and displayed on the home and 
 * calendar pages.
 * A list of contacts is retrieved from the database and the first contact is displayed on the contact page.
 * The current shopping list is retrieved from file and displayed on the home page. If none exists then one
 * is created.
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using Planner.Objects;

namespace Planner
{
    public partial class HomePage : Form
    {
        // Date variables.
        private static DateTime _date = DateTime.Now;
        private DateTime _selectedDate = _date;

        // Initialise the importer classes.
        private CalendarImporter _calendarImportPage = new CalendarImporter();
        private ContactsImporter _contactsImportPage;

        // Initialise the various display pages/classes.
        private HomePageDisplay _homePageDisplayInstance = new HomePageDisplay();
        private CalendarPageDisplay _calendarPageDisplayInstance = new CalendarPageDisplay();
        private EventDisplay _eventDisplayInstance = new EventDisplay();
        private CalendarDayDisplay _calendarDayDisplayInstance = new CalendarDayDisplay();
        private ShoppingListDisplay _shoppingListDisplayInstance;

        // Lists of birthdays & events.
        private List<Person> _birthdays;
        private List<Event> _events;

        // Menu selection handles. Keeps track of currently selected menu.
        private bool _menuOne = true;
        private bool _menuTwo;
        private bool _menuThree;
        //bool menuFour = false;
        private bool _menuFive;

        // Initialise arrays for linking to controls on the home and calendar pages.
        private Label[] _homePageLabelArray = new Label[7];
        private Label[] _calendarPageLabelArray;
        private GroupBox[] _calendarPageGroupBoxArray;

        // Integer for selected contact.
        private int _selectedContactIndex;

        public HomePage()
        {
            Thread splash = new Thread(StartSplash) {IsBackground = true};
            splash.Start();
            InitializeComponent();
            Visible = false;
            lblDate.Text = _date.ToString("dddd d MMMM yyyy");
            PageSetup();
            _homePageDisplayInstance.DisplayEventsOnHomePage(_calendarImportPage, _date, _homePageLabelArray, _eventDisplayInstance);
            _homePageDisplayInstance.DisplayBirthdaysOnHomePage(_birthdays, _date, _homePageLabelArray);
            _calendarDayDisplayInstance.OnUpdateCalendarHandler += UpdateEventsOnCalendarAndHomePages;
            _eventDisplayInstance.OnUpdateCalendarHandler += UpdateEventsOnCalendarAndHomePages;
            Thread.Sleep(2000);
            Visible = true;
            splash.Abort();
        }

        private static void StartSplash()
        {
            SplashScreen splashScreen = new SplashScreen();
            Application.Run(splashScreen);
        }

        private void PageSetup()
        {
            lblMenu1.BackColor = System.Drawing.SystemColors.InactiveCaption;            
            DisplayContacts();
            AttachHomePageLabelsToArray();
            ShowDateOnHomePageGroupBoxes();
            SetCalendarPage();
            GetExistingShoppingListFromFile();
            ActiveControl = lblDayOne;
        }

        private void AttachHomePageLabelsToArray()
        {
            _homePageLabelArray[0] = lblDayOne;
            _homePageLabelArray[1] = lblDayTwo;
            _homePageLabelArray[2] = lblDayThree;
            _homePageLabelArray[3] = lblDayFour;
            _homePageLabelArray[4] = lblDayFive;
            _homePageLabelArray[5] = lblDaySix;
            _homePageLabelArray[6] = lblDaySeven;
        }

        private void ShowDateOnHomePageGroupBoxes()
        {
            grbDayOne.Text = _date.ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.Day);
            grbDayTwo.Text = _date.AddDays(1).ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.AddDays(1).Day);
            grbDayThree.Text = _date.AddDays(2).ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.AddDays(2).Day);
            grbDayFour.Text = _date.AddDays(3).ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.AddDays(3).Day);
            grbDayFive.Text = _date.AddDays(4).ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.AddDays(4).Day);
            grbDaySix.Text = _date.AddDays(5).ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.AddDays(5).Day);
            grbDaySeven.Text = _date.AddDays(6).ToString("dddd d") + CalendarUtility.AddDateSuffix(_date.AddDays(6).Day);
        }

        private void SetCalendarPage()
        {
            lblSelectedMonth.Text = _selectedDate.ToString("MMMM yyyy");
            _events = _calendarImportPage.Events;
            _birthdays = _calendarImportPage.Birthdays;
            _calendarPageDisplayInstance.AddDayOfMonthGroupBoxes(_calendarDayDisplayInstance, _date, _selectedDate, pnlCalendarTable);
            AttachCalendarPageLabelsAndGroupBoxesToArrays();
            _calendarPageDisplayInstance.ChangeColourOfCurrentDay(_date, _selectedDate, _calendarPageLabelArray, _calendarPageGroupBoxArray);
            _calendarPageDisplayInstance.DisplayBirthdaysInCalendar(_birthdays, _selectedDate, _calendarPageLabelArray);
            _calendarPageDisplayInstance.DisplayEventsInCalendar(_events, _selectedDate, _calendarPageLabelArray, _eventDisplayInstance);
        }

        private void UpdateEventsOnCalendarAndHomePages()
        {
            _calendarImportPage.GetEvents();
            _events = _calendarImportPage.Events;
            pnlCalendarTable.Controls.Clear();
            foreach(Label lab in _homePageLabelArray)
            {
                lab.Controls.Clear();
            }
            SetCalendarPage();
            _homePageDisplayInstance.DisplayEventsOnHomePage(_calendarImportPage, _date, _homePageLabelArray, _eventDisplayInstance);
        }

        private void AttachCalendarPageLabelsAndGroupBoxesToArrays()
        {
            _calendarPageLabelArray = new Label[CalendarUtility.NumberOfDaysInMonth(_selectedDate)];
            _calendarPageGroupBoxArray = new GroupBox[CalendarUtility.NumberOfDaysInMonth(_selectedDate)];
            int i = 0;
            int j = 0;
            foreach (GroupBox item in pnlCalendarTable.Controls.OfType<GroupBox>())
            {
                _calendarPageGroupBoxArray[j] = item;
                j++;

                foreach (Label label in (item).Controls.OfType<Label>())
                {
                    _calendarPageLabelArray[i] = label;
                    i++;
                }
            }
        }

        private void DisplayContacts()
        {
            try
            {
                _contactsImportPage = new ContactsImporter();
                
                if (_contactsImportPage.Contacts[_selectedContactIndex] == null) return;
                lblContactName.Text = _contactsImportPage.Contacts[_selectedContactIndex].FirstName + @" " + _contactsImportPage.Contacts[_selectedContactIndex].LastName;
                lblFirstName.Text = _contactsImportPage.Contacts[_selectedContactIndex].FirstName;
                lblLastName.Text = _contactsImportPage.Contacts[_selectedContactIndex].LastName;
                lblDoB.Text = _contactsImportPage.Contacts[_selectedContactIndex].Dob.ToString("dd MMMM yyyy");
                lblAddress.Text = _contactsImportPage.Contacts[_selectedContactIndex].Address;
                lblSuburb.Text = _contactsImportPage.Contacts[_selectedContactIndex].Suburb;
                lblTown.Text = _contactsImportPage.Contacts[_selectedContactIndex].Town;
                lblCountry.Text = _contactsImportPage.Contacts[_selectedContactIndex].Country;
                lblPostCode.Text = _contactsImportPage.Contacts[_selectedContactIndex].Postcode;
            }
            catch(ArgumentOutOfRangeException aor)
            {
                MessageBox.Show(aor.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableShoppingListTextbox()
        {
            txtShopping.BackColor = System.Drawing.SystemColors.Control;
            txtShopping.ReadOnly = false;
            txtShopping.ScrollBars = ScrollBars.Vertical;         
        }

        private void SaveShoppingListAndDisableTextbox()
        {
            txtShopping.BackColor = System.Drawing.SystemColors.InactiveCaption;
            txtShopping.ReadOnly = true;
            txtShopping.ScrollBars = ScrollBars.None;
            ActiveControl = lblDayOne;
        }

        private void NewShoppingList()
        {
            _shoppingListDisplayInstance = new ShoppingListDisplay(CalendarUtility.DateNextSunday(_date), txtShopping.Text);
        }

        private void GetExistingShoppingListFromFile()
        {
            _shoppingListDisplayInstance = new ShoppingListDisplay(CalendarUtility.DateNextSunday(_date));
            if (_shoppingListDisplayInstance.ShoppingListObject != null)
            {
                txtShopping.Text = _shoppingListDisplayInstance.ShoppingListObject.shoppingList;
                grbShopping.Text = @"Shopping List For" + CalendarUtility.DateNextSunday(_date).ToString(" d") + CalendarUtility.AddDateSuffix(CalendarUtility.DateNextSunday(_date).Day) + CalendarUtility.DateNextSunday(_date).ToString(" MMMM");
            }
            else
            {
                grbShopping.Text = @"Shopping List For" + CalendarUtility.DateNextSunday(_date).ToString(" d") + CalendarUtility.AddDateSuffix(CalendarUtility.DateNextSunday(_date).Day) + CalendarUtility.DateNextSunday(_date).ToString(" MMMM");
            }
        }

        private void lblMenu1_MouseEnter(object sender, EventArgs e)
        {
            if (_menuOne)
            {
                return;
            }
            lblMenu1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

        private void lblMenu1_MouseLeave(object sender, EventArgs e)
        {
            if (_menuOne)
            {
                return;
            }
            lblMenu1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
        }

        private void lblMenu2_MouseEnter(object sender, EventArgs e)
        {
            if (_menuTwo)
            {
                return;
            }
            lblMenu2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

        private void lblMenu2_MouseLeave(object sender, EventArgs e)
        {
            if (_menuTwo)
            {
                return;
            }
            lblMenu2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
        }

        private void lblMenu3_MouseEnter(object sender, EventArgs e)
        {
            if (_menuThree)
            {
                return;
            }
            lblMenu3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

        private void lblMenu3_MouseLeave(object sender, EventArgs e)
        {
            if (_menuThree)
            {
                return;
            }
            lblMenu3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
        }

        /*
        private void lblMenu4_MouseEnter(object sender, EventArgs e)
        {
            if (menuFour)
            {
                return;
            }
            lblMenu4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

        private void lblMenu4_MouseLeave(object sender, EventArgs e)
        {
            if (menuFour)
            {
                return;
            }
            lblMenu4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
        }

            */

        private void lblMenu5_MouseEnter(object sender, EventArgs e)
        {
            if (_menuFive)
            {
                return;
            }
            lblMenu5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

        private void lblMenu5_MouseLeave(object sender, EventArgs e)
        {
            if (_menuFive)
            {
                return;
            }
            lblMenu5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
        }

        private void lblMenu1_Click(object sender, EventArgs e)
        {
            if (_menuOne)
            {
                return;
            }
            lblMenu1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            lblMenu2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            //lblMenu4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;

            _menuOne = true;
            _menuTwo = false;
            _menuThree = false;
            //menuFour = false;
            _menuFive = false;

            pnlHome.Visible = true;
            pnlCalendar.Visible = false;
            pnlContacts.Visible = false;
            ActiveControl = lblDayOne;
        }

        private void lblMenu2_Click(object sender, EventArgs e)
        {
            if (_menuTwo)
            {
                return;
            }
            lblMenu1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            lblMenu3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            //lblMenu4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;

            _menuOne = false;
            _menuTwo = true;
            _menuThree = false;
            //menuFour = false;
            _menuFive = false;

            pnlHome.Visible = false;
            pnlCalendar.Visible = true;
            pnlContacts.Visible = false;
        }

        private void lblMenu3_Click(object sender, EventArgs e)
        {
            if (_menuThree)
            {
                return;
            }
            lblMenu1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu3.BackColor = System.Drawing.SystemColors.InactiveCaption;
            //lblMenu4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            lblMenu5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;

            _menuOne = false;
            _menuTwo = false;
            _menuThree = true;
            //menuFour = false;
            _menuFive = false;

            pnlHome.Visible = false;
            pnlCalendar.Visible = false;
            pnlContacts.Visible = true;
        }

        /*
        private void lblMenu4_Click(object sender, EventArgs e)
        {
            if (menuFour)
            {
                return;
            }
            else
            {
                lblMenu1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                lblMenu2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                lblMenu3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                lblMenu4.BackColor = System.Drawing.SystemColors.InactiveCaption;
                lblMenu5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;

                menuOne = false;
                menuTwo = false;
                menuThree = false;
                menuFour = true;
                menuFive = false;

                pnlHome.Visible = false;
                pnlCalendar.Visible = false;
                pnlContacts.Visible = false;
            }
        }

            */

        private void lblMenu5_Click(object sender, EventArgs e)
        {
            new HelpPage {Visible = true};
        }

        private void lblMonthForward_Click(object sender, EventArgs e)
        {
            pnlCalendarTable.Controls.Clear();
            _selectedDate = _selectedDate.AddMonths(1);
            SetCalendarPage();
        }

        private void lblMonthBack_Click(object sender, EventArgs e)
        {
            pnlCalendarTable.Controls.Clear();
            _selectedDate = _selectedDate.AddMonths(-1);
            SetCalendarPage();
        }

        private void lblNextContact_Click(object sender, EventArgs e)
        {
            if (_selectedContactIndex < _contactsImportPage.Contacts.Count - 1)
            {
                _selectedContactIndex = _selectedContactIndex + 1;
                DisplayContacts();
            }
        }

        private void lblLastContact_Click(object sender, EventArgs e)
        {
            if (_selectedContactIndex > 0)
            {
                _selectedContactIndex = _selectedContactIndex - 1;
                DisplayContacts();
            }
        }

        private void txtShopping_DoubleClick(object sender, EventArgs e)
        {
            if (txtShopping.ReadOnly)
            {
                EnableShoppingListTextbox();
            }
            else
            {
                SaveShoppingListAndDisableTextbox();
                NewShoppingList();
            }
        }

        private void lblNew_Click(object sender, EventArgs e)
        {
            ContactForm contact = new ContactForm();
            contact.OnUpdateContactHandler += DisplayContacts;
            contact.Visible = true;
        }

        private void lblEdit_Click(object sender, EventArgs e)
        {
            ContactForm contact = new ContactForm(_selectedContactIndex);
            contact.OnUpdateContactHandler += DisplayContacts;
            contact.Visible = true;
        }

    }
}
