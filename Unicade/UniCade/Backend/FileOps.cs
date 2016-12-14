using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace UniCade
{
    class FileOps
    {
        #region Global Variables

        public static bool processActive;
        public static Process process;
        public static bool urlLaunch;

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the database file from the specified path
        /// </summary>
        public static bool loadDatabase(string path)
        {
            if (!File.Exists(path))
                return false;

            string line;
            int consoleCount = 0;
            Console console = new Console();
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                if (line.Substring(0, 5).Contains("***"))
                {
                    if (consoleCount > 0)
                    {
                        Database.ConsoleList.Add(console);
                    }
                    console = new Console(r[0].Substring(3), r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[7], r[8]);
                    consoleCount++;
                }
                else
                    console.GameList.Add(new Game(r[0], r[1], Int32.Parse(r[2]), r[3], r[4], r[5], r[6], r[7], r[8], r[9], r[10], r[11], r[12], r[13], r[14], r[15], Int32.Parse(r[16])));
            }
            if (consoleCount > 0)
                Database.ConsoleList.Add(console);

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
        public static void saveDatabase(string path)
        {
            //Delete any preexisting database files 
            if (File.Exists(path))
                File.Delete(path);

            try
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (Console c in Database.ConsoleList)
                    {
                        sw.WriteLine(string.Format("***{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|", c.Name, c.EmuPath, c.RomPath, c.PrefPath, c.RomExt, c.GameCount, "Console Info", c.LaunchParam, c.ReleaseDate));
                        if (c.GameCount > 0)
                        {
                            foreach (Game g in c.GameList)
                            {
                                sw.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}", g.FileName, g.Console, g.LaunchCount, g.ReleaseDate, g.Publisher, g.Developer, g.UserScore, g.CriticScore, g.Players, "Trivia", g.Esrb, g.EsrbDescriptor, g.EsrbSummary, g.Description, g.Genres, g.Tags, g.Favorite));
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error saving database. Check path");
                return;
            }
        }

        /// <summary>
        /// Load preferences from the specified file path
        /// </summary>
        public static bool loadPreferences(String path)
        {
            //Delete any preexisting preference files 
            if (!File.Exists(path))
                return false;

            string[] tmp = { "tmp" };
            char[] sep = { '|' };
            string[] tokenString = { " " };
            StreamReader file = new StreamReader(path);
            string line = file.ReadLine();

            tokenString = line.Split(sep);
            String currentUser = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program._databasePath = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program._emuPath = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program._mediaPath = tokenString[1];

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.showSplash = 1;
            }
            else
            {
                SettingsWindow.showSplash = 0;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if ((tokenString[1].Contains("1")))
            {
                SettingsWindow.scanOnStartup = 1;
            }
            else
            {
                SettingsWindow.scanOnStartup = 0;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            SettingsWindow.restrictESRB = Int32.Parse(tokenString[1]);

            file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.requireLogin = 1;
            }
            else
            {
                SettingsWindow.requireLogin = 0;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.viewEsrb = 1;
            }
            else
            {
                SettingsWindow.viewEsrb = 0;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.showLoading = 1;
            }
            else
            {
                SettingsWindow.showLoading = 0;
            }

            line = file.ReadLine();
            tokenString = line.Split(sep);
            if (tokenString[1].Contains("1"))
            {
                SettingsWindow.payPerPlay = 1;
            }
            else
            {
                SettingsWindow.payPerPlay = 0;
            }

            if (tokenString[2].Contains("1"))
            {
                SettingsWindow.perLaunch = 1;
            }
            else
            {
                SettingsWindow.perLaunch = 0;
            }

            //Parse coin count
            SettingsWindow.coins = Int32.Parse(tokenString[3]);
            SettingsWindow.playtime = Int32.Parse(tokenString[4]);

            //Parse user license key
            line = file.ReadLine();
            tokenString = line.Split(sep);
            Program._userLicenseName = tokenString[1];
            Program._userLicenseKey = tokenString[2];

            //Skip ***Users*** line
            file.ReadLine();

            //Parse user data
            while ((line = file.ReadLine()) != null)
            {
                tokenString = line.Split(sep);
                User user = new User(tokenString[0], tokenString[1], Int32.Parse(tokenString[2]), tokenString[3], Int32.Parse(tokenString[4]), tokenString[5], tokenString[6], "null");
                if (tokenString[6].Length > 0)
                {
                    string[] st = tokenString[7].Split('#');
                    String st1 = "";
                    int i = 1;

                    foreach (string s in st)
                    {
                        if ((i % 2 == 0) && (i > 1))
                        {
                            user.Favorites.Add(new Game(st1, s, 0));

                        }
                        st1 = s + ".zip";
                        i++;
                    }
                }
                Database.UserList.Add(user);
            }
            foreach (User u in Database.UserList)
            {
                if (u.Username.Equals(currentUser))
                {
                    SettingsWindow.curUser = u;
                }
            }
            file.Close();
            return true;
        }

        /// <summary>
        /// Save preferences file to the specified path
        /// </summary>
        public static void savePreferences(String path)
        {
            if (File.Exists(path))
                File.Delete(path);

            foreach (User us in Database.UserList)
            {
                if (SettingsWindow.curUser.Username.Equals(us.Username))
                {
                    Database.UserList.Remove(us);
                    Database.UserList.Add(SettingsWindow.curUser);
                    break;
                }
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("CurrentUser|" + SettingsWindow.curUser.Username);
                sw.WriteLine("_databasePath|" + Program._databasePath);
                sw.WriteLine("EmulatorFolderPath|" + Program._emuPath);
                sw.WriteLine("MediaFolderPath|" + Program._mediaPath);
                sw.WriteLine("ShowSplash|" + SettingsWindow.showSplash);
                sw.WriteLine("ScanOnStartup|" + SettingsWindow.scanOnStartup);
                sw.WriteLine("RestrictESRB|" + SettingsWindow.restrictESRB);
                sw.WriteLine("RequireLogin|" + SettingsWindow.requireLogin);
                sw.WriteLine("CmdOrGui|" + SettingsWindow.cmdOrGui);
                sw.WriteLine("LoadingScreen|" + SettingsWindow.showLoading);
                sw.WriteLine("PaySettings|" + SettingsWindow.payPerPlay + "|" + SettingsWindow.perLaunch + "|" + SettingsWindow.coins + "|" + SettingsWindow.playtime);
                sw.WriteLine("License Key|" + Program._userLicenseName + "|" + Program._userLicenseKey);
                sw.WriteLine("***UserData***");
                foreach (User u in Database.UserList)
                {
                    string favs = "";
                    foreach (Game g in u.Favorites)
                    {
                        favs += (g.Title + "#" + g.Console + "#");
                    }
                    sw.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|", u.Username, u.Pass, u.LoginCount, u.Email, u.TotalLaunchCount, u.UserInfo, u.AllowedEsrb, favs);
                }
            }
        }

        /// <summary>
        /// Scan the target directory for new ROM files and add them to the active database
        /// </summary>
        public static bool scan(string targetDirectory)
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
                scanDirectory(subdirectory, targetDirectory);
            return true;
        }

        /// <summary>
        /// Scan the specied folder for games within a single console
        /// Note: This is a helper function called multiple times by the primary scan function
        /// </summary>
        public static bool scanDirectory(string path, string directory)
        {
            string emuName = new DirectoryInfo(path).Name;
            bool foundConsole = false;
            string[] ext;
            bool duplicate = false;

            Console con = new Console();
            foreach (Console c in Database.ConsoleList)
            {
                if (c.Name.Equals(emuName))
                {
                    con = c;
                    foundConsole = true;
                    break;
                }
            }
            if (!foundConsole)
                return false;

            string[] fileEntries = null;
            string[] exs = con.RomExt.Split('*');
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
                if (SettingsWindow.enforceExt > 0)
                {
                    ext = fileName.Split('.');
                    foreach (string s in exs)
                    {
                        if (ext[1].Equals(s))
                        {
                            duplicate = false;
                            foreach (Game g in con.GameList)
                            {
                                if (g.Title.Equals(Path.GetFileName(fileName)))
                                {
                                    duplicate = true;
                                    break;
                                }
                            }
                            if (!duplicate)
                            {
                                con.GameList.Add(new Game(Path.GetFileName(fileName), con.Name, 0));
                            }
                        }
                    }
                }
                else
                {
                    duplicate = false;
                    foreach (Game g in con.GameList)
                    {
                        if (g.Title.Equals(fileName.Split('.')[0]))
                        {
                            duplicate = true;
                            break;
                        }
                    }
                    if (!duplicate)
                    {
                        con.GameList.Add(new Game(Path.GetFileName(fileName), con.Name, 0));
                    }
                }
            }

            //Delete nonexistent games
            bool found = false;
            Game foundGame = null;
            foreach (Game g in con.GameList)
            {
                found = false;
                foreach (string fileName in fileEntries)
                {
                    if (g.Title.Equals(Path.GetFileName(fileName)))
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    con.GameList.Remove(foundGame);
                    found = false;
                    foundGame = null;
                }
            }
            RefreshGameCount();
            return true;
        }

        public static void loadConsoles()
        {
            string line;
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(@"C:\UniCade\ConsoleList.txt");
            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                Database.ConsoleList.Add(new Console(r[0], r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[8], " "));
            }
            file.Close();
        }

        /// <summary>
        /// Launch the specified ROM file using the paramaters specified by the console
        /// </summary>
        public static void launch(Game game, Console console)
        {
            if (SettingsWindow.curUser.AllowedEsrb.Length > 1)
            {
                if (SettingsWindow.calcEsrb(game.Esrb) >= SettingsWindow.calcEsrb(SettingsWindow.curUser.AllowedEsrb))
                {
                    showNotification("NOTICE", "ESRB " + game.Esrb + " Is Restricted for" + SettingsWindow.curUser.Username);
                    return;
                }
            }

            else if (SettingsWindow.restrictESRB > 0)
            {
                if (SettingsWindow.calcEsrb(game.Esrb) >= SettingsWindow.restrictESRB)
                {
                    showNotification("NOTICE", "ESRB " + game.Esrb + " Is Restricted\n");
                    return;
                }
            }

            game.LaunchCount++;
            SettingsWindow.curUser.TotalLaunchCount++;
            process = new Process();
            string gamePath = ("\"" + console.RomPath + game.FileName + "\"");
            string testGamePath = (console.RomPath + game.FileName);
            if (!File.Exists(testGamePath))
            {
                showNotification("System", "ROM does not exist. Launch Failed");
                return;
            }
            string args = "";
            if (console.Name.Equals("MAME"))
            {
                args = console.LaunchParam.Replace("%file", game.Title);
            }
            else
            {
                args = console.LaunchParam.Replace("%file", gamePath);
            }
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(proc_Exited);
            if (console.Name.Equals("PC"))
            {
                process.StartInfo.FileName = args;
                if (args.Contains("url"))
                {
                    urlLaunch = true;
                }

            }
            else
            {
                if (!File.Exists(console.EmuPath))
                {
                    showNotification("System", "Emulator does not exist. Launch Failed");
                    return;
                }
                process.StartInfo.FileName = console.EmuPath;
                process.StartInfo.Arguments = args;
            }
            showNotification("System", "Loading ROM File");
            process.Start();
            processActive = true;
            MainWindow._gameRunning = true;
        }

        private static void proc_Exited(object sender, System.EventArgs e)
        {
            MainWindow._gameRunning = false;
            processActive = false;
        }

        /// <summary>
        /// Kill the currently running process and toggle flags
        /// </summary>
        public static void KillProcess()
        {
            if (urlLaunch)
            {
                SendKeys.SendWait("^%{F4}");
                showNotification("UniCade System", "Attempting Force Close");
                MainWindow._gameRunning = false;
                processActive = false;
                MainWindow.ReHookKeys();
                return;
            }
            else if (process.HasExited)
            {
                return;
            }

            process.Kill();
            MainWindow.ReHookKeys();
            MainWindow._gameRunning = false;
            processActive = false;
        }

        /// <summary>
        /// Restore the default consoles. These changes will take effect Immediately. 
        /// </summary>
        public static void RestoreDefaultConsoles()
        {
            Database.ConsoleList.Add(new Console("Sega Genisis", @"C:\UniCade\Emulators\Fusion\Fusion.exe", @"C:\UniCade\ROMS\Sega Genisis\", "prefPath", ".bin*.iso*.gen*.32x", 0, "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
            Database.ConsoleList.Add(new Console("Wii", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Wii\", "prefPath", ".gcz*.iso", 0, "consoleInfo", "/b /e %file", "2006"));
            Database.ConsoleList.Add(new Console("NDS", @"C:\UniCade\Emulators\NDS\DeSmuME.exe", @"C:\UniCade\ROMS\NDS\", "prefPath", ".nds", 0, "consoleInfo", "%file", "2005"));
            Database.ConsoleList.Add(new Console("GBC", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBC\", "prefPath", ".gbc", 0, "consoleInfo", "%file", "1998"));
            Database.ConsoleList.Add(new Console("MAME", @"C:\UniCade\Emulators\MAME\mame.bat", @"C:\UniCade\Emulators\MAME\roms\", "prefPath", ".zip", 0, "consoleInfo", "", "1980")); //%file -skip_gameinfo -nowindow
            Database.ConsoleList.Add(new Console("PC", @"C:\Windows\explorer.exe", @"C:\UniCade\ROMS\PC\", "prefPath", ".lnk*.url", 0, "consoleInfo", "%file", "1980"));
            Database.ConsoleList.Add(new Console("GBA", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBA\", "prefPath", ".gba", 0, "consoleInfo", "%file", "2001"));
            Database.ConsoleList.Add(new Console("Gamecube", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Gamecube\", "prefPath", ".iso*.gcz", 0, "consoleInfo", "/b /e %file", "2001"));
            Database.ConsoleList.Add(new Console("NES", @"C:\UniCade\Emulators\NES\Jnes.exe", @"C:\UniCade\ROMS\NES\", "prefPath", ".nes", 0, "consoleInfo", "%file", "1983"));
            Database.ConsoleList.Add(new Console("SNES", @"C:\UniCade\Emulators\ZSNES\zsnesw.exe", @"C:\UniCade\ROMS\SNES\", "prefPath", ".smc", 0, "consoleInfo", "%file", "1990"));
            Database.ConsoleList.Add(new Console("N64", @"C:\UniCade\Emulators\Project64\Project64.exe", @"C:\UniCade\ROMS\N64\", "prefPath", ".n64*.z64", 0, "consoleInfo", "%file", "1996"));
            Database.ConsoleList.Add(new Console("PS1", @"C:\UniCade\Emulators\ePSXe\ePSXe.exe", @"C:\UniCade\ROMS\PS1\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-nogui -loadbin %file", "1994"));
            Database.ConsoleList.Add(new Console("PS2", @"C:\UniCade\Emulators\PCSX2\pcsx2.exe", @"C:\UniCade\ROMS\PS2\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2000"));
            Database.ConsoleList.Add(new Console("Atari 2600", @"C:\UniCade\Emulators\Stella\Stella.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "1977"));
            Database.ConsoleList.Add(new Console("Dreamcast", @"C:\UniCade\Emulators\NullDC\nullDC_Win32_Release-NoTrace.exe", @"C:\UniCade\ROMS\Dreamcast\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
            Database.ConsoleList.Add(new Console("PSP", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\PSP\", "prefPath", ".iso*.cso", 0, "consoleInfo", "%file", "2005"));
            Database.ConsoleList.Add(new Console("Wii U", @"C:\UniCade\Emulators\WiiU\cemu.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "2012"));
            Database.ConsoleList.Add(new Console("Xbox 360", @"C:\UniCade\Emulators\X360\x360.exe", @"C:\UniCade\ROMS\X360\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2005"));
            Database.ConsoleList.Add(new Console("PS3", @"C:\UniCade\Emulators\PS3\ps3.exe", @"C:\UniCade\ROMS\PS3\", "prefPath", ".iso", 0, "consoleInfo", "%file", "2009"));
            Database.ConsoleList.Add(new Console("3DS", @"C:\UniCade\Emulators\PS3\3ds.exe", @"C:\UniCade\ROMS\3DS\", "prefPath", ".iso", 0, "consoleInfo", "%file", "2014"));
        }

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        public static void RefreshGameCount()
        {
            Database.TotalGameCount = 0; ;
            foreach (Console c in Database.ConsoleList)
            {
                foreach (Game g in c.GameList)
                {
                    Database.TotalGameCount++;
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

        public static void CreateNewRomDirectory()
        {
            Directory.CreateDirectory(Program._romPath + @"\Sega Genisis");
            Directory.CreateDirectory(Program._romPath + @"\Wii");
            Directory.CreateDirectory(Program._romPath + @"\NDS");
            Directory.CreateDirectory(Program._romPath + @"\GBC");
            Directory.CreateDirectory(Program._romPath + @"\MAME");
            Directory.CreateDirectory(Program._romPath + @"\PC");
            Directory.CreateDirectory(Program._romPath + @"\GBA");
            Directory.CreateDirectory(Program._romPath + @"\Gamecube");
            Directory.CreateDirectory(Program._romPath + @"\NES");
            Directory.CreateDirectory(Program._romPath + @"\SNES");
            Directory.CreateDirectory(Program._romPath + @"\N64");
            Directory.CreateDirectory(Program._romPath + @"\PS1");
            Directory.CreateDirectory(Program._romPath + @"\PS2");
            Directory.CreateDirectory(Program._romPath + @"\PS3");
            Directory.CreateDirectory(Program._romPath + @"\Atari 2600");
            Directory.CreateDirectory(Program._romPath + @"\Dreamcast");
            Directory.CreateDirectory(Program._romPath + @"\PSP");
            Directory.CreateDirectory(Program._romPath + @"\Wii U");
            Directory.CreateDirectory(Program._romPath + @"\Xbox 360");
            Directory.CreateDirectory(Program._romPath + @"\3DS");

        }

        /// <summary>
        /// Generate a new emulator directory with folders for all default emulators
        /// </summary>
        public static void CreateNewEmuDirectory()
        {
            Directory.CreateDirectory(Program._emuPath + @"\Sega Genisis");
            Directory.CreateDirectory(Program._emuPath + @"\Wii");
            Directory.CreateDirectory(Program._emuPath + @"\NDS");
            Directory.CreateDirectory(Program._emuPath + @"\GBC");
            Directory.CreateDirectory(Program._emuPath + @"\MAME");
            Directory.CreateDirectory(Program._emuPath + @"\GBA");
            Directory.CreateDirectory(Program._emuPath + @"\Gamecube");
            Directory.CreateDirectory(Program._emuPath + @"\NES");
            Directory.CreateDirectory(Program._emuPath + @"\SNES");
            Directory.CreateDirectory(Program._emuPath + @"\N64");
            Directory.CreateDirectory(Program._emuPath + @"\PS1");
            Directory.CreateDirectory(Program._emuPath + @"\PS2");
            Directory.CreateDirectory(Program._emuPath + @"\PS3");
            Directory.CreateDirectory(Program._emuPath + @"\Atari 2600");
            Directory.CreateDirectory(Program._emuPath + @"\Dreamcast");
            Directory.CreateDirectory(Program._emuPath + @"\PSP");
            Directory.CreateDirectory(Program._emuPath + @"\Wii U");
            Directory.CreateDirectory(Program._emuPath + @"\Xbox 360");
            Directory.CreateDirectory(Program._emuPath + @"\3DS");
        }

        /// <summary>
        /// Restore default preferences. These updated preferences will take effect immediatly.
        /// NOTE: These changes are not automatically saved to the database file.
        /// </summary>
        public static void RestoreDefaultPreferences()
        {
            SettingsWindow.curUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", "", "");
            Database.UserList.Add(SettingsWindow.curUser);
            SettingsWindow.showSplash = 0;
            SettingsWindow.scanOnStartup = 0;
            SettingsWindow.restrictESRB = 0;
            SettingsWindow.requireLogin = 0;
            SettingsWindow.cmdOrGui = 0;
            SettingsWindow.showLoading = 0;
            SettingsWindow.payPerPlay = 0;
            SettingsWindow.coins = 1;
            SettingsWindow.playtime = 15;
            SettingsWindow.perLaunch = 0;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Display a timed popup notification in the lower right corner of the interface
        /// </summary>
        private static void showNotification(string title, string body)
        {
            NotificationWindow notification = new NotificationWindow(title, body);
            notification.Show();
        }

        #endregion
    }
}

