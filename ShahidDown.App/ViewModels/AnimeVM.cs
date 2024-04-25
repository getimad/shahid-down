using ShahidDown.App.Models;
using ShahidDown.App.ViewModels.Commands;
using ShahidDown.App.ViewModels.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

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

        private ObservableCollection<Anime> _animeList = [];

        public ObservableCollection<Anime> AnimeList
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

        public async void SearchAnime()
        {
            // SearchQuery is not null here. If it was null, the Search Button would have been disabled.

            SearchQuery = SearchQuery!.Trim();

            List<Anime> animeList = await Scraper.ScrapAnimeListAsync(SearchQuery!);

            AnimeList = [.. animeList];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
