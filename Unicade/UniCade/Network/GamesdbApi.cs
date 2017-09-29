using System;
using System.Collections;
using System.Xml;
using UniCade.Backend;
using UniCade.Interfaces;
using Console = UniCade.Objects.Console;

namespace UniCade.Network
{
    /// <summary>
    /// Fetches information from TheGamesDB.
    /// </summary>
    public static class GamesdbApi
    {

        #region Properties 

        /// <summary>
        /// The base image path that should be prepended to all the relative image paths to get the full paths to the images.
        /// </summary>
        public const String BaseImgUrl = @"http://thegamesdb.net/banners/";

        #endregion

        #region  Public Methods


        /// <summary>
        /// Gets a collection of games matched up with loose search terms.
        /// </summary>
        /// <returns>A collection of games that matched the search terms</returns>
        public static bool UpdateGameInfo(IGame game)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"http://thegamesdb.net/api/GetGamesList.php?name=" + game.Title + @"&platform=" +
                             game.ConsoleName);

            //Debug
            xmlDocument.Save("searchresults.xml");

            //Set the root node and fetch the enumerator
            XmlNode rootNode = xmlDocument.DocumentElement;

            if (rootNode == null)
            {
                return false;
            }

            IEnumerator enumerator = rootNode.GetEnumerator();
            // Iterate through all games
            while (enumerator.MoveNext())
            {
                int gameId = -1;
                var gameNode = (XmlNode)enumerator.Current;

                IEnumerator ienumGame = gameNode.GetEnumerator();
                while (ienumGame.MoveNext())
                {
                    var attributeNode = (XmlNode)ienumGame.Current;

                    //Locate the game id attribute
                    if (attributeNode.Name == "id")
                    {
                        int.TryParse(attributeNode.InnerText, out gameId);
                        break;
                    }
                }

                if (gameId < 0)
                {
                    return false;
                }

                xmlDocument = new XmlDocument();
                xmlDocument.Load(@"http://thegamesdb.net/api/GetGame.php?id=" + gameId);
                xmlDocument.Save("gameinfo.xml");

                rootNode = xmlDocument.DocumentElement;

                XmlNode platformNode = rootNode?.FirstChild.NextSibling;

                if (platformNode == null)
                {
                    return false;
                }

                ienumGame = platformNode.GetEnumerator();
                while (ienumGame.MoveNext())
                {
                    var attributeNode = (XmlNode)ienumGame.Current;

                    // Iterate through all platform attributes
                    if (attributeNode.Name == "Overview")
                    {
                        game.Description = attributeNode.InnerText;
                    }
                    else if (attributeNode.Name == "overview")
                    {
                        game.Description = attributeNode.InnerText;
                    }
                    else if (attributeNode.Name == "ESRB")
                    {
                        game.EsrbRatingsRating = Utilties.ParseEsrbRating(attributeNode.InnerText);
                    }
                    else if (attributeNode.Name == "Players")
                    {
                        game.SupportedPlayerCount = attributeNode.InnerText;
                    }
                    else if (attributeNode.Name == "Publisher")
                    {
                        game.PublisherName = attributeNode.InnerText;
                    }
                    else if (attributeNode.Name == "Developer")
                    {
                        game.DeveloperName = attributeNode.InnerText;
                    }
                    else if (attributeNode.Name == "Rating")
                    {
                        game.UserReviewScore = attributeNode.InnerText;
                    }
                    else if (attributeNode.Name == "Genres")
                    {
                        IEnumerator ienumGenres = attributeNode.GetEnumerator();
                        while (ienumGenres.MoveNext())
                        {
                            game.Genres += ((XmlNode)ienumGenres.Current).InnerText + " ";
                        }
                    }
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Gets all data for a specific platform.
        /// </summary>
        internal static bool UpdateConsoleInfo(Console console)
        {
            int consoleId = -1;
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetPlatform.php?id=" + consoleId);

            XmlNode root = doc.DocumentElement;
            if (root != null)
            {
                root.GetEnumerator();

                XmlNode platformNode = root.FirstChild.NextSibling;

                if (platformNode != null)
                {
                    IEnumerator ienumPlatform = platformNode.GetEnumerator();
                    while (ienumPlatform.MoveNext())
                    {
                        var attributeNode = (XmlNode)ienumPlatform.Current;

                        // Iterate through all platform attributes
                        if (attributeNode.Name == "overview")
                        {
                            console.ConsoleInfo = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "developer")
                        {
                            //console.Developer = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "manufacturer")
                        {
                            //console.Manufacturer = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "cpu")
                        {
                            //console.CPU = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "memory")
                        {
                            //console.Memory = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "graphics")
                        {
                            //console.Graphics = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "sound")
                        {
                            //console.Sound = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "display")
                        {
                            //console.Display = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "media")
                        {
                            //console.Media = attributeNode.InnerText;
                        }
                        else if (attributeNode.Name == "maxcontrollers")
                        {
                           // int.TryParse(attributeNode.InnerText, out platform.MaxControllers);
                        }
                        else if (attributeNode.Name == "Rating")
                        {
                            //float.TryParse(attributeNode.InnerText, out platform.Rating);
                        }
                        else if (attributeNode.Name == "Images")
                        {
                           // platform.Images.FromXmlNode(attributeNode);
                        }
                    }
                }

                return true;
            }
            return false;
        }

        #endregion
    }
}
