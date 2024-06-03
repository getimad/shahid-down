using OpenQA.Selenium;
using System.IO;

namespace ShahidDown.App.Services
{
    /// <summary>
    /// This class is the Special Downloader automation.
    /// </summary>
    /// <param name="directoryName"></param>
    /// <param name="downloadpath"></param>
    public class SpecialDownloader(WebController web) : IBaseDownloader
    {
        public string DownloadPath { get; } = web.DownloadPath;

        public IWebDriver DownloadDriver { get; } = web.DownloadDriver;

        public void Start(string url)
        {
            string title = DownloadDriver.FindElement(By.XPath("//h2")).Text;

            if (IsFileExist(title))
                throw new Exception("File already exists.");

            DownloadDriver.FindElement(By.XPath("//div[@class='v-card-text']/button")).Click();

            DownloadDriver.FindElement(By.XPath("//div[@class='v-card-text']/a")).Click();

            WaitForFileDownload(title);
        }

        public bool CanStart(string url)
        {
            DownloadDriver.Navigate().GoToUrl(url);

            return DownloadDriver.FindElement(By.XPath("//div[@class='v-card-text']/button")).Displayed;
        }

        public void Stop()
        {
            DownloadDriver.Quit();
        }

        public void WaitForFileDownload(string title)
        {
            while (!IsFileExist(title))
            {
                Thread.Sleep(3000);
            }
        }

        public bool IsFileExist(string title)
        {
            string data = string.Join('_', title.Split(' ')[2 ..]);

            return Directory.GetFiles(DownloadPath, $"*{data}*.mp4").Length > 0;
        }
    }
}
