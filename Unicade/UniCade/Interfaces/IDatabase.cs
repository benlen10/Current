﻿using System.Collections.Generic;

namespace UniCade
{
    public interface IDatabase
    {
        #region Properties

        /// <summary>
        /// The current number of games across all game consoles
        /// </summary>
        int TotalGameCount { get; set; }

        /// <summary>
        /// The current hash key used to generate the license key
        /// </summary>
        string HashKey { get; set; }

        /// <summary>
        /// The list of consoles for the current database instance
        /// </summary>
        List<IConsole> ConsoleList { get; set; }

        /// <summary>
        /// The list of current users
        /// </summary>
        List<IUser> UserList { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refresh the total game count across all consoles
        /// </summary>
        /// <returns>Total game count</returns>
        int RefreshTotalGameCount();

        #endregion
    }
}