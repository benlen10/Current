using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestList
{
    class User
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
    }
}
