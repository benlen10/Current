using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UniCadeCmd
{
    class User
    {
        private string userName;
        private string pass;
        public int loginCount;
        public int totalLaunchCount;
        private string userInfo;
        private int age;
        public ArrayList favorites;


        //Methods
        public User(string userName, string pass, int loginCount, int totalLaunchCount, string userInfo, int age)
        {
            this.userName = userName;
            this.pass = pass;
            this.loginCount = loginCount;
            this.totalLaunchCount = totalLaunchCount;
            this.userInfo = userInfo;
            this.age = age;
            favorites = new ArrayList();
        }

        public User()
        {
            this.userName = "New User";
        }

        public string getUsername()
        {
            return userName;
        }

        public string getPass()
        {
            return pass;
        }

        public int getLoginCount()
        {
            return loginCount;
        }

        public int getLaunchCount()
        {
            return totalLaunchCount;
        }

        public string getUserInfo()
        {
            return userInfo;
        }

        public int getAge()
        {
            return age;
        }

        public void setName(string s)
        {
            userName = s;
        }

        public void setpass(string s)
        {
            pass = s;
        }

        public void setUserInfo(string s)
        {
            userInfo = s;
        }

        public void seAge(int s)
        {
            age = s;
        }

    }

    }

