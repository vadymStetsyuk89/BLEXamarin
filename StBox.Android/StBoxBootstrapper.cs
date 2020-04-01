using Acr.UserDialogs;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms.Platform.Android;

namespace StBox.Android
{
    public static class StBoxBootstrapper
    {
        public static void Init(FormsAppCompatActivity activity)
        {
            UserDialogs.Init(activity);
            CachedImageRenderer.Init(true);
        }
    }
}