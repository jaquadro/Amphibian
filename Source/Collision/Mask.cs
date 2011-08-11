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
        internal PointFP _pos;

        public abstract bool TestOverlap (Mask mask);

        //public abstract TestResult TestOverlapExt (Mask mask);
        
        public PointFP Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public abstract RectangleFP Bounds { get; }

        public abstract object Clone ();

        public virtual void Draw (SpriteBatch spriteBatch) { }
    }
}
