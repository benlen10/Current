using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade;

namespace UnitTests
{
    [TestClass]
    public class GameLibraryTests
    {
        /// <summary>
        /// Verify that adding a game to a console properly incriments both the
        /// console game count and total game count
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void AddRemoveGameAndVerifyGameCount()
        {
            //Initialize the static database
            IDatabase ActiveDatabase = new Database();

            //Store the original game count
            int originalTotalGameCount = ActiveDatabase.TotalGameCount;

            //Verify that the TotalGameCount is intially set to zero after creating a new database instance
            Assert.AreEqual(0, originalTotalGameCount, "Verify that the TotalGameCount is intially set to zero after creating a new database instance");

            //Create a new console and add it to the database
            string consoleName = "newConsole";
            IConsole newConsole = new UniCade.Console(consoleName);
            ActiveDatabase.ConsoleList.Add(newConsole);

            //Verify that the console game count is intially set to zero
            int originalConsoleGameCount = newConsole.GameCount;
            Assert.AreEqual(0, originalConsoleGameCount, "Verify that the console game count is intially set to zero");

            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", consoleName);
            Assert.IsTrue(newConsole.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Verify that the console game count has been incremented by one
            Assert.AreEqual((originalConsoleGameCount + 1), newConsole.GameCount, "Verify that the console game count has been incremented by one");

            //Verify that the console game count has been incremented by one
            Assert.AreEqual((originalTotalGameCount + 1), ActiveDatabase.TotalGameCount, "Verify that the console game count has been incremented by one");

            //Remove the game
            newConsole.RemoveGame(newGame);

            //Verify that the console game count has been decremented by one after removing the game
            Assert.AreEqual(originalConsoleGameCount, newConsole.GameCount, "Verify that the console game count has been incremented by one");

            //Verify that the console game count has been decremented by one after removing the game
            Assert.AreEqual(originalTotalGameCount, ActiveDatabase.TotalGameCount, "Verify that the console game count has been incremented by one");

            //Verify that attempting to remove a nonexistent game returns false 
            Assert.IsFalse(newConsole.RemoveGame(newGame), "Verify that attempting to remove a nonexistent game returns false");
        }
    }
}
