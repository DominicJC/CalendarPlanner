using System.Windows.Forms;

namespace Planner
{
    public partial class HelpPage : Form
    {
        public HelpPage()
        {
            InitializeComponent();
            txtHelp.Text = ("Home Page\r\n Displays events occurring in the coming week. Double click an event to open the event form for editing.\r\n\r\n");
            txtHelp.Text += ("Double click the shopping list to activate. Double click again to save and de-activate.\r\n\r\n***\r\n\r\n");
            txtHelp.Text += ("Calendar Page\r\n Displays events and birthdays. Double click a particular day to open a new event form.\r\n");
            txtHelp.Text += ("Double click an event to open the event form for editing. Use the forward and back arrows to cycle through months.\r\n\r\n***\r\n\r\n");
            txtHelp.Text += ("Contacts Page\r\n Displays contacts. Use buttons to open a new or edit contact form. Use the forward and back arrows to cycle through contacts.\r\n");
            ActiveControl = lblFocus;
        }
    }
}
