using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Drawing;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public class AYLineMask : Mask
    {
        internal PointFP _p;
        internal FPInt _h;

        public AYLineMask (PointFP p, FPInt h)
        {
            _type = MaskType.AYLine;

            if (h > 0) {
                _p = p;
                _h = h;
            }
            else {
                _p = new PointFP(p.X, p.Y - h);
                _h = 0 - h;
            }
        }

        public override object Clone ()
        {
            AYLineMask mask = new AYLineMask(_p, _h);
            mask._pos = _pos;

            return mask;
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            VectorFP a = (VectorFP)_pos + _p;
            VectorFP b = (VectorFP)_pos + new VectorFP(_p.X, _p.Y + _h);

            Primitives2D.DrawLine(spriteBatch, (float)a.X, (float)a.Y, (float)b.X, (float)b.Y, Color.White);
        }

        public override bool TestOverlap (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return Collision.TestOverlap(mask as PointMask, this);
                case MaskType.Circle:
                    return Collision.TestOverlap(mask as CircleMask, this);
                case MaskType.AXLine:
                    return Collision.TestOverlap(mask as AXLineMask, this);
                case MaskType.AYLine:
                    return Collision.TestOverlap(this, mask as AYLineMask);
                case MaskType.Line:
                    return Collision.TestOverlap(this, mask as LineMask);
                case MaskType.AABB:
                    return Collision.TestOverlap(this, mask as AABBMask);
                case MaskType.Triangle:
                    return Collision.TestOverlap(this, mask as TriangleMask);
                case MaskType.Composite:
                    return Collision.TestOverlap(this, mask as CompositeMask);
            }

            return false;
        }

        public override bool TestOverlapEdge (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return Collision.TestOverlap(mask as PointMask, this);
                case MaskType.Circle:
                    return Collision.TestOverlap(mask as CircleMask, this);
                case MaskType.AXLine:
                    return Collision.TestOverlap(mask as AXLineMask, this);
                case MaskType.AYLine:
                    return Collision.TestOverlap(this, mask as AYLineMask);
                case MaskType.Line:
                    return Collision.TestOverlap(this, mask as LineMask);
                case MaskType.AABB:
                    return Collision.TestOverlap(this, mask as AABBMask);
                case MaskType.Triangle:
                    return Collision.TestOverlap(this, mask as TriangleMask);
                case MaskType.Composite:
                    return Collision.TestOverlap(this, mask as CompositeMask);
            }

            return false;
        }

        public override RectangleFP Bounds
        {
            get { return new RectangleFP(_pos.X + _p.X, _pos.Y + _p.Y, 0, _h); }
        }

        public override bool TestOverlapEdge (FPInt x, FPInt y)
        {
            return Collision.TestOverlapEdge(this, x, y);
        }

        internal PointFP ClosestPoint (PointFP p)
        {
            FPInt px = _pos.X + _p.X;
            FPInt py1 = _pos.Y + _p.Y;
            FPInt py2 = py1 + _h;

            if (p.Y <= py1) {
                return _p;
            }
            else if (p.Y >= py2) {
                return new PointFP(px, py2);
            }

            return new PointFP(px, p.Y);
        }

        // Implicit equation for a line:
        //   (x2 - x1) * F(x, y) = (x2 - x1)y - (y2 - y1)x - (x2 * y1 - x1 * y2)
        // Since we only care about sign and not value, we do not need to divide by (x2 - x1)

        internal bool IntersectsLine (PointFP c, PointFP d)
        {
            FPInt px = _pos.X + _p.X;
            FPInt py1 = _pos.Y + _p.Y;
            FPInt py2 = py1 + _h;

            // Check that given line is on both sides of AYLine
            FPLong xf = py2 - py1;
            FPLong t = px * xf;

            FPLong f1 = xf * c.X - t;
            FPLong f2 = xf * d.X - t;

            if (f1 * f2 >= 0) {
                return false;
            }

            // Check that AYLine is on both sides of given line
            FPLong yf = d.X - c.X;
            t = (d.Y - c.Y) * px + (d.X * c.Y - c.X * d.Y);

            f1 = yf * py1 - t;
            f2 = yf * py2 - t;

            return (f1 * f2 < 0);
        }
    }
}
