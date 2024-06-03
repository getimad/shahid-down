using OpenQA.Selenium;
using System.IO;

namespace ShahidDown.App.Services
{
    public class XFileSharingDownloader(WebController web) : IBaseDownloader
    {
        public string DownloadPath => web.DownloadPath;

        public IWebDriver DownloadDriver => web.DownloadDriver;

        public bool CanStart(string url)
        {
            DownloadDriver.Navigate().GoToUrl(url);

            try
            {
                return DownloadDriver.FindElement(By.Id("method_free")).Displayed;
            }
            catch { return false; }
        }

        public bool IsFileExist(string title)
        {
            return Directory.GetFiles(DownloadPath, title).Length > 0;
        }

        public void Start(string url)
        {
            Thread.Sleep(3_000);

            DownloadDriver.Navigate().Refresh();  // Refresh the page to apply the extension.

            DownloadDriver.FindElement(By.Id("method_free")).Click();

            string title = DownloadDriver.FindElement(By.XPath("//span[@class='dfilename']")).Text;

            if (IsFileExist(title))
                throw new Exception("File already exists.");


            DownloadDriver.FindElement(By.Id("downloadbtn")).Click();

            try
            {
                // Because there are many xFileSharing sites, and some of them have a direct link to the file.
                DownloadDriver.FindElement(By.XPath("//span[@id='direct_link']/a")).Click();
            }
            catch { }

            WaitForFileDownload(title);
        }

        public void Stop()
        {
            DownloadDriver.Quit();
        }

        public void WaitForFileDownload(string title)
        {
            while (!IsFileExist(title))
            {
                Task.Delay(3000);
            }
        }
    }
}
