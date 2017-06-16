using System.IO;
using System.Security.Cryptography;
using System.Text;
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
        /// The current Database instance
        /// </summary>
        public static IDatabase Database = null;

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
            //Initialize the database object
            Database = new Database();

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
            var app = new App();
            app.InitializeComponent();
            app.Run();
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

        #endregion
    }
}