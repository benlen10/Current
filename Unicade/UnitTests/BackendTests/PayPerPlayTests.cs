using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Exceptions;
using UniCade.Interfaces;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

namespace UnitTests.BackendTests
{
    [TestClass]
    public class PayPerPlayTests
    {

        #region Properties

        /// <summary>
        /// The first console in the database
        /// </summary>
        private IConsole _console;

        /// <summary>
        /// The first game in the new console
        /// </summary>
        private IGame _game;

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

            //Create a new game and add it to the console
            _game = new Game("game.bin", _console.ConsoleName);
            _console.AddGame(_game);

            //Add the console to the database
            Database.AddConsole(_console);
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
            try
            {
                FileOps.Launch(_game);
                Assert.Fail("Verify that the game cannot be launched if not enough coins are inserted");
            }
            catch (LaunchException e)
            {
                Assert.IsTrue(e.Message.Contains("Pay"), "Verify that the game cannot be launched if not enough coins are inserted");
            }

            //Insert 4 coins and attempt to launch the game again
            PayPerPlay.CurrentCoins = 4;
            try
            {
                FileOps.Launch(_game);
                Assert.Fail("Verify that the game can be launched if enough coins are inserted");
            }
            catch (LaunchException e)
            {
                Assert.IsFalse(e.Message.Contains("Pay"), "Verify that the game can be launched if enough coins are inserted");
            }

            //Set the required coin count and current coins to zero
            PayPerPlay.CoinsRequired = 0;
            PayPerPlay.CurrentCoins = 0;

            //Verify that a game can be launched if the required coins = 0 even if no coins are currently inserted 
            try
            {
                FileOps.Launch(_game);
                Assert.Fail("Verify that a game can be launched if the required coins = 0 even if no coins are currently inserted");
            }
            catch (LaunchException e)
            {
                Assert.IsFalse(e.Message.Contains("Pay"), "Verify that a game can be launched if the required coins = 0 even if no coins are currently inserted");
            }
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
            FileOps.Launch(_game);
            Assert.IsTrue(originalCoinCount == PayPerPlay.CurrentCoins, "Verify that the current count is not decemented on an unsucuessful launch (ROM file not found)");

            //Verify that the DecrementCoins method properly decrements the coin count
            PayPerPlay.DecrementCoins();
            Assert.IsTrue(PayPerPlay.CurrentCoins == (originalCoinCount - PayPerPlay.CoinsRequired), "Verify that the DecrementCoins method properly decrements the coin count");
        }
    }
}
