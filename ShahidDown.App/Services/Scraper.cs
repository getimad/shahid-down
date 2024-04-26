using HtmlAgilityPack;
using ShahidDown.App.Models;
using System.Windows;

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
                return [];
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
                    Type = Enum.Parse<AnimeTypeEnum>(animeType, true),
                    Status = animeStatus == "مكتمل" ? AnimeStatusEnum.Completed : AnimeStatusEnum.Airing,
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
        public static async Task<AnimeDetails> ScrapAnimeDetailsAsync(Anime anime)
        {
            HtmlWeb web = new();
            HtmlDocument doc = await web.LoadFromWebAsync(anime.Url);

            // Get the total episodes of the anime.
            string TotalEpisodesTarget = doc
                .DocumentNode
                .SelectSingleNode("//div[@class='anime-details']/div[@class='row']/div[4]/div[@class='anime-info']").InnerText;
            string animeTotalEpisodes = Helper.GetOnlyDigitsFromString(TotalEpisodesTarget);

            // Get the last episode of the anime.
            string LastEpisodeTarget = doc
                .DocumentNode
                .SelectSingleNode("//div[@id='DivEpisodesList']/div[last()]//h3/a")
                .InnerText;
            string animeLastEpisode = Helper.GetOnlyDigitsFromString(LastEpisodeTarget);

            AnimeDetails fullAnimeInfo = new()
            {
                Id = anime.Id,
                Title = anime.Title,
                Type = anime.Type,
                Status = anime.Status,
                LastEpisode = animeLastEpisode,
                TotalEpisodes = animeTotalEpisodes == string.Empty ? "N/A" : animeTotalEpisodes,
                Url = anime.Url
            };

            return fullAnimeInfo;
        }
    }
}
