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
        /// Provudes the structure for the UniCade Cloud SQL database
        /// </summary>
        public const string SqlStructure = @"USE unicade;

DROP TABLE IF EXISTS games;
            CREATE TABLE games
            (
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
    }
}
