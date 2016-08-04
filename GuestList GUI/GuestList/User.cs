using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestList
{
    public class User
    {
        private string firstName;
        private string lastName;
        private string birthday;
        private bool blacklist;
        private int priority;
        private int status;


        public User(string firstName, string lastName, string birthday, int priority)
        {
            status = 0;
            blacklist = false;
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthday = birthday;
            this.priority = priority;
        }

        public string getFullName()
        {
            return (firstName + " " + lastName);
        }

        public string getFirstName()
        {
            return firstName;
        }

        public string getLastName()
        {
            return lastName;
        }

        public string getBirthday()
        {
            return birthday;
        }

        public int getStatus()
        {
            return status;
        }

        public int getPriority()
        {
            return priority;
        }

        public void SetPriority(int i)
        {
            priority=i;
        }

        public void SetStatus(int i)
        {
            status = i;
        }

        public void SetFirstName(string s)
        {
            firstName = s;
        }

        public void SetLastName(string s)
        {
            lastName = s;
        }

        public bool blacklistStat()
        {
            return blacklist;
        }

        public void setBlacklist()
        {
            if (blacklist)
            {
                blacklist = false;
            }
            else
            {
                blacklist = true;
            }
        }
    }
}
