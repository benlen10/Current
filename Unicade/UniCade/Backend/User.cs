using System.Collections.Generic;

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
        /// The password for the current user
        /// </summary>
        public string Password { get; set; }

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
        public string AllowedEsrb { get; set; }

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
        public List<IGame> Favorites { get; set; }

        #endregion

        #region Constructors

        public User(string userName, string password, int loginCount, string email, int totalLaunchCount, string userInfo, string allowedEsrb, string profPic)
        {
            Username = userName;
            Password = password;
            LoginCount = loginCount;
            TotalLaunchCount = totalLaunchCount;
            UserInfo = userInfo;
            AllowedEsrb = allowedEsrb;
            Email = email;
            ProfilePicture = profPic;
            Favorites = new List<IGame>();
        }

        #endregion
    }
}

