using ShahidDown.App.Models;
using ShahidDown.App.Services;
using ShahidDown.App.ViewModels.Commands;
using ShahidDown.App.ViewModels.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ShahidDown.App.ViewModels
{
    public class AnimeListVM : INotifyPropertyChanged
    {
        private ObservableCollection<Anime>? _animeList;
        private Anime? _selectedAnime;

        public ObservableCollection<Anime>? AnimeList
        {
            get => _animeList;
            set
            {
                _animeList = value;
                OnPropertyChanged(nameof(AnimeList));
            }
        }

        public Anime? SelectedAnime
        {
            get => _selectedAnime;
            set
            {
                _selectedAnime = value;
                OnPropertyChanged(nameof(SelectedAnime));
            }
        }

        public ICommand SelectedCommand { get; }

        public AnimeListVM()
        {
            Messenger.Instance.Register(nameof(OnSearchCommandExecuted), OnSearchCommandExecuted);

            SelectedCommand = new RelayCommand(param => OnItemSelectedCommandExecuted(), param => CanItemSelectedCommandExecute());
        }

        private void OnSearchCommandExecuted(object data)
        {
            AnimeList = data as ObservableCollection<Anime>;
        }

        private void OnItemSelectedCommandExecuted()
        {
            Messenger.Instance.Send(nameof(OnItemSelectedCommandExecuted), SelectedAnime!);
        }

        private bool CanItemSelectedCommandExecute()
        {
            return SelectedAnime != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
