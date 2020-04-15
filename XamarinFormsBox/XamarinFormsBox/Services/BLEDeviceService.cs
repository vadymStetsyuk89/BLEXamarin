using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using StBox.Environment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XamarinFormsBox.Services.Contracts;

namespace XamarinFormsBox.Services
{
    public class BLEDeviceService : ExtendedBindableObject, IBLEDeviceService
    {
        private const int SCANING_TIMEOUT = 3000;

        private IBluetoothLE _ble;
        private IAdapter _adapter;

        private TaskCompletionSource<List<IDevice>> _scaningCompletion;

        public BLEDeviceService()
        {
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _adapter.ScanTimeoutElapsed += (object sender, EventArgs e) =>
            {
                IsScanning = _adapter.IsScanning;

                ReleaseScaningCompletion(_adapter.DiscoveredDevices.ToArray());
            };
        }

        private bool _isScanning;
        public bool IsScanning {
            get => _isScanning;
            private set => SetProperty<bool>(ref _isScanning, value);
        }

        public async Task<List<IDevice>> ScanForDevicesAsync()
        {
            TaskCompletionSource<List<IDevice>> scaningCompletion = new TaskCompletionSource<List<IDevice>>();
            _scaningCompletion = scaningCompletion;

            if (_ble.State == BluetoothState.On)
            {
                try
                {
                    IsScanning = true;

                    _adapter.ScanTimeout = SCANING_TIMEOUT;

                    await _adapter.StartScanningForDevicesAsync();
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"===> {exc.Message}");
                    Debugger.Break();

                    IsScanning = false;

                    ReleaseScaningCompletion(null);
                }
            }
            else
            {
                ReleaseScaningCompletion(null);
            }

            return await scaningCompletion.Task;
        }

        private void ReleaseScaningCompletion(IEnumerable<IDevice> result)
        {
            if (_scaningCompletion != null)
            {
                _scaningCompletion.SetResult(new List<IDevice>(result == null ? new IDevice[] { } : result));
                _scaningCompletion = null;
            }
        }
    }
}
