using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UniCade.Constants;

namespace UniCade.Objects
{
    public class CurrentSettings
    {
        /// <summary>
        /// Specifies if the UniCade splash screen should be displayed when the interface is launched
        /// </summary>
        internal bool ShowSplashScreen;

        /// <summary>
        /// Specifies if the the ROM directories should be automatically rescanned 
        /// when the interface is launched
        /// </summary>
        internal bool RescanOnStartup;

        /// <summary>
        /// The path to the current media directory
        /// </summary>
        internal string MediaPath = Directory.GetCurrentDirectory() + @"\Database.txt";

        /// <summary>
        /// True if there is a current game process running
        /// </summary>
        internal bool IsProcessActive;

        /// <summary>
        /// The instance of the current game process
        /// </summary>
        internal Process CurrentProcess;

        /// <summary>
        /// Specifies if certain ESRB ratings should be restricted globally (regardless of user)
        /// </summary>
        internal Enums.EsrbRatings RestrictGlobalEsrbRatings;

        /// <summary>
        /// Specifies if you are required to login to a user account on startup
        /// </summary>
        internal bool RequireLogin;

        /// <summary>
        /// Specifies if the command line interface should be launched on startup instead of the GUI
        /// </summary>
        internal bool PerferCmdInterface;

        /// <summary>
        /// Specifies if a loading screen should be displayed when launching a game
        /// </summary>
        internal bool ShowLoadingScreen;

        /// <summary>
        /// Spcifies the launch options for games across all consoles (Template)
        /// </summary>
        internal int LaunchOptions;

        /// <summary>
        /// If this value is greater than 0, passcode protection is enabled
        /// </summary>
        internal int PasswordProtection;

        /// <summary>
        /// Specifies if ROM files are required to have the proper extension in order to be imported
        /// </summary>
        internal bool EnforceFileExtensions;

        /// <summary>
        /// Specifies is PayPerPlay is enforced
        /// </summary>
        internal bool PayPerPlayEnabled;

        /// <summary>
        /// Specifies the number of coins required if payperplay is enabled
        /// </summary>
        internal int CoinsRequired;

        /// <summary>
        /// The user name for the current license holder
        /// </summary>
        internal string UserLicenseName;

        /// <summary>
        /// The curent license key
        /// </summary>
        internal string UserLicenseKey;

        /// <summary>
        /// True if the current license key is valid
        /// </summary>
        internal bool IsLicenseValid = false;

        /// <summary>
        /// The current hash key used to generate the license key
        /// </summary>
        internal string HashKey { get; set; }

        /// <summary>
        /// The list of current users
        /// </summary>
        internal List<User> UserList;

    }
}
