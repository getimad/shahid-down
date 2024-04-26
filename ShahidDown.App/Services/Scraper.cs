using HtmlAgilityPack;
using ShahidDown.App.Models;

namespace ShahidDown.App.Services
{
    public static class Scraper
    {
        private static readonly string _baseUrl = "https://anime4up.cam";

        /// <summary>
        /// Returns a list of anime based on the search query.
        /// </summary>
        /// <param name="query">Represents a search query.</param>
        public static async Task<List<Anime>> ScrapAnimeListAsync(string query)
        {
            List<Anime> animeList = [];

            int count = 0;

            string url = $"{_baseUrl}/?search_param=animes&s={query}";

            HtmlWeb web = new();
            HtmlDocument doc = await web.LoadFromWebAsync(url);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='anime-card-container']");

            if (nodes == null)
            {
                Anime anime = new()
                {
                    Title = "No results found.",
                };

                return [anime];
            }

            foreach (HtmlNode node in nodes)
            {
                string animeTitle = node.SelectSingleNode(
                    ".//div[@class='anime-card-details']/div[@class='anime-card-title']/h3/a"
                    ).InnerText;
                string animeType = node.SelectSingleNode(
                    ".//div[@class='anime-card-details']/div[@class='anime-card-type']/a"
                    ).InnerText;
                string animeStatus = node.SelectSingleNode(
                    ".//div[@class='anime-card-poster']/div[@class='anime-card-status']/a"
                    ).InnerText;
                string animeUrl = node.SelectSingleNode(
                    ".//div[@class='anime-card-details']/div[@class='anime-card-title']/h3/a"
                    ).GetAttributeValue("href", string.Empty);

                Anime anime = new()
                {
                    Id = count++,
                    Title = animeTitle,
                    Type = Enum.Parse<AnimeType>(animeType, true),
                    Status = animeStatus == "مكتمل" ? AnimeStatus.Completed : AnimeStatus.Airing,
                    Url = animeUrl,
                };

                animeList.Add(anime);
            }

            return animeList;
        }

        /// <summary>
        /// Returns full information of an anime.
        /// </summary>
        /// <param name="anime">Represents Anime object</param>
        /// <returns></returns>
        public static async Task<Anime> ScrapAnimeDetailsAsync(Anime anime)
        {
            HtmlWeb web = new();
            HtmlDocument doc = await web.LoadFromWebAsync(anime.Url);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class='anime-details']");

            string animeEpisodes = node
                .SelectSingleNode(".//div[@class='row']/div[4]/div[@class='anime-info']")
                .InnerText
                .Trim()
                .Split("\n")[1];

            if (animeEpisodes == "غير معروف")
            {
                animeEpisodes = "N/A";
            }

            Anime fullAnimeInfo = new()
            {
                Id = anime.Id,
                Title = anime.Title,
                Type = anime.Type,
                Status = anime.Status,
                Episodes = animeEpisodes, // Add episodes to anime object
                Url = anime.Url
            };

            return fullAnimeInfo;
        }
    }
}
