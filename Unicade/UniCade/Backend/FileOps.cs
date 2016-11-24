using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace UniCade
{
    class FileOps
    {
        public static bool processActive;
        public static System.Diagnostics.Process proc;
        public static bool urlLaunch;


        public static bool loadDatabase(string path)

        {
            if (!File.Exists(path))
            {
                System.Console.WriteLine("Database file does not exist");
                return false;
            }
            string line;
            int conCount = 0;
            Console c = new Console();
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                //System.Console.WriteLine("Loop");
                if (line.Substring(0, 5).Contains("***"))
                {
                    if (conCount > 0)
                    {
                        Program._database.consoleList.Add(c);
                    }
                    c = new Console(r[0].Substring(3), r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[7], r[8]);
                    conCount++;
                }
                else
                {
                    c.getGameList().Add(new Game(r[0], r[1], Int32.Parse(r[2]), r[3], r[4], r[5], r[6], r[7], r[8], r[9], r[10], r[11], r[12], r[13], r[14], r[15], Int32.Parse(r[16])));
                    //System.Console.WriteLine(r[0]);
                }
            }
            if (conCount > 0)
            {
                Program._database.consoleList.Add(c);
            }

            if (conCount < 1)
            {
                MessageBox.Show("Fatal Error: Database File is corrupt");
                return false;
            }
            file.Close();
            return true;
        }



        public static void
            saveDatabase(string path)
        {
            Console con = new Console();
            if (File.Exists(path))
            {
                File.Delete(path);
            }


            try
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (Console c in Program._database.consoleList)
                    {
                        string txt = string.Format("***{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|", c.getName(), c.getEmuPath(), c.getRomPath(), c.getPrefPath(), c.getRomExt(), c.gameCount, "Console Info", c.getLaunchParam(), c.getReleaseDate());
                        sw.WriteLine(txt);
                        if (c.gameCount > 0)
                        {
                            foreach (Game g in c.getGameList())
                            {
                                txt = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}", g.getFileName(), g.getConsole(), g.launchCount, g.getReleaseDate(), g.getPublisher(), g.getDeveloper(), g.getUserScore(), g.getCriticScore(), g.getPlayers(), "Trivia", g.getEsrb(), g.getEsrbDescriptor(), g.getEsrbSummary(), g.getDescription(), g.getGenres(), g.getTags(), g.getFav());
                                sw.WriteLine(txt);

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



        public static bool loadPreferences(String path)
        {
            if (!File.Exists(path))
            {
                System.Console.WriteLine("Database file does not exist");
                return false;
            }


            string[] tmp = { "tmp" };
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(path);
            string line = file.ReadLine();


            r = line.Split(sep);
            String currentUser = r[1];
            
            line = file.ReadLine();
            r = line.Split(sep);
            Program._databasePath = r[1];

            line = file.ReadLine();
            r = line.Split(sep);
            Program._emuPath = r[1];

            line = file.ReadLine();
            r = line.Split(sep);
            Program._mediaPath = r[1];

            line = file.ReadLine();
            r = line.Split(sep);
            if (r[1].Contains("1"))
            {
                SettingsWindow.showSplash = 1;
            }
            else
            {
                SettingsWindow.showSplash = 0;
            }

            line = file.ReadLine();
            r = line.Split(sep);
            if ((r[1].Contains("1")))
            {
                SettingsWindow.scanOnStartup = 1;
            }
            else
            {
                SettingsWindow.scanOnStartup = 0;
            }

            line = file.ReadLine();
            r = line.Split(sep);
            SettingsWindow.restrictESRB = Int32.Parse(r[1]);

            file.ReadLine();
            r = line.Split(sep);
            if (r[1].Contains("1"))
            {
                SettingsWindow.requireLogin = 1;
            }
            else
            {
                SettingsWindow.requireLogin = 0;
            }

            line = file.ReadLine();
            r = line.Split(sep);
            if (r[1].Contains("1"))
            {
                SettingsWindow.viewEsrb = 1;
            }
            else
            {
                SettingsWindow.viewEsrb = 0;
            }

            line = file.ReadLine();
            r = line.Split(sep);
            if (r[1].Contains("1"))
            {
                SettingsWindow.showLoading = 1;
            }
            else
            {
                SettingsWindow.showLoading = 0;
            }

            line = file.ReadLine();
            r = line.Split(sep);
            if (r[1].Contains("1"))
            {
                SettingsWindow.payPerPlay = 1;
            }
            else
            {
                SettingsWindow.payPerPlay = 0;
            }

            if (r[2].Contains("1"))
            {
                SettingsWindow.perLaunch = 1;
            }
            else
            {
                SettingsWindow.perLaunch = 0;
            }
            SettingsWindow.coins = Int32.Parse(r[3]);
            SettingsWindow.playtime = Int32.Parse(r[4]);

            line = file.ReadLine();    //Parse License Key
            r = line.Split(sep);
            Program._userLicenseName = r[1];
            Program._userLicenseKey = r[2];

            

            file.ReadLine(); //Skip ***Users*** line

            //Parse user data
            while ((line = file.ReadLine()) != null)
            {
                
               
            
                r = line.Split(sep);

                User u = new User(r[0], r[1], Int32.Parse(r[2]), r[3], Int32.Parse(r[4]), r[5], r[6], "null");
                if (r[6].Length > 0)
                {
                    string[] st = r[7].Split('#');
                    String st1 = "";
                    int i = 1;

                    foreach (string s in st)
                    {
                        
                        if ((i % 2 == 0)&&(i>1))
                        {
                            u.favorites.Add(new Game(st1, s, 0));

                        }
                        st1 = s + ".zip";
                        i++;
                    }
                }
                Program._database.userList.Add(u);

            }
            foreach (User u in Program._database.userList)
            {
                if (u.getUsername().Equals(currentUser))   //Set curUser to default user
                {
                    SettingsWindow.curUser = u;
                    //System.Console.WriteLine("Current user change to " + u.getUsername());
                }
            }

            file.Close();
            return true;
        }

        public static void savePreferences(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            foreach (User us in Program._database.userList)
            {
                if (SettingsWindow.curUser.getUsername().Equals(us.getUsername()))
                {
                    Program._database.userList.Remove(us);
                    Program._database.userList.Add(SettingsWindow.curUser);
                    break;

                }
            }

            using (StreamWriter sw = File.CreateText(path))
            {

                sw.WriteLine("CurrentUser|" + SettingsWindow.curUser.getUsername());
                sw.WriteLine("DatabasePath|" + Program._databasePath);
                sw.WriteLine("EmulatorFolderPath|" + Program._emuPath);
                sw.WriteLine("MediaFolderPath|" + Program._mediaPath);
                sw.WriteLine("ShowSplash|" + SettingsWindow.showSplash);
                sw.WriteLine("ScanOnStartup|" + SettingsWindow.scanOnStartup);
                sw.WriteLine("RestrictESRB|" + SettingsWindow.restrictESRB);
                sw.WriteLine("RequireLogin|" + SettingsWindow.requireLogin);
                sw.WriteLine("CmdOrGui|" + SettingsWindow.cmdOrGui);
                //sw.WriteLine("KeyBindings|" + SettingsWindow.defaultUser);
                sw.WriteLine("LoadingScreen|" + SettingsWindow.showLoading);
                sw.WriteLine("PaySettings|" + SettingsWindow.payPerPlay + "|" + SettingsWindow.perLaunch + "|" + SettingsWindow.coins + "|" + SettingsWindow.playtime);
                sw.WriteLine("License Key|" + Program._userLicenseName + "|" + Program._userLicenseKey);
                sw.WriteLine("***UserData***");
                foreach (User u in Program._database.userList)
                {
                    string favs = "";
                        foreach(Game g in u.favorites)
                    {
                        favs += (g.getTitle() + "#" + g.getConsole()+ "#");
                    }
                    sw.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|", u.getUsername(), u.getPass(), u.getLoginCount(), u.getEmail(), u.getLaunchCount(), u.getUserInfo(), u.getAllowedEsrb(), favs);
                }
            }

        }



        public static bool scan(string targetDirectory)
        {
            string[] subdirectoryEntries = null;
            try
            {
             subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            }catch
            {
                MessageBox.Show("Directory Not Found: " + targetDirectory);
                return false;
            }
            foreach (string subdirectory in subdirectoryEntries)
                scanDirectory(subdirectory, targetDirectory);
            return true;
        }

        public static bool scanDirectory(string path, string directory)  //Scan console
        {
            string emuName = new DirectoryInfo(path).Name;
            bool foundCon = false;
            string[] ex;
            bool dup = false;


            Console con = new Console();
            foreach (Console c in Program._database.consoleList)
            {
                if (c.getName().Equals(emuName))
                {
                    con = c;
                    foundCon = true;
                    break;
                }
            }
            if (!foundCon)
            {
                //System.Console.WriteLine("Console not found");
                return false;
            }
            string[] fileEntries = null;
            string[] exs = con.getRomExt().Split('*');
            try { 
                fileEntries = Directory.GetFiles(path);
            
        }catch
            {
                MessageBox.Show("Directory Not Found: " + path);
                return false;
            }
            foreach (string fileName in fileEntries)
            {
                if (SettingsWindow.enforceExt > 0)
                {
                    ex = fileName.Split('.');
                    foreach (string s in exs)
                    {
                        if (ex[1].Equals(s))
                        {
                            dup = false;
                            foreach (Game g in con.getGameList())
                            {
                                if (g.getTitle().Equals(Path.GetFileName(fileName)))
                                {
                                    dup = true;
                                    break;
                                }
                            }
                            if (!dup)
                            {
                                con.getGameList().Add(new Game(Path.GetFileName(fileName), con.getName(), 0));
                                con.gameCount++;
                            }
                        }
                    }
                }
                else
                {
                    dup = false;
                    foreach (Game g in con.getGameList())
                    {
                        if (g.getTitle().Equals(fileName.Split('.')[0]))
                        {
                            dup = true;
                            break;
                        }
                    }
                    if (!dup)
                    {
                        con.getGameList().Add(new Game(Path.GetFileName(fileName), con.getName(), 0));
                        con.gameCount++;
                    }
                }
            }
            //Delete nonexistent games
            bool found = false;
            Game foundGame = null;
            foreach (Game g in con.getGameList())
            {
                found = false;
                foreach (string fileName in fileEntries)
                {
                    if (g.getTitle().Equals(Path.GetFileName(fileName)))
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    con.getGameList().Remove(foundGame);
                    found = false;
                    foundGame = null;
                }
            }
            refreshGameCount();
            return true;
        }





        public static void loadConsoles()
        {
            string line;
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(@"C:\UniCade\consoleList.txt");
            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                //Console.WriteLine("Length: " + r.Length);

                Program._database.consoleList.Add(new Console(r[0], r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[8], " "));
            }
            file.Close();
        }



       

        public static void launch(Game g, Console c)
        {

            if (SettingsWindow.curUser.getAllowedEsrb().Length > 1)
            {
                int EsrbNum = SettingsWindow.calcEsrb(g.getEsrb());
                if (EsrbNum >= SettingsWindow.calcEsrb(SettingsWindow.curUser.getAllowedEsrb()))
                {
                    NotificationWindow nfw2 = new NotificationWindow("NOTICE", "ESRB " + g.getEsrb() + " Is Restricted for" + SettingsWindow.curUser.getUsername());
                    nfw2.Show();
                    return;
                }
            }
            else if(SettingsWindow.restrictESRB > 0)
            {
                int EsrbNum = SettingsWindow.calcEsrb(g.getEsrb());
                if (EsrbNum >= SettingsWindow.restrictESRB)
                {
                    NotificationWindow nfw1 = new NotificationWindow("NOTICE" , "ESRB " + g.getEsrb() + " Is Restricted\n");
                    nfw1.Show();
                    return;
                }

            }
            

          
            g.launchCount++;
            SettingsWindow.curUser.totalLaunchCount++;
            proc = new System.Diagnostics.Process();
            string gamePath = ("\"" + c.getRomPath() + g.getFileName() + "\"");
            string testGamePath = ( c.getRomPath() + g.getFileName());
            if (!File.Exists(testGamePath))
            {
                NotificationWindow nfw1 = new NotificationWindow("System", "ROM does not exist. Launch Failed");
                nfw1.Show();
                return;
            }
            string args = "";
            if (c.getName().Equals("MAME"))
            {
                 args = c.getLaunchParam().Replace("%file", g.getTitle());
                System.Console.WriteLine("MAME Launch: " + args);
            }
            else
            {
                 args = c.getLaunchParam().Replace("%file", gamePath);
            }


                proc.EnableRaisingEvents = true;
                proc.Exited += new EventHandler(proc_Exited);
            if (c.getName().Equals("PC"))
            {
                proc.StartInfo.FileName =  args ;
                if (args.Contains("url"))
                {
                    urlLaunch = true;
                }
                
            }
            else
            {
                if (!File.Exists(c.getEmuPath())){
                    NotificationWindow nfw1 = new NotificationWindow("System", "Emulator does not exist. Launch Failed");
                    nfw1.Show();
                    return;
                }
                proc.StartInfo.FileName = c.getEmuPath();
                proc.StartInfo.Arguments = args;
            }
            MainWindow.unhookKeys();
            NotificationWindow nfw = new NotificationWindow("System", "Loading ROM File");
            nfw.Show();
            proc.Start();


            processActive = true;
            MainWindow.gameRunning = true;
          
                

        }

        private static void proc_Exited(object sender, System.EventArgs e)
        {
            MainWindow.gameRunning = false;
            processActive = false;
        }


        public static void killProcess()
        {

            if (urlLaunch)
            {
                //System.Windows.unForms.SendKeys.SendWait("%{F4}");
                //Process[] steam = Process.GetProcessesByName("Steam");
                //team[0].Kill();
                System.Windows.Forms.SendKeys.SendWait("^%{F4}");
                NotificationWindow nfw2 = new NotificationWindow("UniCade System" , "Attempting Force Close");
                nfw2.Show();

                MainWindow.gameRunning = false;
                processActive = false;
                MainWindow.hookKeys();
                return;
            }
            else if (proc.HasExited)
            {
                return;
            }
                
            
            proc.Kill();
            MainWindow.hookKeys();
            MainWindow.gameRunning = false;
            processActive = false;
        }



        public static void loadDefaultConsoles()
        {
            Program._database.consoleList.Add(new Console("Sega Genisis", @"C:\UniCade\Emulators\Fusion\Fusion.exe", @"C:\UniCade\ROMS\Sega Genisis\", "prefPath", ".bin*.iso*.gen*.32x", 0, "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
            Program._database.consoleList.Add(new Console("Wii", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Wii\", "prefPath", ".gcz*.iso", 0, "consoleInfo", "/b /e %file", "2006"));
            Program._database.consoleList.Add(new Console("NDS", @"C:\UniCade\Emulators\NDS\DeSmuME.exe", @"C:\UniCade\ROMS\NDS\", "prefPath", ".nds", 0, "consoleInfo", "%file", "2005"));
            Program._database.consoleList.Add(new Console("GBC", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBC\", "prefPath", ".gbc", 0, "consoleInfo", "%file", "1998"));
            Program._database.consoleList.Add(new Console("MAME", @"C:\UniCade\Emulators\MAME\mame.bat", @"C:\UniCade\Emulators\MAME\roms\", "prefPath", ".zip", 0, "consoleInfo", "", "1980")); //%file -skip_gameinfo -nowindow
            Program._database.consoleList.Add(new Console("PC", @"C:\Windows\explorer.exe", @"C:\UniCade\ROMS\PC\", "prefPath", ".lnk*.url", 0, "consoleInfo", "%file", "1980"));

            
            Program._database.consoleList.Add(new Console("GBA", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C:\UniCade\ROMS\GBA\", "prefPath", ".gba", 0, "consoleInfo", "%file", "2001"));
            Program._database.consoleList.Add(new Console("Gamecube", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Gamecube\", "prefPath", ".iso*.gcz", 0, "consoleInfo", "/b /e %file", "2001"));
            Program._database.consoleList.Add(new Console("NES", @"C:\UniCade\Emulators\NES\Jnes.exe", @"C:\UniCade\ROMS\NES\", "prefPath", ".nes", 0, "consoleInfo", "%file", "1983"));
            Program._database.consoleList.Add(new Console("SNES", @"C:\UniCade\Emulators\ZSNES\zsnesw.exe", @"C:\UniCade\ROMS\SNES\", "prefPath", ".smc", 0, "consoleInfo", "%file", "1990"));
            Program._database.consoleList.Add(new Console("N64", @"C:\UniCade\Emulators\Project64\Project64.exe", @"C:\UniCade\ROMS\N64\", "prefPath", ".n64*.z64", 0, "consoleInfo", "%file", "1996"));
            Program._database.consoleList.Add(new Console("PS1", @"C:\UniCade\Emulators\ePSXe\ePSXe.exe", @"C:\UniCade\ROMS\PS1\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-nogui -loadbin %file", "1994"));
            Program._database.consoleList.Add(new Console("PS2", @"C:\UniCade\Emulators\PCSX2\pcsx2.exe", @"C:\UniCade\ROMS\PS2\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2000"));
            Program._database.consoleList.Add(new Console("Atari 2600", @"C:\UniCade\Emulators\Stella\Stella.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "1977"));
            Program._database.consoleList.Add(new Console("Dreamcast", @"C:\UniCade\Emulators\NullDC\nullDC_Win32_Release-NoTrace.exe", @"C:\UniCade\ROMS\Dreamcast\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
            Program._database.consoleList.Add(new Console("PSP", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\PSP\", "prefPath", ".iso*.cso", 0, "consoleInfo", "%file", "2005"));
            Program._database.consoleList.Add(new Console("Wii U", @"C:\UniCade\Emulators\WiiU\cemu.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "file", "2012"));
            Program._database.consoleList.Add(new Console("Xbox 360", @"C:\UniCade\Emulators\X360\x360.exe", @"C:\UniCade\ROMS\X360\", "prefPath", ".iso*.bin*.img", 0, "consoleInfo", "%file", "2005"));
            Program._database.consoleList.Add(new Console("PS3", @"C:\UniCade\Emulators\PS3\ps3.exe", @"C:\UniCade\ROMS\PS3\", "prefPath", ".iso", 0, "consoleInfo", "%file", "2009"));
            Program._database.consoleList.Add(new Console("3DS", @"C:\UniCade\Emulators\PS3\3ds.exe", @"C:\UniCade\ROMS\3DS\", "prefPath", ".iso", 0, "consoleInfo", "%file", "2014"));


        }

        public static void refreshGameCount()
        {
            Database.totalGameCount = 0; ;

            foreach (Console c in Program._database.consoleList)
            {

                foreach (Game g in c.getGameList())
                {

                    Database.totalGameCount++;
                }
            }
            
        }

        public static bool VerifyMediaDirectory()
        {
            if(!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media"))
            {
                MessageBox.Show("Media directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Media\Consoles"))
            {
                MessageBox.Show("Media (Consoles) directory does not exist. Please reinstall or download UniCade Media Package to the current working directory");
                return false;
            }
            if (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Media\Consoles").Length<4)
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
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Logo.png")|| !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\Interface Background.png")|| !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Marquee.png")|| !File.Exists(Directory.GetCurrentDirectory() + @"\Media\Backgrounds\UniCade Icon.ico"))
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

        public static void createNewROMdirectory()
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

        public static void createNewEmudirectory()
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

        public static void defaultPreferences()
        {

            SettingsWindow.curUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", "", "");
            Program._database.userList.Add(SettingsWindow.curUser);
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
    }
}
