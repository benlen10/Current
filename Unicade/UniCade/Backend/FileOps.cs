using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Windows;

namespace UniCade
{
    class FileOps
    {
        #region Properties

        /// <summary>
        /// True if there is a current game process running
        /// </summary>
        public static bool IsProcessActive;

        /// <summary>
        /// The instance of the current game process
        /// </summary>
        public static Process CurrentProcess;

        /// <summary>
        /// True if the current game is a steam URL
        /// </summary>
        public static bool IsSteamGame;

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the database file from the specified path
        /// </summary>
        public static bool LoadDatabase(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            string rawLine;
            int consoleCount = 0;
            IConsole console = new Console("newConsole");
            char[] seperatorChar = { '|' };
            string[] spaceChar = { " " };
            StreamReader file = new StreamReader(path);

            while ((rawLine = file.ReadLine()) != null)
            {
                spaceChar = rawLine.Split(seperatorChar);
                if (rawLine.Substring(0, 5).Contains("***"))
                {
                    if (consoleCount > 0)
                    {
                        Program.Database.AddConsole(console);
                    }
                    console = new Console(spaceChar[0].Substring(3), spaceChar[1], spaceChar[2], spaceChar[3], spaceChar[4], Int32.Parse(spaceChar[5]), spaceChar[6], spaceChar[7], spaceChar[8]);
                    consoleCount++;
                }
                else
                {
                    console.GameList.Add(new Game(spaceChar[0], spaceChar[1], Int32.Parse(spaceChar[2]), spaceChar[3], spaceChar[4], spaceChar[5], spaceChar[6], spaceChar[7], spaceChar[8], spaceChar[9], Enums.ConvertStringToEsrbEnum(spaceChar[10]), spaceChar[11], spaceChar[12], spaceChar[13], spaceChar[14], spaceChar[15], Int32.Parse(spaceChar[16])));
                }
            }
            if (consoleCount > 0)
            {
                Program.Database.AddConsole(console);
            }

            if (consoleCount < 1)
            {
                MessageBox.Show("Fatal Error: Database File is corrupt");
                return false;
            }
            file.Close();
            return true;
        }

        /// <summary>
        /// Save the database to the specified path. Delete any preexisting database files
        /// </summary>
        public static void SaveDatabase(string path)
        {
            //Delete any preexisting database files 
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            try
            {
                using (StreamWriter streamWriter = File.CreateText(path))
                {
                    foreach (IConsole console in Program.Database.ConsoleList)
                    {
                        streamWriter.WriteLine(string.Format("***{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|", console.ConsoleName, console.EmulatorPath, console.RomPath, console.PreferencesPath, console.RomExtension, console.GameCount, "Console Info", console.LaunchParams, console.ReleaseDate));
                        if (console.GameCount > 0)
                        {
                            foreach (IGame game in console.GameList)
                            {
                                streamWriter.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}", game.FileName, game.ConsoleName, game.LaunchCount, game.ReleaseDate, game.PublisherName, game.DeveloperName, game.UserReviewScore, game.CriticReviewScore, game.PlayerCount, "Trivia", game.EsrbRating, game.EsrbDescriptors, game.EsrbSummary, game.Description, game.Genres, game.Tags, game.Favorite));
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error saving database\n" + Program.DatabasePath + "\n"+ e.Message);
                return;
            }
        }

        /// <summary>
        /// Load preferences from the specified file path
        /// </summary>
        public static bool LoadPreferences(String path)
        {
            //Delete any preexisting preference files 
            if (!File.Exists(path))
            {
                return false;
            }

            string[] tmp = { "tmp" };
            char[] sep = { '|' };
            string[] tokenString = { " " };
            StreamReader file = new StreamReader(path);
            string line = file.ReadLine();

            tokenString = line.Split(sep);
            String currentUser = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program.DatabasePath = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program.EmulatorPath = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program.MediaPath = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.ShowSplashScreen = true;
            }
            else
            {
                SettingsWindow.ShowSplashScreen = false;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if ((tokenString[1].Contains("1")))
            {
                SettingsWindow.RescanOnStartup = true;
            }
            else
            {
                SettingsWindow.RescanOnStartup = false;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            SettingsWindow.RestrictGlobalESRB = Enums.ConvertStringToEsrbEnum(tokenString[1]);

            file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.RequireLogin = true;
            }
            else
            {
                SettingsWindow.RequireLogin = false;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.DisplayEsrbWhileBrowsing = true;
            }
            else
            {
                SettingsWindow.DisplayEsrbWhileBrowsing = true;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.ShowLoadingScreen = true;
            }
            else
            {
                SettingsWindow.ShowLoadingScreen = true;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.PayPerPlayEnabled = true;
            }
            else
            {
                SettingsWindow.PayPerPlayEnabled = false;
            }

            if (tokenString[2].Contains("1"))
            {
                SettingsWindow.LaunchOptions = 1;
            }
            else
            {
                SettingsWindow.LaunchOptions = 0;
            }

            //Parse coin count
            SettingsWindow.CoinsRequired = Int32.Parse(tokenString[3]);
            SettingsWindow.Playtime = Int32.Parse(tokenString[4]);

            //Parse user license key
            line = file.ReadLine();
            tokenString = line.Split(sep);
            LicenseEngine.UserLicenseName = tokenString[1];
            LicenseEngine.UserLicenseKey = tokenString[2];

            //Skip ***Users*** line
            file.ReadLine();

            //Parse user data
            while ((line = file.ReadLine()) != null)
            {
                tokenString = line.Split(sep);
                IUser user = new User(tokenString[0], tokenString[1], Int32.Parse(tokenString[2]), tokenString[3], Int32.Parse(tokenString[4]), tokenString[5], Enums.ConvertStringToEsrbEnum(tokenString[6]), "null");
                if (tokenString[6].Length > 0)
                {
                    string[] rawString = tokenString[7].Split('#');
                    String string1 = "";
                    int iterator = 1;

                    foreach (string s in rawString)
                    {
                        if ((iterator % 2 == 0) && (iterator > 1))
                        {
                            user.Favorites.Add(new Game(string1, s));

                        }
                        string1 = s + ".zip";
                        iterator++;
                    }
                }
                Program.Database.UserList.Add(user);
            }
            foreach (IUser user in Program.Database.UserList)
            {
                if (user.Username.Equals(currentUser))
                {
                    SettingsWindow.CurrentUser = user;
                }
            }
            file.Close();
            return true;
        }

        /// <summary>
        /// Save preferences file to the specified path
        /// </summary>
        public static void SavePreferences(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            foreach (IUser user in Program.Database.UserList)
            {
                if (SettingsWindow.CurrentUser.Username.Equals(user.Username))
                {
                    Program.Database.UserList.Remove(user);
                    Program.Database.UserList.Add(SettingsWindow.CurrentUser);
                    break;
                }
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("CurrentUser|" + SettingsWindow.CurrentUser.Username);
                sw.WriteLine("_databasePath|" + Program.DatabasePath);
                sw.WriteLine("EmulatorFolderPath|" + Program.EmulatorPath);
                sw.WriteLine("MediaFolderPath|" + Program.MediaPath);
                sw.WriteLine("ShowSplash|" + SettingsWindow.ShowSplashScreen);
                sw.WriteLine("ScanOnStartup|" + SettingsWindow.RescanOnStartup);
                sw.WriteLine("RestrictESRB|" + SettingsWindow.RestrictGlobalESRB);
                sw.WriteLine("RequireLogin|" + SettingsWindow.RequireLogin);
                sw.WriteLine("CmdOrGui|" + SettingsWindow.PerferCmdInterface);
                sw.WriteLine("LoadingScreen|" + SettingsWindow.ShowLoadingScreen);
                sw.WriteLine("PaySettings|" + SettingsWindow.PayPerPlayEnabled + "|" + SettingsWindow.LaunchOptions + "|" + SettingsWindow.CoinsRequired + "|" + SettingsWindow.Playtime);
                sw.WriteLine("License Key|" + LicenseEngine.UserLicenseName + "|" + LicenseEngine.UserLicenseKey);
                sw.WriteLine("***UserData***");
                foreach (IUser user in Program.Database.UserList)
                {
                    string favs = "";
                    foreach (IGame g in user.Favorites)
                    {
                        favs += (g.Title + "#" + g.ConsoleName + "#");
                    }
                    sw.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|", user.Username, user.GetUserPassword(), user.LoginCount, user.Email, user.TotalLaunchCount, user.UserInfo, user.AllowedEsrb, favs);
                }
            }
        }

        /// <summary>
        /// Scan the target directory for new ROM files and add them to the active database
        /// </summary>
        public static bool Scan(string targetDirectory)
        {
            string[] subdirectoryEntries = null;
            try
            {
                subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            }
            catch
            {
                MessageBox.Show("Directory Not Found: " + targetDirectory);
                return false;
            }
            foreach (string subdirectory in subdirectoryEntries)
            {
                ScanDirectory(subdirectory, targetDirectory);
            }

            return true;
        }

        /// <summary>
        /// Scan the specied folder for games within a single console
        /// Note: This is a helper function called multiple times by the primary scan function
        /// </summary>
        public static bool ScanDirectory(string path, string directory)
        {
            string emuName = new DirectoryInfo(path).Name;
            bool foundConsole = false;
            string[] extension;
            bool duplicate = false;

            IConsole currentConsole = null;
            foreach (IConsole console in Program.Database.ConsoleList)
            {
                if (console.ConsoleName.Equals(emuName))
                {
                    currentConsole = console;
                    foundConsole = true;
                    break;
                }
            }
            if (!foundConsole)
            {
                return false;
            }

            string[] fileEntries = null;
            string[] exs = currentConsole.RomExtension.Split('*');
            try
            {
                fileEntries = Directory.GetFiles(path);
            }
            catch
            {
                MessageBox.Show("Directory Not Found: " + path);
                return false;
            }
            foreach (string fileName in fileEntries)
            {
                if (SettingsWindow.EnforceFileExtensions > 0)
                {
                    extension = fileName.Split('.');
                    foreach (string s in exs)
                    {
                        if (extension[1].Equals(s))
                        {
                            duplicate = false;
                            foreach (IGame game in currentConsole.GameList)
                            {
                                if (game.Title.Equals(Path.GetFileName(fileName)))
                                {
                                    duplicate = true;
                                    break;
                                }
                            }
                            if (!duplicate)
                            {
                                currentConsole.GameList.Add(new Game(Path.GetFileName(fileName), currentConsole.ConsoleName));
                            }
                        }
                    }
                }
                else
                {
                    duplicate = false;
                    foreach (IGame game in currentConsole.GameList)
                    {
                        if (game.Title.Equals(fileName.Split('.')[0]))
                        {
                            duplicate = true;
                            break;
                        }
                    }
                    if (!duplicate)
                    {
                        currentConsole.GameList.Add(new Game(Path.GetFileName(fileName), currentConsole.ConsoleName));
                    }
                }
            }

            //Delete nonexistent games
            bool found = false;
            IGame foundGame = null;
            foreach (IGame game in currentConsole.GameList)
            {
                found = false;
                foreach (string fileName in fileEntries)
                {
                    if (game.Title.Equals(Path.GetFileName(fileName)))
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    currentConsole.GameList.Remove(foundGame);
                    found = false;
                    foundGame = null;
                }
            }
            RefreshGameCount();
            return true;
        }

        /// <summary>
        /// Load all current consoles from the text file in the local directory
        /// </summary>
        public static void LoadConsoles()
        {
            string line;
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(@"C:\UniCade\ConsoleList.txt");
            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                Program.Database.ConsoleList.Add(new Console(r[0], r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[8], " "));
            }
            file.Close();
        }

        /// <summary>
        /// Launch the specified ROM file using the paramaters specified by the console
        /// </summary>
        public static void Launch(IGame game)
        {
            if (!SettingsWindow.CurrentUser.AllowedEsrb.Equals(Enums.ESRB.Null))
            {
                if (game.EsrbRating >= SettingsWindow.CurrentUser.AllowedEsrb)
                {
                    ShowNotification("NOTICE", "ESRB " + game.EsrbRating + " Is Restricted for" + SettingsWindow.CurrentUser.Username);
                    return;
                }
            }

            else if (SettingsWindow.RestrictGlobalESRB > 0)
            {
                if (game.EsrbRating >= SettingsWindow.RestrictGlobalESRB)
                {
                    ShowNotification("NOTICE", "ESRB " + game.EsrbRating + " Is Restricted\n");
                    return;
                }
            }

            game.LaunchCount++;
            SettingsWindow.CurrentUser.TotalLaunchCount++;
            CurrentProcess = new Process();

            //Fetch the console object
            IConsole console = Program.Database.ConsoleList.Find(e => e.ConsoleName.Equals(game.ConsoleName));

            string gamePath = ("\"" + console.RomPath + game.FileName + "\"");
            string testGamePath = (console.RomPath + game.FileName);
            if (!File.Exists(testGamePath))
            {
                ShowNotification("System", "ROM does not exist. Launch Failed");
                return;
            }
            string args = "";
            if (console.ConsoleName.Equals("MAME"))
            {
                args = console.LaunchParams.Replace("%file", game.Title);
            }
            else
            {
                args = console.LaunchParams.Replace("%file", gamePath);
            }
            CurrentProcess.EnableRaisingEvents = true;
            CurrentProcess.Exited += new EventHandler(ProcessExited);
            if (console.ConsoleName.Equals("PC"))
            {
                CurrentProcess.StartInfo.FileName = args;
                if (args.Contains("url"))
                {
                    IsSteamGame = true;
                }

            }
            else
            {
                if (!File.Exists(console.EmulatorPath))
                {
                    ShowNotification("System", "Emulator does not exist. Launch Failed");
                    return;
                }
                CurrentProcess.StartInfo.FileName = console.EmulatorPath;
                CurrentProcess.StartInfo.Arguments = args;
            }
            ShowNotification("System", "Loading ROM File");
            CurrentProcess.Start();
            IsProcessActive = true;
            MainWindow._gameRunning = true;
        }

        /// <summary>
        /// Set instance variables to false after the current game process has exited
        /// </summary>
        private static void ProcessExited(object sender, System.EventArgs e)
        {
            MainWindow._gameRunning = false;
            IsProcessActive = false;
        }

        /// <summary>
        /// Kill the currently running process and toggle flags
        /// </summary>
        public static void KillProcess()
        {
            if (IsSteamGame)
            {
                SendKeys.SendWait("^%{F4}");
                ShowNotification("UniCade System", "Attempting Force Close");
                MainWindow._gameRunning = false;
                IsProcessActive = false;
                MainWindow.ReHookKeys();
                return;
            }
            else if (CurrentProcess.HasExited)
            {
                return;
            }

            CurrentProcess.Kill();
            MainWindow.ReHookKeys();
            MainWindow._gameRunning = false;
            IsProcessActive = false;
        }

        /// <summary>
        /// Restore the default consoles. These changes will take effect Immediately. 
        /// </summary>
        public static void RestoreDefaultConsoles()
        {
            Program.Database.AddConsole(new Console("Sega Genisis", @"C:\UniCade\Emulators\Fusion\Fusion.exe", @"C:\UniCade\ROMS\Sega Genisis\", "prefPath", ".bin*.iso*.gen*.32x", 0, "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
            Program.Database.AddConsole(new Console("Wii", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Wii\", "prefPath", ".gcz*.iso", 0, "consoleInfo", "/b /e %file", "2006"));
            Program.Database.AddConsole(new Console("NDS", @"C:\UniCade\Emulators\NDS\DeSmuME.exe", @"C:\UniCade\ROMS\NDS\", "prefPath", ".nds", 0, "consoleInfo", "%file", "2005"));
            Program.Database.AddConsole(new Console("GBC", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBC\", "prefPath", ".gbc", 0, "consoleInfo", "%file", "1998"));
            Program.Database.AddConsole(new Console("MAME", @"C:\UniCade\Emulators\MAME\mame.bat", @"C:\UniCade\Emulators\MAME\roms\", "prefPath", ".zip", 0, "consoleInfo", "", "1980")); //%file -skip_gameinfo -nowindow
            Program.Database.AddConsole(new Console("PC", @"C:\Windows\explorer.exe", @"C:\UniCade\ROMS\PC\", "prefPath", ".lnk*.url", 0, "consoleInfo", "%file", "1980"));
            Program.Database.AddConsole(new Console("GBA", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBA\", "prefPath", ".gba", 0, "consoleInfo", "%file", "2001"));
            Program.Database.AddConsole(new Console("Gamecube", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Gamecube\", "prefPath", ".iso*.gcz", 0, "consoleInfo", "/b /e %file", "2001"));
            Program.Database.AddConsole(new Console("NES", @"C:\UniCade\Emulators\NES\Jnes.exe", @"C:\UniCade\ROMS\NES\", "prefPath", ".nes", 0, "consoleInfo", "%file", "1983"));
            Program.Database.AddConsole(new Console("SNES", @"C:\UniCade\Emulators\ZSNES\zsnesw.exe", @"C:\UniCade\ROMS\SNES\", "prefPath", ".smc", 0, "consoleInfo", "%file", "1990"));
            Program.Database.AddConsole(new Console("N64", @"C:\UniCade\Emulators\Project64\Project64.exe", @"C:\UniCade\ROMS\N64\", "prefPath", ".n64*.z64", 0, "consoleInfo", "%file", "1996"));
            Program.Database.AddConsole(new Console("PS1", @"C:\UniCade\Emulators\ePSXe\ePSXe.exe", @"C:\UniCade\ROMS\PS1\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-nogui -loadbin %file", "1994"));
            Program.Database.AddConsole(new Console("PS2", @"C:\UniCade\Emulators\PCSX2\pcsx2.exe", @"C:\UniCade\ROMS\PS2\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2000"));
            Program.Database.AddConsole(new Console("Atari 2600", @"C:\UniCade\Emulators\Stella\Stella.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "1977"));
            Program.Database.AddConsole(new Console("Dreamcast", @"C:\UniCade\Emulators\NullDC\nullDC_Win32_Release-NoTrace.exe", @"C:\UniCade\ROMS\Dreamcast\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
            Program.Database.AddConsole(new Console("PSP", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\PSP\", "prefPath", ".iso*.cso", 0, "consoleInfo", "%file", "2005"));
            Program.Database.AddConsole(new Console("Wii U", @"C:\UniCade\Emulators\WiiU\cemu.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "2012"));
            Program.Database.AddConsole(new Console("Xbox 360", @"C:\UniCade\Emulators\X360\x360.exe", @"C:\UniCade\ROMS\X360\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2005"));
            Program.Database.AddConsole(new Console("PS3", @"C:\UniCade\Emulators\PS3\ps3.exe", @"C:\UniCade\ROMS\PS3\", "prefPath", ".iso", 0, "consoleInfo", "%file", "2009"));
            Program.Database.AddConsole(new Console("3DS", @"C:\UniCade\Emulators\PS3\3ds.exe", @"C:\UniCade\ROMS\3DS\", "prefPath", ".iso", 0, "consoleInfo", "%file", "2014"));
        }

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        public static void RefreshGameCount()
        {
            Program.Database.TotalGameCount = 0; ;
            foreach (IConsole console in Program.Database.ConsoleList)
            {
                foreach (IGame g in console.GameList)
                {
                    Program.Database.TotalGameCount++;
                }
            }
        }

        /// <summary>
        /// Validate the integrity of the Media folder located in the current working directory
        /// </summary>
        public static bool VerifyMediaDirectory()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media"))
            {
                MessageBox.Show("Media directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles"))
            {
                MessageBox.Show("Media (Consoles) directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Media\Consoles").Length < 4)
            {
                MessageBox.Show("Media (Consoles) directory is corrupt. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos").Length < 4)
            {
                MessageBox.Show("Media (Console Logos) directory is corrupt. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos"))
            {
                MessageBox.Show("Media (Console Logos) directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Games"))
            {
                MessageBox.Show("Media (Games) directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds"))
            {
                MessageBox.Show("Media (Backgrounds) directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb"))
            {
                MessageBox.Show("Media (ESRB) directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\Interface Background.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Marquee.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Icon.ico"))
            {
                MessageBox.Show("Media (Backgrounds) directory is corrupt. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }

            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (AO).png"))
            {
                MessageBox.Show("Media (ESRB) directory is corrupt. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Create a new ROM directory in the current filesystem
        /// </summary>
        public static void CreateNewRomDirectory()
        {
            Directory.CreateDirectory(Program.RomPath + @"\Sega Genisis");
            Directory.CreateDirectory(Program.RomPath + @"\Wii");
            Directory.CreateDirectory(Program.RomPath + @"\NDS");
            Directory.CreateDirectory(Program.RomPath + @"\GBC");
            Directory.CreateDirectory(Program.RomPath + @"\MAME");
            Directory.CreateDirectory(Program.RomPath + @"\PC");
            Directory.CreateDirectory(Program.RomPath + @"\GBA");
            Directory.CreateDirectory(Program.RomPath + @"\Gamecube");
            Directory.CreateDirectory(Program.RomPath + @"\NES");
            Directory.CreateDirectory(Program.RomPath + @"\SNES");
            Directory.CreateDirectory(Program.RomPath + @"\N64");
            Directory.CreateDirectory(Program.RomPath + @"\PS1");
            Directory.CreateDirectory(Program.RomPath + @"\PS2");
            Directory.CreateDirectory(Program.RomPath + @"\PS3");
            Directory.CreateDirectory(Program.RomPath + @"\Atari 2600");
            Directory.CreateDirectory(Program.RomPath + @"\Dreamcast");
            Directory.CreateDirectory(Program.RomPath + @"\PSP");
            Directory.CreateDirectory(Program.RomPath + @"\Wii U");
            Directory.CreateDirectory(Program.RomPath + @"\Xbox 360");
            Directory.CreateDirectory(Program.RomPath + @"\3DS");

        }

        /// <summary>
        /// Generate a new emulator directory with folders for all default emulators
        /// </summary>
        public static void CreateNewEmuDirectory()
        {
            Directory.CreateDirectory(Program.EmulatorPath + @"\Sega Genisis");
            Directory.CreateDirectory(Program.EmulatorPath + @"\Wii");
            Directory.CreateDirectory(Program.EmulatorPath + @"\NDS");
            Directory.CreateDirectory(Program.EmulatorPath + @"\GBC");
            Directory.CreateDirectory(Program.EmulatorPath + @"\MAME");
            Directory.CreateDirectory(Program.EmulatorPath + @"\GBA");
            Directory.CreateDirectory(Program.EmulatorPath + @"\Gamecube");
            Directory.CreateDirectory(Program.EmulatorPath + @"\NES");
            Directory.CreateDirectory(Program.EmulatorPath + @"\SNES");
            Directory.CreateDirectory(Program.EmulatorPath + @"\N64");
            Directory.CreateDirectory(Program.EmulatorPath + @"\PS1");
            Directory.CreateDirectory(Program.EmulatorPath + @"\PS2");
            Directory.CreateDirectory(Program.EmulatorPath + @"\PS3");
            Directory.CreateDirectory(Program.EmulatorPath + @"\Atari 2600");
            Directory.CreateDirectory(Program.EmulatorPath + @"\Dreamcast");
            Directory.CreateDirectory(Program.EmulatorPath + @"\PSP");
            Directory.CreateDirectory(Program.EmulatorPath + @"\Wii U");
            Directory.CreateDirectory(Program.EmulatorPath + @"\Xbox 360");
            Directory.CreateDirectory(Program.EmulatorPath + @"\3DS");
        }

        /// <summary>
        /// Restore default preferences. These updated preferences will take effect immediatly.
        /// NOTE: These changes are not automatically saved to the database file.
        /// </summary>
        public static void RestoreDefaultPreferences()
        {
            SettingsWindow.CurrentUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", Enums.ESRB.Null , "");
            Program.Database.UserList.Add(SettingsWindow.CurrentUser);
            SettingsWindow.ShowSplashScreen = false;
            SettingsWindow.RescanOnStartup = false;
            SettingsWindow.RestrictGlobalESRB = 0;
            SettingsWindow.RequireLogin = false;
            SettingsWindow.PerferCmdInterface = false;
            SettingsWindow.ShowLoadingScreen = false;
            SettingsWindow.PayPerPlayEnabled = false;
            SettingsWindow.CoinsRequired = 1;
            SettingsWindow.Playtime = 15;
            SettingsWindow.LaunchOptions = 0;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Display a timed popup notification in the lower right corner of the interface
        /// </summary>
        private static void ShowNotification(string title, string body)
        {
            NotificationWindow notification = new NotificationWindow(title, body);
            notification.Show();
        }

        #endregion
    }
}

