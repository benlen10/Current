using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCadeCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Database dat = new Database();
            bool loginStat = false;
            System.Console.WriteLine("Please enter username");
            string userName = System.Console.ReadLine();
            foreach(User u in dat.userList)
            {
                if (userName.Equals(u.getUsername()))
                {

                    System.Console.WriteLine("Please enter password");
                    if (System.Console.ReadLine().Equals(u.getPass()))
                    {
                        System.Console.WriteLine("Password Accepted");
                        loginStat = true;
                        break;
                    }
                }
                if (!loginStat)
                {
                    Environment.Exit(1);
                }
            }


        }

        //Methods

        public void displayConsoles()
        {

        }

    }
}
