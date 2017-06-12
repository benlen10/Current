using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade;

namespace UnitTests
{
    [TestClass]
    public class GameLibraryTests
    {
        #region Properties

        /// <summary>
        /// The current database instance
        /// </summary>
        IDatabase Database;

        /// <summary>
        /// A new Random instance to generate a random id tag
        /// </summary>
        Random random;

        /// <summary>
        /// The randomly generated int value for the current test instance 
        /// </summary>
        int id;

        /// <summary>
        /// The first console in the database
        /// </summary>
        IConsole console;

        #endregion

        /// <summary>
        /// Create a new database instance, add a console and generate a new random int id
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initialize the static database
            Database = new Database();

            //Generate a new random id integer
            random = new Random();
            id = random.Next();

            //Create a new console and add it to the database
            console = new UniCade.Console("newConsole");
            Database.ConsoleList.Add(console);
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
            int originalTotalGameCount = Database.TotalGameCount;

            //Verify that the TotalGameCount is intially set to zero after creating a new database instance
            Assert.AreEqual(0, originalTotalGameCount, "Verify that the TotalGameCount is intially set to zero after creating a new database instance");

            //Verify that the console game count is intially set to zero
            int originalConsoleGameCount = console.GameCount;
            Assert.AreEqual(0, originalConsoleGameCount, "Verify that the console game count is intially set to zero");

            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", console.ConsoleName);
            Assert.IsTrue(console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Verify that the console game count has been incremented by one
            Assert.AreEqual((originalConsoleGameCount + 1), console.GameCount, "Verify that the console game count has been incremented by one");

            //Refresh the database that the total game count has been incremented by one
            Database.RefreshTotalGameCount();
            Assert.AreEqual((originalTotalGameCount + 1), Database.TotalGameCount, "Verify that the console game count has been incremented by one");

            //Remove the game
            console.RemoveGame(newGame);

            //Verify that the console game count has been decremented by one after removing the game
            Assert.AreEqual(originalConsoleGameCount, console.GameCount, "Verify that the console game count has been incremented by one");

            //Refresh the database and verify that the console game count has been decremented by one after removing the game
            Database.RefreshTotalGameCount();
            Assert.AreEqual(originalTotalGameCount, Database.TotalGameCount, "Verify that the console game count has been incremented by one");

            //Verify that attempting to remove a nonexistent game returns false 
            Assert.IsFalse(console.RemoveGame(newGame), "Verify that attempting to remove a nonexistent game returns false");
        }

        /// <summary>
        /// Verify that adding duplicate games with the same filename or game name are disallowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateConsolesDisallowed()
        {
            //Attempt to add a new console with the same name and verify this returns false
            IConsole console2 = new UniCade.Console(console.ConsoleName);
        }

        /// <summary>
        /// Verify that adding duplicate games with the same filename or game name are disallowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateGamesDisallowed()
        {
            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", console.ConsoleName);
            Assert.IsTrue(console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Attempt to add another game with the same filename and verify that duplicates are not allowed
            IGame game1 = new Game("newGame.bin", console.ConsoleName);
            Assert.IsFalse(console.AddGame(game1), "Verify that dupliate games with the same filename are not allowed");

            //Attempt to add another game with the same game title and verify that duplicates are not allowed
            IGame game2 = new Game("newGame.iso", console.ConsoleName);
            Assert.IsFalse(console.AddGame(game1), "Verify that dupliate games with the same title are not allowed");
        }


    }
}
