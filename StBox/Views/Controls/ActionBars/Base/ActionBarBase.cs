using Xamarin.Forms;

namespace StBox.Views.Controls.ActionBars.Base
{
    public class ActionBarBase : ContentView
    {
        private static readonly string _BINDING_CONTEXT_SOURCE_PATH = "ActionBarViewModel";

        public ActionBarBase()
        {
            SetBinding(BindingContextProperty, new Binding(_BINDING_CONTEXT_SOURCE_PATH));
        }
    }
}
