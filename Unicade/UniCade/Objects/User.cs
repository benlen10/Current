using System;
using System.Collections.Generic;
using UniCade.Backend;
using UniCade.Constants;

namespace UniCade.Objects
{
    public class User : IUser
    {
        #region Constants

        private const int MAX_USERNAME_LENGTH = 30;

        #endregion

        #region Properties

        /// <summary>
        /// The current username
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Username cannot be null");
                }
                if (value.Length < 5)
                {
                    throw new ArgumentException("Username must be at least 4 chars");
                }
                if (value.Length > MAX_USERNAME_LENGTH)
                {
                    throw new ArgumentException(String.Format("Username cannot exceed {0} chars", MAX_USERNAME_LENGTH));
                }
                _username = value;
            }
        }

        /// <summary>
        /// The numer of times this user has logged in
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// The total number of games that this user has launched
        /// </summary>
        public int TotalLaunchCount { get; set; }

        /// <summary>
        /// A brief description of the user
        /// </summary>
        public string UserInfo { get; set; }

        /// <summary>
        /// The max allowed ESRB for the current user (Parental Controls)
        /// </summary>
        public Enums.ESRB AllowedEsrb { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password for the current user
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// A list of the user's favorite games
        /// </summary>
        public List<IGame> FavoritesList { get; set; }


        #endregion

        #region Private Instance Variables

        /// <summary>
        /// The current username
        /// </summary>
        private string _username;

        /// <summary>
        /// The password for the current user
        /// </summary>
        private string _password;

        #endregion

        #region Constructors

        public User(string userName, string password, int loginCount, string email, int totalLaunchCount, string userInfo, Enums.ESRB allowedEsrb, string profPic)
        {
            Username = userName;
            _password = password;
            LoginCount = loginCount;
            TotalLaunchCount = totalLaunchCount;
            UserInfo = userInfo;
            AllowedEsrb = allowedEsrb;
            Email = email;
            ProfilePicture = profPic;
            FavoritesList = new List<IGame>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the password for the current user
        /// </summary>
        /// <param name="password">The new password</param>
        /// <returns>true if the password was changed successfully</returns>
        public bool SetUserPassword(string password)
        {
            _password = password;
            return true;
        }

        /// <summary>
        /// Return the current password for the user
        /// </summary>
        /// <returns>the current user's password</returns>
        public string GetUserPassword()
        {
            return _password;
        }

        #endregion
    }
}

