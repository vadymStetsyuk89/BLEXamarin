using StBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StBox.Services
{
    public interface INavigationService
    {
        bool IsBackButtonVisible { get; }

        ViewModelBase PreviousPageViewModel { get; }

        ViewModelBase LastPageViewModel { get; }

        IReadOnlyCollection<ViewModelBase> CurrentViewModelsNavigationStack { get; }

        void Initialize(Type initPageVMType);

        //void DisposeStack();

        Task NavigateToAsync(Type navigateTo, object parameter = null);

        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;

        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();

        Task GoBackAsync(object arguments = null);
    }
}
