using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniCade.Backend;
using UniCade.Network;

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
            SqlLiteClient.Initalize();
        }

        #endregion
    }
}
