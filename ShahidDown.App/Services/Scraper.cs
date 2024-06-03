using HtmlAgilityPack;
using ShahidDown.App.Models;

namespace ShahidDown.App.Services
{
    public static class Scraper
    {
        /// <summary>
        /// Returns a list of anime based on the search query.
        /// </summary>
        /// <param name="query">Represents a search query.</param>
        public static async Task<List<Anime>> ScrapAnimeListAsync(string query)
        {
            string searchUrl = Server.GetSearchUrl(query);

            HtmlWeb web = new();
            HtmlDocument doc = await web.LoadFromWebAsync(searchUrl);

            IEnumerable<HtmlNode> animesNodes = doc
                .DocumentNode
                .SelectSingleNode("//div[@class='anime-list-content']/div")
                .ChildNodes
                .Elements("div");

            List<Anime> animeList = [];

            int count = 0;

            foreach (HtmlNode animeNode in animesNodes)
            {
                string title = GetInnerTextOrDefault(animeNode, ".//div[@class='anime-card-title']/h3/a");
                string type = GetInnerTextOrDefault(animeNode, ".//div[@class='anime-card-type']/a");
                string status = GetInnerTextOrDefault(animeNode, ".//div[@class='anime-card-status']/a");

                Anime anime = new Anime()
                {
                    Id = ++count,
                    Title = title,
                    Type = Cleaner.CleanTypeData(type),
                    Status = Cleaner.CleanStatusData(status),
                };

                animeList.Add(anime);
            }

            return animeList;
        }

        /// <summary>
        /// Returns more details about specific anime.
        /// </summary>
        /// <param name="anime">Represents Anime object</param>
        /// <returns></returns>
        public static async Task<AnimeDetails> ScrapAnimeDetailsAsync(Anime anime)
        {
            HtmlWeb web = new();

            string url = Server.GetAnimeUrl(anime.UrlFriendlyTitle);

            HtmlDocument doc = await web.LoadFromWebAsync(url);

            string episodes = GetInnerTextOrDefault(doc.DocumentNode, "(//div[@class='anime-info'])[4]");

            string myAnimeListUrl = doc
                .DocumentNode
                .SelectSingleNode("//a[@class='anime-mal']")
                .GetAttributeValue("href", "");

            AnimeDetails animeDetails = new AnimeDetails()
            {
                Id = anime.Id,
                Title = anime.Title,
                Type = anime.Type,
                Status = anime.Status,
                Episodes = Cleaner.CleanEpisodeData(episodes),
                MyAnimeListUrl = myAnimeListUrl
            };

            return animeDetails;
        }


        /// <summary>
        /// Returns downloads link for specific episode of an anime.
        /// </summary>
        /// <param name="anime"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        public static async Task<List<DownloadLink>> ScrapDownloadUrlsAsync(Anime anime, int episode)
        {
            HtmlWeb web = new();

            string url = Server.GetEpisodeUrl(anime.UrlFriendlyTitle, episode);

            HtmlDocument doc = await web.LoadFromWebAsync(url);

            if (IsPageNotFound(doc))
                throw new NodeNotFoundException("Page not found.");

            List<DownloadLink> downloadLinks = GetDownloadLinks(doc);

            DownloadLink? specialDownloadLink = GetSpecialDownloadLink(doc);

            if (specialDownloadLink is not null)
                downloadLinks.Add(specialDownloadLink);

            return downloadLinks;
        }

        private static bool IsPageNotFound(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode($"//div[@class='page-404']") is not null;
        }

        private static DownloadLink? GetSpecialDownloadLink(HtmlDocument doc)
        {
            string? specialDownloadLink = doc
                .DocumentNode
                .SelectSingleNode($"(//div[@class='dw-online']/a)[2]")?
                .GetAttributeValue("href", null);

            if (specialDownloadLink is null)
            {
                return null;
            }

            return new DownloadLink()
            {
                Type = DownloadLinkTypeEnum.Special,
                Url = specialDownloadLink
            };
        }

        private static List<DownloadLink> GetDownloadLinks(HtmlDocument doc)
        {
            IEnumerable<HtmlNode> downloadNodes = doc
                .DocumentNode
                .SelectSingleNode("(//ul[@class='quality-list'])[last()]")
                .ChildNodes
                .Elements("a");

            List<DownloadLink> downloadLinks = [];

            foreach (HtmlNode downloadNode in downloadNodes)
            {
                string downloadType = downloadNode.InnerText;
                string downloadLink = downloadNode.GetAttributeValue("href", "");

                if (Enum.TryParse(downloadType, true, out DownloadLinkTypeEnum type))
                {
                    DownloadLink link = new DownloadLink()
                    {
                        Type = type,
                        Url = downloadLink
                    };

                    downloadLinks.Add(link);
                }

                if (downloadType == "updown" || downloadType == "uupbom" || downloadType == "upbaam")
                {
                    DownloadLink link = new DownloadLink()
                    {
                        Type = DownloadLinkTypeEnum.XFileSharing,
                        Url = downloadLink
                    };

                    downloadLinks.Add(link);
                }
            }

            return downloadLinks;
        }

        private static string GetInnerTextOrDefault(HtmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath)?.InnerText ?? throw new NodeNotFoundException($"Node not found: {nameof(node)}");
        }
    }
}
