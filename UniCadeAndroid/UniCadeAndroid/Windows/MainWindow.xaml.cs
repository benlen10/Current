
using System.Collections;
using UniCadeAndroid.Interfaces;

// ReSharper disable once CheckNamespace
namespace UniCadeAndroid.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Properties

        /// <summary>
        /// The list of currently active consoles
        /// </summary>
        public static ArrayList ActiveConsoleList;

        /// <summary>
        /// The index number for the currently displayed console
        /// </summary>
        public static int IndexNumber;

        /// <summary>
        /// True if the password entered in the passWindow is valid
        /// </summary>
        public static bool IsPasswordValid;

        /// <summary>
        /// True if the game selection screen is the current screen
        /// </summary>
        public static bool IsGameSelectionPageActive;

        /// <summary>
        /// True if a game is currently running
        /// </summary>
        public static bool IsGameRunning;

        /// <summary>
        /// True if the game info window is currently active
        /// </summary>
        public static bool IsInfoWindowActive;

        /// <summary>
        /// The current console that is selected
        /// </summary>
        public IConsole CurrentConsole;

        /// <summary>
        /// True if the SettingsWindow is currently visible
        /// </summary>
        public static bool IsSettingsWindowActive;

        /// <summary>
        /// True if currenly only favorites are being displayed
        /// </summary>
        public static bool IsFavoritesViewActive;

        /// <summary>
        /// Specifies if the ESRB logo should be displayed while browsing games
        /// </summary>
        public static bool DisplayEsrbWhileBrowsing;

        /// <summary>
        /// Specifies if games with a restricted ESRB rating should be hidden 
        /// </summary>
        public static bool HideRestrictedEsrbGames;

        #endregion
    }
}