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
        private string? _totalepisodes;
        private string? _lastEpisode;

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

        public string? LastEpisode
        {
            get => _lastEpisode;
            set
            {
                _lastEpisode = value;
                OnPropertyChanged(nameof(LastEpisode));
            }
        }

        public string? TotalEpisodes
        {
            get => _totalepisodes;
            set
            {
                _totalepisodes = value;
                OnPropertyChanged(nameof(TotalEpisodes));
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
            LastEpisode = animeDetails?.LastEpisode;
            TotalEpisodes = animeDetails?.TotalEpisodes;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
