using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UniCadeCmd
{
    class SQLclient
    {

        public static bool connectSQL()
        {
            SqlConnection myConnection = new SqlConnection("user id=root;" + "password=Star6120;server=serverurl;" + "Trusted_Connection=yes;" + "database=database; " + "connection timeout=30");

            try
            {
                myConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return false;
            }

        }
    }
}
