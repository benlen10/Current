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

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static void Initalize()
        {
            TotalGameCount = 0;
            ConsoleList = new List<IConsole>();
            UserList = new List<IUser>();

            //If preferences file does not exist, load default preference values and save a new file
            if (!FileOps.LoadPreferences(PreferencesPath))
            {
                FileOps.RestoreDefaultPreferences();
                FileOps.SavePreferences(PreferencesPath);
                ShowNotification("WARNING", "Preference file not found.\n Loading defaults...");
            }

            //If the specified rom directory does not exist, creat a new one in with the default path
            if (!Directory.Exists(RomPath))
            {
                Directory.CreateDirectory(RomPath);
                FileOps.CreateNewRomDirectory();
            }

            //If the specified emulator directory does not exist, creat a new one in with the default path
            if (!Directory.Exists(EmulatorPath))
            {
                Directory.CreateDirectory(EmulatorPath);
                FileOps.CreateNewEmuDirectory();
                //MessageBox.Show("Emulator directory not found. Creating new directory structure");
            }

            //Verify the integrity of the local media directory and end the program if corruption is dectected  
            if (!FileOps.VerifyMediaDirectory())
            {
                return;
            }

            //If the current user is null, generate the default UniCade user and set as the current user  
            if (SettingsWindow.CurrentUser == null)
            {
                SettingsWindow.CurrentUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", Enums.ESRB.Null, "");
            }

            //Verify the current user license and set flag
            if (LicenseEngine.ValidateSHA256(LicenseEngine.UserLicenseName + LicenseEngine.HashKey, LicenseEngine.UserLicenseKey))
            {
                LicenseEngine.IsLicenseValid = true;
            }

            //If the database file does not exist in the specified location, load default values and rescan rom directories
            if (!FileOps.LoadDatabase(DatabasePath))
            {
                FileOps.RestoreDefaultConsoles();
                FileOps.Scan(RomPath);
                try
                {
                    FileOps.SaveDatabase(DatabasePath);
                }
                catch
                {
                    MessageBox.Show("Error Saving Database\n" + DatabasePath);
                }
                ShowNotification("WARNING", "Database file not found.\n Loading defaults...");
            }

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