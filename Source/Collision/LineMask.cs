using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Drawing;

namespace Amphibian.Collision
{
    public sealed class LineMask : Mask
    {
        internal Vector2 _p0;
        internal float _w;
        internal float _h;

        public LineMask (Vector2 p0, Vector2 p1)
        {
            _type = MaskType.Line;
            _p0 = p0;
            _w = p1.X - p0.X;
            _h = p1.Y - p0.Y;
        }

        public LineMask (Vector2 p0, float dw, float dh)
        {
            _type = MaskType.Line;
            _p0 = p0;
            _w = dw;
            _h = dh;
        }

        public override object Clone ()
        {
            LineMask mask = new LineMask(_p0, _w, _h);
            mask._pos = _pos;

            return mask;
        }

        public override void Draw (SpriteBatch spriteBatch)
        {
            Primitives2D.DrawLine(spriteBatch, _pos + _p0, _pos + new Vector2(_p0.X + _w, _p0.Y + _h), Color.White);
        }

        public override BoundingRectangle Bounds
        {
            get 
            {
                float minx = (_w >= 0) ? _p0.X : _p0.X + _w;
                float miny = (_h >= 0) ? _p0.Y : _p0.Y + _h;
                return new BoundingRectangle(_pos.X + minx, _pos.Y + miny, Math.Abs(_w), Math.Abs(_h)); 
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
                    return Collision.TestOverlap(mask as AYLineMask, this);
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

        public override TestResult TestOverlapExt (Mask mask)
        {
            switch (mask._type) {
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
            }

            return TestResult.None;
        }

        public TestResult TestOverlapExt (PointMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (LineMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (CircleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (AABBMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (TriangleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        internal static float Signed2DTriArea (Vector2 a, Vector2 b, Vector2 c)
        {
            return (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);
        }

        internal TestResult LineIntersect (Vector2 a, Vector2 b)
        {
            Vector2 c = _pos + _p0;
            Vector2 d = c + new Vector2(_w, _h);

            return LineIntersect(a, b, c, d);
        }

        internal static TestResult LineIntersect (Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float a1 = Signed2DTriArea(a, b, d);
            float a2 = Signed2DTriArea(a, b, c);
            float t1 = a1 * a2;

            // Deal with colinear lines
            if (a1 == 0 && a2 == 0) {
                if (a.X == c.X && !RangesOverlap(a.Y, b.Y, c.Y, d.Y)) {
                    return TestResult.None;
                }
                else if (!RangesOverlap(a.X, b.X, c.X, d.X)) {
                    return TestResult.None;
                }

                return TestResult.Overlapping | TestResult.Edge;
            }

            // Now deal with normal intersections
            else if (t1 <= 0) {
                float a3 = Signed2DTriArea(c, d, a);
                float a4 = a3 + a2 - a1;
                float t2 = a3 * a4;

                if (t2 <= 0) {
                    if (t1 * t2 == 0) {
                        return TestResult.Overlapping | TestResult.Edge;
                    }
                    else {
                        return TestResult.Overlapping;
                    }
                }
            }

            return TestResult.None;
        }

        internal Vector2 ClosestPoint (Vector2 p)
        {
            Vector2 a = _pos + _p0;
            Vector2 ab = new Vector2(_w, _h);

            float t = Vector2.Dot(p - a, ab);
            if (t <= 0) {
                return a;
            }
            else {
                float d = Vector2.Dot(ab, ab);
                if (t >= d) {
                    return a + ab;
                }
                else {
                    t = t / d;
                    return a + (t * ab);
                }
            }
        }

        internal static bool RangesOverlap (float a1, float a2, float b1, float b2)
        {
            float min1 = a1;
            float max1 = a2;
            if (min1 > max1) {
                min1 = a2;
                max1 = a1;
            }

            float min2 = b1;
            float max2 = b2;
            if (min2 > max2) {
                min2 = b2;
                max2 = b1;
            }

            return !(min1 > max2 || min2 > max1);
        }

        internal bool IntersectsLine (Vector2 c, Vector2 d)
        {
            Vector2 a = _pos + _p0;
            Vector2 b = a + new Vector2(_w, _h);

            return IntersectsLine(a, b, c, d);
        }

        internal static bool IntersectsLine (Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            // Check that given line is on both sides of this line
            float xf = b.Y - a.Y;
            float yf = b.X - a.X;
            float t = d.X * c.Y - c.X * d.Y;

            float f1 = xf * c.Y - yf * c.X - t;
            float f2 = xf * d.Y - yf * d.X - t;

            if (f1 * f2 >= 0) {
                return false;
            }

            // Check that AXLine is on both sides of given line
            xf = d.Y - c.Y;
            yf = d.X - c.X;
            t = d.X * c.Y - c.X * d.Y;

            f1 = xf * a.Y - yf * a.X - t;
            f2 = xf * b.Y - yf * b.X - t;

            return (f1 * f2 < 0);
        }
    }
}
