using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ValdemoroEn1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewTransportPage : ContentPage
    {
        public WebViewTransportPage()
        {
            InitializeComponent();
        }

        void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (!e.Url.Contains("citram"))
            {
                e.Cancel = true;
            }
        }
    }
}
