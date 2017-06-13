using System.Collections.Generic;

namespace UniCade
{
    public class Database : IDatabase
    {
        #region Properties

        /// <summary>
        /// The list of consoles for the current database instance
        /// </summary>
        public List<IConsole> ConsoleList { get; set; }

        /// <summary>
        /// The list of current users
        /// </summary>
        public List<IUser> UserList { get; set; }

        /// <summary>
        /// The current number of games across all game consoles
        /// </summary>
        public int TotalGameCount { get; set; }

        /// <summary>
        /// The current hash key used to generate the license key
        /// </summary>
        public string HashKey { get; set; }

        #endregion

        #region Constructors

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

        /// <summary>
        /// Add a new console to the database
        /// </summary>
        /// <param name="console"></param>
        /// <returns>true if the console was sucuessfully added</returns>
        public bool AddConsole(IConsole console)
        {
            //Return false if a console with a duplicate name already exists
            if(ConsoleList.Find(e => e.ConsoleName.Equals(console.ConsoleName)) != null)
            {
                return false;
            }

            ConsoleList.Add(console);
            return true;
        }

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        /// <returns>Total game count</returns>
        public int RefreshTotalGameCount()
        {
            int count = 0;
            foreach(Console console in ConsoleList)
            {
                count += console.GameCount;
            }
            TotalGameCount = count;
            return count;
        }

        #endregion
    }
}
