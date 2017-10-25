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
using Console = UniCade.Objects.Console;

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
            SqlLiteClient.CreateNewSqlDatabase();
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
            SqlLiteClient.CreateNewSqlDatabase();

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
            const string otherConsoles = "GBA";
            const string videoLink = "htp:\\youtube.com";
            const int gamesdbApiId = 001;
            const int mobygamesApiId = 001;
            const string mobyGamesUrl = "mobygamesUrl";
            const int igdbApiId = 003;


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
                SupportedPlayerCount = supportedPlayers,
                OtherPlatforms = otherConsoles,
                VideoLink = videoLink,
                GamesdbApiId = gamesdbApiId,
                MobygamesApiId = mobygamesApiId,
                MobyGamesUrl = mobyGamesUrl,
                IgdbApiId = igdbApiId
            };
            game.SetLaunchCount(launchCount);

            //Upload the game
            Assert.IsTrue(SqlLiteClient.UploadGame(game), "Verify the game is uploaded properly");

            //Download the game info
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
            Assert.AreEqual(otherConsoles, game.OtherPlatforms, "Verify that the other playforms list is correct");
            Assert.AreEqual(videoLink, game.VideoLink, "Verify that the video link is correct");
            Assert.AreEqual(gamesdbApiId, game.GamesdbApiId, "Verify that the GamesdbApiId is correct");
            Assert.AreEqual(mobygamesApiId, game.MobygamesApiId, "Verify that the MobygamesApiId is correct");
            Assert.AreEqual(mobyGamesUrl, game.MobyGamesUrl, "Verify that the MobyGamesUrl is correct");
            Assert.AreEqual(igdbApiId, game.IgdbApiId, "Verify that the IgdbApiId is correct");
        }

        /// <summary>
        /// Verify that console info is properly uploaded and downloaded from the SQL database
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void TestUploadDownloadConsoles()
        {
            SqlLiteClient.Connect();
            SqlLiteClient.CreateNewSqlDatabase();

            const string userName = "BenLen";
            const string password = "tempPass";

            SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null");
            Assert.IsTrue(SqlLiteClient.Login(userName, password));

            //Create a new console and populate all fields
            const string consoleName = "SNES";
            const string releaseDate = "1992";
            const string emulatorExePath = @"C:\temp\zsnes.exe";
            const string romFolderPath = @"C:\Roms\";
            const string romExtensions = ".snes";
            const string consoleInfo = "Description";
            const string launchParams = "args";
            const string developer = "Nintendo";
            const string cpu = "SNES CPU";
            const string ram = "256MB";
            const string graphics = "Properity Nintendo Card";
            const string displayResolution = "640x480";
            const string consoleRating = "9/10";
            const string additionalConsoleInfo = "Addional SNES info";
            const int gamesDbApiId = 001;
            const int mobygamesApiId = 002;
            const int igdbApiId = 003;


            Console console = new Console(consoleName)
            {
                ReleaseDate = releaseDate,
                EmulatorExePath = emulatorExePath,
                RomFolderPath = romFolderPath,
                RomExtension = romExtensions,
                ConsoleInfo = consoleInfo,
                LaunchParams = launchParams,
                Developer = developer,
                Cpu = cpu,
                Ram = ram,
                Graphics = graphics,
                DisplayResolution = displayResolution,
                ConsoleRating = consoleRating,
                AdditionalConsoleInfo = additionalConsoleInfo,
                GamesdbApiId = gamesDbApiId,
                MobygamesApiId = mobygamesApiId,
                IgdbApiId = igdbApiId

            };

            //Upload the console
            SqlLiteClient.UploadConsole(console);

            //Create a new blank console
            console = new Console(consoleName);

            //Download the console info
            SqlLiteClient.DownloadConsoleInfo(console);

            //Verify that the console info has been properly restored
            Assert.AreEqual(consoleName, console.ConsoleName, "Verify that the ConsoleName is correct");
            Assert.AreEqual(releaseDate, console.ReleaseDate, "Verify that the releaseDate is correct");
            Assert.AreEqual(emulatorExePath, console.EmulatorExePath, "Verify that the emulatorExePath is correct");
            Assert.AreEqual(romFolderPath, console.RomFolderPath, "Verify that the romFolderPath is correct");
            Assert.AreEqual(romExtensions, console.RomExtension, "Verify that the romExtensions is correct");
            Assert.AreEqual(consoleInfo, console.ConsoleInfo, "Verify that the consoleInfo is correct");
            Assert.AreEqual(launchParams, console.LaunchParams, "Verify that the launchParams is correct");
            Assert.AreEqual(developer, console.Developer, "Verify that the Developer is correct");
            Assert.AreEqual(cpu, console.Cpu, "Verify that the Cpu is correct");
            Assert.AreEqual(ram, console.Ram, "Verify that the Ram is correct");
            Assert.AreEqual(graphics, console.Graphics, "Verify that the Graphics is correct");
            Assert.AreEqual(displayResolution, console.DisplayResolution, "Verify that the DisplayResolution is correct");
            Assert.AreEqual(consoleRating, console.ConsoleRating, "Verify that the ConsoleRating is correct");
            Assert.AreEqual(additionalConsoleInfo, console.AdditionalConsoleInfo, "Verify that the AdditionalConsoleInfo is correct");
            Assert.AreEqual(gamesDbApiId, console.GamesdbApiId, "Verify that the GamesdbApiId is correct");
            Assert.AreEqual(mobygamesApiId, console.MobygamesApiId, "Verify that the MobygamesApiId is correct");
            Assert.AreEqual(igdbApiId, console.IgdbApiId, "Verify that the IgdbApiId is correct");
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
            SqlLiteClient.CreateNewSqlDatabase();

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
