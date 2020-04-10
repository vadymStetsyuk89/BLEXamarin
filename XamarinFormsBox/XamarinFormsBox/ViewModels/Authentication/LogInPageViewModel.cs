using StBox.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsBox.AppEnvironment.Arguments.Navigation;
using XamarinFormsBox.DependencyServices;
using XamarinFormsBox.DependencyServices.Contracts;

namespace XamarinFormsBox.ViewModels.Authentication
{
    public class LogInPageViewModel : ContentPageBaseViewModel
    {

        public LogInPageViewModel()
        {
            ResetForm();
        }

        public ICommand OnSignInCommand => new Command(async () =>
        {
            if (IsFormValid())
            {
                OnFireBaseCommandResult authResult = await DependencyService.Get<IFireAuthDepService>().SignInAsync(EmailInput, PasswordInput);

                Debugger.Break();
            }
            else
            {
                await DialogService.ToastAsync("Form is not valid");
            }
        });

        public ICommand OnRegisterCommand => new Command(async () =>
        {
            await NavigationService.NavigateToAsync<RegisterPageViewModel>();
        });

        public ICommand OnForgotPasswordCommand => new Command(() =>
        {
            Debugger.Break();
        });

        public ICommand OnResetVerificationLinkCommand => new Command(() =>
        {
            Debugger.Break();
        });

        private string _emailInput;
        public string EmailInput
        {
            get => _emailInput;
            set => SetProperty<string>(ref _emailInput, value);
        }

        private string _passwordInput;
        public string PasswordInput
        {
            get => _passwordInput;
            set => SetProperty<string>(ref _passwordInput, value);
        }

        private string _infoMessage;
        public string InfoMessage
        {
            get => _infoMessage;
            private set => SetProperty<string>(ref _infoMessage, value);
        }

        public override async Task InitializeAsync_NEED_TO_DEFINE_LC(object navigationData)
        {
            await base.InitializeAsync_NEED_TO_DEFINE_LC(navigationData);

            if (navigationData is NavigatedInfoMessageArgs)
            {
                InfoMessage = ((NavigatedInfoMessageArgs)navigationData).Message;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            ResetForm();
        }

        private bool IsFormValid() => !string.IsNullOrEmpty(EmailInput)
            && !string.IsNullOrEmpty(PasswordInput);

        private void ResetForm()
        {
            EmailInput = string.Empty;
            PasswordInput = string.Empty;
            InfoMessage = string.Empty;

#if DEBUG
            EmailInput = "@mailinator.com";
            PasswordInput = "catduck";
#endif
        }
    }
}
