using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using UniCadeAndroid.Constants;
using UniCadeAndroid.Exceptions;
using UniCadeAndroid.Interfaces;
using UniCadeAndroid.Windows;
using UniCadeAndroid.Objects;
using Console = UniCadeAndroid.Objects.Console;

namespace UniCadeAndroid.Backend
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
        /// <returns>false if the database file does not exist</returns>
        public static bool LoadDatabase(string path = ConstValues.DatabaseFileName)
        {
            //First check if the database file exists
            if (!File.Exists(path))
            {
                return false;
            }

            List<Console> consoleList;

            DataContractSerializer s = new DataContractSerializer(typeof(List<Console>));
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
               consoleList = (List<Console>)s.ReadObject(fs);
            }

            consoleList.ForEach(c => Database.AddConsole(c));

            return true;
        }

        /// <summary>
        /// Save the database to the specified path. Delete any preexisting database files
        /// </summary>
        public static bool SaveDatabase(string path = ConstValues.DatabaseFileName)
        {
            var consoleList = Database.GetConsoleList().Select(consoleName => (Console)Database.GetConsole(consoleName)).ToList();
            
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };

            DataContractSerializer s = new DataContractSerializer(typeof(List<Console>));
            using (var xmlWriter = XmlWriter.Create(path, xmlWriterSettings))
            {
                s.WriteObject(xmlWriter, consoleList);
            }
            
            return true;
        }


        /// <summary>
        /// Load preferences from the specified file path
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static bool LoadPreferences(string path = ConstValues.PreferencesFileName)
        {
            //First check if the database file exists
            if (!File.Exists(path))
            {
                return false;
            }

            CurrentSettings currentSettings;
            
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(CurrentSettings));
            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                currentSettings = (CurrentSettings)dataContractSerializer.ReadObject(fileStream);
            }

            Program.ShowSplashScreen = currentSettings.ShowSplashScreen;
            Program.RescanOnStartup = currentSettings.RescanOnStartup;
            Program.RestrictGlobalEsrbRatings = currentSettings.RestrictGlobalEsrbRatings;
            Program.UseModernEsrbLogos = currentSettings.UseModernEsrbLogos;
            Program.PerferCmdInterface = currentSettings.PerferCmdInterface;
            Program.ShowLoadingScreen = currentSettings.ShowLoadingScreen;
            Program.EnforceFileExtensions = currentSettings.EnforceFileExtensions;
            Program.UserLicenseKey = currentSettings.UserLicenseKey;
            Program.UserLicenseName = currentSettings.UserLicenseName;
            Program.PasswordProtection = currentSettings.PasswordProtection;
            currentSettings.UserList.ForEach(u => Database.AddUser(u));
            
            return true;

        }

        /// <summary>
        /// Save preferences file to the specified path
        /// </summary>
        public static bool SavePreferences(string path = ConstValues.PreferencesFileName)
        {

            var currentSettings = new CurrentSettings
            {
                ShowSplashScreen = Program.ShowSplashScreen,
                RescanOnStartup = Program.RescanOnStartup,
                RestrictGlobalEsrbRatings = Program.RestrictGlobalEsrbRatings,
                UseModernEsrbLogos = Program.UseModernEsrbLogos,
                PerferCmdInterface = Program.PerferCmdInterface,
                EnforceFileExtensions = Program.EnforceFileExtensions,
                ShowLoadingScreen = Program.ShowLoadingScreen,
   
                UserLicenseKey = Program.UserLicenseKey,
                UserLicenseName = Program.UserLicenseName,
                PasswordProtection = Program.PasswordProtection
            };

            foreach (string userName in Database.GetUserList())
            {
                currentSettings.UserList.Add((User)Database.GetUser(userName));
            }


            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };

            DataContractSerializer s = new DataContractSerializer(typeof(CurrentSettings));
            using (var xmlWriter = XmlWriter.Create(path, xmlWriterSettings))
            {
                //s.WriteObject(xmlWriter, currentSettings);
            }
            return true;
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
            string[] fileEntries = null;
            try
            {
                fileEntries = Directory.GetFiles(console.RomFolderPath);
            }
            catch
            {
                Trace.WriteLine("Directory Not Found");
            }

            //Add games to the current console object
            if (fileEntries != null)
            {
                foreach (string fileName in fileEntries)
                {
                    string gameTitle = fileName.Split('.')[0];
                    if (gameTitle != null)
                    {
                        if (console.GetGame(gameTitle) == null)
                        {
                            try
                            {
                                console.AddGame(new Game(Path.GetFileName(fileName), console.ConsoleName));
                            }
                            catch (ArgumentException exception)
                            {
                                Trace.WriteLine("Game cannot be added: " + exception.Message);
                            }
                        }
                    }
                }
            }

            //Delete nonexistent games
            foreach (string gameTitle in console.GetGameList())
            {
                if (fileEntries != null)
                {
                    if (!fileEntries.Contains(gameTitle))
                    {
                        console.RemoveGame(gameTitle);
                    }
                }
            }
            return true;
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
                if (game.EsrbRating > Database.GetCurrentUser().AllowedEsrbRatings)
                {
                    throw new LaunchException(("ESRB " + game.EsrbRating + " Is Restricted for" + Database.GetCurrentUser().Username));
                }
            }

            if (Program.RestrictGlobalEsrbRatings > 0)
            {
                if (game.EsrbRating > Program.RestrictGlobalEsrbRatings)
                {
                    throw new LaunchException(("ESRB " + game.EsrbRating + " Is Restricted globally"));
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
                //SendKeys.SendWait("^%{F4}");
                ShowNotification("UniCade System", "Attempting Force Close");
                MainWindow.IsGameRunning = false;
                Program.IsProcessActive = false;
                return;
            }
            if (Program.CurrentProcess.HasExited)
            {
                return;
            }

            Program.CurrentProcess.Kill();
            MainWindow.IsGameRunning = false;
            Program.IsProcessActive = false;
        }

        /// <summary>
        /// Restore the default consoles. These changes will take effect Immediately. 
        /// </summary>
        public static void RestoreDefaultConsoles()
        {
            Database.AddConsole(new Console("Sega Genesis", @"C:\UniCade\Emulators\Fusion\Fusion.exe", @"C:\UniCade\ROMS\Sega Genesis\", ".bin*.iso*.gen*.32x", "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
            Database.AddConsole(new Console("Nintendo Wii", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Nintendo Wii\", ".gcz*.iso", "consoleInfo", "/b /e %file", "2006"));
            Database.AddConsole(new Console("Nintendo DS", @"C:\UniCade\Emulators\NDS\DeSmuME.exe", @"C:\UniCade\ROMS\Nintendo DS\", ".nds", "consoleInfo", "%file", "2005"));
            Database.AddConsole(new Console("Nintendo GBC", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\Nintendo GBC\", ".gbc", "consoleInfo", "%file", "1998"));
            Database.AddConsole(new Console("MAME", @"C:\UniCade\Emulators\MAME\mame.bat", @"C:\UniCade\Emulators\MAME\roms\", ".zip", "consoleInfo", "", "1980")); //%file -skip_gameinfo -nowindow
            Database.AddConsole(new Console("PC", @"C:\Windows\explorer.exe", @"C:\UniCade\ROMS\PC\", ".lnk*.url", "consoleInfo", "%file", "1980"));
            Database.AddConsole(new Console("Nintendo GBA", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\Nintendo GBA\", ".gba", "consoleInfo", "%file", "2001"));
            Database.AddConsole(new Console("Nintendo Gamecube", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Nintendo Gamecube\", ".iso*.gcz", "consoleInfo", "/b /e %file", "2001"));
            Database.AddConsole(new Console("NES", @"C:\UniCade\Emulators\NES\Jnes.exe", @"C:\UniCade\ROMS\NES\", ".nes", "consoleInfo", "%file", "1983"));
            Database.AddConsole(new Console("SNES", @"C:\UniCade\Emulators\ZSNES\zsnesw.exe", @"C:\UniCade\ROMS\SNES\", ".smc", "consoleInfo", "%file", "1990"));
            Database.AddConsole(new Console("Nintendo N64", @"C:\UniCade\Emulators\Project64\Project64.exe", @"C:\UniCade\ROMS\Nintendo N64\", ".n64*.z64", "consoleInfo", "%file", "1996"));
            Database.AddConsole(new Console("Sony Playstation", @"C:\UniCade\Emulators\ePSXe\ePSXe.exe", @"C:\UniCade\ROMS\Sony Playstation\", ".iso*.bin*.img", "consoleInfo", "-nogui -loadbin %file", "1994"));
            Database.AddConsole(new Console("Sony Playstation 2", @"C:\UniCade\Emulators\PCSX2\pcsx2.exe", @"C:\UniCade\ROMS\Sony Playstation 2\", ".iso*.bin*.img", "consoleInfo", "%file", "2000"));
            Database.AddConsole(new Console("Atari 2600", @"C:\UniCade\Emulators\Stella\Stella.exe", @"C:\UniCade\ROMS\Atari 2600\", ".iso*.bin*.img", "consoleInfo", "file", "1977"));
            Database.AddConsole(new Console("Sega Dreamcast", @"C:\UniCade\Emulators\NullDC\nullDC_Win32_Release-NoTrace.exe", @"C:\UniCade\ROMS\Sega Dreamcast\", ".iso*.bin*.img", "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
            Database.AddConsole(new Console("Sony PSP", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\Sony PSP\", ".iso*.cso", "consoleInfo", "%file", "2005"));
            Database.AddConsole(new Console("Nintendo Wii U", @"C:\UniCade\Emulators\WiiU\cemu.exe", @"C:\UniCade\ROMS\Atari 2600\", ".iso*.bin*.img", "consoleInfo", "file", "2012"));
            Database.AddConsole(new Console("Nintendo 3DS", @"C:\UniCade\Emulators\PS3\3ds.exe", @"C:\UniCade\ROMS\Nintendo 3DS\", ".iso", "consoleInfo", "%file", "2014"));
        }

        /// <summary>
        /// Validate the integrity of the Media folder located in the current working directory
        /// </summary>
        public static bool VerifyMediaDirectoryIntegrity()
        {
            //Check for write privlages
            if (!Utilties.HasWriteAccessToFolder(Directory.GetCurrentDirectory()))
            {
                //MessageBox.Show(Strings.CurrentDirectoryWriteProtected);
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Media");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + ConstValues.ConsoleImagesPath))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + ConstValues.ConsoleImagesPath);
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + ConstValues.ConsoleLogoImagesPath))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + ConstValues.ConsoleLogoImagesPath);
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + ConstValues.GameImagesPath))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + ConstValues.GameImagesPath);
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
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Null;
            Program.PerferCmdInterface = false;
            Program.ShowLoadingScreen = false;
        }

        /// <summary>
        /// Preforms the initial file system operations when the program is launched
        /// </summary>
        public static bool StartupScan()
        {
            //Verify the integrity of the local media directory and end the program if corruption is dectected  
            if (!VerifyMediaDirectoryIntegrity())
            {
                return false;
            }

            //If preferences file does not exist, load default preference values and save a new file
            if (!LoadPreferences())
            {
                RestoreDefaultPreferences();
                SavePreferences();
                ShowNotification("WARNING", "Preference file not found.\n Loading defaults...");
            }

            //Verify the current user license and set flag
            Program.IsLicenseValid = CryptoEngine.ValidateLicense(Program.UserLicenseName, Program.UserLicenseKey);

            //If the database file does not exist in the specified location, load default values and rescan rom directories
            if (!LoadDatabase())
            {
                RestoreDefaultConsoles();
                ScanAllConsoles();
                try
                {
                    SaveDatabase();
                }
                catch
                {
                    //MessageBox.Show(Strings.ErrorSavingDatabase + Strings.TheInterfaceWillNowExit);
                    return false;
                }
            }
            //Generate folders within the Console directory
            foreach (string consoleName in Database.GetConsoleList())
            {
                string consoleDirectory = Directory.GetCurrentDirectory() + ConstValues.GameImagesPath + consoleName;
                if (!Directory.Exists(consoleDirectory))
                {
                    Directory.CreateDirectory(consoleDirectory);
                }
            }

            return true;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Display a timed popup notification in the lower right corner of the interface
        /// </summary>
        private static void ShowNotification(string title, string body)
        {
            //NotificationWindow notification = new NotificationWindow(title, body);
            //notification.Show();
        }

        #endregion
    }
}

