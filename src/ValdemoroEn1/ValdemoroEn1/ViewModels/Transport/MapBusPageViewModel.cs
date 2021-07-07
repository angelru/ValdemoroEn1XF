using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using ValdemoroEn1.Models;
using ValdemoroEn1.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ValdemoroEn1.ViewModels
{
    public class MapBusPageViewModel : BaseViewModel
    {
        private ObservableCollection<MapsBusItem> _mapsBusItem = new ObservableCollection<MapsBusItem>();
        public ObservableCollection<MapsBusItem> MapsBusItem
        {
            get => _mapsBusItem;
            set => SetProperty(ref _mapsBusItem, value);
        }

        Color _lineColor;
        public Color LineColor
        {
            get => _lineColor;
            set => SetProperty(ref _lineColor, value);
        }

        private readonly ObservableCollection<MapsBusItem> _mapsBusRedItem = new ObservableCollection<MapsBusItem>
            {
                new MapsBusItem
                {
                    Id = 1,
                    Name = "FFCC-Ambulatorio-FFCC",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/91161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/91161H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 2,
                    Name = "FFCC-El Restón",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/92161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/92161H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 3,
                    Name = "Circular",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/93161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/93161H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 4,
                    Name = "FFCC-Polígonos Norte",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/94161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/94161H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 5,
                    Name = "FFCC-P.I. Rompecubas",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/95161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/95161H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 6,
                    Name = "FFCC-El Caracol-El Restón II",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/96161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/96161H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 7,
                    Name = "FFCC-Hospital-El Restón",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/97161H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/97161H2.pdf"
                },
            };

        private readonly ObservableCollection<MapsBusItem> _mapsBusGreenItem = new ObservableCollection<MapsBusItem>
            {
                new MapsBusItem
                {
                    Id = 414,
                    Name = "Madrid (Villaverde Bajo-Cruce)-C.P. Madrid III",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8414H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8414H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 416,
                    Name = "Valdemoro (Hospital)-San Martín de la Vega-Titulcia-Colmenar de Oreja",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8416H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8416H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 422,
                    Name = "Madrid (Legazpi)-Valdemoro",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8422H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8422H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 424,
                    Name = "Madrid (Legazpi) - Valdemoro (El Restón)",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8424H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8424H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 425,
                    Name = "Valdemoro (Hospital) - Ciempozuelos",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8425H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8425H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 426,
                    Name = "Madrid (Legazpi) - Ciempozuelos",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8426H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8426H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 428,
                    Name = "Getafe - Valdemoro",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8428H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8428H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 466,
                    Name = "Parla - Valdemoro",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8466H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8466H2.pdf"
                },
                new MapsBusItem
                {
                    Id = 401,
                    Name = "Madrid (Atocha) - Pinto - Valdemoro",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8N401H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8N401H2.pdf",
                },
                new MapsBusItem
                {
                    Id = 402,
                    Name = "Madrid (Atocha) - Ciempozuelos - Aranjuez",
                    GoUrl = "https://www.crtm.es/datos_lineas/horarios/8N402H1.pdf",
                    GoBackUrl = "https://www.crtm.es/datos_lineas/horarios/8N402H2.pdf",
                },
            };

        public MapBusPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public DelegateCommand<string> OpenMapUrlCommand => new DelegateCommand<string>(OpenMapURL).ObservesCanExecute(() => CanNavigate);

        private async void OpenMapURL(string url)
        {
            CanNavigate = false;
            var oprtions = new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Hide,
            };
            await Browser.OpenAsync(new Uri(url), oprtions);
            CanNavigate = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            int page = parameters.GetValue<int>("busItem");
            if (page is 1)
            {
                Title = GlobalSettings.Urbanos;
                LineColor = Color.Red;
                MapsBusItem = _mapsBusRedItem;
            }
            else
            {
                Title = GlobalSettings.Interurbanos;
                LineColor = Color.Green;
                MapsBusItem = _mapsBusGreenItem;
            }
            base.OnNavigatedTo(parameters);
        }
    }
}
