using Plugin.BLE.Abstractions.Contracts;
using StBox.Environment;
using StBox.Locator;
using StBox.Services;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsBox.AppEnvironment;

namespace XamarinFormsBox.ViewModels
{
    public class CharacteristicItemViewModel : ExtendedBindableObject
    {

        public CharacteristicItemViewModel(ICharacteristic source)
        {
            Characteristic = source;
            Name = Characteristic.Name;

#if DEBUG
            if (BLASpecificationCodes.UART_TX_CHARACTERISTIC == source.Id)
            {
                Name = "TX Characteristic";
            }
            else if (BLASpecificationCodes.UART_RX_CHARACTERISTIC == source.Id)
            {
                Name = "RX Characteristic";

            }
#endif
        }

        public ICommand OnExploreCharacteristicCommand => new Command(async () =>
        {
            await ViewModelLocator.Resolve<INavigationService>().NavigateToAsync<CharacteristicDetailsViewModel>(Characteristic);
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
