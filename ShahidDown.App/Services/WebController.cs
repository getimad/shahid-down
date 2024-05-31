using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.IO;

namespace ShahidDown.App.Services
{
    public class WebController
    {
        public string DownloadPath { get; }
        public IWebDriver DownloadDriver { get; }

        public WebController(string directoryName, string? downloadpath = null)
        {
            DownloadPath = Path.Combine(downloadpath ?? Directory.GetCurrentDirectory(), directoryName);
            Directory.CreateDirectory(DownloadPath);

            EdgeOptions options = new EdgeOptions();
            options.AddExtension(Path.Combine(Directory.GetCurrentDirectory(), "Extensions", "uBlock_v1_57_2.crx"));
            options.AddUserProfilePreference("download.default_directory", DownloadPath);

            DownloadDriver = new EdgeDriver(options);
            DownloadDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);  // This will wait for 10 seconds if the element is not found.

            Thread.Sleep(3_000);  // Wait for the extension to load.

            DownloadDriver.Navigate().Refresh();  // Refresh the page to apply the extension.
        }

        public void Dispose()
        {
            DownloadDriver.Quit();
        }
    }
}
