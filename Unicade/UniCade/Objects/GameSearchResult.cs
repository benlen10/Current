using System;

namespace UniCade.Objects {
    /// <summary>
    /// Represents a search result when searching for games.
    /// </summary>
    public class GameSearchResult {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// Title of the game.
        /// </summary>
        public String Title;

        /// <summary>
        /// Date on which the game was released.
        /// </summary>
        public String ReleaseDate;

        /// <summary>
        /// Which platform the game is for.
        /// </summary>
        public String Platform;
    }
}
