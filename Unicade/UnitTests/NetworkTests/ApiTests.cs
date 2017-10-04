using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Network;
using UniCade.Objects;

namespace UnitTests.NetworkTests
{
    [TestClass]
    public class ApiTests
    {

        #region Properties

        /// <summary>
        /// The current game object for API testing
        /// </summary>
        private Game _game;

        /// <summary>
        /// The current consoleName for API testing
        /// </summary>
        private const string _consoleName = "Sony Playstation";

        #endregion 


        /// <summary>
        /// Initalize the database and create a new game
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();

            //Create a new game 
            _game = new Game("Resident Evil 2.bin", _consoleName);
        }

        #region API Tests

        /// <summary>
        /// Verify that the GamesDB API properly fetches info for games
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyGamesDbApiScraping()
        {
            //Attempt to fetch info from GameDB API
            GamesdbApi.UpdateGameInfo(_game);

            //Verify that the game info has been properly scraped
            Assert.IsTrue(_game.Description.Contains("Suspense"),"Verify that the game desription is properly fetched");
            Assert.AreEqual(Enums.EsrbRatings.Mature, _game.EsrbRatingsRating, "Verify that the ESRB rating is properly fetched");
            Assert.AreEqual("Capcom", _game.PublisherName, "Verify that the publisher is properly fetched");

            //Verify that game images have been properly scraped
            string mediaDirectoryPath = Directory.GetCurrentDirectory() + @"\Media\Games\" + _game.ConsoleName + "\\";
            Assert.IsTrue(File.Exists(mediaDirectoryPath + _game.Title + "_BoxBack.jpg"));
            Assert.IsTrue(File.Exists(mediaDirectoryPath + _game.Title + "_BoxFront.jpg"));
            Assert.IsTrue(File.Exists(mediaDirectoryPath + _game.Title + "_Screenshot.jpg"));
        }

        /// <summary>
        /// Verify that the MobyGames API properly fetches info for games
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyMobyGamesApiScraping()
        {
            //Create a new game
            Game game = new Game("Resident Evil 2.bin", "Sony Playstation");

            //Attempt to fetch info from MobyGames API

            //Verify that the game info has been properly scraped
            //TODO: Verify all fields
        }

        #endregion

        /// <summary>
        /// Cleanup and delete the temp media directory
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(Directory.GetCurrentDirectory() + @"\Media\", true);
        }
    }
}
