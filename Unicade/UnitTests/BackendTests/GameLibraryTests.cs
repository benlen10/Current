using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade;
using UniCade.Backend;
using UniCade.Interfaces;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

namespace UnitTests
{
    [TestClass]
    public class GameLibraryTests
    {
        #region Properties

        /// <summary>
        /// A new Random instance to generate a random id tag
        /// </summary>
        private Random Random;

        /// <summary>
        /// The randomly generated int value for the current test instance 
        /// </summary>
        private int Id;

        /// <summary>
        /// The first console in the database
        /// </summary>
        private IConsole Console;

        #endregion

        /// <summary>
        /// Create a new database instance, add a console and generate a new random int id
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();

            //Generate a new random id integer
            Random = new Random();
            Id = Random.Next();

            //Create a new console and add it to the database
            Console = new Console("newConsole");
            Database.AddConsole(Console);
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
            int originalConsoleGameCount = Console.GetGameCount();
            Assert.AreEqual(0, originalConsoleGameCount, "Verify that the console game count is intially set to zero");

            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", Console.ConsoleName);
            Assert.IsTrue(Console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Verify that the console game count has been incremented by one
            Assert.AreEqual((originalConsoleGameCount + 1), Console.GetGameCount(), "Verify that the console game count has been incremented by one");

            //Refresh the database that the total game count has been incremented by one
            Database.RefreshTotalGameCount();
            Assert.AreEqual((originalTotalGameCount + 1), Database.GetTotalGameCount(), "Verify that the console game count has been incremented by one");

            //Remove the game
            Console.RemoveGame(newGame.Title);

            //Verify that the console game count has been decremented by one after removing the game
            Assert.AreEqual(originalConsoleGameCount, Console.GetGameCount(), "Verify that the console game count has been incremented by one");

            //Refresh the database and verify that the console game count has been decremented by one after removing the game
            Database.RefreshTotalGameCount();
            Assert.AreEqual(originalTotalGameCount, Database.GetTotalGameCount(), "Verify that the console game count has been incremented by one");

            //Verify that attempting to remove a nonexistent game returns false 
            Assert.IsFalse(Console.RemoveGame(newGame.Title), "Verify that attempting to remove a nonexistent game returns false");
        }

        /// <summary>
        /// Verify that adding duplicate games with the same filename or game name are disallowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateConsolesDisallowed()
        {
            //Attempt to add a new console with the same name and verify this returns false
            IConsole console2 = new Console(Console.ConsoleName);
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
            IGame newGame = new Game("newGame.bin", Console.ConsoleName);
            Assert.IsTrue(Console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Attempt to add another game with the same filename and verify that duplicates are not allowed
            IGame game1 = new Game("newGame.bin", Console.ConsoleName);
            Assert.IsFalse(Console.AddGame(game1), "Verify that dupliate games with the same filename are not allowed");

            //Attempt to add another game with the same game title and verify that duplicates are not allowed
            IGame game2 = new Game("newGame.iso", Console.ConsoleName);
            Assert.IsFalse(Console.AddGame(game1), "Verify that dupliate games with the same title are not allowed");
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
            Assert.IsFalse(Console.AddGame(game), "Verify that adding a game to an incorrect console is not allowed");
        }


        [TestMethod]
        [Priority(1)]
        public void VerifyAddingGameWithNullConsoleDissalowed()
        {
            //Attempt to add another game with the same filename and verify that duplicates are not allowed
            IGame game = null;
            try
            {
                game = new Game("newGame.bin", Console.ConsoleName);
            }
            catch (ArgumentException)
            {
                
            }

            if (game == null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
    }
}
