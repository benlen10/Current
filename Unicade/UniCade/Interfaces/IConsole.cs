using System.Collections.Generic;

namespace UniCade
{
    public interface IConsole
    {
        string ConsoleInfo { get; set; }
        string EmuPath { get; set; }
        int GameCount { get; }
        List<IGame> GameList { get; }
        string LaunchParam { get; set; }
        string Name { get; set; }
        string PrefPath { get; set; }
        string ReleaseDate { get; set; }
        string RomExt { get; set; }
        string RomPath { get; set; }

        bool AddGame(IGame game);
        bool RemoveGame(IGame game);
    }
}