using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
  id              int unsigned NOT NULL auto_increment, # Unique ID for the record
  filename        varchar(255),      # full ROM filename
  title           varchar(255),      # Game title
  Console         varchar(255),      # Console game
  LaunchCount     smallint(127),     # Number of times the game has been launched
  releaseDate     varchar(255),      # Release date
  publisher       varchar(255),      # Game publisher
  developer       varchar(255),      # Game developer
  userscore       varchar(255),      # Average user score
  criticscore     varchar(255),      # Average critic score
  players         varchar(255),      # Number of supported local players
  trivia          varchar(255),      # Trivia or extra info related to the game
  esrb            varchar(255),      # ESRB content rating
  esrbdescriptors varchar(255),      # ESRB content rating descriptors
  esrbsummary     varchar(255),      # ESRB content rating summary
  description     varchar(9000),     # Brief game description
  genres          varchar(255),      # Main genres associated with the game
  tags            varchar(255),      # Revelant game tags
  favorite       smallint(127),      # int value describing the favorite status
  PRIMARY KEY(id));";


        /// <summary>
        /// Creates a new table to store all unicade users
        /// </summary>
        public const string CreateUsersTable = @"
DROP TABLE IF EXISTS users;
CREATE TABLE users(
  username        varchar(255),      # 
  email           varchar(255),      # 
  userinfo        varchar(255),      # 
  userLoginCount  smallint(127),     # 
  userLaunchCount smallint(127),     # 
  alowedEsrb      varchar(255),      # 
  PRIMARY KEY(id));";
    }
}
