using System.Windows.Input;

namespace ShahidDown.App.ViewModels.Commands
{
    public class SearchCommand : ICommand
    {
        public AnimeVM AnimeVM { get; set; }

        public event EventHandler? CanExecuteChanged;

        public SearchCommand(AnimeVM animeVM)
        {
            AnimeVM = animeVM;
        }

        public bool CanExecute(object? parameter)
        {
            return true; // Always return true
        }

        public void Execute(object? parameter)
        {
            AnimeVM.SearchAnime();
        }
    }
}
