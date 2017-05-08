namespace UniCade
{
    public interface IGame
    {
        string ConsoleName { get; set; }
        string CriticScore { get; set; }
        string Description { get; set; }
        string Developer { get; set; }
        string Esrb { get; set; }
        string EsrbDescriptor { get; set; }
        string EsrbSummary { get; set; }
        int Favorite { get; set; }
        string FileName { get; set; }
        string Genres { get; set; }
        int LaunchCount { get; set; }
        string Players { get; set; }
        string Publisher { get; set; }
        string ReleaseDate { get; set; }
        string Tags { get; set; }
        string Title { get; set; }
        string Trivia { get; set; }
        string UserScore { get; set; }
    }
}