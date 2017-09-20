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
        }

        #region Property Validation Tests

        /// <summary>
        /// 
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

        #endregion

        #region Public Methods tests




        #endregion

    }
}
