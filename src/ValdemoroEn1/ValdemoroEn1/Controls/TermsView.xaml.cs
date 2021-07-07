using Prism.Commands;
using System;
using ValdemoroEn1.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ValdemoroEn1.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsView : ContentView
    {
        public TermsView()
        {
            InitializeComponent();
        }

        public string TermsGoogleURL { get; private set; } = GlobalSettings.TermsGoogleURL;
        public string PrivacyGoogleURL { get; private set; } = GlobalSettings.PrivacyGoogleURL;

        public DelegateCommand<string> OpenURLCommand => new DelegateCommand<string>(OpenURL);

        async void OpenURL(string url)
        {
            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }
    }
}