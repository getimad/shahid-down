namespace ShahidDown.App.Services
{
    internal static class Helper
    {
        public static string GetOnlyDigitsFromString(string input)
        {
            return string.Concat(input.Where(char.IsDigit));
        }
    }
}
