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
    }
}
