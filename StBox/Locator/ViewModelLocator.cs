using Autofac;
using System;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Forms;

namespace StBox.Locator
{
    public static class ViewModelLocator
    {
        private const string VIEW_WIRING_PATH_SEGMENT = ".Views.";
        private const string VIEW_MODEL_WIRING_PATH_SEGMENT = ".ViewModels.";
        private const string MODEL_NAME_PART = "Model";

        private static IContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached(
                "AutoWireViewModel",
                typeof(bool),
                typeof(ViewModelLocator),
                default(bool),
                propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
                {
                    Element view = bindable as Element;
                    if (view == null)
                    {
                        Debugger.Break();
                        return;
                    }

                    Type viewType = view.GetType();
                    string viewName = viewType.FullName.Replace(VIEW_WIRING_PATH_SEGMENT, VIEW_MODEL_WIRING_PATH_SEGMENT);
                    string viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                    //string viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);
                    string viewModelName = $"{viewName}{MODEL_NAME_PART}, {viewAssemblyName}";

                    Type viewModelType = Type.GetType(viewModelName);
                    if (viewModelType == null)
                    {
                        Debugger.Break();
                        return;
                    }
                    /// TODO: maybe define `box base view model`
                    /// 
                    object viewModel = _container.Resolve(viewModelType);
                    /// TODO: check is VM is resolved
                    /// 
                    view.BindingContext = viewModel;
                });

        public static bool GetAutoWireViewModel(BindableObject bindable) =>
            (bool)bindable.GetValue(AutoWireViewModelProperty);

        public static void SetAutoWireViewModel(BindableObject bindable, bool value) =>
            bindable.SetValue(AutoWireViewModelProperty, value);

        public static T Resolve<T>() => _container.Resolve<T>();

        public static bool IsRegistred<T>() => _container.IsRegistered<T>();

        public static void RegisterDependencies(DependenciesProvider dependenciesProvider)
        {
            if (_container != null) _container.Dispose();

            ContainerBuilder builder = new ContainerBuilder();
            dependenciesProvider.RegisterDependencies(builder);

            _container = builder.Build();
        }
    }
}
