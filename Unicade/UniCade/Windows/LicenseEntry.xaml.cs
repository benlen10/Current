using System.Windows;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for LicenseEntry.xaml
    /// </summary>
    public partial class LicenseEntry : Window
    {

        public LicenseEntry()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }

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
            if (Program.ValidateSHA256(Program._userLicenseName + Database.HashKey, Program._userLicenseKey))
            {
                MessageBox.Show(this, "License is VALID");
                Program._validLicense = true;
                //Save the entered values to the currently active global variables
                Program._userLicenseName = Textbox_User.Text;
                Program._userLicenseKey = Textbox_Key.Text;
                FileOps.savePreferences(Program._prefPath);
                Close();
            }
            else
            {
                Program._validLicense = false;
                FileOps.savePreferences(Program._prefPath);
                MessageBox.Show(this, "License is INVALID");
                Close();
            }
        }
    }
}
