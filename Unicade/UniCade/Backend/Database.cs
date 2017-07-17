using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = UniCade.Objects.Console;

namespace UniCade.Backend
{
    static class Database
    {
        #region Properties

        /// <summary>
        /// The current list of consoles
        /// </summary>
        public static List<IConsole> ConsoleList { get; set; }

        /// <summary>
        /// The list of current users
        /// </summary>
        public static List<IUser> UserList { get; set; }

        /// <summary>
        /// The current number of games across all game consoles
        /// </summary>
        public static int TotalGameCount { get; set; }

        /// <summary>
        /// The current user object 
        /// </summary>
        public static IUser CurrentUser;

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new console to the database
        /// </summary>
        /// <param name="console"></param>
        /// <returns>true if the console was sucuessfully added</returns>
        public static bool AddConsole(IConsole console)
        {
            //Return false if a console with a duplicate name already exists
            if (ConsoleList.Find(e => e.ConsoleName.Equals(console.ConsoleName)) != null)
            {
                return false;
            }

            ConsoleList.Add(console);
            return true;
        }

        /// <summary>
        /// Add a new console to the database
        /// </summary>
        /// <param name="console"></param>
        /// <param name="consoleName">The name of the console to remove</param>
        /// <returns>true if the console was sucuessfully added</returns>
        public static bool RemoveConsole(string consoleName)
        {
            //Attempt to fetch the console from the current list
            IConsole console = ConsoleList.Find(e => e.ConsoleName.Equals(consoleName));

            if (console != null)
            {
                ConsoleList.Remove(console);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        /// <returns>Total game count</returns>
        public static int RefreshTotalGameCount()
        {
            var count = 0;
            ConsoleList.ForEach(c => count += c.GameCount);
            TotalGameCount = count;
            return count;
        }

        #endregion

    }
}
