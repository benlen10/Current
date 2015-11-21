using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            dat.consoleList.Add(new Console("GBA", "emuPath", "romPath", "prefPath", "romExt", 0, "consoleInfo", "launchParam"));

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
            string text = string.Format("{0} (Total Games: {1})",c.getName(), 0);
            System.Console.WriteLine(text);
        }


    }

    }

