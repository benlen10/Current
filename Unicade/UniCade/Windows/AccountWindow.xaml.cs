using System.Windows;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {

        #region Properties

        int _accountType;

        #endregion

        /// <summary>
        /// Public constructor for the AccountWindow class
        /// </summary>
        public AccountWindow(int accountType)
        {
            _accountType = accountType;
            InitializeComponent();
        }

        /// <summary>
        /// Close the current window instance
        /// </summary>
        private void AccountWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Create a new Unicade account with the given info
        /// </summary>
        private void AccountWindow_ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Check for invalid input
            if ((Textbox_Username.Text == null) || (Textbox_Email.Text == null) || (Textbox_Email.Text == null) || (Textbox_UserInfo.Text == null))
            {
                MessageBox.Show("Fields cannot be empty");
                return;
            }
            if ((Textbox_Username.Text.Length > 30) || (Textbox_Email.Text.Length > 30) || (Textbox_Email.Text.Length > 30) || (Textbox_UserInfo.Text.Length > 100))
            {
                MessageBox.Show("Invalid Length");
                return;
            }
            if (!Textbox_Email.Text.Contains("@"))
            {
                MessageBox.Show("Invalid Email");
                return;
            }

            //Create a new SQL user if the account type is UniCade Cloud
            if (_accountType == 0)
            {
                SQLclient.CreateUser(Textbox_Username.Text, Textbox_Email.Text, Textbox_Email.Text, Textbox_UserInfo.Text, "Null", "NullProfPath");
            }
            else
            {
                //Create a new local user if the account type standard Unicade
                User u = new User(Textbox_Username.Text, Textbox_Password.Text, 0, Textbox_Email.Text, 0, Textbox_UserInfo.Text, "Mature", "null");
                Database.UserList.Add(u);
                SettingsWindow._curUser = u;
            }
            Close();
        }
    }
}
