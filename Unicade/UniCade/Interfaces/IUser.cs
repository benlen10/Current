using System.Collections.Generic;

namespace UniCade
{
    public interface IUser
    {
        string AllowedEsrb { get; set; }
        string Email { get; set; }
        List<IGame> Favorites { get; set; }
        int LoginCount { get; set; }
        string Pass { get; set; }
        string ProfilePic { get; set; }
        int TotalLaunchCount { get; set; }
        string UserInfo { get; set; }
        string Username { get; set; }
    }
}