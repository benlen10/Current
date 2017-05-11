using System.Collections.Generic;

namespace UniCade
{
    public interface IConsole
    {
        /// <summary>
        /// Basic console description and info
        /// </summary>
        string ConsoleInfo { get; set; }
        string EmulatorPath { get; set; }
        int GameCount { get; }
        List<IGame> GameList { get; }
        string LaunchParams { get; set; }
        string ConsoleName { get; set; }
        string PreferencesPath { get; set; }
        string ReleaseDate { get; set; }
        string RomExtension { get; set; }
        string RomPath { get; set; }

        #region Methods

        bool AddGame(IGame game);

        bool RemoveGame(IGame game);

        #endregion
    }
}