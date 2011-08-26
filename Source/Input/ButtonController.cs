using System;

namespace Amphibian.Input
{
    public abstract class ButtonController<TActionSet> : InputController 
        where TActionSet : struct
    {
        public abstract bool ButtonPressed (TActionSet action);

        public abstract bool ButtonReleased (TActionSet action);

        public abstract bool ButtonHeld (TActionSet action);
    }
}
