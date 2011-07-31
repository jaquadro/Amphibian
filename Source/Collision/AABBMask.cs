using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Collision
{
    public class AABBMask : Mask
    {
        internal Vector2 _point;
        internal float _w;
        internal float _h;

        public AABBMask (Vector2 p, float width, float height)
        {
            _type = MaskType.AABB;
            _point = p;
            _w = width;
            _h = height;
        }

        public AABBMask (Vector2 p0, Vector2 p1)
        {
            _type = MaskType.AABB;

            float minx = (p0.X < p1.X) ? p0.X : p1.X;
            float miny = (p0.Y < p1.Y) ? p0.Y : p1.Y;

            _point = new Vector2(minx, miny);
            _w = Math.Abs(p1.X - p0.X);
            _h = Math.Abs(p1.Y - p0.Y);
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
                    return Collision.TestOverlap(this, mask as AABBMask);
                case MaskType.Triangle:
                    return Collision.TestOverlap(this, mask as TriangleMask);
            }

            return false;
        }

        public override TestResult TestOverlapExt (Mask mask)
        {
            switch (mask._type) {
                case MaskType.Point:
                    return CollisionTR.TestOverlap(mask as PointMask, this);
                case MaskType.Line:
                    return CollisionTR.TestOverlap(mask as LineMask, this);
                case MaskType.Circle:
                    return CollisionTR.TestOverlap(mask as CircleMask, this);
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
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (CircleMask mask)
        {
            return CollisionTR.TestOverlap(mask, this);
        }

        public TestResult TestOverlapExt (AABBMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        public TestResult TestOverlapExt (TriangleMask mask)
        {
            return CollisionTR.TestOverlap(this, mask);
        }

        internal Vector2 ClosestPoint (Vector2 p)
        {
            //edge = false;
            Vector2 r = _pos + _point;
            Vector2 q = p;

            if (q.X <= r.X) {
                q.X = r.X;
                //edge = true;
            }
            else {
                float mx = r.X + _w;
                if (q.X >= mx) {
                    q.X = mx;
                    //edge = true;
                }
            }

            if (q.Y <= r.Y) {
                q.Y = r.Y;
                //edge = true;
            }
            else {
                float my = r.Y + _h;
                if (q.Y >= my) {
                    q.Y = my;
                    //edge = true;
                }
            }

            return q;
        }

        internal TestResult LineIntersect (Vector2 p0, Vector2 p1)
        {
            Vector2 r = _pos + _point;

            float rw = r.X + _w;
            float rh = r.Y + _h;

            if (p0.X == p1.X) {
                // Parallel to Y
                if (p0.X < r.X || p0.X > r.X + _w) {
                    return TestResult.None;
                }

                float pmin = Math.Min(p0.Y, p1.Y);
                float pmax = p0.Y + p1.Y - pmin;
                if (pmin > rh || pmax < r.Y) {
                    return TestResult.None;
                }

                if (p0.X == r.X || p0.X == rw) {
                    return TestResult.Overlapping | TestResult.Edge;
                }
                if (pmin == rh || pmax == r.Y) {
                    return TestResult.Overlapping | TestResult.Edge;
                }

                return TestResult.Overlapping;
            }
            else if (p0.Y == p1.Y) {
                // Parallel to X
                if (p0.Y < r.Y || p0.Y > rh) {
                    return TestResult.None;
                }

                float pmin = Math.Min(p0.X, p1.X);
                float pmax = p0.X + p1.X - pmin;
                if (pmin > rw || pmax < r.X) {
                    return TestResult.None;
                }

                if (p0.Y == r.Y || p0.Y == rh) {
                    return TestResult.Overlapping | TestResult.Edge;
                }
                if (pmin == rw || pmax == r.X) {
                    return TestResult.Overlapping | TestResult.Edge;
                }

                return TestResult.Overlapping;
            }

            float m = (p0.Y - p1.Y) / (p0.X - p1.X);
            float c = p0.Y - (m * p0.X);

            float topIntersect = c;
            float botIntersect = c;

            if (m > 0) {
                // Slope descending left to right
                topIntersect += (m * r.X);          // t = mL + c
                botIntersect += (m * rw);           // b = mR + c
            }
            else {
                // Slope descending right to left
                topIntersect += (m * rw);           // t = mR + c
                botIntersect += (m * r.X);          // b = mL + c
            }

            float topTriPoint = p0.Y;
            float botTriPoint = p1.Y;

            if (p0.Y > p1.Y) {
                // P0 is actually lower
                topTriPoint = p1.Y;
                botTriPoint = p0.Y;
            }

            float topOver = (topIntersect > topTriPoint) ? topIntersect : topTriPoint;
            float botOver = (botIntersect < botTriPoint) ? botIntersect : botTriPoint;

            // Test ranges
            if (topOver > botOver || botOver < r.Y || topOver > rh) {
                return TestResult.None;
            }

            // Test edges
            if (topOver == botOver || botOver == r.Y || topOver == rh) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.Overlapping;
        }

        internal bool IntersectsLine (Vector2 p0, Vector2 p1)
        {
            Vector2 r = _pos + _point;

            float rw = r.X + _w;
            float rh = r.Y + _h;

            if (p0.X == p1.X) {
                // Parallel to Y
                if (p0.X < r.X || p0.X > r.X + _w) {
                    return false;
                }

                float pmin = Math.Min(p0.Y, p1.Y);
                float pmax = p0.Y + p1.Y - pmin;

                return !(pmin > rh || pmax < r.Y);
            }
            else if (p0.Y == p1.Y) {
                // Parallel to X
                if (p0.Y < r.Y || p0.Y > rh) {
                    return false;
                }

                float pmin = Math.Min(p0.X, p1.X);
                float pmax = p0.X + p1.X - pmin;

                return !(pmin > rw || pmax < r.X);
            }

            float m = (p0.Y - p1.Y) / (p0.X - p1.X);
            float c = p0.Y - (m * p0.X);

            float topIntersect = c;
            float botIntersect = c;

            if (m > 0) {
                // Slope descending left to right
                topIntersect += (m * r.X);          // t = mL + c
                botIntersect += (m * rw);           // b = mR + c
            }
            else {
                // Slope descending right to left
                topIntersect += (m * rw);           // t = mR + c
                botIntersect += (m * r.X);          // b = mL + c
            }

            float topTriPoint = p0.Y;
            float botTriPoint = p1.Y;

            if (p0.Y > p1.Y) {
                // P0 is actually lower
                topTriPoint = p1.Y;
                botTriPoint = p0.Y;
            }

            float topOver = (topIntersect > topTriPoint) ? topIntersect : topTriPoint;
            float botOver = (botIntersect < botTriPoint) ? botIntersect : botTriPoint;

            return !(topOver > botOver || botOver < r.Y || topOver > rh);
        }
    }
}
