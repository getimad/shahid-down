using ShahidDown.App.Models;
using ShahidDown.App.Services;
using ShahidDown.App.ViewModels.Commands;
using ShahidDown.App.ViewModels.Helpers;
using System.ComponentModel;
using System.Windows.Input;

namespace ShahidDown.App.ViewModels
{
    public class AnimeDownloadVM : INotifyPropertyChanged
    {
        private Anime? _selectedAnime;
        private bool? _isAllOptionSelected;
        private bool? _isQueryOptionSelected;
        private string? _downloadQuery;

        public bool? IsAllOptionSelected
        {
            get => _isAllOptionSelected;
            set
            {
                if (_isAllOptionSelected != value)
                {
                    _isAllOptionSelected = value;
                    OnPropertyChanged(nameof(IsAllOptionSelected));
                }
            }
        }

        public bool? IsQueryOptionSelected
        {
            get => _isQueryOptionSelected;
            set
            {
                if (_isQueryOptionSelected != value)
                {
                    _isQueryOptionSelected = value;
                    OnPropertyChanged(nameof(IsQueryOptionSelected));
                }
            }
        }

        public string? DownloadQuery
        {
            get => _downloadQuery;
            set
            {
                _downloadQuery = value;
                OnPropertyChanged(nameof(DownloadQuery));
            }
        }

        public ICommand? DownloadCommand { get; }

        public AnimeDownloadVM()
        {
            IsAllOptionSelected = true;
            IsQueryOptionSelected = false;

            DownloadCommand = new RelayCommand(param => OnDownloadCommandExecuted(), param => CanExecuteDownloadCommand());

            Messenger.Instance.Register(nameof(OnItemSelectedCommandExecuted), OnItemSelectedCommandExecuted);
        }

        private void OnDownloadCommandExecuted()
        {
            if (IsAllOptionSelected == true)
                OnDownloadAll();

            else if (IsQueryOptionSelected == true)
                OnDownloadQuery();
        }

        private bool CanExecuteDownloadCommand()
        {
            return _selectedAnime != null;
        }

        private async void OnDownloadAll()
        {
            MP4UploadDownloader downloader = new MP4UploadDownloader();

            int episode = 1;

            await Task.Run(async () =>
            {
                while (true)
                {
                    string downloadUrl = await Scraper.ScrapDownloadUrlAsync(_selectedAnime!, episode);

                    downloader.Start(downloadUrl);

                    episode++;
                }
            });
        }

        private void OnDownloadQuery()
        {

        }

        private void OnItemSelectedCommandExecuted(object data)
        {
            _selectedAnime = data as Anime;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
