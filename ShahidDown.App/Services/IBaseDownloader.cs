using OpenQA.Selenium;

namespace ShahidDown.App.Services
{
    /// <summary>
    /// This interface is the base downloader.
    /// </summary>
    public interface IBaseDownloader
    {
        public string DownloadPath { get; }

        public IWebDriver DownloadDriver { get; }

        public void Start(string url);

        public bool CanStart(string url);

        public void Stop();

        public void WaitForFileDownload(string title);

        public bool IsFileExist(string title);
    }
}
