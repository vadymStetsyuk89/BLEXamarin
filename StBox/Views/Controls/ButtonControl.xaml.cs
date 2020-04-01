using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StBox.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ButtonControl : ContentView
    {
        private static readonly string BUTTON_BACKGROUND_COLOR_LUMINOSITY_ANIMATION_NAME = "button_background_color_luminosity_animation_name";

        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(
            propertyName: nameof(ButtonText),
            returnType: typeof(string),
            declaringType: typeof(ButtonControl),
            defaultValue: default(string));

        public static readonly BindableProperty ButtonFontSizeProperty = BindableProperty.Create(
            propertyName: nameof(ButtonFontSize),
            returnType: typeof(double),
            declaringType: typeof(ButtonControl),
            defaultValue: default(double));

        public static readonly BindableProperty ButtonTextColorProperty = BindableProperty.Create(
            propertyName: nameof(ButtonTextColor),
            returnType: typeof(Color),
            declaringType: typeof(ButtonControl),
            defaultValue: default(Color));

        public static readonly BindableProperty ButtonFontFamilyProperty = BindableProperty.Create(
            propertyName: nameof(ButtonFontFamily),
            returnType: typeof(string),
            declaringType: typeof(ButtonControl),
            defaultValue: default(string));

        public static readonly BindableProperty ButtonTextHorizontalAlignmentProperty = BindableProperty.Create(
            propertyName: nameof(ButtonTextHorizontalAlignment),
            returnType: typeof(LayoutOptions),
            declaringType: typeof(ButtonControl),
            defaultValue: LayoutOptions.CenterAndExpand);

        public static readonly BindableProperty ButtonTextVerticalAlignmentProperty = BindableProperty.Create(
            propertyName: nameof(ButtonTextVerticalAlignment),
            returnType: typeof(LayoutOptions),
            declaringType: typeof(ButtonControl),
            defaultValue: LayoutOptions.CenterAndExpand);

        public static readonly BindableProperty ButtonBackgroundColorProperty = BindableProperty.Create(
            propertyName: nameof(ButtonBackgroundColor),
            returnType: typeof(Color),
            declaringType: typeof(ButtonControl),
            defaultValue: Color.Transparent,
            propertyChanged: (bindable, oldVal, newVal) => ((ButtonControl)bindable)?.OnButtonBackgroundColor());

        public static readonly BindableProperty ButtonBorderThicknessProperty = BindableProperty.Create(
            propertyName: nameof(ButtonBorderThickness),
            returnType: typeof(int),
            declaringType: typeof(ButtonControl),
            defaultValue: default(int));

        public static readonly BindableProperty ButtonCornerRadiusProperty = BindableProperty.Create(
            propertyName: nameof(ButtonCornerRadius),
            returnType: typeof(int),
            declaringType: typeof(ButtonControl),
            defaultValue: default(int));

        public static readonly BindableProperty ButtonBorderCollorProperty = BindableProperty.Create(
            propertyName: nameof(ButtonBorderCollor),
            returnType: typeof(Color),
            declaringType: typeof(ButtonControl),
            defaultValue: Color.Transparent);

        public static readonly BindableProperty ButtonPaddingProperty = BindableProperty.Create(
            propertyName: nameof(ButtonPadding),
            returnType: typeof(Thickness),
            declaringType: typeof(ButtonControl),
            defaultValue: new Thickness(6));

        public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(ButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(ButtonControl),
            defaultValue: default(ICommand));

        public static readonly BindableProperty ButtonCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(ButtonCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(ButtonControl),
            defaultValue: default(object));

        private readonly TapGestureRecognizer _mainContentSpotTapGestureRecognizer = new TapGestureRecognizer();

        public ButtonControl()
        {
            InitializeComponent();

            ///
            /// Wiring button `text` properties, with appropriate label view
            /// 
            _buttonText_Label.SetBinding(Label.TextProperty, new Binding(nameof(ButtonText), source: this));
            _buttonText_Label.SetBinding(Label.FontSizeProperty, new Binding(nameof(ButtonFontSize), mode: BindingMode.TwoWay, source: this));
            _buttonText_Label.SetBinding(Label.TextColorProperty, new Binding(nameof(ButtonTextColor), mode: BindingMode.TwoWay, source: this));
            _buttonText_Label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(ButtonFontFamily), mode: BindingMode.TwoWay, source: this));
            _buttonText_Label.SetBinding(View.HorizontalOptionsProperty, new Binding(nameof(ButtonTextHorizontalAlignment), mode: BindingMode.TwoWay, source: this));
            _buttonText_Label.SetBinding(View.VerticalOptionsProperty, new Binding(nameof(ButtonTextVerticalAlignment), mode: BindingMode.TwoWay, source: this));

            ///
            /// Wiring button `box` properties, with appropriate container view
            /// 
            _mainContentSpot_ExtendedContentView.SetBinding(VisualElement.BackgroundColorProperty, new Binding(nameof(ButtonBackgroundColor), mode: BindingMode.TwoWay, source: this));
            _mainContentSpot_ExtendedContentView.SetBinding(ContentViewExtended.BorderThicknessProperty, new Binding(nameof(ButtonBorderThickness), mode: BindingMode.TwoWay, source: this));
            _mainContentSpot_ExtendedContentView.SetBinding(ContentViewExtended.CornerRadiusProperty, new Binding(nameof(ButtonCornerRadius), mode: BindingMode.OneWay, source: this));
            _mainContentSpot_ExtendedContentView.SetBinding(ContentViewExtended.BorderColorProperty, new Binding(nameof(ButtonBorderCollor), mode: BindingMode.TwoWay, source: this));
            _content_ContentView.SetBinding(Xamarin.Forms.Layout.PaddingProperty, new Binding(nameof(ButtonPadding), mode: BindingMode.TwoWay, source: this));

            _mainContentSpotTapGestureRecognizer.Command = new Command((object param) =>
            {
                ButtonCommand?.Execute(ButtonCommandParameter);

                this.AbortAnimation(BUTTON_BACKGROUND_COLOR_LUMINOSITY_ANIMATION_NAME);

                new Animation(((v) => _backing_BoxView.Opacity = (v <= .4) ? v : .8 - v), 0, .8)
                    .Commit(this, BUTTON_BACKGROUND_COLOR_LUMINOSITY_ANIMATION_NAME, length: 125, finished: (d, b) => _backing_BoxView.Opacity = 0);
            });
            _mainContentSpot_ExtendedContentView.GestureRecognizers.Add(_mainContentSpotTapGestureRecognizer);
            OnButtonBackgroundColor();
        }

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonControl.ButtonCommandProperty);
            set => SetValue(ButtonControl.ButtonCommandProperty, value);
        }

        public object ButtonCommandParameter
        {
            get => GetValue(ButtonCommandParameterProperty);
            set => SetValue(ButtonCommandParameterProperty, value);
        }

        public Thickness ButtonPadding
        {
            get => (Thickness)GetValue(ButtonControl.ButtonPaddingProperty);
            set => SetValue(ButtonControl.ButtonPaddingProperty, value);
        }

        public Color ButtonBorderCollor
        {
            get => (Color)GetValue(ButtonControl.ButtonBorderCollorProperty);
            set => SetValue(ButtonControl.ButtonBorderCollorProperty, value);
        }

        public int ButtonCornerRadius
        {
            get => (int)GetValue(ButtonControl.ButtonCornerRadiusProperty);
            set => SetValue(ButtonControl.ButtonCornerRadiusProperty, value);
        }

        public int ButtonBorderThickness
        {
            get => (int)GetValue(ButtonControl.ButtonBorderThicknessProperty);
            set => SetValue(ButtonControl.ButtonBorderThicknessProperty, value);
        }

        public Color ButtonBackgroundColor
        {
            get => (Color)GetValue(ButtonControl.ButtonBackgroundColorProperty);
            set => SetValue(ButtonControl.ButtonBackgroundColorProperty, value);
        }

        public LayoutOptions ButtonTextVerticalAlignment
        {
            get => (LayoutOptions)GetValue(ButtonControl.ButtonTextVerticalAlignmentProperty);
            set => SetValue(ButtonControl.ButtonTextVerticalAlignmentProperty, value);
        }

        public LayoutOptions ButtonTextHorizontalAlignment
        {
            get => (LayoutOptions)GetValue(ButtonControl.ButtonTextHorizontalAlignmentProperty);
            set => SetValue(ButtonControl.ButtonTextHorizontalAlignmentProperty, value);
        }

        public string ButtonFontFamily
        {
            get => GetValue(ButtonControl.ButtonFontFamilyProperty) as string;
            set => SetValue(ButtonControl.ButtonFontFamilyProperty, value);
        }

        public Color ButtonTextColor
        {
            get => (Color)GetValue(ButtonControl.ButtonTextColorProperty);
            set => SetValue(ButtonControl.ButtonTextColorProperty, value);
        }

        public double ButtonFontSize
        {
            get => (double)GetValue(ButtonControl.ButtonFontSizeProperty);
            set => SetValue(ButtonControl.ButtonFontSizeProperty, value);
        }

        public string ButtonText
        {
            get => GetValue(ButtonControl.ButtonTextProperty) as string;
            set => SetValue(ButtonControl.ButtonTextProperty, value);
        }

        private void OnButtonBackgroundColor()
        {
            //_backing_BoxView.BackgroundColor = ButtonBackgroundColor.WithLuminosity(1 - ButtonBackgroundColor.Luminosity);
            _backing_BoxView.BackgroundColor = (ButtonBackgroundColor.Luminosity <= .5) ? Color.WhiteSmoke : Color.LightGray;
        }
    }
}