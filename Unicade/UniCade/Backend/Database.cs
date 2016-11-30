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
    }
}
