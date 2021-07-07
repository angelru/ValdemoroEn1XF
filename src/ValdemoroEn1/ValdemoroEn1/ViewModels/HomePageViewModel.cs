using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using ValdemoroEn1.Events;
using ValdemoroEn1.Utils;
using Xamarin.Essentials;

namespace ValdemoroEn1.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public HomePageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ProfileEvent>().Publish();
        }

        public string TermsGoogleURL { get; private set; } = GlobalSettings.TermsGoogleURL;
        public string PrivacyGoogleURL { get; private set; } = GlobalSettings.PrivacyGoogleURL;
        public string CitramURL { get; private set; } = GlobalSettings.CitramWeb;

        public DelegateCommand<string> OpenURLCommand => new DelegateCommand<string>(OpenURL).ObservesCanExecute(() => CanNavigate);
        public DelegateCommand SendEmailCommand => new DelegateCommand(SendEmail).ObservesCanExecute(() => CanNavigate);

        async void OpenURL(string url)
        {
            CanNavigate = false;
            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
            CanNavigate = true;
        }

        async void SendEmail()
        {
            CanNavigate = false;
            try
            {
                List<string> recipients = new List<string>
                {
                   GlobalSettings.ContactEmail
                };
                var message = new EmailMessage
                {
                    Subject = "ValdemoroEn1",
                    Body = "Cuerpo del mensaje",
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                _ = fbsEx.Message;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            CanNavigate = true;
        }

    }
}
