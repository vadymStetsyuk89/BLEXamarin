using Firebase;
using Firebase.Auth;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFormsBox.DependencyServices;
using XamarinFormsBox.DependencyServices.Contracts;
using XamarinFormsBox.Droid.DependencyServices;

[assembly: Dependency(typeof(FireAuthDepService))]
namespace XamarinFormsBox.Droid.DependencyServices
{
    public class FireAuthDepService : IFireAuthDepService
    {
        private const string SUCCESSFUL_MESSAGE = "Successful";
        private const string ERROR_MESSAGE = "Error";

        private FirebaseAuth _firebaseAuth;

        public FireAuthDepService()
        {
            _firebaseAuth = FirebaseAuth.GetInstance(FirebaseApp.Instance);
            _firebaseAuth.AuthState += OnFirebaseAuthAuthState;
        }

        private void OnFirebaseAuthAuthState(object sender, FirebaseAuth.AuthStateEventArgs e)
        {

        }

        public Task<OnFireBaseCommandResult> UserEmailVerification(string firebaseUserUId) =>
            Task<OnFireBaseCommandResult>.Run(async () =>
            {
                OnFireBaseCommandResult result = null;

                if (_firebaseAuth.CurrentUser != null)
                {
                    if (_firebaseAuth.CurrentUser.Uid.Equals(firebaseUserUId))
                    {
                        try
                        {
                            await _firebaseAuth.CurrentUser.SendEmailVerificationAsync(null);
                            result = new OnFireBaseCommandResult(true, SUCCESSFUL_MESSAGE);
                        }
                        catch (System.Exception exc)
                        {
                            result = new OnFireBaseCommandResult(false, exc.Message);
                        }
                    }
                    else
                    {
                        /// TODO:
                        /// 
                        Debugger.Break();
                        result = new OnFireBaseCommandResult(false, "Provided UId does not match with UId of logged firebase user.");
                    }
                }
                else
                {
                    /// TODO:
                    /// 
                    Debugger.Break();
                    result = new OnFireBaseCommandResult(false, "There is no logged firebase user.");
                }

                return result;
            });

        public Task<OnFireBaseCommandResult<string>> RegisterNewUserAsync(string email, string password) =>
            Task<OnFireBaseCommandResult<string>>.Run(async () =>
            {
                OnFireBaseCommandResult<string> result = null;

                try
                {
                    IAuthResult authResult = await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
                    result = new OnFireBaseCommandResult<string>(authResult.User.Uid, true, SUCCESSFUL_MESSAGE);
                }
                catch (System.Exception exc)
                {
                    return new OnFireBaseCommandResult<string>(string.Empty, false, exc.Message);
                }

                return result;
            });

        public Task<OnFireBaseCommandResult<string>> SignInAsync(string email, string password) =>
            Task<OnFireBaseCommandResult<string>>.Run(async () =>
            {
                OnFireBaseCommandResult<string> result = null;

                try
                {
                    IAuthResult authResult = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                    result = new OnFireBaseCommandResult<string>(authResult.User.Uid, true, SUCCESSFUL_MESSAGE);
                }
                catch (System.Exception exc)
                {
                    return new OnFireBaseCommandResult<string>(string.Empty, false, exc.Message);
                }

                return result;
            });
    }
}