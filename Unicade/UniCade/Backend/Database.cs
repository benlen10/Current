using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UniCade
{
    public class Database
    {
        public Database()
        {
            ConsoleList = new List<Console>();
            UserList = new List<User>();
        }

        #region Properties

        public List<Console> ConsoleList { get; set; }
        public List<User> UserList { get; set; }
        public static int TotalGameCount { get; set; }
        public static string HashKey { get; set; }

        #endregion
    }
}
