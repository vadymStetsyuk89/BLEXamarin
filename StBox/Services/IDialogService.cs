using System;
using System.Threading.Tasks;

namespace StBox.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);

        Task ToastAsync(string message, string actionName, Action action);

        Task ToastAsync(string message);
    }
}
