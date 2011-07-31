using System;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public class AXLineMask : Mask
    {
        internal Vector2 _p;
        internal float _w;

        public AXLineMask (Vector2 p, float w)
        {
            _type = MaskType.AXLine;

            if (w > 0) {
                _p = p;
                _w = w;
            }
            else {
                _p = new Vector2(p.X - w, p.Y);
                _w = 0 - w;
            }
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
            }

            return false;
        }

        /*public bool TestOverlap (PointMask mask)
        {
            return false;
        }

        public bool TestOverlap (CircleMask mask)
        {
            Vector2 c1 = mask._pos + mask._p;
            Vector2 c2 = ClosestPoint(c1);

            float d2 = Vector2.DistanceSquared(c1, c2);
            float r2 = mask._radius * mask._radius;

            return (r2 < d2);
        }

        public bool TestOverlap (AXLineMask mask)
        {
            return false;
        }

        //public bool TestOverlap (AYLineMask mask)
        //{

        //}

        public bool TestOverlap (LineMask mask)
        {
            Vector2 c = mask._pos + mask._p0;
            Vector2 d = c + new Vector2(mask._w, mask._h);

            return IntersectsLine(c, d);
        }

        public bool TestOverlap (AABBMask mask)
        {
            float py = _pos.Y + _p.Y;
            float px1 = _pos.X + _p.X;
            float px2 = px1 + _w;

            Vector2 r1 = mask._pos + mask._point;
            Vector2 r2 = r1 + new Vector2(mask._w, mask._h);

            return !(py > r2.Y || py < r1.Y || px1 > r2.X || px2 < r1.X);
        }

        public bool TestOverlap (TriangleMask mask)
        {
            Vector2 a = mask._pos + mask._p0;
            Vector2 b = mask._pos + mask._p0;
            Vector2 c = mask._pos + mask._p0;

            if (IntersectsLine(a, b) || IntersectsLine(b, c) || IntersectsLine(a, c)) {
                return true;
            }

            Vector2 q = mask.Barycentric(_pos + _p);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }*/

        public override TestResult TestOverlapExt (Mask mask)
        {
            /*switch (mask._type) {
                case MaskType.Point:
                    return CollisionTR.TestOverlap(mask as PointMask, this);
                case MaskType.Line:
                    return CollisionTR.TestOverlap(this, mask as LineMask);
                case MaskType.Circle:
                    return CollisionTR.TestOverlap(this, mask as CircleMask);
                case MaskType.AABB:
                    return CollisionTR.TestOverlap(this, mask as AABBMask);
                case MaskType.Triangle:
                    return CollisionTR.TestOverlap(this, mask as TriangleMask);
            }*/

            return TestResult.None;
        }

        /*public TestResult TestOverlap (PointMask mask)
        {
            //return CollisionTR.TestOverlap(mask, this);
            return TestResult.None;
        }

        public TestResult TestOverlap (LineMask mask)
        {
            //return CollisionTR.TestOverlap(this, mask);
            return TestResult.None;
        }

        public TestResult TestOverlap (CircleMask mask)
        {
            //return CollisionTR.TestOverlap(this, mask);
            return TestResult.None;
        }

        public TestResult TestOverlap (AABBMask mask)
        {
            //return CollisionTR.TestOverlap(this, mask);
            return TestResult.None;
        }

        public TestResult TestOverlap (TriangleMask mask)
        {
            //return CollisionTR.TestOverlap(this, mask);
            return TestResult.None;
        }*/

        internal Vector2 ClosestPoint (Vector2 p)
        {
            float py = _pos.Y + _p.Y;
            float px1 = _pos.X + _p.X;
            float px2 = px1 + _w;

            if (p.X <= px1) {
                return _p;
            }
            else if (p.X >= px2) {
                return new Vector2(px2, py);
            }

            return new Vector2(p.X, py);
        }

        // Implicit equation for a line:
        //   (x2 - x1) * F(x, y) = (x2 - x1)y - (y2 - y1)x - (x2 * y1 - x1 * y2)
        // Since we only care about sign and not value, we do not need to divide by (x2 - x1)

        internal bool IntersectsLine (Vector2 c, Vector2 d)
        {
            float py = _pos.Y + _p.Y;
            float px1 = _pos.X + _p.X;
            float px2 = px1 + _w;

            // Check that given line is on both sides of AXLine
            float yf = px2 - px1;
            float t = py * yf;

            float f1 = yf * c.Y - t;
            float f2 = yf * d.Y - t;

            if (f1 * f2 >= 0) {
                return false;
            }

            // Check that AXLine is on both sides of given line
            float xf = d.Y - c.Y;
            t = (d.X - c.X) * py - (d.X * c.Y - c.X * d.Y);

            f1 = xf * px1 - t;
            f2 = xf * px2 - t;

            return (f1 * f2 < 0);
        }
    }
}
