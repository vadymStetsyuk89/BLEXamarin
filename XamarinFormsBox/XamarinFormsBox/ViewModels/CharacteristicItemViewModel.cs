using Plugin.BLE.Abstractions.Contracts;
using StBox.Environment;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels
{
    public class CharacteristicItemViewModel : ExtendedBindableObject
    {

        public CharacteristicItemViewModel(ICharacteristic source)
        {
            Characteristic = source;
            Name = Characteristic.Name;
        }

        public ICommand OnExploreCharacteristicCommand => new Command(async () =>
        {
            //await ViewModelLocator.Resolve<INavigationService>().NavigateToAsync(Service)
        });

        private ICharacteristic _characteristic;
        public ICharacteristic Characteristic
        {
            get => _characteristic;
            private set => SetProperty<ICharacteristic>(ref _characteristic, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty<string>(ref _name, value);
        }
    }
}
