using ShahidDown.App.Models;
using ShahidDown.App.ViewModels.Helpers;
using System.ComponentModel;

namespace ShahidDown.App.ViewModels
{
    public class AnimeDetailsVM : INotifyPropertyChanged
    {
        private string? _title;
        private AnimeType? _type;
        private AnimeStatus? _status;
        private string? _episodes;

        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public AnimeType? Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public AnimeStatus? Status
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

        public AnimeDetailsVM()
        {
            Messenger.Instance.Register(nameof(OnItemSelectedCommandExecuted), OnItemSelectedCommandExecuted);
        }

        private void OnItemSelectedCommandExecuted(object data)
        {
            Anime? anime = data as Anime;

            Title = anime?.Title;
            Type = anime?.Type;
            Status = anime?.Status;
            Episodes = anime?.Episodes;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
