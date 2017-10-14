using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Objects;

namespace UniCade.Network
{
    internal class MobyGamesApi
    {
        #region Properties

        /// <summary>
        /// The base URL for the MobyGames api
        /// </summary>
        private const string MobygamesApiBaseUrl = "https://api.mobygames.com/v1/games?title=";

        #endregion

        #region Public Methods
        /// <summary>
        /// Update the info for the Game object with the MobyGames API
        /// </summary>
        /// <returns>A list of MobyGame objects</returns>
        public static async Task<List<MobyGameResult>> FetchGameInfo(IGame game)
        {
            //Replace all spaces with underscores for the api request
            string title = game.Title.Replace(' ', '_');
            using (var httpClient = new HttpClient())
            {
                string mobyUrl = MobygamesApiBaseUrl + title + "&api_key=" + ConstValues.MobyGamesApiKey;
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(mobyUrl);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string result = await httpResponseMessage.Content.ReadAsStringAsync();
                    var rootResult = JsonConvert.DeserializeObject<MobyRootObject>(result);
                    var gameResult = rootResult.games.First();

                    //If a game was located, populate the game object fields
                    if (gameResult != null)
                    {
                        game.MobygamesApiId = gameResult.game_id;
                        game.MobyGamesUrl = gameResult.moby_url;
                        if (WebOps.ParseDescription)
                        {
                            game.Description = gameResult.description;
                        }
                        if (WebOps.ParseGenres)
                        {
                            game.Genres = ConvertGenreListToString(gameResult.genres);
                        }
                        if (WebOps.ParseOtherPlatforms)
                        {
                            game.OtherPlatforms = ConvertPlatformListToString(gameResult.platforms);
                        }
                        if (WebOps.ParseUserScore)
                        {
                            game.UserReviewScore = gameResult.moby_score.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    return rootResult.games;
                }
                return null;
            }
        }


        #endregion

        #region  Helper Methods

        /// <summary>
        /// Convert a list of MobyGenre objects to a single comma seperated string
        /// </summary>
        /// <param name="genreList">List of MobyGenre objects</param>
        /// <returns>a comma seperated string of genres</returns>
        private static string ConvertGenreListToString(List<MobyGenre> genreList)
        {
            StringBuilder resultString = new StringBuilder();
            genreList.ForEach(g => resultString.Append(g.genre_name + ", "));

            //Trim the last trailing comma
            resultString.Remove(resultString.Length - 2, 2);
            return resultString.ToString();
        }

        /// <summary>
        /// Convert a list of MobyPlatform objects to a single comma seperated string
        /// </summary>
        /// <param name="platformList">List of MobyPlatform objects</param>
        /// <returns>a comma seperated string of platforms</returns>
        private static string ConvertPlatformListToString(List<MobyPlatform> platformList)
        {
            StringBuilder resultString = new StringBuilder();
            platformList.ForEach(p => resultString.Append(p.platform_name + ", "));

            //Trim the last trailing comma
            resultString.Remove(resultString.Length - 2, 2);
            return resultString.ToString();
        }

        #endregion

    }
}
