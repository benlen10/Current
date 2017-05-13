using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using UniCade.Windows;

namespace UniCade
{
    public class Program
    {
        #region Properties

        public static Database ActiveDatabase = null;
        public static string DatabasePath = Directory.GetCurrentDirectory() + @"\Database.txt";
        public static string RomPath = @"C:\UniCade\ROMS";
        public static string MediaPath = @"C:\UniCade\Media";
        public static string EmulatorPath = @"C:\UniCade\Emulators";
        public static string PreferencesPath = Directory.GetCurrentDirectory() + @"\Preferences.txt";
        public static int CoinsRequired = 0;
        public static bool RemainingPlaytime = true;
        public static string UserLicenseName;
        public static string UserLicenseKey;
        public static bool IsLicenseValid = false;
        public static IUser CurrentUser;


        #endregion

        [System.STAThreadAttribute]

        /// <summary>
        /// Entry point for the program
        /// </summary>
        public static void Main(string[] args)
        {
            //Initialize the database object
            ActiveDatabase = new Database();

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
                SettingsWindow.CurrentUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", "", "");
            }

            //Verify the current user license and set flag
            if (ValidateSHA256(UserLicenseName + ActiveDatabase.HashKey, UserLicenseKey))
            {
                IsLicenseValid = true;
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
            txt = txt + ("\nDeveloper: " + game.Developer + "\n");
            txt = txt + ("\nPublisher: " + game.Publisher + "\n");
            txt = txt + ("\nPlayers: " + game.Players + "\n");
            txt = txt + ("\nCritic Score: " + game.CriticScore + "\n");
            txt = txt + ("\nESRB Rating: " + game.Tags + "\n");
            txt = txt + ("\nESRB Descriptors: " + game.EsrbDescriptor + "\n");
            txt = txt + ("\nGame Description: " + game.Description + "\n");
            return txt;
        }

        /// <summary>
        /// Hashes a string using SHA256 algorthim 
        /// </summary>
        /// <param name="data"> The input string to be hashed</param>
        /// <returns>Hashed string using SHA256</returns>
        public static string SHA256Hash(string data)
        {
            if (data == null)
            {
                return null;
            }
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Return true if the input hash matches the stored hash data
        /// </summary>
        public static bool ValidateSHA256(string input, string storedHashData)
        {
            string getHashInputData = SHA256Hash(input);
            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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