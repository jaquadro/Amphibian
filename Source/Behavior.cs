using System;
using System.Collections.Generic;

namespace Amphibian
{
    public abstract class Behavior : LogicGroup
    {
        protected Behavior ()
            : base()
        {
        }
    }

    public abstract class InterpBehavior : Behavior
    {
        protected InterpBehavior ()
            : base()
        {
        }

        public abstract void Interpolate (double alpha);
    }
}
