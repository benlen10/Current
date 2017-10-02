using System.Windows;
using UniCade.Backend;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for PassWindow.xaml
    /// </summary>
    public partial class PassWindow
    {
        #region Constructors

        /// <summary>
        /// Public constructor for the password window
        /// </summary>
        public PassWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Close the current window instance
        /// </summary>
        private void PassWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Check if the entered pass is valid, otherwise display an error
        /// </summary>
        private void PassWindow_EnterButton_Click(object sender, RoutedEventArgs e)
        {
                if (CryptoEngine.Sha256Hash(TextboxPassword.Password).Equals(Program.PasswordProtection))
                {
                    DialogResult = true;
                    MainWindow.IsPasswordValid = true;
                }
                else
                {
                    MessageBox.Show("Incorrect Password");
                    TextboxPassword.Clear();
                    MainWindow.IsPasswordValid = false;
                }
        }

        #endregion

    }
}
