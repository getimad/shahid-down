namespace ShahidDown.App.Models
{
    public class DownloadLink
    {
        public required DownloadLinkTypeEnum Type { get; set; }
        public required string Url { get; set; }
    }
}
