using Expandable;
using FontAwesome;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ValdemoroEn1.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpandableContentView : ContentView
    {
        public static readonly BindableProperty FirstIconProperty = BindableProperty.Create(nameof(FirstIcon), typeof(string), typeof(ExpandableContentView), default(string));
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ExpandableContentView), default(string));
        public static readonly BindableProperty SecondIconProperty = BindableProperty.Create(nameof(SecondIcon), typeof(string), typeof(ExpandableContentView), default(string));
        public static readonly BindableProperty TemplateProperty = BindableProperty.Create(nameof(TemplateContent), typeof(View), typeof(ExpandableContentView), default(View));

        public string FirstIcon
        {
            get => (string)GetValue(FirstIconProperty);
            set => SetValue(FirstIconProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string SecondIcon
        {
            get => (string)GetValue(SecondIconProperty);
            set => SetValue(SecondIconProperty, value);
        }

        public View TemplateContent
        {
            get => (View)GetValue(TemplateProperty);
            set => SetValue(TemplateProperty, value);
        }

        public ExpandableContentView()
        {
            InitializeComponent();
        }

        void OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (SecondIcon != null)
            {
                switch (e.Status)
                {
                    case ExpandStatus.Collapsing:
                        SecondIcon = FontAwesomeIcons.AngleDown;
                        break;
                    case ExpandStatus.Expanding:
                        SecondIcon = FontAwesomeIcons.AngleUp;
                        break;
                    default:
                        return;
                }
            }
        }
    }
}