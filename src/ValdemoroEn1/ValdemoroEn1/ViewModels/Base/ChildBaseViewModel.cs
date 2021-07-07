using Prism;
using Prism.Navigation;
using System;

namespace ValdemoroEn1.ViewModels
{
    public class ChildBaseViewModel : BaseViewModel, IActiveAware
    {
        protected static bool HasInitialized { get; set; }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value, RaiseIsActiveChanged);
        }

        public event EventHandler IsActiveChanged;

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        public ChildBaseViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
