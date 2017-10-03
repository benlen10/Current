using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Objects;

namespace UnitTests.NetworkTests
{
    [TestClass]
    public class ApiTests
    {
        /// <summary>
        /// Initalize the database
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();
        }

        #region API Tests

        /// <summary>
        /// Verify that the GamesDB API properly fetches info for games
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyGamesDbApiScraping()
        {
            //Create a new game (Resident Evil 2)

            //Attempt to fetch info from GameDB API

            //Verify that the game info has been properly scraped
            //TODO: Verify all fields
        }

        /// <summary>
        /// Verify that the MobyGames API properly fetches info for games
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyMobyGamesApiScraping()
        {
            //Create a new game (Resident Evil 2)

            //Attempt to fetch info from MobyGames API

            //Verify that the game info has been properly scraped
            //TODO: Verify all fields
        }

        #endregion
    }
}
