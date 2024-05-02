using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.IO;

namespace ShahidDown.App.Services
{
    public class MP4UploadDownloader
    {
        private IWebDriver _driver;

        private string _downloadpath;
        private string _extensionPath;

        public MP4UploadDownloader()
        {
            // Path to save the downloaded anime
            _downloadpath = Path.Combine(Directory.GetCurrentDirectory(), "Anime");

            // Path to the extension
            _extensionPath = Path.Combine(Directory.GetCurrentDirectory(), "Extensions");

            // Set the options for the Edge driver
            EdgeOptions options = new EdgeOptions();
            options.AddExtensions(Directory.GetFiles(_extensionPath, "*.crx"));
            options.AddUserProfilePreference("download.default_directory", _downloadpath);

            // Create a new Edge driver
            _driver = new EdgeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        /// <summary>
        /// Start the download process.
        /// </summary>
        /// <param name="url"></param>
        public void Start(string url)
        {
            // Wait for 3 seconds
            Thread.Sleep(3000);

            // Navigate to the anime download page
            _driver.Navigate().GoToUrl(url);

            // Get the title of the anime
            string title = _driver.FindElement(By.XPath("//div[@class='name']/h4")).Text;

            // Check if the file is not exist
            if (!IsFileExist(title))
            {
                // Click to choose the free download method
                _driver.FindElement(By.Id("method_free")).Click();

                // Wait 30 seconds for the download button to appear
                 Thread.Sleep(30_000);

                // Click the download button
                _driver.FindElement(By.Id("downloadbtn")).Click();

                // Wait for the file to be downloaded
                WaitForFileDownload(title);
            }
        }

        /// <summary>
        /// Wait for the file to be downloaded.
        /// </summary>
        /// <param name="value"></param>
        private void WaitForFileDownload(string value)
        {
            // Check every 1 seconds if the file is downloaded

            while (!IsFileExist(value))
            {
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Check if the file already exist.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsFileExist(string value)
        {
            // Create directory if not exist
            Directory.CreateDirectory(_downloadpath);

            // Check if the file already exist
            string[] files = Directory.GetFiles(_downloadpath, $"*{value}*.mp4");

            return files.Length > 0;
        }
    }
}
