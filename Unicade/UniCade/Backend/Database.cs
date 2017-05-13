using System.Collections.Generic;

namespace UniCade
{
    public class Database : IDatabase
    {
        #region Properties

        public List<IConsole> ConsoleList { get; set; }
        public List<IUser> UserList { get; set; }
        public int TotalGameCount { get; set; }
        public string HashKey { get; set; }

        #endregion

        /// <summary>
        /// Public constructor for the Database class
        /// </summary>
        public Database()
        {
            TotalGameCount = 0;
            HashKey = null;
            ConsoleList = new List<IConsole>();
            UserList = new List<IUser>();
        }

    }
}
