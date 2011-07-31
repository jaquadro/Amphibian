using System;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public class AYLineMask : Mask
    {
        internal Vector2 _p;
        internal float _h;

        public AYLineMask (Vector2 p, float h)
        {
            _type = MaskType.AYLine;

            if (h > 0) {
                _p = p;
                _h = h;
            }
            else {
                _p = new Vector2(p.X, p.Y - h);
                _h = 0 - h;
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
                    return Collision.TestOverlap(mask as AXLineMask, this);
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
            float px = _pos.X + _p.X;
            float py1 = _pos.Y + _p.Y;
            float py2 = py1 + _h;

            Vector2 r1 = mask._pos + mask._point;
            Vector2 r2 = r1 + new Vector2(mask._w, mask._h);

            return !(px > r2.X || px < r1.X || py1 > r2.Y || py2 < r1.Y);
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
            float px = _pos.X + _p.X;
            float py1 = _pos.Y + _p.Y;
            float py2 = py1 + _h;

            if (p.Y <= py1) {
                return _p;
            }
            else if (p.Y >= py2) {
                return new Vector2(px, py2);
            }

            return new Vector2(px, p.Y);
        }

        // Implicit equation for a line:
        //   (x2 - x1) * F(x, y) = (x2 - x1)y - (y2 - y1)x - (x2 * y1 - x1 * y2)
        // Since we only care about sign and not value, we do not need to divide by (x2 - x1)

        internal bool IntersectsLine (Vector2 c, Vector2 d)
        {
            float px = _pos.X + _p.X;
            float py1 = _pos.Y + _p.Y;
            float py2 = py1 + _h;

            // Check that given line is on both sides of AYLine
            float xf = py2 - py1;
            float t = px * xf;

            float f1 = xf * c.X - t;
            float f2 = xf * d.X - t;

            if (f1 * f2 >= 0) {
                return false;
            }

            // Check that AYLine is on both sides of given line
            float yf = d.X - c.X;
            t = (d.Y - c.Y) * px + (d.X * c.Y - c.X * d.Y);

            f1 = yf * py1 - t;
            f2 = yf * py2 - t;

            return (f1 * f2 < 0);
        }
    }
}
