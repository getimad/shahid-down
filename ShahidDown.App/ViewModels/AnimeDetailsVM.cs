using ShahidDown.App.Models;
using ShahidDown.App.ViewModels.Helpers;
using System.ComponentModel;

namespace ShahidDown.App.ViewModels
{
    public class AnimeDetailsVM : INotifyPropertyChanged
    {
        private string? _title;
        private AnimeTypeEnum? _type;
        private AnimeStatusEnum? _status;
        private string? _episodes;
        private string? _score;
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

        public string? Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
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

        private void OnItemSelectedCommandExecuted(object data)
        {
            AnimeDetails? animeDetails = data as AnimeDetails;

            Title = animeDetails?.Title;
            Type = animeDetails?.Type;
            Status = animeDetails?.Status;
            Episodes = animeDetails?.Episodes;
            Score = animeDetails?.Score;
            MyAnimeListUrl = animeDetails?.MyAnimeListUrl;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
