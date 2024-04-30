using ShahidDown.App.Models;

namespace ShahidDown.App.Services
{
    internal static class Cleaner
    {
        internal static AnimeTypeEnum CleanTypeData(string input)
        {
            return input.ToLower() switch
            {
                "tv" => AnimeTypeEnum.Tv,
                "movie" => AnimeTypeEnum.Movie,
                "ova" => AnimeTypeEnum.Ova,
                "ona" => AnimeTypeEnum.Ona,
                "special" => AnimeTypeEnum.Special,
                _ => throw new InvalidOperationException($"Invalid type: {input}")
            };
        }

        internal static AnimeStatusEnum CleanStatusData(string input)
        {
            return input switch
            {
                "يعرض الان" => AnimeStatusEnum.Airing,
                "مكتمل" => AnimeStatusEnum.Completed,
                _ => throw new InvalidOperationException($"Invalid status: {input}")
            };
        }

        internal static string CleanEpisodeData(string input)
        {
            string data = input.Split("\n")[2].Trim();

            if (data == "غير معروف")
            {
                return "N/A";
            }

            return data;
        }
    }
}
