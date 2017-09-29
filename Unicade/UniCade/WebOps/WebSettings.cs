using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Windows.Forms;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Resources;

namespace UniCade.WebOps
{
    internal class WebSettings
    {
        #region Properties

        /// <summary>
        /// Specifies if Metacritic.com will be scraped
        /// </summary>
        public static bool ScanMetacritic = true;

        /// <summary>
        /// Specifies if Metacritic.com will be scraped
        /// </summary>
        public static bool ScanMobygames = true;

        /// <summary>
        /// Specifies if the publisher will be scraped
        /// </summary>
        public static bool ParsePublisher = true;

        /// <summary>
        /// Specifies if the critic score will be scraped
        /// </summary>
        public static bool ParseCriticScore = true;

        /// <summary>
        /// Specifies if the developer will be scraped
        /// </summary>
        public static bool ParseDeveloper = true;

        /// <summary>
        /// Specifies if the description will be scraped
        /// </summary>
        public static bool ParseDescription = true;

        /// <summary>
        /// Specifies if the ESRB rating will be scraped
        /// </summary>
        public static bool ParseEsrbRating = true;

        /// <summary>
        /// Specifies if the ESRB descriptor will be scraped
        /// </summary>
        public static bool ParseEsrbDescriptors = true;

        /// <summary>
        /// Specifies if the player count will be scraped
        /// </summary>
        public static bool ParsePlayerCount = true;

        /// <summary>
        /// Specifies if the release date will be scraped
        /// </summary>
        public static bool ParseReleaseDate = true;

        /// <summary>
        /// Specifies if the box front image will be scraped
        /// </summary>
        public static bool ParseBoxFrontImage = true;

        /// <summary>
        /// Specifies if the box back image will be scraped
        /// </summary>
        public static bool ParseBoxBackImage = true;

        /// <summary>
        /// Specifies if the screenshot will be scraped
        /// </summary>
        public static bool ParseScreenshot = true;

        /// <summary>
        /// Specifies if the current game name will be scraped
        /// </summary>
        public static string CurrentGameName;

        #endregion

        #region Public Methods

        /// <summary>
        /// Scrape game info for the specified game from online databases
        /// </summary>
        public static bool ScrapeInfo(IGame game)
        {
            if (game == null) { return false; }
            //Replace invalid chars within game title


            //If neither site returns any errors, return true
            return true;
        }

        #endregion
    }
}
