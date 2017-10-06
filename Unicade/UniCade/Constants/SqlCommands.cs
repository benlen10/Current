
namespace UniCade.Constants
{
    internal static class SqlCommands
    {
        /// <summary>
        /// Creates a new games table for a single user
        /// </summary>
        public const string CreateGamesTable = @"
DROP TABLE IF EXISTS games_[Username];
CREATE TABLE games_[Username](
  filename        TEXT NOT NULL, -- full ROM filename
  title           TEXT NOT NULL, -- Game title
  console         TEXT NOT NULL, -- Console game
  launchCount     INTEGER,   -- Number of times the game has been launched
  releaseDate     TEXT,      -- Release date
  publisher       TEXT,      -- Game publisher
  developer       TEXT,      -- Game developer
  userscore       TEXT,      -- Average user score
  criticscore     TEXT,      -- Average critic score
  players         TEXT,      -- Number of supported local players
  trivia          TEXT,      -- Trivia or extra info related to the game
  esrbRating      TEXT,      -- ESRB content rating
  esrbDescriptors TEXT,      -- ESRB content rating descriptors
  esrbSummary     TEXT,      -- ESRB content rating summary
  description     INTEGER,   -- Brief game description
  genres          TEXT,      -- Main genres associated with the game
  tags            TEXT,      -- Revelant game tags
  favorite        INTEGER,   -- int value describing the favorite status
  PRIMARY KEY(title, console));";


        /// <summary>
        /// Creates a new table to store all unicade users
        /// </summary>
        public const string CreateUsersTable = @"
DROP TABLE IF EXISTS users;
CREATE TABLE users(
  username        TEXT,
  password        TEXT,
  email           TEXT,     
  userinfo        TEXT, 
  userLoginCount  INTEGER, 
  userLaunchCount INTEGER,
  allowedEsrb      TEXT,
  PRIMARY KEY(username));";
    }
}
