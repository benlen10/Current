using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCadeCmd
{
    class User
    {
        private string userName;
        private string pass;
        private int loginCount;
        private int totalLaunchCount;
        private string userInfo;
        private int age;


        //Methods
        public User(string userName, string pass, int loginCount, int totalLaunchCount, string userInfo, int age)
        {
            this.userName = userName;
            this.pass = pass;
            this.loginCount = loginCount;
            this.totalLaunchCount = totalLaunchCount;
            this.userInfo = userInfo;
            this.age = age;
        }

        public string getUsername()
        {
            return userName;
        }

        public string getPass()
        {
            return pass;
        }


    }

    }

