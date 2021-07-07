using Acr.UserDialogs;
using FontAwesome;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.IO;
using ValdemoroEn1.Events;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using ValdemoroEn1.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ValdemoroEn1.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        private ValdeUser _valdeUser;
        public ValdeUser ValdeUser
        {
            get => _valdeUser;
            set => SetProperty(ref _valdeUser, value);
        }

        private ImageSource _photoFile;
        public ImageSource PhotoFile
        {
            get => _photoFile;
            set => SetProperty(ref _photoFile, value);
        }

        private string _iconExpand = FontAwesomeIcons.AngleDown;
        public string IconExpand
        {
            get => _iconExpand;
            set => SetProperty(ref _iconExpand, value);
        }

        public ObservableCollection<MyMenuItem> MenuItems { get; private set; }
        public ObservableCollection<MyMenuItem> SubMenuItems { get; private set; }
        public ObservableCollection<MyMenuItem> EndMenuItems { get; private set; }

        public MenuPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            MenuItems = new ObservableCollection<MyMenuItem>
            {
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.Utensils,
                    Text = "Restaurantes",
                    PageName = nameof(InfoItemPage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.Utensils,
                    Text = "Comida a domicilio",
                    PageName = nameof(InfoItemPage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.BusAlt,
                    Text = "Transporte",
                    PageName = nameof(MainTabbedPage)
                }
            };

            SubMenuItems = new ObservableCollection<MyMenuItem>
            {
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.ClinicMedical,
                    Text = "Centros médico",
                    PageName = nameof(InfoItemPage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.Tooth,
                    Text = "Dentista",
                    PageName = nameof(InfoItemPage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.FirstAid,
                    Text = "Farmacias",
                    PageName = nameof(InfoItemPage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.Hands,
                    Text = "Fisioterapia",
                    PageName = nameof(InfoItemPage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.Dumbbell,
                    Text = "Gimnasio",
                    PageName = nameof(InfoItemPage)
                }
            };

            EndMenuItems = new ObservableCollection<MyMenuItem>
            {
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.QuestionCircle,
                    Text = "Ayuda",
                    PageName = nameof(HomePage)
                },
                new MyMenuItem()
                {
                    Icon = FontAwesomeIcons.PowerOff,
                    Text = "Cerrar sesión",
                    PageName = "Logout"
                }
            };

            eventAggregator.GetEvent<ProfileEvent>().Subscribe(GetProfileEvent);
        }

        public DelegateCommand<MyMenuItem> NavigateCommand => new DelegateCommand<MyMenuItem>(Navigate).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand ShowProfileCommand => new DelegateCommand(ShowProfile).ObservesCanExecute(() => CanNavigate);

        private async void Navigate(MyMenuItem myMenuItem)
        {
            CanNavigate = false;
            if (myMenuItem.PageName is nameof(InfoItemPage))
            {
                NavigationParameters keyValuePairs = new NavigationParameters
                {
                    { "query", myMenuItem.Text }
                };

                await NavigationService.NavigateAsync("NavigationPage/" + myMenuItem.PageName, keyValuePairs);
            }
            else if (myMenuItem.PageName is "Logout")
            {
                var result = await UserDialogs.Instance.ConfirmAsync(GlobalSettings.Sesion, null, GlobalSettings.Yes, GlobalSettings.No);
                if (result)
                {
                    Preferences.Remove("rememberLogin");
                    await NavigationService.NavigateAsync("/NavigationPage/MainPage");
                }
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/" + myMenuItem.PageName);
            }
            CanNavigate = true;
        }

        private async void ShowProfile()
        {
            CanNavigate = false;
            await Application.Current.MainPage.Navigation.PushModalAsync(new ProfilePagePopup());
            CanNavigate = true;
        }

        private async void GetProfileEvent()
        {
            ValdeUser user = await UtilsMethods.GetUserAsync();

            if (user?.Photo is null)
            {
                PhotoFile = ImageSource.FromFile("profile.png");
            }
            else
            {
                PhotoFile = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(user.Photo)));
            }

            ValdeUser = user;
        }
    }
}