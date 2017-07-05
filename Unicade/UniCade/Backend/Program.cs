using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UniCade.Backend;
using UniCade.ConsoleInterface;
using UniCade.Constants;
using UniCade.Exceptions;
using UniCade.Windows;

namespace UniCade
{
    public class Program
    {
        #region Properties

        /// <summary>
        /// The current list of consoles
        /// </summary>
        public static List<IConsole> ConsoleList { get; set; }

        /// <summary>
        /// The list of current users
        /// </summary>
        public static List<IUser> UserList { get; set; }

        /// <summary>
        /// The current number of games across all game consoles
        /// </summary>
        public static int TotalGameCount { get; set; }

        /// <summary>
        /// The path to the Database.txt file
        /// </summary>
        public static string DatabasePath = Directory.GetCurrentDirectory() + @"\Database.txt";

        /// <summary>
        /// The path to the current ROM directory
        /// </summary>
        public static string RomPath = @"C:\UniCade\ROMS";

        /// <summary>
        /// The path to the current media directory
        /// </summary>
        public static string MediaPath = @"C:\UniCade\Media";

        /// <summary>
        /// The path to the current Emulators directory
        /// </summary>
        public static string EmulatorPath = @"C:\UniCade\Emulators";

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
        /// Specifies if certain ESRB ratings should be restricted globally (regardless of user)
        /// </summary>
        public static Enums.ESRB RestrictGlobalESRB;

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
        public static int EnforceFileExtensions;

        /// <summary>
        /// The current user object 
        /// </summary>
        public static IUser CurrentUser;

        /// <summary>
        /// The current application 
        /// </summary>
        public static App App;

        public static SettingsWindow SettingsWindow;

        #endregion

        [System.STAThreadAttribute]

        /// <summary>
        /// Entry point for the program
        /// </summary>
        public static void Main(string[] args)
        {
            //Initalize the properties
            Initalize();

            FileOps.StartupScan();

            if (!PerferCmdInterface)
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
        /// Initalize the current properties
        /// </summary>
        public static void Initalize()
        {
            TotalGameCount = 0;
            ConsoleList = new List<IConsole>();
            UserList = new List<IUser>();
            IUser UniCadeUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", Enums.ESRB.Null, "");
            UserList.Add(UniCadeUser);
            Program.CurrentUser = UniCadeUser;

        }

        /// <summary>
        /// Display all game info fields in plain text format
        /// </summary>
        public static string DisplayGameInfo(IGame game)
        {
            string txt = "";
            txt = txt + ("\nTitle: " + game.Title + "\n");
            txt = txt + ("\nRelease Date: " + game.ReleaseDate + "\n");
            txt = txt + ("\nConsole: " + game.ConsoleName + "\n");
            txt = txt + ("\nLaunch Count: " + game.LaunchCount.ToString() + "\n");
            txt = txt + ("\nDeveloper: " + game.DeveloperName + "\n");
            txt = txt + ("\nPublisher: " + game.PublisherName + "\n");
            txt = txt + ("\nPlayers: " + game.PlayerCount + "\n");
            txt = txt + ("\nCritic Score: " + game.CriticReviewScore + "\n");
            txt = txt + ("\nESRB Rating: " + game.Tags + "\n");
            txt = txt + ("\nESRB Descriptors: " + game.EsrbDescriptors + "\n");
            txt = txt + ("\nGame Description: " + game.Description + "\n");
            return txt;
        }

        #region Helper Methods

        /// <summary>
        /// Display a timed notification in the bottom left corner of the interface 
        /// </summary>
        private static void ShowNotification(string titleText, string bodyText)
        {
            NotificationWindow notification = new NotificationWindow(titleText, bodyText);
            notification.Show();
        }

        /// <summary>
        /// Add a new console to the database
        /// </summary>
        /// <param name="console"></param>
        /// <returns>true if the console was sucuessfully added</returns>
        public static bool AddConsole(IConsole console)
        {
            //Return false if a console with a duplicate name already exists
            if (ConsoleList.Find(e => e.ConsoleName.Equals(console.ConsoleName)) != null)
            {
                return false;
            }

            ConsoleList.Add(console);
            return true;
        }

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        /// <returns>Total game count</returns>
        public static int RefreshTotalGameCount()
        {
            int count = 0;
            foreach (Console console in ConsoleList)
            {
                count += console.GameCount;
            }
            TotalGameCount = count;
            return count;


            #endregion
        }
    }
}