using StBox.Views.Controls.Popovers.Arguments;

namespace StBox.Views.Controls.Popovers
{
    public interface IPopoverKeeper
    {
        void ShowPopover(IPopover popiver, ShowPopoverArgs showPopoverArgs);

        void HidePopover(IPopover popover);

        void HideAllPopovers();
    }
}
