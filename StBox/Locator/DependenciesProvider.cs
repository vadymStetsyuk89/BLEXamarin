using Autofac;
using StBox.AppLocalState;
using StBox.Services;

namespace StBox.Locator
{
    public abstract class DependenciesProvider
    {
        private ContainerBuilder _builder;

        public void RegisterDependencies(ContainerBuilder builder)
        {
            _builder = builder;

            /// Services
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            ProvideAppDependencies(builder);
        }

        protected abstract void ProvideAppDependencies(ContainerBuilder builder);

        /// <summary>
        /// To register `app state reducers` simply call `RegisterAppSettingReducer` in your implementation of this method.
        /// If you don't, leave it without any calls. All reducers will be registered as singletons.
        /// </summary>
        protected abstract void ProvideAppSettingsReducers();

        protected void RegisterAppSettingReducer<TReducer, TReducerContract>()
            where TReducer : StateReducer
            where TReducerContract : IStateReducer
        {
            _builder.RegisterType<TReducer>().As<TReducerContract>().SingleInstance();
        }
    }
}
