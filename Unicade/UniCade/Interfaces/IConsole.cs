using System.Collections.Generic;

namespace UniCade
{
    public interface IConsole
    {
        #region Properties

        /// <summary>
        /// Basic console description and info
        /// </summary>
        string ConsoleInfo { get; set; }

        /// <summary>
        /// Full path for the emulators folder
        /// </summary>
        string EmulatorPath { get; set; }

        /// <summary>
        /// The current game count for the console
        /// </summary>
        int GameCount { get; }

        /// <summary>
        /// A list of game objects for the current console instance
        /// </summary>
        List<IGame> GameList { get; }

        /// <summary>
        /// The launch params for the current emulator
        /// </summary>
        string LaunchParams { get; set; }

        /// <summary>
        /// The common display name for the console
        /// </summary>
        string ConsoleName { get; set; }

        /// <summary>
        /// The full path for the console preferences file
        /// </summary>
        string PreferencesPath { get; set; }

        /// <summary>
        /// The original release date for the console
        /// </summary>
        string ReleaseDate { get; set; }

        /// <summary>
        /// The extensions for the current console
        /// </summary>
        string RomExtension { get; set; }

        /// <summary>
        /// The full path to the rom directory for the current console
        /// </summary>
        string RomPath { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new game to the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>'true' if the game was sucuessfully added</returns>
        bool AddGame(IGame game);

        /// <summary>
        /// Remove the specified game from the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>true if the game was sucuessfully removed</returns>
        bool RemoveGame(IGame game);

        #endregion
    }
}