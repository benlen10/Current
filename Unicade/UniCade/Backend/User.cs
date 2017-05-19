using System.Collections.Generic;

namespace UniCade
{
    public class User : IUser
    {
        #region Constructors

        public User(string userName, string pass, int loginCount, string email, int totalLaunchCount, string userInfo, string allowedEsrb, string profPic)
        {
            Username = userName;
            Password = pass;
            LoginCount = loginCount;
            TotalLaunchCount = totalLaunchCount;
            UserInfo = userInfo;
            AllowedEsrb = allowedEsrb;
            Email = email;
            ProfilePicture = profPic;
            Favorites = new List<IGame>();
        }

        #endregion

        #region Properties

        public string Username { get; set; }
        public string Password { get; set; }
        public int LoginCount { get; set; }
        public int TotalLaunchCount { get; set; }
        public string UserInfo { get; set; }
        public string AllowedEsrb { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public List<IGame> Favorites { get; set; }

        #endregion
    }
}

