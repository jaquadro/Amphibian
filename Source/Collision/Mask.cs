using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public enum MaskType
    {
        Point,
        Circle,
        AXLine,
        AYLine,
        Line,
        AABB,
        Triangle,
        Composite
    }

    public abstract class Mask : ICloneable
    {
        internal MaskType _type;
        internal SharedPointFP _pos;

        protected Mask ()
        {
            _pos = new SharedPointFP(0, 0);
        }

        public abstract bool TestOverlap (Mask mask);

        public virtual bool TestOverlap (FPInt x, FPInt y)
        {
            return TestOverlap(new PointMask(new PointFP(x, y)));
        }

        //public abstract TestResult TestOverlapExt (Mask mask);
        
        public SharedPointFP Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public void Offset (FPInt offsetX, FPInt offsetY)
        {
            _pos.X += offsetX;
            _pos.Y += offsetY;
        }

        public abstract RectangleFP Bounds { get; }

        public abstract object Clone ();

        public virtual void Draw (SpriteBatch spriteBatch) { }
    }
}
