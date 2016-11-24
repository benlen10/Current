using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UniCade
{
    public class User
    {
        private string userName;
        private string pass;
        public int loginCount;
        public int totalLaunchCount;
        private string userInfo;
        private string allowedEsrb;
        private string email;
        private string profPic;
        public ArrayList favorites;


        //Methods
        public User(string userName, string pass, int loginCount, string email, int totalLaunchCount, string userInfo, string allowedEsrb, string profPic)
        {
            this.userName = userName;
            this.pass = pass;
            this.loginCount = loginCount;
            this.totalLaunchCount = totalLaunchCount;
            this.userInfo = userInfo;
            this.allowedEsrb = allowedEsrb;
            this.email = email;
            this.profPic = profPic;
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

        public string getEmail()
        {
            return email;
        }

        public string getProfPic()
        {
            return profPic;
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

        public string getAllowedEsrb()
        {
            return allowedEsrb;
        }

        public void setName(string s)
        {
            userName = s;
        }

        public void setpass(string s)
        {
            pass = s;
        }

        public void setEmail(string s)
        {
            email = s;
        }

        public void setProfPic(string s)
        {
            profPic = s;
        }

        public void setUserInfo(string s)
        {
            userInfo = s;
        }

        public void setAllowedEsrb(string s)
        {
            allowedEsrb = s;
        }

    }

    }

