using Plugin.BLE.Abstractions.Contracts;
using StBox.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsBox.Services.Contracts;

namespace XamarinFormsBox.ViewModels
{
    public class MainPageViewModel : ContentPageBaseViewModel
    {
        private readonly IBLEDeviceService _bLEDeviceService;

        public MainPageViewModel(IBLEDeviceService bLEDeviceService)
        {
            _bLEDeviceService = bLEDeviceService;

            Devices = new ObservableCollection<DeviceItemViewModel>();
        }

        public ICommand OnScanForeDevicesCommand => new Command(async () =>
        {

            IsBusy = _bLEDeviceService.IsScanning;
            Devices.Clear();

            List<IDevice> exploredDevices = await _bLEDeviceService.ScanForDevicesAsync();

            foreach (DeviceItemViewModel deviceVM in exploredDevices
                .Where(device => ExcludeUnknownDevices ? !string.IsNullOrEmpty(device.Name) : true)
                .Select(device => BuildDeviceItem(device)))
            {
                Devices.Add(deviceVM);
            }

            IsBusy = _bLEDeviceService.IsScanning;




            //Devices.Clear();

            //if (_ble.State == BluetoothState.On)
            //{
            //    if (_adapter.IsScanning)
            //    {
            //        await DialogService.ToastAsync("Wait, still scanning.");
            //    }
            //    else
            //    {
            //        _adapter.ScanTimeout = SCANING_TIMEOUT;

            //        try
            //        {
            //            IsBusy = true;
            //            await _adapter.StartScanningForDevicesAsync();
            //        }
            //        catch (Exception exc)
            //        {
            //            await DialogService.ToastAsync($"Boom while scanning. {exc.Message}.");
            //        }
            //    }
            //}
            //else
            //{
            //    await DialogService.ToastAsync("Turn on bluetooth.");
            //}
        });

        private ObservableCollection<DeviceItemViewModel> _devices;
        public ObservableCollection<DeviceItemViewModel> Devices {
            get => _devices;
            private set => SetProperty<ObservableCollection<DeviceItemViewModel>>(ref _devices, value);
        }

        private bool _excludeUnknownDevices;
        public bool ExcludeUnknownDevices {
            get => _excludeUnknownDevices;
            set => SetProperty<bool>(ref _excludeUnknownDevices, value);
        }

        //private void OnAdapterDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        //{
        //    IDevice incommingDevice = e.Device;
        //    DeviceItemViewModel existingDevice = Devices.FirstOrDefault(deviceItem => deviceItem.Device.Id == incommingDevice.Id);

        //    if (existingDevice != null)
        //    {
        //        int index = Devices.IndexOf(existingDevice);
        //        Devices.Add(BuildDeviceItem(incommingDevice));
        //        Devices.Remove(existingDevice);
        //    }
        //    else
        //    {
        //        Devices.Add(BuildDeviceItem(incommingDevice));
        //    }
        //}

        private DeviceItemViewModel BuildDeviceItem(IDevice source)
        {
            return new DeviceItemViewModel(source);
        }

        private void OnAdapterScanTimeoutElapsed(object sender, System.EventArgs e)
        {
            IsBusy = false;
        }
    }
}
