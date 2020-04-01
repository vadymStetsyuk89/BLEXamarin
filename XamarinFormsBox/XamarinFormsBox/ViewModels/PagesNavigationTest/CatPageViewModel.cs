using StBox.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels.PagesNavigationTest
{
    public class CatPageViewModel : ContentPageBaseViewModel
    {

        public ICommand OnNavToDogCommand => new Command(async () => {
            await NavigationService.NavigateToAsync<DogPageViewModel>();
        });

        public ICommand OnNavToBirdCommand => new Command(async () => {
            await NavigationService.NavigateToAsync<BirdPageViewModel>();
        });
    }
}
