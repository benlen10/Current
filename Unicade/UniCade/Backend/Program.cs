using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UniCade.Backend;
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
        /// The number of coins required to launch a game if PayPerPlay is enabled
        /// </summary>
        public static int CoinsRequired = 0;

        /// <summary>
        /// True or false if playtime is remaining if PayPerPlay is enabled
        /// </summary>
        public static bool RemainingPlaytime = true;

        /// <summary>
        /// The current user object 
        /// </summary>
        public static IUser CurrentUser;

        #endregion

        [System.STAThreadAttribute]

        /// <summary>
        /// Entry point for the program
        /// </summary>
        public static void Main(string[] args)
        {
            Initalize();

            FileOps.StartupScan();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static void Initalize()
        {
            TotalGameCount = 0;
            ConsoleList = new List<IConsole>();
            UserList = new List<IUser>();

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
        }

        #endregion
    }
}