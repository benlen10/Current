using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

namespace UnitTests.BackendTests
{
    [TestClass]
    public class GameTests
    {

        #region Properties

        /// <summary>
        /// The first console in the database
        /// </summary>
        private IConsole _console;

        /// <summary>
        /// The first game in the console
        /// </summary>
        private IGame _game;

        #endregion

        /// <summary>
        /// Create a new database instance, add a console and add a game to the new console
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();

            //Create a new console and add it to the database
            _console = new Console("newConsole");
            Database.AddConsole(_console);

            //Create a new game and add it to the console
            _game = new Game("myGame.bin", _console.ConsoleName);
            _console.AddGame(_game);
        }

        #region Property Validation Tests

        /// <summary>
        /// Verify that the game filename is properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameFileName()
        {
            IGame newGame = null;

            //Verify that a null value for the filename is not allowed
            try
            {
                newGame = new Game(null, _console.ConsoleName);
                Assert.Fail("Verify that a null value for a filename is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a filename is not allowed");
            }

            //Verify that invalid chars are not allowed in the filename
            const string invalidCharName = "a|c.bin";
            try
            {
                newGame = new Game(invalidCharName, _console.ConsoleName);
                Assert.Fail("Verify that invalid chars are not allowed in the console name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the filename");
            }

            //Verify that an invalid filename (without a '.') is not allowed
            const string invalidFileName = "filename";
            try
            {
                newGame = new Game(invalidFileName, _console.ConsoleName);
                Assert.Fail("Verify that an invalid filename (without a '.') is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that an invalid filename (without a '.') is not allowed");
            }

            //Verify that a filename that is less than MinfilenameLength chars is not allowed
            string shortfilename = new string('-', ConstValues.MinGameFileNameLength - 1);
            try
            {
                newGame = new Game(shortfilename, _console.ConsoleName);
                Assert.Fail($"Verify that a filename that is less than {ConstValues.MinGameFileNameLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a filename that is less than {ConstValues.MinGameFileNameLength} chars is not allowed");
            }

            //Verify that a filenamethat exceeds MaxConsoleNameLength chars is not allowed
            string longFileName = new string('-', ConstValues.MaxGameFileNameLength + 1);
            try
            {
                newGame = new Game(longFileName, _console.ConsoleName);
                Assert.Fail($"Verify that a filename that exceeds {ConstValues.MaxGameFileNameLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a consle name that exceeds {ConstValues.MaxGameFileNameLength} chars is not allowed");
            }

            //Verify that a game with an invalid filename is not created
            Assert.IsNull(newGame, "Verify that a game with an invalid filename is not created");

            //Verify that valid console names are properly saved
            const string validfilename = "myNewGame.iso";
            newGame = new Game(validfilename, _console.ConsoleName);
            Assert.AreEqual(newGame.FileName, validfilename, "Verify that a game with a valid filename is properly created");
        }

        /// <summary>
        /// Verify that the game description is properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameDescription()
        {
            //Verify that a null value for the description is not allowed
            try
            {
                _game.Description = null;
                Assert.Fail("Verify that a null value for a description is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a description is not allowed");
            }

            //Verify that invalid chars are not allowed in the description
            const string invalidDescription = "invalid|description";
            try
            {
                _game.Description = invalidDescription;
                Assert.Fail("Verify that invalid chars are not allowed in the console name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the description");
            }

            //Verify that a descriptionthat exceeds MaxConsoleNameLength chars is not allowed
            string longdescription = new string('-', ConstValues.MaxGameDescriptionLength + 1);
            try
            {
                _game.Description = longdescription;
                Assert.Fail($"Verify that a description that exceeds {ConstValues.MaxGameDescriptionLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a consle name that exceeds {ConstValues.MaxGameDescriptionLength} chars is not allowed");
            }

            //Verify that a game with an invalid description is not created
            Assert.IsNull(_game.Description, "Verify that an invalid description was not saved");

            //Verify that valid console names are properly saved
            const string validDescription = "valid description";
            _game.Description = validDescription;
            Assert.AreEqual(_game.Description, validDescription, "Verify that a valid description is properly saved");
        }

        /// <summary>
        /// Verify that the game release date is properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameReleaseDate()
        {
            //Verify that a null value for the release date is not allowed
            try
            {
                _game.ReleaseDate = null;
                Assert.Fail("Verify that a null value for a release date is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a release date is not allowed");
            }

            //Verify that a relase date must be only digits
            const string invalidReleaseDate = "200I";
            
            try
            {
                _game.ReleaseDate = invalidReleaseDate;
                Assert.Fail("Verify that a relase date must be only digits");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a relase date must be only digits");
            }

            //Verify that the release date is four digits
            try
            {
                _game.ReleaseDate = "20005";
                Assert.Fail("Verify that the release date must be four digits");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that the release date must be four digits");
            }

            //Verify that an invalid release date is not saved
            Assert.IsNull(_game.ReleaseDate, "Verify that an invalid release date was not saved");

            //Verify that a valid release date is properly saved
            const string validReleaseDate = "2001";
            _game.ReleaseDate = validReleaseDate;
            Assert.AreEqual(_game.ReleaseDate, validReleaseDate, "Verify that a valid release date is properly saved");
        }

        /// <summary>
        /// Verify that the game developer is properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameDeveloper()
        {
            //Verify that a null value for the developer is not allowed
            try
            {
                _game.DeveloperName = null;
                Assert.Fail("Verify that a null value for a developer is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a developer is not allowed");
            }

            //Verify that invalid chars are not allowed in the developer
            const string invalidDeveloper = "invalid|developer";
            try
            {
                _game.DeveloperName = invalidDeveloper;
                Assert.Fail("Verify that invalid chars are not allowed in the developer name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the developer");
            }

            //Verify that a developerthat exceeds MaxConsoleNameLength chars is not allowed
            string longDeveloper = new string('-', ConstValues.MaxPublisherDeveloperLength + 1);
            try
            {
                _game.DeveloperName = longDeveloper;
                Assert.Fail($"Verify that a developer that exceeds {ConstValues.MaxPublisherDeveloperLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a consle name that exceeds {ConstValues.MaxPublisherDeveloperLength} chars is not allowed");
            }

            //Verify that a game with an invalid developer is not created
            Assert.IsNull(_game.DeveloperName, "Verify that an invalid developer was not saved");

            //Verify that valid console names are properly saved
            const string validDeveloper = "valid developer";
            _game.DeveloperName = validDeveloper;
            Assert.AreEqual(_game.DeveloperName, validDeveloper, "Verify that a valid developer is properly saved");
        }

        /// <summary>
        /// Verify that the game publisher is properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGamePublisher()
        {
            //Verify that a null value for the publisher is not allowed
            try
            {
                _game.PublisherName = null;
                Assert.Fail("Verify that a null value for a publisher is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a publisher is not allowed");
            }

            //Verify that invalid chars are not allowed in the publisher
            const string invalidPublisher = "invalid|publisher";
            try
            {
                _game.PublisherName = invalidPublisher;
                Assert.Fail("Verify that invalid chars are not allowed in the publisher name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the publisher name");
            }

            //Verify that a publisher that exceeds MaxPublisherDeveloperLength chars is not allowed
            string longPublisher = new string('-', ConstValues.MaxPublisherDeveloperLength + 1);
            try
            {
                _game.PublisherName = longPublisher;
                Assert.Fail($"Verify that a publisher that exceeds {ConstValues.MaxPublisherDeveloperLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a publisher name that exceeds {ConstValues.MaxPublisherDeveloperLength} chars is not allowed");
            }

            //Verify that a game with an invalid publisher is not created
            Assert.IsNull(_game.PublisherName, "Verify that an invalid publisher was not saved");

            //Verify that valid [ublisher names are properly saved
            const string validPublisher = "valid publisher";
            _game.PublisherName = validPublisher;
            Assert.AreEqual(_game.PublisherName, validPublisher, "Verify that a valid publisher is properly saved");
        }

        /// <summary>
        /// Verify that the game genres are properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameGenres()
        {
            //Verify that a null value for the genre is not allowed
            try
            {
                _game.Genres = null;
                Assert.Fail("Verify that a null value for a genre is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a genre is not allowed");
            }

            //Verify that invalid chars are not allowed in the genre
            const string invalidgenre = "invalid|genre";
            try
            {
                _game.Genres = invalidgenre;
                Assert.Fail("Verify that invalid chars are not allowed in the genre name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the genre name");
            }

            //Verify that a genre that exceeds MaxgenregenreLength chars is not allowed
            string longGenre = new string('-', ConstValues.MaxGameGenreLength + 1);
            try
            {
                _game.Genres = longGenre;
                Assert.Fail($"Verify that a genre that exceeds {ConstValues.MaxGameGenreLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a genre name that exceeds {ConstValues.MaxGameGenreLength} chars is not allowed");
            }

            //Verify that a game with an invalid genre is not created
            Assert.IsNull(_game.Genres, "Verify that an invalid genre was not saved");

            //Verify that valid generes are properly saved
            const string validgenre = "valid genre";
            _game.Genres = validgenre;
            Assert.AreEqual(_game.Genres, validgenre, "Verify that a valid genre is properly saved");
        }

        /// <summary>
        /// Verify that the game UserReviewScore are properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameUserReviewScore()
        {
            //Verify that a null value for the UserReviewScore is not allowed
            try
            {
                _game.UserReviewScore = null;
                Assert.Fail("Verify that a null value for a UserReviewScore is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a UserReviewScore is not allowed");
            }

            //Verify that invalid chars are not allowed in the UserReviewScore
            const string invalidUserReviewScore = "invalid|UserReviewScore";
            try
            {
                _game.UserReviewScore = invalidUserReviewScore;
                Assert.Fail("Verify that invalid chars are not allowed in the UserReviewScore name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the UserReviewScore name");
            }

            //Verify that a UserReviewScore that exceeds MaxUserReviewScoreUserReviewScoreLength chars is not allowed
            string longUserReviewScore = new string('-', ConstValues.MaxGameReviewScoreLength + 1);
            try
            {
                _game.UserReviewScore = longUserReviewScore;
                Assert.Fail($"Verify that a UserReviewScore that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a UserReviewScore name that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }

            //Verify that a game with an invalid UserReviewScore is not created
            Assert.IsNull(_game.UserReviewScore, "Verify that an invalid UserReviewScore was not saved");

            //Verify that a valid UserReviewScore is properly saved
            const string validUserReviewScore = "80/100";
            _game.UserReviewScore = validUserReviewScore;
            Assert.AreEqual(_game.UserReviewScore, validUserReviewScore, "Verify that a valid UserReviewScore is properly saved");
        }

        /// <summary>
        /// Verify that the game CriticReviewScore are properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameCriticReviewScore()
        {
            //Verify that a null value for the CriticReviewScore is not allowed
            try
            {
                _game.CriticReviewScore = null;
                Assert.Fail("Verify that a null value for a CriticReviewScore is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a CriticReviewScore is not allowed");
            }

            //Verify that invalid chars are not allowed in the CriticReviewScore
            const string invalidCriticReviewScore = "invalid|CriticReviewScore";
            try
            {
                _game.CriticReviewScore = invalidCriticReviewScore;
                Assert.Fail("Verify that invalid chars are not allowed in the CriticReviewScore name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the CriticReviewScore name");
            }

            //Verify that a CriticReviewScore that exceeds MaxCriticReviewScoreCriticReviewScoreLength chars is not allowed
            string longCriticReviewScore = new string('-', ConstValues.MaxGameReviewScoreLength + 1);
            try
            {
                _game.CriticReviewScore = longCriticReviewScore;
                Assert.Fail($"Verify that a CriticReviewScore that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a CriticReviewScore name that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }

            //Verify that a game with an invalid CriticReviewScore is not created
            Assert.IsNull(_game.CriticReviewScore, "Verify that an invalid CriticReviewScore was not saved");

            //Verify that a valid CriticReviewScore is properly saved
            const string validCriticReviewScore = "80/100";
            _game.CriticReviewScore = validCriticReviewScore;
            Assert.AreEqual(_game.CriticReviewScore, validCriticReviewScore, "Verify that a valid CriticReviewScore is properly saved");
        }

        /// <summary>
        /// Verify that the game Trivia is properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameTrivia()
        {
            //Verify that a null value for the Trivia is not allowed
            try
            {
                _game.Trivia = null;
                Assert.Fail("Verify that a null value for a Trivia is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a Trivia is not allowed");
            }

            //Verify that invalid chars are not allowed in the Trivia
            const string invalidTrivia = "invalid|Trivia";
            try
            {
                _game.Trivia = invalidTrivia;
                Assert.Fail("Verify that invalid chars are not allowed in the Trivia name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the Trivia name");
            }

            //Verify that a Trivia that exceeds MaxTriviaTriviaLength chars is not allowed
            string longTrivia = new string('-', ConstValues.MaxGameReviewScoreLength + 1);
            try
            {
                _game.Trivia = longTrivia;
                Assert.Fail($"Verify that a Trivia that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a Trivia name that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }

            //Verify that a game with an invalid Trivia is not created
            Assert.IsNull(_game.Trivia, "Verify that an invalid Trivia was not saved");

            //Verify that a valid Trivia is properly saved
            const string validTrivia = "validTrivia";
            _game.Trivia = validTrivia;
            Assert.AreEqual(_game.Trivia, validTrivia, "Verify that a valid Trivia is properly saved");
        }

        /// <summary>
        /// Verify that the game SupportedPlayerCount are properly validated
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGameSupportedPlayerCount()
        {
            //Verify that a null value for the SupportedPlayerCount is not allowed
            try
            {
                _game.SupportedPlayerCount = null;
                Assert.Fail("Verify that a null value for a SupportedPlayerCount is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a SupportedPlayerCount is not allowed");
            }

            //Verify that invalid chars are not allowed in the SupportedPlayerCount
            const string invalidSupportedPlayerCount = "invalid|SupportedPlayerCount";
            try
            {
                _game.SupportedPlayerCount = invalidSupportedPlayerCount;
                Assert.Fail("Verify that invalid chars are not allowed in the SupportedPlayerCount name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the SupportedPlayerCount name");
            }

            //Verify that a SupportedPlayerCount that exceeds MaxSupportedPlayerCountSupportedPlayerCountLength chars is not allowed
            string longSupportedPlayerCount = new string('-', ConstValues.MaxGameReviewScoreLength + 1);
            try
            {
                _game.SupportedPlayerCount = longSupportedPlayerCount;
                Assert.Fail($"Verify that a SupportedPlayerCount that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a SupportedPlayerCount name that exceeds {ConstValues.MaxGameReviewScoreLength} chars is not allowed");
            }

            //Verify that a game with an invalid SupportedPlayerCount is not created
            Assert.IsNull(_game.SupportedPlayerCount, "Verify that an invalid SupportedPlayerCount was not saved");

            //Verify that a valid SupportedPlayerCount is properly saved
            const string validSupportedPlayerCount = "validSupportedPlayerCount";
            _game.SupportedPlayerCount = validSupportedPlayerCount;
            Assert.AreEqual(_game.SupportedPlayerCount, validSupportedPlayerCount, "Verify that a valid SupportedPlayerCount is properly saved");
        }



        #endregion

        #region Public Methods tests




        #endregion

    }
}
