using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Details.Request.Enums;
using GoogleApi.Entities.Places.Photos.Request;
using GoogleApi.Entities.Places.Search.Text.Request;
using GoogleApi.Entities.Places.Search.Text.Response;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using Xamarin.Forms;

namespace ValdemoroEn1.ViewModels
{
    public class InfoItemPageViewModel : BaseViewModel
    {
        private PlacesTextSearchRequest _placesTextSearchRequest;
        private string _nextPageToken;

        private InfoItem _selectedItemInfo;
        public InfoItem SelectedItemInfo
        {
            get => _selectedItemInfo;
            set => SetProperty(ref _selectedItemInfo, value);
        }

        private ImageSource _googleLogo;
        public ImageSource GoogleLogo
        {
            get => _googleLogo;
            set => SetProperty(ref _googleLogo, value);
        }

        private int _itemTreshold = 5;
        public int ItemTreshold
        {
            get => _itemTreshold;
            set => SetProperty(ref _itemTreshold, value);
        }

        public ObservableRangeCollection<InfoItem> ItemInfo { get; } = new ObservableRangeCollection<InfoItem>();

        private readonly string[] _postalCode = new string[] { "28340", "28341", "28342" };
        public InfoItemPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public DelegateCommand LoadMoreItemInfoCommand => new DelegateCommand(async () => await RunSafe(LoadMoreAsync(), false));

        private async Task LoadMoreAsync()
        {
            if (Loading || ItemInfo.Count < 5) return;

            if (_nextPageToken is null)
            {
                ItemTreshold = -1;
            }
            else
            {
                Loading = true;

                var textSearch = new PlacesTextSearchRequest
                {
                    Key = _placesTextSearchRequest.Key,
                    Query = _placesTextSearchRequest.Query,
                    Language = _placesTextSearchRequest.Language,
                    PageToken = _nextPageToken
                };

                var placesTextSearchResponse = await GooglePlaces.TextSearch.QueryAsync(textSearch);

                _nextPageToken = placesTextSearchResponse.NextPageToken;

                var items = await GetInfoItemsAsync(placesTextSearchResponse.Results);

                Loading = false;

                ItemInfo.AddRange(items);
            }
        }

        public DelegateCommand NavigateItemCommand => new DelegateCommand(async () => await RunSafe(OpenItemAsync())).ObservesCanExecute(() => CanNavigate);

        private async Task OpenItemAsync()
        {
            CanNavigate = false;

            ObservableCollection<ImageSource> photos = new ObservableCollection<ImageSource>();

            var placesDetailsResponse = await GooglePlaces.Details.QueryAsync(new PlacesDetailsRequest
            {
                Key = GlobalSettings.ApiKey,
                PlaceId = SelectedItemInfo.PlaceId,
                Language = Language.Spanish,
                Fields = FieldTypes.Basic | FieldTypes.Contact
            });

            await Task.WhenAll(placesDetailsResponse.Result.Photos.Select(async ph =>
            {
                var photo = await GooglePlaces.Photos.QueryAsync(new PlacesPhotosRequest
                {
                    Key = GlobalSettings.ApiKey,
                    PhotoReference = ph.PhotoReference,
                    MaxWidth = 1600,
                });

                ImageSource image = ImageSource.FromStream(() => { return photo.Stream; });

                photos.Add(image);
            }));

            NavigationParameters keyValuePairs = new NavigationParameters
            {
                { "placeDetails", placesDetailsResponse },
                { "photos", photos }
            };

            await NavigationService.NavigateAsync("DetailItemPage", keyValuePairs);

            CanNavigate = true;
        }

        private async Task<ObservableRangeCollection<InfoItem>> GetInfoItemsAsync(IEnumerable<TextResult> results)
        {
            ObservableRangeCollection<InfoItem> itemInfo = new ObservableRangeCollection<InfoItem>();

            await Task.WhenAll(results.Select(async search =>
            {
                if (search.Photos != null && _postalCode.Any(search.FormattedAddress.Contains))
                {
                    var photo = await GooglePlaces.Photos.QueryAsync(new PlacesPhotosRequest
                    {
                        Key = GlobalSettings.ApiKey,
                        PhotoReference = search.Photos.Select(s => s.PhotoReference).FirstOrDefault(),
                        MaxWidth = 1600
                    });

                    itemInfo.Add(new InfoItem
                    {
                        PlaceId = search.PlaceId,
                        Photo = ImageSource.FromStream(() => { return photo.Stream; }),
                        Name = search.Name
                    });
                }
            }));

            return itemInfo;
        }

        private async Task GetListInfoItemAsync(string query)
        {
            _placesTextSearchRequest = new PlacesTextSearchRequest
            {
                Key = GlobalSettings.ApiKey,
                Query = string.Format("{0} {1}", query, GlobalSettings.CityQuery),
                Language = Language.Spanish
            };

            var placesTextSearchResponse = await GooglePlaces.TextSearch.QueryAsync(_placesTextSearchRequest);

            _nextPageToken = placesTextSearchResponse.NextPageToken;

            var itemInfo = await GetInfoItemsAsync(placesTextSearchResponse.Results);

            Title = null;
            ItemInfo.Clear();
            ItemTreshold = _itemTreshold;
            ItemInfo.AddRange(itemInfo);

            GoogleLogo = ImageSource.FromFile("powered_by_google_on_white.png");
            Title = query;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("query"))
            {
                string query = parameters.GetValue<string>("query");
                await RunSafe(GetListInfoItemAsync(query));
            }
        }

    }
}