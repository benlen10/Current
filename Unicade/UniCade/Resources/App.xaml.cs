using System.Windows;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;

        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            window = new MainWindow();
            window.Show();
        }

        internal void InitializeComponent()
        {
        }
    }
}

