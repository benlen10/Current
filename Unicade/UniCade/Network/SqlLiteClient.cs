using System.Data.SQLite;
using System.IO;
using System.Text;
using UniCade.Backend;
using UniCade.Constants;
using UniCade.Interfaces;
using UniCade.Objects;

namespace UniCade.Network
{
    internal static class SqlLiteClient
    {
        #region Properties

        /// <summary>
        /// The current connection to the database
        /// </summary>
        private static SQLiteConnection _connection;

        /// <summary>
        /// The currently logged in username
        /// </summary>
        private static string _currentSqlUsername;

        #endregion

        #region  Methods

        /// <summary>
        /// Create a new local database file and connect
        /// </summary>
        internal static void Connect()
        {
            if (!File.Exists(ConstValues.SqlDatabaseFileName))
            {
                SQLiteConnection.CreateFile(ConstValues.SqlDatabaseFileName);
            }
            if (_connection == null)
            {
                _connection = new SQLiteConnection($"Data Source={ConstValues.SqlDatabaseFileName};Version=3;");
                _connection.Open();
            }
        }


        internal static void CreateUsersTable()
        {
            ExecuteNonQuery(SqlCommands.CreateUsersTable);
        }

        internal static bool CreateNewUser(string username, string password, string email, string userInfo, string allowedEsrb)
        {
            //Check if an user with the existing username exists
            string command = $"SELECT * FROM users WHERE username = \"{username}\"";
            var reader = ExecuteQuery(command);
            if (reader.HasRows)
            {
                return false;
            }

            //Create a new user entry in the users table
            command = $"INSERT INTO users (username,password,email,userinfo,allowedEsrb) VALUES (\"{username}\", \"{password}\", \"{email}\", \"{userInfo}\", \"{allowedEsrb}\");";
            ExecuteNonQuery(command);

            //Create a games table for the new user
            command = SqlCommands.CreateGamesTable.Replace("[Username]", username);
            ExecuteNonQuery(command);
            return true;
        }

        internal static void DeleteCurrentUser()
        {
            if (_currentSqlUsername != null)
            {
                //Delete the current user from the users table
                string command = $"DELETE FROM users WHERE username = \"{_currentSqlUsername}\";";
                ExecuteNonQuery(command);

                //Drop the user's games table
                command = $"DROP TABLE games_{_currentSqlUsername}";
                ExecuteNonQuery(command);

                //Set the current user to null
                _currentSqlUsername = null;
            }
        }

        internal static bool Login(string username, string password)
        {
            string command = $"SELECT * FROM users WHERE username = \"{username}\"";

            var reader = ExecuteQuery(command);
            while (reader.Read())
            {
                if (password.Equals(reader["password"].ToString()))
                {
                    _currentSqlUsername = username;
                    return true;
                }
            }
            return false;
        }

        internal static void Logout()
        {
            _currentSqlUsername = null;
        }

        internal static bool UploadGame(IGame g)
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }
            int fav = g.Favorite ? 1 : 0;
            string command = $"INSERT INTO games_{ _currentSqlUsername}  VALUES (\"{g.FileName}\", \"{g.Title}\", \"{g.ConsoleName}\" ,{g.GetLaunchCount()}, \"{g.ReleaseDate}\", \"{g.PublisherName}\", \"{g.DeveloperName}\", \"{g.UserReviewScore}\",  \"{g.CriticReviewScore}\", \"{g.SupportedPlayerCount}\", \"{g.Trivia}\", \"{g.EsrbRatingsRating.GetStringValue()}\", \"{g.GetEsrbDescriptorsString()}\", \"{g.EsrbSummary}\", \"{g.Description}\", \"{g.Genres}\", \"{g.Tags}\", {fav});";
            ExecuteNonQuery(command);

            return true;
        }

        internal static bool DeleteAllUserGames()
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }

            string command = $"DELETE FROM games_{_currentSqlUsername}";
            ExecuteNonQuery(command);
            return true;
        }

        internal static bool DownloadGameInfo(IGame game)
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }

            string command = $"SELECT * FROM games_{_currentSqlUsername} WHERE filename = \"{game.FileName}\" AND console = \"{game.ConsoleName}\"";

            var reader = ExecuteQuery(command);

            //Return false if the game is not found
            if (!reader.HasRows)
            {
                return false;
            }

            //Populate the game fields
            reader.Read();
            game.SetLaunchCount(int.Parse(reader["launchCount"].ToString()));
            game.ReleaseDate = reader["releaseDate"].ToString();
            game.PublisherName = reader["publisher"].ToString();
            game.DeveloperName = reader["developer"].ToString();
            game.UserReviewScore = reader["userScore"].ToString();
            game.CriticReviewScore = reader["criticScore"].ToString();
            game.SupportedPlayerCount = reader["players"].ToString();
            game.Trivia = reader["trivia"].ToString();
            game.EsrbRatingsRating = Utilties.ParseEsrbRating(reader["esrbRating"].ToString());
            game.AddEsrbDescriptorsFromString(reader["esrbDescriptors"].ToString());
            game.EsrbSummary = reader["esrbSummary"].ToString();
            game.Description = reader["description"].ToString();
            game.Genres = reader["genres"].ToString();
            game.Tags = reader["tags"].ToString();
            game.Favorite = int.Parse(reader["favorite"].ToString()) == 1;
            return true;
        }

        internal static string GetCurrentUsername()
        {
            return _currentSqlUsername;
        }


        internal static bool UploadAllGamesForConsole(IConsole console)
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }
            StringBuilder command = new StringBuilder();

            foreach (string gameName in console.GetGameList())
            {
                Game g = (Game)console.GetGame(gameName);
                int fav = g.Favorite ? 1 : 0;
                command.Append(
                    $"INSERT INTO games_{_currentSqlUsername}  VALUES (\"{g.FileName}\", \"{g.Title}\", \"{g.ConsoleName}\" ,{g.GetLaunchCount()}, \"{g.ReleaseDate}\", \"{g.PublisherName}\", \"{g.DeveloperName}\", \"{g.UserReviewScore}\",  \"{g.CriticReviewScore}\", \"{g.SupportedPlayerCount}\", \"{g.Trivia}\", \"{g.EsrbRatingsRating.GetStringValue()}\", \"{g.GetEsrbDescriptorsString()}\", \"{g.EsrbSummary}\", \"{g.Description}\", \"{g.Genres}\", \"{g.Tags}\", {fav});\n");
            }
            ExecuteNonQuery(command.ToString());

            return false;
        }

        internal static bool UploadAllGames()
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }

            foreach (string consoleName in Database.GetConsoleList())
            {
                UploadAllGamesForConsole(Database.GetConsole(consoleName));
            }
            return true;
        }


        internal static bool DownloadAllGamesForConsole(IConsole console)
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }

            string command = $"SELECT * FROM games_{_currentSqlUsername} WHERE console = \"{console.ConsoleName}\";";

            var reader = ExecuteQuery(command);

            while (reader.Read())
            {
                Game game = (Game) console.GetGame(reader["title"].ToString());
                if (game != null)
                {
                    game.SetLaunchCount(int.Parse(reader["launchCount"].ToString()));
                    game.ReleaseDate = reader["releaseDate"].ToString();
                    game.PublisherName = reader["publisher"].ToString();
                    game.DeveloperName = reader["developer"].ToString();
                    game.UserReviewScore = reader["userScore"].ToString();
                    game.CriticReviewScore = reader["criticScore"].ToString();
                    game.SupportedPlayerCount = reader["players"].ToString();
                    game.Trivia = reader["trivia"].ToString();
                    game.EsrbRatingsRating = Utilties.ParseEsrbRating(reader["esrbRating"].ToString());
                    game.AddEsrbDescriptorsFromString(reader["esrbDescriptors"].ToString());
                    game.EsrbSummary = reader["esrbSummary"].ToString();
                    game.Description = reader["description"].ToString();
                    game.Genres = reader["genres"].ToString();
                    game.Tags = reader["tags"].ToString();
                    game.Favorite = int.Parse(reader["favorite"].ToString()) == 1;
                }
            }

            return false;
        }

        internal static bool DownloadAllGames()
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }

            foreach (string consoleName in Database.GetConsoleList())
            {
                DownloadAllGamesForConsole(Database.GetConsole(consoleName));
            }

            return false;
        }


        #endregion

        #region  Helper Methods

        private static int ExecuteNonQuery(string input)
        {
            Connect();
            SQLiteCommand command = new SQLiteCommand(input, _connection);
            return command.ExecuteNonQuery();
        }

        private static SQLiteDataReader ExecuteQuery(string query)
        {
            Connect();
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            return command.ExecuteReader();
        }

        #endregion
    }
}
