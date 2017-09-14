using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Windows;

namespace UnitTests.GUI_Tests
{
    [TestClass]
    public class SettingsWindowTests
    {
        /// <summary>
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SettingsWindowTest1()
        {
            Database.Initalize();
            var window = new SettingsWindow();
            //window.SaveGameInfo();
            var b = new Button();
            var args = EventArgs.Empty;
            window.GamesTab_RescrapeGameButton_Click(b, args);
        }
    }
}

//Button press syntax
//window.GamesTab_Button_DownloadConsole.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));