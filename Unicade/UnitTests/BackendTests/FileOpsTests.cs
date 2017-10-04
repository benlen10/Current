using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Objects;
using UniCade.Resources;
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
            //Declare all preferences 
            const bool showSplashScreen = true;
            const bool rescanOnStartup = true;
            const Enums.EsrbRatings globalEsrbRestriction = Enums.EsrbRatings.Mature;
            const bool preferCmdInterface = true;
            const bool showLoadingScreen = true;
            const string passwordProtection = "Password";
            const bool enforceFileExtension = true;
            const bool payPerPlayEnabled = true;
            const int coinsRequired = 3;
            const int currentCoins = 5;
            const string userLicenseName = "BenLen10";
            const string userLiceneseKey = "myKey";

            //Set the preferences
            Program.ShowSplashScreen = showSplashScreen;
            Program.RescanOnStartup = rescanOnStartup;
            Program.RestrictGlobalEsrbRatings = globalEsrbRestriction;
            Program.PerferCmdInterface = preferCmdInterface;
            Program.ShowLoadingScreen = showLoadingScreen;
            Program.PasswordProtection = passwordProtection;
            Program.EnforceFileExtensions = enforceFileExtension;
            PayPerPlay.PayPerPlayEnabled = payPerPlayEnabled;
            PayPerPlay.CoinsRequired = coinsRequired;
            PayPerPlay.CurrentCoins = currentCoins;
            Program.UserLicenseName = userLicenseName;
            Program.UserLicenseKey = userLiceneseKey;

            //Create a new user
            const string userName = "benlen10";
            const string userPassword = "tempPass";
            const int userLoginCount = 3;
            const string userEmail = "benlen10@unicade.com";
            const int totalLaunchCount = 11;
            const string userInfo = "tempInfo";
            const Enums.EsrbRatings userAllowedEsrb = Enums.EsrbRatings.Teen;
            const string profPicPath = @"c:\temp\";
            User user = new User(userName, userPassword,userLoginCount, userEmail, totalLaunchCount, userInfo, userAllowedEsrb, profPicPath);

            //Create a new game
            const string gameFileName = "Resident Evil 2.bin";
            const string gameTitle = "Resident Evil 2";
            const string consoleName = "Sony Playstation";
            Game game = new Game(gameFileName, consoleName);

            //Add the game to the user's favorites list
            user.AddFavorite(game);

            //Add the new user to the database
            Database.AddUser(user);

            //Save the preferences XML file
            const string preferencesPath = "testPreferences.xml";
            FileOps.SavePreferences(preferencesPath);

            //Modify the current preferences 
            Program.ShowSplashScreen = false;
            Program.RescanOnStartup = false;
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Null;
            Program.PerferCmdInterface = false;
            Program.ShowLoadingScreen = false;
            Program.PasswordProtection = "none";
            Program.EnforceFileExtensions = false;
            PayPerPlay.PayPerPlayEnabled = false;
            PayPerPlay.CoinsRequired = 1;
            PayPerPlay.CurrentCoins = 0;
            Program.UserLicenseName = "temp";
            Program.UserLicenseKey = "temp";

            //Reset the database and clear the user list
            Database.Initalize();

            //Set the user and game variables to Null
            user = null;
            game = null;

            //Load the preferences XML file
            FileOps.LoadPreferences(preferencesPath);

            //Verify that the preference fields have been properly loaded
            Assert.AreEqual(showSplashScreen, Program.ShowSplashScreen, "Verify that ShowSplashScreen has been loaded properly");
            Assert.AreEqual(rescanOnStartup, Program.RescanOnStartup, "Verify that RescanOnStartup has been loaded properly");
            Assert.AreEqual(globalEsrbRestriction, Program.RestrictGlobalEsrbRatings, "Verify that RestrictGlobalEsrbRatings has been loaded properly");
            Assert.AreEqual(preferCmdInterface, Program.PerferCmdInterface, "Verify that PerferCmdInterface has been loaded properly");
            Assert.AreEqual(passwordProtection, Program.PasswordProtection, "Verify that PasswordProtection has been loaded properly");
            Assert.AreEqual(enforceFileExtension, Program.EnforceFileExtensions, "Verify that EnforceFileExtensions has been loaded properly");
            Assert.AreEqual(payPerPlayEnabled, PayPerPlay.PayPerPlayEnabled, "Verify that PayPerPlayEnabled has been loaded properly");
            Assert.AreEqual(coinsRequired, PayPerPlay.CoinsRequired, "Verify that CoinsRequired has been loaded properly");
            Assert.AreEqual(currentCoins, PayPerPlay.CurrentCoins, "Verify that CurrentCoins has been loaded properly");
            Assert.AreEqual(userLicenseName, Program.UserLicenseName, "Verify that UserLicenseName has been loaded properly");
            Assert.AreEqual(userLiceneseKey, Program.UserLicenseKey, "Verify that UserLicenseKey has been loaded properly");

            //Attempt to fetch the user
            user = (User) Database.GetUser(userName);
            Assert.IsNotNull(user, "Verify that the user has been loaded");

            //Verify that the user info has been loaded properly
            Assert.AreEqual(userName, user.Username, "Verify that the UserName has been properly loaded");
            Assert.AreEqual(userLoginCount, user.GetUserLoginCount(), "Verify that the UserName has been properly loaded");
            Assert.AreEqual(userEmail, user.Email, "Verify that the UserName has been properly loaded");
            Assert.AreEqual(totalLaunchCount, user.GetUserLaunchCount(), "Verify that the UserName has been properly loaded");
            Assert.AreEqual(userInfo, user.UserInfo, "Verify that the UserName has been properly loaded");
            Assert.AreEqual(userAllowedEsrb, user.AllowedEsrbRatings, "Verify that the UserName has been properly loaded");
            Assert.AreEqual(profPicPath, user.ProfilePicture, "Verify that the UserName has been properly loaded");
            Assert.IsTrue(user.ValidatePassword(userPassword), "Verify that the userPassword has been properly loaded");

            //Attempt to fetch the user's favorite game list
            game = (Game) user.GetFavoritesList().First();
            Assert.IsNotNull(user, "Verify that the game has been loaded");

            //Verify that the game info has been properly loaded
            Assert.AreEqual(gameTitle, game.Title, "Verify that the game's title has been properly loaded");
            Assert.AreEqual(gameFileName, game.FileName, "Verify that the game's filename has been properly loaded");
            Assert.AreEqual(consoleName, game.ConsoleName, "Verify that the game's console name has been properly loaded");

        }

        /// <summary>
        /// Create new ROM fles in a temp rom directory and verify that they are properly imported
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyRomScanning()
        {
            //Create a new rom folder in the test directory
            string directoryPath = Directory.GetCurrentDirectory() + @"\TestRoms\";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //Create a new console
            Console console = new Console("SNES");
            console.RomFolderPath = directoryPath;

            //Populate the new rom directory 
            string romFilename1 = "Super Mario World.snes";
            string romFilename2 = "Super Mario Kart.snes";
            string romTitle1 = "Super Mario World";
            string romTitle2 = "Super Mario Kart";
            var rom1 = File.Create(directoryPath + romFilename1);
            var rom2 = File.Create(directoryPath + romFilename2);

            //Scan the rom directory
            FileOps.ScanSingleConsole(console);

            //Verify that the rom files have been properly imported
            Assert.IsNotNull(console.GetGame(romTitle1), "Verify that the first ROM is properly imported");
            Assert.IsNotNull(console.GetGame(romTitle2), "Verify that the second ROM is properly imported");

            //Cleanup 
            rom1.Close();
            rom2.Close();
            Directory.Delete(directoryPath, true);
        }

        #endregion
    }
}
