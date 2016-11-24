using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace UniCade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;

        /*public App(KeyboardHook keyboardHook)
        {
            if (keyboardHook == null) throw new ArgumentNullException("keyboardHook");
            keyboardHook.KeyCombinationPressed += KeyCombinationPressed;
        }*/

        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            window = new MainWindow();

            window.Show();
        }


        internal void InitializeComponent()
        {
            //throw new NotImplementedException();
        }
    }
}

