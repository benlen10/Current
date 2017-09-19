using System;
using System.Collections.Generic;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;

namespace UniCade.Objects
{
    internal class User : IUser
    {

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
                if (_userExists && (Database.GetUser(value) != null))
                {
                    throw new ArgumentException("Username already exists");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Username contains invalid characters");
                }
                if (_userExists && _username.Equals("UniCade"))
                {
                    throw new ArgumentException("Default UniCade account cannot be renamed");
                }
                if (value.Length < ConstValues.MinUsernameLength)
                {
                    throw new ArgumentException("Username must be at least 4 chars");
                }
                if (value.Length > ConstValues.MaxUsernameLength)
                {
                    throw new ArgumentException($"Username cannot exceed {ConstValues.MaxUsernameLength} chars");
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
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("User info contains invalid characters");
                }
                if (value.Length > ConstValues.MaxUserInfoLength)
                {
                    throw new ArgumentException($"User info cannot exceed {ConstValues.MaxUserInfoLength} chars");
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
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Email contains invalid characters");
                }
                if (!value.Contains("@"))
                {
                    throw new ArgumentException("Email is invalid");
                }
                if (value.Length > ConstValues.MaxEmailLength)
                {
                    throw new ArgumentException($"Email cannot exceed {ConstValues.MaxEmailLength} chars");
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
                if (value.Length > ConstValues.MaxPathLength)
                {
                    throw new ArgumentException($"Profile Picture path cannot exceed {ConstValues.MaxPathLength} chars");
                }
                _profilePicturePath = value;
            }
        }

        /// <summary>
        /// The max allowed ESRB for the current user (Parental Controls)
        /// </summary>
        public Enums.Esrb AllowedEsrb { get; set; }

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

        /// <summary>
        /// specifies if this user is teh default unicade user
        /// </summary>
        private readonly bool _userExists;

        /// <summary>
        /// A list of the user's favorite games
        /// </summary>
        private List<IGame> _favoritesList;

        #endregion

        #region Constructors

        public User(string userName, string password, int loginCount, string email, int totalLaunchCount, string userInfo, Enums.Esrb allowedEsrb, string profPic)
        {
            Username = userName;
            _password = password;
            _userLoginCount = loginCount;
            _userLaunchCount = totalLaunchCount;
            UserInfo = userInfo;
            AllowedEsrb = allowedEsrb;
            Email = email;
            ProfilePicture = profPic;
            _favoritesList = new List<IGame>();
            _userExists = true;
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
            if (password == null)
            {
                throw new ArgumentException("Password cannot be null");
            }
            if (password.Length < ConstValues.MinUserPasswordLength)
            {
                throw new ArgumentException($"Password length cannot be less than {ConstValues.MinUserPasswordLength} chars");
            }
            if (password.Length > ConstValues.MaxUserPasswordLength)
            {
                throw new ArgumentException($"Password length cannot exceed {ConstValues.MaxUserPasswordLength} chars");
            }
            if (Utilties.CheckForInvalidChars(password))
            {
                throw new ArgumentException("Password contains invalid characters");
            }

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

        /// <summary>
        ///Add a favorie game to the current favorites list
        /// </summary>
        public bool AddFavorite(IGame game)
        {
            return true;
        }

        /// <summary>
        /// Remove a game from the current favorites list
        /// </summary>
        public bool RemoveFavorite(IGame game)
        {
            if (game != null)
            {
                return _favoritesList.Remove(game);
            }
            return false;
        }

        /// <summary>
        /// Return a copt of the current favories list of IGame Objects
        /// </summary>
        public List<IGame> GetFavoritesList()
        {
            return new List<IGame>(_favoritesList);
        }

        #endregion
    }
}

