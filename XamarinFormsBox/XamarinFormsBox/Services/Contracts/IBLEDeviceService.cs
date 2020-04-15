using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinFormsBox.Services.Contracts
{
    public interface IBLEDeviceService
    {
        bool IsScanning { get; }

        Task<List<IDevice>> ScanForDevicesAsync();
    }
}
