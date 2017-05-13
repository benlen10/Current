using System.Collections.Generic;

namespace UniCade
{
    public class Console : IConsole
    {

        #region Properties

        public string ConsoleName { get; set; }
        public string ReleaseDate { get; set; }
        public List<IGame> GameList { get; private set; }
        public string EmulatorPath { get; set; }
        public string PreferencesPath { get; set; }
        public string RomPath { get; set; }
        public string RomExtension { get; set; }
        public string ConsoleInfo { get; set; }
        public string LaunchParams { get; set; }
        public int GameCount { get; private set; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Basic constructor for a new game console
        /// </summary>
        public Console(string consoleName)
        {
            ConsoleName = consoleName;
            GameList = new List<IGame>();
        }

        /// <summary>
        /// Full constructor for a new game console
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emuPath"></param>
        /// <param name="romPath"></param>
        /// <param name="prefPath"></param>
        /// <param name="romExt"></param>
        /// <param name="gameCount"></param>
        /// <param name="consoleInfo"></param>
        /// <param name="launchParam"></param>
        /// <param name="releaseDate"></param>
        public Console(string name, string emuPath, string romPath, string prefPath, string romExt, int gameCount, string consoleInfo, string launchParam, string releaseDate)
        {
            ConsoleName = name;
            EmulatorPath = emuPath;
            RomPath = romPath;
            PreferencesPath = prefPath;
            RomExtension = romExt;
            GameCount = gameCount;
            ConsoleInfo = consoleInfo;
            LaunchParams = launchParam;
            ReleaseDate = releaseDate;
            GameList = new List<IGame>();
        }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Add a new game to the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>'true' if the game was sucuessfully added</returns>
        public bool AddGame(IGame game)
        {
            //If the game console does not match the current console, return false
            if (!game.ConsoleName.Equals(ConsoleName))
            {
                return false;
            }

            //If a game with an identical file name already exists, return false
            if (GameList.Find(e => e.FileName.Equals(game.FileName)) != null)
            {
                return false;
            }

            //If all conditions are valid, add the game and increment the game count for both the console and database 
            GameList.Add(game);
            GameCount++;
            Program.ActiveDatabase.TotalGameCount++;
            return true;
        }

        /// <summary>
        /// Remove the specified game from the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>true if the game was sucuessfully removed</returns>
        public bool RemoveGame(IGame game)
        {
            //If the game console does not match the current console, return false
            if (!game.ConsoleName.Equals(ConsoleName))
            {
                return false;
            }

            //Attempt to locate the specified game by fileName
            IGame gameToRemove = GameList.Find(e => e.FileName.Equals(game.FileName));
            if(gameToRemove != null)
            {
                //Remove the game and decriment both the console game count and total game count
                GameList.Remove(gameToRemove);
                GameCount--;
                Program.ActiveDatabase.TotalGameCount--;
            }
            return false;
        }

        #endregion
    }
}