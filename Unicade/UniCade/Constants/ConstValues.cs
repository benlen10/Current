
namespace UniCade.Constants
{
    internal static class ConstValues
    {
        #region Constants 

        /// <summary>
        /// The maximum number of consoles allowed 
        /// </summary>
        public const int MaxConsoleCount = 100;

        /// <summary>
        /// The maximum number of games allowed per console
        /// </summary>
        public const int MaxGameCount = 5000;

        /// <summary>
        /// The maximum number of users allowed 
        /// </summary>
        public const int MaxUserCount = 50;

        /// <summary>
        /// The max length for console
        /// </summary>
        public const int MaxConsoleNameLength = 35;

        /// <summary>
        /// The max length for directory paths
        /// </summary>
        public const int MaxPathLength = 1000;

        /// <summary>
        /// The min length for directory paths
        /// </summary>
        public const int MinPathLength = 4;

        /// <summary>
        /// The max length a ROM file extension
        /// </summary>
        public const int MaxFileExtLength = 1000;

        /// <summary>
        /// The max length a the console info
        /// </summary>
        public const int MaxConsoleInfoLength = 1000;

        /// <summary>
        /// The max length a the console info
        /// </summary>
        public const int MaxLaunchParamsLength = 1000;

        /// <summary>
        /// The max char length for a username
        /// </summary>
        public const int MaxUsernameLength = 30;

        /// <summary>
        /// The max char length for user info descriptions
        /// </summary>
        public const int MaxUserInfoLength = 200;

        /// <summary>
        /// The max char length for user email addresses
        /// </summary>
        public const int MaxEmailLength = 200;

        /// <summary>
        /// The max char length for game filenames
        /// </summary>
        public const int MaxGameFilenameLength = 200;

        /// <summary>
        /// The max char length for game titles
        /// </summary>
        public const int MaxGameTitleLength = 200;

        /// <summary>
        /// The max char length for game descriptions
        /// </summary>
        public const int MaxGameDescriptionLength = 500;

        /// <summary>
        /// The max char length for game publisher names
        /// </summary>
        public const int MaxPublisherDeveloperLength = 500;

        /// <summary>
        /// The max char length for game generes
        /// </summary>
        public const int MaxGameGenreLength = 500;

        /// <summary>
        /// The max char length for game tags
        /// </summary>
        public const int MaxGameTagsLength = 500;

        /// <summary>
        /// The max char length for game user/critic review scores
        /// </summary>
        public const int MaxGameReviewScoreLength = 20;

        /// <summary>
        /// The max char length for game trivia
        /// </summary>
        public const int MaxGameTriviaLength = 500;

        /// <summary>
        /// The max char length for game trivia
        /// </summary>
        public const int MaxGamePlayercountLength = 500;

        /// <summary>
        /// The max char length for game trivia
        /// </summary>
        public const int MaxGameEsrbDescriptorsLength = 500;

        /// <summary>
        /// The max char length for game trivia
        /// </summary>
        public const int MaxGameEsrbSummaryLength = 1000;

        #endregion

        #region  Static Readonly Fields

        /// <summary>
        /// Global invalid characters 
        /// </summary>
        public static readonly char[] InvalidChars = new char[] { '|', '*' };

        #endregion
    }
}
