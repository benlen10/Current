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
                EsrbRating = Enums.EsrbRatings.Everyone
            };

            IGame gameRatedE10 = new Game("game2.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Everyone10
            };

            IGame gameRatedT = new Game("game3.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Teen
            };

            IGame gameRatedM = new Game("game4.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Mature
            };

            IGame gameRatedAo = new Game("game5.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Ao
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
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Everyone can be launched properly when the global rating is set to Everyone");
            }

            //Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail(
                    "Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Everyone 10+ is restricted when global rating is set to Everyone");
            }

            //Set the global ESRB restriction to Everyone 10+
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Everyone10;

            //Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail(
                    "Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Everyone 10+ can be launched properly when the global rating is set to Everyone 10+");
            }

            //Verify that a game rated Teen is restricted when global rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail("Verify that a game rated Teen is restricted when global rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Teen is restricted when global rating is set to Everyone 10+");
            }

            //Set the global ESRB restriction to Teen
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Teen;

            //Verify that a game rated Teen can be launched properly when the global rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail(
                    "Verify that a game rated Teen can be launched properly when the global rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Teen can be launched properly when the global rating is set to Teen");
            }

            //Verify that a game rated Mature is restricted when global rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature is restricted when global rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Mature is restricted when global rating is set to Teen");
            }

            //Set the global ESRB restriction to Mature
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Mature;

            //Verify that a game rated Mature can be launched properly when the global rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail(
                    "Verify that a game rated Mature can be launched properly when the global rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Mature can be launched properly when the global rating is set to Mature");
            }

            //Verify that a game rated Ao is restricted when global rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao is restricted when global rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Ao is restricted when global rating is set to Mature");
            }

            //Set the global ESRB restriction to Ao
            Program.RestrictGlobalEsrbRatings = Enums.EsrbRatings.Ao;

            //Verify that a game rated Mature can be launched properly when the global rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail(
                    "Verify that a game rated Mature can be launched properly when the global rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Mature can be launched properly when the global rating is set to Ao");
            }

            //Verify that a game rated Ao can be launched properly when the global rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao can be launched properly when the global rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Ao can be launched properly when the global rating is set to Ao");
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
                EsrbRating = Enums.EsrbRatings.Everyone
            };

            IGame gameRatedE10 = new Game("game2.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Everyone10
            };

            IGame gameRatedT = new Game("game3.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Teen
            };

            IGame gameRatedM = new Game("game4.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Mature
            };

            IGame gameRatedAo = new Game("game5.bin", _console.ConsoleName)
            {
                EsrbRating = Enums.EsrbRatings.Ao
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
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Everyone can be launched properly when the user rating is set to Everyone");
            }

            //Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail("Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Everyone 10+ is restricted when user rating is set to Everyone");
            }

            //Set the user ESRB restriction to Everyone 10+
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Everyone10;

            //Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedE10);
                Assert.Fail(
                    "Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Everyone 10+ can be launched properly when the user rating is set to Everyone 10+");
            }

            //Verify that a game rated Teen is restricted when user rating is set to Everyone 10+
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail("Verify that a game rated Teen is restricted when user rating is set to Everyone 10+");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Teen is restricted when user rating is set to Everyone 10+");
            }

            //Set the user ESRB restriction to Teen
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Teen;

            //Verify that a game rated Teen can be launched properly when the user rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedT);
                Assert.Fail(
                    "Verify that a game rated Teen can be launched properly when the user rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Teen can be launched properly when the user rating is set to Teen");
            }

            //Verify that a game rated Mature is restricted when user rating is set to Teen
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail("Verify that a game rated Mature is restricted when user rating is set to Teen");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Mature is restricted when user rating is set to Teen");
            }

            //Set the user ESRB restriction to Mature
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Mature;

            //Verify that a game rated Mature can be launched properly when the user rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail(
                    "Verify that a game rated Mature can be launched properly when the user rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Mature can be launched properly when the user rating is set to Mature");
            }

            //Verify that a game rated Ao is restricted when user rating is set to Mature
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao is restricted when user rating is set to Mature");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Ao is restricted when user rating is set to Mature");
            }

            //Set the user ESRB restriction to Ao
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Ao;

            //Verify that a game rated Mature can be launched properly when the user rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedM);
                Assert.Fail(
                    "Verify that a game rated Mature can be launched properly when the user rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Mature can be launched properly when the user rating is set to Ao");
            }

            //Verify that a game rated Ao can be launched properly when the user rating is set to Ao
            try
            {
                FileOps.Launch(gameRatedAo);
                Assert.Fail("Verify that a game rated Ao can be launched properly when the user rating is set to Ao");
            }
            catch (Exception exception)
            {
                Assert.IsFalse(exception.Message.Contains("ESRB"),
                    "Verify that a game rated Ao can be launched properly when the user rating is set to Ao");
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
                    EsrbRating = esrb
                };

                //Verify that the game can be launched when the global rating is set to null
                try
                {
                    FileOps.Launch(game);
                    Assert.Fail(
                        $"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the global rating is set to Null");
                }
                catch (Exception exception)
                {
                    Assert.IsFalse(exception.Message.Contains("ESRB"),
                        $"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the global rating is set to Null");
                }
            }

            //Set the current user ESRB restriction to null
            Database.GetCurrentUser().AllowedEsrbRatings = Enums.EsrbRatings.Null;

            //Loop through each esrb rating and the game can be launched
            foreach (Enums.EsrbRatings esrb in Enum.GetValues(typeof(Enums.EsrbRatings)))
            {
                IGame game = new Game("game.bin", _console.ConsoleName)
                {
                    EsrbRating = esrb
                };

                //Verify that the game can be launched when the user rating is set to null
                try
                {
                    FileOps.Launch(game);
                    Assert.Fail(
                        $"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the user rating is set to Null");
                }
                catch (Exception exception)
                {
                    Assert.IsFalse(exception.Message.Contains("ESRB"),
                        $"Verify that a game rated {esrb.GetStringValue()} can be launched properly when the user rating is set to Null");
                }
            }
        }

        /// <summary>
        /// Verify that duplicate descriptors are not allowed for a game
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateDescriptorsAreNotAllowed()
        {
            //Create a new game object and verify that a descriptor can be added
            IGame game = new Game("myGame.bin", _console.ConsoleName);

            //Verify that a descriptor can be added properly
            Assert.IsTrue(game.AddEsrbDescriptor(Enums.EsrbDescriptors.MildLanguage),
                "Verify that a descriptor can be added properly");

            //Verify that a duplicate descriptor cannot be added
            Assert.IsFalse(game.AddEsrbDescriptor(Enums.EsrbDescriptors.MildLanguage),
                "Verify that a duplicate descriptor cannot be added");
        }

        /// <summary>
        /// Verify that ESRB descriptors are properly added, returned and removed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyAddRemoveEsrbDescriptors()
        {
            //Create a new game object and verify that a descriptor can be added
            IGame game = new Game("myGame.bin", _console.ConsoleName);

            //Add the first descriptor
            Assert.IsTrue(game.AddEsrbDescriptor(Enums.EsrbDescriptors.MildLanguage));

            //Add the second descriptor
            Assert.IsTrue(game.AddEsrbDescriptor(Enums.EsrbDescriptors.MildViolence));

            //Verify the current ESRB descriptor count
            Assert.IsTrue(game.GetEsrbDescriptors().Count == 2, "Verify the current ESRB descriptor count");

            //Verify that a nonexistent descriptor cannot be deleted
            Assert.IsFalse(game.DeleteEsrbDescriptor(Enums.EsrbDescriptors.MildBlood));

            //Remove an existing ESRB descriptor
            Assert.IsTrue(game.DeleteEsrbDescriptor(Enums.EsrbDescriptors.MildLanguage));

            //Verify that the descriptor count has been decremented
            Assert.IsTrue(game.GetEsrbDescriptors().Count == 1,
                "Verify that the descriptor count has been decremented");

            //Clear the esrb desriptor list
            game.ClearEsrbDescriptors();

            //Verify that the descriptors have been properly cleared
            Assert.IsTrue(game.GetEsrbDescriptors().Count == 0,
                "Verify that the descriptors have been properly cleared");
        }

        /// <summary>
        /// Verify that ESRB descriptors are properly parsed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyParseEsrbDescriptors()
        {
            //Create a new game
            IGame game = new Game("myGame.bin", _console.ConsoleName);

            //Parse the ESRB content descriptors from a string
            const string input = "Mild Violence, Mild Language, comic mischief";
            game.AddEsrbDescriptorsFromString(input);

            //Verify that the first descriptor has been properly parsed
            Assert.IsTrue(game.GetEsrbDescriptors().Contains(Enums.EsrbDescriptors.MildViolence),
                "Verify that the first descriptor has been properly parsed");

            //Verify that the second descriptor has been properly parsed
            Assert.IsTrue(game.GetEsrbDescriptors().Contains(Enums.EsrbDescriptors.MildLanguage),
                "Verify that the second descriptor has been properly parsed");

            //Verify that the third descriptor has been properly parsed
            Assert.IsTrue(game.GetEsrbDescriptors().Contains(Enums.EsrbDescriptors.ComicMischief),
                "Verify that the third descriptor has been properly parsed");

            //Verify that a nonexistnent descriptor does not exist in the current list
            Assert.IsFalse(game.GetEsrbDescriptors().Contains(Enums.EsrbDescriptors.StrongLanguage),
                "Verify that the third descriptor has been properly parsed");
        }

        /// <summary>
        /// Verify that ESRB descriptors are properly parsed
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyPrintEsrbDescriptorsAsString()
        {
            //Create a new game
            IGame game = new Game("myGame.bin", _console.ConsoleName);

            //Parse the ESRB content descriptors from a string
            const string descriptorString = "Mild Violence, Mild Language, Comic Mischief";
            game.AddEsrbDescriptorsFromString(descriptorString);

            //Verify that the proper string is returned
            Assert.IsTrue(game.GetEsrbDescriptorsString().Equals(descriptorString));
        }

    }
}
