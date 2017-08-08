﻿using System;
using System.Collections.Generic;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;

namespace UniCade.Objects
{
    public class User : IUser
    {
        #region Constants

        /// <summary>
        /// The max char length for a username
        /// </summary>
        private const int MAX_USERNAME_LENGTH = 30;

        /// <summary>
        /// The max char length for user info descriptions
        /// </summary>
        private const int MAX_USER_INFO_LENGTH = 200;

        /// <summary>
        /// The max char length for user email addresses
        /// </summary>
        private const int MAX_EMAIL_LENGTH = 200;

        /// <summary>
        /// The max path length for local directories
        /// </summary>
        private const int MAX_PATH_LENGTH = 1000;

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
        /// A brief description of the user
        /// </summary>
        public string UserInfo
        {
            get => _userInfo;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("User info cannot be null");
                }
                if (value.Length > MAX_USER_INFO_LENGTH)
                {
                    throw new ArgumentException(String.Format("User info cannot exceed {0} chars", MAX_USER_INFO_LENGTH));
                }
                _userInfo = value;
            }
        }

        /// <summary>
        /// The user's email address
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Email cannot be null");
                }
                if (value.Length < 6)
                {
                    throw new ArgumentException("Email must be at least 5 chars");
                }
                if (!value.Contains("@"))
                {
                    throw new ArgumentException("Email is invalid");
                }
                if (value.Length > MAX_EMAIL_LENGTH)
                {
                    throw new ArgumentException(String.Format("Email cannot exceed {0} chars", MAX_EMAIL_LENGTH));
                }
                _email = value;
            }
        }

        /// <summary>
        /// The password for the current user
        /// </summary>
        public string ProfilePicture
        {
            get => _profilePicturePath;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Profile picture path cannot be null");
                }
                if (value.Length > MAX_PATH_LENGTH)
                {
                    throw new ArgumentException(String.Format("Profile Picture path cannot exceed {0} chars", MAX_PATH_LENGTH));
                }
                _profilePicturePath = value;
            }
        }

        /// <summary>
        /// A list of the user's favorite games
        /// </summary>
        public List<IGame> FavoritesList { get; set; }


        /// <summary>
        /// The max allowed ESRB for the current user (Parental Controls)
        /// </summary>
        public Enums.ESRB AllowedEsrb { get; set; }

        #endregion

        #region Private Instance Variables

        /// <summary>
        /// The current username
        /// </summary>
        private string _username;

        /// <summary>
        /// The current user description
        /// </summary>
        private string _userInfo;

        /// <summary>
        /// The password for the current user
        /// </summary>
        private string _password;

        /// <summary>
        /// The email address for the current user
        /// </summary>
        private string _email;

        /// <summary>
        /// The path to the user profile picture
        /// </summary>
        private string _profilePicturePath;

        /// <summary>
        /// The total number of games this user has launched
        /// </summary>
        private int _userLaunchCount;

        /// <summary>
        /// The total number of times this user has logged in
        /// </summary>
        private int _userLoginCount;

        #endregion

        #region Constructors

        public User(string userName, string password, int loginCount, string email, int totalLaunchCount, string userInfo, Enums.ESRB allowedEsrb, string profPic)
        {
            Username = userName;
            _password = password;
            _userLoginCount = loginCount;
            _userLaunchCount = totalLaunchCount;
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

        /// <summary>
        /// Return the total numer of games this user has launched
        /// </summary>
        /// <returns>userLaunchCount</returns>
        public int GetUserLaunchCount()
        {
            return _userLaunchCount;
        }

        /// <summary>
        /// Incriment the launch count for the current user by 1
        /// </summary>
        public void IncrementUserLaunchCount()
        {
            _userLaunchCount++;
        }

        /// <summary>
        /// Return the total number of times this user has logged in
        /// </summary>
        /// <returns>userLoginCount</returns>
        public int GetUserLoginCount()
        {
            return _userLoginCount;
        }

        /// <summary>
        /// Incriment the login count for the current user by 1
        /// </summary>
        public void IncrementUserLoginCount()
        {
            _userLoginCount++;
        }

        #endregion
    }
}

