using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Enumerations
{
    public abstract class StateData
    {
        public enum State
        {
            Insert = 0,
            Delete = 1,
            Update = 2,
            DoNothing = 3
        }
        public State state { get; set; }
    }
}
