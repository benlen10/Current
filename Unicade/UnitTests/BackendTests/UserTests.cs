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
    public class UserTests
    {

        /// <summary>
        /// Create a new database instance and a new user object
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();

            //Create a new user
            _user = new User("user", "temp", 0, "user@unicade.com", 0, " ", Enums.Esrb.Null, "");
        }

        #region  Properties

        private IUser _user;

        #endregion
        

        #region  User Property Validation Tests

        /// <summary>
        /// Verify that console names are properly validated 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateUsername()
        {
            //Verify that a null value for the username is not allowed
            try
            {
                _user.Username = null;
                Assert.Fail("Verify that a null value for a username is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a username is not allowed");
            }

            //Verify that an empty value for the username is not allowed
            try
            {
                _user.Username = "";
                Assert.Fail("Verify that an empty value for a username is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that an empty value for a username is not allowed");
            }

            //Verify that invalid chars are not allowed in the username
            const string invalidUsername = "a|c";
            try
            {
                _user.Username = invalidUsername;
                Assert.Fail("Verify that invalid chars are not allowed in the console name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the username");
            }

            //Verify that a username that is less than MinUsernameLength chars is not allowed
            string shortUsername = new string('-', ConstValues.MinUsernameLength - 1);
            try
            {
                _user.Username = shortUsername;
                Assert.Fail($"Verify that a username that is less than {ConstValues.MinUsernameLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a consle name that is less than {ConstValues.MinUsernameLength} chars is not allowed");
            }

            //Verify that a usernamethat exceeds MaxConsoleNameLength chars is not allowed
            string longName = new string('-', ConstValues.MaxUsernameLength + 1);
            try
            {
                _user.Username = longName;
                Assert.Fail($"Verify that a username that exceeds {ConstValues.MaxUsernameLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a consle name that exceeds {ConstValues.MaxUsernameLength} chars is not allowed");
            }

            //Verify that valid console names are properly saved
            const string validUserName = "validUsername";
            _user.Username = validUserName;
            Assert.AreEqual(_user.Username, validUserName, "Verify that valid ROM paths are properly saved");
        }

        #endregion

        #region Public User Function 

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserLaunchCount()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserLoginCount()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyChangingUserPassword()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUniCadeAccountEditingRestrictions()
        {

        }

        #endregion

    }
}
