using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using StBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFormsBox.AppEnvironment;

namespace XamarinFormsBox.ViewModels
{
    public class DeviceServicesViewModel : ContentPageBaseViewModel
    {
        private IAdapter _adapter;

        public DeviceServicesViewModel()
        {
            Services = new List<ServiceItemViewModel>();

            _adapter = CrossBluetoothLE.Current.Adapter;
        }

        private IDevice _targetDevice;
        public IDevice TargetDevice {
            get => _targetDevice;
            private set => SetProperty<IDevice>(ref _targetDevice, value);
        }

        private bool _isConnected;
        public bool IsConnected {
            get => _isConnected;
            private set => SetProperty<bool>(ref _isConnected, value);
        }

        private List<ServiceItemViewModel> _services;
        public List<ServiceItemViewModel> Services {
            get => _services;
            private set => SetProperty<List<ServiceItemViewModel>>(ref _services, value);
        }

        public override async void Dispose()
        {
            base.Dispose();

            try
            {
                Services = new List<ServiceItemViewModel>();
                IsConnected = false;
                await _adapter.DisconnectDeviceAsync(TargetDevice);
            }
            catch (Exception exc)
            {
                Debugger.Break();
            }
        }

        public override async Task InitializeAsync_NEED_TO_DEFINE_LC(object navigationData)
        {
            await base.InitializeAsync_NEED_TO_DEFINE_LC(navigationData);

            if (navigationData is IDevice)
            {
                TargetDevice = (IDevice)navigationData;

                try
                {
                    IsBusy = true;
                    await _adapter.ConnectToDeviceAsync(TargetDevice);
                    IsBusy = false;
                }
                catch (Exception exc)
                {
                    await DialogService.ToastAsync("Can't connect to device");
                }
            }
        }

        protected override void OnSubscribeOnAppEvents()
        {
            base.OnSubscribeOnAppEvents();

            _adapter.DeviceConnected += OnAdapterDeviceConnected;
            _adapter.DeviceDisconnected += OnAdapterDeviceDisconnected;
        }

        protected override void OnUnsubscribeFromAppEvents()
        {
            base.OnUnsubscribeFromAppEvents();

            _adapter.DeviceConnected -= OnAdapterDeviceConnected;
            _adapter.DeviceDisconnected -= OnAdapterDeviceDisconnected;
        }

        private Task ExtractDeviceServicesAsync(IDevice deveice)
        {
            IsBusy = true;

            return Task.Run(async () =>
            {
                try
                {
                    IEnumerable<IService> foundServices = await deveice.GetServicesAsync();
                    List<ServiceItemViewModel> builtServices = BuildServiceItems(foundServices);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Services = builtServices;
                        IsBusy = false;
                    });
                }
                catch (Exception exc)
                {
                    await DialogService.ToastAsync("Can't resolve device services.");

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Services = new List<ServiceItemViewModel>();
                        IsBusy = false;
                    });
                }
            });
        }

        private void OnAdapterDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            IsConnected = false;
        }

        private async void OnAdapterDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            IsConnected = true;
            await ExtractDeviceServicesAsync(TargetDevice);
        }

        private List<ServiceItemViewModel> BuildServiceItems(IEnumerable<IService> services)
        {
            List<ServiceItemViewModel> result = new List<ServiceItemViewModel>();

            if (services != null)
            {
                foreach (IService service in services)
                {
                    if (service.Id == BLASpecificationCodes.BATTERY_SERVICE
                        || service.Id == BLASpecificationCodes.HEALTH_THERMOMETER_SERVICE
                        || service.Id == BLASpecificationCodes.HEART_RATE_SERVICE
                        || service.Id == BLASpecificationCodes.UART_SERVICE)
                    {
                        result.Add(new ServiceItemViewModel(service));
                    }
                }
            }

            return result;
        }
    }
}
