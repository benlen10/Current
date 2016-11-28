using System.Collections.Generic;

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
        public int TotalGameCount { get; set; }
        public string HashKey { get; set; }

        #endregion
    }
}
