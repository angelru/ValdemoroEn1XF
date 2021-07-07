using Acr.UserDialogs;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using ValdemoroEn1.Utils;

namespace ValdemoroEn1.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _canNavigate = true;
        public bool CanNavigate
        {
            get => _canNavigate;
            set => SetProperty(ref _canNavigate, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _loading;
        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        public async Task RunSafe(Task task, bool showLoading = true)
        {
            try
            {
                if (IsBusy) return;

                IsBusy = true;

                if (showLoading) UserDialogs.Instance.ShowLoading(GlobalSettings.Loading);

                await task;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(GlobalSettings.Error, null, GlobalSettings.Accept);

            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
