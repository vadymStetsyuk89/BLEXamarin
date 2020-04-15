using System;
using System.Collections.Generic;
using System.Text;

namespace StBox.AppLocalState
{
    public abstract class StateReducer : IStateReducer
    {
        public abstract string Key { get; protected set; }
    }
}
