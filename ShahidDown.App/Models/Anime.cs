namespace ShahidDown.App.Models
{
    /// <summary>
    /// Represents an anime.
    /// </summary>
    public class Anime
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required AnimeTypeEnum Type { get; set; }
        public required AnimeStatusEnum Status { get; set; }
        public required string Url { get; set; }
    }
}
