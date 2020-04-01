using StBox.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels.PagesNavigationTest
{
    public class BirdPageViewModel : ContentPageBaseViewModel
    {
        public ICommand OnNavToCatCommand => new Command(async () => {
            await NavigationService.NavigateToAsync<CatPageViewModel>();
        });

        public ICommand OnNavToDogCommand => new Command(async () => {
            await NavigationService.NavigateToAsync<DogPageViewModel>();
        });
    }
}
