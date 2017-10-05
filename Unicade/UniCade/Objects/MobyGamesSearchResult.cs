using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace UniCade.Objects
{
    public class MobyGenre
    {
        public string genre_category { get; set; }
        public int genre_category_id { get; set; }
        public int genre_id { get; set; }
        public string genre_name { get; set; }
    }

    public class MobyPlatform
    {
        public string first_release_date { get; set; }
        public int platform_id { get; set; }
        public string platform_name { get; set; }
    }

    public class MobySampleCover
    {
        public int height { get; set; }
        public string image { get; set; }
        public List<string> platforms { get; set; }
        public string thumbnail_image { get; set; }
        public int width { get; set; }
    }

    public class MobyGameResult
    {
        public string description { get; set; }
        public int game_id { get; set; }
        public List<MobyGenre> genres { get; set; }
        public double moby_score { get; set; }
        public string moby_url { get; set; }
        public int num_votes { get; set; }
        public object official_url { get; set; }
        public List<MobyPlatform> platforms { get; set; }
        public MobySampleCover MobySampleCover { get; set; }
        public List<object> sample_screenshots { get; set; }
        public string title { get; set; }
    }

    public class MobyRootObject
    {
        public List<MobyGameResult> games { get; set; }
    }
}
