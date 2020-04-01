using Plugin.BLE.Abstractions.Contracts;
using StBox.Environment;
using StBox.Locator;
using StBox.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels
{
    public class DeviceItemViewModel : ExtendedBindableObject
    {
        private const string NO_NAME_STUB = "Unknown device";

        public DeviceItemViewModel(IDevice device)
        {
            Device = device;
            Name = string.IsNullOrEmpty(Device.Name) ? NO_NAME_STUB : Device.Name;
        }

        public ICommand OnExploreDeviceCommand => new Command(async () =>
        {
            await ViewModelLocator.Resolve<INavigationService>().NavigateToAsync<DeviceServicesViewModel>(Device);
        });

        private IDevice _device;
        public IDevice Device
        {
            get => _device;
            private set => SetProperty<IDevice>(ref _device, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty<string>(ref _name, value);
        }
    }
}
