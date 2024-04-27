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
                "لم يعرض بعد" => AnimeStatusEnum.NotYetAired,
                _ => throw new InvalidOperationException($"Invalid status: {input}")
            };
        }

        internal static string CleanEpisodeData(string input)
        {
            return input.Split(' ')[0];
        }

        internal static string CleanSubTitleUrl(string input)
        {
            return input[25..^1];
        }
    }
}
