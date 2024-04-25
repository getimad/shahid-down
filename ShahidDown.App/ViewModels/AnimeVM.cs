using ShahidDown.App.Models;
using ShahidDown.App.ViewModels.Commands;
using ShahidDown.App.ViewModels.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShahidDown.App.ViewModels
{
    public class AnimeVM : INotifyPropertyChanged
    {
        private string? _searchQuery;

        public string? SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private ObservableCollection<Anime>? _animeList;

        public ObservableCollection<Anime>? AnimeList
        {
            get => _animeList;
            set
            {
                _animeList = value;
                OnPropertyChanged(nameof(AnimeList));
            }
        }

        public SearchCommand SearchCommand { get; set; }

        public AnimeVM()
        {
            SearchCommand = new SearchCommand(this);
        }

        public void SearchAnime()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                return;
            }

            AnimeList = new ObservableCollection<Anime>(Scraper.ScrapAnimeList(SearchQuery));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
