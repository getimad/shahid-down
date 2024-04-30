using System.Text.RegularExpressions;

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
        public string UrlFriendlyTitle
        {
            get => Regex.Replace(Title, @"[^0-9a-zA-Z\s-]", "")
                        .Replace(" ", "-")
                        .ToLower();
        }
    }
}
