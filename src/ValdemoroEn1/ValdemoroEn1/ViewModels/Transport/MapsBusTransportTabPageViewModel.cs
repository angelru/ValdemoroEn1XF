using Prism.Commands;
using Prism.Navigation;

namespace ValdemoroEn1.ViewModels
{
    public class MapsBusTransportTabPageViewModel : ChildBaseViewModel
    {
        public MapsBusTransportTabPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public DelegateCommand<string> OpenMapPageCommand => new DelegateCommand<string>(OpenMapPage).ObservesCanExecute(() => CanNavigate);

        private async void OpenMapPage(string page)
        {
            CanNavigate = false;
            var keyValuePairs = new NavigationParameters
                {
                    { "busItem", page }
                };

            await NavigationService.NavigateAsync("MapBusPage", keyValuePairs);
            CanNavigate = true;
        }

    }
}
