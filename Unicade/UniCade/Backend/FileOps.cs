using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniCade.Constants;
using UniCade.Exceptions;
using UniCade.Interfaces;
using UniCade.Objects;
using UniCade.Resources;
using Console = UniCade.Objects.Console;

namespace UniCade.Backend
{
    internal class FileOps
    {
        #region Properties

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
            StreamReader file = new StreamReader(path);

            while ((rawLine = file.ReadLine()) != null)
            {
                var spaceChar = rawLine.Split(seperatorChar);
                if (rawLine.Substring(0, 5).Contains("***"))
                {
                    if (consoleCount > 0)
                    {
                        Database.AddConsole(console);
                    }
                    console = new Console(spaceChar[0].Substring(3), spaceChar[1], spaceChar[2], spaceChar[3], spaceChar[4], spaceChar[6], spaceChar[7], spaceChar[8]);
                    consoleCount++;
                }
                else
                {
                    console.AddGame(new Game(spaceChar[0], spaceChar[1], Int32.Parse(spaceChar[2]), spaceChar[3], spaceChar[4], spaceChar[5], spaceChar[6], spaceChar[7], spaceChar[8], spaceChar[9], Enums.ConvertStringToEsrbEnum(spaceChar[10]), spaceChar[11], spaceChar[12], spaceChar[13], spaceChar[14], spaceChar[15], spaceChar[16]));
                }
            }
            if (consoleCount > 0)
            {
                Database.AddConsole(console);
            }

            if (consoleCount < 1)
            {
                MessageBox.Show(Strings.DatabaseCorrupt);
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
                    var consoleList = Database.GetConsoleList();
                    foreach (string consoleName in consoleList)
                    {
                        IConsole console = Database.GetConsole(consoleName);
                        streamWriter.WriteLine(
                            $"***{console.ConsoleName}|{console.EmulatorExePath}|{console.RomFolderPath}|PrefPath|{console.RomExtension}|{console.GetGameCount()}|Console Info|{console.LaunchParams}|{console.ReleaseDate}|");
                        if (console.GetGameCount() > 0)
                        {
                            var gameList = console.GetGameList();
                            foreach (string gameTitle in gameList)
                            {
                                IGame game = console.GetGame(gameTitle);
                                streamWriter.WriteLine(
                                    $"{game.FileName}|{game.ConsoleName}|{game.GetLaunchCount()}|{game.ReleaseDate}|{game.PublisherName}|{game.DeveloperName}|{game.UserReviewScore}|{game.CriticReviewScore}|{game.SupportedPlayerCount}|Trivia|{game.EsrbRatingsRating}|{game.EsrbDescriptors}|{game.EsrbSummary}|{game.Description}|{game.Genres}|{game.Tags}|{game.Favorite}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Strings.ErrorSavingDatabase + Program.DatabasePath + Strings.NewLine + e.Message);
            }
        }

        /// <summary>
        /// Load preferences from the specified file path
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static bool LoadPreferences(string path)
        {
            try
            {
                //Delete any preexisting preference files 
                if (!File.Exists(path))
                {
                    return false;
                }

                char[] sep = {'|'};
                StreamReader file = new StreamReader(path);
                string line = file.ReadLine();

                var tokenString = line.Split(sep);
                string currentUser = tokenString[1];

                file.ReadLine();
                //tokenString = line.Split(sep);
                //Program.DatabasePath = tokenString[1];

                file.ReadLine();
                //Default emulator path (Depricated)
                //tokenString = line.Split(sep);
                //tokenString[1];

                file.ReadLine();
                //tokenString = line.Split(sep);
                //Program.MediaPath = tokenString[1];

                line = file.ReadLine();
                tokenString = line.Split(sep);
                Program.ShowSplashScreen = tokenString[1].Contains("1");

                line = file.ReadLine();
                tokenString = line.Split(sep);
                Program.RescanOnStartup = (tokenString[1].Contains("1"));

                line = file.ReadLine();
                tokenString = line.Split(sep);
                Program.RestrictGlobalEsrbRatings = Enums.ConvertStringToEsrbEnum(tokenString[1]);

                file.ReadLine();
                tokenString = line.Split(sep);
                Program.RequireLogin = tokenString[1].Contains("1");

                line = file.ReadLine();
                tokenString = line.Split(sep);
                MainWindow.DisplayEsrbWhileBrowsing = tokenString[1].Contains("1");

                line = file.ReadLine();
                tokenString = line.Split(sep);
                Program.ShowLoadingScreen = tokenString[1].Contains("1");

                line = file.ReadLine();
                tokenString = line.Split(sep);
                PayPerPlay.PayPerPlayEnabled = tokenString[1].Contains("1");

                Program.LaunchOptions = tokenString[2].Contains("1") ? 1 : 0;

                //Parse coin count
                PayPerPlay.CoinsRequired = Int32.Parse(tokenString[3]);
                PayPerPlay.Playtime = Int32.Parse(tokenString[4]);

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
                    IUser user = new User(tokenString[0], tokenString[1], Int32.Parse(tokenString[2]), tokenString[3],
                        Int32.Parse(tokenString[4]), tokenString[5], Enums.ConvertStringToEsrbEnum(tokenString[6]),
                        "null");
                    if (tokenString[6].Length > 0)
                    {
                        string[] rawString = tokenString[7].Split('#');
                        var string1 = "";
                        int iterator = 1;

                        foreach (string s in rawString)
                        {
                            if ((iterator % 2 == 0) && (iterator > 1))
                            {
                                user.AddFavorite(new Game(string1, s));

                            }
                            string1 = s + ".zip";
                            iterator++;
                        }
                    }
                    if (user.Username != "UniCade")
                    {
                        Database.AddUser(user);
                    }
                }
                var userList = Database.GetUserList();
                foreach (string username in userList)
                {
                    IUser user = Database.GetUser(username);
                    if (user.Username.Equals(currentUser))
                    {
                        Database.SetCurrentUser(user.Username);
                    }
                }
                file.Close();
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
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

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("CurrentUser|" + Database.GetCurrentUser().Username);
                sw.WriteLine("_databasePath|" + Program.DatabasePath);
                sw.WriteLine("EmulatorFolderPath|" + "DefaultEmulatorPath");
                sw.WriteLine("MediaFolderPath|" + Program.MediaPath);
                sw.WriteLine("ShowSplash|" + Program.ShowSplashScreen);
                sw.WriteLine("ScanOnStartup|" + Program.RescanOnStartup);
                sw.WriteLine("RestrictESRB|" + Program.RestrictGlobalEsrbRatings);
                sw.WriteLine("RequireLogin|" + Program.RequireLogin);
                sw.WriteLine("CmdOrGui|" + Program.PerferCmdInterface);
                sw.WriteLine("LoadingScreen|" + Program.ShowLoadingScreen);
                sw.WriteLine("PaySettings|" + PayPerPlay.PayPerPlayEnabled + "|" + Program.LaunchOptions + "|" + PayPerPlay.CoinsRequired + "|" + PayPerPlay.Playtime);
                sw.WriteLine("License Key|" + LicenseEngine.UserLicenseName + "|" + LicenseEngine.UserLicenseKey);
                sw.WriteLine("***UserData***");

                var userList = Database.GetUserList();
                foreach (string username in userList)
                {
                    IUser user = Database.GetUser(username);
                    string favs = "";
                    foreach (IGame g in user.GetFavoritesList())
                    {
                        favs += (g.Title + "#" + g.ConsoleName + "#");
                    }
                    sw.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|", user.Username, user.GetUserPassword(), user.GetUserLoginCount(), user.Email, user.GetUserLaunchCount(), user.UserInfo, user.AllowedEsrbRatings, favs);
                }
            }
        }

        /// <summary>
        /// Scan the target directory for new ROM files and add them to the active database
        /// </summary>
        public static void ScanAllConsoles()
        {
            foreach (string consoleName in Database.GetConsoleList())
            {
                ScanSingleConsole(Database.GetConsole(consoleName));
            }
        }

        /// <summary>
        /// Scan the specied folder for games within a single console
        /// Note: This is a helper function called multiple times by the primary scan function
        /// </summary>
        public static bool ScanSingleConsole(IConsole console)
        {
            if (console == null)
            {
                return false;
            }
            //Attempt to open the directory and fetch file entries
            string[] fileEntries;
            try
            {
                fileEntries = Directory.GetFiles(console.RomFolderPath);
            }
            catch
            {
                MessageBox.Show(Strings.DirectoryNotFound + console.RomFolderPath);
                return false;
            }

            //Add games to the current console object
            foreach (string fileName in fileEntries)
            {
                string gameTitle = fileName.Split('.')[0];
                if (console.GetGame(gameTitle) == null)
                {
                    console.AddGame(new Game(Path.GetFileName(fileName), console.ConsoleName));
                }
            }

            //Delete nonexistent games
            var gameTitleList = console.GetGameList();
            foreach (string gameTitle in gameTitleList)
            {
                if (!fileEntries.Contains(gameTitle))
                {
                    console.RemoveGame(gameTitle);
                }
            }
            return true;
        }

        /// <summary>
        /// Load all current consoles from the text file in the local directory
        /// </summary>
        public static void LoadConsoles()
        {
            string line;
            char[] sep = { '|' };
            StreamReader file = new StreamReader(@"C:\UniCade\ConsoleList.txt");
            while ((line = file.ReadLine()) != null)
            {
                var r = line.Split(sep);
                Database.AddConsole(new Console(r[0], r[1], r[2], r[3], r[4], r[6], r[8], ""));
            }
            file.Close();
        }

        /// <summary>
        /// Launch the specified ROM file using the paramaters specified by the console
        /// </summary>
        public static bool Launch(IGame game)
        {
            if (game == null)
            {
                throw new LaunchException("Game is Null");
            }
            if (Database.GetCurrentUser().AllowedEsrbRatings > 0)
            {
                if (game.EsrbRatingsRating > Database.GetCurrentUser().AllowedEsrbRatings)
                {
                    throw new LaunchException(("ESRB " + game.EsrbRatingsRating + " Is Restricted for" + Database.GetCurrentUser().Username));
                }
            }

            if (Program.RestrictGlobalEsrbRatings > 0)
            {
                if (game.EsrbRatingsRating > Program.RestrictGlobalEsrbRatings)
                {
                    throw new LaunchException(("ESRB " + game.EsrbRatingsRating + " Is Restricted globally"));
                }
            }

            if (PayPerPlay.PayPerPlayEnabled && (PayPerPlay.CoinsRequired > 0))
            {
                if (PayPerPlay.CurrentCoins < PayPerPlay.CoinsRequired)
                {
                    throw new LaunchException(("Pay Per Play Active: Please Insert Coins"));
                }
            }

            //Fetch the console object
            IConsole console = Database.GetConsole(game.ConsoleName);

            string gamePath = ("\"" + console.RomFolderPath + game.FileName + "\"");
            string testGamePath = (console.RomFolderPath + game.FileName);
            if (!File.Exists(testGamePath))
            {
                throw new LaunchException(("ROM does not exist. Launch Failed"));
            }
            var args = console.LaunchParams.Replace("%file", console.ConsoleName.Equals("MAME") ? game.Title : gamePath);

            if (console.ConsoleName.Equals("PC"))
            {
                Program.CurrentProcess.StartInfo.FileName = args;
                if (args.Contains("url"))
                {
                    IsSteamGame = true;
                }

            }
            else
            {
                if (!File.Exists(console.EmulatorExePath))
                {
                    throw new LaunchException(("Emulator does not exist. Launch Failed"));
                }
                Program.CurrentProcess.StartInfo.FileName = console.EmulatorExePath;
                Program.CurrentProcess.StartInfo.Arguments = args;
            }

            Program.CurrentProcess = new Process { EnableRaisingEvents = true };
            Program.CurrentProcess.Exited += ProcessExited;

            //Only decrement coin count on a sucuessful launch
            if (PayPerPlay.PayPerPlayEnabled && (PayPerPlay.CoinsRequired > 0))
            {
                PayPerPlay.DecrementCoins();
            }

            game.IncrementLaunchCount();
            Database.GetCurrentUser().IncrementUserLaunchCount();
            Program.CurrentProcess.Start();
            Program.IsProcessActive = true;
            MainWindow.IsGameRunning = true;
            return true;
        }

        /// <summary>
        /// Set instance variables to false after the current game process has exited
        /// </summary>
        private static void ProcessExited(object sender, EventArgs e)
        {
            MainWindow.IsGameRunning = false;
            Program.IsProcessActive = false;
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
                MainWindow.IsGameRunning = false;
                Program.IsProcessActive = false;
                MainWindow.KeyboardHook.HookKeys();
                return;
            }
            if (Program.CurrentProcess.HasExited)
            {
                return;
            }

            Program.CurrentProcess.Kill();
            MainWindow.KeyboardHook.HookKeys();
            MainWindow.IsGameRunning = false;
            Program.IsProcessActive = false;
        }

        /// <summary>
        /// Restore the default consoles. These changes will take effect Immediately. 
        /// </summary>
        public static void RestoreDefaultConsoles()
        {
            Database.AddConsole(new Console("Sega Genisis", @"C:\UniCade\Emulators\Fusion\Fusion.exe", @"C:\UniCade\ROMS\Sega Genisis\", "prefPath", ".bin*.iso*.gen*.32x", "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
            Database.AddConsole(new Console("Wii", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Wii\", "prefPath", ".gcz*.iso", "consoleInfo", "/b /e %file", "2006"));
            Database.AddConsole(new Console("NDS", @"C:\UniCade\Emulators\NDS\DeSmuME.exe", @"C:\UniCade\ROMS\NDS\", "prefPath", ".nds", "consoleInfo", "%file", "2005"));
            Database.AddConsole(new Console("GBC", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBC\", "prefPath", ".gbc", "consoleInfo", "%file", "1998"));
            Database.AddConsole(new Console("MAME", @"C:\UniCade\Emulators\MAME\mame.bat", @"C:\UniCade\Emulators\MAME\roms\", "prefPath", ".zip", "consoleInfo", "", "1980")); //%file -skip_gameinfo -nowindow
            Database.AddConsole(new Console("PC", @"C:\Windows\explorer.exe", @"C:\UniCade\ROMS\PC\", "prefPath", ".lnk*.url", "consoleInfo", "%file", "1980"));
            Database.AddConsole(new Console("GBA", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBA\", "prefPath", ".gba", "consoleInfo", "%file", "2001"));
            Database.AddConsole(new Console("Gamecube", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Gamecube\", "prefPath", ".iso*.gcz", "consoleInfo", "/b /e %file", "2001"));
            Database.AddConsole(new Console("NES", @"C:\UniCade\Emulators\NES\Jnes.exe", @"C:\UniCade\ROMS\NES\", "prefPath", ".nes", "consoleInfo", "%file", "1983"));
            Database.AddConsole(new Console("SNES", @"C:\UniCade\Emulators\ZSNES\zsnesw.exe", @"C:\UniCade\ROMS\SNES\", "prefPath", ".smc", "consoleInfo", "%file", "1990"));
            Database.AddConsole(new Console("N64", @"C:\UniCade\Emulators\Project64\Project64.exe", @"C:\UniCade\ROMS\N64\", "prefPath", ".n64*.z64", "consoleInfo", "%file", "1996"));
            Database.AddConsole(new Console("PS1", @"C:\UniCade\Emulators\ePSXe\ePSXe.exe", @"C:\UniCade\ROMS\PS1\", "prefPath", ".iso*.bin*.img", "consoleInfo", "-nogui -loadbin %file", "1994"));
            Database.AddConsole(new Console("PS2", @"C:\UniCade\Emulators\PCSX2\pcsx2.exe", @"C:\UniCade\ROMS\PS2\", "prefPath", ".iso*.bin*.img", "consoleInfo", "%file", "2000"));
            Database.AddConsole(new Console("Atari 2600", @"C:\UniCade\Emulators\Stella\Stella.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", "consoleInfo", "file", "1977"));
            Database.AddConsole(new Console("Dreamcast", @"C:\UniCade\Emulators\NullDC\nullDC_Win32_Release-NoTrace.exe", @"C:\UniCade\ROMS\Dreamcast\", "prefPath", ".iso*.bin*.img", "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
            Database.AddConsole(new Console("PSP", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\PSP\", "prefPath", ".iso*.cso", "consoleInfo", "%file", "2005"));
            Database.AddConsole(new Console("Wii U", @"C:\UniCade\Emulators\WiiU\cemu.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", "consoleInfo", "file", "2012"));
            Database.AddConsole(new Console("Xbox 360", @"C:\UniCade\Emulators\X360\x360.exe", @"C:\UniCade\ROMS\X360\", "prefPath", ".iso*.bin*.img", "consoleInfo", "%file", "2005"));
            Database.AddConsole(new Console("PS3", @"C:\UniCade\Emulators\PS3\ps3.exe", @"C:\UniCade\ROMS\PS3\", "prefPath", ".iso", "consoleInfo", "%file", "2009"));
            Database.AddConsole(new Console("3DS", @"C:\UniCade\Emulators\PS3\3ds.exe", @"C:\UniCade\ROMS\3DS\", "prefPath", ".iso", "consoleInfo", "%file", "2014"));
        }

        /// <summary>
        /// Validate the integrity of the Media folder located in the current working directory
        /// </summary>
        public static bool VerifyMediaDirectory()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt);
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Consoles);
                return false;
            }
            if (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Media\Consoles").Length < 4)
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Consoles);
                return false;
            }
            if (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos").Length < 4)
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Consoles);
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles\Logos"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Consoles);
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Games"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Games);
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Backgrounds);
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Esrb);
                return false;
            }
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\Interface Background.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Marquee.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Icon.ico"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Backgrounds);
                return false;
            }

            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Everyone 10+.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Teen.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Mature.png") || !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Esrb\Adults Only (Ao).png"))
            {
                MessageBox.Show(Strings.MediaDirectoryCorrupt_Esrb);
                return false;
            }
            return true;
        }



        /// <summary>
        /// Restore default preferences. These updated preferences will take effect immediatly.
        /// NOTE: These changes are not automatically saved to the database file.
        /// </summary>
        public static void RestoreDefaultPreferences()
        {
            Database.RestoreDefaultUser();
            Program.ShowSplashScreen = false;
            Program.RescanOnStartup = false;
            Program.RestrictGlobalEsrbRatings = 0;
            Program.RequireLogin = false;
            Program.PerferCmdInterface = false;
            Program.ShowLoadingScreen = false;
            PayPerPlay.PayPerPlayEnabled = false;
            PayPerPlay.CoinsRequired = 1;
            PayPerPlay.Playtime = 15;
            Program.LaunchOptions = 0;
        }

        /// <summary>
        /// Preforms the initial file system operations when the program is launched
        /// </summary>
        public static void StartupScan()
        {
            //If preferences file does not exist, load default preference values and save a new file
            if (!LoadPreferences(Program.PreferencesPath))
            {
                RestoreDefaultPreferences();
                SavePreferences(Program.PreferencesPath);
                ShowNotification("WARNING", "Preference file not found.\n Loading defaults...");
            }


            //Verify the integrity of the local media directory and end the program if corruption is dectected  
            if (!VerifyMediaDirectory())
            {
                return;
            }

            //Verify the current user license and set flag
            if (LicenseEngine.ValidateSha256(LicenseEngine.UserLicenseName + LicenseEngine.HashKey, LicenseEngine.UserLicenseKey))
            {
                LicenseEngine.IsLicenseValid = true;
            }

            //If the database file does not exist in the specified location, load default values and rescan rom directories
            if (!LoadDatabase(Program.DatabasePath))
            {
                RestoreDefaultConsoles();
                ScanAllConsoles();
                try
                {
                    SaveDatabase(Program.DatabasePath);
                }
                catch
                {
                    MessageBox.Show(Strings.ErrorSavingDatabase + Program.DatabasePath);
                }
                ShowNotification("WARNING", "Database file not found.\n Loading defaults...");
            }
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

