using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        internal Vector2 _pos;

        public abstract bool TestOverlap (Mask mask);

        public abstract TestResult TestOverlapExt (Mask mask);
        
        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public abstract BoundingRectangle Bounds { get; }

        public abstract object Clone ();

        public virtual void Draw (SpriteBatch spriteBatch) { }
    }
}
