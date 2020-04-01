﻿using Plugin.BLE.Abstractions.Contracts;
using StBox.Environment;
using StBox.Locator;
using StBox.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels
{
    public class ServiceItemViewModel : ExtendedBindableObject
    {

        public ServiceItemViewModel(IService source)
        {
            Service = source;
            Name = Service.Name;
        }

        public ICommand OnExploreServiceCommand => new Command(async () =>
        {
            await ViewModelLocator.Resolve<INavigationService>().NavigateToAsync<DeviceCharacteristicsViewModel>(Service);
        });

        private IService _service;
        public IService Service
        {
            get => _service;
            private set => SetProperty<IService>(ref _service, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty<string>(ref _name, value);
        }
    }
}
