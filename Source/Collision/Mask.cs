using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;
using Amphibian.Drawing;

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

        private static Brush _brush;

        protected static Brush GetBrush (SpriteBatch spriteBatch)
        {
            if (_brush == null) {
                _brush = new SolidColorBrush(spriteBatch.GraphicsDevice, Color.White);
            }
            return _brush;
        }

        protected Mask ()
        {
            _pos = new SharedPointFP(0, 0);
        }

        public abstract bool TestOverlap (Mask mask);

        public abstract bool TestOverlapEdge (Mask mask);

        public virtual bool TestOverlap (FPInt x, FPInt y)
        {
            return TestOverlap(new PointMask(new PointFP(x, y)));
        }

        public virtual bool TestOverlapEdge (FPInt x, FPInt y)
        {
            return TestOverlap(x, y);
        }

        public virtual bool TestOverlap (AXLine line) { return false; }

        public virtual bool TestOverlapEdge (AXLine line) { return false; }

        public virtual bool TestOverlap (AYLine line) { return false; }

        public virtual bool TestOverlapEdge (AYLine line) { return false; }

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

        public virtual void Draw (SpriteBatch spriteBatch, Pen pen) { }

        public virtual void Draw (SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Pens.White);
        }
    }
}
