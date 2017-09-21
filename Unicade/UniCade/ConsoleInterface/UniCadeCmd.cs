using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UniCade.Backend;
using UniCade.Exceptions;
using UniCade.Interfaces;
using UniCade.Resources;

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
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SwHide = 0;
        private const int SwShow = 5;

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
        [SuppressMessage("ReSharper", "LocalizableElement")]
        private static void ConsoleSelection()
        {
            while (true)
            {
                //Display heading and options
                Console.WriteLine("Options: Exit: (x), Global Rescan (r):, Console Info: (i) <console>, Switch User (s)");
                Console.WriteLine(Strings.AvailableConsoles);

                //Print all available consoles
                Database.GetConsoleList().ForEach(Console.WriteLine);

                //Fetch user input
                string input = Console.ReadLine();
                if (input != null)
                {
                    //(c) = Exit interface
                    if (input.Equals("(x)"))
                    {
                        FileOps.SaveDatabase(Program.DatabasePath);
                        FileOps.SavePreferences(Program.PreferencesPath);
                        return;
                    }
                    else if (input.Contains("(r)"))
                    {
                        FileOps.ScanAllConsoles();
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
                            Console.WriteLine("Console not found");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method will display the full list of games for the current console
        /// </summary>
        [SuppressMessage("ReSharper", "LocalizableElement")]
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

                Console.WriteLine($"{console.ConsoleName} (Total Games: {console.GetGameCount()})");
                Console.WriteLine("Additional Options: Show Info: (i) <game>, Close (c), Toggle Favs View (fv), Toggle Fav (f))\n");
                if (favoritesView)
                {
                    Console.WriteLine("[VIEWING FAVORITES ONLY]");
                }

                //Print all games
                var gameList = console.GetGameList();
                foreach (string gameTitle in gameList)
                {
                    IGame game = console.GetGame(gameTitle);
                    if (favoritesView)
                    {
                        if (game.Favorite)
                        {
                            Console.WriteLine(game.Title);
                        }
                    }
                    else
                    {
                        Console.WriteLine(game.Title);
                    }

                }

                //Fetch user input
                string input = Console.ReadLine();
                if (input != null)
                {
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
                            Console.WriteLine("Favorites View Enabled");
                            favoritesView = true;
                        }
                        else
                        {
                            Console.WriteLine("Favorites View Disabled");
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
                                Console.WriteLine(game.Title + " Added to favorites");
                            }
                            else
                            {
                                game.Favorite = false;
                                Console.WriteLine(game.Title + " Removed from favorites");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: Game not found");
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
                            catch (LaunchException e)
                            {
                                Console.WriteLine("Launch Error: " + e.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: Game not found");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Display detailed info for the current game
        /// </summary>
        /// <param name="game">The game to display info for</param>
        [SuppressMessage("ReSharper", "LocalizableElement")]
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
            gameInfo += ("\nESRB Rating: " + game.EsrbRatingsRating + "\n");
            gameInfo += ("\nESRB Descriptors: " + game.EsrbDescriptors + "\n");
            gameInfo += ("\nGame Description: " + game.Description + "\n");
            Console.WriteLine(gameInfo);
            Console.WriteLine(Strings.PressAnyKeyToReturnToPreviousMenu);
            Console.ReadLine();
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

            Console.WriteLine(Strings.Console + console.ConsoleName + Strings.NewLine);
            Console.WriteLine(Strings.ReleaseDate + console.ReleaseDate);
            Console.WriteLine(Strings.EmulatorPath + console.EmulatorExePath);
            Console.WriteLine(Strings.RomPath + console.RomFolderPath);
            Console.WriteLine(Strings.RomExtension + console.RomExtension);
            Console.WriteLine(Strings.LaunchParam + console.LaunchParams);
            Console.WriteLine(Strings.ConsoleInfo + console.ConsoleInfo);
            Console.WriteLine(Strings.PressAnyKeyToReturnToPreviousMenu);
            Console.ReadLine();
        }

        /// <summary>
        /// Logout and switch to a differnt user
        /// </summary>
        private static void SwitchUser()
        {
            Console.WriteLine(Strings.AvailableUsers);
            Database.GetUserList().ForEach(Console.WriteLine);
            Console.Write(Strings.NewLine);

            while (true)
            {
                Console.WriteLine(Strings.PleaseEnterUsername);
                string userName = Console.ReadLine();
                if (userName != null)
                {
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
                            string pass = Console.ReadLine();
                            Console.WriteLine(Strings.PleaseEnterPassword);
                            if (pass != null)
                            {
                                if (pass.Equals("x"))
                                {
                                    return;
                                }
                                if (pass.Equals(user.GetUserPassword()))
                                {
                                    Console.WriteLine(Strings.PasswordAccepted);
                                    Database.SetCurrentUser(user.Username);
                                    Database.GetCurrentUser().IncrementUserLoginCount();
                                    return;
                                }
                            }
                        }
                    }
                        Console.WriteLine(Strings.UsernameNotFound);
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
                ShowWindow(handle, SwShow);
            }
        }

        /// <summary>
        /// Hide the console window
        /// </summary>
        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SwHide);
        }

        #endregion
    }
}
