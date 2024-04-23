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
        Ona
    }

    /// <summary>
    /// Represents an anime with its details.
    /// </summary>
    public class Anime
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public AnimeType Type { get; set; }
        public int Episodes { get; set; } 
    }
}
