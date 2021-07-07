using Prism;
using Prism.Ioc;
using ValdemoroEn1.ViewModels;
using ValdemoroEn1.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ValdemoroEn1
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Akavache.Registrations.Start("ValdemoroEn1");

            bool mainLogin = Preferences.ContainsKey("rememberLogin");

            if (mainLogin)
            {
                await NavigationService.NavigateAsync("MenuPage/NavigationPage/HomePage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/MainPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();

            containerRegistry.RegisterForNavigation<InfoItemPage, InfoItemPageViewModel>();
            containerRegistry.RegisterForNavigation<DetailItemPage, DetailItemPageViewModel>();

            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<BusTransportTabPage, BusTansportTabPageViewModel>();
            containerRegistry.RegisterForNavigation<MapsBusTransportTabPage, MapsBusTransportTabPageViewModel>();
            containerRegistry.RegisterForNavigation<WebViewTransportPage, WebViewTransportPageViewModel>();
            containerRegistry.RegisterForNavigation<MapBusPage, MapBusPageViewModel>();
        }
    }
}
