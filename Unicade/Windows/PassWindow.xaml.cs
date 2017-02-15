using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UniCade.Windows
{
    /// <summary>
    /// Interaction logic for PassWindow.xaml
    /// </summary>
    public partial class PassWindow : Window
    {

        /// <summary>
        /// Public constructor for the password window
        /// </summary>
        public PassWindow()
        {
            InitializeComponent();
        }

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
            Int32.TryParse(Textbox_Password.Text, out int n);
            if (n > 0)
            {
                if (Int32.Parse(Textbox_Password.Text) == SettingsWindow._passProtect)
                {
                    DialogResult = DialogResult.OK;
                    MainWindow._validPAss = true;
                }

            }
            Textbox_Password.Text = null;
        }
    }
}
