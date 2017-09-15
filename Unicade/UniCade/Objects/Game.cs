using System;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;

namespace UniCade.Objects
{
    internal class Game : IGame
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
                if (value.Length > ConstValues.MaxGameFilenameLength)
                {
                    throw new ArgumentException($"Game file name length cannot exceed {ConstValues.MaxGameFilenameLength} chars");
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
                if (value.Length < 1)
                {
                    throw new ArgumentException("Game title must be 1 or more characters characters");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Game title contains invalid characters");
                }
                if (value.Length > ConstValues.MaxGameTitleLength)
                {
                    throw new ArgumentException($"Game title length cannot exceed {ConstValues.MaxGameTitleLength} chars");
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
                if (value.Length > ConstValues.MaxConsoleNameLength)
                {
                    throw new ArgumentException($"Console name length cannot exceed {ConstValues.MaxConsoleNameLength} chars");
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
                if (value.Length > ConstValues.MaxGameDescriptionLength)
                {
                    throw new ArgumentException($"Game description length cannot exceed {ConstValues.MaxGameDescriptionLength} chars");
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
                if ((value.Length != 4 )&& (value.Length != 0))
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
                if (value.Length > ConstValues.MaxPublisherDeveloperLength)
                {
                    throw new ArgumentException($"Publisher name length cannot exceed {ConstValues.MaxPublisherDeveloperLength} chars");
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
                if (value.Length > ConstValues.MaxPublisherDeveloperLength)
                {
                    throw new ArgumentException($"Developer name length cannot exceed {ConstValues.MaxPublisherDeveloperLength} chars");
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
                if (value.Length > ConstValues.MaxGameGenreLength)
                {
                    throw new ArgumentException($"Genres length cannot exceed {ConstValues.MaxGameGenreLength} chars");
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
                if (value.Length > ConstValues.MaxGameTagsLength)
                {
                    throw new ArgumentException($"Tags length cannot exceed {ConstValues.MaxGameTagsLength} chars");
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
                if (value.Length > ConstValues.MaxGameReviewScoreLength)
                {
                    throw new ArgumentException($"User review score length cannot exceed {ConstValues.MaxGameReviewScoreLength} chars");
                }
                _userReviewScore = value;
            }
        }

        /// <summary>
        /// The average critic review score out of 100
        /// </summary>
        public string CriticReviewScore
        {
            get => _criticReviewScore;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Critic Review Score cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Critic Review score contains invalid characters");
                }
                if (value.Length > ConstValues.MaxGameReviewScoreLength)
                {
                    throw new ArgumentException($"Critic review score length cannot exceed {ConstValues.MaxGameReviewScoreLength} chars");
                }
                _criticReviewScore = value;
            }
        }

        /// <summary>
        /// Trivia facts for the current game
        /// </summary>
        public string Trivia
        {
            get => _trivia;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Game trivia cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Game trivia contains invalid characters");
                }
                if (value.Length > ConstValues.MaxGameTriviaLength)
                {
                    throw new ArgumentException($"Game trivia length cannot exceed {ConstValues.MaxGameTriviaLength} chars");
                }
                _trivia = value;
            }
        }

        /// <summary>
        /// The supported number of players
        /// </summary>
        public string SupportedPlayerCount
        {
            get => _supportedPlayerCount;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Supported Player count cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Supported Player count contains invalid characters");
                }
                if (value.Length > ConstValues.MaxGamePlayercountLength)
                {
                    throw new ArgumentException($"Supported Player count length cannot exceed {ConstValues.MaxGamePlayercountLength} chars");
                }
                _supportedPlayerCount = value;
            }
        }

        /// <summary>
        /// The ESRB content descriptors
        /// </summary>
        public string EsrbDescriptors
        {
            get => _esrbDescriptors;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("ESRB Descriptors cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("ESRB Descriptors t contains invalid characters");
                }
                if (value.Length > ConstValues.MaxGamePlayercountLength)
                {
                    throw new ArgumentException($"ESRB Descriptors cannot exceed {ConstValues.MaxGamePlayercountLength} chars");
                }
                _esrbDescriptors = value;
            }
        }

        /// <summary>
        /// Detailed summary of the ESRB rating
        /// </summary>
        public string EsrbSummary
        {
            get => _esrbSummary;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("ESRB summary cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("ESRB summary contains invalid characters");
                }
                if (value.Length > ConstValues.MaxGamePlayercountLength)
                {
                    throw new ArgumentException($"ESRB summary length cannot exceed {ConstValues.MaxGamePlayercountLength} chars");
                }
                _esrbSummary = value;
            }
        }

        /// <summary>
        /// Int value representing the favorite status of the game
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// The ESRB content rating
        /// </summary>
        public Enums.Esrb EsrbRating { get; set; }

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
        private string _supportedPlayerCount;

        /// <summary>
        /// The ESRB content descriptors
        /// </summary>
        private string _esrbDescriptors;

        /// <summary>
        /// Detailed summary of the ESRB rating
        /// </summary>
        private string _esrbSummary;

        /// <summary>
        /// The number of times the game has been launched
        /// </summary>
        private int _launchCount;

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
            _launchCount = 0;
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
        /// <param name="supportedPlayerCount">The supported number of players</param>
        /// <param name="trivia">Trivia facts for the current game</param>
        /// <param name="esrbRating">The ESRB content rating</param>
        /// <param name="esrbDescriptor">The ESRB content descriptors</param>
        /// <param name="esrbSummary">Detailed summary of the ESRB rating</param>
        /// <param name="description">Brief game description or overview</param>
        /// <param name="genres">The genere(s) for the current game</param>
        /// <param name="tags">A list of common tags tags for the current game</param>
        /// <param name="isFavorite"></param>
        public Game(string fileName, string consoleName, int launchCount, string releaseDate, string publisherName, string developerName, string userReviewScore, string criticScore, string supportedPlayerCount, string trivia, Enums.Esrb esrbRating, string esrbDescriptor, string esrbSummary, string description, string genres, string tags, string isFavorite)
        {
            FileName = fileName;
            ConsoleName = consoleName;
            _launchCount = launchCount;
            ReleaseDate = releaseDate;
            PublisherName = publisherName;
            DeveloperName = developerName;
            UserReviewScore = userReviewScore;
            CriticReviewScore = criticScore;
            SupportedPlayerCount = supportedPlayerCount;
            Trivia = trivia;
            EsrbRating = esrbRating;
            Description = description;
            EsrbDescriptors = esrbDescriptor;
            EsrbSummary = esrbSummary;
            Genres = genres;
            Tags = tags;

            Favorite = (isFavorite.Equals("True"));

            //Parse the game title from the raw ROM filename
            if (fileName.Length > 2)
            {
                Title = fileName.Substring(0, fileName.IndexOf('.'));
            }
        }

        #endregion

        #region  Public Methods

        /// <summary>
        /// Return the number of times the game has been launched
        /// </summary>
        /// <returns>LaunchCount</returns>
        public int GetLaunchCount()
        {
            return _launchCount;
        }

        /// <summary>
        /// Increments the current launch count by 1
        /// </summary>
        public void IncrementLaunchCount()
        {
            _launchCount++;
        }

        /// <summary>
        /// Reets the current launch count to 0
        /// </summary>
        public void ResetLaunchCount()
        {
            _launchCount = 0;
        }

        #endregion
    }
}
