using System.Collections.Generic;

namespace UniCade
{
    public static class Database
    {
        #region Properties

        public static List<Console> ConsoleList { get; set; }
        public static List<User> UserList { get; set; }
        public static int TotalGameCount { get; set; }
        public static string HashKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize the static database members
        /// </summary>
        public static void Initialize()
        {
            TotalGameCount = 0;
            HashKey = null;
            ConsoleList = new List<Console>();
            UserList = new List<User>();
        }

        #endregion
    }
}
