using System;
using System.Diagnostics;
using UniCadeAndroid.Constants;

namespace UniCadeAndroid.Backend
{
    internal class Program
    {
        #region Properties

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
        public static Enums.EsrbRatings RestrictGlobalEsrbRatings;

        /// <summary>
        /// Specifies if the modern ESRB logos will be displayed instead of the classic logos
        /// </summary>
        public static bool UseModernEsrbLogos;

        /// <summary>
        /// Specifies if the command line interface should be launched on startup instead of the GUI
        /// </summary>
        public static bool PerferCmdInterface;

        /// <summary>
        /// Specifies if a loading screen should be displayed when launching a game
        /// </summary>
        public static bool ShowLoadingScreen;

        /// <summary>
        /// The password required to launch the settings window
        /// </summary>
        public static string PasswordProtection;

        /// <summary>
        /// Specifies if ROM files are required to have the proper extension in order to be imported
        /// </summary>
        public static bool EnforceFileExtensions;

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

            //Validate the media directory and attempt to laod both the database.xml and preferences.xml files
            if (!FileOps.StartupScan())
            {
                return;
            }

            //Refresh the total came count across all consoles
            Database.RefreshTotalGameCount();

            /*
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
            */
        }

        #endregion

        #region Helper Methods

        #endregion

        /// <summary>
        /// The user name for the current license holder
        /// </summary>
        public static string UserLicenseName;

        /// <summary>
        /// The curent license key
        /// </summary>
        public static string UserLicenseKey;

        /// <summary>
        /// True if the current license key is valid
        /// </summary>
        public static bool IsLicenseValid = false;
    }
}