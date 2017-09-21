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
    public class DatabaseTests
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
        /// Verify that the console count value is properly updated when 
        /// consoles are added or removed from the database
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyConsoleCount()
        {
            //Verify that the ConsoleCount equals 1 after adding one console in the Initalize function
            Assert.AreEqual(1, Database.GetConsoleCount(),
                "Verify that the ConsoleCount equals 1 after adding one console in the Initalize function");

            //Create a new console and add it to the database
            IConsole console2 = new Console("console2");
            Database.AddConsole(console2);

            //Verify that the console count is incrimenented after adding the second console
            Assert.AreEqual(2, Database.GetConsoleCount(),
                "Verify that the console count is incrimenented after adding the second console");

            //Remove one console and verify that the ConsoleCount has been decremented by 1
            Database.RemoveConsole(_console.ConsoleName);
            Assert.AreEqual(1, Database.GetConsoleCount(),
                "Remove one console and verify that the ConsoleCount has been decremented by 1");

            //Verify that you are not able to delete the last console
            Assert.IsFalse(Database.RemoveConsole(console2.ConsoleName),
                "Verify that you are not able to delete the last console");
            Assert.AreEqual(1, Database.GetConsoleCount(), "Verify that the console count has not been decremented");
        }


        /// <summary>
        /// Verify that the user count value is properly updated when 
        /// users are added or removed from the database
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserCount()
        {
            //Verify that the user count is equal to 1 intially 
            Assert.AreEqual(1, Database.GetUserCount(), "Verify that the user count is equal to 1 intially ");

            //Create a new user and add them to the database
            IUser user2 = new User("User2", "temp", 0, "user2@temp.com", 0, " ", Enums.EsrbRatings.Null, "");
            Database.AddUser(user2);

            //Verify that the user count is incrimenented after adding the second user
            Assert.AreEqual(2, Database.GetUserCount(),
                "Verify that the user count is incrimenented after adding the second user");

            //Remove the new user and verify the user count has been decremented by one
            Database.RemoveUser(user2.Username);
            Assert.AreEqual(1, Database.GetUserCount(),
                "Remove the new user and verify the user count has been decremented by one");

            //Verify that you are not able to delete the UniCade user
            Assert.IsFalse(Database.RemoveUser("UniCade"), "Verify that you are not able to delete the UniCade user");
            Assert.AreEqual(1, Database.GetUserCount(),
                "Verify that the console count has not been decremented after attempting to delete the UniCade user");
        }


        /// <summary>
        /// Verify the GetCurrentUser and SetCurrentUser Functions
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyGetAndSetCurrentUser()
        {
            //Verify that the current user is intially set to the default UniCade account
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals("UniCade"));

            //Verify SetCurrentUser returns false when attempting to add a user that does not exist in the current list 
            const string nonExistentUser = "tempUser";
            Assert.IsFalse(Database.SetCurrentUser(nonExistentUser));

            //Verify that the CurrentUser is not modified after attempting to add a nonexistent user
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals("UniCade"));

            //Add a new user to the UserList and attempt to change the current user
            IUser newUser = new User("newUser", "temp", 0, "newUser@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
            Database.AddUser(newUser);
            Assert.IsTrue(Database.SetCurrentUser(newUser.Username));

            //Verify that the current user has been changed
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals(newUser.Username));

            //Remove the newUser from the userList and verify that the current user has been reset to unicade
            Database.RemoveUser(newUser.Username);
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals("UniCade"));

            //Call RestoreDafaultUser and verify that the current user has been reset to UniCade
            Database.RestoreDefaultUser();
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals("UniCade"));

            //Add a new user to the UserList and attempt to change the current user
            IUser newUser2 = new User("newUser2", "temp", 0, "newUser2@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
            Database.AddUser(newUser2);
            Assert.IsTrue(Database.SetCurrentUser(newUser2.Username));

            //Verify that the current user has been changed
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals(newUser2.Username));

            //Call RestoreDefaultUser and verify that the current user has been reset to UniCade
            Database.RestoreDefaultUser();
            Assert.IsTrue(Database.GetCurrentUser().Username.Equals("UniCade"));
        }


        /// <summary>
        /// Verify that adding a game to a console properly incriments both the
        /// console game count and total game count
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyTotalGameCount()
        {
            //Store the original game count
            int originalTotalGameCount = Database.GetTotalGameCount();

            //Verify that the TotalGameCount is intially set to zero after creating a new database instance
            Assert.AreEqual(0, originalTotalGameCount,
                "Verify that the TotalGameCount is intially set to zero after creating a new database instance");

            //Verify that the console game count is intially set to zero
            int originalConsoleGameCount = _console.GetGameCount();
            Assert.AreEqual(0, originalConsoleGameCount, "Verify that the console game count is intially set to zero");

            //Add a game to the new console and verify that AddGame returns true
            IGame newGame = new Game("newGame.bin", _console.ConsoleName);
            Assert.IsTrue(_console.AddGame(newGame), "Verify that AddGame returns true when adding a new (valid) game");

            //Verify that the console game count has been incremented by one
            Assert.AreEqual((originalConsoleGameCount + 1), _console.GetGameCount(),
                "Verify that the console game count has been incremented by one");

            //Refresh the database that the total game count has been incremented by one
            Database.RefreshTotalGameCount();
            Assert.AreEqual((originalTotalGameCount + 1), Database.GetTotalGameCount(),
                "Verify that the console game count has been incremented by one");

            //Remove the game
            _console.RemoveGame(newGame.Title);

            //Verify that the console game count has been decremented by one after removing the game
            Assert.AreEqual(originalConsoleGameCount, _console.GetGameCount(),
                "Verify that the console game count has been incremented by one");

            //Refresh the database and verify that the console game count has been decremented by one after removing the game
            Database.RefreshTotalGameCount();
            Assert.AreEqual(originalTotalGameCount, Database.GetTotalGameCount(),
                "Verify that the console game count has been incremented by one");

            //Verify that attempting to remove a nonexistent game returns false 
            Assert.IsFalse(_console.RemoveGame(newGame.Title),
                "Verify that attempting to remove a nonexistent game returns false");
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
            Assert.IsFalse(Database.AddConsole(console2),
                "Verify that adding a console with a duplicate name is not allowed");

            //Verify that you are able to add new console with a unique name
            IConsole newConsole = new Console("uniqueConsole");
            Assert.IsTrue(Database.AddConsole(newConsole),
                "Verify that you are able to add new console with a unique name");
        }

        /// <summary>
        /// Verify that adding duplicate users with the same username
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyDuplicateUsersDisallowed()
        {
            //Verify that you are not able to add a new user with the username "UniCade"
            IUser uniCadeUser = new User("UniCade", "temp", 0, "unicade@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
            Assert.IsFalse(Database.AddUser(uniCadeUser));

            //Verify that you can add a new user with a unique username
            IUser newUser = new User("newUser", "temp", 0, "newuser@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
            Assert.IsTrue(Database.AddUser(newUser));

            //Verify that you can add a new user with a unique username
            IUser dupUser = new User("newUser", "temp", 0, "newuser2@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
            Assert.IsFalse(Database.AddUser(dupUser));
        }

        /// <summary>
        /// Verify that you are not able to add more than the max allowed number of consoles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyMaxConsoleCountRestrictions()
        {
            for (int iterator = 1; iterator < ConstValues.MaxConsoleCount; iterator++)
            {
                IConsole console = new Console(("console" + iterator));
                Assert.IsTrue(Database.AddConsole(console), $"Verify that console number {iterator}can be added properly");
            }

            IConsole lastConsole = new Console("lastConsole");
            Assert.IsFalse(Database.AddConsole(lastConsole), $"Verify that console number {ConstValues.MaxConsoleCount + 1} cannot be added since it exceeeds {ConstValues.MaxConsoleCount}");
        }

        /// <summary>
        /// Verify that you are not able to add more than the max allowed number of users
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyMaxUserCountRestrictions()
        {
            for (int iterator = 1; iterator < ConstValues.MaxUserCount; iterator++)
            {
                IUser user = new User(("user" + iterator), "temp", 0, "user@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
                Assert.IsTrue(Database.AddUser(user), $"Verify that user number {iterator}can be added properly");
            }

            IUser lastUser = new User("lastUser", "temp", 0, "user@unicade.com", 0, " ", Enums.EsrbRatings.Null, "");
            Assert.IsFalse(Database.AddUser(lastUser), $"Verify that user number {ConstValues.MaxUserCount + 1} cannot be added since it exceeeds {ConstValues.MaxUserCount}");
        }
    }
}
