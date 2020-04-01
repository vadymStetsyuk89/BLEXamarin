using Android.Content;
using StBox.Android.Renderers;
using StBox.Views.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(EditorExtended), typeof(EditorExtendedRenderer))]
namespace StBox.Android.Renderers
{
    public class EditorExtendedRenderer : EditorRendererBase
    {
        public EditorExtendedRenderer(Context context)
            : base(context)
        {
        }
    }
}