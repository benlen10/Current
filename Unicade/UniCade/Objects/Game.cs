using System;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;

namespace UniCade.Objects
{
    public class Game : IGame
    {

        #region Public Properties

        /// <summary>
        /// The raw filename for the ROM file
        /// </summary>
        public string FileName
        {
            get => _fileName;
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Game file name cannot be null");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("Game file name must be 3 or more characters characters");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Game file name contains invalid characters");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Game file name contains invalid characters");
                }
                if (!value.Contains("."))
                {
                    throw new ArgumentException("Game file name is invalid");
                }
                if (value.Length > ConstValues.MAX_GAME_FILENAME_LENGTH)
                {
                    throw new ArgumentException($"Game file name length cannot exceed {ConstValues.MAX_GAME_FILENAME_LENGTH} chars");
                }
                _fileName = value;
            }
        }

        /// <summary>
        /// The common title (display name) of the game
        /// </summary>
        public string Title
        {
            get => _title;
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Game title cannot be null");
                }
                if (value.Length < 2)
                {
                    throw new ArgumentException("Game title must be 1 or more characters characters");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Game title contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_GAME_TITLE_LENGTH)
                {
                    throw new ArgumentException($"Game title length cannot exceed {ConstValues.MAX_GAME_TITLE_LENGTH} chars");
                }
                _title = value;
            }
        }

        /// <summary>
        /// The name of the console that the game belongs to
        /// </summary>
        public string ConsoleName
        {
            get => _consoleName;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Console name cannot be null");
                }
                if (value.Length == 0)
                {
                    throw new ArgumentException("Console name cannot be empty");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Username contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_CONSOLE_NAME_LENGTH)
                {
                    throw new ArgumentException($"Console name length cannot exceed {ConstValues.MAX_CONSOLE_NAME_LENGTH} chars");
                }
                _consoleName = value;
            }
        }

        /// <summary>
        /// Brief game description or overview
        /// </summary>
        public string Description
        {
            get => _gameDescription;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Game description cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Game description contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_GAME_DESCRIPTION_LENGTH)
                {
                    throw new ArgumentException($"Game description length cannot exceed {ConstValues.MAX_GAME_DESCRIPTION_LENGTH} chars");
                }
                _gameDescription = value;
            }
        }

        /// <summary>
        /// The original release date of the game
        /// </summary>
        public string ReleaseDate
        {
            get => _releaseDate;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Release date cannot be null");
                }
                if (!Utilties.IsAllDigits(value))
                {
                    throw new ArgumentException("Release date must be only digits");
                }
                if (value.Length != 4)
                {
                    throw new ArgumentException("Release date must be four digits");
                }
                _releaseDate = value;
            }
        }

        /// <summary>
        /// The publisher of the game
        /// </summary>
        public string PublisherName
        {
            get => _publisherName;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Publisher name cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Publisher name contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_PUBLISHER_DEVELOPER_LENGTH)
                {
                    throw new ArgumentException($"Publisher name length cannot exceed {ConstValues.MAX_PUBLISHER_DEVELOPER_LENGTH} chars");
                }
                _publisherName = value;
            }
        }

        /// <summary>
        /// The developer of the game
        /// </summary>
        public string DeveloperName
        {
            get => _developerName;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Developer name cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Developer name contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_PUBLISHER_DEVELOPER_LENGTH)
                {
                    throw new ArgumentException($"Developer name length cannot exceed {ConstValues.MAX_PUBLISHER_DEVELOPER_LENGTH} chars");
                }
                _developerName = value;
            }
        }

        /// <summary>
        /// The genere(s) for the current game
        /// </summary>
        public string Genres
        {
            get => _genres;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Genres cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Genres contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_GAME_GENRE_LENGTH)
                {
                    throw new ArgumentException($"Genres length cannot exceed {ConstValues.MAX_GAME_GENRE_LENGTH} chars");
                }
                _genres = value;
            }
        }

        /// <summary>
        /// A list of common tags tags for the current gam
        /// </summary>
        public string Tags
        {
            get => _tags;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Tags cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Tags contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_GAME_TAGS_LENGTH)
                {
                    throw new ArgumentException($"Tags length cannot exceed {ConstValues.MAX_GAME_TAGS_LENGTH} chars");
                }
                _tags = value;
            }
        }

        /// <summary>
        /// The average user review score out of 100
        /// </summary>
        public string UserReviewScore
        {
            get => _userReviewScore;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("User Review Score cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("User Review score contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_GAME_REVIEW_SCORE_LENGTH)
                {
                    throw new ArgumentException($"User review score length cannot exceed {ConstValues.MAX_GAME_REVIEW_SCORE_LENGTH} chars");
                }
                _userReviewScore = value;
            }
        }

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
        public bool Favorite { get; set; }

        /// <summary>
        /// The ESRB content rating
        /// </summary>
        public Enums.ESRB EsrbRating { get; set; }

        /// <summary>
        /// The current launch count for the game
        /// </summary>
        public int LaunchCount { get; set; }

        #endregion

        #region Private Instance Fields

        /// <summary>
        /// The name of the console that the game belongs to
        /// </summary>
        private string _consoleName;

        /// <summary>
        /// The name of the console that the game belongs to
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The title of the game
        /// </summary>
        private string _title;

        /// <summary>
        /// A brief description for the current game
        /// </summary>
        private string _gameDescription;

        /// <summary>
        /// The release date for the current game
        /// </summary>
        private string _releaseDate;

        /// <summary>
        /// The release date for the current game
        /// </summary>
        private string _publisherName;

        /// <summary>
        /// The release date for the current game
        /// </summary>
        private string _developerName;

        /// <summary>
        /// The current game genres
        /// </summary>
        private string _genres;

        /// <summary>
        /// A list of common tags tags for the current gam
        /// </summary>
        private string _tags;

        /// <summary>
        /// The average user review score out of 100
        /// </summary>
        private string _userReviewScore;

        /// <summary>
        /// The average critic review score out of 100
        /// </summary>
        private string _criticReviewScore;

        /// <summary>
        /// Trivia facts for the current game
        /// </summary>
        private string _trivia;

        /// <summary>
        /// The supported number of players
        /// </summary>
        private string _supportedPlayers;

        /// <summary>
        /// The ESRB content descriptors
        /// </summary>
        private string _esrbDescriptors;

        /// <summary>
        /// Detailed summary of the ESRB rating
        /// </summary>
        private string _esrbSummary;

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

            Favorite = (isFavorite > 0);

            //Parse the game title from the raw ROM filename
            if (fileName.Length > 2)
            {
                Title = fileName.Substring(0, fileName.IndexOf('.'));
            }
        }

        #endregion
    }
}
