using Acr.UserDialogs;
using Plugin.FirebaseAuth;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.IO;
using ValdemoroEn1.Events;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using ValdemoroEn1.Validations;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ValdemoroEn1.ViewModels
{
    public class ProfilePagePopupViewModel : BindableBase
    {
        private FileResult _photo;

        private ImageSource _photoFile;
        public ImageSource PhotoFile
        {
            get => _photoFile;
            set => SetProperty(ref _photoFile, value);
        }

        private ValidatableObject<string> _name;
        public ValidatableObject<string> Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private bool _canNavigate = true;
        public bool CanNavigate
        {
            get => _canNavigate;
            set => SetProperty(ref _canNavigate, value);
        }

        private readonly INavigationService NavigationService;

        private readonly IEventAggregator _eventAggregator;

        public ProfilePagePopupViewModel()
        {
            _name = new ValidatableObject<string>();
            _eventAggregator = (IEventAggregator)App.Current.Container.Resolve(typeof(IEventAggregator));
            NavigationService = (INavigationService)App.Current.Container.Resolve(typeof(INavigationService));
            AddValidations();
            Init();
        }

        private async void Init()
        {
            ValdeUser user = await UtilsMethods.GetUserAsync();

            if (user is null | user.Photo is null)
            {
                PhotoFile = ImageSource.FromFile("profile.png");
            }
            else
            {
                PhotoFile = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(user.Photo)));
            }

            Name.Value = user.Name;
            Email = user.Email;
        }

        public DelegateCommand GoBackCommand => new DelegateCommand(GoBackPage).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand OpenCameraGalleryCommand => new DelegateCommand(OpenCameraGallery).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand UpdateProfileCommand => new DelegateCommand(UpdateProfile).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand ValidateNameCommand => new DelegateCommand(() => ValidateName());

        private async void GoBackPage()
        {
            CanNavigate = false;
            await Application.Current.MainPage.Navigation.PopModalAsync();
            CanNavigate = true;
        }

        private async void OpenCameraGallery()
        {
            CanNavigate = false;
            var result = await UserDialogs.Instance.ActionSheetAsync(GlobalSettings.Choose, GlobalSettings.Cancel, null, null, GlobalSettings.TextCamera, GlobalSettings.TextGallery);

            if (result is GlobalSettings.TextCamera)
            {
                try
                {
                    _photo = await MediaPicker.CapturePhotoAsync();
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Feature is now supported on the device
                    _ = fnsEx;
                }
                catch (PermissionException pEx)
                {
                    // Permissions not granted
                    _ = pEx;
                }
                catch (Exception ex)
                {
                    _ = ex;
                }
            }

            if (result is GlobalSettings.TextGallery)
            {
                try
                {
                    _photo = await MediaPicker.PickPhotoAsync();
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Feature is now supported on the device
                    _ = fnsEx;
                }
                catch (PermissionException pEx)
                {
                    // Permissions not granted
                    _ = pEx;
                }
                catch (Exception ex)
                {
                    _ = ex;
                }
            }

            if (_photo != null)
            {
                PhotoFile = ImageSource.FromFile(_photo.FullPath);
            }
            CanNavigate = true;
        }

        private async void UpdateProfile()
        {
            CanNavigate = false;
            if (ValidateName())
            {
                string profilePhoto;
                ValdeUser user = await UtilsMethods.GetUserAsync();
                if (_photo is null)
                {
                    profilePhoto = user.Photo;
                }
                else
                {
                    profilePhoto = Convert.ToBase64String(File.ReadAllBytes(_photo.FullPath));
                }
                user.Photo = profilePhoto;
                user.Name = Name.Value;
                await UtilsMethods.SaveUserAsync(user);
                _eventAggregator.GetEvent<ProfileEvent>().Publish();
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            CanNavigate = true;
        }

        private async void Delete()
        {
            CanNavigate = false;
            var confirm = await UserDialogs.Instance.ConfirmAsync(GlobalSettings.DeleteAccount, null, GlobalSettings.Yes, GlobalSettings.No);
            if (confirm)
            {
                UserDialogs.Instance.ShowLoading(GlobalSettings.Loading);
                await CrossFirebaseAuth.Current.Instance.CurrentUser.DeleteAsync();
                UserDialogs.Instance.HideLoading();
                Preferences.Remove("rememberLogin");
                await NavigationService.NavigateAsync("/NavigationPage/MainPage");
            }
            CanNavigate = true;
        }

        private bool ValidateName()
        {
            return _name.Validate();
        }

        private void AddValidations()
        {
            _name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.NameRequiredRule });
        }
    }
}
