using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.IO;

namespace ShahidDown.App.Services
{
    /// <summary>
    /// This class is the base class for all downloaders.
    /// </summary>
    public abstract class BaseDownloader
    {
        protected IWebDriver _driver;
        protected string _path;

        public BaseDownloader(string directoryName, string? downloadpath = null)
        {
            _path = Path.Combine(downloadpath ?? Directory.GetCurrentDirectory(), @$"Anime\{directoryName}");
            Directory.CreateDirectory(_path);

            EdgeOptions options = new EdgeOptions();
            options.AddExtension(Path.Combine(Directory.GetCurrentDirectory(), "Extensions", "uBlock_v1_57_2.crx"));
            options.AddUserProfilePreference("download.default_directory", _path);

            _driver = new EdgeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public abstract void Start(string url);

        public abstract bool CanStart(string url);

        public abstract void Stop();

        protected abstract void WaitForFileDownload(string title);

        protected abstract bool IsFileExist(string title);
    }
}
