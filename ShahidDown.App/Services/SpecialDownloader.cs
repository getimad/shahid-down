using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.IO;

namespace ShahidDown.App.Services
{
    public class SpecialDownloader
    {
        private IWebDriver _driver;
        private string _downloadpath;

        public SpecialDownloader(string directory)
        {
            _downloadpath = Path.Combine(Directory.GetCurrentDirectory(), @$"Anime\{directory}");
            Directory.CreateDirectory(_downloadpath);

            EdgeOptions options = new EdgeOptions();
            options.AddUserProfilePreference("download.default_directory", _downloadpath);

            _driver = new EdgeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Start(string url)
        {
            _driver.Navigate().GoToUrl(url);

            string title = _driver.FindElement(By.XPath("//h2")).Text;

            if (!IsFileExist(title))
            {
                _driver.FindElement(By.XPath("//div[@class='v-card-text']/button")).Click();

                _driver.FindElement(By.XPath("//div[@class='v-card-text']/a")).Click();

                WaitForFileDownload(title);
            }
        }

        public void Stop()
        {
            _driver.Quit();
        }

        private void WaitForFileDownload(string title)
        {
            while (!IsFileExist(title))
            {
                Thread.Sleep(3000);
            }
        }

        private bool IsFileExist(string title)
        {
            string data = string.Join('_', title.Split(' ')[2 ..]);

            return Directory.GetFiles(_downloadpath, $"*{data}*.mp4").Length > 0;
        }
    }
}
