using Acr.UserDialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using ValdemoroEn1.Validations;
using Xamarin.Essentials;

namespace ValdemoroEn1.ViewModels
{
    public class BusTansportTabPageViewModel : ChildBaseViewModel
    {
        private ObservableCollection<TransportItem> _searchBus = new ObservableCollection<TransportItem>();
        public ObservableCollection<TransportItem> SearchBus
        {
            get => _searchBus;
            set => SetProperty(ref _searchBus, value);
        }

        private ValidatableObject<string> _idSearch;
        public ValidatableObject<string> IdSearch
        {
            get => _idSearch;
            set => SetProperty(ref _idSearch, value);
        }

        public BusTansportTabPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _idSearch = new ValidatableObject<string>();
            AddValidations();
        }

        public DelegateCommand SearchTimeBusCommand => new DelegateCommand(SearchTimeBus).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand ValidateSearchCommand => new DelegateCommand(() => ValidateSearch()).ObservesCanExecute(() => CanNavigate);

        private void SearchTimeBus()
        {
            if (ValidateSearch())
            {
                NavigateTimePage(IdSearch.Value);
                SaveSearch(IdSearch.Value, "Búsqueda");
            }
        }

        private void LoadTimeBus(string id)
        {
            NavigateTimePage(id);
        }

        private async void NavigateTimePage(string id)
        {
            CanNavigate = false;
            NavigationParameters keyValuePairs = new NavigationParameters
            {
                { "url", string.Concat(GlobalSettings.CitramURL, id) }
            };
            await NavigationService.NavigateAsync("WebViewTransportPage", keyValuePairs);
            CanNavigate = true;
        }

        private async void EditTimeBus(string id)
        {
            CanNavigate = false;
            var response = await UserDialogs.Instance.PromptAsync(GlobalSettings.DescriptionBus, null, GlobalSettings.Accept, GlobalSettings.Cancel);

            if (response.Ok && !string.IsNullOrWhiteSpace(response.Text))
            {
                string result = Preferences.Get("searchBus", "default");
                var search = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                search[id] = response.Text;
                SearchBus.First(f => f.Id == id).Description = response.Text;
                Preferences.Set("searchBus", JsonConvert.SerializeObject(search));
            }
            CanNavigate = true;
        }

        private async void DeleteTimeBus(string id)
        {
            CanNavigate = false;
            var confirm = await UserDialogs.Instance.ConfirmAsync(GlobalSettings.DeleteBus, null, GlobalSettings.Yes, GlobalSettings.No);
            if (confirm)
            {
                string result = Preferences.Get("searchBus", "default");
                var search = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                search.Remove(id);
                SearchBus.Remove(_searchBus.Where(w => w.Id == id).First());
                if (search.Any())
                {
                    Preferences.Set("searchBus", JsonConvert.SerializeObject(search));
                }
                else
                {
                    Preferences.Remove("searchBus");
                }
            }
            CanNavigate = true;
        }

        void SaveSearch(string id, string description)
        {
            string result = Preferences.Get("searchBus", "default");

            if (result is "default")
            {
                var temList = new Dictionary<string, string>
                {
                    { id, description }
                };
                AddRefreshList(id, description);
                Preferences.Set("searchBus", JsonConvert.SerializeObject(temList));
            }
            else
            {
                var search = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                if (!search.Keys.Any(id.Equals))
                {
                    search.Add(id, description);
                    AddRefreshList(id, description);
                    Preferences.Set("searchBus", JsonConvert.SerializeObject(search));
                }
            }
        }

        void AddRefreshList(string id, string description)
        {
            SearchBus.Add(new TransportItem
            {
                Id = id,
                Description = description,
                LoadTimeBusCommand = new DelegateCommand<string>(LoadTimeBus).ObservesCanExecute(() => CanNavigate),
                EditTimeBusCommand = new DelegateCommand<string>(EditTimeBus).ObservesCanExecute(() => CanNavigate),
                DeleteTimeBusCommand = new DelegateCommand<string>(DeleteTimeBus).ObservesCanExecute(() => CanNavigate)
            });
        }

        bool ValidateSearch()
        {
            return _idSearch.Validate();
        }

        void AddValidations()
        {
            _idSearch.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.CodeBusRequiredRule });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            string result = Preferences.Get("searchBus", "default");
            if (result != "default" && !SearchBus.Any())
            {
                var search = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                foreach (var item in search)
                {
                    AddRefreshList(item.Key, item.Value);
                }
            }
            base.OnNavigatedTo(parameters);
        }
    }
}
