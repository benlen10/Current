using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.ConsoleInterface
{
    class UniCadeCmd
    {

        public static void Run()
        {
            ShowConsoleWindow();
            while (true)
            {
                System.Console.WriteLine("Enter Cmd (Type 'exit' to close)");
                string line = System.Console.ReadLine();
                if (line.Contains("exit"))
                {
                    HideConsoleWindow();
                    return;
                }
            }
        }

        public static void PrepAndRun()
        {
            //Prep
            MainWindow.UnhookKeys();
            Program.App.MainWindow.Hide();

            //Run
            Run();

            //Cleanup
            MainWindow.KeyboardHook.HookKeys();
            Program.App.MainWindow.Show();
        }









        public static void displayConsoles()
        {
            while (true)
            {
                System.Console.WriteLine("Available Consoles:   [Exit: (c), Rescan (r):, Info: (i), GUI (g), Switch User (u), (s) Settings, (uf) User Favs (d) Download Info <Console>");
                string list = "";
                foreach (IConsole console in Program.ConsoleList)
                {
                    list = list + " " + "[" + console.ConsoleName + "]";
                }
                System.Console.WriteLine(list);

                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    FileOps.SaveDatabase(Program.DatabasePath);
                    FileOps.SavePreferences(Program.PreferencesPath);
                    return;
                }
                else if (input.Contains("(r)"))
                {
                    FileOps.Scan(Program.RomPath);
                }

                else if (input.Equals("(uf)"))
                {
                    displayUserFavs();
                }


                else if (input.Contains("(u)"))
                {
                    Login();
                }
                else if (input.Contains("(d)"))
                {

                    foreach (Console console in Program.ConsoleList)
                    {
                        if (input.Contains(console.ConsoleName))
                        {
                            foreach (Game game in console.GameList)
                            {
                                WebOps.ScrapeInfo(game);
                            }
                        }
                    }

                }
                else if (input.Contains("(i)"))
                {
                    foreach (IConsole console in Program.ConsoleList)
                    {
                        if (input.Contains(console.ConsoleName))
                        {
                            DisplayConsoleInfo(console);
                        }
                    }
                }
                foreach (IConsole console in Program.ConsoleList)
                {
                    if (input.Equals(console.ConsoleName))
                    {
                        displayGameList(console);
                    }
                }
            }
        }

        public static void displayGameList(IConsole console)
        {
            bool fav = false;
            while (true)
            {

                string text = string.Format("{0} (Total Games: {1})", console.ConsoleName, console.GameCount);
                System.Console.WriteLine(text);
                System.Console.WriteLine("Additional Options:Info: (i) <game>, Close (c),Global Fav (gf), (uf) Add User Fav, Display Favorites (f) Console Info (ci)\n");

                //Display Game List
                foreach (Game g in console.GameList)
                {
                    if (fav)
                    {
                        if (g.Favorite == 1)
                        {
                            System.Console.WriteLine(g.Title);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(g.Title);
                    }

                }


                string input = System.Console.ReadLine();
                string s = input.Substring(3);
                if (input.Contains("(i)"))
                {
                    foreach (Game g in console.GameList)
                    {
                        if (s.Contains(g.Title))
                        {
                            System.Console.Write(DisplayGameInfo(g));
                            string inp = System.Console.ReadLine();
                            while (true)
                            {
                                inp = System.Console.ReadLine();
                                if (inp.Equals("(c)"))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (input.Equals("(ci)"))
                {
                    DisplayConsoleInfo(console);
                }
                else if (input.Equals("(f)"))
                {
                    if (!fav)
                    {
                        fav = true;
                    }
                    else
                    {
                        fav = false;
                    }
                }
                else if (input.Contains("(uf)"))
                {
                    //Program.CurrentUser.FavoritesList.Add(g);
                    System.Console.WriteLine("\n***Added to User Favorites***\n");
                }
                else if (input.Contains("(gf)"))
                {
                    foreach (Game g in console.GameList)
                    {
                        if (input.Substring(4).Contains(g.Title))
                        {
                            if (g.Favorite < 1)
                            {
                                g.Favorite = 1;
                                System.Console.WriteLine("\n***Added to Favorites***\n");
                            }
                            else
                            {
                                g.Favorite = 0;
                                System.Console.WriteLine("\n***Removed From Favorites***\n");
                            }
                            break;
                        }
                    }
                }

                else if (input.Equals("(c)"))
                {
                    return;
                }
                else




                {
                    foreach (Game game in console.GameList)
                    {
                        if (input.Contains(game.Title))
                        {
                            FileOps.Launch(game);

                        }
                    }
                }
            }

        }

        public static void displayUserFavs()
        {
            Program.CurrentUser.FavoritesList.ForEach(e => System.Console.WriteLine(e.Title));
            while (true)
            {
                System.Console.WriteLine("[Type (c) to close info window]\n");
                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    return;
                }
                //Check for matching input string to launch
                IGame game = Program.CurrentUser.FavoritesList.Find(e => e.Title.Equals(input));
                if (game != null)
                {
                    FileOps.Launch(game);
                }
            }
        }


        public static string DisplayGameInfo(IGame game)
        {
            string txt = "";

            txt = txt + ("[Type (c) to close info window]\n");
            txt = txt + ("\nTitle: " + game.Title + "\n");
            txt = txt + ("\nRelease Date: " + game.ReleaseDate + "\n");
            txt = txt + ("\nDeveloper: " + game.DeveloperName + "\n");
            txt = txt + ("\nPublisher: " + game.PublisherName + "\n");
            txt = txt + ("\nPlayers: " + game.PlayerCount + "\n");
            txt = txt + ("\nUser Score: " + game.UserReviewScore + "\n");
            txt = txt + ("\nCritic Score: " + game.CriticReviewScore + "\n");
            txt = txt + ("\nESRB Rating: " + game.EsrbRating + "\n");
            txt = txt + ("\nESRB Descriptors: " + game.EsrbDescriptors + "\n");
            txt = txt + ("\nmame Description: " + game.Description + "\n");
            return txt;
        }

        public static void DisplayConsoleInfo(IConsole console)
        {
            while (true)
            {
                System.Console.WriteLine("[Type (c) to close info window]\n");
                System.Console.WriteLine("Console: " + console.ConsoleName);
                System.Console.WriteLine("Release Date: " + console.ReleaseDate);
                System.Console.WriteLine("Emulator Path: " + console.EmulatorPath);
                System.Console.WriteLine("Rom Path: " + console.RomPath);
                System.Console.WriteLine("Rom Extension: " + console.RomExtension);
                System.Console.WriteLine("Launch Param: " + console.LaunchParams);
                System.Console.WriteLine("Console Info: " + console.ConsoleInfo);
                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    return;
                }
            }
        }

        public static void Login()
        {
            while (true)
            {
                System.Console.WriteLine("Please enter username (Type x to exit)");
                string userName = System.Console.ReadLine();
                if (userName.Equals("x"))
                {
                    return;
                }
                foreach (User user in Program.UserList)
                {
                    if (userName.Equals(user.Username))
                    {
                        while (true)
                        {
                            string ps = System.Console.ReadLine();
                            System.Console.WriteLine("Please enter password");
                            if (ps.Equals("x"))
                            {
                                return;
                            }
                            if (ps.Equals(user.GetUserPassword()))
                            {
                                System.Console.WriteLine("Password Accepted");
                                Program.CurrentUser = user;
                                Program.CurrentUser.LoginCount++;
                                return;
                            }
                        }
                    }
                }
            }
        }






        #region Helper Methods

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

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        #endregion
    }
}
