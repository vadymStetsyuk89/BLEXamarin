using Autofac;
using StBox.Locator;
using XamarinFormsBox.Services;
using XamarinFormsBox.Services.Contracts;
using XamarinFormsBox.ViewModels;
using XamarinFormsBox.ViewModels.Authentication;

namespace XamarinFormsBox.AppEnvironment
{
    public class MyDependenciesProvider : DependenciesProvider
    {
        protected override void ProvideAppDependencies(ContainerBuilder builder)
        {
            /// View models
            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<DeviceServicesViewModel>();
            builder.RegisterType<DeviceCharacteristicsViewModel>();
            builder.RegisterType<CharacteristicDetailsViewModel>();

            builder.RegisterType<LogInPageViewModel>();
            builder.RegisterType<RegisterPageViewModel>();

            /// Services
            builder.RegisterType<BLEDeviceService>().As<IBLEDeviceService>().SingleInstance();

            /// Etc...
        }

        protected override void ProvideAppSettingsReducers()
        {
            throw new System.NotImplementedException();
        }
    }
}
