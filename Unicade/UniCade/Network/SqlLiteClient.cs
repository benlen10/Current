using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.Network
{
    static class SqlLiteClient
    {

        private static SQLiteConnection sql_con;
        private static SQLiteCommand sql_cmd;
        private static SQLiteDataAdapter DB;
        private static DataSet DS = new DataSet();
        private static DataTable DT = new DataTable();

        // Holds our connection with the database
        static SQLiteConnection m_dbConnection;
    }
}
