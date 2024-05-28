namespace ShahidDown.App.ViewModels
{
    /// <summary>
    /// Represents the main view model. This class is the entry point for the view model.
    /// </summary>
    public class MainVM
    {
        public AnimeListVM AnimeListVM { get; }
        public AnimeSearchVM AnimeSearchVM { get; }
        public AboutVM AboutVM { get; }

        public MainVM()
        {
            AnimeListVM = new AnimeListVM();
            AnimeSearchVM = new AnimeSearchVM();
            AboutVM = new AboutVM();
        }
    }
}
