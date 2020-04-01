using StBox.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels.PagesNavigationTest
{
    public class DogPageViewModel : ContentPageBaseViewModel
    {
        public ICommand OnNavToCatCommand => new Command(async () => {
            await NavigationService.NavigateToAsync<CatPageViewModel>();
        });

        public ICommand OnNavToBirdCommand => new Command(async () => {
            await NavigationService.NavigateToAsync<BirdPageViewModel>();
        });
    }
}
