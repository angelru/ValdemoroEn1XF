using Acr.UserDialogs;
using Plugin.FirebaseAuth;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using Xamarin.Essentials;

namespace ValdemoroEn1.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IGoogleClientManager _googleClientManager;
        private readonly IPageDialogService _pageDialogService;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _googleClientManager = CrossGoogleClient.Current;
            _pageDialogService = pageDialogService;
        }

        public DelegateCommand ShowLoginCommand => new DelegateCommand(ShowLogin).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand ShowRegisterCommand => new DelegateCommand(ShowRegister).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand LoginGoogleCommand => new DelegateCommand(LoginGoogle).ObservesCanExecute(() => CanNavigate);

        async void ShowLogin()
        {
            CanNavigate = false;
            await NavigationService.NavigateAsync("LoginPage");
            CanNavigate = true;
        }

        async void ShowRegister()
        {
            CanNavigate = false;
            await NavigationService.NavigateAsync("RegisterPage");
            CanNavigate = true;
        }

        async void LoginGoogle()
        {
            CanNavigate = false;
            try
            {
                var googleResponse = await _googleClientManager.LoginAsync();
                if (googleResponse.Status is GoogleActionStatus.Completed)
                {
                    if (googleResponse.Data != null)
                    {
                        GoogleUser googleUser = googleResponse.Data;
                        string idToken = CrossGoogleClient.Current.IdToken;
                        string accessToken = CrossGoogleClient.Current.AccessToken;
                        await Task.Delay(100);
                        SignInGoogle(idToken, accessToken, googleUser.Picture.AbsoluteUri);
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(null, googleResponse.Message, GlobalSettings.Accept);
                    }
                }
            }
            catch (GoogleClientSignInNetworkErrorException)
            {

            }
            catch (GoogleClientSignInCanceledErrorException)
            {

            }
            catch (GoogleClientSignInInvalidAccountErrorException)
            {

            }
            catch (GoogleClientSignInInternalErrorException)
            {

            }
            catch (GoogleClientNotInitializedErrorException)
            {

            }
            catch (GoogleClientBaseException)
            {

            }
            finally
            {

            }
            CanNavigate = true;
        }

        async void SignInGoogle(string idToken, string token, string image)
        {
            UserDialogs.Instance.ShowLoading(GlobalSettings.Loading);
            var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(idToken, token);
            var authResult = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);

            UserDialogs.Instance.HideLoading();
            if (authResult.User != null)
            {
                byte[] photo;
                using (HttpClient client = new HttpClient())
                {
                    photo = await client.GetByteArrayAsync(image);
                }

                var user = new ValdeUser
                {
                    Id = authResult.User.Uid,
                    Email = authResult.User.Email,
                    Name = authResult.User.DisplayName,
                    Photo = Convert.ToBase64String(photo),
                    Token = token
                };
                Preferences.Set("rememberLogin", true);
                await UtilsMethods.SaveUserAsync(user);
                await NavigationService.NavigateAsync("/MenuPage/NavigationPage/HomePage");
            }
        }

        public void Logout()
        {
            _googleClientManager.Logout();
        }

    }
}
