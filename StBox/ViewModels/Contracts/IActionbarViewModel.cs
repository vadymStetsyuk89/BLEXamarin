using System.Threading.Tasks;

namespace StBox.ViewModels.Contracts
{
    public interface IActionbarViewModel
    {
        /// <summary>
        /// The idea is that <see cref="IActionbarViewModel"/> is defined as `nested` part of <see cref="ContentPageBaseViewModel"/>,
        /// and this is the reference to the relative `content page VM`.
        /// </summary>
        ContentPageBaseViewModel RelativeContentPageBaseViewModel { get; }

        Task InitializeAsync(object navigationData);

        void Dispose();
    }
}
