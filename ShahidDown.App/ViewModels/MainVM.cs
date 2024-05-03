namespace ShahidDown.App.ViewModels
{
    /// <summary>
    /// Represents the main view model. This class is the entry point for the view model.
    /// </summary>
    public class MainVM
    {
        public AnimeListVM AnimeListVM { get; }
        public AnimeSearchVM AnimeSearchVM { get; }
        public AnimeDetailsVM AnimeDetailsVM { get; }
        public AnimeDownloadVM AnimeDownloadVM { get; }
        public AboutVM AboutVM { get; }

        public MainVM()
        {
            AnimeDetailsVM = new AnimeDetailsVM();
            AnimeListVM = new AnimeListVM();
            AnimeSearchVM = new AnimeSearchVM();
            AnimeDownloadVM = new AnimeDownloadVM();
            AboutVM = new AboutVM();
        }
    }
}
