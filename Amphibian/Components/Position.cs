using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.EntitySystem;
using Amphibian.Geometry;
using Microsoft.Xna.Framework;

namespace Amphibian.Components
{
    public sealed class Position : IComponent
    {
        private FPInt _x;
        private FPInt _y;
        private FPInt _prevX;
        private FPInt _prevY;

        public Position ()
        {
        }

        public Position (FPInt x, FPInt y)
        {
            _x = x;
            _y = y;
            _prevX = x;
            _prevY = y;
        }

        public PointFP Location
        {
            get { return new PointFP(_x, _y); }
        }

        public FPInt PreviousX
        {
            get { return _prevX; }
        }

        public FPInt PreviousY
        {
            get { return _prevY; }
        }

        public FPInt X
        {
            get { return _x; }
            set
            {
                _prevX = _x;
                _x = value;
            }
        }

        public FPInt Y
        {
            get { return _y; }
            set
            {
                _prevY = _y;
                _y = value;
            }
        }

        public Microsoft.Xna.Framework.Vector2 ToVector2()
        {
            return new Vector2((float)this.X, (float)this.Y);
        }
    }
}
