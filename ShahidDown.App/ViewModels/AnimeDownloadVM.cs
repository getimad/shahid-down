using HtmlAgilityPack;
using Microsoft.Win32;
using ShahidDown.App.Models;
using ShahidDown.App.Services;
using ShahidDown.App.ViewModels.Commands;
using ShahidDown.App.ViewModels.Helpers;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ShahidDown.App.ViewModels
{
    public class AnimeDownloadVM : INotifyPropertyChanged
    {
        private Anime? _selectedAnime;
        private string? _downlaodPath;
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
                if (value is not null)
                    _downloadQuery = value.Trim();
                else
                    _downloadQuery = value;

                OnPropertyChanged(nameof(DownloadQuery));
            }
        }

        public string? DownloadPath
        {
            get => _downlaodPath;
            set
            {
                _downlaodPath = value;

                OnPropertyChanged(nameof(DownloadPath));
            }
        }

        public ICommand? DownloadCommand { get; }
        public ICommand? SetDownloadPathCommand { get; }

        public AnimeDownloadVM()
        {
            IsAllOptionSelected = true;
            IsQueryOptionSelected = false;

            DownloadCommand = new RelayCommand(param => OnDownloadCommandExecuted(), param => CanExecuteDownloadCommand());
            SetDownloadPathCommand = new RelayCommand(param => SetDownloadPathCommandExecuted(), param => CanExecuteSetDownloadPathCommand());

            Messenger.Instance.Register(nameof(OnOpenAnimeItemWindowCommandExecuted), OnOpenAnimeItemWindowCommandExecuted);
        }

        private void SetDownloadPathCommandExecuted()
        {
            OpenFolderDialog openFolderDialog = new()
            {
                Title = "Select Download Path",
            };

            if (openFolderDialog.ShowDialog() == true)
            {
                DownloadPath = openFolderDialog.FolderName;
            }
        }

        private bool CanExecuteSetDownloadPathCommand()
        {
            return true;
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
            return _selectedAnime != null && (IsAllOptionSelected == true || (IsQueryOptionSelected == true && !string.IsNullOrWhiteSpace(DownloadQuery)));
        }

        private async void OnDownloadAll()
        {
            WebController webController = new(_selectedAnime!.UrlFriendlyTitle, DownloadPath);

            int episode = 1;

            await Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await DownloadProcess(episode, webController);
                    }
                    catch (NodeNotFoundException)
                    {
                        MessageBox.Show($"The episode N°{episode} is not found. Or the download process reached to the end.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }

                    episode++;
                }
            });

            webController.Dispose();
        }

        private async void OnDownloadQuery()
        {
            Match match = Regex.Match(_downloadQuery!, @"(?<start>\d+)-(?<end>\d+)|(?<single>\d+)");

            if (match.Groups["start"].Success && match.Groups["end"].Success)
            {
                int start = int.Parse(match.Groups["start"].Value);
                int end = int.Parse(match.Groups["end"].Value);

                await OnDownloadQueryRange(start, end);
            }

            else if (match.Groups["single"].Success)
            {
                int single = int.Parse(match.Groups["single"].Value);

                await OnDownloadQuerySingle(single);
            }
            
            else
            {
                MessageBox.Show("Invalid query format. Please enter something like '4-11' or '5'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task OnDownloadQuerySingle(int episode)
        {
            WebController webController = new(_selectedAnime!.UrlFriendlyTitle, DownloadPath);

            await Task.Run(async () =>
            {
                try
                {
                    await DownloadProcess(episode, webController);
                }
                catch (NodeNotFoundException)
                {
                    MessageBox.Show($"Episode N°{episode} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            webController.Dispose();
        }

        private async Task OnDownloadQueryRange(int start, int end)
        {
            WebController webController = new(_selectedAnime!.UrlFriendlyTitle, DownloadPath);

            await Task.Run(async () =>
            {
                for (int episode = start; episode <= end; episode++)
                {
                    try
                    {
                        await DownloadProcess(episode, webController);
                    }
                    catch (NodeNotFoundException)
                    {
                        MessageBox.Show($"Episode N°{episode} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

            webController.Dispose();
        }

        private async Task DownloadProcess(int episode, WebController webController)
        {
            List<DownloadLink> links = await Scraper.ScrapDownloadUrlsAsync(_selectedAnime!, episode);

            if (links.Count == 0)
            {
                MessageBox.Show($"Sorry, There is no download server for episode N°{episode}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            foreach (DownloadLink link in links)
            {
                IBaseDownloader downloader = CreateDownloader(link.Type, webController);

                if (downloader.CanStart(link.Url))
                {
                    downloader.Start(link.Url);
                    break;
                }
            }
        }

        private void OnOpenAnimeItemWindowCommandExecuted(object data)
        {
            _selectedAnime = data as Anime;
        }

        private IBaseDownloader CreateDownloader(DownloadLinkTypeEnum type, WebController webController)
        {
            return type switch
            {
                DownloadLinkTypeEnum.Special => new SpecialDownloader(webController),
                DownloadLinkTypeEnum.MP4Upload => new Mp4UploadDownloader(webController),
                DownloadLinkTypeEnum.XFileSharing => new XFileSharingDownloader(webController),
                _ => throw new NotImplementedException()
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
