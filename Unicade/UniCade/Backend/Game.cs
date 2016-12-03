namespace UniCade
{
    public class Game
    {
        #region Constructors

        public Game(string fileName, string con, int LaunchCount)
        {
            FileName = fileName;
            Console = con;
            Title = fileName.Substring(0, fileName.IndexOf('.'));
        }

        public Game(string fileName, string con, int launchCount, string releaseDate, string publisher, string developer, string userScore, string criticScore, string players, string trivia, string esrb, string esrbDescriptor, string esrbSummary, string description, string genres, string tags, int fav)
        {
            FileName = fileName;
            Console = con;
            Favorite = fav;
            LaunchCount = launchCount;
            ReleaseDate = releaseDate;
            Publisher = publisher;
            Developer = developer;
            UserScore = userScore;
            CriticScore = criticScore;
            Players = players;
            Trivia = trivia;
            Esrb = esrb;
            Description = description;
            EsrbDescriptor = esrbDescriptor;
            EsrbSummary = esrbSummary;
            Genres = genres;
            Tags = tags;

            //Parse title from filename
            if (fileName.Length > 2)
                Title = fileName.Substring(0, fileName.IndexOf('.'));
        }

        #endregion

        #region Properties

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Console { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Genres { get; set; }
        public string Tags { get; set; }
        public string UserScore { get; set; }
        public string CriticScore { get; set; }
        public string Trivia { get; set; }
        public string Players { get; set; }
        public string Esrb { get; set; }
        public string EsrbDescriptor { get; set; }
        public string EsrbSummary { get; set; }
        public int Favorite { get; set; }
        public int LaunchCount { get; set; }

        #endregion
    }
}
