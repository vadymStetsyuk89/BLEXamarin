using StBox.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsBox.AppEnvironment.Arguments.Navigation;
using XamarinFormsBox.DependencyServices;
using XamarinFormsBox.DependencyServices.Contracts;

namespace XamarinFormsBox.ViewModels.Authentication
{
    public class RegisterPageViewModel : ContentPageBaseViewModel
    {
        public RegisterPageViewModel()
        {
            ResetForm();
        }

        public ICommand OnRegisterCommand => new Command(async () =>
        {
            if (IsFormValid())
            {
                OnFireBaseCommandResult<string> authResult = await DependencyService.Get<IFireAuthDepService>().RegisterNewUserAsync(EmailInput, PasswordInput);

                if (authResult.IsSuccessful)
                {
                    string commandResultMessage = "User registered successfully.";

                    OnFireBaseCommandResult emailVerificationResult = await DependencyService.Get<IFireAuthDepService>().UserEmailVerification(authResult.Payload);

                    if (emailVerificationResult.IsSuccessful)
                    {
                        commandResultMessage = $"{commandResultMessage} Verification link was sent to {EmailInput} email";
                    }
                    else
                    {
                        commandResultMessage = $"{commandResultMessage} Can't send verification link to email {EmailInput}.";
                        System.Console.WriteLine($"{commandResultMessage}. ERROR details: {emailVerificationResult.Message}");
                    }

                    await NavigationService.NavigateToAsync<LogInPageViewModel>(new NavigatedInfoMessageArgs(commandResultMessage));
                }
                else
                {
                    await DialogService.ToastAsync(authResult.Message);
                }
            }
            else
            {
                await DialogService.ToastAsync("Form is not valid");
            }
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

        private string _confirmPasswordInput;
        public string ConfirmPasswordInput
        {
            get => _confirmPasswordInput;
            set => SetProperty<string>(ref _confirmPasswordInput, value);
        }

        public override void Dispose()
        {
            base.Dispose();

            ResetForm();
        }

        private bool IsFormValid() => !string.IsNullOrEmpty(EmailInput)
            && !string.IsNullOrEmpty(PasswordInput)
            && !string.IsNullOrEmpty(ConfirmPasswordInput)
            && PasswordInput.Equals(ConfirmPasswordInput);

        private void ResetForm()
        {
            EmailInput = string.Empty;
            PasswordInput = string.Empty;
            ConfirmPasswordInput = string.Empty;

#if DEBUG
            EmailInput = "@mailinator.com";
            PasswordInput = "catduck";
            ConfirmPasswordInput = "catduck";
#endif
        }
    }
}
