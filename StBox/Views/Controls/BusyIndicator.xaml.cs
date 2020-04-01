using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StBox.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusyIndicator : ContentView
    {
        public static readonly BindableProperty PadCanvasColorProperty = BindableProperty.Create(
            nameof(PadCanvasColor),
            typeof(Color),
            typeof(BusyIndicator),
            defaultValue: default(Color),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                if (bindable is BusyIndicator declarer)
                {
                    declarer.ApplyPadCanvasColor();
                }
            });

        public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(
            nameof(IndicatorColor),
            typeof(Color),
            typeof(BusyIndicator),
            defaultValue: default(Color),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                if (bindable is BusyIndicator declarer)
                {
                    declarer.ApplyIndicatorColor();
                }
            });

        public BusyIndicator()
        {
            InitializeComponent();
        }

        public Color PadCanvasColor
        {
            get => (Color)GetValue(BusyIndicator.PadCanvasColorProperty);
            set => SetValue(BusyIndicator.PadCanvasColorProperty, value);
        }

        public Color IndicatorColor
        {
            get => (Color)GetValue(BusyIndicator.IndicatorColorProperty);
            set => SetValue(BusyIndicator.IndicatorColorProperty, value);
        }

        private void ApplyPadCanvasColor() => _padCanvas_BoxView.BackgroundColor = PadCanvasColor;

        private void ApplyIndicatorColor()
        {
            _spinnerIndicator_ActivityIndicator.Color = IndicatorColor;
        }
    }
}