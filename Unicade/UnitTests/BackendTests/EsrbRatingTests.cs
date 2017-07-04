using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UniCade;
using UniCade.Constants;

namespace UnitTests
{

    [TestClass]
    public class EsrbRatingTests
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
            Program.ConsoleList.Add(Console);
        }


        /// <summary>
        /// Verify that Global ESRB Content restrictions properly restrict game launches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyGlobalEsrbRestrictions()
        {
            //Create a new game for each rating
            IGame gameRatedE = new Game("game1.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Everyone
            };

            IGame gameRatedE10 = new Game("game2.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Everyone10
            };

            IGame gameRatedT = new Game("game3.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Teen
            };

            IGame gameRatedM = new Game("game4.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Mature
            };

            IGame gameRatedAO = new Game("game5.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.AO
            };

            IGame gameRatedRP = new Game("game5.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Null
            };


            //Set the global ESRB restriction to everyone
            Program.RestrictGlobalESRB = Enums.ESRB.Everyone;

            //Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone
            Assert.IsFalse(FileOps.Launch(gameRatedE).Contains("ESRB"), "Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone");

            //Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone
            Assert.IsTrue(FileOps.Launch(gameRatedE10).Contains("ESRB"), "Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone");

        }

        /// <summary>
        /// Verify that adding a game to an incorrect console is not allowed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserEsrbRestrictions()
        {
        }



    }
}
