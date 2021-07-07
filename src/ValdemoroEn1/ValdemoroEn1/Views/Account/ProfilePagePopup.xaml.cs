using ValdemoroEn1.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ValdemoroEn1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePagePopup : ContentPage
    {
        public ProfilePagePopup()
        {
            InitializeComponent();
            BindingContext = new ProfilePagePopupViewModel();
        }
    }
}
