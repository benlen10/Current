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

        public static string _databasePath = Directory.GetCurrentDirectory() + @"\Database.txt";
        public static string _romPath = @"C:\UniCade\ROMS";
        public static string _mediaPath = @"C:\UniCade\Media";
        public static string _emuPath = @"C:\UniCade\Emulators";
        public static string _prefPath = Directory.GetCurrentDirectory() + @"\Preferences.txt";
        public static int _coins = 0;
        public static bool _playtimeRemaining = true;
        public static string _userLicenseName;
        public static string _userLicenseKey;
        public static bool _validLicense = false;
        public User _user;

        #endregion

        [System.STAThreadAttribute]

        /// <summary>
        /// Entry point for the program
        /// </summary>
        public static void Main(string[] args)
        {
            //Initialize the static database
            Database.Initialize();

            //If preferences file does not exist, load default preference values and save a new file
            if (!FileOps.loadPreferences(_prefPath))
            {
                FileOps.RestoreDefaultPreferences();
                FileOps.savePreferences(_prefPath);
                ShowNotification("WARNING", "Preference file not found.\n Loading defaults...");
            }

            //If the specified rom directory does not exist, creat a new one in with the default path
            if (!Directory.Exists(_romPath))
            {
                Directory.CreateDirectory(_romPath);
                FileOps.CreateNewRomDirectory();
            }

            //If the specified emulator directory does not exist, creat a new one in with the default path
            if (!Directory.Exists(_emuPath))
            {
                Directory.CreateDirectory(_emuPath);
                FileOps.CreateNewEmuDirectory();
                //MessageBox.Show("Emulator directory not found. Creating new directory structure");
            }

            //Verify the integrity of the local media directory and end the program if corruption is dectected  
            if (!FileOps.VerifyMediaDirectory())
            {
                return;
            }

            //If the current user is null, generate the default UniCade user and set as the current user  
            if (SettingsWindow._curUser == null)
            {
                SettingsWindow._curUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", "", "");
            }

            //Verify the current user license and set flag
            if (ValidateSHA256(_userLicenseName + Database.HashKey, _userLicenseKey))
            {
                _validLicense = true;
            }

            //If the database file does not exist in the specified location, load default values and rescan rom directories
            if (!FileOps.loadDatabase(_databasePath))
            {
                FileOps.RestoreDefaultConsoles();
                FileOps.scan(_romPath);
                try
                {
                    FileOps.saveDatabase(_databasePath);
                }
                catch
                {
                    MessageBox.Show("Error Saving Database\n" + _databasePath);
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
        public static string DisplayGameInfo(Game game)
        {
            string txt = "";
            txt = txt + ("\nTitle: " + game.Title + "\n");
            txt = txt + ("\nRelease Date: " + game.ReleaseDate + "\n");
            txt = txt + ("\nConsole: " + game.Console + "\n");
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
        private static void ShowNotification(string title, string body)
        {
            NotificationWindow nfw = new NotificationWindow(title, body);
            nfw.Show();
        }

        #endregion
    }
}