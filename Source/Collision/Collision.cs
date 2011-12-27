using System;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;

namespace Amphibian.Collision
{
    public static class Collision
    {
        // Point -- [____] Collision Tests

        public static bool TestOverlap (PointMask ptMask1, PointMask ptMask2)
        {
            return false;
        }

        public static bool TestOverlap (CircleMask cMask, FPInt x, FPInt y)
        {
            VectorFP p1 = (VectorFP)cMask._pos + cMask._p;

            FPInt dx = p1.X - x;
            FPInt dy = p1.Y - y;
            FPInt d2 = dx * dx + dy * dy;
            FPInt r2 = cMask._radius * cMask._radius;

            return (d2 < r2);
        }

        public static bool TestOverlap (PointMask ptMask, CircleMask cMask)
        {
            VectorFP p0 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlap(cMask, p0.X, p0.Y);
        }

        public static bool TestOverlap (PointMask ptMask, AXLineMask xlMask)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, AXLine xl)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, AYLineMask ylMask)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, AYLine yl)
        {
            return false;
        }

        public static bool TestOverlap (PointMask ptMask, LineMask lnMask)
        {
            return false;
        }

        public static bool TestOverlap (AABBMask rMask, FPInt x, FPInt y)
        {
            VectorFP p1 = (VectorFP)rMask._pos + rMask._point;

            return (x > p1.X && x < p1.X + rMask._w && y > p1.Y && y < p1.Y + rMask._h);
        }

        public static bool TestOverlap (PointMask ptMask, AABBMask rMask)
        {
            VectorFP p0 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlap(rMask, p0.X, p0.Y);
        }

        public static bool TestOverlap (TriangleMask triMask, FPInt x, FPInt y)
        {
            PointFP bary = triMask.Barycentric(new VectorFP(x, y));

            return (bary.X > 0 && bary.Y > 0 && (bary.X + bary.Y) < 1);
        }

        public static bool TestOverlap (PointMask ptMask, TriangleMask triMask)
        {
            PointFP bary = triMask.Barycentric((VectorFP)ptMask._pos + ptMask._point);

            return (bary.X > 0 && bary.Y > 0 && (bary.X + bary.Y) < 1);
        }

        // Circle -- [____] Collision Tests

        public static bool TestOverlap (CircleMask cMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, cMask);
        }

        public static bool TestOverlap (CircleMask cMask1, CircleMask cMask2)
        {
            VectorFP p0 = (VectorFP)cMask1._pos + cMask1._p;
            VectorFP p1 = (VectorFP)cMask2._pos + cMask2._p;

            FPInt dx = p1.X - p0.X;
            FPInt dy = p1.Y - p0.Y;
            FPInt d2 = dx * dx + dy * dy;
            FPInt r = cMask1._radius + cMask2._radius;
            FPInt r2 = r * r;

            return (d2 < r2);
        }

        public static bool TestOverlap (CircleMask cMask, AXLineMask xlMask)
        {
            VectorFP c1 = (VectorFP)cMask._pos + cMask._p;
            VectorFP c2 = xlMask.ClosestPoint(c1);

            FPInt d2 = VectorFP.DistanceSquared(c1, c2);
            FPInt r2 = cMask._radius * cMask._radius;

            return (r2 < d2);
        }

        public static bool TestOverlap (CircleMask cMask, AXLine xl)
        {
            VectorFP c1 = (VectorFP)cMask._pos + cMask._p;
            VectorFP c2 = xl.ClosestPoint(c1);

            FPInt d2 = VectorFP.DistanceSquared(c1, c2);
            FPInt r2 = cMask._radius * cMask._radius;

            return (r2 < d2);
        }

        public static bool TestOverlap (CircleMask cMask, AYLineMask ylMask)
        {
            VectorFP c1 = (VectorFP)cMask._pos + cMask._p;
            VectorFP c2 = ylMask.ClosestPoint(c1);

            FPInt d2 = VectorFP.DistanceSquared(c1, c2);
            FPInt r2 = cMask._radius * cMask._radius;

            return (r2 < d2);
        }

        public static bool TestOverlap (CircleMask cMask, AYLine yl)
        {
            VectorFP c1 = (VectorFP)cMask._pos + cMask._p;
            VectorFP c2 = yl.ClosestPoint(c1);

            FPInt d2 = VectorFP.DistanceSquared(c1, c2);
            FPInt r2 = cMask._radius * cMask._radius;

            return (r2 < d2);
        }

        public static bool TestOverlap (CircleMask cMask, LineMask lnMask)
        {
            VectorFP c = (VectorFP)cMask._pos + cMask._p;
            VectorFP p = lnMask.ClosestPoint(c);

            p = p - c;
            FPInt p2 = p.LengthSquared();
            FPInt r2 = cMask._radius * cMask._radius;

            return (p2 < r2);
        }

        public static bool TestOverlap (CircleMask cMask, AABBMask rMask)
        {
            VectorFP p0 = (VectorFP)cMask._pos + cMask._p;
            VectorFP p1 = rMask.ClosestPoint(p0);

            FPInt dx = p1.X - p0.X;
            FPInt dy = p1.Y - p0.Y;
            FPInt d2 = dx * dx + dy * dy;
            FPInt r2 = cMask._radius * cMask._radius;

            return (d2 < r2);
        }

        public static bool TestOverlap (CircleMask cMask, TriangleMask triMask)
        {
            VectorFP p0 = (VectorFP)cMask._pos + cMask._p;
            VectorFP p1 = triMask.ClosestPoint(p0);

            FPInt dx = p1.X - p0.X;
            FPInt dy = p1.Y - p0.Y;
            FPInt d2 = dx * dx + dy * dy;
            FPInt r2 = cMask._radius * cMask._radius;

            return (d2 < r2);
        }

        // AXLine -- [____] Collision Tests

        public static bool TestOverlap (AXLineMask xlMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, xlMask);
        }

        public static bool TestOverlap (AXLineMask xlMask, CircleMask cMask)
        {
            return TestOverlap(cMask, xlMask);
        }

        public static bool TestOverlap (AXLineMask xlMask1, AXLineMask xlMask2)
        {
            return false;
        }

        public static bool TestOverlap (AXLineMask xlMask, AYLineMask ylMask)
        {
            VectorFP c = (VectorFP)xlMask._pos + xlMask._p;
            VectorFP d = c + new VectorFP(xlMask._w, 0);

            return ylMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AXLineMask xlMask, AYLine ylMask)
        {
            VectorFP c = (VectorFP)xlMask._pos + xlMask._p;
            VectorFP d = c + new VectorFP(xlMask._w, 0);

            return ylMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AXLineMask xlMask, LineMask lnMask)
        {
            VectorFP c = (VectorFP)lnMask._pos + lnMask._p0;
            VectorFP d = c + new VectorFP(lnMask._w, lnMask._h);

            return xlMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AXLineMask xlMask, AABBMask rMask)
        {
            FPInt py = xlMask._pos.Y + xlMask._p.Y;
            FPInt px1 = xlMask._pos.X + xlMask._p.X;
            FPInt px2 = px1 + xlMask._w;

            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(py >= r2.Y || py <= r1.Y || px1 >= r2.X || px2 <= r1.X);
        }

        public static bool TestOverlap (AXLineMask xlMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (xlMask.IntersectsLine(a, b) || xlMask.IntersectsLine(b, c) || xlMask.IntersectsLine(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric((VectorFP)xlMask._pos + xlMask._p);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        public static bool TestOverlap (AXLine xlMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, xlMask);
        }

        public static bool TestOverlap (AXLine xlMask, CircleMask cMask)
        {
            return TestOverlap(cMask, xlMask);
        }

        public static bool TestOverlap (AXLine xlMask1, AXLineMask xlMask2)
        {
            return false;
        }

        public static bool TestOverlap (AXLine xlMask, AYLineMask ylMask)
        {
            return ylMask.IntersectsLine(xlMask.LeftPoint, xlMask.RightPoint);
        }

        public static bool TestOverlap (AXLine xlMask, LineMask lnMask)
        {
            VectorFP c = (VectorFP)lnMask._pos + lnMask._p0;
            VectorFP d = c + new VectorFP(lnMask._w, lnMask._h);

            return xlMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AXLine xlMask, AABBMask rMask)
        {
            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(xlMask.Y >= r2.Y || xlMask.Y <= r1.Y || xlMask.Left >= r2.X || xlMask.Right <= r1.X);
        }

        public static bool TestOverlap (AXLine xlMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (xlMask.IntersectsLine(a, b) || xlMask.IntersectsLine(b, c) || xlMask.IntersectsLine(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric(xlMask.LeftPoint);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        // AYLine -- [____] Collision Tests

        public static bool TestOverlap (AYLineMask ylMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask, CircleMask cMask)
        {
            return TestOverlap(cMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask, AXLine xlMask)
        {
            return TestOverlap(xlMask, ylMask);
        }

        public static bool TestOverlap (AYLineMask ylMask1, AYLineMask ylMask2)
        {
            return false;
        }

        public static bool TestOverlap (AYLineMask ylMask, LineMask lnMask)
        {
            VectorFP c = (VectorFP)lnMask._pos + lnMask._p0;
            VectorFP d = c + new VectorFP(lnMask._w, lnMask._h);

            return ylMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AYLineMask ylMask, AABBMask rMask)
        {
            FPInt px = ylMask._pos.X + ylMask._p.X;
            FPInt py1 = ylMask._pos.Y + ylMask._p.Y;
            FPInt py2 = py1 + ylMask._h;

            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(px >= r2.X || px <= r1.X || py1 >= r2.Y || py2 <= r1.Y);
        }

        public static bool TestOverlap (AYLineMask ylMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (ylMask.IntersectsLine(a, b) || ylMask.IntersectsLine(b, c) || ylMask.IntersectsLine(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric((VectorFP)ylMask._pos + ylMask._p);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        public static bool TestOverlap (AYLine ylMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, ylMask);
        }

        public static bool TestOverlap (AYLine ylMask, CircleMask cMask)
        {
            return TestOverlap(cMask, ylMask);
        }

        public static bool TestOverlap (AYLine ylMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, ylMask);
        }

        public static bool TestOverlap (AYLine ylMask1, AYLineMask ylMask2)
        {
            return false;
        }

        public static bool TestOverlap (AYLine ylMask, LineMask lnMask)
        {
            VectorFP c = (VectorFP)lnMask._pos + lnMask._p0;
            VectorFP d = c + new VectorFP(lnMask._w, lnMask._h);

            return ylMask.IntersectsLine(c, d);
        }

        public static bool TestOverlap (AYLine ylMask, AABBMask rMask)
        {
            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(ylMask.X >= r2.X || ylMask.X <= r1.X || ylMask.Top >= r2.Y || ylMask.Bottom <= r1.Y);
        }

        public static bool TestOverlap (AYLine ylMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (ylMask.IntersectsLine(a, b) || ylMask.IntersectsLine(b, c) || ylMask.IntersectsLine(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric((VectorFP)ylMask.TopPoint);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }
        
        // Line -- [____] Collision Tests

        public static bool TestOverlap (LineMask lnMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, CircleMask cMask)
        {
            return TestOverlap(cMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, AXLine xlMask)
        {
            return TestOverlap(xlMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, AYLineMask ylMask)
        {
            return TestOverlap(ylMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask, AYLine ylMask)
        {
            return TestOverlap(ylMask, lnMask);
        }

        public static bool TestOverlap (LineMask lnMask1, LineMask lnMask2)
        {
            VectorFP a = (VectorFP)lnMask1._pos + lnMask1._p0;
            VectorFP b = a + new VectorFP(lnMask1._w, lnMask1._h);

            return lnMask2.IntersectsLine(a, b);
        }

        public static bool TestOverlap (LineMask lnMask, AABBMask rMask)
        {
            VectorFP p0 = (VectorFP)lnMask._pos + lnMask._p0;
            VectorFP p1 = new VectorFP(p0.X + lnMask._w, p0.Y + lnMask._h);

            return rMask.IntersectsLine(p0, p1);
        }

        public static bool TestOverlap (LineMask lnMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            //TestResult t = lnMask.LineIntersect(a, b);

            if (lnMask.IntersectsLine(a, b) || lnMask.IntersectsLine(b, c) || lnMask.IntersectsLine(a, c)) {
                return true;
            }

            // Check for containment
            VectorFP r = triMask.Barycentric((VectorFP)lnMask._pos + lnMask._p0);

            return (r.X >= 0 && r.Y >= 0 && (r.X + r.Y) <= 1);
        }

        // AABB -- [____] Collision Tests

        public static bool TestOverlap (AABBMask rMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, CircleMask cMask)
        {
            return TestOverlap(cMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, AXLineMask xlMask)
        {
            return TestOverlap(xlMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, AXLine xlMask)
        {
            return TestOverlap(xlMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, AYLineMask ylMask)
        {
            return TestOverlap(ylMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, AYLine ylMask)
        {
            return TestOverlap(ylMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask, LineMask lnMask)
        {
            return TestOverlap(lnMask, rMask);
        }

        public static bool TestOverlap (AABBMask rMask1, AABBMask rMask2)
        {
            VectorFP p0 = (VectorFP)rMask1._pos + rMask1._point;
            VectorFP p1 = (VectorFP)rMask2._pos + rMask2._point;

            FPInt x1 = p0.X + rMask1._w;
            FPInt x2 = p1.X + rMask2._w;
            FPInt y1 = p0.Y + rMask1._h;
            FPInt y2 = p1.Y + rMask2._h;

            return !(x1 <= p1.X || p0.X >= x2 || y1 <= p1.Y || p0.Y >= y2);
        }

        public static bool TestOverlap (AABBMask rMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (rMask.IntersectsLine(a, b) || rMask.IntersectsLine(b, c) || rMask.IntersectsLine(a, c)) {
                return true;
            }

            VectorFP r = rMask._pos + new VectorFP(rMask._point.X + (rMask._w >> 1), rMask._point.Y + (rMask._h >> 1));
            VectorFP q = triMask.Barycentric(r);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        // Triangle -- [____] Collision Tests

        public static bool TestOverlap (TriangleMask triMask, PointMask ptMask)
        {
            return TestOverlap(ptMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, CircleMask cMask)
        {
            return TestOverlap(cMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AXLineMask xlMAsk)
        {
            return TestOverlap(xlMAsk, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AXLine xlMAsk)
        {
            return TestOverlap(xlMAsk, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AYLineMask ylMask)
        {
            return TestOverlap(ylMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AYLine ylMask)
        {
            return TestOverlap(ylMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, LineMask lnMask)
        {
            return TestOverlap(lnMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask, AABBMask rMask)
        {
            return TestOverlap(rMask, triMask);
        }

        public static bool TestOverlap (TriangleMask triMask1, TriangleMask triMask2)
        {
            VectorFP a = (VectorFP)triMask2._pos + triMask2._p0;
            VectorFP b = (VectorFP)triMask2._pos + triMask2._p1;
            VectorFP c = (VectorFP)triMask2._pos + triMask2._p2;

            VectorFP d = (VectorFP)triMask1._pos + triMask1._p0;
            VectorFP e = (VectorFP)triMask1._pos + triMask1._p1;
            VectorFP f = (VectorFP)triMask1._pos + triMask1._p2;

            if (LineMask.IntersectsLine(a, b, d, e) || LineMask.IntersectsLine(a, b, e, f) ||
                LineMask.IntersectsLine(a, b, d, f) || LineMask.IntersectsLine(b, c, d, e) ||
                LineMask.IntersectsLine(b, c, e, f) || LineMask.IntersectsLine(b, c, d, f) ||
                LineMask.IntersectsLine(a, c, d, e) || LineMask.IntersectsLine(a, c, e, f)) 
            {
                return true;
            }

            // Check for containment
            VectorFP q = triMask2.Barycentric(d);
            if (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1) {
                return true;
            }

            q = triMask1.Barycentric(a);
            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        // Composite -- [____] Collision Tests

        public static bool TestOverlap (AXLine xl, CompositeMask comMask)
        {
            RectangleFP comBounds = comMask.Bounds;
            if (xl.Left >= comBounds.Right || xl.Right <= comBounds.Left || xl.Y <= comBounds.Top || xl.Y >= comBounds.Bottom) {
                return false;
            }

            foreach (Mask m in comMask._components) {
                if (m.TestOverlap(xl)) {
                    return true;
                }
            }

            return false;
        }

        public static bool TestOverlap (AYLine yl, CompositeMask comMask)
        {
            RectangleFP comBounds = comMask.Bounds;
            if (yl.X >= comBounds.Right || yl.X <= comBounds.Left || yl.Bottom <= comBounds.Top || yl.Top >= comBounds.Bottom) {
                return false;
            }

            foreach (Mask m in comMask._components) {
                if (m.TestOverlap(yl)) {
                    return true;
                }
            }

            return false;
        }

        public static bool TestOverlap (Mask mask, CompositeMask comMask)
        {
            if (!mask.Bounds.Intersects(comMask.Bounds)) {
                return false;
            }

            foreach (Mask m in comMask._components) {
                if (mask.TestOverlap(m)) {
                    return true;
                }
            }

            return false;
        }

        public static bool TestOverlap (CompositeMask comMask, Mask mask)
        {
            return TestOverlap(mask, comMask);
        }

        public static bool TestOverlap (CompositeMask comMask1, CompositeMask comMask2)
        {
            if (!comMask1.Bounds.Intersects(comMask2.Bounds)) {
                return false;
            }

            foreach (Mask m1 in comMask1._components) {
                if (!m1.Bounds.Intersects(comMask2.Bounds)) {
                    return false;
                }

                foreach (Mask m2 in comMask2._components) {
                    if (m1.TestOverlap(m2)) {
                        return true;
                    }
                }
            }

            return false;
        }

        // Point -- [____] Collision + Edge Tests

        public static bool TestOverlapEdge (PointMask ptMask, FPInt x, FPInt y)
        {
            VectorFP p2 = (VectorFP)ptMask._pos + ptMask._point;

            return x == p2.X && y == p2.Y;
        }

        public static bool TestOverlapEdge (PointMask ptMask1, PointMask ptMask2)
        {
            VectorFP p1 = (VectorFP)ptMask1._pos + ptMask1._point;

            return TestOverlapEdge(ptMask2, p1.X, p1.Y);
        }

        public static bool TestOverlapEdge (CircleMask cMask, FPInt x, FPInt y)
        {
            VectorFP p1 = (VectorFP)cMask._pos + cMask._p;

            FPInt dx = p1.X - x;
            FPInt dy = p1.Y - y;
            FPInt d2 = dx * dx + dy * dy;
            FPInt r2 = cMask._radius * cMask._radius;

            return (d2 <= r2);
        }

        public static bool TestOverlapEdge (PointMask ptMask, CircleMask cMask)
        {
            VectorFP p0 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(cMask, p0.X, p0.Y);
        }

        public static bool TestOverlapEdge (AXLineMask xlMask, FPInt x, FPInt y)
        {
            VectorFP p2 = (VectorFP)xlMask._pos + xlMask._p;

            return (y == p2.Y && x >= p2.X && x <= p2.X + xlMask._w);
        }

        public static bool TestOverlapEdge (AXLine xlMask, FPInt x, FPInt y)
        {
            return (y == xlMask.Y && x >= xlMask.Left && x <= xlMask.Right);
        }

        public static bool TestOverlapEdge (PointMask ptMask, AXLineMask xlMask)
        {
            VectorFP p1 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(xlMask, p1.X, p1.Y);
        }

        public static bool TestOverlapEdge (PointMask ptMask, AXLine xlMask)
        {
            VectorFP p1 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(xlMask, p1.X, p1.Y);
        }

        public static bool TestOverlapEdge (AYLineMask ylMask, FPInt x, FPInt y)
        {
            VectorFP p2 = (VectorFP)ylMask._pos + ylMask._p;

            return (x == p2.X && y >= p2.Y && y <= p2.Y + ylMask._h);
        }

        public static bool TestOverlapEdge (AYLine ylMask, FPInt x, FPInt y)
        {
            return (x == ylMask.X && y >= ylMask.Top && y <= ylMask.Bottom);
        }

        public static bool TestOverlapEdge (PointMask ptMask, AYLineMask ylMask)
        {
            VectorFP p1 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(ylMask, p1.X, p1.Y);
        }

        public static bool TestOverlapEdge (PointMask ptMask, AYLine ylMask)
        {
            VectorFP p1 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(ylMask, p1.X, p1.Y);
        }

        public static bool TestOverlapEdge (LineMask lnMask, FPInt x, FPInt y)
        {
            PointFP cp = lnMask.ClosestPoint(new PointFP(x, y));

            return x == cp.X && y == cp.Y;
        }

        public static bool TestOverlapEdge (PointMask ptMask, LineMask lnMask)
        {
            VectorFP p1 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(lnMask, p1.X, p1.Y);
        }

        public static bool TestOverlapEdge (AABBMask rMask, FPInt x, FPInt y)
        {
            VectorFP p1 = (VectorFP)rMask._pos + rMask._point;

            return (x >= p1.X && x <= p1.X + rMask._w && y >= p1.Y && y <= p1.Y + rMask._h);
        }

        public static bool TestOverlapEdge (PointMask ptMask, AABBMask rMask)
        {
            VectorFP p0 = (VectorFP)ptMask._pos + ptMask._point;

            return TestOverlapEdge(rMask, p0.X, p0.Y);
        }

        public static bool TestOverlapEdge (TriangleMask triMask, FPInt x, FPInt y)
        {
            PointFP bary = triMask.Barycentric(new VectorFP(x, y));

            return (bary.X >= 0 && bary.Y >= 0 && (bary.X + bary.Y) <= 1);
        }

        public static bool TestOverlapEdge (PointMask ptMask, TriangleMask triMask)
        {
            PointFP bary = triMask.Barycentric((VectorFP)ptMask._pos + ptMask._point);

            return (bary.X >= 0 && bary.Y >= 0 && (bary.X + bary.Y) <= 1);
        }

        // AXLine -- [____] Collision + Edge Tests

        public static bool TestOverlapEdge (AXLineMask xlMask, AABBMask rMask)
        {
            FPInt py = xlMask._pos.Y + xlMask._p.Y;
            FPInt px1 = xlMask._pos.X + xlMask._p.X;
            FPInt px2 = px1 + xlMask._w;

            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(py > r2.Y || py < r1.Y || px1 > r2.X || px2 < r1.X);
        }

        public static bool TestOverlapEdge (AXLine xlMask, AABBMask rMask)
        {
            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(xlMask.Y > r2.Y || xlMask.Y < r1.Y || xlMask.Left > r2.X || xlMask.Right < r1.X);
        }

        public static bool TestOverlapEdge (AXLineMask xlMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (xlMask.IntersectsLineEdge(a, b) || xlMask.IntersectsLineEdge(b, c) || xlMask.IntersectsLineEdge(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric((VectorFP)xlMask._pos + xlMask._p);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        public static bool TestOverlapEdge (AXLine xlMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (xlMask.IntersectsLineEdge(a, b) || xlMask.IntersectsLineEdge(b, c) || xlMask.IntersectsLineEdge(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric(xlMask.LeftPoint);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        // AYLine -- [____] Collision + Edge Tests

        public static bool TestOverlapEdge (AYLine ylMask, AABBMask rMask)
        {
            VectorFP r1 = (VectorFP)rMask._pos + rMask._point;
            VectorFP r2 = r1 + new VectorFP(rMask._w, rMask._h);

            return !(ylMask.Top > r2.Y || ylMask.Bottom < r1.Y || ylMask.X > r2.X || ylMask.X < r1.X);
        }

        public static bool TestOverlapEdge (AYLine ylMask, TriangleMask triMask)
        {
            VectorFP a = (VectorFP)triMask._pos + triMask._p0;
            VectorFP b = (VectorFP)triMask._pos + triMask._p1;
            VectorFP c = (VectorFP)triMask._pos + triMask._p2;

            if (ylMask.IntersectsLineEdge(a, b) || ylMask.IntersectsLineEdge(b, c) || ylMask.IntersectsLineEdge(a, c)) {
                return true;
            }

            VectorFP q = triMask.Barycentric(ylMask.TopPoint);

            return (q.X >= 0 && q.Y >= 0 && (q.X + q.Y) <= 1);
        }

        // AABB -- [____] Collision Tests

        public static bool TestOverlapEdge (AABBMask rMask, AXLineMask xlMask)
        {
            return TestOverlapEdge(xlMask, rMask);
        }

        public static bool TestOverlapEdge (AABBMask rMask, AXLine xlMask)
        {
            return TestOverlapEdge(xlMask, rMask);
        }

        public static bool TestOverlapEdge (AABBMask rMask, AYLine ylMask)
        {
            return TestOverlapEdge(ylMask, rMask);
        }

        // Triangle -- [____] Collision Tests

        public static bool TestOverlapEdge (TriangleMask triMask, AXLineMask xlMAsk)
        {
            return TestOverlapEdge(xlMAsk, triMask);
        }

        public static bool TestOverlapEdge (TriangleMask triMask, AXLine xlMAsk)
        {
            return TestOverlapEdge(xlMAsk, triMask);
        }

        public static bool TestOverlapEdge (TriangleMask triMask, AYLine ylMAsk)
        {
            return TestOverlapEdge(ylMAsk, triMask);
        }
    }
}
