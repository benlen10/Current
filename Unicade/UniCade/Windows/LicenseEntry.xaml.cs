using System.Windows;
using UniCade.Backend;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for LicenseEntry.xaml
    /// </summary>
    public partial class LicenseEntry
    {

        #region Constructor

        /// <summary>
        /// Public constructor for the LicenseEntry class
        /// </summary>
        public LicenseEntry()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Close the current window instance
        /// </summary>
        private void LicenseEntryWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Attempt to validate the entered license key
        /// </summary>
        private void LicenseEntryWindow_ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            //Bad input checks
            if ((TextboxUser.Text == null) || (TextboxKey.Text == null))
            {
                MessageBox.Show("Missing Required Fields");
                return;
            }

            //If the key is valid, save the key text to the preferences file and close the current window
            if (CryptoEngine.ValidateLicense(TextboxUser.Text, TextboxKey.Text))
            {
                MessageBox.Show(this, "License is VALID");
                Program.IsLicenseValid = true;

                //Save the entered values 
                Program.UserLicenseName = TextboxUser.Text;
                Program.UserLicenseKey = TextboxKey.Text;
                FileOps.SavePreferences();
                Close();
            }
            else
            {
                Program.IsLicenseValid = false;
                FileOps.SavePreferences();
                MessageBox.Show(this, "License is INVALID");
                Close();
            }
        }

        #endregion

    }
}
