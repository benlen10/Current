using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Objects;

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

        #region Public User Function Tests

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyChangingUserPassword()
        {
            //Fetch the original password
            string originalPassword = _user.GetUserPassword();

            //Verify that a null value for the password is not allowed
            try
            {
                _user.SetUserPassword(null);
                Assert.Fail("Verify that a null value for a password is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that a null value for a password is not allowed");
            }

            //Verify that invalid chars are not allowed in the password
            const string invalidpassword = "pass|word";
            try
            {
                _user.SetUserPassword(invalidpassword);
                Assert.Fail("Verify that invalid chars are not allowed in the console name");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that invalid chars are not allowed in the password");
            }

            //Verify that a password that is less than MinpasswordLength chars is not allowed
            string shortpassword = new string('-', ConstValues.MinUserPasswordLength - 1);
            try
            {
                _user.SetUserPassword(shortpassword);
                Assert.Fail($"Verify that a password that is less than {ConstValues.MinUserPasswordLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a password that is less than {ConstValues.MinUserPasswordLength} chars is not allowed");
            }

            //Verify that a passwordthat exceeds MaxConsoleNameLength chars is not allowed
            string longName = new string('-', ConstValues.MaxUserPasswordLength + 1);
            try
            {
                _user.SetUserPassword(longName);
                Assert.Fail($"Verify that a password that exceeds {ConstValues.MaxUserPasswordLength} chars is not allowed");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true,
                    $"Verify that a password that exceeds {ConstValues.MaxUserPasswordLength} chars is not allowed");
            }

            //Verify that the password has not been changed
            Assert.AreEqual(originalPassword, _user.GetUserPassword(), "Verify that the password has not been changed");

            //Set a new user password
            const string newPassword = "newPassword";
            _user.SetUserPassword(newPassword);

            //Verify that the password has been properly saved
            Assert.IsTrue(_user.GetUserPassword().Equals(newPassword), "Verify that a valid password is properly saved");
        }

        /// <summary>
        /// Verify that the user launch count is properly incremented
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserLaunchCount()
        {
            //Verify that the user launch count is intially set to zero
            Assert.IsTrue(_user.GetUserLaunchCount() == 0, "Verify that the user launch count is intially set to zero");

            //Increment the user launch count
            _user.IncrementUserLaunchCount();

            //Verify that the user launch count has been properly incremented
            Assert.IsTrue(_user.GetUserLaunchCount() == 1, "Verify that the user launch count is intially set to zero");
        }

        /// <summary>
        /// Verify that the user login count can be properly incremented 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUserLoginCount()
        {
            //Verify that the user login count is intially set to zero
            Assert.IsTrue(_user.GetUserLoginCount() == 0, "Verify that the user login count is intially set to zero");

            //Increment the user login count
            _user.IncrementUserLoginCount();

            //Verify that the user login count has been properly incremented
            Assert.IsTrue(_user.GetUserLoginCount() == 1, "Verify that the user login count is intially set to zero");
        }

        /// <summary>
        /// Verify that some fields for the default unicade account cannot be edited
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void VerifyUniCadeAccountEditingRestrictions()
        {
            //Fetch the UniCade user object
            IUser unicadeUser = Database.GetCurrentUser();

            //Verify that you are not able to edit the username for the UniCade account
            try
            {
                unicadeUser.Username = "newUsername";
                Assert.Fail("Verify that you are not able to edit the username for the UniCade account");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that you are not able to edit the username for the UniCade account");
            }

            //Verify that you are not able to edit the password for the UniCade account
            try
            {
                unicadeUser.SetUserPassword("newPass");
                Assert.Fail("Verify that you are not able to edit the password for the UniCade account");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that you are not able to edit the password for the UniCade account");
            }

            //Verify that you are not able to edit the email for the UniCade account
            try
            {
                unicadeUser.Email = "newAddress@gmail.com";
                Assert.Fail("Verify that you are not able to edit the email for the UniCade account");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that you are not able to edit the email for the UniCade account");
            }

            //Verify that you are not able to edit the user info for the UniCade account
            try
            {
                unicadeUser.UserInfo = "NewUserInfo";
                Assert.Fail("Verify that you are not able to edit the user info for the UniCade account");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that you are not able to edit the user info for the UniCade account");
            }

            //Verify that you are not able to edit the profile pic path for the UniCade account
            try
            {
                unicadeUser.UserInfo = "NewUserInfo";
                Assert.Fail("Verify that you are not able to edit the profile pic for the UniCade account");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true, "Verify that you are not able to edit the profile pic path for the UniCade account");
            }
        }

        #endregion

    }
}
