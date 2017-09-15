using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Interfaces;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

namespace UnitTests.BackendTests
{
    [TestClass]
    public class GameLibraryTests
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
        /// Verify that adding a game to a console properly incriments both the
        /// console game count and total game count
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void AddRemoveGameAndVerifyGameCount()
        {
            //Store the original game count
            int originalTotalGameCount = Database.GetTotalGameCount();

            //Verify that the TotalGameCount is intially set to zero after creating a new database instance
            Assert.AreEqual(0, originalTotalGameCount, "Verify that the TotalGameCount is intially set to zero after creating a new database instance");

            //Verify that the console game count is intially set to zero
            int originalConsoleGameCount = _console.GetGameCount();
            Assert.AreEqual(0, originalConsoleGameCount, "Verify that the console game count is intially set to zero");

            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", _console.ConsoleName);
            Assert.IsTrue(_console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Verify that the console game count has been incremented by one
            Assert.AreEqual((originalConsoleGameCount + 1), _console.GetGameCount(), "Verify that the console game count has been incremented by one");

            //Refresh the database that the total game count has been incremented by one
            Database.RefreshTotalGameCount();
            Assert.AreEqual((originalTotalGameCount + 1), Database.GetTotalGameCount(), "Verify that the console game count has been incremented by one");

            //Remove the game
            _console.RemoveGame(newGame.Title);

            //Verify that the console game count has been decremented by one after removing the game
            Assert.AreEqual(originalConsoleGameCount, _console.GetGameCount(), "Verify that the console game count has been incremented by one");

            //Refresh the database and verify that the console game count has been decremented by one after removing the game
            Database.RefreshTotalGameCount();
            Assert.AreEqual(originalTotalGameCount, Database.GetTotalGameCount(), "Verify that the console game count has been incremented by one");

            //Verify that attempting to remove a nonexistent game returns false 
            Assert.IsFalse(_console.RemoveGame(newGame.Title), "Verify that attempting to remove a nonexistent game returns false");
        }

        /// <summary>
        /// Verify that adding duplicate games with the same filename or game name are disallowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateConsolesDisallowed()
        {
            //Attempt to add a new console with the same name and verify this returns false
            IConsole console2 = new Console(_console.ConsoleName);
            Assert.IsFalse(Database.AddConsole(console2), "Verify that adding a console with a duplicate name is not allowed");
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
            //Attempt to add another game with the same filename and verify that duplicates are not allowed
            IGame game = new Game("newGame.bin", "differentConsole");
            Assert.IsFalse(_console.AddGame(game), "Verify that adding a game to an incorrect console is not allowed");
        }


        [TestMethod]
        [Priority(1)]
        public void VerifyAddingGameWithNullConsoleDissalowed()
        {
            //Attempt to add another game with the same filename and verify that duplicates are not allowed
            IGame game = null;
            try
            {
                game = new Game("newGame.bin", _console.ConsoleName);
            }
            catch (ArgumentException)
            {
                
            }

            Assert.IsTrue(game == null);
        }
    }
}
