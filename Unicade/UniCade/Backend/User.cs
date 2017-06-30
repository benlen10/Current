using System.Collections.Generic;
using UniCade.Constants;

namespace UniCade
{
    public class User : IUser
    {
        #region Properties

        /// <summary>
        /// The current (unique) username
        /// </summary>
        public string Username { get; set; }

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

        /// <summary>
        /// The password for the current user
        /// </summary>
        private string Password;

        #endregion

        #region Constructors

        public User(string userName, string password, int loginCount, string email, int totalLaunchCount, string userInfo, Enums.ESRB allowedEsrb, string profPic)
        {
            Username = userName;
            Password = password;
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
        public bool SetUserPassword(string password){
            Password = password;
            return true;
        }

        /// <summary>
        /// Return the current password for the user
        /// </summary>
        /// <returns>the current user's password</returns>
        public string GetUserPassword(){
            return Password;
        }

        #endregion
    }
}

