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
            if ((Textbox_User.Text == null) || (Textbox_Key.Text == null))
            {
                MessageBox.Show("Missing Required Fields");
                return;
            }

            //If the key is valid, save the key text to the preferences file and close the current window
            if (LicenseEngine.ValidateSha256(LicenseEngine.UserLicenseName + LicenseEngine.HashKey, LicenseEngine.UserLicenseKey))
            {
                MessageBox.Show(this, "License is VALID");
                LicenseEngine.IsLicenseValid = true;

                //Save the entered values 
                LicenseEngine.UserLicenseName = Textbox_User.Text;
                LicenseEngine.UserLicenseKey = Textbox_Key.Text;
                FileOps.SavePreferences(Program.PreferencesPath);
                Close();
            }
            else
            {
                LicenseEngine.IsLicenseValid = false;
                FileOps.SavePreferences(Program.PreferencesPath);
                MessageBox.Show(this, "License is INVALID");
                Close();
            }
        }

        #endregion

    }
}
