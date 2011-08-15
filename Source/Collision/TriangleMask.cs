using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amphibian.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public class TriangleMask : Mask
    {
        internal PointFP _p0;
        internal PointFP _p1;
        internal PointFP _p2;
        internal FPInt _det;
        internal RectangleFP _bound;

        public TriangleMask (PointFP p0, PointFP p1, PointFP p2)
        {
            _type = MaskType.Triangle;
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;

            VectorFP a = (VectorFP)_pos + _p0;
            VectorFP b = (VectorFP)_pos + _p1;
            VectorFP c = (VectorFP)_pos + _p2;

            _det = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            //_det = 1 / _det;

            FPInt minx = FPMath.Min(_p0.X, FPMath.Min(_p1.X, _p2.X));
            FPInt maxx = FPMath.Max(_p0.X, FPMath.Max(_p1.X, _p2.X));
            FPInt miny = FPMath.Min(_p0.Y, FPMath.Min(_p1.Y, _p2.Y));
            FPInt maxy = FPMath.Max(_p0.Y, FPMath.Max(_p1.Y, _p2.Y));

            _bound = new RectangleFP(minx, miny, maxx - minx, maxy - miny);
        }

        public override object Clone ()
        {
            TriangleMask mask = new TriangleMask(_p0, _p1, _p2);
            mask._pos = _pos;

            return mask;
        }

        public override RectangleFP Bounds
        {
            get { return _bound; }
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            VectorFP a = (VectorFP)_pos + _p0;
            VectorFP b = (VectorFP)_pos + _p1;
            VectorFP c = (VectorFP)_pos + _p2;

            Primitives2D.DrawLine(spriteBatch, (float)a.X, (float)a.Y, (float)b.X, (float)b.Y, Color.White);
            Primitives2D.DrawLine(spriteBatch, (float)a.X, (float)a.Y, (float)c.X, (float)c.Y, Color.White);
            Primitives2D.DrawLine(spriteBatch, (float)b.X, (float)b.Y, (float)c.X, (float)c.Y, Color.White);
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
                    return Collision.TestOverlap(mask as AYLineMask, this);
                case MaskType.Line:
                    return Collision.TestOverlap(mask as LineMask, this);
                case MaskType.AABB:
                    return Collision.TestOverlap(mask as AABBMask, this);
                case MaskType.Triangle:
                    return Collision.TestOverlap(this, mask as TriangleMask);
                case MaskType.Composite:
                    return Collision.TestOverlap(this, mask as CompositeMask);
            }

            return false;
        }

        public override bool TestOverlap (FPInt x, FPInt y)
        {
            return Collision.TestOverlap(this, x, y);
        }

        /*public override TestResult TestOverlapExt (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return CollisionTR.TestOverlap(mask as PointMask, this);
                case MaskType.Line:
                    return CollisionTR.TestOverlap(mask as LineMask, this);
                case MaskType.Circle:
                    return CollisionTR.TestOverlap(mask as CircleMask, this);
                case MaskType.AABB:
                    return CollisionTR.TestOverlap(mask as AABBMask, this);
                case MaskType.Triangle:
                    return CollisionTR.TestOverlap(this, mask as TriangleMask);
            }

            return TestResult.None;
        }

        public TestResult TestOverlapExt (PointMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (LineMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (CircleMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (AABBMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (TriangleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }*/

        internal PointFP Barycentric (PointFP p)
        {
            //Vector2 a = _pos + _p0;
            //Vector2 b = _pos + _p1;
            //Vector2 c = _pos + _p2;

            PointFP a = _p0;
            PointFP b = _p1;
            PointFP c = _p2;
            p = (VectorFP)p - _pos;

            FPInt v = (b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y);
            FPInt u = (c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y);

            return new PointFP(v / _det, u / _det);
        }

        internal PointFP ClosestPoint (PointFP p)
        {
            VectorFP a = (VectorFP)_pos + _p0;
            VectorFP b = (VectorFP)_pos + _p1;
            VectorFP c = (VectorFP)_pos + _p2;

            // Check if P in region outside A
            VectorFP ab = b - a;
            VectorFP ac = c - a;
            VectorFP ap = p - a;
            FPInt d1 = VectorFP.Dot(ab, ap);
            FPInt d2 = VectorFP.Dot(ac, ap);
            if (d1 <= 0 && d2 <= 0) {
                return a;
            }

            // Check if P in region outside B
            VectorFP bp = p - b;
            FPInt d3 = VectorFP.Dot(ab, bp);
            FPInt d4 = VectorFP.Dot(ac, bp);
            if (d3 >= 0 && d4 <= d3) {
                return b;
            }

            // Check if P in edge region of AB
            FPInt vc = d1 * d4 - d3 * d2;
            if (vc <= 0 && d1 >= 0 && d3 <= 0) {
                FPInt v = d1 / (d1 - d3);
                return a + v * ab;
            }

            // Check if P in region outside C
            VectorFP cp = p - c;
            FPInt d5 = VectorFP.Dot(ab, cp);
            FPInt d6 = VectorFP.Dot(ac, cp);
            if (d6 >= 0 && d5 <= d6) {
                return c;
            }

            // Check if P in edge region of AC
            FPInt vb = d5 * d2 - d1 * d6;
            if (vb <= 0 && d2 >= 0 && d6 <= 0) {
                FPInt w = d2 / (d2 - d6);
                return a + w * ac;
            }

            // Check if P in edge region of BC
            FPInt va = d3 * d6 - d5 * d4;
            if (va <= 0 && (d4 - d3) >= 0 && (d5 - d6) >= 0) {
                FPInt w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + w * (c - b);
            }

            // P inside region
            return p;
        }
    }
}
