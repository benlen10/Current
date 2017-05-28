using System.Windows;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region Properties

        /// <summary>
        /// An int value that represents the current user type (local or cloud)
        /// </summary>
        int UserType;

        #endregion

        #region Constructors

        /// <summary>
        /// Public constructor for the LoginWindow instance
        /// </summary>
        public LoginWindow(int userType)
        {
            UserType = userType;
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Close the current window instance
        /// </summary>
        private void LoginWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Attempt to authenticate user against the database credentials
        /// </summary>
        private void LoginWindow_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Bad input checks
            if ((Textbox_Username.Text == null) || (Textbox_Password.Text == null))
            {
                MessageBox.Show("Fields cannot be blank");
                return;
            }

            //If the user is a SQL client, preform SQL user authentication 
            if (UserType == 0)
            {
                if (SQLclient.AuthiencateUser(Textbox_Username.Text, Textbox_Password.Text))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show(this, "Incorrect login details");
                }
            }

            //If the user is a local account, validate the info and close the window if sucuessful 
            else
            {
                foreach (IUser u in Program.Database.UserList)
                {
                    if (u.Username.Equals(Textbox_Username.Text))
                    {
                        if (u.GetUserPassword().Equals(Textbox_Password.Text))
                        {
                            SettingsWindow.CurrentUser = u;
                            Close();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(this, "Incorrect Password");
                            return;
                        }
                    }
                }
                MessageBox.Show(this, "User does not exist");
                return;
            }
        }

        #endregion

    }
}
