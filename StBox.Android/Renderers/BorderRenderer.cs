using Android.App;
using Android.Graphics.Drawables;
using System;
using Xamarin.Forms.Platform.Android;

namespace StBox.Android.Renderers
{
    class BorderRenderer : IDisposable
    {

        private GradientDrawable _background;

        public Drawable GetBorderBackground(Xamarin.Forms.Color borderColor, Xamarin.Forms.Color backgroundColor, float borderWidth, float borderRadius)
        {
            if (_background != null)
            {
                _background.Dispose();
                _background = null;
            }

            borderWidth = borderWidth > 0 ? borderWidth : 0;
            borderRadius = borderRadius > 0 ? borderRadius : 0;
            borderColor = borderColor != Xamarin.Forms.Color.Default ? borderColor : Xamarin.Forms.Color.Transparent;
            backgroundColor = backgroundColor != Xamarin.Forms.Color.Default ? backgroundColor : Xamarin.Forms.Color.Transparent;

            float strokeWidth = Application.Context.ToPixels(borderWidth);
            float radius = Application.Context.ToPixels(borderRadius);
            _background = new GradientDrawable();
            _background.SetColor(backgroundColor.ToAndroid());

            if (radius > 0)
                _background.SetCornerRadius(radius);

            if (borderColor != Xamarin.Forms.Color.Transparent && strokeWidth > 0)
            {
                _background.SetStroke((int)strokeWidth, borderColor.ToAndroid());
            }

            return _background;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_background != null)
                {
                    _background.Dispose();
                    _background = null;
                }
            }
        }
    }
}