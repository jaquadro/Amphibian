using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public class AABBMask : Mask
    {
        internal PointFP _point;
        internal FPInt _w;
        internal FPInt _h;

        public AABBMask (PointFP p, FPInt width, FPInt height)
        {
            _type = MaskType.AABB;
            _point = p;
            _w = width;
            _h = height;
        }

        public AABBMask (PointFP p0, PointFP p1)
        {
            _type = MaskType.AABB;

            FPInt minx = (p0.X < p1.X) ? p0.X : p1.X;
            FPInt miny = (p0.Y < p1.Y) ? p0.Y : p1.Y;

            _point = new PointFP(minx, miny);
            _w = FPMath.Abs(p1.X - p0.X);
            _h = FPMath.Abs(p1.Y - p0.Y);
        }

        public AABBMask(PointFP[] pointFP):this(pointFP[0],pointFP[1])
        {
        }

        public override object Clone ()
        {
            AABBMask mask = new AABBMask(_point, _w, _h);
            mask._pos = _pos;

            return mask;
        }

        public override RectangleFP Bounds
        {
            get { return new RectangleFP(_pos.X + _point.X, _pos.Y + _point.Y, _w, _h); }
        }

        /*public override void Draw (SpriteBatch spriteBatch, Pen pen)
        {
            int x = (int)(_pos.X + _point.X);
            int y = (int)(_pos.Y + _point.Y);

            Draw2D.DrawRectangle(spriteBatch, new Rectangle(x, y, (int)_w, (int)_h), pen);
        }*/

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
                    return Collision.TestOverlapEdge(mask as AXLineMask, this);
                case MaskType.AYLine:
                    return Collision.TestOverlap(mask as AYLineMask, this);
                case MaskType.Line:
                    return Collision.TestOverlap(mask as LineMask, this);
                case MaskType.AABB:
                    return Collision.TestOverlap(this, mask as AABBMask);
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

        public override bool TestOverlapEdge (FPInt x, FPInt y)
        {
            return Collision.TestOverlapEdge(this, x, y);
        }

        public override bool TestOverlap (AXLine line)
        {
            return Collision.TestOverlap(line, this);
        }

        public override bool TestOverlapEdge (AXLine line)
        {
            return Collision.TestOverlapEdge(line, this);
        }

        public override bool TestOverlap (AYLine line)
        {
            return Collision.TestOverlap(line, this);
        }

        public override bool TestOverlapEdge (AYLine line)
        {
            return Collision.TestOverlapEdge(line, this);
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
        }*/

        internal PointFP ClosestPoint (PointFP p)
        {
            VectorFP r = (VectorFP)_pos + _point;
            PointFP q = p;

            if (q.X <= r.X) {
                q.X = r.X;
            }
            else {
                FPInt mx = r.X + _w;
                if (q.X >= mx) {
                    q.X = mx;
                }
            }

            if (q.Y <= r.Y) {
                q.Y = r.Y;
            }
            else {
                FPInt my = r.Y + _h;
                if (q.Y >= my) {
                    q.Y = my;
                }
            }

            return q;
        }

        /*internal TestResult LineIntersect (PointFP p0, PointFP p1)
        {
            VectorFP r = (VectorFP)_pos + _point;

            FPInt rw = r.X + _w;
            FPInt rh = r.Y + _h;

            if (p0.X == p1.X) {
                // Parallel to Y
                if (p0.X < r.X || p0.X > r.X + _w) {
                    return TestResult.None;
                }

                FPInt pmin = FPMath.Min(p0.Y, p1.Y);
                FPInt pmax = p0.Y + p1.Y - pmin;
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

                FPInt pmin = FPMath.Min(p0.X, p1.X);
                FPInt pmax = p0.X + p1.X - pmin;
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

            FPInt m = (p0.Y - p1.Y) / (p0.X - p1.X);
            FPInt c = p0.Y - (m * p0.X);

            FPInt topIntersect = c;
            FPInt botIntersect = c;

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

            FPInt topTriPoint = p0.Y;
            FPInt botTriPoint = p1.Y;

            if (p0.Y > p1.Y) {
                // P0 is actually lower
                topTriPoint = p1.Y;
                botTriPoint = p0.Y;
            }

            FPInt topOver = (topIntersect > topTriPoint) ? topIntersect : topTriPoint;
            FPInt botOver = (botIntersect < botTriPoint) ? botIntersect : botTriPoint;

            // Test ranges
            if (topOver > botOver || botOver < r.Y || topOver > rh) {
                return TestResult.None;
            }

            // Test edges
            if (topOver == botOver || botOver == r.Y || topOver == rh) {
                return TestResult.Overlapping | TestResult.Edge;
            }

            return TestResult.Overlapping;
        }*/

        internal bool IntersectsLine (PointFP p0, PointFP p1)
        {
            VectorFP r = (VectorFP)_pos + _point;

            FPInt rw = r.X + _w;
            FPInt rh = r.Y + _h;

            if (p0.X == p1.X) {
                // Parallel to Y
                if (p0.X < r.X || p0.X > r.X + _w) {
                    return false;
                }

                FPInt pmin = FPMath.Min(p0.Y, p1.Y);
                FPInt pmax = p0.Y + p1.Y - pmin;

                return !(pmin > rh || pmax < r.Y);
            }
            else if (p0.Y == p1.Y) {
                // Parallel to X
                if (p0.Y < r.Y || p0.Y > rh) {
                    return false;
                }

                FPInt pmin = FPMath.Min(p0.X, p1.X);
                FPInt pmax = p0.X + p1.X - pmin;

                return !(pmin > rw || pmax < r.X);
            }

            FPInt m = (p0.Y - p1.Y) / (p0.X - p1.X);
            FPInt c = p0.Y - (m * p0.X);

            FPInt topIntersect = c;
            FPInt botIntersect = c;

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

            FPInt topTriPoint = p0.Y;
            FPInt botTriPoint = p1.Y;

            if (p0.Y > p1.Y) {
                // P0 is actually lower
                topTriPoint = p1.Y;
                botTriPoint = p0.Y;
            }

            FPInt topOver = (topIntersect > topTriPoint) ? topIntersect : topTriPoint;
            FPInt botOver = (botIntersect < botTriPoint) ? botIntersect : botTriPoint;

            return !(topOver > botOver || botOver < r.Y || topOver > rh);
        }
    }
}
