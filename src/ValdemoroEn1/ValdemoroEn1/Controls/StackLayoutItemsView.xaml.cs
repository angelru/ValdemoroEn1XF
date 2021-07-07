using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ValdemoroEn1.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StackLayoutItemsView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(StackLayoutItemsView), default(ICollection));

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public StackLayoutItemsView()
        {
            InitializeComponent();
        }
    }
}