using System.Windows;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class App : Application
    {
        private MainWindow _window;

        protected override void OnStartup(StartupEventArgs e)
        {
            _window = new MainWindow();
            _window.Show();
        }

        internal void InitializeComponent()
        {
        }
    }
}

