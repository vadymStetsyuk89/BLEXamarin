using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using StBox.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels
{
    public class MainPageViewModel : ContentPageBaseViewModel
    {
        private const int SCANING_TIMEOUT = 5000;

        private IBluetoothLE _ble;
        private IAdapter _adapter;

        public MainPageViewModel()
        {
            Devices = new ObservableCollection<DeviceItemViewModel>();

            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _adapter.DeviceDiscovered += OnAdapterDeviceDiscovered;

            _adapter.ScanTimeoutElapsed += OnAdapterScanTimeoutElapsed;
        }

        public ICommand OnScanForeDevicesCommand => new Command(async () =>
        {
            Devices.Clear();

            if (_ble.State == BluetoothState.On)
            {
                if (_adapter.IsScanning)
                {
                    await DialogService.ToastAsync("Wait, still scanning.");
                }
                else
                {
                    _adapter.ScanTimeout = SCANING_TIMEOUT;

                    try
                    {
                        IsScanning = true;
                        await _adapter.StartScanningForDevicesAsync();
                    }
                    catch
                    {
                        IsScanning = false;
                    }
                }
            }
            else
            {
                await DialogService.ToastAsync("Turn on bluetooth.");
            }
        });

        private ObservableCollection<DeviceItemViewModel> _devices;
        public ObservableCollection<DeviceItemViewModel> Devices
        {
            get => _devices;
            private set => SetProperty<ObservableCollection<DeviceItemViewModel>>(ref _devices, value);
        }

        private bool _isScanning;
        public bool IsScanning
        {
            get => _isScanning;
            private set {
                IsBusy = value;
                SetProperty<bool>(ref _isScanning, value);
            }
        }

        private void OnAdapterDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            IDevice incommingDevice = e.Device;
            DeviceItemViewModel existingDevice = Devices.FirstOrDefault(deviceItem => deviceItem.Device.Id == incommingDevice.Id);

            if (existingDevice != null)
            {
                int index = Devices.IndexOf(existingDevice);
                Devices.Add(BuildDeviceItem(incommingDevice));
                Devices.Remove(existingDevice);
            }
            else
            {
                Devices.Add(BuildDeviceItem(incommingDevice));
            }
        }

        private DeviceItemViewModel BuildDeviceItem(IDevice source)
        {
            return new DeviceItemViewModel(source);
        }

        private void OnAdapterScanTimeoutElapsed(object sender, System.EventArgs e)
        {
            IsScanning = false;
        }
    }
}
