using Autofac;
using StBox.Services;

namespace StBox.Locator
{
    public abstract class DependenciesProvider
    {
        public void RegisterDependencies(ContainerBuilder builder) {

            /// Services
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            ProvideAppDependencies(builder);
        }

        protected abstract void ProvideAppDependencies(ContainerBuilder builder);
    }
}
