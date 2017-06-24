using System;
using System.Windows;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for PassWindow.xaml
    /// </summary>
    public partial class PassWindow : Window
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
            Int32.TryParse(Textbox_Password.Password, out int n);
            if (n > 0)
            {
                if (Int32.Parse(Textbox_Password.Password) == Program.PasswordProtection)
                {
                    DialogResult = true;
                    MainWindow.IsPasswordValid = true;
                }
                else
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
            Close();
        }

        #endregion

    }
}
