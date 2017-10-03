using System.Windows;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Network;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        #region Private Instance Variables

        /// <summary>
        /// An int value that represents the current user type (local or cloud)
        /// </summary>
        readonly Enums.UserType _userType;

        #endregion

        #region Constructors

        /// <summary>
        /// Public constructor for the LoginWindow instance
        /// </summary>
        public LoginWindow(Enums.UserType userType)
        {
            _userType = userType;
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
            if ((TextboxUsername.Text == null) || (TextboxPassword.Text == null))
            {
                MessageBox.Show("Fields cannot be blank");
                return;
            }

            //If the user is a SQL client, preform SQL user authentication 
            if (_userType.Equals(Enums.UserType.CloudAccount))
            {
                if (SqlClient.AuthiencateUser(TextboxUsername.Text, TextboxPassword.Text))
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
                var userList = Database.GetUserList();
                foreach (string username in userList)
                {
                    IUser user = Database.GetUser(username);
                    if (user.Username.Equals(TextboxUsername.Text))
                    {
                        if (user.ValidatePassword(TextboxPassword.Text))
                        {
                            Database.SetCurrentUser(user.Username);
                            Close();
                            return;
                        }
                            MessageBox.Show(this, "Incorrect Password");
                            return;
                    }
                }
                MessageBox.Show(this, "User does not exist");

            }
        }

        #endregion

    }
}
