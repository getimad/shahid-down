using System.Windows.Input;

namespace ShahidDown.App.ViewModels.Commands
{
    public class SearchCommand : ICommand
    {
        public AnimeVM AnimeVM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public SearchCommand(AnimeVM animeVM)
        {
            AnimeVM = animeVM;
        }

        public bool CanExecute(object? parameter)
        {
            string? query = parameter as string;
            return !string.IsNullOrWhiteSpace(query);
        }

        public void Execute(object? parameter)
        {
            AnimeVM.SearchAnime();
        }
    }
}
