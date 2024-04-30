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

        private static string GetInnerTextOrDefault(HtmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath)?.InnerText ?? throw new NodeNotFoundException($"Node not found: {nameof(node)}");
        }
    }
}
