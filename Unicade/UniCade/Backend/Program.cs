﻿using System;
using System.Diagnostics;
using System.IO;
using UniCade.ConsoleInterface;
using UniCade.Constants;
using UniCade.Interfaces;

namespace UniCade.Backend
{
    public class Program
    {
        #region Properties

        /// <summary>
        /// The path to the Database.txt file
        /// </summary>
        public static string DatabasePath = Directory.GetCurrentDirectory() + @"\Database.txt";

        /// <summary>
        /// The path to the Preferences.txt file
        /// </summary>
        public static string PreferencesPath = Directory.GetCurrentDirectory() + @"\Preferences.txt";

        /// <summary>
        /// Specifies if the UniCade splash screen should be displayed when the interface is launched
        /// </summary>
        public static bool ShowSplashScreen;

        /// <summary>
        /// Specifies if the the ROM directories should be automatically rescanned 
        /// when the interface is launched
        /// </summary>
        public static bool RescanOnStartup;

        /// <summary>
        /// The path to the current media directory
        /// </summary>
        public static string MediaPath = Directory.GetCurrentDirectory() + @"\Database.txt";

        /// <summary>
        /// True if there is a current game process running
        /// </summary>
        public static bool IsProcessActive;

        /// <summary>
        /// The instance of the current game process
        /// </summary>
        public static Process CurrentProcess;

        /// <summary>
        /// Specifies if certain ESRB ratings should be restricted globally (regardless of user)
        /// </summary>
        public static Enums.Esrb RestrictGlobalEsrb;

        /// <summary>
        /// Specifies if you are required to login to a user account on startup
        /// </summary>
        public static bool RequireLogin;

        /// <summary>
        /// Specifies if the command line interface should be launched on startup instead of the GUI
        /// </summary>
        public static bool PerferCmdInterface;

        /// <summary>
        /// Specifies if a loading screen should be displayed when launching a game
        /// </summary>
        public static bool ShowLoadingScreen;

        /// <summary>
        /// Spcifies the launch options for games across all consoles (Template)
        /// </summary>
        public static int LaunchOptions;

        /// <summary>
        /// If this value is greater than 0, passcode protection is enabled
        /// </summary>
        public static int PasswordProtection;

        /// <summary>
        /// Specifies if ROM files are required to have the proper extension in order to be imported
        /// </summary>
        public static bool EnforceFileExtensions;

        /// <summary>
        /// The current application 
        /// </summary>
        public static App App;

        #endregion

        #region  Public Methods

        /// <summary>
        /// Entry point for the program
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Main(string[] args)
        {
            //Initalize the database, preform an initial scan and refresh the total game count
            Database.Initalize();
            FileOps.StartupScan();
            Database.RefreshTotalGameCount();

            //Launch either the GUI or the legacy command line interface
            if (PerferCmdInterface)
            {
                UniCadeCmd.Run();
            }
            else
            {
                App = new App();
                App.InitializeComponent();
                App.Run();
            }
        }

        /// <summary>
        /// Display all game info fields in plain text format
        /// </summary>
        public static string DisplayGameInfo(IGame game)
        {
            string text = "";
            text += ("\nTitle: " + game.Title + "\n");
            text += ("\nRelease Date: " + game.ReleaseDate + "\n");
            text += ("\nConsole: " + game.ConsoleName + "\n");
            text += ("\nLaunch Count: " + game.GetLaunchCount().ToString() + "\n");
            text += ("\nDeveloper: " + game.DeveloperName + "\n");
            text += ("\nPublisher: " + game.PublisherName + "\n");
            text += ("\nPlayers: " + game.SupportedPlayerCount + "\n");
            text += ("\nCritic Score: " + game.CriticReviewScore + "\n");
            text += ("\nESRB Rating: " + game.Tags + "\n");
            text += ("\nESRB Descriptors: " + game.EsrbDescriptors + "\n");
            text += ("\nGame Description: " + game.Description + "\n");
            return text;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Display a timed notification in the bottom left corner of the interface 
        /// </summary>
        private static void ShowNotification(string titleText, string bodyText)
        {
            NotificationWindow notification = new NotificationWindow(titleText, bodyText);
            notification.Show();
        }

        #endregion
    }
}