using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Drawing;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public class AXLineMask : Mask
    {
        internal PointFP _p;
        internal FPInt _w;

        public AXLineMask (PointFP p, FPInt w)
        {
            _type = MaskType.AXLine;

            if (w > 0) {
                _p = p;
                _w = w;
            }
            else {
                _p = new PointFP(p.X - w, p.Y);
                _w = 0 - w;
            }
        }

        public override object Clone ()
        {
            AXLineMask mask = new AXLineMask(_p, _w);
            mask._pos = _pos;

            return mask;
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            VectorFP a = (VectorFP)_pos + _p;
            VectorFP b = (VectorFP)_pos + new VectorFP(_p.X + _w, _p.Y);

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
                    return Collision.TestOverlap(this, mask as AXLineMask);
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
                    return Collision.TestOverlap(this, mask as AXLineMask);
                case MaskType.AYLine:
                    return Collision.TestOverlap(this, mask as AYLineMask);
                case MaskType.Line:
                    return Collision.TestOverlap(this, mask as LineMask);
                case MaskType.AABB:
                    return Collision.TestOverlapEdge(this, mask as AABBMask);
                case MaskType.Triangle:
                    return Collision.TestOverlapEdge(this, mask as TriangleMask);
                case MaskType.Composite:
                    return Collision.TestOverlap(this, mask as CompositeMask);
            }

            return false;
        }

        public override RectangleFP Bounds
        {
            get { return new RectangleFP(_pos.X + _p.X, _pos.Y + _p.Y, _w, 0); }
        }

        public override bool TestOverlapEdge (FPInt x, FPInt y)
        {
            return Collision.TestOverlapEdge(this, x, y);
        }

        /*public override TestResult TestOverlapExt (Mask mask)
        {
            return TestResult.None;
        }*/

        internal PointFP ClosestPoint (PointFP p)
        {
            FPInt py = _pos.Y + _p.Y;
            FPInt px1 = _pos.X + _p.X;
            FPInt px2 = px1 + _w;

            if (p.X <= px1) {
                return _p;
            }
            else if (p.X >= px2) {
                return new PointFP(px2, py);
            }

            return new PointFP(p.X, py);
        }

        // Implicit equation for a line:
        //   (x2 - x1) * F(x, y) = (x2 - x1)y - (y2 - y1)x - (x2 * y1 - x1 * y2)
        // Since we only care about sign and not value, we do not need to divide by (x2 - x1)

        internal bool IntersectsLine (PointFP c, PointFP d)
        {
            FPInt py = _pos.Y + _p.Y;
            FPInt px1 = _pos.X + _p.X;
            FPInt px2 = px1 + _w;

            // Check that given line is on both sides of AXLine
            FPLong yf = px2 - px1;
            FPLong t = py * yf;

            FPLong f1 = yf * c.Y - t;
            FPLong f2 = yf * d.Y - t;

            if (f1 * f2 >= 0) {
                return false;
            }

            // Check that AXLine is on both sides of given line
            FPLong xf = d.Y - c.Y;
            t = (d.X - c.X) * py - (d.X * c.Y - c.X * d.Y);

            f1 = xf * px1 - t;
            f2 = xf * px2 - t;

            return (f1 * f2 < 0);
        }

        internal bool IntersectsLineEdge (PointFP c, PointFP d)
        {
            FPInt py = _pos.Y + _p.Y;
            FPInt px1 = _pos.X + _p.X;
            FPInt px2 = px1 + _w;

            // Check that given line is on both sides of AXLine
            FPLong yf = px2 - px1;
            FPLong t = py * yf;

            FPLong f1 = yf * c.Y - t;
            FPLong f2 = yf * d.Y - t;

            if (f1 * f2 > 0) {
                return false;
            }

            // Check that AXLine is on both sides of given line
            FPLong xf = d.Y - c.Y;
            t = (d.X - c.X) * py - (d.X * c.Y - c.X * d.Y);

            f1 = xf * px1 - t;
            f2 = xf * px2 - t;

            return (f1 * f2 <= 0);
        }
    }
}
