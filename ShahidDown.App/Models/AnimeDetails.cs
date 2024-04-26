namespace ShahidDown.App.Models
{
    public class AnimeDetails : Anime
    {
        public required string LastEpisode { get; set; }
        public required string TotalEpisodes { get; set; }
    }
}
