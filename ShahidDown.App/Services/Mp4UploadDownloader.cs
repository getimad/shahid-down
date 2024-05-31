using OpenQA.Selenium;
using System.IO;

namespace ShahidDown.App.Services
{
    /// <summary>
    /// This class is the Mp4Upload Downloader automation.
    /// </summary>
    /// <param name="directoryName"></param>
    /// <param name="downloadpath"></param>
    public class Mp4UploadDownloader(WebController web) : IBaseDownloader
    {
        public string DownloadPath { get; } = web.DownloadPath;

        public IWebDriver DownloadDriver { get; } = web.DownloadDriver;

        public void Start(string url)
        {
            string title = DownloadDriver.FindElement(By.XPath("//h4")).Text;

            if (!IsFileExist(title))
            {
                DownloadDriver.FindElement(By.XPath("//input[@type='submit']")).Click();

                Thread.Sleep(30_000);

                DownloadDriver.FindElement(By.XPath("//button[@id='downloadbtn']")).Click();

                WaitForFileDownload(title);
            }
        }

        public bool CanStart(string url)
        {
            DownloadDriver.Navigate().GoToUrl(url);

            return DownloadDriver.FindElement(By.XPath("//input[@type='submit']")).Displayed;
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
            return Directory.GetFiles(DownloadPath, title).Length > 0;
        }
    }
}
