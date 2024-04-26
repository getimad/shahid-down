using ShahidDown.App.Models;
using ShahidDown.App.Services;
using ShahidDown.App.ViewModels.Commands;
using ShahidDown.App.ViewModels.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ShahidDown.App.ViewModels
{
    public class AnimeSearchVM : INotifyPropertyChanged
    {
        private string? _searchQuery;

        public string? SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                onPropertyChanged(nameof(SearchQuery));
            }
        }

        public ICommand? SearchCommand { get; }

        public AnimeSearchVM()
        {
            SearchCommand = new RelayCommand(param => OnSearchCommandExecuted(), param => CanExecuteSearchCommand());
        }

        private async void OnSearchCommandExecuted()
        {
            List<Anime> animeList = await Scraper.ScrapAnimeListAsync(SearchQuery!);

            if (animeList.Count == 0)
            {
                MessageBox.Show("No anime found!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            ObservableCollection<Anime> preparedAnimeList = new ObservableCollection<Anime>(animeList);

            Messenger.Instance.Send(nameof(OnSearchCommandExecuted), preparedAnimeList);
        }

        private bool CanExecuteSearchCommand()
        {
            return !string.IsNullOrWhiteSpace(SearchQuery);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
