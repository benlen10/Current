using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCade;
using UniCade.Backend;

namespace UnitTests.Backend_Tests
{
    [TestClass]
    public class PayPerPlayTests
    {

        #region Properties

        /// <summary>
        /// A new Random instance to generate a random id tag
        /// </summary>
        Random Random;

        /// <summary>
        /// The randomly generated int value for the current test instance 
        /// </summary>
        int Id;

        /// <summary>
        /// The first console in the database
        /// </summary>
        IConsole Console;

        /// <summary>
        /// The first game in the new console
        /// </summary>
        IGame Game;

        #endregion

        /// <summary>
        /// Create a new database instance, add a console and generate a new random int id
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Program.Initalize();

            //Generate a new random id integer
            Random = new Random();
            Id = Random.Next();

            //Create a new console and add it to the database
            Console = new UniCade.Console("newConsole");

            //Create a new game and add it to the console
            Game = new Game("game.bin", Console.ConsoleName);
            Console.AddGame(Game);

            //Add the console to the database
            Program.AddConsole(Console);
        }

        /// <summary>
        /// Verify that PayPerPlay properly restricts game launches when enabled
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyPayPerPlayLaunchRestrictions()
        {
            //Enable PayPerPlay and set coin counts
            PayPerPlay.PayPerPlayEnabled = true;
            PayPerPlay.CoinsRequired = 2;
            PayPerPlay.CurrentCoins = 0;

            //Attempt to launch a game without inserting any coins and verify that the launch is restricted
            string result = FileOps.Launch(Game);
            Assert.IsTrue(result.Contains("Pay"), "Verify that the game cannot be launched if not enough coins are inserted");

            //Insert 4 coins and attempt to launch the game again
            PayPerPlay.CurrentCoins = 4;
            Assert.IsFalse(FileOps.Launch(Game).Contains("Pay"), "Verify that the game can be launched if enough coins are inserted");

            //Set the required coin count and current coins to zero
            PayPerPlay.CoinsRequired = 0;
            PayPerPlay.CurrentCoins = 0;

            //Verify that a game can be launched if the required coins = 0 even if no coins are currently inserted 
            Assert.IsFalse(FileOps.Launch(Game).Contains("Pay"), "Verify that a game can be launched if the required coins = 0 even if no coins are currently inserted ");
        }

        /// <summary>
        /// Verify that PayPerPlay properly restricts game launches when enabled
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyPayPerPlayCoinCounts()
        {
            //Enable PayPerPlay and set counts 
            PayPerPlay.PayPerPlayEnabled = true;
            PayPerPlay.CoinsRequired = 2;
            PayPerPlay.CurrentCoins = 8;
            int originalCoinCount = PayPerPlay.CurrentCoins;

            //Verify that the current count is not decemented on an unsucuessful launch (ROM file not found)
            FileOps.Launch(Game);
            Assert.IsTrue(originalCoinCount == PayPerPlay.CurrentCoins, "Verify that the current count is not decemented on an unsucuessful launch (ROM file not found)");

            //Verify that the DecrementCoins method properly decrements the coin count
            PayPerPlay.DecrementCoins();
            Assert.IsTrue(PayPerPlay.CurrentCoins == (originalCoinCount - PayPerPlay.CoinsRequired), "Verify that the DecrementCoins method properly decrements the coin count");
        }
    }
}
