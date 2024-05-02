using ShahidDown.App.Models;
using ShahidDown.App.Services;
using ShahidDown.App.ViewModels.Helpers;
using System.ComponentModel;
using System.Xml.Linq;

namespace ShahidDown.App.ViewModels
{
    public class AnimeDetailsVM : INotifyPropertyChanged
    {
        private string? _title;
        private AnimeTypeEnum? _type;
        private AnimeStatusEnum? _status;
        private string? _episodes;
        private string? _myAnimeListUrl;

        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public AnimeTypeEnum? Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public AnimeStatusEnum? Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string? Episodes
        {
            get => _episodes;
            set
            {
                _episodes = value;
                OnPropertyChanged(nameof(Episodes));
            }
        }

        public string? MyAnimeListUrl
        {
            get => _myAnimeListUrl;
            set
            {
                _myAnimeListUrl = value;
                OnPropertyChanged(nameof(MyAnimeListUrl));
            }
        }

        public AnimeDetailsVM()
        {
            Messenger.Instance.Register(nameof(OnItemSelectedCommandExecuted), OnItemSelectedCommandExecuted);
        }

        private async void OnItemSelectedCommandExecuted(object data)
        {
            Anime? selectedAnime = data as Anime;

            AnimeDetails animeDetails = await Scraper.ScrapAnimeDetailsAsync(selectedAnime!);

            Title = animeDetails.Title;
            Type = animeDetails.Type;
            Status = animeDetails.Status;
            Episodes = animeDetails.Episodes;
            MyAnimeListUrl = animeDetails.MyAnimeListUrl;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
