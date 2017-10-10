using System;
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

        #region Private Instance Variables

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

            //Create a new SQL user if the account type is UniCade Cloud
            try
            {
                if (_userType.Equals(Enums.UserType.CloudAccount))
                {
                    SqlLiteClient.CreateNewUser(TextboxUsername.Text, TextboxPassword.Text, TextboxEmail.Text,
                        TextboxUserInfo.Text, "Null");
                }
                else
                {
                    //Create a new local user if the account type standard 
                    IUser user = new User(TextboxUsername.Text, TextboxPassword.Text, 0, TextboxEmail.Text, 0,
                        TextboxUserInfo.Text, Enums.EsrbRatings.Null, "null");
                    Database.AddUser(user);
                }
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }
            Close();
        }
    }
}
