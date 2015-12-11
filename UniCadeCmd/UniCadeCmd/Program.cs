using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace UniCadeCmd
{
    class Program
    {
        static Database dat;
        public static string databasePath = @"C:\UniCade\Databse.txt";
        public static string romPath = @"C:\UniCade\ROMS";

        static void Main(string[] args)
        {
            dat = new Database();
            dat.userList.Add(new User("Ben", "temp", 0, 0, " ", 20));
            if (!loadDatabase(databasePath))
            {
                loadDefaultConsoles();
                scan(romPath);
                saveDatabase(databasePath);
            }
            
            login();
            displayConsoles(); 

        }

        //Methods

       public static void login()
        {
            while (true)
            {
                System.Console.WriteLine("Please enter username");
                string userName = System.Console.ReadLine();
                foreach (User u in dat.userList)
                {
                    if (userName.Equals(u.getUsername()))
                    {
                        while (true)
                        {
                            System.Console.WriteLine("Please enter password");
                            if (System.Console.ReadLine().Equals(u.getPass()))
                            {
                                System.Console.WriteLine("Password Accepted");
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static void displayConsoles()
        {
            while (true)
            {
                System.Console.WriteLine("Available Consoles:   [Exit: (c), Rescan (r):, Info: (i) <Console> ]");
                string list = "";
                foreach (Console c in dat.consoleList)
                {
                    list = list + " " + c.getName();
                }
                System.Console.WriteLine(list);
                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    saveDatabase(databasePath);
                    return;
                }
                else if (input.Contains("(r)"))
                {
                    scan(romPath);
                }
                else if (input.Contains("(i)"))
                {
                    foreach (Console c in dat.consoleList)
                    {
                        if (input.Contains(c.getName()))
                        {
                            displayConsoleInfo(c);
                        }
                    }
                }
                    foreach (Console c in dat.consoleList)
                {
                    if (input.Equals(c.getName()))
                    {
                        displayGameList(c);
                    }
                }
            }
        }

        public static void displayGameList(Console c)
        {
            while (true)
            {
                string text = string.Format("{0} (Total Games: {1})", c.getName(), c.gameCount);
                System.Console.WriteLine(text);
                System.Console.WriteLine("Additional Options:Info: (i) <game>, Close (c), Console Info (ci)\n");
                foreach (Game g in c.getGameList())
                {
                    System.Console.WriteLine(g.getTitle());
                }
                string input = System.Console.ReadLine();
                string s = input.Substring(3);
                if (input.Contains("(i)")){ 
                    foreach (Game g in c.getGameList())
                    {
                        if (s.Contains(g.getTitle()))
                        {
                            displayGameInfo(g);
                        }
                    }
                }
                else if (input.Equals("(ci)"))
                {
                    displayConsoleInfo(c);
                }
                else if (input.Equals("(c)")){
                    return;
                }
                //System.Console.WriteLine("YES\n\n\n\n\n");
                    foreach (Game g1 in c.getGameList())              //Launch Game
                    {
                        if (input.Contains(g1.getTitle()))
                        {
                        var m_command = new System.Diagnostics.Process();
                        string gamePath = ("\"" + c.getRomPath() + g1.getFileName() + "\"");
                        string args = c.getLaunchParam().Replace("%file", gamePath);
                        System.Console.WriteLine(args);
                        m_command.StartInfo.FileName = c.getEmuPath();
                        m_command.StartInfo.Arguments = args;
                        m_command.Start();
                     
                        //Process.Start(c.getEmuPath() + " "+ c.getRomPath()+ g1.getFileName());

                    }
                    }

                
            }

        }


        public static void displayGameInfo(Game g)
        {
            while (true)
            {
                System.Console.WriteLine("[Type (c) to close info window]\n");
                System.Console.WriteLine("Title: " + g.getTitle());
                System.Console.WriteLine("Release Date: " + g.getReleaseDate());
                System.Console.WriteLine("Developer: " + g.getDeveloper());
                System.Console.WriteLine("Publisher: " + g.getPublisher());
                System.Console.WriteLine("Players: " + g.getPlayers());
                System.Console.WriteLine("User Score: " + g.getUserScore());
                System.Console.WriteLine("Critic Score: " + g.getCriticScore());
                System.Console.WriteLine("ESRB Rating: " + g.getEsrb());
                System.Console.WriteLine("ESRB Descriptor: " + g.getEsrbDescriptor());
                System.Console.WriteLine("Game Description: " + g.getDescription());

                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    return;
                }
            }

            }
        public static void displayConsoleInfo(Console c)
        {
            while (true)
            {
                System.Console.WriteLine("[Type (c) to close info window]\n");
                System.Console.WriteLine("Console: " + c.getName());
                System.Console.WriteLine("Release Date: " + c.getReleaseDate());
                System.Console.WriteLine("Emulator Path: " + c.getEmuPath());
                System.Console.WriteLine("Rom Path: " + c.getRomPath());
                System.Console.WriteLine("Rom Extension: " + c.getRomExt());
                System.Console.WriteLine("Launch Param: " + c.getLaunchParam());
                System.Console.WriteLine("Console Info: " + c.getConsoleInfo());
                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    return;
                }
            }
        }


        public static void scan(string targetDirectory) { 
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach(string subdirectory in subdirectoryEntries)
            scanDirectory(subdirectory, targetDirectory);
    }

    public static void scanDirectory(string path, string directory)
        {
            string emuName = new DirectoryInfo(path).Name;
            bool foundCon = false;
            Console con = new Console();
            foreach(Console c in dat.consoleList)
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
                return;
            }
            string[] fileEntries = Directory.GetFiles(path);

            foreach (string fileName in fileEntries)
            {

                con.getGameList().Add(new Game(Path.GetFileName(fileName), con.getName(), 0));
                con.gameCount++;
            }
        }

        public static void loadDefaultConsoles()
        {
            dat.consoleList.Add(new Console("GBA", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C: \UniCade\ROMS\GBA\", "prefPath", "romExt", 0, "consoleInfo", "%file", "2001"));
            dat.consoleList.Add(new Console("Gamecube", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Gamecube\", "prefPath", "romExt", 0, "consoleInfo", "/b /e %file", "2001"));
            dat.consoleList.Add(new Console("NES", @"C:\UniCade\Emulators\NES\Jnes.exe", @"C:\UniCade\ROMS\NES\", "prefPath", "romExt", 0, "consoleInfo", "%file", "1983"));
            dat.consoleList.Add(new Console("SNES", @"C:\UniCade\Emulators\ZSNES\zsnesw.exe", @"C:\UniCade\ROMS\SNES\", "prefPath", "romExt", 0, "consoleInfo", "%file", "1990"));
            dat.consoleList.Add(new Console("N64", @"C:\UniCade\Emulators\Project64\Project64.exe", @"C:\UniCade\ROMS\N64\", "prefPath", "romExt", 0, "consoleInfo", "%file", "1996"));
            dat.consoleList.Add(new Console("PS1", @"C:\UniCade\Emulators\ePSXe\ePSXe.exe", @"C:\UniCade\ROMS\PS1\", "prefPath", "romExt", 0, "consoleInfo", "-nogui -loadbin %file", "1994"));
            dat.consoleList.Add(new Console("PS2", @"C:\UniCade\Emulators\PCSX2\pcsx2.exe", @"C:\UniCade\ROMS\PS2\", "prefPath", "romExt", 0, "consoleInfo", "%file", "2000"));
            dat.consoleList.Add(new Console("Atari 2600", @"C:\UniCade\Emulators\Stella\Stella.exe", @"C:\UniCade\ROMS\Atari 2600\", "prefPath", "romExt", 0, "consoleInfo", "file", "1977"));
            dat.consoleList.Add(new Console("Dreamcast", @"C:\UniCade\Emulators\NullDC\nullDC_Win32_Release-NoTrace.exe", @"C:\UniCade\ROMS\Dreamcast\", "prefPath", "romExt", 0, "consoleInfo", "-config ImageReader:defaultImage=%file", "1998"));
            dat.consoleList.Add(new Console("PSP", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\PSP\", "prefPath", "romExt", 0, "consoleInfo", "%file", "2005"));
            dat.consoleList.Add(new Console("Sega Genisis", @"C:\UniCade\Emulators\Fusion\Fusion.exe", @"C:\UniCade\ROMS\Sega Genisis\", "prefPath", "romExt", 0, "consoleInfo", "%file -gen -auto -fullscreen", "1990"));
            dat.consoleList.Add(new Console("Wii", @"C:\UniCade\Emulators\Dolphin\dolphin.exe", @"C:\UniCade\ROMS\Wii\", "prefPath", "romExt", 0, "consoleInfo", "/b /e %file", "2006"));
            dat.consoleList.Add(new Console("Nintendo DS", @"C:\UniCade\Emulators\NDS\DeSmuME.exe", @"C:\UniCade\ROMS\NDS\", "prefPath", "romExt", 0, "consoleInfo", "%file", "2005"));
            dat.consoleList.Add(new Console("GBC", @"C:\UniCade\Emulators\GBA\VisualBoyAdvance.exe", @"C: \UniCade\ROMS\GBC\", "prefPath", "romExt", 0, "consoleInfo", "%file", "1998"));
            //dat.consoleList.Add(new Console("SCUMMVM", @"C:\UniCade\Emulators\PPSSPP\PPSSPPWindows64.exe", @"C:\UniCade\ROMS\SCUMMVM\", "prefPath", "romExt", 0, "consoleInfo", "%file", "0000"));
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

                dat.consoleList.Add(new Console(r[0], r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[7], r[8]));
            }
            file.Close();
        }

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
            string[] tmp = { "tmp" };
            char[] sep = { '|' };
            string[] r = { " " };
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                r = line.Split(sep);
                //System.Console.WriteLine("Loop");
                if (line.Substring(0, 5).Contains("***"))
                {
                    if (conCount > 0) {
                        dat.consoleList.Add(c);
                            }
                    c = new Console(r[0].Substring(3), r[1], r[2], r[3], r[4], Int32.Parse(r[5]), r[6], r[7], r[8]);
                    conCount++;
                }
                else
                {
                    c.getGameList().Add(new Game(r[0], r[1], Int32.Parse(r[2]), r[3], r[4], r[5], r[6], r[7], r[8], r[9], r[10], r[11], r[12], r[13],tmp ,tmp ));
                    //System.Console.WriteLine(r[0]);
                }
            }
            file.Close();
            return true;
        }



        public static void saveDatabase(string path)
        {
            Console con = new Console();
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (Console c in dat.consoleList)
                {
                    string txt = string.Format("***{0}|{1}|{2}|{3}|{4}|{5}|{7}|{8}|", c.getName(), c.getEmuPath(), c.getRomPath(), c.getPrefPath(), c.getRomExt(), c.gameCount, c.getConsoleInfo(), c.getlaunchParam(), c.getReleaseDate());
                    sw.WriteLine(txt);
                    foreach (Game g in c.getGameList())
                    {
                         txt = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{7}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|", g.getFileName(), g.getConsole(), g.launchCount, g.getReleaseDate(), g.getPublisher(), g.getDeveloper(), g.getUserScore(), g.getCriticScore(), g.getPlayers(), g.getTrivia(), g.getEsrb(), g.getEsrbDescriptor(),g.getEsrbSummary(), g.getDescription(), g.getGenres(), g.getTags());
                        sw.WriteLine(txt);

                    }
                }

            }
        }

    }

    }