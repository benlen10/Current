using System.Collections.Generic;

namespace UniCade.Interfaces
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
        string EmulatorExePath { get; set; }

        /// <summary>
        /// The launch params for the current emulator
        /// </summary>
        string LaunchParams { get; set; }

        /// <summary>
        /// The common display name for the console
        /// </summary>
        string ConsoleName { get; set; }

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
        string RomFolderPath { get; set; }

        #endregion

        #region  API IDs

        /// <summary>
        /// The unique game identifier for the GamesDB API
        /// </summary>
        int GamesdbApiId { get; set; }

        /// <summary>
        /// The unique game identifier for the MobyGames API
        /// </summary>
        int MobygamesApiId { get; set; }

        /// <summary>
        /// The unique game identifier for the IGDB API
        /// </summary>
        int IgdbApiId { get; set; }

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
        /// <returns>true if the game was sucuessfully removed</returns>
        bool RemoveGame(string gameTitle);

        /// <summary>
        /// Return the IGame object with the specified title
        /// </summary>
        /// <param name="gameTitle">The title of the game to fetch</param>
        /// <returns>IGame object with the matching title</returns>
        IGame GetGame(string gameTitle);

        /// <summary>
        /// Return a string list of all game titles
        /// </summary>
        /// <returns></returns>
        List<string> GetGameList();

        /// <summary>
        /// Return the current number of games in the console
        /// </summary>
        /// <returns>the current game count</returns>
        int GetGameCount();

        #endregion
    }
}