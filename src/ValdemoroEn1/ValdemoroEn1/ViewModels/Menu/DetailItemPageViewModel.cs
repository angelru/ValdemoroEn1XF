using FontAwesome;
using GoogleApi.Entities.Places.Details.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ValdemoroEn1.ViewModels
{
    public class DetailItemPageViewModel : BaseViewModel
    {
        private string _iconExpand;
        public string IconExpand
        {
            get => _iconExpand;
            set => SetProperty(ref _iconExpand, value);
        }

        private string _iconClock;
        public string IconClock
        {
            get => _iconClock;
            set => SetProperty(ref _iconClock, value);
        }

        private string _textExpand;
        public string TextExpand
        {
            get => _textExpand;
            set => SetProperty(ref _textExpand, value);
        }

        private List<string> _listExpand;
        public List<string> ListExpand
        {
            get => _listExpand;
            set => SetProperty(ref _listExpand, value);
        }

        private ObservableCollection<ImageSource> _photoItem;
        public ObservableCollection<ImageSource> PhotoItem
        {
            get => _photoItem;
            set => SetProperty(ref _photoItem, value);
        }

        private ObservableCollection<DetailItem> _itemDetail;
        public ObservableCollection<DetailItem> ItemDetail
        {
            get => _itemDetail;
            set => SetProperty(ref _itemDetail, value);
        }

        private ImageSource _googleLogo;
        public ImageSource GoogleLogo
        {
            get => _googleLogo;
            set => SetProperty(ref _googleLogo, value);
        }

        private string _name;

        public DetailItemPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private async void OpenMapExecute(string text)
        {
            CanNavigate = false;

            var placemark = new Placemark
            {
                CountryName = null,
                AdminArea = null,
                Thoroughfare = text,
                Locality = null,
            };

            var options = new MapLaunchOptions { Name = _name };
            await Map.OpenAsync(placemark, options);

            CanNavigate = true;
        }

        private async void OpenUrlExecute(string text)
        {
            CanNavigate = false;

            if (text.Contains("http"))
            {
                await Browser.OpenAsync(new Uri(text), BrowserLaunchMode.SystemPreferred);
            }

            CanNavigate = true;
        }

        private void OpenTlfExecute(string text)
        {
            CanNavigate = false;

            try
            {
                PhoneDialer.Open(text);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                _ = anEx.Message;
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                _ = ex.Message;
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                _ = ex.Message;
            }

            CanNavigate = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var placesDetailsResponse = parameters.GetValue<PlacesDetailsResponse>("placeDetails");
            var photos = parameters.GetValue<ObservableCollection<ImageSource>>("photos");

            _name = placesDetailsResponse.Result.Name;
            Title = placesDetailsResponse.Result.Name;

            ItemDetail = new ObservableCollection<DetailItem>
            {
                new DetailItem
                {
                    Icon =  FontAwesomeIcons.MapMarkedAlt,
                    Text = placesDetailsResponse.Result.Vicinity,
                    Command = new DelegateCommand<string>(OpenMapExecute).ObservesCanExecute(() => CanNavigate)
                },
                new DetailItem
                {
                     Icon = FontAwesomeIcons.Phone,
                     Text = placesDetailsResponse.Result.FormattedPhoneNumber ?? GlobalSettings.WithOutTlf,
                     Command = new DelegateCommand<string>(OpenTlfExecute).ObservesCanExecute(() => CanNavigate)
                },
                new DetailItem
                {
                     Icon = FontAwesomeIcons.Globe,
                     Text = placesDetailsResponse.Result.Website ?? GlobalSettings.WithOutWeb,
                     Command = new DelegateCommand<string>(OpenUrlExecute).ObservesCanExecute(() => CanNavigate)
                }
             };

            IconExpand = FontAwesomeIcons.AngleDown;
            IconClock = FontAwesomeIcons.Clock;
            GoogleLogo = ImageSource.FromFile("powered_by_google_on_white.png");

            if (placesDetailsResponse.Result.OpeningHours is null)
            {
                TextExpand = GlobalSettings.WithOutSchedule;
                IconExpand = null;
            }
            else
            {
                TextExpand = placesDetailsResponse.Result.OpeningHours.OpenNow ? GlobalSettings.Open : GlobalSettings.Close;
                ListExpand = placesDetailsResponse.Result.OpeningHours.WeekdayTexts as List<string>;
            }

            PhotoItem = photos;
            base.OnNavigatedTo(parameters);
        }
    }
}
