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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniCade;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static SettingsWindow sw;
        public MainWindow()
        {
            InitializeComponent();
        }


        public void loadImage() {

            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("C:\\Users\\Ben\\Dropbox\\Workspace\\Unicade\\UniCade\\Images\\Arcade.jpg");
            b.EndInit();
            image.Source = b;
            
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            loadImage();
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
    }
}
