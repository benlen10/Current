using System;
using System.Runtime.InteropServices;
using UniCade.Backend;
using UniCade.Exceptions;
using UniCade.Interfaces;
using UniCade.Objects;

namespace UniCade.ConsoleInterface
{
    internal class UniCadeCmd
    {
        #region Properties

        /// <summary>
        /// DLL Imports to display the console window with the GUI
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        #endregion

        #region Public Methods

        /// <summary>
        /// Run the command line interface directly
        /// </summary>
        public static void Run()
        {
            ShowConsoleWindow();
            ConsoleSelection();
        }

        /// <summary>
        /// Call this method if the GUI interface is currently running
        /// This method will unhook all global hotkeys and hide the main window before
        /// launching the command line interface
        /// </summary>
        public static void PrepAndRun()
        {
            //Unhook all global hotkeys and hide the MainWindow instance
            MainWindow.UnhookKeys();
            Program.App.MainWindow.Hide();

            //Launch the console
            Run();

            //Cleanup after the console window is closed
            MainWindow.KeyboardHook.HookKeys();
            Program.App.MainWindow.Show();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This is the home menu for the command line interface
        /// this menu provides options for the user and exists within an infinte loop
        /// </summary>
        private static void ConsoleSelection()
        {
            while (true)
            {
                //Display heading and options
                System.Console.WriteLine("Options: Exit: (x), Global Rescan (r):, Console Info: (i) <console>, Switch User (s)");
                System.Console.WriteLine("Available Consoles: ");

                //Print all available consoles
                Database.GetConsoleList().ForEach(c => System.Console.WriteLine(c));

                //Fetch user input
                string input = System.Console.ReadLine();

                //(c) = Exit interface
                if (input.Equals("(x)"))
                {
                    FileOps.SaveDatabase(Program.DatabasePath);
                    FileOps.SavePreferences(Program.PreferencesPath);
                    return;
                }
                else if (input.Contains("(r)"))
                {
                    FileOps.Scan(Database.RomPath);
                }
                //(s) = Switch User
                else if (input.Contains("(s)"))
                {
                    SwitchUser();
                }
                //(i) = Console Info
                else if (input.Contains("(i)"))
                {
                    var consoleList = Database.GetConsoleList();
                    foreach (string consoleName in consoleList)
                    {
                        IConsole console = Database.GetConsole(consoleName);
                        if (input.Contains(console.ConsoleName))
                        {
                            DisplayConsoleInfo(console);
                        }
                    }
                }
                //If no modifiers are provided, display the game list for the console
                else
                {
                    IConsole console = Database.GetConsole(input);
                    if (console != null)
                    {
                        DisplayGameList(console);
                    }
                    else
                    {
                        System.Console.WriteLine("Console not found");
                    }
                }
            }
        }

        /// <summary>
        /// This method will display the full list of games for the current console
        /// </summary>
        public static void DisplayGameList(IConsole console)
        {
            if (console == null)
            {
                return;
            }

            bool favoritesView = false;
            while (true)
            {
                //Display the current console, game count and available options

                System.Console.WriteLine(string.Format("{0} (Total Games: {1})", console.ConsoleName, console.GetGameCount()));
                System.Console.WriteLine("Additional Options: Show Info: (i) <game>, Close (c), Toggle Favs View (fv), Toggle Fav (f))\n");
                if (favoritesView)
                {
                    System.Console.WriteLine("[VIEWING FAVORITES ONLY]");
                }

                //Print all games
                var gameList = console.GetGameList();
                foreach (string gameTitle in gameList)
                {
                    IGame g = console.GetGame(gameTitle);
                    if (favoritesView)
                    {
                        if (g.Favorite)
                        {
                            System.Console.WriteLine(g.Title);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(g.Title);
                    }

                }

                //Fetch user input
                string input = System.Console.ReadLine();
                string s = input.Substring(3);
                if (input.Contains("(i)"))
                {
                    string gameTitle = console.GetGameList().Find(g => s.Contains(g));
                    IGame game = console.GetGame(gameTitle);
                    DisplayGameInfo(game);
                }
                //(ci) = Display console info
                else if (input.Equals("(ci)"))
                {
                    DisplayConsoleInfo(console);
                }
                //(fv) = Toggle favorites view
                else if (input.Equals("(fv)"))
                {
                    if (!favoritesView)
                    {
                        System.Console.WriteLine("Favorites View Enabled");
                        favoritesView = true;
                    }
                    else
                    {
                        System.Console.WriteLine("Favorites View Disabled");
                        favoritesView = false;
                    }
                }
                //(f) = Toggle favorite status
                else if (input.Contains("(f)"))
                {
                    string gameTitle = console.GetGameList().Find(g => input.Substring(4).Contains(g));
                    IGame game = console.GetGame(gameTitle);

                    if (game != null)
                    {
                        if (game.Favorite)
                        {
                            game.Favorite = true;
                            System.Console.WriteLine(game.Title + " Added to favorites");
                        }
                        else
                        {
                            game.Favorite = false;
                            System.Console.WriteLine(game.Title + " Removed from favorites");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Error: Game not found");
                    }
                }
                //(c) = Close current dialog
                else if (input.Equals("(c)"))
                {
                    return;
                }
                //If no modifier is provided, attempt to launch the game with the matching title
                else
                {
                    string gameTitle = console.GetGameList().Find(g => input.Equals(g));
                    IGame game = console.GetGame(gameTitle);
                    if (game != null)
                    {
                        try
                        {
                            FileOps.Launch(game);
                        }
                        catch(LaunchException e)
                        {
                            System.Console.WriteLine("Launch Error: " + e.Message);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Error: Game not found");
                    }
                }
            }
        }

        /// <summary>
        /// Display detailed info for the current game
        /// </summary>
        /// <param name="game">The game to display info for</param>
        private static void DisplayGameInfo(IGame game)
        {
            if (game == null)
            {
                return;
            }

            string gameInfo = "";
            gameInfo += ("\nTitle: " + game.Title + "\n");
            gameInfo += ("\nRelease Date: " + game.ReleaseDate + "\n");
            gameInfo += ("\nDeveloper: " + game.DeveloperName + "\n");
            gameInfo += ("\nPublisher: " + game.PublisherName + "\n");
            gameInfo += ("\nPlayers: " + game.SupportedPlayerCount + "\n");
            gameInfo += ("\nUser Score: " + game.UserReviewScore + "\n");
            gameInfo += ("\nCritic Score: " + game.CriticReviewScore + "\n");
            gameInfo += ("\nESRB Rating: " + game.EsrbRating + "\n");
            gameInfo += ("\nESRB Descriptors: " + game.EsrbDescriptors + "\n");
            gameInfo += ("\nGame Description: " + game.Description + "\n");
            System.Console.WriteLine(gameInfo);
            System.Console.WriteLine("\nPress any key to return to previous menu\n");
            System.Console.ReadLine();
        }

        /// <summary>
        /// Display detailed info for the current console
        /// </summary>
        /// <param name="console"></param>
        private static void DisplayConsoleInfo(IConsole console)
        {
            if (console == null)
            {
                return;
            }

            System.Console.WriteLine("Console: " + console.ConsoleName + "\n");
            System.Console.WriteLine("Release Date: " + console.ReleaseDate);
            System.Console.WriteLine("Emulator Path: " + console.EmulatorPath);
            System.Console.WriteLine("Rom Path: " + console.RomPath);
            System.Console.WriteLine("Rom Extension: " + console.RomExtension);
            System.Console.WriteLine("Launch Param: " + console.LaunchParams);
            System.Console.WriteLine("Console Info: " + console.ConsoleInfo);
            System.Console.WriteLine("\nPress any key to return to previous menu\n");
            System.Console.ReadLine();
        }

        /// <summary>
        /// Logout and switch to a differnt user
        /// </summary>
        private static void SwitchUser()
        {
            System.Console.WriteLine("Available Users");
            Database.GetUserList().ForEach(u => System.Console.WriteLine(u));
            System.Console.Write("\n");

            while (true)
            {
                System.Console.WriteLine("Please enter username (Type 'x' to exit)");
                string userName = System.Console.ReadLine();
                if (userName.Equals("x"))
                {
                    return;
                }

                //Attempt to fetch the user with the matching username
                IUser user = Database.GetUser(userName);
                if (user != null)
                {
                    while (true)
                    {
                        string ps = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter password (Type 'x' to exit)");
                        if (ps.Equals("x"))
                        {
                            return;
                        }
                        if (ps.Equals(user.GetUserPassword()))
                        {
                            System.Console.WriteLine("Password Accepted");
                            Database.SetCurrentUser(user);
                            Database.GetCurrentUser().IncrementUserLoginCount();
                            return;
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("Username not found");
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Show the console window
        /// </summary>
        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
        }

        /// <summary>
        /// Hide the console window
        /// </summary>
        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }

        #endregion
    }
}
