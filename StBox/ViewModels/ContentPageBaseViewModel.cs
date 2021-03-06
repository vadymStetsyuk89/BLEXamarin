﻿
using StBox.ViewModels.Contracts;
using StBox.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace StBox.ViewModels
{
    public abstract class ContentPageBaseViewModel : PageNavigationBaseViewModel, 
        IHandlePageVisualCycles
    {
        private Dictionary<Guid, bool> _busySequence = new Dictionary<Guid, bool>();

        public ContentPageBaseViewModel() { }

        private ObservableCollection<IPopupContext> _popups = new ObservableCollection<IPopupContext>();
        public ObservableCollection<IPopupContext> Popups
        {
            get => _popups;
            private set => SetProperty<ObservableCollection<IPopupContext>>(ref _popups, value);
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get => _refreshCommand;
            protected set => SetProperty<ICommand>(ref _refreshCommand, value);
        }

        private string _appBackgroundImage;
        public string AppBackgroundImage
        {
            get => _appBackgroundImage;
            protected set => SetProperty<string>(ref _appBackgroundImage, value);
        }

        private bool _isPullToRefreshEnabled;
        public bool IsPullToRefreshEnabled
        {
            get => _isPullToRefreshEnabled;
            protected set => SetProperty<bool>(ref _isPullToRefreshEnabled, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty<bool>(ref _isRefreshing, value);
        }

        private bool _isMenuVisible;
        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set => SetProperty<bool>(ref _isMenuVisible, value);
        }

        private bool _isPopupsVisible;
        public bool IsPopupsVisible
        {
            get => _isPopupsVisible;
            set => SetProperty<bool>(ref _isPopupsVisible, value);
        }

        private IActionbarViewModel _actionBarViewModel;
        public IActionbarViewModel ActionBarViewModel
        {
            get => _actionBarViewModel;
            protected set
            {
                _actionBarViewModel?.Dispose();

                SetProperty(ref _actionBarViewModel, value);
            }
        }

        public override Task InitializeAsync_NEED_TO_DEFINE_LC(object navigationData)
        {
            ActionBarViewModel?.InitializeAsync(navigationData);

            return base.InitializeAsync_NEED_TO_DEFINE_LC(navigationData);
        }

        public override void Dispose()
        {
            base.Dispose();

            ActionBarViewModel?.Dispose();
        }

        protected async void ExecuteActionWithBusy(Func<Task> asyncFunc)
        {
            if (asyncFunc == null)
            {
                Debugger.Break();
                return;
            }

            Guid busyKey = Guid.NewGuid();
            SetBusy(busyKey, true);

            try
            {
                await asyncFunc();
            }
            catch (Exception exc)
            {
                Debugger.Break();
                Console.WriteLine($"{typeof(ContentPageBaseViewModel).FullName} at ExecuteActionWithBusy. Details: {exc.Message}");
            }

            SetBusy(busyKey, false);
        }

        protected async void ExecuteActionWithBusy<TParameter>(Func<TParameter, Task> asyncFunc, TParameter parameter)
        {
            if (asyncFunc == null)
            {
                Debugger.Break();
                return;
            }

            Guid busyKey = Guid.NewGuid();
            SetBusy(busyKey, true);

            try
            {
                await asyncFunc(parameter);
            }
            catch (Exception exc)
            {
                Debugger.Break();
                Console.WriteLine($"{typeof(ContentPageBaseViewModel).FullName} at ExecuteActionWithBusy. Details: {exc.Message}");
            }

            SetBusy(busyKey, false);
        }

        public void SetBusy(Guid guidKey, bool isBusy)
        {
            if (_busySequence.ContainsKey(guidKey))
            {
                _busySequence[guidKey] = isBusy;
            }
            else
            {
                _busySequence.Add(guidKey, isBusy);
            }

            try
            {
                _busySequence.Where(keyValue => !keyValue.Value).Select(keyValue => keyValue.Key).ToArray().ForEach(guid => _busySequence.Remove(guid));

                IsBusy = _busySequence.Any();
            }
            catch (Exception exc)
            {
                Debugger.Break();

                _busySequence = new Dictionary<Guid, bool>();
                IsBusy = false;
            }
        }

        void IHandlePageVisualCycles.OnPageDisappearing()
        {
            /// TODO: add virtual methods
            /// TODO: (probably) define approach of informing nested parts about `visual cycles`
        }

        void IHandlePageVisualCycles.OnPageAppearing()
        {
            /// TODO: add virtual methods
            /// TODO: (probably) define approach of informing nested parts about `visual cycles`
        }
    }
}
