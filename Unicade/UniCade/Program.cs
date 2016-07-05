using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace UniCadeCmd
{
    class Program
    {
        public static Database dat;
        public static string databasePath = @"C:\UniCade\Databse.txt";
        public static string romPath = @"C:\UniCade\ROMS";
        public static string mediaPath = @"C:\UniCade\Media";
        public static string emuPath = @"C:\UniCade\Emulators";
        public static string prefPath = @"C:\UniCade\Preferences.txt";
        public static User curUser;
        public static int coins = 0;
        public static bool playtimeRemaining = true;
        public static GUI gui;
        public static string userLicenseName;
        public static string userLicenseKey;
        public static bool validLicense;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);



            dat = new Database();
            
            if (!FileOps.loadPreferences(prefPath))
            {
                
                
                FileOps.defaultPreferences();
                System.Console.WriteLine("Preference file not found. Loading default values");
            }

            if(!ValidateSHA256(userLicenseName + Database.getHashKey(), userLicenseKey))
            {
                MessageBox.Show("Invalid License Key");
                LicenseEntry le = new LicenseEntry();

                    le.ShowDialog();
                if (!validLicense)
                {
                    return;
                }
            }

            if (!FileOps.loadDatabase(databasePath))
            {
                FileOps.loadDefaultConsoles();
                FileOps.scan(romPath);
                FileOps.saveDatabase(databasePath);
            }

            if (curUser == null)
            {
                curUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", "","");
                dat.userList.Add(curUser);
            }

            if (SettingsWindow.requireLogin == 1)
            {
                login();
            }
            if (SettingsWindow.cmdOrGui < 1)
            {
                gui = new GUI();
                gui.ShowDialog();
            }
            displayConsoles(); 

        }

        //Methods

       public static void login()
        {
            while (true)
            {
                System.Console.WriteLine("Please enter username (Type x to exit)");
                string userName = System.Console.ReadLine();
                if (userName.Equals("x"))
                {
                    return;
                }
                foreach (User u in dat.userList)
                {
                    if (userName.Equals(u.getUsername()))
                    {
                        while (true)
                        {
                            string ps = System.Console.ReadLine();
                            System.Console.WriteLine("Please enter password");
                            if (ps.Equals("x"))
                            {
                                return;
                            }
                            if (ps.Equals(u.getPass()))
                            {
                                System.Console.WriteLine("Password Accepted");
                                curUser = u;
                                curUser.loginCount++;
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
                System.Console.WriteLine("{"+ curUser.getUsername()+ "} Available Consoles:   [Exit: (c), Rescan (r):, Info: (i), GUI (g), Switch User (u), (s) Settings, (uf) User Favs (d) Download Info <Console>");
                string list = "";
                foreach (Console c in dat.consoleList)
                {
                    list = list + " " + "["+ c.getName()  + "]";
                }
                System.Console.WriteLine(list);

                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    FileOps.saveDatabase(databasePath);
                    FileOps.savePreferences(prefPath);
                    return;
                }
                else if (input.Contains("(r)"))
                {
                    FileOps.scan(romPath);
                }

                else if (input.Equals("(uf)"))
                {
                    displayUserFavs();
                }


                else if (input.Contains("(s)"))
                {
                    if (SettingsWindow.passProtect>0)
                    {
                        System.Console.WriteLine("Enter Password");
                        string inp = System.Console.ReadLine();
                        int n;
                        Int32.TryParse(inp, out n);
                        if (n>0) {
                            if (Int32.Parse(inp).Equals(SettingsWindow.passProtect))
                            {
                                SettingsWindow sw = new SettingsWindow();
                                sw.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        SettingsWindow sw = new SettingsWindow();

                       sw.ShowDialog();
                    }
                }
                else if (input.Contains("(g)"))
                {
                    GUI gui = new GUI();
                    gui.ShowDialog();
                }
                else if (input.Contains("(u)"))
                {
                    login();
                }
                else if (input.Contains("(d)"))
                {

                    foreach (Console c in dat.consoleList)
                    {
                        if (input.Contains(c.getName()))
                        {
                            foreach (Game g in c.getGameList())
                            {
                                WebOps.scrapeInfo(g);
                            }
                        }
                    }
                    
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
            bool fav = false;
            while (true)
            {
                
                string text = string.Format("{0} (Total Games: {1})", c.getName(), c.gameCount);
                System.Console.WriteLine(text);
                System.Console.WriteLine("Additional Options:Info: (i) <game>, Close (c),Global Fav (gf), (uf) Add User Fav, Display Favorites (f) Console Info (ci)\n");

                //Display Game List
                foreach (Game g in c.getGameList())
                {
                    if (fav)
                    {
                        if (g.getFav() == 1)
                        {
                            System.Console.WriteLine(g.getTitle());
                        }
                    }
                    else if (SettingsWindow.viewEsrb > 1)
                    {
                        if (SettingsWindow.calcEsrb(g.getEsrb()) <= SettingsWindow.restrictESRB)
                        {
                            System.Console.WriteLine(g.getTitle());
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(g.getTitle());
                    }

                }


                string input = System.Console.ReadLine();
                string s = input.Substring(3);
                if (input.Contains("(i)"))
                {
                    foreach (Game g in c.getGameList())
                    {
                        if (s.Contains(g.getTitle()))
                        {
                            System.Console.Write(displayGameInfo(g));
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
                    displayConsoleInfo(c);
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
                    curUser.favorites.Add(c.getName() + "*" + input.Substring(5));
                    System.Console.WriteLine("\n***Added to User Favorites***\n");
                }
                else if (input.Contains("(gf)"))
                {
                    foreach (Game g in c.getGameList())
                    {
                        if (input.Substring(4).Contains(g.getTitle()))
                        {
                            if (g.getFav() < 1)
                            {
                                g.setFav(1);
                                System.Console.WriteLine("\n***Added to Favorites***\n");
                            }
                            else
                            {
                                g.setFav(0);
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
                    //System.Console.WriteLine("YES\n\n\n\n\n");
                    foreach (Game g1 in c.getGameList())
                    {
                        if (input.Contains(g1.getTitle()))
                        {
                            FileOps.launch(g1, c);

                        }
                    }
                }
            }

        }

        public static void displayUserFavs()
        {
            string str = "";
            foreach (string s in curUser.favorites)
            {
                string[] st = s.Split('*');
                if (st.Length > 1)
                {
                    str += (st[0] + " " + st[1]+"\n");
                }
            }
            while (true)
            {
                System.Console.WriteLine("[Type (c) to close info window]\n");
                System.Console.WriteLine(str);
                string input = System.Console.ReadLine();
                if (input.Equals("(c)"))
                {
                    return;
                }
                //Check for matching input string to launch
                foreach (string s in curUser.favorites)
                {
                    string[] st = s.Split('*');
                    if (st.Length > 1)
                    {
                        str = (st[0] + " " + st[1] + "\n");
                        if (str.Contains(input))
                        {
                            foreach(Console c in Program.dat.consoleList)
                            {
                                if (c.getName().Equals(st[0]))
                                {
                                    foreach (Game g in c.getGameList()){
                                        if (st[1].Contains(g.getTitle())){
                                            FileOps.launch(g, c);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


       public static string displayGameInfo(Game g)
        {
            string txt = "";

                txt = txt +("[Type (c) to close info window]\n");
                txt = txt + ("\nTitle: " + g.getTitle() +"\n");
                txt = txt + ("\nRelease Date: " + g.getReleaseDate() + "\n");
                txt = txt + ("\nDeveloper: " + g.getDeveloper() + "\n");
                txt = txt + ("\nPublisher: " + g.getPublisher() + "\n");
                txt = txt + ("\nPlayers: " + g.getPlayers() + "\n");
                txt = txt + ("\nUser Score: " + g.getUserScore() + "\n");
                txt = txt + ("\nCritic Score: " + g.getCriticScore() + "\n");
                txt = txt + ("\nESRB Rating: " + g.getEsrb() + "\n");
                txt = txt + ("\nESRB Descriptors: " + g.getEsrbDescriptor() + "\n");
                txt = txt + ("\nmame Description: " + g.getDescription() + "\n");
                return txt;
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

        static void OnProcessExit(object sender, EventArgs e)
        {
            Taskbar.Show();
        }
        protected virtual void OnClosing() {
            Taskbar.Show();
        }

        public static string SHA256Hash(string data)
        {
            if(data == null)
            {
                return null;
            }
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }


        public static bool ValidateSHA256(string input, string storedHashData)
        {
            string getHashInputData = SHA256Hash(input);
            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }





    }

    }