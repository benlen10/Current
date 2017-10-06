using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Network;
using UniCade.Objects;

namespace UnitTests.NetworkTests
{
    [TestClass]
    public class SqlTests
    {
        /// <summary>
        /// Initalize the database
        /// </summary>
        [TestInitialize]
        public void Initalize()
        {
            //Initalize the program
            Database.Initalize();
        }

        #region SQL Tests

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NullTest()
        {
            SqlLiteClient.Connect();
            SqlLiteClient.CreateUsersTable();

            const string userName = "BenLen";
            const string password = "tempPass";

            SqlLiteClient.CreateNewUser(userName, password, "benlen10@gmail.com", "userInfo", "Null");
            Assert.IsTrue(SqlLiteClient.Login(userName, password));

            string gameName = "Super Mario World.snes";
            string consoleName = "SNES";
            Game game = new Game(gameName, consoleName);
            Assert.IsTrue(SqlLiteClient.UploadGame(game));

            //game = new Game("Null.bin", "Null");

        }

        #endregion
    }
}
