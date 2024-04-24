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
        Special
    }

    /// <summary>
    /// Represents the status of an anime.
    /// </summary>
    public enum AnimeStatus
    {
        Airing,
        Completed
    }

    /// <summary>
    /// Represents an anime.
    /// </summary>
    public class Anime
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required AnimeType Type { get; set; }
        public required AnimeStatus Status { get; set; }
    }
}
