using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace UniCadeCmd
{
    class Program
    {
        static Database dat;

        static void Main(string[] args)
        {
            dat = new Database();
            bool loginStat = false;

            dat.userList.Add(new User("Ben", "temp", 0, 0, " ", 20));
            loadDefaultConsoles();

            System.Console.WriteLine("Please enter username");
            string userName = System.Console.ReadLine();
            foreach (User u in dat.userList)
            {
                if (userName.Equals(u.getUsername()))
                {
                    while (!loginStat)
                    {
                        System.Console.WriteLine("Please enter password");
                        if (System.Console.ReadLine().Equals(u.getPass()))
                        {
                            System.Console.WriteLine("Password Accepted");
                            loginStat = true;
                            break;
                        }
                    }
                }
            }
                if (!loginStat)
                {
                    Environment.Exit(1);
                }
            while (true)
            {
                displayConsoles();
                
            }

        }

        //Methods

        public static void displayConsoles()
        {
            System.Console.WriteLine("Available Consoles:");
            string list = "";
            foreach (Console c in dat.consoleList)
            {
                list = list + " " + c.getName();
            }
            System.Console.WriteLine(list);
            string input = System.Console.ReadLine();
            foreach (Console c in dat.consoleList)
            {
                if (input.Equals(c.getName())){
                    displayGameList(c);
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
                if (input.Contains("(i)")){
                    System.Console.WriteLine("YAS");
                    string s = input.Substring(3);
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
            }

        }


        public static void displayGameInfo(Game g)
        {
            while (true)
            {
                System.Console.WriteLine("Title " + g.getTitle());
                System.Console.WriteLine("Release Date " + g.getReleaseDate());
                System.Console.WriteLine("Developer " + g.getDeveloper());
                System.Console.WriteLine("Publisher " + g.getPublisher());
                System.Console.WriteLine("Players " + g.getPlayers());
                System.Console.WriteLine("User Score " + g.getUserScore());
                System.Console.WriteLine("Critic Score " + g.getCriticScore());
                System.Console.WriteLine("ESRB Rating " + g.getEsrb());
                System.Console.WriteLine("ESRB Descriptor " + g.getEsrbDescriptor());
                System.Console.WriteLine("Game Description " + g.getDescription());

                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    return;
                }
            }

            }
        public static void displayConsoleInfo(Console c)
        {

        }


        public static void scan(string targetDirectory) { 
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach(string subdirectory in subdirectoryEntries)
            scanDirectory(subdirectory, targetDirectory);
    }

    public static void scanDirectory(string path, string directory)
        {
            bool foundCon = false;
            Console con = new Console();
            foreach(Console c in dat.consoleList)
            {
                if (c.getName().Equals(directory))
                {
                    con = c;
                    foundCon = true;
                    break;
                }
            }
            if (!foundCon)
            {
                System.Console.WriteLine("Console not found");
                return;
            }
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
            {
                con.getGameList().Add(new Game(fileName, con.getName(), 0));
                con.gameCount++;
            }
        }

        public static void loadDefaultConsoles()
        {
            Console c = new Console("GBA", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam");
            c.getGameList().Add(new Game("Final Fantasy II.gba", "GBA", 1));
            c.getGameList().Add(new Game("Super Metroid.gba", "GBA", 1));
            dat.consoleList.Add(c);
            dat.consoleList.Add(new Console("Gamecube", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
            dat.consoleList.Add(new Console("NES", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
            dat.consoleList.Add(new Console("SNES", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
            dat.consoleList.Add(new Console("N64", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
            dat.consoleList.Add(new Console("PS1", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
            dat.consoleList.Add(new Console("PS2", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
            dat.consoleList.Add(new Console("PSP", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));
        }




    }

    }

