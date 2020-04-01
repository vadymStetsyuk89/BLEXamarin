using Autofac;
using StBox.Locator;
using XamarinFormsBox.ViewModels;
using XamarinFormsBox.ViewModels.PagesNavigationTest;

namespace XamarinFormsBox.AppEnvironment
{
    public class MyDependenciesProvider : DependenciesProvider
    {
        protected override void ProvideAppDependencies(ContainerBuilder builder)
        {
            /// View models
            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<BirdPageViewModel>();
            builder.RegisterType<CatPageViewModel>();
            builder.RegisterType<DogPageViewModel>();

            /// Services Etc...
        }
    }
}
