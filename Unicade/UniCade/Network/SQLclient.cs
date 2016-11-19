using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace UniCade
{
    class SQLclient
    {
        #region Global Variables

        public static string sqlUser;
        public static MySqlConnection conn;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initiate a new SQL connection and return the connection string
        /// </summary>
        public static string connectSQL()
        {
            //Generate a new SQL connection
            conn = new MySqlConnection("server=127.0.0.1;" + "uid=root;" + "pwd=Star6120;" + "database=unicade;");

            try
            {
                sqlUser = null;
                conn.Open();
                return "connected";
            }
            catch
            {
                MessageBox.Show("UniCade Cloud connection Error");
                return string.Empty;
            }
        }

        /// <summary>
        /// Execute the SQL command passed in as a string param
        /// </summary>
        public static string processSQLcommand(string s)
        {
            //If the SQL connection is not already active, call connect function
            if (conn == null)
            {
                connectSQL();
            }
            MySqlCommand myCommand = new MySqlCommand(s, conn);

            //Attempt to execute the SQL command and print and exceptions to the console
            StringBuilder sb = new StringBuilder();
            try
            {
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
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

        /// <summary>
        /// Upload the entire game library from all consoles to the SQL database
        /// </summary>
        public static void uploadAllGames()
        {
            foreach (Console c in Database.ConsoleList)
            {
                c.GameList.ForEach(g => uploadGame(g));
            }
        }

        /// <summary>
        /// Download the entire game library from all active consoles
        /// </summary>
        public static void DownloadAllGames()
        {
            foreach (Console c in Database.ConsoleList)
            {
                for (int i = 0; i < c.GameList.Count; i++)
                {
                    Game g = (Game)c.GameList[i];
                    Game game = null;
                    game = getSingleGame(g.Console, g.Title);
                    if ((game != null) && (game.FileName.Length > 3))
                    {
                        c.GameList[i] = game;
                    }
                }
            }
        }

        /// <summary>
        /// Upload a single game to the database and return false if the game already exists
        /// </summary>
        public static bool uploadGame(Game g)
        {
            if (conn == null)
                connectSQL();

            //Check if the game already exists in the database
            MySqlCommand myCommand = new MySqlCommand("Use unicade;" + "select * FROM " + sqlUser + "_games WHERE filename = " + "\"" + g.FileName + "\"" + " AND console = " + "\"" + g.Console + "\"" + ";", conn);
            MySqlDataReader myReader = myCommand.ExecuteReader();
            if (myReader.Read())
            {
                if ((SafeGetString(myReader, 1).Equals(g.FileName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    System.Console.WriteLine("User Already Exists");
                    myReader.Close();
                    myCommand.Dispose();
                    return false;
                }
            }
            myReader.Close();
            myCommand.Dispose();

            //Generate a new command to add the game to the database
            string command = "Use unicade;" + " INSERT INTO " + sqlUser + "_games (filename,title, Console, LaunchCount, releaseDate, publisher, developer, userscore, criticscore, players, trivia, esrb, esrbdescriptors, esrbsummary, description, genres, tags, favorite)" + " VALUES (" + "\"" + g.FileName + "\",\"" + g.Title + "\",\"" + g.Console + "\",\"" + g.LaunchCount + "\",\"" + g.ReleaseDate + "\",\"" + g.Publisher + "\",\"" + g.Developer + "\",\"" + g.UserScore + "\",\"" + g.CriticScore + "\",\"" + g.Players + "\",\"" + g.Trivia + "\",\"" + g.Esrb + "\",\"" + g.EsrbDescriptor + "\",\"" + g.EsrbSummary + "\",\"" + g.Description + "\",\"" + g.Genres + "\",\"" + g.Tags + "\",\"" + g.Favorite + "\");";
            myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();
            return true;
        }

        public static Game getSingleGame(string con, string gam)
        {
            if (conn == null)
                connectSQL();

            MySqlCommand myCommand = new MySqlCommand("Use unicade;" + "select * FROM " + sqlUser + "_games WHERE title = " + "\"" + gam + "\"" + " AND console = " + "\"" + con + "\"" + ";", conn);
            MySqlDataReader myReader = null;
            try
            {
                myReader = myCommand.ExecuteReader();
                Game g = null;
                if (myReader.Read())
                {
                    g = new Game(SafeGetString(myReader, 1), SafeGetString(myReader, 3), SafeGetInt32(myReader, 4), SafeGetString(myReader, 5), SafeGetString(myReader, 6), SafeGetString(myReader, 7), SafeGetString(myReader, 8), SafeGetString(myReader, 9), SafeGetString(myReader, 10), SafeGetString(myReader, 11), SafeGetString(myReader, 12), SafeGetString(myReader, 13), SafeGetString(myReader, 14), SafeGetString(myReader, 15), SafeGetString(myReader, 16), SafeGetString(myReader, 17), SafeGetInt32(myReader, 18));
                }
                else
                {
                    myCommand.Dispose();
                    myReader.Close();
                    return null;
                }

                myReader.Close();
                myCommand.Dispose();
                return g;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                myReader.Close();
                return null;
            }
        }

        /// <summary>
        /// Verify user credentals against the remote database.
        /// Return true if the credentials are valid
        /// </summary>
        public static bool AuthiencateUser(string user, string pass)
        {
            if (conn == null)
                connectSQL();

            //Generate a new SQL command
            string command = "Use unicade;" + "select * FROM users WHERE username = " + "\"" + user + "\"" + " OR email = " + "\"" + user + "\"" + ";";
            MySqlCommand myCommand = new MySqlCommand(command, conn);

            //Execute the command and handle the response
            MySqlDataReader myReader = myCommand.ExecuteReader();
            myReader.Read();
            if (pass.Equals(SafeGetString(myReader, 2), StringComparison.InvariantCultureIgnoreCase))
            {
                sqlUser = user;
                myReader.Close();
                myCommand.Dispose();
                return true;
            }
            else
            {
                myReader.Close();
                myCommand.Dispose();
                return false;
            }
        }

        /// <summary>
        /// Create a new user in the remote SQL database
        /// </summary>
        public static bool CreateUser(string username, string pass, string email, string info, string esrb, string profPic)
        {
            if (conn == null)
                connectSQL();

            MySqlCommand myCommand = new MySqlCommand("Use unicade;" + "select * FROM users WHERE username = " + "\"" + username + "\"" + " OR email = " + "\"" + email + "\"" + ";", conn);
            MySqlDataReader myReader = myCommand.ExecuteReader();
            if (myReader.Read())
            {
                if ((SafeGetString(myReader, 1).Equals(username, StringComparison.InvariantCultureIgnoreCase)) || (SafeGetString(myReader, 3).Equals(email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    myReader.Close();
                    myCommand.Dispose();
                    return false;
                }
            }
            myReader.Close();
            myCommand.Dispose();

            //If operation is sucuessful, add the new user into the SQL database
            string command = "Use unicade;" + "INSERT INTO users (username,password,email,info,allowedEsrb,logincount,LaunchCount,profilepic) VALUES (\"" + username + "\",\"" + pass + "\",\"" + email + "\",\"" + info + "\",\"" + esrb + "\",\"" + "0" + "\",\"" + "0" + "\",\"" + "nullProfPath" + "\");";
            myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();

            //Fetch the SQL structure command from the TextFiles class
            command = TextFiles.SqlStructure;

            //Execute the command and add the new user to the database
            command = command.Replace("games", (username + "_games"));
            myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();
            myReader.Close();
            myCommand.Dispose();
            return true;
        }

        /// <summary>
        /// Deletes all games for the current user from the SQL database
        /// </summary>
        public static void Deletegames()
        {
            if (conn == null)
                connectSQL();

            //Generate and execute the command to delete all games for the current user
            string command = "Use unicade;" + "DELETE FROM " + sqlUser + "_games WHERE id>0;";
            MySqlCommand myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();
            myCommand.Dispose();
        }

        /// <summary>
        /// Delete the current user profile and the associated game library from the remote database
        /// </summary>
        public static void DeleteUser()
        {
            if (conn == null)
                connectSQL();

            //Generate and execute the command to remove the user profile from the database
            string command = "Use unicade;" + "DELETE FROM users WHERE username = " + "\"" + sqlUser + "\";";
            MySqlCommand myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();
            myCommand.Dispose();

            //Generate and execute the command to remove the user's game library from the database
            command = "Use unicade;" + "DROP TABLE " + sqlUser + "_games;";
            myCommand = new MySqlCommand(command, conn);
            myCommand.ExecuteNonQuery();
            myCommand.Dispose();
            sqlUser = null;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Retrive the string response from the SQL server and handle invalid responses by returning null
        /// </summary>
        public static string SafeGetString(MySqlDataReader reader, int colIndex)
        {
            try
            {
                if (!reader.IsDBNull(colIndex))
                    return reader.GetString(colIndex);
                else
                    return string.Empty;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrive the int response from the SQL server and handle invalid responses by returning null
        /// </summary>
        public static int SafeGetInt32(MySqlDataReader reader, int colIndex)
        {
            try
            {
                if (!reader.IsDBNull(colIndex))
                    return reader.GetInt32(colIndex);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}
