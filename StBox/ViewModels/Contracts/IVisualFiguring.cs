using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StBox.ViewModels.Contracts
{
    public interface IVisualFiguring
    {
        Type RelativeViewType { get; }

        string TabHeader { get; }

        void Dispose();

        Task InitializeAsync(object navigationData);
    }
}
