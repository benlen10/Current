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
    public class EsrbRatingTests
    {

        #region Properties

        /// <summary>
        /// The first console in the database
        /// </summary>
        IConsole _console;

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
        /// Verify that Global ESRB Content restrictions properly restrict game launches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyGlobalEsrbRestrictions()
        {
            //Create a new game for each rating
            IGame gameRatedE = new Game("game1.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Everyone
            };

            IGame gameRatedE10 = new Game("game2.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Everyone10
            };

            IGame gameRatedT = new Game("game3.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Teen
            };

            IGame gameRatedM = new Game("game4.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Mature
            };

            IGame gameRatedAo = new Game("game5.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Ao
            };

            //Set the global ESRB restriction to Everyone
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Everyone;

            //Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone
            try
            {
                FileOps.Launch(gameRatedE);
                Assert.Fail(
                    "Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone");
            }

            //Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail("Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone");
            }

            //Set the global ESRB restriction to Everyone 10+
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Everyone10;

            //Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail("Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+");
            }

            //Verify that a game rated Teen is restricted when global rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail("Verify that a game rated Teen is restricted when global rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Teen is restricted when global rating is set to Everyone 10+");
            }

            //Set the global ESRB restriction to Teen
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Teen;

            //Verify that a game rated Teen can be launched properly when the global rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail("Verify that a game rated Teen can be launched properly when the global rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Teen can be launched properly when the global rating is set to Teen");
            }

            //Verify that a game rated Mature is restricted when global rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature is restricted when global rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Mature is restricted when global rating is set to Teen");
            }

            //Set the global ESRB restriction to Mature
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Mature;

            //Verify that a game rated Mature can be launched properly when the global rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature can be launched properly when the global rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the global rating is set to Mature");
            }

            //Verify that a game rated Ao is restricted when global rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao is restricted when global rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Ao is restricted when global rating is set to Mature");
            }

            //Set the global ESRB restriction to Ao
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Ao;

            //Verify that a game rated Mature can be launched properly when the global rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature can be launched properly when the global rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the global rating is set to Ao");
            }

            //Verify that a game rated Ao can be launched properly when the global rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao can be launched properly when the global rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Ao can be launched properly when the global rating is set to Ao");
            }
        }

        /// <summary>
        /// Verify that user specific ESRB Content restrictions properly restrict game launches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserEsrbRestrictions()
        {
            //Create a new game for each rating
            IGame gameRatedE = new Game("game1.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Everyone
            };

            IGame gameRatedE10 = new Game("game2.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Everyone10
            };

            IGame gameRatedT = new Game("game3.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Teen
            };

            IGame gameRatedM = new Game("game4.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Mature
            };

            IGame gameRatedAo = new Game("game5.bin", _console.ConsoleName)
            {
                EsrbRatingsRating = Enums.EsrbRatings.Ao
            };

            //Set the user ESRB restriction to Everyone
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Everyone;

            //Verify that a game rated Everyone can be launched properly when the user rating is set to Everyone
            try
            {
                FileOps.Launch(gameRatedE);
                Assert.Fail(
                    "Verify that a game rated Everyone can be launched properly when the user rating is set to Everyone");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Everyone can be launched properly when the user rating is set to Everyone");
            }

            //Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail("Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone");
            }

            //Set the user ESRB restriction to Everyone 10+
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Everyone10;

            //Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail("Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+");
            }

            //Verify that a game rated Teen is restricted when user rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail("Verify that a game rated Teen is restricted when user rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Teen is restricted when user rating is set to Everyone 10+");
            }

            //Set the user ESRB restriction to Teen
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Teen;

            //Verify that a game rated Teen can be launched properly when the user rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail("Verify that a game rated Teen can be launched properly when the user rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Teen can be launched properly when the user rating is set to Teen");
            }

            //Verify that a game rated Mature is restricted when user rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature is restricted when user rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Mature is restricted when user rating is set to Teen");
            }

            //Set the user ESRB restriction to Mature
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Mature;

            //Verify that a game rated Mature can be launched properly when the user rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature can be launched properly when the user rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the user rating is set to Mature");
            }

            //Verify that a game rated Ao is restricted when user rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao is restricted when user rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"), "Verify that a game rated Ao is restricted when user rating is set to Mature");
            }

            //Set the user ESRB restriction to Ao
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Ao;

            //Verify that a game rated Mature can be launched properly when the user rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature can be launched properly when the user rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Mature can be launched properly when the user rating is set to Ao");
            }

            //Verify that a game rated Ao can be launched properly when the user rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao can be launched properly when the user rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"), "Verify that a game rated Ao can be launched properly when the user rating is set to Ao");
            }
        }

        /// <summary>
        /// Verify that games with a Null/empty ESRB rating are allowed to be launched regardless of rating restriction

        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyNullLaunchRestriction()
        {
            //Set the global ESRB restriction to null
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Null;

            //Loop through each esrb rating and the game can be launched
            foreach (Enums.EsrbRatings esrb in Enum.GetValues(typeof(Enums.EsrbRatings)))
            {
                IGame game = new Game("game.bin", _console.ConsoleName)
                {
                    EsrbRatingsRating = esrb
                };

                //Verify that the game can be launched when the global rating is set to null
                try
                {
                    FileOps.Launch(game);
                    Assert.Fail($"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the global rating is set to Null");
                }
                catch(Exception exception)
                {
                    Assert.IsFalse(exception.Message.Contains("ESRB"), $"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the global rating is set to Null");
                }
            }

            //Set the current user ESRB restriction to null
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Null;

            //Loop through each esrb rating and the game can be launched
            foreach (Enums.EsrbRatings esrb in Enum.GetValues(typeof(Enums.EsrbRatings)))
            {
                IGame game = new Game("game.bin", _console.ConsoleName)
                {
                    EsrbRatingsRating = esrb
                };

                //Verify that the game can be launched when the user rating is set to null
                try
                {
                    FileOps.Launch(game);
                    Assert.Fail($"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the user rating is set to Null");
                }
                catch (Exception exception)
                {
                    Assert.IsFalse(exception.Message.Contains("ESRB"), $"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the user rating is set to Null");
                }
            }
        }
    }
}
