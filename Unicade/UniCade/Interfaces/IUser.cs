using System.Collections.Generic;

namespace UniCade
{
    public interface IUser
    {
        #region Properties

        /// <summary>
        /// The max allowed ESRB for the current user (Parental Controls)
        /// </summary>
        string AllowedEsrb { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// A list of the user's favorite games
        /// </summary>
        List<IGame> Favorites { get; set; }

        /// <summary>
        /// The numer of times this user has logged in
        /// </summary>
        int LoginCount { get; set; }

        /// <summary>
        /// The filename and location for the user's profile pictur
        /// </summary>
        string ProfilePicture { get; set; }

        /// <summary>
        /// The total number of games that this user has launched
        /// </summary>
        int TotalLaunchCount { get; set; }

        /// <summary>
        /// A brief description of the user
        /// </summary>
        string UserInfo { get; set; }

        /// <summary>
        /// The current (unique) username
        /// </summary>
        string Username { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the password for the current user
        /// </summary>
        /// <param name="password">The new password</param>
        /// <returns>true if the password was changed successfully</returns>
        bool SetUserPassword(string password);

        /// <summary>
        /// Return the current password for the user
        /// </summary>
        /// <returns>the current user's password</returns>
        string GetUserPassword();

        #endregion
    }
}