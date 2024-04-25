using HtmlAgilityPack;
using ShahidDown.App.Models;
using System.Collections.ObjectModel;

namespace ShahidDown.App.ViewModels.Helpers
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
    }
}
