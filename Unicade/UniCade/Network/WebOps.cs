using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UniCade
{
    class WebOps
    {
        #region Properties

        public static int ScanMetacritic = 1;
        public static int ScanMobygames = 1;
        public static int Publisher = 1;
        public static int CriticScore = 1;
        public static int Developer = 1;
        public static int Description = 1;
        public static int EsrbRating = 1;
        public static int EsrbDescriptor = 1;
        public static int PlayerCount = 1;
        public static int ReleaseDate = 1;
        public static int BoxFrontImage = 1;
        public static int BoxBackImage = 1;
        public static int Screenshot = 1;
        public static string CurrentGameName;
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
            if (ScanMobygames == 0 && ScanMetacritic == 0)
            {
                return false;
            }

            //Attempt to scrape mobygames if the site setting is enabled
            if (ScanMobygames > 0)
            {
                if (!ScrapeMobyGames(game))
                {
                    return false;
                }
            }

            //Attempt to scrape metacritic if the site setting is enabled
            if (ScanMetacritic > 0)
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
        public static bool ScrapeMobyGames(IGame g)
        {
            //Check for bad input
            if (g == null)
            {
                MessageBox.Show("Invalid game");
                return false;
            }

            //Generate the target url and convert the game title to lower case
            string url = ("http://www.mobygames.com/game/" + g.ConsoleName + "/" + CurrentGameName);
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
            if (EsrbRating > 0)
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
                    g.EsrbRating = "Everyone";
                }
                else if (s.Contains("Kids to Adults"))
                {
                    g.EsrbRating = "Everyone (KA)";
                }
                else if (s.Contains("Everyone 10+"))
                {
                    g.EsrbRating = "Everyone 10+";
                }
                else if (s.Contains("Teen"))
                {
                    g.EsrbRating = "Teen";
                }
                else if (s.Contains("Mature"))
                {
                    g.EsrbRating = "Mature";
                }
                else if (s.Contains("Mature"))
                {
                    g.EsrbRating = "Mature";
                }
                else if (s.Contains("Adults Only"))
                {
                    g.EsrbRating = "AO (Adults Only)";
                }
            }

            //Parse Release Date
            if (ReleaseDate > 0)
            {
                //Locate the "release-info" tag within the HTML text
                int tmp = html.IndexOf("release-info");

                //If the parsed index is valid, set the game release date to the value of the parsed text
                if (tmp > 0)
                {
                    int indexB = html.IndexOf("release-info", (tmp + 20));
                    g.ReleaseDate = html.Substring((indexB + 14), 4);
                }

                //Parse Critic Score
                tmp = 0;
                tmp = html.IndexOf("scoreHi");

                //If the parsed index is valid, set the critic score to the value of the parsed text
                if (tmp > 0)
                {
                    string criticScore = html.Substring((tmp + 9), 2); 
                    g.CriticReviewScore = html.Substring((tmp + 9));
                }
            }

            //Parse Publisher
            if (Publisher > 0)
            {
                int tmp = 0;
                tmp = html.IndexOf("/company/");

                //If the parsed index is valid, set the game company to the value of the parsed text
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("-", tmp + 10);
                    g.PublisherName = html.Substring((tmp + 9), tmp2 - (tmp + 9));
                }
            }

            //Parse description
            if (Description > 0)
            {
                int tmp = 0;
                tmp = html.IndexOf("Description<");

                //Locate the beginning of the game description
                if (tmp > 0)
                {
                    //Locate the end of the game description text
                    int tmp2 = html.IndexOf("<div class", tmp + 15);

                    //If the parsed index is valid, set the game description to the value of the parsed text
                    if (tmp2 > 0)
                    {
                        //Remove invalid characters from the description
                        string description = html.Substring((tmp + 16), tmp2 - (tmp + 16));
                        description = RemoveInvalidChars(description);

                        //Trim the description if it exceeds the max length
                        if (description.Length > MAXDESCRIPTIONLENGTH)
                        {
                            description = description.Substring(0, MAXDESCRIPTIONLENGTH);
                        }

                        //Set the game description to the formatted string
                        g.Description = description;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Scrape Metacritic for info related to the specific game
        /// </summary>
        public static bool ScrapeMetacritic(IGame g)
        {
            string metaCon = "";

            //Convert the console to the string used by metacritic
            if (g.ConsoleName.Equals("PS1"))
            {
                metaCon = "playstation";
            }
            else if (g.ConsoleName.Equals("N64"))
            {
                metaCon = "nintendo-64";
            }
            else if (g.ConsoleName.Equals("GBA"))
            {
                metaCon = "game-boy-advance";
            }
            else if (g.ConsoleName.Equals("PSP"))
            {
                metaCon = "psp";
            }
            else if (g.ConsoleName.Equals("Gamecube"))
            {
                metaCon = "gamecube";
            }
            else if (g.ConsoleName.Equals("Wii"))
            {
                metaCon = "wii";
            }
            else if (g.ConsoleName.Equals("NDS"))
            {
                metaCon = "ds";
            }
            else if (g.ConsoleName.Equals("Dreamcast"))
            {
                metaCon = "dreamcast";
            }

            //Return false if the console is not supported
            if (metaCon.Length < 1)
            {
                return false;
            }

            //Generate the target metacritic url
            string url = ("http://www.metacritic.com/game/" + metaCon + "/" + CurrentGameName + "/details");
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
            if (EsrbDescriptor > 0)
            {
                int tmp = 0;
                tmp = html.IndexOf("ESRB Descriptors:");

                //If the parsed index is valid, set the ESRB rating to the value of the parsed text
                if (tmp > 0)
                {
                    //Locate the end of the Rating tag
                    int tmp2 = html.IndexOf("</td>", tmp + 26);
                    if (tmp2 > 0)
                    {
                        g.EsrbDescriptors = html.Substring((tmp + 26), tmp2 - (tmp + 26));
                    }
                }
            }

            //Parse player count (Metacritic)
            if (PlayerCount > 0)
            {       
                int tmp = 0;
                tmp = html.IndexOf("Players");

                //If the parsed index is valid, set the player count to the value of the parsed text
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("<", tmp + 17);
                    if (tmp2 > 0)
                    {
                        g.PlayerCount = html.Substring((tmp + 17), tmp2 - (tmp + 17));
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
