using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StBox.Environment.Helpers.Extensions
{
    /// <summary>
    /// How to use: in your XAML image - Source="{extensions:ImageResource PeakMVP.Images.ic_logo-colored_2x.png}"
    /// also image source should be provided as `Embeded resource` (take a look to the image src properties in solution explorer).
    /// </summary>
    [ContentProperty("Source")]
    public sealed class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null) return null;

            return ImageSource.FromResource(Source);
        }
    }
}
