using System.Collections.Generic;
using UniCade.Constants;

namespace UniCade.Interfaces
{
    public interface IUser
    {
        #region Properties

        /// <summary>
        /// The max allowed ESRB for the current user (Parental Controls)
        /// </summary>
        Enums.ESRB AllowedEsrb { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// A list of the user's favorite games
        /// </summary>
        List<IGame> FavoritesList { get; set; }

        /// <summary>
        /// The filename and location for the user's profile pictur
        /// </summary>
        string ProfilePicture { get; set; }


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


        /// <summary>
        /// Return the total numer of games this user has launched
        /// </summary>
        /// <returns>userLaunchCount</returns>
        int GetUserLaunchCount();


        /// <summary>
        /// Incriment the launch count for the current user by 1
        /// </summary>
        void IncrementUserLaunchCount();

        /// <summary>
        /// Return the total number of times this user has logged in
        /// </summary>
        /// <returns>userLoginCount</returns>
        int GetUserLoginCount();

        /// <summary>
        /// Incriment the login count for the current user by 1
        /// </summary>
        void IncrementUserLoginCount();

        #endregion
    }
}