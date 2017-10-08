using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Network;
using UniCade.Objects;

namespace UnitTests.NetworkTests
{
    [TestClass]
    public class SqlTests
    {
        /// <summary>
        /// Initalize the sqlite database
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            SqlLiteClient.Connect();
            SqlLiteClient.CreateUsersTable();
        }

        #region SQL Tests

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void TestUploadDownloadGames()
        {
            SqlLiteClient.Connect();
            SqlLiteClient.CreateUsersTable();

            const string userName = "BenLen";
            const string password = "tempPass";

            SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null");
            Assert.IsTrue(SqlLiteClient.Login(userName, password));

            //Create a new game and populate all fields
            const string fileName = "Super Mario World.snes";
            const string gameConsole = "SNES";
            const string gameTitle = "Super Mario World";
            const int launchCount = 3;
            const string gameDescription = "A new Mario adventure";
            const string gameReleaseDate = "1995";
            const string publisherName = "Nintendo";
            const string developerName = "Nintendo";
            const Enums.EsrbRatings esrbRating = Enums.EsrbRatings.Everyone;
            const string genres = "Platformer";
            const string tags = "Mario, Platformers";
            const string userReviewScorew = "95/100";
            const string criticReviewScore = "93/100";
            const string trivia = "This game was later ported to the GBA";
            const string supportedPlayers = "2";


            Game game = new Game(fileName, gameConsole)
            {
                Description = gameDescription,
                ReleaseDate = gameReleaseDate,
                PublisherName = publisherName,
                DeveloperName = developerName,
                EsrbRatingsRating = esrbRating,
                Genres = genres,
                Tags = tags,
                UserReviewScore = userReviewScorew,
                CriticReviewScore = criticReviewScore,
                Trivia = trivia,
                SupportedPlayerCount = supportedPlayers
            };
            game.SetLaunchCount(launchCount);

            Assert.IsTrue(SqlLiteClient.UploadGame(game), "Verify the game is uploaded properly");

            game = new Game(fileName, gameConsole);
            SqlLiteClient.DownloadGameInfo(game);

            //Verify the game metadata has been downloaded properly
            Assert.AreEqual(fileName, game.FileName, "Verify that the game FileName is correct");
            Assert.AreEqual(gameTitle, game.Title, "Verify that the game Title is correct");
            Assert.AreEqual(publisherName, game.PublisherName, "Verify that the game PublisherName is correct");
            Assert.AreEqual(developerName, game.DeveloperName, "Verify that the game DeveloperName is correct");
            Assert.AreEqual(esrbRating, game.EsrbRatingsRating, "Verify that the game Esrb rating is correct");
            Assert.AreEqual(genres, game.Genres, "Verify that the game genres are correct");
            Assert.AreEqual(tags, game.Tags, "Verify that the game tags are correct");
            Assert.AreEqual(userReviewScorew, game.UserReviewScore, "Verify that the game user review score is correct");
            Assert.AreEqual(criticReviewScore, game.CriticReviewScore, "Verify that the game critic review score is correct");
            Assert.AreEqual(trivia, game.Trivia, "Verify that the game trivia is correct");
            Assert.AreEqual(supportedPlayers, game.SupportedPlayerCount, "Verify that the game trivia is correct");
            Assert.AreEqual(launchCount, game.GetLaunchCount(), "Verify that the game launch count is correct");
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void TestLoginFunctionality()
        {
            const string userName = "BenLen";
            const string password = "tempPass";
            const string invalidPass = "tempPass2";

            //Create a new user
            Assert.IsTrue(SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null"), "Verify that the user is properly created");

            //Verify that an invalid password is not accepted
            Assert.IsFalse(SqlLiteClient.Login(userName, invalidPass), "Verify that an invalid password is not accepted");

            //Verify that the current user is stil null
            Assert.IsNull(SqlLiteClient.GetCurrentUsername(), "Verify that the current user is stil null");

            //Verify that a valid password returns true
            Assert.IsTrue(SqlLiteClient.Login(userName, password), "Verify that a valid password returns true");

            //Verify that the current user has been updated
            Assert.IsTrue(SqlLiteClient.GetCurrentUsername().Equals(userName), "Verify that the current user has been updated");
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyCreateDeleteUsers()
        {
            const string userName = "BenLen";
            const string password = "tempPass";

            //Create a new user
            Assert.IsTrue(SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null"), "Verify that the user is properly created");

            //Verify that a user with a duplicate username cannot be created
            Assert.IsFalse(SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null"), "Verify that the user is properly created");

            //Verify that the user login is accepted
            Assert.IsTrue(SqlLiteClient.Login(userName, password), "Verify that the user login is accepted");

            //Verify that the current user has been updated
            Assert.IsTrue(SqlLiteClient.GetCurrentUsername().Equals(userName), "Verify that the current user has been updated");

            //Delete the current user
            SqlLiteClient.DeleteCurrentUser();

            //Verify that the current user is now null
            Assert.IsNull(SqlLiteClient.GetCurrentUsername(), "Verify that the current user is now null");
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDeleteUserGames()
        {
            SqlLiteClient.Connect();
            SqlLiteClient.CreateUsersTable();

            const string userName = "BenLen";
            const string password = "tempPass";

            SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null");
            Assert.IsTrue(SqlLiteClient.Login(userName, password));

            //Create a new game
            const string fileName = "Super Mario World.snes";
            const string gameConsole = "SNES";
            Game game = new Game(fileName, gameConsole);

            //Upload the game
            Assert.IsTrue(SqlLiteClient.UploadGame(game), "Verify that the game is sucuessfully uploaded");

            //Confirm that the game exists in the database
            Assert.IsTrue(SqlLiteClient.DownloadGameInfo(game), "Confirm that the game exists in the database");

            //Delete all games for the current user
            SqlLiteClient.DeleteAllUserGames();

            //Verify that the game no longer exists
            Assert.IsFalse(SqlLiteClient.DownloadGameInfo(game), "Verify that the game no longer exists");
        }


        #endregion

        #region  Helper Methods

        /// <summary>
        /// Cleanup the sqlite database file
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            /*
            if (File.Exists(ConstValues.SqlDatabaseFileName))
            {
                File.Delete(ConstValues.SqlDatabaseFileName);
            }
            */
        }

        #endregion
    }
}
