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
        private string Id;
        private bool blacklist;
        private int priority;
        private int status;
        private string notes;


        public User(string firstName, string lastName, string birthday, int priority, int status, string Id, string notes)
        {
            blacklist = false;
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthday = birthday;
            this.priority = priority;
            this.status = status;
            this.Id = Id;
            this.notes = notes;
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

        public string getNotes()
        {
            return notes;
        }

        public void setNotes(string s)
        {
            notes = s;
        }

        public string getId()
        {
            return Id;
        }
        public void SetId(string s)
        {
            Id = s;
        }
    }
}
