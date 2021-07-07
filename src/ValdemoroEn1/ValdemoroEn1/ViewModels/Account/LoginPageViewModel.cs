using Acr.UserDialogs;
using Plugin.FirebaseAuth;
using Prism.Commands;
using Prism.Navigation;
using System;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using ValdemoroEn1.Validations;
using Xamarin.Essentials;

namespace ValdemoroEn1.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            AddValidations();
        }

        public DelegateCommand LoginCommand => new DelegateCommand(Login).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand RecoveryPassowrdCommand => new DelegateCommand(RecoveryPassword).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand ValidateEmailCommand => new DelegateCommand(() => ValidateEmail());
        public DelegateCommand ValidatePasswordCommand => new DelegateCommand(() => ValidatePassword());

        private async void Login()
        {
            CanNavigate = false;
            if (Validate())
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(GlobalSettings.Loading);

                    var result = await CrossFirebaseAuth.Current.Instance.SignInWithEmailAndPasswordAsync(Email.Value, Password.Value);

                    if (result.User != null)
                    {
                        Preferences.Set("rememberLogin", true);
                        var valdeUser = new ValdeUser
                        {
                            Id = result.User.Uid,
                            Email = result.User.Email,
                            Name = result.User.DisplayName
                        };
                        await UtilsMethods.SaveUserAsync(valdeUser);
                        await NavigationService.NavigateAsync("/MenuPage/NavigationPage/HomePage");
                    }
                }
                catch (Exception ex)
                {
                    if (ex is FirebaseAuthException authEx)
                    {
                        switch (authEx.ErrorType)
                        {
                            case ErrorType.InvalidCredentials:
                                await UserDialogs.Instance.AlertAsync(GlobalSettings.InvalidEmailPassword, null, GlobalSettings.Accept);
                                break;
                            case ErrorType.TooManyRequests:
                                await UserDialogs.Instance.AlertAsync(GlobalSettings.Error, null, GlobalSettings.Accept);
                                break;
                            default:
                                break;
                        }
                    }
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            CanNavigate = true;
        }

        private async void RecoveryPassword()
        {
            CanNavigate = false;
            var result = await UserDialogs.Instance.PromptAsync(GlobalSettings.YouEmail, null, GlobalSettings.Accept, GlobalSettings.Cancel);

            if (result.Ok && !string.IsNullOrWhiteSpace(result.Text))
            {
                await CrossFirebaseAuth.Current.Instance.SendPasswordResetEmailAsync(result.Text);
                await UserDialogs.Instance.AlertAsync(GlobalSettings.SendEmail, null, GlobalSettings.Accept);
            }
            CanNavigate = true;
        }

        private bool Validate()
        {
            bool isValidEmail = ValidateEmail();
            bool isValidPassword = ValidatePassword();
            return isValidEmail && isValidPassword;
        }

        private bool ValidateEmail()
        {
            return _email.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }

        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.EmailRequiredRule });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = GlobalSettings.EmailInvalidRule });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.PasswordRequiredRule });
        }

    }
}
