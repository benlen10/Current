using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UniCade;
using UniCade.Backend;
using UniCade.Windows;

namespace UnitTests.GUI_Tests
{
    [TestClass]
    public class SettingsWindowTests
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SettingsWindowTest1()
        {
            Program.Initalize();
            SettingsWindow window = new SettingsWindow();
            //window.SaveGameInfo();
            Button b = new Button();
            EventArgs args = EventArgs.Empty;
            window.GamesTab_RescrapeGameButton_Click(b, args);
        }
    }
}

//Button press syntax
//window.GamesTab_Button_DownloadConsole.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
