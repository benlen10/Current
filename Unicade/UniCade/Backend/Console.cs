using System.Collections.Generic;

namespace UniCade
{
    public class Console
    {
        #region Constructors 

        public Console()
        {
            Name = "null";
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

        public bool addGame(Game game)
        {
            //If the game console does not match the current console, return false
            if (!game.Console.Equals(Name))
                return false;

            //If a game with an identical file name already exists, return false
            if (GameList.Find(e => e.FileName.Equals(game.FileName)) != null)
                return false;

            //If all conditions are passed, add the game, increment the game count and return true
            GameList.Add(game);
            GameCount++;
            return true;
        }

        #endregion
    }
}