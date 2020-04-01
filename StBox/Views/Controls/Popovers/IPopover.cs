using System.Collections;

namespace StBox.Views.Controls.Popovers
{
    public interface IPopover
    {
        bool IsPopoverVisible { get; set; }

        IList ItemContext { get; set; }

        object SelectedItem { get; set; }

        string HintText { get; set; }

        bool IsHaveSameWidth { get; set; }
    }
}
