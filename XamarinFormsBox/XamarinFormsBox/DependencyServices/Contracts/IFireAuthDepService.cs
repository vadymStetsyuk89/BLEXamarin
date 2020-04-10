using System.Threading.Tasks;

namespace XamarinFormsBox.DependencyServices.Contracts
{
    public interface IFireAuthDepService
    {
        Task<OnFireBaseCommandResult> UserEmailVerification(string firebaseUserUId);

        Task<OnFireBaseCommandResult<string>> RegisterNewUserAsync(string email, string password);

        Task<OnFireBaseCommandResult<string>> SignInAsync(string email, string password);
    }
}
