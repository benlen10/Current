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
            if (conn == null)
            {
                connectSQL();
            }
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

        public static string uploadGame(Game g)
        {
            if (conn == null)
            {
                connectSQL();
            }
            String command = "Use unicade;" + " INSERT INTO games" + "(filename,title, Console, launchcount, releaseDate, publisher, developer, userscore, criticscore, players, trivia, esrb, esrbdescriptors, esrbsummary, description, genres, tags, favorite)" + " VALUES (" + "\"" + g.getFileName() + "\",\"" + g.getTitle() + "\",\"" + g.getConsole() + "\",\"" + g.launchCount + "\",\"" + g.getReleaseDate() + "\",\"" + g.getPublisher() + "\",\"" + g.getDeveloper() + "\",\"" + g.getUserScore() + "\",\"" + g.getCriticScore() + "\",\"" + g.getPlayers() + "\",\"" + g.getTrivia() + "\",\"" + g.getEsrb() + "\",\"" + g.getEsrbDescriptor() + "\",\"" + g.getEsrbSummary() + "\",\"" + g.getDescription() + "\",\"" + g.getGenres() + "\",\"" + g.getTags() + "\",\"" + g.getFav() + "\");";
            MySqlCommand myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();
            return command;
        }

        public static string getGameByConsole(string con)
        {
            return null;
        }

        public static Game getSingleGame(string con, string gam)
        {
            if (conn == null)
            {
                connectSQL();
            }
            MySqlCommand myCommand = new MySqlCommand("Use unicade;"+ "select * FROM games WHERE title = "+ "\""+ gam + "\""+ " AND console = " + "\""+ con + "\""  +  ";", conn);

            StringBuilder sb = new StringBuilder();
            try
            {
                MySqlDataReader myReader = null;

                myReader = myCommand.ExecuteReader();
                int col = 1;
                myReader.Read();
                Game g = new Game(SafeGetString(myReader,1), SafeGetString(myReader,3), SafeGetInt32(myReader,4), SafeGetString(myReader, 5), SafeGetString(myReader, 6), SafeGetString(myReader, 7), SafeGetString(myReader, 8), SafeGetString(myReader, 9), SafeGetString(myReader, 10), SafeGetString(myReader,11), SafeGetString(myReader, 12), SafeGetString(myReader, 13), SafeGetString(myReader, 14), SafeGetString(myReader, 15), SafeGetString(myReader, 16), SafeGetString(myReader, 17), SafeGetInt32(myReader, 18));
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
            if (conn == null)
            {
                connectSQL();
            }
            return null;
        }

        public static string getUsers()
        {
            if (conn == null)
            {
                connectSQL();
            }
            return null;
        }

        public static bool authiencateUser()
        {
            if (conn == null)
            {
                connectSQL();
            }
            return false;
        }

        public static bool createUser(User u)
        {
            if (conn == null)
            {
                connectSQL();
            }
            return false;
        }

        public static bool createUser(string username, string pass, string email, string info, string esrb, string profPic)
        {
            if (conn == null)
            {
                connectSQL();
            }
            string command = "Use unicade;" + "INSERT INTO users (username,password,email,info,allowedEsrb,logincount,launchcount,profilepic) VALUES (\"" + username + "\",\"" + pass + "\",\"" + email + "\",\"" + info + "\",\"" + esrb + "\",\"" + "0" + "\",\"" + "0" + "\",\"" + "nullProfPath" + "\");";
            MySqlCommand myCommand = new MySqlCommand(command,conn);
            myCommand.ExecuteNonQuery();
            return true;
        }

        public static bool deleteUser()
        {
            if (conn == null)
            {
                connectSQL();
            }
            return false;
        }

        public static string SafeGetString(MySqlDataReader reader, int colIndex)
        {
            if (conn == null)
            {
                connectSQL();
            }
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            else
                return string.Empty;
        }

        public static int SafeGetInt32(MySqlDataReader reader, int colIndex)
        {
            if (conn == null)
            {
                connectSQL();
            }
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt32(colIndex);
            else
                return 0;
        }

    }
}
