namespace UniCade
{
    public interface IGame
    {
        string ConsoleName { get; set; }
        string CriticReviewScore { get; set; }
        string Description { get; set; }
        string DeveloperName { get; set; }
        string EsrbRating { get; set; }
        string EsrbDescriptors { get; set; }
        string EsrbSummary { get; set; }
        int Favorite { get; set; }
        string FileName { get; set; }
        string Genres { get; set; }
        int LaunchCount { get; set; }
        string PlayerCount { get; set; }
        string PublisherName { get; set; }
        string ReleaseDate { get; set; }
        string Tags { get; set; }
        string Title { get; set; }
        string Trivia { get; set; }
        string UserReviewScore { get; set; }
    }
}