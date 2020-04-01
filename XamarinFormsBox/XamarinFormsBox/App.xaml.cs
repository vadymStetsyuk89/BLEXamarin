using StBox.Locator;
using StBox.Services;
using Xamarin.Forms;
using XamarinFormsBox.AppEnvironment;
using XamarinFormsBox.ViewModels;

namespace XamarinFormsBox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ViewModelLocator.RegisterDependencies(new MyDependenciesProvider());
        }

        protected override void OnStart()
        {
            INavigationService navigationService = ViewModelLocator.Resolve<INavigationService>();
            navigationService.Initialize(typeof(MainPageViewModel));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
