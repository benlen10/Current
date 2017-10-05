using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
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
        private const string mobygamesApiBaseUrl = "https://api.mobygames.com/v1/games?title=";

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A list of MobyGame objects</returns>
        public static async Task<List<MobyGameResult>> FetchGameInfo(Game game)
        {
            //Replace all spaces with underscores for the api request
            string title = game.Title.Replace(' ', '_');
            using (var httpClient = new HttpClient())
            {
                string mobyUrl = mobygamesApiBaseUrl + gameTitle + "&api_key=" + ConstValues.MobyGamesApiKey;
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(mobyUrl);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string result = await httpResponseMessage.Content.ReadAsStringAsync();
                    var rootResult = JsonConvert.DeserializeObject<MobyRootObject>(result);
                    return rootResult.games;
                }
                    return null;
            }
        }

        #endregion



#endregion
    }
}
