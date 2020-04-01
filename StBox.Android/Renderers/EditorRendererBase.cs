using Android.Content;
using Android.Graphics.Drawables;
using StBox.Android.Environment.Helpers;
using StBox.Environment;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace StBox.Android.Renderers
{
    public abstract class EditorRendererBase : EditorRenderer
    {
        public EditorRendererBase(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                RemoveUnderscore();
            }
        }

        private void RemoveUnderscore()
        {
            if (Control != null && Element != null)
            {
                Control.Background = new ColorDrawable(BaseSingleton<ValuesNormalizer>.Instance.ResolveNativeColor(Element.BackgroundColor));
            }
        }
    }
}