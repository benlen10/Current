using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using UniCade;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Objects;
using Console = UniCade.Objects.Console;

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
            Console = new Console("newConsole");
            Database.AddConsole(Console);
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

            IGame gameRatedNone = new Game("game5.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Null
            };


            //Set the global ESRB restriction to Everyone
            Program.RestrictGlobalESRB = Enums.ESRB.Everyone;

            //Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone
            Assert.IsFalse(FileOps.Launch(gameRatedE).Contains("ESRB"), "Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone");

            //Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone
            Assert.IsTrue(FileOps.Launch(gameRatedE10).Contains("ESRB"), "Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone");

            //Set the global ESRB restriction to Everyone 10+
            Program.RestrictGlobalESRB = Enums.ESRB.Everyone10;

            //Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+
            Assert.IsFalse(FileOps.Launch(gameRatedE10).Contains("ESRB"), "Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+");

            //Verify that a game rated Teen is restricted when global rating is set to Everyone 10+
            Assert.IsTrue(FileOps.Launch(gameRatedT).Contains("ESRB"), "Verify that a game rated Teen is restricted when global rating is set to Everyone 10+");

            //Set the global ESRB restriction to Teen
            Program.RestrictGlobalESRB = Enums.ESRB.Teen;

            //Verify that a game rated Teen can be launched properly when the global rating is set to Teen
            Assert.IsFalse(FileOps.Launch(gameRatedT).Contains("ESRB"), "Verify that a game rated Teen can be launched properly when the global rating is set to Teen");

            //Verify that a game rated Mature is restricted when global rating is set to Teen
            Assert.IsTrue(FileOps.Launch(gameRatedM).Contains("ESRB"), "Verify that a game rated Mature is restricted when global rating is set to Teen");

            //Set the global ESRB restriction to Mature
            Program.RestrictGlobalESRB = Enums.ESRB.Mature;

            //Verify that a game rated Mature can be launched properly when the global rating is set to Mature
            Assert.IsFalse(FileOps.Launch(gameRatedM).Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the global rating is set to Mature");

            //Verify that a game rated AO is restricted when global rating is set to Mature
            Assert.IsTrue(FileOps.Launch(gameRatedAO).Contains("ESRB"), "Verify that a game rated AO is restricted when global rating is set to Mature");

            //Set the global ESRB restriction to AO
            Program.RestrictGlobalESRB = Enums.ESRB.AO;

            //Verify that a game rated Mature can be launched properly when the global rating is set to AO
            Assert.IsFalse(FileOps.Launch(gameRatedM).Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the global rating is set to AO");

            //Verify that a game rated AO can be launched properly when the global rating is set to AO
            Assert.IsFalse(FileOps.Launch(gameRatedAO).Contains("ESRB"), "Verify that a game rated AO can be launched properly when the global rating is set to AO");
        }

        /// <summary>
        /// Verify that user specific ESRB Content restrictions properly restrict game launches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserEsrbRestrictions()
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

            IGame gameRatedNone = new Game("game5.bin", Console.ConsoleName)
            {
                EsrbRating = Enums.ESRB.Null
            };

            //Set the user ESRB restriction to Everyone
            Database.CurrentUser.AllowedEsrb = Enums.ESRB.Everyone;

            //Verify that a game rated Everyone can be launched properly when the user rating is set to Everyone
            Assert.IsFalse(FileOps.Launch(gameRatedE).Contains("ESRB"), "Verify that a game rated Everyone can be launched properly when the user rating is set to Everyone");

            //Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone
            Assert.IsTrue(FileOps.Launch(gameRatedE10).Contains("ESRB"), "Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone");

            //Set the user ESRB restriction to Everyone 10+
            Database.CurrentUser.AllowedEsrb = Enums.ESRB.Everyone10;

            //Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+
            Assert.IsFalse(FileOps.Launch(gameRatedE10).Contains("ESRB"), "Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+");

            //Verify that a game rated Teen is restricted when user rating is set to Everyone 10+
            Assert.IsTrue(FileOps.Launch(gameRatedT).Contains("ESRB"), "Verify that a game rated Teen is restricted when user rating is set to Everyone 10+");

            //Set the user ESRB restriction to Teen
            Database.CurrentUser.AllowedEsrb = Enums.ESRB.Teen;

            //Verify that a game rated Teen can be launched properly when the user rating is set to Teen
            Assert.IsFalse(FileOps.Launch(gameRatedT).Contains("ESRB"), "Verify that a game rated Teen can be launched properly when the user rating is set to Teen");

            //Verify that a game rated Mature is restricted when user rating is set to Teen
            Assert.IsTrue(FileOps.Launch(gameRatedM).Contains("ESRB"), "Verify that a game rated Mature is restricted when user rating is set to Teen");

            //Set the user ESRB restriction to Mature
            Database.CurrentUser.AllowedEsrb = Enums.ESRB.Mature;

            //Verify that a game rated Mature can be launched properly when the user rating is set to Mature
            Assert.IsFalse(FileOps.Launch(gameRatedM).Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the user rating is set to Mature");

            //Verify that a game rated AO is restricted when user rating is set to Mature
            Assert.IsTrue(FileOps.Launch(gameRatedAO).Contains("ESRB"), "Verify that a game rated AO is restricted when user rating is set to Mature");

            //Set the user ESRB restriction to AO
            Database.CurrentUser.AllowedEsrb = Enums.ESRB.AO;

            //Verify that a game rated Mature can be launched properly when the user rating is set to AO
            Assert.IsFalse(FileOps.Launch(gameRatedM).Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the user rating is set to AO");

            //Verify that a game rated AO can be launched properly when the user rating is set to AO
            Assert.IsFalse(FileOps.Launch(gameRatedAO).Contains("ESRB"), "Verify that a game rated AO can be launched properly when the user rating is set to AO");
        }

        /// <summary>
        /// Verify that games with a Null/empty ESRB rating are allowed to be launched regardless of rating restriction
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyNullLaunchRestriction()
        {

            //Set the global ESRB restriction to null
            Program.RestrictGlobalESRB = Enums.ESRB.Null;

            //Loop through each esrb rating and the game can be launched
            foreach (Enums.ESRB esrb in Enum.GetValues(typeof(Enums.ESRB)))
            {
                IGame game = new Game("game.bin", Console.ConsoleName)
                {
                    EsrbRating = esrb
                };

                //Verify that the game can be launched when the global rating is set to null
                Assert.IsFalse(FileOps.Launch(game).Contains("ESRB"), String.Format("Verify that a game rated {0} can be launched properly when the user rating is set to Null", esrb.GetStringValue()));
            }

            //Set the current user ESRB restriction to null
            Database.CurrentUser.AllowedEsrb = Enums.ESRB.Null;

            //Loop through each esrb rating and the game can be launched
            foreach (Enums.ESRB esrb in Enum.GetValues(typeof(Enums.ESRB)))
            {
                IGame game = new Game("game.bin", Console.ConsoleName)
                {
                    EsrbRating = esrb
                };

                //Verify that the game can be launched when the global rating is set to null
                Assert.IsFalse(FileOps.Launch(game).Contains("ESRB"), String.Format("Verify that a game rated {0} can be launched properly when the user rating is set to Null", esrb.GetStringValue()));
            }
        }
    }
}
