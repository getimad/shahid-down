namespace ShahidDown.App.Models
{
    /// <summary>
    /// Represents the type of an anime.
    /// </summary>
    public enum AnimeType
    {
        All,
        Tv,
        Movie,
        Ona,
        Ova,
        Special
    }

    /// <summary>
    /// Represents the status of an anime.
    /// </summary>
    public enum AnimeStatus
    {
        Airing,
        Completed,
    }

    /// <summary>
    /// Represents an anime.
    /// </summary>
    public class Anime
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public AnimeType? Type { get; set; }
        public AnimeStatus? Status { get; set; }
        public string? Url { get; set; }
    }
}
