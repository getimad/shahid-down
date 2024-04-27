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
            List<Anime> animeList = [];

            int count = 0;

            string searchUrl = Server.GetSearchUrl(query);

            HtmlWeb web = new();
            HtmlDocument doc = await web.LoadFromWebAsync(searchUrl);
            HtmlNodeCollection animesNodes = doc
                .DocumentNode
                .SelectNodes("//div[@class='search-card']");

            if (animesNodes is null)
            {
                return animeList;
            }

            foreach (HtmlNode animeNode in animesNodes)
            {
                string title = GetInnerTextOrDefault(animeNode, ".//a/h3");
                string type = GetInnerTextOrDefault(animeNode, ".//span[@class='anime-type']");
                string status = GetInnerTextOrDefault(animeNode, ".//div[@class='anime-status']/a");
                string episodes = GetInnerTextOrDefault(animeNode, ".//span[@class='anime-aired']", "N/A");
                string url = GetAttributeValueOrDefault(animeNode, ".//a", "href");

                Anime anime = new Anime()
                {
                    Id = count++,
                    Title = title,
                    Type = Cleaner.CleanTypeData(type),
                    Status = Cleaner.CleanStatusData(status),
                    Episodes = Cleaner.CleanEpisodeData(episodes),
                    SubTitleUrl = Cleaner.CleanSubTitleUrl(url)
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

            string animeDetailsUrl = Server.GetAnimeUrl(anime.SubTitleUrl);

            HtmlDocument animeDetailsDoc = await web.LoadFromWebAsync(animeDetailsUrl);

            string rating = GetInnerTextOrDefault(animeDetailsDoc.DocumentNode, "//span[@class='score']");
            string myAnimeListUrl = GetAttributeValueOrDefault(animeDetailsDoc.DocumentNode, "//div[@class='external-links']/a[2]", "href");

            AnimeDetails animeDetails = new AnimeDetails()
            {
                Id = anime.Id,
                Title = anime.Title,
                Type = anime.Type,
                Status = anime.Status,
                Episodes = anime.Episodes,
                SubTitleUrl = anime.SubTitleUrl,
                Score = rating,
                MyAnimeListUrl = myAnimeListUrl
            };

            return animeDetails;
        }

        private static string GetInnerTextOrDefault(HtmlNode node, string xPath, string defaultValue = "Unknown")
        {
            return node?.SelectSingleNode(xPath)?.InnerText ?? defaultValue;
        }

        private static string GetAttributeValueOrDefault(HtmlNode node, string xPath, string attribute, string defaultValue = "Unknown")
        {
            return node?.SelectSingleNode(xPath)?.GetAttributeValue(attribute, defaultValue) ?? defaultValue;
        }
    }
}
