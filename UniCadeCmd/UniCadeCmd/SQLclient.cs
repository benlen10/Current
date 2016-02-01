using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace UniCadeCmd
{
    class SQLclient
    {
        public static MySqlConnection conn;

        public static string connectSQL()
        {
            
            conn = new MySqlConnection("server=127.0.0.1;"+ "uid=root;" +"pwd=Star6120;"+"database=unicade;");

            try
            {
                conn.Open();
                return "connected";
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return e.ToString();
            }

        }

        public static string processSQLcommand(string s)
        {
            MySqlCommand myCommand = new MySqlCommand(s, conn);

            StringBuilder sb = new StringBuilder();
            try
            {
                MySqlDataReader myReader = null;

                 myReader= myCommand.ExecuteReader();
                int col = 0;
                while (myReader.Read())
                {
                    sb.Append(myReader.GetString(col));
                }
                myReader.Close();
                myCommand.Dispose();
                return sb.ToString();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                
                return e.ToString();
            }


        }

        public static bool addGame()
        {
            return false;
        }

        public static string getGameByConsole(string con)
        {
            return null;
        }

        public static Game getSingleGame(string con, string gam)
        {
            MySqlCommand myCommand = new MySqlCommand("Use unicade;"+ "select * FROM games WHERE title = "+ "\""+ gam + "\""+ " AND console = " + "\""+ con + "\""  +  ";", conn);

            StringBuilder sb = new StringBuilder();
            try
            {
                MySqlDataReader myReader = null;

                myReader = myCommand.ExecuteReader();
                int col = 1;
                myReader.Read();
                Game g = new Game(myReader.GetString(1), myReader.GetString(3), myReader.GetInt32(4), myReader.GetString(5), myReader.GetString(6), myReader.GetString(7), myReader.GetString(8), myReader.GetString(9), myReader.GetString(10), myReader.GetString(11), myReader.GetString(12), myReader.GetString(13), myReader.GetString(14), myReader.GetString(15), myReader.GetString(16), myReader.GetString(16), myReader.GetInt32(16));
                while (myReader.Read())
                {
                    sb.Append(myReader.GetString(col));
                }
                myReader.Close();
                myCommand.Dispose();
                return g;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());

                return null;
            }

        }

        public static string getAllGames()
        {
            return null;
        }

        public static string getUsers()
        {
            return null;
        }

        public static bool authiencateUser()
        {
            return false;
        } 

    }
}
