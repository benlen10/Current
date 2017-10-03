using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

namespace UnitTests.BackendTests
{
    [TestClass]
    public class FileOpsTests
    {
        /// <summary>
        /// Create a new database instance
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();
        }

        #region FileOps Tests

        /// <summary>
        /// Verify that data is properly saved and loaded from the database XML file
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifySaveLoadDatabase()
        {
            //Create a new console and populate all fields
            const string consoleName = "SNES";
            const string releaseDate = "1992";
            const string emulatorExePath = @"C:\temp\zsnes.exe";
            const string romFolderPath = @"C:\Roms\";
            const string romExtensions = ".snes";
            const string consoleInfo = "Description";
            const string launchParams = "args";


            Console console = new Console(consoleName)
            {
                ReleaseDate = releaseDate,
                EmulatorExePath = emulatorExePath,
                RomFolderPath = romFolderPath,
                RomExtension = romExtensions,
                ConsoleInfo = consoleInfo,
                LaunchParams = launchParams
            };

            //Create a new game and populate all fields
            const string fileName = "Super Mario World.snes";
            const string gameTitle = "Super Mario World";
            const string gameDescription = "A new Mario adventure";
            const string gameReleaseDate = "1995";
            const string publisherName = "Nintendo";
            const string developerName = "Nintendo";
            const string genres = "Platformer";
            const string tags = "Mario, Platformers";
            const string userReviewScorew = "95/100";
            const string criticReviewScore = "93/100";
            const string trivia = "This game was later ported to the GBA";
            const string supportedPlayers = "2";


            Game game = new Game(fileName, console.ConsoleName)
            {
                Description = gameDescription,
                ReleaseDate = gameReleaseDate,
                PublisherName = publisherName,
                DeveloperName = developerName,
                Genres = genres,
                Tags = tags,
                UserReviewScore = userReviewScorew,
                CriticReviewScore = criticReviewScore,
                Trivia = trivia,
                SupportedPlayerCount = supportedPlayers
            };

            //Increment and save the launch count
            game.IncrementLaunchCount();
            game.IncrementLaunchCount();
            int launchCount = game.GetLaunchCount();

            //Add the game to the console
            console.AddGame(game);

            //Add the console to the database
            Database.AddConsole(console);

            //Save the database XML file
            const string databasePath = "testDatabase.xml";
            FileOps.SaveDatabase(databasePath);

            //Reset the database
            Database.Initalize();

            //Set both the game an console objects to null
            console = null;
            game = null;

            //Verify that the console list has been cleared
            Assert.IsTrue(Database.GetConsoleCount() == 0);

            //Load the database 
            FileOps.LoadDatabase(databasePath);

            //Verify that the console exists
            var consoleList = Database.GetConsoleList();
            var consoleCount = Database.GetConsoleCount();
            console = (Console) Database.GetConsole(consoleName);
            Assert.IsNotNull(console, "Verify that the console exists");

            //Verify that the console info has been properly loaded
            Assert.AreEqual(consoleName, console.ConsoleName, "Verify that the ConsoleName is correct");
            Assert.AreEqual(releaseDate, console.ReleaseDate, "Verify that the releaseDate is correct");
            Assert.AreEqual(emulatorExePath, console.EmulatorExePath, "Verify that the emulatorExePath is correct");
            Assert.AreEqual(romFolderPath, console.RomFolderPath, "Verify that the romFolderPath is correct");
            Assert.AreEqual(romExtensions, console.RomExtension, "Verify that the romExtensions is correct");
            Assert.AreEqual(consoleInfo, console.ConsoleInfo, "Verify that the consoleInfo is correct");
            Assert.AreEqual(launchParams, console.LaunchParams, "Verify that the launchParams is correct");
            Assert.AreEqual(1, console.GetGameCount(), "Verify that the gameCount is correct");

            //Attempt to fetch the game and verify that it exists
            game = (Game) console.GetGame(gameTitle);
            Assert.IsNotNull(game);

            //Verify that game metadata has been properly loaded
            Assert.AreEqual(fileName, game.FileName, "Verify that the game FileName is correct");
            Assert.AreEqual(gameTitle, game.Title, "Verify that the game Title is correct");
            Assert.AreEqual(publisherName, game.PublisherName, "Verify that the game PublisherName is correct");
            Assert.AreEqual(developerName, game.DeveloperName, "Verify that the game DeveloperName is correct");
            Assert.AreEqual(genres, game.Genres, "Verify that the game genres are correct");
            Assert.AreEqual(tags, game.Tags, "Verify that the game tags are correct");
            Assert.AreEqual(userReviewScorew, game.UserReviewScore, "Verify that the game user review score is correct");
            Assert.AreEqual(criticReviewScore, game.CriticReviewScore, "Verify that the game critic review score is correct");
            Assert.AreEqual(trivia, game.Trivia, "Verify that the game trivia is correct");
            Assert.AreEqual(supportedPlayers, game.SupportedPlayerCount, "Verify that the game trivia is correct");
            Assert.AreEqual(launchCount, game.GetLaunchCount(), "Verify that the game launch count is correct");

            //Cleanup and delete the database file
            File.Delete(databasePath);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifySaveLoadPrefrences()
        {
            //Set all preferences

            //Save the preferences XML file

            //Load the preferences XML file

            //Verify that the perferences have been properly loaded
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyRomScanning()
        {
            //Create a new rom directory

            //Populate the new rom directory 

            //Scan the rom directory

            //Verify that the rom files have been properly imported
        }

        #endregion
    }
}
