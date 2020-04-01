using StBox.Environment.Exceptions;
using StBox.ViewModels;
using StBox.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StBox.Services
{
    public class NavigationService : INavigationService
    {

        private Dictionary<Type, ContentPageBase> _cahce = new Dictionary<Type, ContentPageBase>();

        public bool IsBackButtonVisible
        {
            get => CurrentViewModelsNavigationStack.Count != 1;
        }

        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                BoxNavigationPageView navigationPage = GetMainNavigationPage();

                object context = navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2].BindingContext;

                return context as ViewModelBase;
            }
        }

        public ViewModelBase LastPageViewModel
        {
            get
            {
                return (GetMainNavigationPage()).Navigation.NavigationStack.LastOrDefault().BindingContext as ViewModelBase;
            }
        }

        /// <summary>
        /// App navigation init point. Don't forget to provide valid VM type of the 
        /// init page.
        /// </summary>
        /// <param name="initPageVMType">The VM type of the app init page (should be inherited from <see cref="ContentPageBaseViewModel"/>)</param>
        public async void Initialize(Type initPageVMType)
        {
            if (initPageVMType.IsSubclassOf(typeof(ContentPageBaseViewModel)))
            {
                Page initPage = ResolvePage(initPageVMType);
                await ((ViewModelBase)initPage.BindingContext).InitializeAsync_NEED_TO_DEFINE_LC(null);

                Application.Current.MainPage = new BoxNavigationPageView(initPage);
            }
            else
            {
                throw new StBoxException("The type of the `initPageVMType` should be inherited from `ContentPageBaseViewModel`");
            }
        }

        public IReadOnlyCollection<ViewModelBase> CurrentViewModelsNavigationStack
        {
            get
            {
                BoxNavigationPageView navigationPage = GetMainNavigationPage();

                IReadOnlyCollection<ViewModelBase> readOnlyResult =
                    navigationPage.Navigation.NavigationStack
                    .Select<Page, ViewModelBase>(p => (ViewModelBase)p.BindingContext)
                    .ToList<ViewModelBase>()
                    .AsReadOnly();

                return readOnlyResult;
            }
        }

        public Task NavigateToAsync(Type navigateTo, object parameter = null) =>
            InternalNavigateToExistedAsync(navigateTo, parameter);

        public Task NavigateToAsync<TViewModel>(object parameter = null)
            where TViewModel : ViewModelBase =>
            InternalNavigateToExistedAsync(typeof(TViewModel), parameter);

        public Task RemoveLastFromBackStackAsync()
        {
            BoxNavigationPageView navigationPage = GetMainNavigationPage();

            Page pageToRemove = navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2];
            //DisposeBindingContext(pageToRemove);
            navigationPage.Navigation.RemovePage(pageToRemove);

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            BoxNavigationPageView navigationPage = GetMainNavigationPage();

            for (int i = 0; i < navigationPage.Navigation.NavigationStack.Count - 1; i++)
            {
                var page = navigationPage.Navigation.NavigationStack[i];

                //DisposeBindingContext(page);

                navigationPage.Navigation.RemovePage(page);
            }

            return Task.FromResult(true);
        }

        public async Task GoBackAsync(object arguments = null)
        {
            BoxNavigationPageView navigationPage = GetMainNavigationPage();

            //DisposeBindingContext(mainPage.CurrentPage);

            await navigationPage.PopAsync();

            if (navigationPage.CurrentPage.BindingContext is ViewModelBase viewModel)
            {
                await viewModel.InitializeAsync_NEED_TO_DEFINE_LC(arguments);
            }
        }

        private async Task InternalNavigateToExistedAsync(Type viewModelType, object parameter)
        {
            BoxNavigationPageView navigationPage = GetMainNavigationPage();

            List<Page> stack = navigationPage.Navigation.NavigationStack.ToList();
            Page targetPage = stack.FirstOrDefault(page => page.BindingContext.GetType().FullName == viewModelType.FullName);

            if (targetPage != null)
            {
                int targetPageIndex = stack.IndexOf(targetPage);
                int stepsToForBackStack = 0;

                for (int i = 0; i < stack.Count; i++)
                {
                    if (i > targetPageIndex)
                    {
                        stepsToForBackStack++;
                    }
                }

                if (stepsToForBackStack > 1)
                {
                    stepsToForBackStack--;

                    for (int i = 1; i <= stepsToForBackStack; i++)
                    {
                        await RemoveLastFromBackStackAsync();
                    }

                    await GoBackAsync();
                }
                else if (stepsToForBackStack == 1)
                {
                    await GoBackAsync();
                }

                await ((ViewModelBase)targetPage.BindingContext).InitializeAsync_NEED_TO_DEFINE_LC(parameter);
            }
            else
            {
                Page page = ResolvePage(viewModelType);
                await navigationPage.PushAsync(page, false);

                await (page.BindingContext as ViewModelBase).InitializeAsync_NEED_TO_DEFINE_LC(parameter);
            }
        }

        private Page ResolvePage(Type viewModelType)
        {
            try
            {
                Type pageType = EvaluatePageType(viewModelType);

                ContentPageBase resultPage = null;

                if (_cahce.ContainsKey(pageType))
                {
                    resultPage = _cahce[pageType];
                }
                else
                {
                    resultPage = Activator.CreateInstance(pageType) as ContentPageBase;
                }

                if (resultPage == null)
                {
                    throw new StBoxException($"Can't activate `Page` instance. Be sure that targeting page is inharited from `ContentPageBase`.");
                }

                if (resultPage.BindingContext == null)
                {
                    throw new StBoxException($"Created page don't define `binding context`, " +
                        $"it's unexpected for this flow. " +
                        $"Probably set `locator:ViewModelLocator.AutoWireViewModel=True` in your page view." +
                        $" Or provide `bindig context` in another way.");
                }

                if (!_cahce.ContainsKey(pageType))
                {
                    _cahce.Add(pageType, resultPage);
                }

                return resultPage;
            }
            catch (Exception exc)
            {
                throw new StBoxException("Can't resolve page. Check inner exception.", exc);
            }
        }

        /// <summary>
        /// Evaluates `view page` type due to the provided VM type
        /// </summary>
        private Type EvaluatePageType(Type viewModelType)
        {
            string viewName = viewModelType.FullName.Replace("Model", string.Empty);
            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            string viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);

            Type viewType = Type.GetType(viewAssemblyName);

            return viewType;
        }

        private BoxNavigationPageView GetMainNavigationPage()
        {
            BoxNavigationPageView mainNavigationPage = Application.Current.MainPage as BoxNavigationPageView;

            if (mainNavigationPage == null)
            {
                throw new StBoxException($"App main page is not `BoxNavigationPageView`. " +
                    $"Navigation service expects that app main page is `BoxNavigationPageView`. " +
                    $"Don't forget to initialize `navigation service` before use it.");
            }

            return mainNavigationPage;
        }

        /// <summary>
        /// Dispose binding context of the target page.
        /// </summary>
        //private void DisposeBindingContext(Page targetPage)
        //{
        //    if (targetPage?.BindingContext is ViewModelBase)
        //    {
        //        ((ViewModelBase)targetPage.BindingContext).Dispose();
        //    }
        //}

        //public void DisposeStack()
        //{
        //    if (Application.Current.MainPage is BoxNavigationPageView mainPage)
        //    {
        //        mainPage.Navigation.NavigationStack.ForEach(page => DisposeBindingContext(page));
        //    }
        //}
    }
}