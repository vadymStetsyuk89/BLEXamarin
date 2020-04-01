using Android.Content;
using Android.Views;
using StBox.Android.Renderers;
using StBox.Views.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryExtended), typeof(EntryExtendedRenderer))]
namespace StBox.Android.Renderers
{
    public class EntryExtendedRenderer : EntryRenderer
    {
        private BorderRenderer _renderer;

        private readonly Context _context;

        private const GravityFlags DefaultGravity = GravityFlags.CenterVertical;

        public EntryExtendedRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;
            Control.Gravity = DefaultGravity;

            EntryExtended entryExtended = Element as EntryExtended;
            UpdateBackground(entryExtended);
            UpdatePadding(entryExtended);
            UpdateTextAlighnment(entryExtended);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Element == null)
                return;
            EntryExtended entryEx = Element as EntryExtended;
            if (e.PropertyName == EntryExtended.BorderWidthProperty.PropertyName ||
                e.PropertyName == EntryExtended.BorderColorProperty.PropertyName ||
                e.PropertyName == EntryExtended.BorderRadiusProperty.PropertyName ||
                e.PropertyName == EntryExtended.BackgroundColorProperty.PropertyName)
            {
                UpdateBackground(entryEx);
            }
            else if (e.PropertyName == EntryExtended.LeftPaddingProperty.PropertyName ||
                e.PropertyName == EntryExtended.RightPaddingProperty.PropertyName)
            {
                UpdatePadding(entryEx);
            }
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
            {
                UpdateTextAlighnment(entryEx);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_renderer != null)
                {
                    _renderer.Dispose();
                    _renderer = null;
                }
            }
        }

        private void UpdateBackground(EntryExtended entryEx)
        {
            if (_renderer != null)
            {
                _renderer.Dispose();
                _renderer = null;
            }
            _renderer = new BorderRenderer();

            Control.Background = _renderer.GetBorderBackground(entryEx.BorderColor, entryEx.BackgroundColor, entryEx.BorderWidth, entryEx.BorderRadius);
        }

        private void UpdatePadding(EntryExtended entryEx)
        {
            Control.SetPadding((int)_context.ToPixels(entryEx.LeftPadding), 0,
                (int)_context.ToPixels(entryEx.RightPadding), 0);
        }

        private void UpdateTextAlighnment(EntryExtended entryEx)
        {
            var gravity = DefaultGravity;
            switch (entryEx.HorizontalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Start:
                    gravity |= GravityFlags.Start;
                    break;
                case Xamarin.Forms.TextAlignment.Center:
                    gravity |= GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    gravity |= GravityFlags.End;
                    break;
            }
            Control.Gravity = gravity;
        }
    }
}