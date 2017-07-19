using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCade.Constants;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

namespace UniCade.Backend
{
    static class Database
    {
        #region Properties

        /// <summary>
        /// The current list of consoles
        /// </summary>
        private static List<IConsole> ConsoleList;

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

        /// <summary>
        /// The current number of consoles in the ConsoleList
        /// </summary>
        public static int ConsoleCount { get; private set; }

        /// <summary>
        /// The current number of consoles in the ConsoleList
        /// </summary>
        public static int UserCount { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initalize the current properties
        /// </summary>
        public static void Initalize()
        {
            TotalGameCount = 0;
            ConsoleList = new List<IConsole>();
            UserList = new List<IUser>();
            IUser uniCadeUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", Enums.ESRB.Null, "");
            UserList.Add(uniCadeUser);
            CurrentUser = uniCadeUser;
        }

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
            ConsoleCount++;
            return true;
        }

        /// <summary>
        /// Add a new console to the database
        /// </summary>
        /// <param name="consoleName">The name of the console to remove</param>
        /// <returns>true if the console was sucuessfully added</returns>
        public static bool RemoveConsole(string consoleName)
        {
            //Attempt to fetch the console from the current list
            IConsole console = ConsoleList.Find(e => e.ConsoleName.Equals(consoleName));

            if (console != null)
            {
                ConsoleList.Remove(console);
                ConsoleCount--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the console object with the matching name
        /// </summary>
        /// <param name="consoleName">The name of the console to fetch</param>
        /// <returns>IConsole object with matching name</returns>
        public static IConsole GetConsole(string consoleName)
        {
            return ConsoleList.Find(c => c.ConsoleName.Equals(consoleName));
        }

        /// <summary>
        /// Return a string list of all console names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetConsoleList()
        {
            return ConsoleList.Select(c => c.ConsoleName).ToList();
        }

        /// <summary>
        /// Add a new user to the currentUserList
        /// Return false if the username already exists
        /// </summary>
        /// <returns>true if the user was added sucuessfully</returns>
        public static bool AddUser(IUser user)
        {
            if (UserList.Find(u => u.Username.Equals(user.Username)) == null)
            {
                UserList.Add(user);
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
