using Prism.Navigation;

namespace ValdemoroEn1.ViewModels
{
    public class WebViewTransportPageViewModel : BaseViewModel
    {
        string _urlSource;
        public string UrlSource
        {
            get => _urlSource;
            set => SetProperty(ref _urlSource, value);
        }

        public WebViewTransportPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            UrlSource = parameters.GetValue<string>("url");
            base.OnNavigatedTo(parameters);
        }
    }
}
