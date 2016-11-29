﻿using System.Collections.Generic;

namespace UniCade
{
    public class User
    {
        #region Constructors

        public User(string userName, string pass, int loginCount, string email, int totalLaunchCount, string userInfo, string allowedEsrb, string profPic)
        {
            Username = userName;
            Pass = pass;
            LoginCount = loginCount;
            TotalLaunchCount = totalLaunchCount;
            UserInfo = userInfo;
            AllowedEsrb = allowedEsrb;
            Email = email;
            ProfilePic = profPic;
            Favorites = new List<Game>();
        }

        #endregion

        #region Properties

        public string Username { get; set; }
        private string Pass { get; set; }
        public int LoginCount { get; set; }
        public int TotalLaunchCount { get; set; }
        private string UserInfo { get; set; }
        private string AllowedEsrb { get; set; }
        private string Email { get; set; }
        private string ProfilePic { get; set; }
        public List<Game> Favorites { get; set; }

        #endregion
    }
}

