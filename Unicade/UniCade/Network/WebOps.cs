using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UniCade
{
    class WebOps
    {
        #region Properties

        public static int metac = 1;
        public static int mobyg = 1;
        public static int year = 1;
        public static int publisher = 1;
        public static int critic = 1;
        public static int developer = 1;
        public static int description = 1;
        public static int esrb = 1;
        public static int esrbDescriptor = 1;
        public static int players = 1;
        public static int releaseDate = 1;
        public static int boxFront = 1;
        public static int boxBack = 1;
        public static int screenshot = 1;
        public static string gameName;
        public static int maxDescriptionLength = 5000;

        #endregion

        #region Public Methods

        /// <summary>
        /// Scrape game info for the specified game from online databases
        /// </summary>
        public static bool scrapeInfo(Game g)
        {
            //Replace invalid chars within game title
            gameName = g.Title.Replace(" - ", " ");
            gameName = gameName.Replace(" ", "-");
            gameName = gameName.Replace("'", "");

            //If neither site is scraped, return false
            if (mobyg == 0 && metac == 0)
                return false;

            //Attempt to scrape mobygames if the site setting is enabled
            if (mobyg > 0)
                if (!scrapeMobyGames(g))
                    return false;

            //Attempt to scrape metacritic if the site setting is enabled
            if (metac > 0)
                if (!scrapeMetacritic(g))
                    return false;

            //If neither site returns any errors, return true
            return true;
        }

        /// <summary>
        /// Scrape info for the specified game from Mobygames.com
        /// </summary>
        public static bool scrapeMobyGames(Game g)
        {
            //Check for bad input
            if (g == null)
            {
                MessageBox.Show("Invalid game");
                return false;
            }

            //Generate the target url and convert the game title to lower case
            string url = ("http://www.mobygames.com/game/" + g.Console + "/" + gameName);
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
            if (esrb > 0)
            {
                int indexA = html.IndexOf("ESRB");
                if (indexA < 0)
                {
                    indexA = 0;
                }
                string s = html; 
                
                //Convert the parsed text to a valid ESRB rating
                if (s.Contains("Everyone"))
                    g.Esrb = "Everyone";
                else if (s.Contains("Kids to Adults"))
                    g.Esrb = "Everyone (KA)";
                else if (s.Contains("Everyone 10+"))
                    g.Esrb = "Everyone 10+";
                else if (s.Contains("Teen"))
                    g.Esrb = "Teen";
                else if (s.Contains("Mature"))
                    g.Esrb = "Mature";
                else if (s.Contains("Mature"))
                    g.Esrb = "Mature";
                else if (s.Contains("Adults Only"))
                    g.Esrb = "AO (Adults Only)";
            }

            //Parse Release Date
            if (releaseDate > 0)
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
                    g.CriticScore = html.Substring((tmp + 9));
                }
            }

            //Parse Publisher
            if (publisher > 0)
            {
                int tmp = 0;
                tmp = html.IndexOf("/company/");

                //If the parsed index is valid, set the game company to the value of the parsed text
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("-", tmp + 10);
                    g.Publisher = html.Substring((tmp + 9), tmp2 - (tmp + 9));
                }
            }

            //Parse description
            if (description > 0)
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
                        if (description.Length > maxDescriptionLength)
                        {
                            description = description.Substring(0, maxDescriptionLength);
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
        public static bool scrapeMetacritic(Game g)
        {
            string metaCon = "";

            //Convert the console to the string used by metacritic
            if (g.Console.Equals("PS1"))
                metaCon = "playstation";
            else if (g.Console.Equals("N64"))
                metaCon = "nintendo-64";
            else if (g.Console.Equals("GBA"))
                metaCon = "game-boy-advance";
            else if (g.Console.Equals("PSP"))
                metaCon = "psp";
            else if (g.Console.Equals("Gamecube"))
                metaCon = "gamecube";
            else if (g.Console.Equals("Wii"))
                metaCon = "wii";
            else if (g.Console.Equals("NDS"))
                metaCon = "ds";
            else if (g.Console.Equals("Dreamcast"))
                metaCon = "dreamcast";

            //Return false if the console is not supported
            if (metaCon.Length < 1)
                return false;
            
            //Generate the target metacritic url
            string url = ("http://www.metacritic.com/game/" + metaCon + "/" + gameName + "/details");
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
            if (esrbDescriptor > 0)
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
                        g.EsrbDescriptor = html.Substring((tmp + 26), tmp2 - (tmp + 26));
                    }
                }
            }

            //Parse player count (Metacritic)
            if (players > 0)
            {       
                int tmp = 0;
                tmp = html.IndexOf("Players");

                //If the parsed index is valid, set the player count to the value of the parsed text
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("<", tmp + 17);
                    if (tmp2 > 0)
                    {
                        g.Players = html.Substring((tmp + 17), tmp2 - (tmp + 17));
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
