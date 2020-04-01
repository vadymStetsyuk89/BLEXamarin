using StBox.Services;
using StBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsBox.ViewModels
{
    public class MainPageViewModel : ContentPageBaseViewModel
    {
        private readonly IDialogService _FOO_dialogService;

        public MainPageViewModel(IDialogService dialogService)
        {
            _FOO_dialogService = dialogService;
        }

        public ICommand FooCommand => new Command(async () =>
        {
            await DialogService.ToastAsync("Hello world");
        });

        public ICommand OnButtonControlCommand => new Command(() =>
        {
            IsBusy = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                IsBusy = false;

                StackListTestSrc = new List<string>() { "Hello", "Beautiful", "World" };

                return false;
            });
        });

        public ICommand RefreshCommand => new Command(() =>
        {
            IsRefreshing = true;

            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                IsRefreshing = false;

                return false;
            });
        });

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty<bool>(ref _isRefreshing, value);
        }

        private List<string> _stackListTestSrc;
        public List<string> StackListTestSrc
        {
            get => _stackListTestSrc;
            set => SetProperty<List<string>>(ref _stackListTestSrc, value);
        }
    }
}
