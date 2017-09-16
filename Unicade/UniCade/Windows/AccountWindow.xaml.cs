using System.Windows;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Network;
using UniCade.Objects;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow
    {

        #region Private Instance VAriables

        /// <summary>
        /// Integer value that represents the current account type (local or cloud)
        /// </summary>
        private readonly Enums.UserType _userType;

        #endregion

        /// <summary>
        /// Public constructor for the AccountWindow class
        /// </summary>
        public AccountWindow(Enums.UserType userType)
        {
            _userType = userType;
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
            if ((TextboxUsername.Text == null) || TextboxEmail.Text == null || (TextboxUserInfo.Text == null))
            {
                MessageBox.Show("Fields cannot be empty");
                return;
            }
            if ((TextboxUsername.Text.Length > 30) || (TextboxEmail.Text.Length > 30) || (TextboxEmail.Text.Length > 30) || (TextboxUserInfo.Text.Length > 100))
            {
                MessageBox.Show("Invalid Length");
                return;
            }
            if (!TextboxEmail.Text.Contains("@"))
            {
                MessageBox.Show("Invalid Email");
                return;
            }

            //Create a new SQL user if the account type is UniCade Cloud
            if (_userType == 0)
            {
                SqlClient.CreateUser(TextboxUsername.Text, TextboxEmail.Text, TextboxEmail.Text, TextboxUserInfo.Text, "Null", "NullProfPath");
            }
            else
            {
                //Create a new local user if the account type standard Unicade
                IUser user = new User(TextboxUsername.Text, TextboxPassword.Text, 0, TextboxEmail.Text, 0, TextboxUserInfo.Text, Enums.Esrb.Null, "null");
                Database.AddUser(user);
                Database.SetCurrentUser(user.Username);
            }
            Close();
        }
    }
}
