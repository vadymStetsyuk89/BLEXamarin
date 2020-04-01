using System;

namespace StBox.Views.Contracts
{
    public interface IPopupContext
    {
        Type RelativeViewType { get; }

        string Title { get; }
    }
}
