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
    public class ConsoleTests
    {
        #region Properties

        /// <summary>
        /// The first console in the database
        /// </summary>
        private IConsole _console;

        #endregion

        #region Console Property Tests 

        /// <summary>
        /// Verify that ROM directory paths are properly validated 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateConsoleName()
        {
            //Verify that a null value for the ROM folder path is not allowed
            try
            {
                _console.ConsoleName = null;
                Assert.Fail("Verify that a null value for ConsoleName is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for ConsoleName is not allowed");
            }

            //Verify that an empty value for the ROM folder path is not allowed
            try
            {
                _console.ConsoleName = "";
                Assert.Fail("Verify that an empty value for ConsoleName is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that an empty value for ConsoleName is not allowed");
            }

            //Verify that invalid chars are not allowed in the ConsoleName
            const string invalidConsoleName = "a|c";
            try
            {
                _console.ConsoleName = invalidConsoleName;
                Assert.Fail("Verify that invalid chars are not allowed in the console name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the console name");
            }

            //Verify that a console name that exceeds MaxConsoleNameLength chars is not allowed
            string longName = new string('-', ConstValues.MaxConsoleNameLength + 1);
            try
            {
                _console.ConsoleName = longName;
                Assert.Fail($"Verify that a consle name that exceeds {ConstValues.MaxConsoleNameLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a consle name that exceeds {ConstValues.MaxConsoleNameLength} chars is not allowed");
            }

            //Verify that valid ROM paths are properly saved
            string validConsoleName = "validConsole";
            _console.ConsoleName = validConsoleName;
            Assert.AreEqual(_console.ConsoleName, validConsoleName, "Verify that valid ROM paths are properly saved");
        }

        /// <summary>
        /// Verify that ROM directory paths are properly validated 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateReleaseDate()
        {
            //Verify that a null value for the release date is not allowed
            try
            {
                _console.ConsoleName = null;
                Assert.Fail("Verify that a null value for the release date is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for the release date is not allowed");
            }

            //Verify that a release date under 4 chars is not allowed
            string shortReleaseDate = new string('-', 3);
            try
            {
                _console.ReleaseDate = shortReleaseDate;
                Assert.Fail("Verify that a release date under 4 chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a release date under 4 chars is not allowed");
            }

            //Verify that the release date must be all numbers 
            string invalidReleaseDate = "200I";
            try
            {
                _console.ReleaseDate = invalidReleaseDate;
                Assert.Fail("Verify that the release date must be all numbers");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "VVerify that the release date must be all numbers");
            }

            //Verify that valid release date is properly saved
            string validReleaseDate = "2007";
            _console.ReleaseDate = validReleaseDate;
            Assert.AreEqual(_console.ReleaseDate, validReleaseDate, "Verify that valid ROM paths are properly saved");
        }

        /// <summary>
        /// Verify that ROM directory paths are properly validated 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRomFolderPath()
        {
            //Verify that a null value for the ROM folder path is not allowed
            try
            {
                _console.RomFolderPath = null;
                Assert.Fail("Verify that a null value for the ROM folder path is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for the ROM folder path is not allowed");
            }

            //Verify that a ROM path under 4 chars is not allowed
            string shortPath = new string('-', ConstValues.MinPathLength - 1);
            try
            {
                _console.RomFolderPath = shortPath;
                Assert.Fail($"Verify that a ROM path under {ConstValues.MinPathLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, $"Verify that a ROM path under {ConstValues.MinPathLength} chars is not allowed");
            }

            //Verify that invalid chars are not allowed in the ROM path
            const string invalidCharPath = "a|c";
            try
            {
                _console.RomFolderPath = invalidCharPath;
                Assert.Fail("Verify that invalid chars are not allowed in the ROM path");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the ROM path");
            }

            //Verify that invalid path strings are now allowed
            const string invalidRomPath = "invalidRomPath";
            try
            {
                _console.RomFolderPath = invalidRomPath;
                Assert.Fail("Verify that invalid path strings are now allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid path strings are now allowed");
            }

            //Verify that a ROM path that exceeds 4000 chars is not allowed
            string longPath = new string('-', ConstValues.MaxPathLength + 1);
            try
            {
                _console.RomFolderPath = longPath;
                Assert.Fail($"Verify that a ROM path that exceeds {ConstValues.MaxPathLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a ROM path that exceeds {ConstValues.MaxPathLength} chars is not allowed");
            }

            //Verify a valid ROM path does not throw an exception
            const string validRomPath = "C:\\ROMS";
            try
            {
                _console.RomFolderPath = validRomPath;
                Assert.IsTrue(true, "Verify a valid ROM path does not throw an exception");
            }
            catch (ArgumentException)
            {
                Assert.Fail("Verify a valid ROM path does not throw an exception");
            }

            //Verify that valid ROM paths are properly saved
            Assert.AreEqual(_console.RomFolderPath, validRomPath, "Verify that valid ROM paths are properly saved");
        }

        #endregion

        /// <summary>
        /// Create a new database instance, add a console and generate a new random int id
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();

            //Create a new console and add it to the database
            _console = new Console("newConsole");
            Database.AddConsole(_console);
        }


        #region Games Tests

        /// <summary>
        /// Verify that adding duplicate games with the same filename or game name are disallowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateGamesDisallowed()
        {
            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", _console.ConsoleName);
            Assert.IsTrue(_console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Attempt to add another game with the same filename and verify that duplicates are not allowed
            IGame game1 = new Game("newGame.bin", _console.ConsoleName);
            Assert.IsFalse(_console.AddGame(game1), "Verify that dupliate games with the same filename are not allowed");

            //Attempt to add another game with the same game title and verify that duplicates are not allowed
            IGame game2 = new Game("newGame.iso", _console.ConsoleName);
            Assert.IsFalse(_console.AddGame(game2), "Verify that dupliate games with the same title are not allowed");
        }

        /// <summary>
        /// Verify that adding a game to an incorrect console is not allowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void AddGameToIncorrectConsoleDisallowed()
        {
            //Attempt to add a new game to a different console
            IGame game = new Game("newGame.bin", "differentConsole");
            Assert.IsFalse(_console.AddGame(game), "Verify that adding a game to an incorrect console is not allowed");

            //Verify that a new game can be added to the proper console
            IGame game2 = new Game("newGame.bin", _console.ConsoleName);
            Assert.IsTrue(_console.AddGame(game2), "Verify that a new game can be added to the proper console");
        }

        /// <summary>
        /// Verify that you are not able to add more than the max allowed number of consoles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyMaxGameCountRestrictions()
        {
            for (int iterator = 1; iterator <= ConstValues.MaxGameCount; iterator++)
            {
                IGame game = new Game($"newGame {iterator}.bin", _console.ConsoleName);
                Assert.IsTrue(_console.AddGame(game), $"Verify that game number {iterator} can be added properly");
            }

            IGame lastGame = new Game("lastGame.bin", _console.ConsoleName);
            Assert.IsFalse(_console.AddGame(lastGame), $"Verify that game number {ConstValues.MaxGameCount + 1} cannot be added since it exceeeds {ConstValues.MaxGameCount}");
        }

        #endregion
    }
}
