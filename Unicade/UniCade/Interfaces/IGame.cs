using UniCade.Constants;

namespace UniCade
{
    public interface IGame
    {
        #region Properties

        /// <summary>
        /// The name of the console that the game belongs to
        /// </summary>
        string ConsoleName { get; set; }

        /// <summary>
        /// The average critic review score out of 100
        /// </summary>
        string CriticReviewScore { get; set; }

        /// <summary>
        /// Brief game description or overview
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The developer of the game
        /// </summary>
        string DeveloperName { get; set; }

        /// <summary>
        /// The ESRB content rating
        /// </summary>
        Enums.ESRB EsrbRating { get; set; }

        /// <summary>
        /// The ESRB content descriptors
        /// </summary>
        string EsrbDescriptors { get; set; }

        /// <summary>
        /// Detailed summary of the ESRB rating
        /// </summary>
        string EsrbSummary { get; set; }

        /// <summary>
        /// Int value representing the favorite status of the game
        /// </summary>
        int Favorite { get; set; }

        /// <summary>
        /// The raw filename for the ROM file
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// The genere(s) for the current game
        /// </summary>
        string Genres { get; set; }

        /// <summary>
        /// The current launch count for the game
        /// </summary>
        int LaunchCount { get; set; }

        /// <summary>
        /// The supported number of players
        /// </summary>
        string PlayerCount { get; set; }

        /// <summary>
        /// The publisher of the game
        /// </summary>
        string PublisherName { get; set; }

        /// <summary>
        /// The original release date of the game
        /// </summary>
        string ReleaseDate { get; set; }

        /// <summary>
        /// A list of common tags tags for the current gam
        /// </summary>
        string Tags { get; set; }

        /// <summary>
        /// The common title (display name) of the game
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Trivia facts for the current game
        /// </summary>
        string Trivia { get; set; }

        /// <summary>
        /// The average user review score out of 100
        /// </summary>
        string UserReviewScore { get; set; }

        #endregion
    }
}