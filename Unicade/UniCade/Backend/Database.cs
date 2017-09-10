using System;
using System.Collections.Generic;
using System.Linq;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Objects;

namespace UniCade.Backend
{
    internal static class Database
    {

        #region  Private Static Variables

        /// <summary>
        /// The path to the current ROM directory
        /// </summary>
        public static string RomPath = @"C:\UniCade\ROMS";

        /// <summary>
        /// The path to the current media directory
        /// </summary>
        public static string MediaPath = @"C:\UniCade\Media";

        /// <summary>
        /// The path to the current Emulators directory
        /// </summary>
        public static string EmulatorPath
        {
            get => _emulatorFolderPath;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Emulator folder path cannot be null");
                }
                if (value.Length < 4)
                {
                    throw new ArgumentException("Emulator folder path too short");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Emulator folder path contains invalid characters");
                }
                if (!value.Contains(":\\"))
                {
                    throw new ArgumentException("Emulator path invalid");
                }
                if (value.Length > ConstValues.MAX_PATH_LENGTH)
                {
                    throw new ArgumentException(String.Format("Emulator folder path cannot exceed {0} chars", ConstValues.MAX_PATH_LENGTH));
                }
                _emulatorFolderPath = value;
            }
        }

        /// <summary>
        /// The current number of consoles present in the database
        /// </summary>
        private static int _consoleCount;

        /// <summary>
        /// The current list of consoles
        /// </summary>
        private static List<IConsole> _consoleList;

        /// <summary>
        /// The list of current users
        /// </summary>
        private static List<IUser> _userList;

        /// <summary>
        /// The current number of users in the database
        /// </summary>
        private static int _userCount;

        /// <summary>
        /// The current number of games across all game consoles
        /// </summary>
        private static int _totalGameCount;

        /// <summary>
        /// The current user object 
        /// </summary>
        private static IUser _currentUser;

        /// <summary>
        /// The default UniCade user  
        /// </summary>
        private static IUser _defaultUser;

        /// <summary>
        /// The path for the emulators folder
        /// </summary>
        private static string _emulatorFolderPath;

        /// <summary>
        /// The path for the roms folder
        /// </summary>
        private static string _romFolderPath;

        /// <summary>
        /// The path for the media folder
        /// </summary>
        private static string _mediaFolderPath;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initalize the current properties
        /// </summary>
        public static void Initalize()
        {
            _totalGameCount = 0;
            _consoleList = new List<IConsole>();
            _userList = new List<IUser>();
            IUser uniCadeUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", Enums.Esrb.Null, "");
            _userList.Add(uniCadeUser);
            _currentUser = uniCadeUser;
            _defaultUser = uniCadeUser;
            RomPath = @"C:\UniCade\ROMS";
            MediaPath = @"C:\UniCade\Media";
            EmulatorPath = @"C:\UniCade\Emulators";
        }

        /// <summary>
        /// Add a new console to the database
        /// </summary>a
        /// <param name="console"></param>
        /// <returns>true if the console was sucuessfully added</returns>
        public static bool AddConsole(IConsole console)
        {
            //Return false if a console with a duplicate name already exists
            if (_consoleList.Find(e => e.ConsoleName.Equals(console.ConsoleName)) != null)
            {
                return false;
            }

            //Verify that the console count is valid
            if (_consoleCount >= ConstValues.MAX_CONSOLE_COUNT)
            {
                return false;
            }

            _consoleList.Add(console);
            _consoleCount++;
            return true;
        }

        /// <summary>
        /// Add a new console to the database
        /// </summary>
        /// <param name="consoleName">The name of the console to remove</param>
        /// <returns>true if the console was sucuessfully added</returns>
        public static bool RemoveConsole(string consoleName)
        {
            //Ensure that at least one console remains
            if ((_consoleCount <= 1))
            {
                return false;
            }

            //Attempt to fetch the console from the current list
            IConsole console = _consoleList.Find(e => e.ConsoleName.Equals(consoleName));

            if (console != null)
            {
                _consoleList.Remove(console);
                _consoleCount--;
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
            return _consoleList.Find(c => c.ConsoleName.Equals(consoleName));
        }

        /// <summary>
        /// Return a string list of all console names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetConsoleList()
        {
            return _consoleList.Select(c => c.ConsoleName).ToList();
        }

        public static int GetConsoleCount()
        {
            return _consoleCount;
        }

        /// <summary>
        /// Add a new user to the currentUserList
        /// Return false if the username already exists
        /// </summary>
        /// <returns>true if the user was added sucuessfully</returns>
        public static bool AddUser(IUser user)
        {
            //Verify that the user count does not exceed the max value
            if (_userCount >= ConstValues.MAX_USER_COUNT)
            {
                return false;
            }

            //Verify that the username is unique
            if (_userList.Find(u => u.Username.Equals(user.Username)) == null)
            {
                _userList.Add(user);
                _userCount++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove a user from the current _userList
        /// Return false if the user is not found
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True if the user was removed sucuessfully</returns>
        public static bool RemoveUser(string username)
        {
            //Ensure that at least one console remains and that UniCade account cannot be deleted
            if ((_consoleCount > 1) && !username.Equals("UniCade"))
            {
                //Fetch the user
                IUser user = _userList.Find(u => u.Username.Equals(username));
                if (user != null)
                {
                    //If the user being removed is the current user, restore the default current user
                    if (username.Equals(_currentUser.Username))
                    {
                        RestoreDefaultUser();
                    }

                    //Remove the user, decrement the usercount and return true
                    _userList.Remove(user);
                    _userCount--;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the IUser object with the matching username
        /// </summary>
        /// <param name="username">The username of the user to fetch</param>
        /// <returns>IUser object with matching username</returns>
        public static IUser GetUser(string username)
        {
            return _userList.Find(c => c.Username.Equals(username));
        }

        /// <summary>
        /// Return a string list of all usernames
        /// </summary>
        /// <returns>List of all usernames</returns>
        public static List<string> GetUserList()
        {
            return _userList.Select(u => u.Username).ToList();
        }

        /// <summary>
        /// Return the current number of users present in the database
        /// </summary>
        /// <returns>User count</returns>
        public static int GetUserCount()
        {
            return _userCount;
        }

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        /// <returns>Total game count</returns>
        public static int RefreshTotalGameCount()
        {
            var count = 0;
            _consoleList.ForEach(c => count += c.GetGameCount());
            _totalGameCount = count;
            return count;
        }

        /// <summary>
        /// Return the total number of games across all consoles
        /// </summary>
        /// <returns>Total game count</returns>
        public static int GetTotalGameCount()
        {
            return _totalGameCount;
        }

        /// <summary>
        /// Return the current IUser object
        /// </summary>
        /// <returns>currentUser</returns>
        public static IUser GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Return the current IUser object
        /// </summary>
        public static bool SetCurrentUser(IUser user)
        {
            if (user != null)
            {
                _currentUser = user;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the CurrentUser to the default UniCade user
        /// </summary>
        public static void RestoreDefaultUser()
        {
            _currentUser = _defaultUser;
        }

        #endregion

    }
}
