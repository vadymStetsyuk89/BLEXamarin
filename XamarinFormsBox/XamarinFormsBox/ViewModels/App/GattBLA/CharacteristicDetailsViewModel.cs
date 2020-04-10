using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using StBox.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsBox.AppEnvironment;
using XamarinFormsBox.Models.Gatt.Characteristics;

namespace XamarinFormsBox.ViewModels
{
    public class CharacteristicDetailsViewModel : ContentPageBaseViewModel
    {
        public ICommand OnReadValueCommand => new Command(async () =>
        {
            try
            {
                byte[] rawValue = await TargetCharacteristic.ReadAsync();

                LastReadValue = TryExtractCharacteristicValue(rawValue);
            }
            catch (Exception exc)
            {
                await DialogService.ToastAsync($"Can't read value. {exc.Message}");
            }
        });

        public ICommand OnWriteValueCommand => new Command(async () =>
        {
            try
            {
                byte[] encodedInput = EncodeStringToBytes(WriteValueInput);

                await TargetCharacteristic.WriteAsync(encodedInput);

                WriteValueInput = string.Empty;

                await DialogService.ToastAsync("Updated successfully");
            }
            catch (Exception exc)
            {
                await DialogService.ToastAsync($"Error: {exc.Message}");
            }
        });

        private string _writeValueInput = string.Empty;
        public string WriteValueInput {
            get => _writeValueInput;
            set => SetProperty<string>(ref _writeValueInput, value);
        }

        private bool _isSubscribedToChanges;
        public bool IsSubscribedToChanges {
            get => _isSubscribedToChanges;
            set {
                SetProperty<bool>(ref _isSubscribedToChanges, value);
                OnResolveCharacteristicChangesSubsctiption();
            }
        }

        private string _lastReadValue;
        public string LastReadValue {
            get => _lastReadValue;
            private set => SetProperty<string>(ref _lastReadValue, value);
        }

        private DateTime _lastValueDateStamp;
        public DateTime LastValueDateStamp {
            get => _lastValueDateStamp;
            private set => SetProperty<DateTime>(ref _lastValueDateStamp, value);
        }

        private string _permissions;
        public string Permissions {
            get => _permissions;
            private set => SetProperty<string>(ref _permissions, value);
        }

        private ICharacteristic _targetCharacteristic;
        public ICharacteristic TargetCharacteristic {
            get => _targetCharacteristic;
            private set {
                if (TargetCharacteristic != null)
                    TargetCharacteristic.ValueUpdated -= OnTargetCharacteristicValueUpdated;

                SetProperty<ICharacteristic>(ref _targetCharacteristic, value);

                if (value != null)
                {
                    Permissions = (value.CanRead ? "Read " : "") +
                       (value.CanWrite ? "Write " : "") +
                       (value.CanUpdate ? "Update" : "");
                }
                else
                {
                    Permissions = string.Empty;
                }

                OnResolveCharacteristicChangesSubsctiption();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            WriteValueInput = string.Empty;
            LastReadValue = string.Empty;

            if (TargetCharacteristic != null)
                TargetCharacteristic.ValueUpdated -= OnTargetCharacteristicValueUpdated;

            TargetCharacteristic = null;
        }

        public override async Task InitializeAsync_NEED_TO_DEFINE_LC(object navigationData)
        {
            if (navigationData is ICharacteristic)
            {
                TargetCharacteristic = (ICharacteristic)navigationData;
            }

            await base.InitializeAsync_NEED_TO_DEFINE_LC(navigationData);
        }

        private async void OnResolveCharacteristicChangesSubsctiption()
        {
            if (TargetCharacteristic != null)
            {
                if (IsSubscribedToChanges)
                {
                    try
                    {
                        await TargetCharacteristic.StartUpdatesAsync();
                        TargetCharacteristic.ValueUpdated += OnTargetCharacteristicValueUpdated;
                    }
                    catch (Exception exc)
                    {
                        await DialogService.ToastAsync($"Listen to updates: {exc.Message}");
                        TargetCharacteristic.ValueUpdated -= OnTargetCharacteristicValueUpdated;
                        IsSubscribedToChanges = false;
                    }
                }
                else
                {
                    TargetCharacteristic.ValueUpdated -= OnTargetCharacteristicValueUpdated;
                }
            }
        }

        private byte[] EncodeStringToBytes(string source)
        {
            byte[] result = null;

            if (!string.IsNullOrEmpty(source))
            {
                source
                    .Split(' ')
                    .Where(token => !string.IsNullOrEmpty(token))
                    .Select(token => Convert.ToByte(token, 16))
                    .ToArray();
            }

            if (result == null) result = new byte[] { };

            return result;
        }

        private string TryExtractCharacteristicValue(byte[] rawValue)
        {
            string result = "";

            try
            {
                if (rawValue != null && TargetCharacteristic != null)
                {
                    /// TODO: need to rewrite!!! 
                    /// TODO: Remove whole `if statement`!!!
                    /// 
                    if (TargetCharacteristic.Id == BLASpecificationCodes.BATTERY_LEVEL_CHARACTERISTIC)
                    {
                        result = $"{rawValue.FirstOrDefault().ToString()}%";
                    }
                    else if (TargetCharacteristic.Id == BLASpecificationCodes.MEASUREMENT_INTERVAL_CHARACTERISTIC)
                    {
                        if (rawValue.Any())
                        {
                            result = $"{rawValue.FirstOrDefault().ToString()} seconds interval";
                        }
                        else
                        {
                            result = rawValue.ToHexString();
                        }
                    }
                    else if (TargetCharacteristic.Id == BLASpecificationCodes.BODY_SENSOR_LOCATION_CHARACTERISTIC)
                    {
                        if (rawValue.Any() && Enum.TryParse<BodySensorLocations>(rawValue.FirstOrDefault().ToString(), out BodySensorLocations parsed))
                        {
                            result = $"{parsed.GetDescription()}";
                        }
                        else
                        {
                            result = rawValue.ToHexString();
                        }
                    }
                    else if (TargetCharacteristic.Id == BLASpecificationCodes.HEART_RATE_MEASUREMENT_CHARACTERISTIC)
                    {
                        string heartRate = $"Rate: {rawValue[1]}";

                        string energyExpended = $"Energy expended: {rawValue[2]}";

                        result = $"{heartRate} {energyExpended}";
                    }
                    else if (TargetCharacteristic.Id == BLASpecificationCodes.UART_RX_CHARACTERISTIC)
                    {
                        result = $"{rawValue.ToHexString()} hex bytes";
                    }
                    else if (TargetCharacteristic.Id == BLASpecificationCodes.UART_TX_CHARACTERISTIC)
                    {
                        result = $"{rawValue.ToHexString()} hex bytes";
                    }
                    else
                    {
                        result = $"{rawValue.ToHexString()} hex bytes";
                    }
                }
            }
            catch
            {
                Console.WriteLine("ERROR: while extracting values");
            }

            LastValueDateStamp = DateTime.Now;

            return result;
        }

        private void OnTargetCharacteristicValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            if (e.Characteristic != null)
            {
                LastReadValue = TryExtractCharacteristicValue(e.Characteristic.Value);
                //await DialogService.ToastAsync($"Value updated: `{TryExtractCharacteristicValue(e.Characteristic.Value)}`");
            }
        }
    }
}
