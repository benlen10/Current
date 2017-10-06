using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
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
            ExecuteNonQuery(Constants.SqlCommands.CreateUsersTable);
        }

        internal static void CreateNewUser(string username, string password, string email, string userInfo, string allowedEsrb)
        {
            //Create a new user entry in the users table
            string command = $"INSERT INTO users (username,password,email,userinfo,allowedEsrb) VALUES (\"{username}\", \"{password}\", \"{email}\", \"{userInfo}\", \"{allowedEsrb}\");";
            ExecuteNonQuery(command);

            //Create a games table for the new user
            command = SqlCommands.CreateGamesTable.Replace("[UserName]", username);
            ExecuteNonQuery(command);
        }

        internal static bool DeleteCurrentUser(string username)
        {
            if (username.Equals(_currentSqlUsername))
            {
                string command = $"DELETE FROM users WHERE username = \"{username}\";";
                ExecuteNonQuery(command);
                _currentSqlUsername = null;
                return true;
            }
            return false;
        }

        internal static bool Login(string username, string password)
        {
            string command = $"SELECT * FROM users WHERE username = \"{username}\"";

            var reader = ExecuteQuery(command);
            while (reader.Read())
            {
                if (password.Equals(reader["password"]))
                {
                    _currentSqlUsername = username;
                }
                return true;
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

            string command =  $"INSERT INTO + { _currentSqlUsername}_games (filename, title, Console, LaunchCount, releaseDate, publisher, developer, userscore, criticscore, players, trivia, esrb, esrbdescriptors, esrbsummary, description, genres, tags, favorite) VALUES (\"{g.FileName}\" \"{g.Title}\" \"{g.ConsoleName}\" \"{g.GetLaunchCount()}\" \"{g.ReleaseDate}\" \"{g.PublisherName}\" \"{g.DeveloperName}\" \"{g.UserReviewScore}\" \"{g.SupportedPlayerCount}\" \"{g.Trivia}\" \"{g.EsrbRatingsRating.GetStringValue()}\" \"{g.GetEsrbDescriptorsString()}\" \"{g.EsrbSummary}\" \"{g.Description}\" \"{g.Genres}\" \"{g.Tags}\" \"{g.Favorite}\"  );";
            ExecuteNonQuery(command);

            return true;
        }

        internal static bool DeleteAllUserGames()
        {
            if (_currentSqlUsername == null)
            {
                return false;
            }

            return true;
        }

        internal static bool UploadAllGames()
        {
            return true;
        }

            internal static bool DownloadGameInfo(IGame game)
        {
            return false;
        }

        internal static bool DownloadAllGamesForConsole(IConsole console)
        {
            return false;
        }

        internal static bool DownloadAllGames()
        {
            return false;
        }

        internal static string GetCurrentUsername()
        {
            return _currentSqlUsername;
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
