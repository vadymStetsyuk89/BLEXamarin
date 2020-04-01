using Plugin.BLE.Abstractions.Contracts;
using StBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels
{
    public class DeviceCharacteristicsViewModel : ContentPageBaseViewModel
    {

        public DeviceCharacteristicsViewModel()
        {
            Characteristics = new List<CharacteristicItemViewModel>();

        }

        private IService _targetService;
        public IService TargetService
        {
            get => _targetService;
            private set => SetProperty<IService>(ref _targetService, value);
        }

        private List<CharacteristicItemViewModel> _characteristics;
        public List<CharacteristicItemViewModel> Characteristics
        {
            get => _characteristics;
            private set => SetProperty<List<CharacteristicItemViewModel>>(ref _characteristics, value);
        }

        public override async Task InitializeAsync_NEED_TO_DEFINE_LC(object navigationData)
        {
            await base.InitializeAsync_NEED_TO_DEFINE_LC(navigationData);

            if (navigationData is IService)
            {
                TargetService = (IService)navigationData;

                await ExtractCharacteristicsAsync(TargetService);
            }
        }

        private Task ExtractCharacteristicsAsync(IService service)
        {
            IsBusy = true;

            return Task.Run(async () =>
            {
                try
                {
                    IEnumerable<ICharacteristic> foundCharacteristics = await service.GetCharacteristicsAsync();
                    List<CharacteristicItemViewModel> builtCharacteristics = new List<CharacteristicItemViewModel>(foundCharacteristics
                        .Select<ICharacteristic, CharacteristicItemViewModel>(_ => BuildCharacteristicItem(_)));

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Characteristics = builtCharacteristics;
                        IsBusy = false;
                    });
                }
                catch (Exception exc)
                {
                    await DialogService.ToastAsync("Can't resolve characteristics.");

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Characteristics = new List<CharacteristicItemViewModel>();
                        IsBusy = false;
                    });
                }

            });
        }

        private CharacteristicItemViewModel BuildCharacteristicItem(ICharacteristic characteristic)
        {
            return new CharacteristicItemViewModel(characteristic);
        }
    }
}
