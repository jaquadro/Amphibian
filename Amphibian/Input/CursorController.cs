using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Input
{
    public abstract class CursorController<TActionSet> : ButtonController<TActionSet>
        where TActionSet : struct
    {
        private int _prevX;
        private int _prevY;

        public abstract bool VirtualInterface { get; }

        public abstract int X { get; }

        public abstract int Y { get; }

        public abstract bool LocationValid { get; }

        public virtual int DeltaX
        {
            get { return X - _prevX; }
        }

        public virtual int DeltaY
        {
            get { return Y - _prevY; }
        }

        public override void Refresh()
        {
            _prevX = X;
            _prevY = Y;
        }
    }
}
