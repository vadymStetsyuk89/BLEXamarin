using Autofac;
using StBox.Locator;
using XamarinFormsBox.ViewModels;

namespace XamarinFormsBox.AppEnvironment
{
    public class MyDependenciesProvider : DependenciesProvider
    {
        protected override void ProvideAppDependencies(ContainerBuilder builder)
        {
            /// View models
            builder.RegisterType<MainPageViewModel>();

            /// Services Etc...
        }
    }
}
