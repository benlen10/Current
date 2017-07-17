using System;
using UniCade.Constants;

namespace UniCade.Objects
{
    public class Game : IGame
    {

        #region Properties

        /// <summary>
        /// The raw filename for the ROM file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The common title (display name) of the game
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the console that the game belongs to
        /// </summary>
        public string ConsoleName
        {
            get
            {
                return ConsoleName;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Console name cannot be null");

                ConsoleName = value;
            }
        }

        /// <summary>
        /// Brief game description or overview
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The original release date of the game
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// The publisher of the game
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// The developer of the game
        /// </summary>
        public string DeveloperName { get; set; }

        /// <summary>
        /// The genere(s) for the current game
        /// </summary>
        public string Genres { get; set; }

        /// <summary>
        /// A list of common tags tags for the current gam
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// The average user review score out of 100
        /// </summary>
        public string UserReviewScore { get; set; }

        /// <summary>
        /// The average critic review score out of 100
        /// </summary>
        public string CriticReviewScore { get; set; }

        /// <summary>
        /// Trivia facts for the current game
        /// </summary>
        public string Trivia { get; set; }

        /// <summary>
        /// The supported number of players
        /// </summary>
        public string PlayerCount { get; set; }

        /// <summary>
        /// The ESRB content rating
        /// </summary>
        public Enums.ESRB EsrbRating { get; set; }

        /// <summary>
        /// The ESRB content descriptors
        /// </summary>
        public string EsrbDescriptors { get; set; }

        /// <summary>
        /// Detailed summary of the ESRB rating
        /// </summary>
        public string EsrbSummary { get; set; }

        /// <summary>
        /// Int value representing the favorite status of the game
        /// </summary>
        public int Favorite { get; set; }

        /// <summary>
        /// The current launch count for the game
        /// </summary>
        public int LaunchCount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor for the Game class
        /// </summary>
        /// <param name="fileName">the raw filename for the ROM file</param>
        /// <param name="consoleName">The name of the console that the game belongs to</param>
        public Game(string fileName, string consoleName)
        {
            FileName = fileName;
            ConsoleName = consoleName;
            Title = fileName.Substring(0, fileName.IndexOf('.'));
            LaunchCount = 0;
        }

        /// <summary>
        /// Full constructor for the Game class
        /// </summary>
        /// <param name="fileName">The raw ROM filename for the game</param>
        /// <param name="consoleName">the name of the console taht the game belongs to</param>
        /// <param name="launchCount">The current launch count for the game</param>
        /// <param name="releaseDate">The original release date of the game</param>
        /// <param name="publisherName">The publisher of the game</param>
        /// <param name="developerName">The developer of teh game</param>
        /// <param name="userReviewScore">The average user review score out of 100</param>
        /// <param name="criticScore">The average critic review score out of 100</param>
        /// <param name="playerCount">The supported number of players</param>
        /// <param name="trivia">Trivia facts for the current game</param>
        /// <param name="esrbRating">The ESRB content rating</param>
        /// <param name="esrbDescriptor">The ESRB content descriptors</param>
        /// <param name="esrbSummary">Detailed summary of the ESRB rating</param>
        /// <param name="description">Brief game description or overview</param>
        /// <param name="genres">The genere(s) for the current game</param>
        /// <param name="tags">A list of common tags tags for the current game</param>
        /// <param name="isFavorite"></param>
        public Game(string fileName, string consoleName, int launchCount, string releaseDate, string publisherName, string developerName, string userReviewScore, string criticScore, string playerCount, string trivia, Enums.ESRB esrbRating, string esrbDescriptor, string esrbSummary, string description, string genres, string tags, int isFavorite)
        {
            FileName = fileName;
            ConsoleName = consoleName;
            Favorite = isFavorite;
            LaunchCount = launchCount;
            ReleaseDate = releaseDate;
            PublisherName = publisherName;
            DeveloperName = developerName;
            UserReviewScore = userReviewScore;
            CriticReviewScore = criticScore;
            PlayerCount = playerCount;
            Trivia = trivia;
            EsrbRating = esrbRating;
            Description = description;
            EsrbDescriptors = esrbDescriptor;
            EsrbSummary = esrbSummary;
            Genres = genres;
            Tags = tags;

            //Parse the game title from the raw ROM filename
            if (fileName.Length > 2)
            {
                Title = fileName.Substring(0, fileName.IndexOf('.'));
            }
        }

        #endregion
    }
}
