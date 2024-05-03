using ShahidDown.App.ViewModels.Commands;
using System.Diagnostics;
using System.Windows.Input;

namespace ShahidDown.App.ViewModels
{
    public class AboutVM
    {
        public string SourceCode { get; } = "https://github.com/getimad/shahid-down";

        public ICommand? OpenHyperlinkCommand { get; }

        public AboutVM()
        {
            OpenHyperlinkCommand = new RelayCommand(param => OnOpenHyperlinkCommand(param), param => CanOpenHyperlinkCommand(param));
        }

        private void OnOpenHyperlinkCommand(object? url)
        {
            if (url is string uri)
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }

        private bool CanOpenHyperlinkCommand(object? url)
        {
            return true;
        }
    }
}
