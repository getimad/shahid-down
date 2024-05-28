namespace ShahidDown.App.ViewModels
{
    /// <summary>
    /// Represents the AnimeItem view model. This class is the entry point for the view model.
    /// </summary>
    public class AnimeItemVM
    {
        public AnimeDetailsVM AnimeDetailsVM { get; }
        public AnimeDownloadVM AnimeDownloadVM { get; }

        public AnimeItemVM()
        {
            AnimeDetailsVM = new AnimeDetailsVM();
            AnimeDownloadVM = new AnimeDownloadVM();
        }
    }
}
