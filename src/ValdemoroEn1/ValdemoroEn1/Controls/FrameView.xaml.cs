using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ValdemoroEn1.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrameView : ContentView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FrameView), default(string));
        public static readonly BindableProperty CardContentProperty = BindableProperty.Create(nameof(CardContent), typeof(View), typeof(FrameView), default(View));
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemView), default(ICommand));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ItemView), default(object));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public View CardContent
        {
            get => (View)GetValue(CardContentProperty);
            set => SetValue(CardContentProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public FrameView()
        {
            InitializeComponent();
        }
    }
}