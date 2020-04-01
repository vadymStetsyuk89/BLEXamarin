using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageView : ContentPage
    {
        public MainPageView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}