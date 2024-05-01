namespace ShahidDown.App.Services
{
    internal static class Server
    {
        private static readonly string _baseUrl = "https://anime4up.cam";

        internal static string GetSearchUrl(string query)
        {
            return $"{_baseUrl}/?search_param=animes&s={query}";
        }

        internal static string GetAnimeUrl(string animeSubTitleUrl)
        {
            return $"{_baseUrl}/anime/{animeSubTitleUrl}/";
        }

        internal static string GetEpisodeUrl(string animeSubTitleUrl, int episode)
        {
            return $"{_baseUrl}/episode/{animeSubTitleUrl}-%d8%a7%d9%84%d8%ad%d9%84%d9%82%d8%a9-{episode}/";
        }
    }
}
