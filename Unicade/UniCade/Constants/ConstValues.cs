using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.Constants
{
    static class ConstValues
    {
        #region Constants 

        /// <summary>
        /// The maximum number of consoles allowed 
        /// </summary>
        public const int MAX_CONSOLE_COUNT = 100;

        /// <summary>
        /// The maximum number of games allowed per console
        /// </summary>
        public const int MAX_GAME_COUNT = 5000;

        /// <summary>
        /// The maximum number of users allowed 
        /// </summary>
        public const int MAX_USER_COUNT = 50;

        /// <summary>
        /// The max length for console
        /// </summary>
        public const int MAX_CONSOLE_NAME_LENGTH = 35;

        /// <summary>
        /// The max length for directory paths
        /// </summary>
        public const int MAX_PATH_LENGTH = 1000;

        /// <summary>
        /// The max length a ROM file extension
        /// </summary>
        public const int MAX_FILE_EXT_LENGTH = 1000;

        /// <summary>
        /// The max length a the console info
        /// </summary>
        public const int MAX_CONSOLE_INFO_LENGTH = 1000;

        /// <summary>
        /// The max length a the console info
        /// </summary>
        public const int MAX_LAUNCH_PARAMS_LENGTH = 1000;

        /// <summary>
        /// The max char length for a username
        /// </summary>
        public const int MAX_USERNAME_LENGTH = 30;

        /// <summary>
        /// The max char length for user info descriptions
        /// </summary>
        public const int MAX_USER_INFO_LENGTH = 200;

        /// <summary>
        /// The max char length for user email addresses
        /// </summary>
        public const int MAX_EMAIL_LENGTH = 200;

        /// <summary>
        /// The max char length for game filenames
        /// </summary>
        public const int MAX_GAME_FILENAME_LENGTH = 200;

        /// <summary>
        /// The max char length for game titles
        /// </summary>
        public const int MAX_GAME_TITLE_LENGTH = 200;

        /// <summary>
        /// The max char length for game titles
        /// </summary>
        public const int MAX_GAME_DESCRIPTION_LENGTH = 500;

        /// <summary>
        /// The max char length for game titles
        /// </summary>
        public const int MAX_PUBLISHER_DEVELOPER_LENGTH = 500;

        #endregion

        #region  Static Readonly Fields

        /// <summary>
        /// Global invalid characters 
        /// </summary>
        public static readonly char[] InvalidChars = new char[] { '|', '*' };

        #endregion
    }
}
