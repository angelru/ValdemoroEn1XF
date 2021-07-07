using Acr.UserDialogs;
using Plugin.FirebaseAuth;
using Prism.Commands;
using Prism.Navigation;
using System;
using ValdemoroEn1.Utils;
using ValdemoroEn1.Validations;

namespace ValdemoroEn1.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        private ValidatableObject<string> _name;
        public ValidatableObject<string> Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

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

        public RegisterPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _name = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            AddValidations();
        }


        public DelegateCommand RegisterCommand => new DelegateCommand(Register).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand ValidateNameCommand => new DelegateCommand(() => ValidateName());
        public DelegateCommand ValidateEmailCommand => new DelegateCommand(() => ValidateEmail());
        public DelegateCommand ValidatePasswordCommand => new DelegateCommand(() => ValidatePassword());

        private async void Register()
        {
            CanNavigate = false;

            if (Validate())
            {
                try
                {
                    UserDialogs.Instance.ShowLoading(GlobalSettings.Loading);
                    var authResult = await CrossFirebaseAuth.Current.Instance.CreateUserWithEmailAndPasswordAsync(Email.Value, Password.Value);

                    if (authResult.User != null)
                    {
                        await authResult.User.SendEmailVerificationAsync();
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync(GlobalSettings.SendEmail, null, GlobalSettings.Accept);
                        await NavigationService.GoBackAsync();
                    }

                }
                catch (Exception ex)
                {
                    if (ex is FirebaseAuthException authEx)
                    {
                        switch (authEx.ErrorType)
                        {
                            case ErrorType.InvalidUser:
                            case ErrorType.Email:
                                await UserDialogs.Instance.AlertAsync(GlobalSettings.EmailExists, null, GlobalSettings.Accept);
                                break;
                            default:
                                break;
                        }
                    }
                    UserDialogs.Instance.HideLoading();
                }

                CanNavigate = true;
            }
        }

        private bool Validate()
        {
            bool isValidName = ValidateName();
            bool isValidEmail = ValidateEmail();
            bool isValidPassword = ValidatePassword();
            return isValidName && isValidEmail && isValidPassword;
        }

        private bool ValidateName()
        {
            return _name.Validate();
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
            _name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.NameRequiredRule });
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.EmailRequiredRule });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = GlobalSettings.EmailInvalidRule });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = GlobalSettings.PasswordRequiredRule });
        }
    }
}
