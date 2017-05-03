using System.Collections.Generic;

namespace UniCade
{
    public class Console
    {
        #region Constructors 

        public Console()
        {
            Name = "null";
            GameList = new List<Game>();
        }

        public Console(string name, string emuPath, string romPath, string prefPath, string romExt, int gameCount, string consoleInfo, string launchParam, string releaseDate)
        {
            Name = name;
            EmuPath = emuPath;
            RomPath = romPath;
            PrefPath = prefPath;
            RomExt = romExt;
            GameCount = gameCount;
            ConsoleInfo = consoleInfo;
            LaunchParam = launchParam;
            ReleaseDate = releaseDate;
            GameList = new List<Game>();
        }

        #endregion 

        #region Properties

        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public List<Game> GameList { get; private set; }
        public string EmuPath { get; set; }
        public string PrefPath { get; set; }
        public string RomPath { get; set; }
        public string RomExt { get; set; }
        public string ConsoleInfo { get; set; }
        public string LaunchParam { get; set; }
        public int GameCount { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new game to the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>'true' if the game was sucuessfully added</returns>
        public bool AddGame(Game game)
        {
            //If the game console does not match the current console, return false
            if (!game.Console.Equals(Name))
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
            Database.TotalGameCount++;
            return true;
        }

        /// <summary>
        /// Remove the specified game from the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>true if the game was sucuessfully removed</returns>
        public bool RemoveGame(Game game)
        {
            //If the game console does not match the current console, return false
            if (!game.Console.Equals(Name))
            {
                return false;
            }

            //Attempt to locate the specified game by fileName
            Game gameToRemove = GameList.Find(e => e.FileName.Equals(game.FileName));
            if(gameToRemove != null)
            {
                //Remove the game and decriment both the console game count and total game count
                GameList.Remove(gameToRemove);
                GameCount--;
                Database.TotalGameCount--;
            }
            return false;
        }

        #endregion
    }
}