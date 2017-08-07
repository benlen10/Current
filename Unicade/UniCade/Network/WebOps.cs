using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UniCade.Constants;
using UniCade.Interfaces;

namespace UniCade
{
    class WebOps
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

        /// <summary>
        /// Specifies if the max number of characters allowed for a game description
        /// </summary>
        public const int MAXDESCRIPTIONLENGTH = 5000;

        #endregion

        #region Public Methods

        /// <summary>
        /// Scrape game info for the specified game from online databases
        /// </summary>
        public static bool ScrapeInfo(IGame game)
        {
            if(game == null) { return false; }
            //Replace invalid chars within game title
            CurrentGameName = game.Title.Replace(" - ", " ");
            CurrentGameName = CurrentGameName.Replace(" ", "-");
            CurrentGameName = CurrentGameName.Replace("'", "");

            //If neither site is scraped, return false
            if (ScanMobygames && ScanMetacritic)
            {
                return false;
            }

            //Attempt to scrape mobygames if the site setting is enabled
            if (ScanMobygames)
            {
                if (!ScrapeMobyGames(game))
                {
                    return false;
                }
            }

            //Attempt to scrape metacritic if the site setting is enabled
            if (ScanMetacritic)
            {
                if (!ScrapeMetacritic(game))
                {
                    return false;
                }
            }

            //If neither site returns any errors, return true
            return true;
        }

        /// <summary>
        /// Scrape info for the specified game from Mobygames.com
        /// </summary>
        public static bool ScrapeMobyGames(IGame game)
        {
            //Check for bad input
            if (game == null)
            {
                MessageBox.Show("Invalid game");
                return false;
            }

            //Generate the target url and convert the game title to lower case
            string url = ("http://www.mobygames.com/game/" + game.ConsoleName + "/" + CurrentGameName);
            url = url.ToLower();

            //Create a new WebClient and attempt a connection
            WebClient site = new WebClient();
            string html = "";
            try
            {
                html = site.DownloadString(url);
            }
            catch
            {
                MessageBox.Show("Connection Error");
                return false;
            }

            //Parse ESRB rating from Mobygames
            if (ParseEsrbRating)
            {
                int indexA = html.IndexOf("ESRB");
                if (indexA < 0)
                {
                    indexA = 0;
                }
                string s = html; 
                
                //Convert the parsed text to a valid ESRB rating
                if (s.Contains("Everyone"))
                {
                    game.EsrbRating = Enums.ESRB.Everyone;
                }
                else if (s.Contains("Kids to Adults"))
                {
                    game.EsrbRating = Enums.ESRB.Everyone;
                }
                else if (s.Contains("Everyone 10+"))
                {
                    game.EsrbRating = Enums.ESRB.Everyone10;
                }
                else if (s.Contains("Teen"))
                {
                    game.EsrbRating = Enums.ESRB.Teen;
                }
                else if (s.Contains("Mature"))
                {
                    game.EsrbRating = Enums.ESRB.Mature;
                }
                else if (s.Contains("Adults Only"))
                {
                    game.EsrbRating = Enums.ESRB.AO;
                }
            }

            //Parse Release Date
            if (ParseReleaseDate)
            {
                //Locate the "release-info" tag within the HTML text
                int tempCharIndex = html.IndexOf("release-info");

                //If the parsed index is valid, set the game release date to the value of the parsed text
                if (tempCharIndex > 0)
                {
                    int indexB = html.IndexOf("release-info", (tempCharIndex + 20));
                    game.ReleaseDate = html.Substring((indexB + 14), 4);
                }

                //Parse Critic Score
                tempCharIndex = 0;
                tempCharIndex = html.IndexOf("scoreHi");

                //If the parsed index is valid, set the critic score to the value of the parsed text
                if (tempCharIndex > 0)
                {
                    string criticScore = html.Substring((tempCharIndex + 9), 2); 
                    game.CriticReviewScore = html.Substring((tempCharIndex + 9));
                }
            }

            //Parse Publisher
            if (ParsePublisher)
            {
                int tempCharIndex = 0;
                tempCharIndex = html.IndexOf("/company/");

                //If the parsed index is valid, set the game company to the value of the parsed text
                if (tempCharIndex > 0)
                {
                    int tempCharIndex2 = html.IndexOf("-", tempCharIndex + 10);
                    game.PublisherName = html.Substring((tempCharIndex + 9), tempCharIndex2 - (tempCharIndex + 9));
                }
            }

            //Parse description
            if (ParseDescription)
            {
                int tempCharIndex = 0;
                tempCharIndex = html.IndexOf("Description<");

                //Locate the beginning of the game description
                if (tempCharIndex > 0)
                {
                    //Locate the end of the game description text
                    int tempCharIndex2 = html.IndexOf("<div class", tempCharIndex + 15);

                    //If the parsed index is valid, set the game description to the value of the parsed text
                    if (tempCharIndex2 > 0)
                    {
                        //Remove invalid characters from the description
                        string description = html.Substring((tempCharIndex + 16), tempCharIndex2 - (tempCharIndex + 16));
                        description = RemoveInvalidChars(description);

                        //Trim the description if it exceeds the max length
                        if (description.Length > MAXDESCRIPTIONLENGTH)
                        {
                            description = description.Substring(0, MAXDESCRIPTIONLENGTH);
                        }

                        //Set the game description to the formatted string
                        game.Description = description;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Scrape Metacritic for info related to the specific game
        /// </summary>
        public static bool ScrapeMetacritic(IGame game)
        {
            string consoleName = "";

            //Convert the console to the string used by metacritic
            if (game.ConsoleName.Equals("PS1"))
            {
                consoleName = "playstation";
            }
            else if (game.ConsoleName.Equals("N64"))
            {
                consoleName = "nintendo-64";
            }
            else if (game.ConsoleName.Equals("GBA"))
            {
                consoleName = "game-boy-advance";
            }
            else if (game.ConsoleName.Equals("PSP"))
            {
                consoleName = "psp";
            }
            else if (game.ConsoleName.Equals("Gamecube"))
            {
                consoleName = "gamecube";
            }
            else if (game.ConsoleName.Equals("Wii"))
            {
                consoleName = "wii";
            }
            else if (game.ConsoleName.Equals("NDS"))
            {
                consoleName = "ds";
            }
            else if (game.ConsoleName.Equals("Dreamcast"))
            {
                consoleName = "dreamcast";
            }

            //Return false if the console is not supported
            if (consoleName.Length < 1)
            {
                return false;
            }

            //Generate the target metacritic url
            string url = ("http://www.metacritic.com/game/" + consoleName + "/" + CurrentGameName + "/details");
            url = url.ToLower();

            //Generate the WebRequest from the url and set the user agent to a supported browser
            var http = (HttpWebRequest)WebRequest.Create(url);
            http.UserAgent = "Chrome";

            //Attempt to connect to metacritic.com
            string html;
            try
            {
                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                html = sr.ReadToEnd();
                response.Close();
            }
            catch
            {
                MessageBox.Show("Connection Error");
                return false;
            }

            //Parse ESRB descriptors
            if (ParseEsrbDescriptors)
            {
                int tempCharIndex = 0;
                tempCharIndex = html.IndexOf("ESRB Descriptors:");

                //If the parsed index is valid, set the ESRB rating to the value of the parsed text
                if (tempCharIndex > 0)
                {
                    //Locate the end of the Rating tag
                    int tempCharIndex2 = html.IndexOf("</td>", tempCharIndex + 26);
                    if (tempCharIndex2 > 0)
                    {
                        game.EsrbDescriptors = html.Substring((tempCharIndex + 26), tempCharIndex2 - (tempCharIndex + 26));
                    }
                }
            }

            //Parse player count (Metacritic)
            if (ParsePlayerCount)
            {       
                int tempCharIndex = 0;
                tempCharIndex = html.IndexOf("Players");

                //If the parsed index is valid, set the player count to the value of the parsed text
                if (tempCharIndex > 0)
                {
                    int tempCharIndex2 = html.IndexOf("<", tempCharIndex + 17);
                    if (tempCharIndex2 > 0)
                    {
                        game.PlayerCount = html.Substring((tempCharIndex + 17), tempCharIndex2 - (tempCharIndex + 17));
                    }
                }
            }
            return true;
            }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Remove and replace all invalid chars from the input string
        /// </summary>
        private static string RemoveInvalidChars(string str)
        {
            str = Regex.Replace(str, @"\t|\n|\r", " ");
            return str.Replace("\"", "");
        }

        #endregion
    }
}
